using Hack.io.Interface;
using Hack.io.Utility;
using System.Diagnostics;
using System.IO;
using System.Text;
using static Hack.io.MIDI.MIDI;

namespace Hack.io.MIDI;

public partial class MIDI : List<MIDI.Sequence>, ILoadSaveFile
{
    /// <summary>
    /// Indicates the type of the MIDI file
    /// </summary>
    /// <remarks>This can be 0, 1 or 2</remarks>
    public short Type { get; private set; } = 1;
    /// <summary>
    /// Indicates the timebase of the MIDI file as ticks/pulses per quarter note.
    /// </summary>
    /// <remarks>Defaults to 120 since that seems to work fine.</remarks>
    public short TimeBase { get; private set; } = 120;

    /// <summary>
    /// Indicates the MicroTempo of the MIDI file
    /// </summary>
    /// <remarks>This is derived from Track #0's microtempo</remarks>
    public int MicroTempo => 0 == Count ? 500000 : this[0].MicroTempo;
    /// <summary>
    /// Indicates the Tempo of the MIDI file
    /// </summary>
    /// <remarks>This is derived from Track #0's tempo</remarks>
    public double Tempo => MidiUtility.MicroTempoToTempo(MicroTempo);
    /// <summary>
    /// Indicates the total length of the MIDI file, in ticks/pulses
    /// </summary>
    public int Length
    {
        get
        {
            int result = 0;
            foreach (var trk in this)
            {
                int l = trk.Length;
                if (l > result) result = l;
            }
            return result;
        }
    }

    public void Load(Stream Strm)
    {
        if (!_TryReadChunk(Strm, out KeyValuePair<string, byte[]> chunk) ||
            "MThd" != chunk.Key ||
            6 > chunk.Value.Length)
            throw new InvalidDataException("The stream is not a MIDI file format.");
        MemoryStream tstm = new(chunk.Value, false);
        Type = tstm.ReadInt16();
        short trackCount = tstm.ReadInt16(); //Do we even need this?
        TimeBase = tstm.ReadInt16();
        tstm.Dispose();

        while (_TryReadChunk(Strm, out chunk))
        {
            if ("MTrk" == chunk.Key)
            {
                tstm = new(chunk.Value, false);
                Sequence trk = [];
                trk.Load(tstm);
                Add(trk);
                tstm.Dispose();
            }
        }

        static bool _TryReadChunk(Stream stream, out KeyValuePair<string, byte[]> chunk)
        {
            chunk = default;
            try
            {
                var name = stream.ReadString(4, Encoding.ASCII);
                int len = stream.ReadInt32();
                byte[] buf = new byte[len];
                if (stream.Read(buf) != len)
                    return false;
                chunk = new KeyValuePair<string, byte[]>(name, buf);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public void Save(Stream Strm)
    {
        throw new NotImplementedException();
    }


    public class Sequence : List<Event>, ILoadSaveFile
    {
        /// <summary>
        /// Indicates the events as absolutely positioned events
        /// </summary>
        public IEnumerable<Event> AbsoluteEvents
        {
            get
            {
                var runningStatus = default(byte);
                var channelPrefix = (byte)0xFF;
                var pos = 0;
                foreach (var e in this)
                {
                    pos += e.Position;
                    var hs = true;
                    if (0 != e.Message.Status)
                        runningStatus = e.Message.Status;
                    else
                        hs = false;
                    byte r = runningStatus;
                    if (!hs && r < 0xF0 && 0xFF != channelPrefix)
                        r = unchecked((byte)((r & 0xF0) | channelPrefix));

                    yield return new Event(pos, e.Message.Clone());
                }
            }
        }
        /// <summary>
        /// Indicates the MicroTempo of the MIDI sequence
        /// </summary>
        public int MicroTempo
        {
            get
            {
                foreach (var e in AbsoluteEvents)
                {
                    switch (e.Message.Status & 0xF0)
                    {
                        case 0x80:
                        case 0x90:
                            return 500000;
                    }
                    if (e.Message.Status == 0xFF)
                    {
                        if (e.Message is MessageMeta mm && 0x51 == mm.Data1)
                        {
                            return BitConverter.IsLittleEndian ?
                                (mm.Data[0] << 16) | (mm.Data[1] << 8) | mm.Data[2] :
                                (mm.Data[2] << 16) | (mm.Data[1] << 8) | mm.Data[0];
                        }
                    }
                }
                return 500000;
            }
        }
        /// <summary>
		/// Indicates the length of the MIDI sequence
		/// </summary>
		public int Length
        {
            get
            {
                var l = 0;
                byte r = 0;
                foreach (var e in this)
                {
                    var m = e.Message;
                    if (0 != m.Status)
                        r = m.Status;
                    l += e.Position;
                    if (r == 0xFF && m is MessageMeta mm && mm.Data1 == 0x2F)
                        break;
                }
                return l + 1;
            }
        }

        public bool HasChannel(int channel)
        {
            var ofs = 0;
            foreach (var ev in this)
            {
                switch (ev.Message.Status & 0xF0)
                {
                    case 0x80:
                    case 0x90:
                    case 0xA0:
                    case 0xB0:
                    case 0xC0:
                    case 0xD0:
                    case 0xE0:
                        if (channel == ev.Message.Channel)
                            return true;
                        break;
                    default:
                        ofs += ev.Position;
                        break;

                }
            }

            return false;
        }

        public void Load(Stream Strm)
        {
            byte rs = 0;
            int delta = Strm.ReadVariableLength() ?? throw new Exception();

            while(Strm.Position < Strm.Length)
            {
                int? i = Strm.ReadByte();
                bool hasStatus = false;
                byte b = (byte)i;
                if (0x7F < b)
                {
                    hasStatus = true;
                    rs = b;
                    i = Strm.ReadByte();
                    if (-1 != i)
                        b = (byte)i;
                    else
                        b = 0;
                }
                var st = hasStatus ? rs : (byte)0;
                Message? m = null;
                switch (rs & 0xF0)
                {
                    case 0x80:
                        if (i == -1) throw new EndOfStreamException();
                        m = new MessageNoteOff(b, (byte)Strm.ReadByte(), unchecked((byte)(st & 0x0F)));
                        break;
                    case 0x90:
                        if (i == -1) throw new EndOfStreamException();
                        m = new MessageNoteOn(b, (byte)Strm.ReadByte(), unchecked((byte)(st & 0x0F)));
                        break;
                    case 0xA0:
                        if (i == -1) throw new EndOfStreamException();
                        m = new MessageKeyPressure(b, (byte)Strm.ReadByte(), unchecked((byte)(st & 0x0F)));
                        break;
                    case 0xB0:
                        if (i == -1) throw new EndOfStreamException();
                        m = new MessageControlChange(b, (byte)Strm.ReadByte(), unchecked((byte)(st & 0x0F)));
                        break;
                    case 0xC0:
                        if (i == -1) throw new EndOfStreamException();
                        m = new MessagePatchChange(b, unchecked((byte)(st & 0x0F)));
                        break;
                    case 0xD0:
                        if (i == -1) throw new EndOfStreamException();
                        m = new MessageChannelPressure(b, unchecked((byte)(st & 0x0F)));
                        break;
                    case 0xE0:
                        if (i == -1) throw new EndOfStreamException();
                        m = new MessageChannelPitch(b, (byte)Strm.ReadByte(), unchecked((byte)(st & 0x0F)));
                        break;
                    case 0xF0:
                        switch (rs & 0xF)
                        {
                            case 0xF:
                                if (i == -1)
                                    throw new EndOfStreamException();
                                var l = Strm.ReadVariableLength() ?? throw new EndOfStreamException();
                                var ba = new byte[l];
                                if (l != Strm.Read(ba, 0, ba.Length))
                                    throw new EndOfStreamException();
                                m = b switch
                                {
                                    0 => new MessageMetaSequenceNumber(ba),
                                    1 => new MessageMetaText(ba),
                                    2 => new MessageMetaCopyright(ba),
                                    3 => new MessageMetaSequenceOrTrackName(ba),
                                    4 => new MessageMetaInstrumentName(ba),
                                    5 => new MessageMetaLyric(ba),
                                    6 => new MessageMetaMarker(ba),
                                    7 => new MessageMetaCuePoint(ba),
                                    8 => new MessageMetaProgramName(ba),
                                    9 => new MessageMetaDevicePortName(ba),
                                    0x20 => new MessageMetaChannelPrefix(ba),
                                    0x21 => new MessageMetaPort(ba),
                                    0x2F => new MessageMetaEndOfTrack(ba),
                                    0x51 => new MessageMetaTempo(ba),
                                    0x58 => new MessageMetaTimeSignature(ba),
                                    0x59 => new MessageMetaKeySignature(ba),
                                    _ => new MessageMeta(b, ba),
                                };
                                break;
                            case 0x0:
                                if (i == -1) throw new EndOfStreamException();
                                var bb = b;
                                var d = new List<byte>(128);
                                if (0xF7 == bb)
                                {
                                    m = new MessageSysex([]);
                                    break;
                                }
                                d.Add(bb);
                                while (0xF7 != bb)
                                {
                                    var rb = Strm.ReadByte();
                                    if (0 > rb)
                                        throw new EndOfStreamException("Unterminated MIDI sysex message in file.");
                                    if (0xF7 == rb)
                                        break;
                                    bb = unchecked((byte)rb);
                                    d.Add(bb);

                                }
                                d.Add(0xF7);
                                ba = [.. d];
                                m = new MessageSysex(ba);
                                break;
                            case 0x2:
                                if (i == -1) throw new EndOfStreamException();
                                m = new MessageSongPosition(b, (byte)Strm.ReadByte());
                                break;
                            case 0x3:
                                if (i == -1) throw new EndOfStreamException();
                                m = new MessageSongSelect(b);
                                break;
                            case 0x6:
                                if (i == -1) throw new EndOfStreamException();
                                m = new MessageTuneRequest();
                                break;
                            // system reatime messages follow. Shouldn't be present in a file but we handle them anyway
                            case 0x8:
                                if (i == -1) throw new EndOfStreamException();
                                m = new MessageRealTimeTimingClock();
                                break;
                            case 0xA:
                                if (i == -1) throw new EndOfStreamException();
                                m = new MessageRealTimeStart();
                                break;
                            case 0xB:
                                if (i == -1) throw new EndOfStreamException();
                                m = new MessageRealTimeContinue();
                                break;
                            case 0xC:
                                if (i == -1) throw new EndOfStreamException();
                                m = new MessageRealTimeStop();
                                break;
                            case 0xE:
                                if (i == -1) throw new EndOfStreamException();
                                m = new MessageRealTimeActiveSensing();
                                break;
                            default:
                                throw new NotSupportedException("The MIDI message is not recognized.");
                        }
                        break;
                }

#pragma warning disable CS8604 // Possible null reference argument.
                Add(new Event(delta, m));
#pragma warning restore CS8604 // Possible null reference argument.

                //all midi events need a following delay, even if that delay is Zero.
                i = Strm.ReadVariableLength();
                if (i is null)
                    break;
                delta = i.Value;
            }
        }

        public void Save(Stream Strm)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Creates an event at the specified position with the specified MIDI message
    /// </summary>
    /// <param name="position">The position in MIDI ticks</param>
    /// <param name="message">The MIDI message for this event</param>
    public partial class Event(int position, Message message) : ICloneable
    {
        /// <summary>
        /// Indicates the position in MIDI ticks
        /// </summary>
        public int Position { get; private set; } = position;
        /// <summary>
        /// Indicates the MIDI message associated with this event
        /// </summary>
        public Message Message { get; private set; } = message;
        /// <summary>
        /// Creates a deep copy of the MIDI event
        /// </summary>
        /// <returns>A new, equivelent MIDI event</returns>
        public Event Clone() => new(Position, Message.Clone());
        object ICloneable.Clone() => Clone();
        /// <summary>
        /// Returns a string representation of the event
        /// </summary>
        /// <returns>A string representation of the event</returns>
        public override string ToString() => $"{Position}::{Message}";
    }

    /// <summary>
    /// Creates a new instance of a MIDI time signature with the specified parameters
    /// </summary>
    /// <param name="numerator">The numerator</param>
    /// <param name="denominator">The denominator</param>
    /// <param name="midiTicksPerMetronomeTick">The MIDI ticks per metronome tick</param>
    /// <param name="thirtySecondNotesPerQuarterNote">The 32nd notes per quarter note</param>
    public partial struct TimeSignature(byte numerator, short denominator, byte midiTicksPerMetronomeTick, byte thirtySecondNotesPerQuarterNote)
    {
        /// <summary>
        /// Indicates the numerator of the time signature
        /// </summary>
        public byte Numerator { get; private set; } = numerator;
        /// <summary>
        /// Indicates the denominator of the time signature
        /// </summary>
        public short Denominator { get; private set; } = denominator;
        /// <summary>
        /// Indicates the MIDI ticks/pulses per metronome tick
        /// </summary>
        public byte MidiTicksPerMetronomeTick { get; private set; } = midiTicksPerMetronomeTick;
        /// <summary>
        /// Indicates the 32nd notes per quarter note
        /// </summary>
        public byte ThirtySecondNotesPerQuarterNote { get; private set; } = thirtySecondNotesPerQuarterNote;

        /// <summary>
        /// Indicates the default time signature
        /// </summary>
        public static TimeSignature Default => new(4, 4, 24, 8);
        /// <summary>
        /// Retrieves a string representation of the time signature
        /// </summary>
        /// <returns>A string representing the time signature</returns>
        public override readonly string ToString() => $"{Numerator}/{Denominator}";
    }

    public struct KeySignature
    {
        /// <summary>
        /// The number of sharps in the signature (0-7)
        /// </summary>
        public byte SharpsCount { get; private set; }
        /// <summary>
        /// The number of flats in the signature (0-7)
        /// </summary>
        public byte FlatsCount { get; private set; }
        /// <summary>
        /// Indicates true if the scale is minor, otherwise false if it is major
        /// </summary>
        public bool IsMinor { get; private set; }
        /// <summary>
        /// Creates a new instance with the specified paramters
        /// </summary>
        /// <param name="sigCode">The signature code: negative for flats, positive for sharps (-7 to 7, inclusive)</param>
        /// <param name="isMinor">Indicates whether or not the scale is minor</param>
        public KeySignature(sbyte sigCode, bool isMinor)
        {
            if (0 > sigCode)
            {
                FlatsCount = unchecked((byte)-sigCode);
                SharpsCount = 0;
            }
            else
            {
                FlatsCount = 0;
                SharpsCount = unchecked((byte)sigCode);
            }
            IsMinor = isMinor;
        }
        /// <summary>
        /// Indicates the default value for the MIDI key signature
        /// </summary>
        public static KeySignature Default => new(0, false);
        /// <summary>
        /// Retrieves a string representation of the key signature
        /// </summary>
        /// <returns>A string representing the key signature</returns>
        public override readonly string ToString()
        {
            sbyte scode;
            if (0 < FlatsCount)
                scode = unchecked((sbyte)-FlatsCount);
            else
                scode = unchecked((sbyte)SharpsCount);
            if (!IsMinor)
            {
                const string FLATS = " FBbEbAbDbGbCb";
                const string SHARPS = "G D E A B F#C#";

                if (0 == scode)
                    return "C major";
                if (0 > scode)
                    return FLATS.Substring((-scode) * 2, 2).TrimStart() + " major";
                //else if(0<scode)
                return SHARPS.Substring(scode * 2, 2).TrimStart() + " major";
            }
            else
            {
                const string FLATS = " D G C FBbEbAb";
                const string SHARPS = " E BF#C#G#D#A#";

                if (0 == scode)
                    return "A minor";
                if (0 > scode)
                    return FLATS.Substring((-scode) * 2, 2).TrimStart() + " minor";
                //else if(0<scode)
                return SHARPS.Substring(scode * 2, 2).TrimStart() + " minor";
            }

        }
    }
}
