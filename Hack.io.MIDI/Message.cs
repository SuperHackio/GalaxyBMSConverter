using Hack.io.Interface;
using System.Text;

namespace Hack.io.MIDI;

public partial class MIDI : List<MIDI.Sequence>, ILoadSaveFile
{
    /// <summary>
    /// Creates a MIDI message with the specified status byte
    /// </summary>
    /// <param name="status">The MIDI status byte</param>
    public partial class Message(byte status) : ICloneable
    {
        /// <summary>
        /// Indicates the MIDI status byte
        /// </summary>
        public byte Status { get; private set; } = status;
        /// <summary>
        /// Indicates the channel of the MIDI message. Only applies to MIDI channel messages, not MIDI system messages
        /// </summary>
        public byte Channel => unchecked((byte)(Status & 0xF));
        /// <summary>
        /// Indicates the length of the message payload
        /// </summary>
        public virtual int PayloadLength => 0;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected virtual Message CloneImpl() => new(Status);
        /// <summary>
        /// Creates a deep copy of the message
        /// </summary>
        /// <returns>A message that is equivelent to the specified message</returns>
        public Message Clone() => CloneImpl();
        object ICloneable.Clone() => Clone();
    }

    /// <summary>
    /// Creates a MIDI message with the specified status and payload
    /// </summary>
    /// <param name="status">The MIDI status byte</param>
    /// <param name="data1">The data byte</param>
    public partial class MessageByte(byte status, byte data1) : Message(status)
    {
        /// <summary>
        /// Indicates the data byte for the MIDI message
        /// </summary>
        public byte Data1 { get; private set; } = data1;
        /// <summary>
        /// Indicates the payload length for this MIDI message
        /// </summary>
        public override int PayloadLength => 1;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageByte(Status, Data1);
    }

    /// <summary>
    /// Represents a MIDI message a payload word (2 bytes)
    /// </summary>
    public partial class MessageWord : MessageByte
    {
        /// <summary>
        /// Creates a MIDI message with the specified status and payload
        /// </summary>
        /// <param name="status">The MIDI status byte</param>
        /// <param name="data1">The first data byte</param>
        /// <param name="data2">The second data byte</param>
        public MessageWord(byte status, byte data1, byte data2) : base(status, data1) => Data2 = data2;
        /// <summary>
        /// Creates a MIDI message with the specified status and payload
        /// </summary>
        /// <param name="status">The MIDI status byte</param>
        /// <param name="data">The data word</param>
        public MessageWord(byte status, short data) : base(status, unchecked((byte)(data & 0x7F))) => Data2 = unchecked((byte)((data / 256) & 0x7F));
        /// <summary>
        /// Indicates the payload length for this MIDI message
        /// </summary>
        public override int PayloadLength => 2;
        /// <summary>
        /// Indicates the second data byte
        /// </summary>
        public byte Data2 { get; private set; }
        /// <summary>
        /// Indicates the data word
        /// </summary>
        public short Data => unchecked((short)(Data1 + Data2 * 256));
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageWord(Status, Data1, Data2);
    }

    /// <summary>
    /// Represents a MIDI meta-event message with an arbitrary length payload
    /// </summary>
    public partial class MessageMeta : MessageByte
    {
        /// <summary>
        /// Creates a MIDI message with the specified status, type and payload
        /// </summary>
        /// <param name="type">The type of the MIDI message</param>
        /// <param name="data">The payload of the MIDI message, as bytes</param>
        public MessageMeta(byte type, byte[] data) : base(0xFF, type) => Data = data;
        /// <summary>
        /// Creates a MIDI message with the specified status, type and payload
        /// </summary>
        /// <param name="type">The type of the MIDI message</param>
        /// <param name="text">The payload of the MIDI message, as ASCII text</param>
        public MessageMeta(byte type, string text) : base(0xFF, type) => Data = Encoding.ASCII.GetBytes(text);
        /// <summary>
        /// Indicates the type of the meta-message
        /// </summary>
        public byte Type => Data1;
        /// <summary>
        /// Indicates the payload length for this MIDI message
        /// </summary>
        public override int PayloadLength => -1;
        /// <summary>
        /// Indicates the payload data, as bytes
        /// </summary>
        public byte[] Data { get; private set; }
        /// <summary>
        /// Indicates the payload data, as ASCII text
        /// </summary>
        public string Text => Encoding.ASCII.GetString(Data);
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMeta(Type, Data);
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Meta: {Type:x2}, Length: {Data.Length}";
    }
    
    /// <summary>
    /// Represents a MIDI sequence number meta message
    /// </summary>
    public partial class MessageMetaSequenceNumber : MessageMeta
    {
        internal MessageMetaSequenceNumber(byte[] data) : base(0, data) { }
        /// <summary>
        /// Creates a new message with the specified sequence number
        /// </summary>
        /// <param name="sequenceNumber">The sequence number</param>
        public MessageMetaSequenceNumber(short sequenceNumber) : base(0, new byte[] { unchecked((byte)(sequenceNumber & 0x7F)), unchecked((byte)((sequenceNumber / 256) & 0x7F)) })
        {

        }
        /// <summary>
        /// Creates a new message with the default sequence number
        /// </summary>
        public MessageMetaSequenceNumber() : base(0, Array.Empty<byte>())
        {

        }

        /// <summary>
        /// Indicates the sequence number, or -1 if there was none specified
        /// </summary>
        public short SequenceNumber
        {
            get
            {
                if (0 == Data.Length)
                    return -1;
                return unchecked((short)(Data[0] + Data[1] * 256));
            }
        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Sequence Number: {(0 == Data.Length ? "<default>" : SequenceNumber.ToString())}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaSequenceNumber(Data);
    }

    /// <summary>
    /// Represents a MIDI text meta message
    /// </summary>
    public partial class MessageMetaText : MessageMeta
    {
        internal MessageMetaText(byte[] data) : base(1, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaText(string text) : base(1, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Text: {Text}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaText(Data);
    }

    /// <summary>
    /// Represents a MIDI copyright meta message
    /// </summary>
    partial class MessageMetaCopyright : MessageMeta
    {
        internal MessageMetaCopyright(byte[] data) : base(2, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaCopyright(string text) : base(2, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Copyright: {Text}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaCopyright(Data);
    }

    /// <summary>
    /// Represents a MIDI sequence/track name meta message
    /// </summary>
    public partial class MessageMetaSequenceOrTrackName : MessageMeta
    {
        internal MessageMetaSequenceOrTrackName(byte[] data) : base(3, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaSequenceOrTrackName(string text) : base(3, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Sequence/Track Name: {Text}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaSequenceOrTrackName(Data);
    }
    
    /// <summary>
    /// Represents a MIDI instrument name meta message
    /// </summary>
    public partial class MessageMetaInstrumentName : MessageMeta
    {
        internal MessageMetaInstrumentName(byte[] data) : base(4, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaInstrumentName(string text) : base(4, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => "Instrument Name: " + Text;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaInstrumentName(Data);
    }
    
    /// <summary>
    /// Represents a MIDI lyric meta message
    /// </summary>
    public partial class MessageMetaLyric : MessageMeta
    {
        internal MessageMetaLyric(byte[] data) : base(5, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaLyric(string text) : base(5, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Lyric: {Text}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaLyric(Data);
    }
    
    /// <summary>
    /// Represents a MIDI marker meta message
    /// </summary>
    public partial class MessageMetaMarker : MessageMeta
    {
        internal MessageMetaMarker(byte[] data) : base(6, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaMarker(string text) : base(6, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Marker: {Text}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaMarker(Data);
    }
    
    /// <summary>
    /// Represents a MIDI cue point meta message
    /// </summary>
    public partial class MessageMetaCuePoint : MessageMeta
    {
        internal MessageMetaCuePoint(byte[] data) : base(7, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaCuePoint(string text) : base(7, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Cue Point: {Text}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaCuePoint(Data);
    }
    
    /// <summary>
    /// Represents a MIDI program name meta message
    /// </summary>
    public partial class MessageMetaProgramName : MessageMeta
    {
        internal MessageMetaProgramName(byte[] data) : base(8, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaProgramName(string text) : base(8, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Program Name: {Text}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaProgramName(Data);
    }
    
    /// <summary>
    /// Represents a MIDI device port name meta message
    /// </summary>
    public partial class MessageMetaDevicePortName : MessageMeta
    {
        internal MessageMetaDevicePortName(byte[] data) : base(9, data) { }
        /// <summary>
        /// Creates a new instance with the specified text
        /// </summary>
        /// <param name="text">The text</param>
        public MessageMetaDevicePortName(string text) : base(9, text ?? "")
        {

        }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Device Port Name: {Text}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaDevicePortName(Data);
    }
    
    /// <summary>
    /// Represents a MIDI channel prefix meta message
    /// </summary>
    public partial class MessageMetaChannelPrefix : MessageMeta
    {
        internal MessageMetaChannelPrefix(byte[] data) : base(0x20, data) { }
        /// <summary>
        /// Creates a new instance with the specified channel
        /// </summary>
        /// <param name="channelPrefix">The channel (0-15)</param>
        public MessageMetaChannelPrefix(byte channelPrefix) : base(0x20, new byte[] { unchecked((byte)(channelPrefix & 0x0F)) })
        {

        }
        /// <summary>
        /// Indicates the channel for the channel prefix
        /// </summary>
        public byte ChannelPrefix { get { return Data[0]; } }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Channel Prefix: {ChannelPrefix}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaChannelPrefix(Data);
    }
    
    /// <summary>
    /// Represents a MIDI port meta message
    /// </summary>
    public partial class MessageMetaPort : MessageMeta
    {
        internal MessageMetaPort(byte[] data) : base(0x21, data) { }
        /// <summary>
        /// Creates a new instance with the specified port
        /// </summary>
        /// <param name="port">The port (0-127)</param>
        public MessageMetaPort(byte port) : base(0x21, new byte[] { unchecked((byte)(port & 0x7F)) })
        {

        }
        /// <summary>
        /// Indicates the port
        /// </summary>
        public byte Port { get { return Data[0]; } }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Port: {Port}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaPort(Data);
    }
    
    /// <summary>
    /// Represents a MIDI end of track meta message
    /// </summary>
    public partial class MessageMetaEndOfTrack : MessageMeta
    {
        internal MessageMetaEndOfTrack(byte[] data) : base(0x2F, data) { }
        /// <summary>
        /// Creates a new instance 
        /// </summary>
        public MessageMetaEndOfTrack() : base(0x2F, Array.Empty<byte>()) { }
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => "<End of Track>";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaEndOfTrack(Data);
    }
    
    /// <summary>
    /// Represents a MIDI tempo meta message
    /// </summary>
    public partial class MessageMetaTempo : MessageMeta
    {
        internal MessageMetaTempo(byte[] data) : base(0x51, data) { }
        /// <summary>
        /// Creates a new instance with the specified tempo
        /// </summary>
        /// <param name="tempo">The tempo</param>
        public MessageMetaTempo(double tempo) : this(MidiUtility.TempoToMicroTempo(tempo))
        {

        }
        /// <summary>
        /// Creates a new instance with the specified microtempo
        /// </summary>
        /// <param name="microTempo">The microtempo</param>
        public MessageMetaTempo(int microTempo) : base(0x51, BitConverter.IsLittleEndian ? //Might not be needed...
                                [unchecked((byte)(microTempo >> 16)), unchecked((byte)((microTempo >> 8) & 0xFF)), unchecked((byte)(microTempo & 0xFF))] :
                                [unchecked((byte)(microTempo & 0xFF)), unchecked((byte)((microTempo >> 8) & 0xFF)), unchecked((byte)(microTempo >> 16))])
        { }
        /// <summary>
        /// Indicates the microtempo of the MIDI message
        /// </summary>
        public int MicroTempo => BitConverter.IsLittleEndian ? (Data[0] << 16) | (Data[1] << 8) | Data[2] : (Data[2] << 16) | (Data[1] << 8) | Data[0];
        /// <summary>
        /// Indicates the tempo of the MIDI message
        /// </summary>
        public double Tempo => MidiUtility.MicroTempoToTempo(MicroTempo);
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Tempo: {Tempo}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaTempo(Data);
    }

    /// <summary>
    /// Represents a MIDI time signature meta message
    /// </summary>
    public partial class MessageMetaTimeSignature : MessageMeta
    {
        internal MessageMetaTimeSignature(byte[] data) : base(0x58, data) { }
        /// <summary>
        /// Creates a new instance with the specified tempo
        /// </summary>
        /// <param name="timeSignature">The time signature</param>
        public MessageMetaTimeSignature(TimeSignature timeSignature) : base(0x58, new byte[] {
                        timeSignature.Numerator,
                        unchecked((byte)(Math.Log(timeSignature.Denominator)/Math.Log(2))),
                        timeSignature.MidiTicksPerMetronomeTick,
                        timeSignature.ThirtySecondNotesPerQuarterNote })
        {

        }
        /// <summary>
        /// Indicates the time signature
        /// </summary>
        public TimeSignature TimeSignature => new(Data[0], (short)Math.Pow(2, Data[1]), Data[2], Data[3]);
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Time Signature: {TimeSignature}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaTimeSignature(Data);
    }

    /// <summary>
    /// Represents a MIDI time signature meta message
    /// </summary>
    public partial class MessageMetaKeySignature : MessageMeta
    {
        internal MessageMetaKeySignature(byte[] data) : base(0x59, data) { }
        /// <summary>
        /// Creates a new instance with the specified tempo
        /// </summary>
        /// <param name="keySignature">The time signature</param>
        public MessageMetaKeySignature(KeySignature keySignature) : base(0x59, new byte[] {
                        unchecked((byte)(0<keySignature.FlatsCount?-keySignature.FlatsCount:keySignature.SharpsCount)),
                        unchecked((byte)(keySignature.IsMinor?1:0))})
        {

        }
        /// <summary>
        /// Indicates the key signature
        /// </summary>
        public KeySignature KeySignature => new(unchecked((sbyte)Data[0]), 0 != Data[1]);
        /// <summary>
        /// Retrieves a string representation of the message
        /// </summary>
        /// <returns>A string representing the message</returns>
        public override string ToString() => $"Key Signature: {KeySignature}";
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageMetaKeySignature(Data);
    }

    /// <summary>
    /// Represents a MIDI system exclusive message with an arbitrary length payload
    /// </summary>
    /// <remarks>
    /// Creates a MIDI message with the specified status, type and payload
    /// </remarks>
    /// <param name="data">The payload of the MIDI message, as bytes</param>
    public partial class MessageSysex(byte[] data) : Message(0xF0)
    {
        /// <summary>
        /// Indicates the payload length for this MIDI message
        /// </summary>
        public override int PayloadLength => -1;
        /// <summary>
        /// Indicates the payload data, as bytes
        /// </summary>
        public byte[] Data { get; private set; } = data;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageSysex(Data);
        /// <summary>
        /// Returns a string representation of the message
        /// </summary>
        /// <returns>The string representation of the message</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Sysex: ");
            for (var i = 0; i < Data.Length; i++)
                sb.Append(Data[i].ToString("X2"));
            return sb.ToString();
        }
    }

    // internal class used by the framework
    //sealed class MidiMessageSysexPart : MidiMessage
    //{
    //    internal MidiMessageSysexPart(byte[] data) : base(0)
    //    {
    //        Data = data;
    //    }
    //    public byte[] Data { get; set; }
    //    public override int PayloadLength => -1;
    //    /// <summary>
    //    /// When overridden in a derived class, implements Clone()
    //    /// </summary>
    //    /// <returns>The cloned MIDI message</returns>
    //    protected override MidiMessage CloneImpl()
    //    {
    //        return new MidiMessageSysexPart(Data);
    //    }
    //}

    /// <summary>
    /// Represents a MIDI song position message
    /// </summary>
    public sealed class MessageSongPosition : MessageWord
    {
        /// <summary>
        /// Creates a new MIDI song position message
        /// </summary>
        /// <param name="position">Indicates the new song position in beats since the start</param>
        public MessageSongPosition(short position) : base(0xF2, position)
        {
        }
        /// <summary>
        /// Creates a new MIDI song position message
        /// </summary>
        /// <param name="position1">The high part of the position</param>
        /// <param name="position2">The low part of the position</param>
        public MessageSongPosition(byte position1, byte position2) : base(0xF2, position1, position2)
        {
        }
        /// <summary>
        /// Indicates the new song position in beats since the start
        /// </summary>
        public short Position => Data;
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageSongPosition(Position);
    }

    /// <summary>
    /// Represents a MIDI song select message
    /// </summary>
    /// <remarks>
    /// Creates a new MIDI song select message
    /// </remarks>
    /// <param name="songId">Indicates the new song id</param>
    public sealed class MessageSongSelect(byte songId) : MessageByte(0xF3, songId)
    {
        /// <summary>
        /// Indicates the song id
        /// </summary>
        public byte SongId => Data1;
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageSongSelect(SongId);
    }

    /// <summary>
    /// Represents a MIDI tune request message
    /// </summary>
    public sealed class MessageTuneRequest : Message
    {
        /// <summary>
        /// Creates a new MIDI tune request message
        /// </summary>
        public MessageTuneRequest() : base(0xF6)
        {
        }
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageTuneRequest();
    }

    /// <summary>
    /// Represents a MIDI note on message
    /// </summary>
    /// <remarks>
    /// Creates a new MIDI note on message
    /// </remarks>
    /// <param name="noteId">The MIDI note id (0-127)</param>
    /// <param name="velocity">The MIDI velocity (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public partial class MessageNoteOn(byte noteId, byte velocity, byte channel) : MessageWord(unchecked((byte)(0x90 | channel)), noteId, velocity)
    {
        /// <summary>
        /// Creates a new MIDI note on message
        /// </summary>
        /// <param name="note">The MIDI note</param>
        /// <param name="velocity">The MIDI velocity (0-127)</param>
        /// <param name="channel">The MIDI channel (0-15)</param>
        public MessageNoteOn(string note, byte velocity, byte channel) : this(MidiUtility.NoteToNoteId(note), velocity, channel)
        {

        }
        /// <summary>
        /// Indicates the MIDI note id to play
        /// </summary>
        public byte NoteId => Data1;
        /// <summary>
        /// Indicates the note for the message
        /// </summary>
        public string Note => MidiUtility.NoteIdToNote(NoteId, true);
        /// <summary>
        /// Indicates the velocity of the note to play
        /// </summary>
        public byte Velocity => Data2;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageNoteOn(NoteId, Velocity, Channel);
        /// <summary>
        /// Gets a string representation of this message
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Note On: {Note}, Velocity: {Velocity}, Channel: {Channel}";
    }

    /// <summary>
    /// Represents a MIDI note off message
    /// </summary>
    /// <remarks>
    /// Creates a new MIDI note off message
    /// </remarks>
    /// <param name="noteId">The MIDI note id (0-127)</param>
    /// <param name="velocity">The MIDI velocity (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    /// <remarks><paramref name="velocity"/> is not used</remarks>
    public partial class MessageNoteOff(byte noteId, byte velocity, byte channel) : MessageWord(unchecked((byte)(0x80 | channel)), noteId, velocity)
    {
        /// <summary>
        /// Creates a new MIDI note off message
        /// </summary>
        /// <param name="note">The MIDI note</param>
        /// <param name="velocity">The MIDI velocity (0-127)</param>
        /// <param name="channel">The MIDI channel (0-15)</param>
        /// <remarks><paramref name="velocity"/> is not used</remarks>
        public MessageNoteOff(string note, byte velocity, byte channel) : this(MidiUtility.NoteToNoteId(note), velocity, channel)
        {

        }
        /// <summary>
        /// Indicates the MIDI note id to turn off
        /// </summary>
        public byte NoteId => Data1;
        /// <summary>
        /// Indicates the note for the message
        /// </summary>
        public string Note => MidiUtility.NoteIdToNote(NoteId, true);
        /// <summary>
        /// Indicates the velocity of the note to turn off
        /// </summary>
        /// <remarks>This value is not used</remarks>
        public byte Velocity => Data2;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageNoteOff(NoteId, Velocity, Channel);
        /// <summary>
        /// Gets a string representation of this message
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Note Off: {MidiUtility.NoteIdToNote(NoteId)}, Velocity: {Velocity}, Channel: {Channel}";
    }

    /// <summary>
    /// Represents a MIDI key pressure/aftertouch message
    /// </summary>
    /// <remarks>
    /// Creates a new MIDI key pressure/aftertouch message
    /// </remarks>
    /// <param name="noteId">The MIDI note id (0-127)</param>
    /// <param name="pressure">The MIDI pressure (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public partial class MessageKeyPressure(byte noteId, byte pressure, byte channel) : MessageWord(unchecked((byte)(0xA0 | channel)), noteId, pressure)
    {
        /// <summary>
        /// Creates a new MIDI key pressure/aftertouch message
        /// </summary>
        /// <param name="note">The MIDI note</param>
        /// <param name="pressure">The MIDI pressure (0-127)</param>
        /// <param name="channel">The MIDI channel (0-15)</param>
        public MessageKeyPressure(string note, byte pressure, byte channel) : this(MidiUtility.NoteToNoteId(note), pressure, channel)
        {

        }
        /// <summary>
        /// Indicates the assocated MIDI note id
        /// </summary>
        public byte NoteId => Data1;
        /// <summary>
        /// Indicates the note for the message
        /// </summary>
        public string Note => MidiUtility.NoteIdToNote(NoteId, true);
        /// <summary>
        /// Indicates the pressure of the note (aftertouch)
        /// </summary>
        public byte Pressure => Data2;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageKeyPressure(NoteId, Pressure, Channel);
        /// <summary>
        /// Gets a string representation of this message
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Key Pressure: {Note}, Pressure: {Pressure}, Channel: {Channel}";
    }

    /// <summary>
    /// Represents a MIDI continuous controller message
    /// </summary>
    /// <remarks>
    /// Creates a new MIDI continuous controller message
    /// </remarks>
    /// <param name="controlId">The MIDI controller id (0-127)</param>
    /// <param name="value">The MIDI value (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public partial class MessageControlChange(byte controlId, byte value, byte channel) : MessageWord(unchecked((byte)(0xB0 | channel)), controlId, value)
    {
        /// <summary>
        /// Indicates the assocated MIDI controller id
        /// </summary>
        public byte ControlId => Data1;
        /// <summary>
        /// Indicates the value of the controller
        /// </summary>
        public byte Value => Data2;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageControlChange(ControlId, Value, Channel);
        /// <summary>
        /// Gets a string representation of this message
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"CC: {ControlId}, Value: {Value}, Channel: {Channel}";
    }

    /// <summary>
    /// Represents a MIDI key pressure/aftertouch message
    /// </summary>
    /// <remarks>
    /// Creates a new MIDI key pressure/aftertouch message
    /// </remarks>
    /// <param name="patchId">The MIDI patch Id (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public partial class MessagePatchChange(byte patchId, byte channel) : MessageByte(unchecked((byte)(0xC0 | channel)), patchId)
    {
        /// <summary>
        /// Indicates the assocated MIDI patch id
        /// </summary>
        public byte PatchId => Data1;

        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessagePatchChange(PatchId, Channel);
        /// <summary>
        /// Gets a string representation of this message
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Patch Change: {PatchId}, Channel: {Channel}";
    }

    /// <summary>
    /// Represents a MIDI key pressure/aftertouch message
    /// </summary>
    /// <remarks>
    /// Creates a new MIDI key pressure/aftertouch message
    /// </remarks>
    /// <param name="pressure">The MIDI pressure (0-127)</param>
    /// <param name="channel">The MIDI channel (0-15)</param>
    public partial class MessageChannelPressure(byte pressure, byte channel) : MessageByte(unchecked((byte)(0xD0 | channel)), pressure)
    {
        /// <summary>
        /// Indicates the pressure of the channel (aftertouch)
        /// </summary>
        /// <remarks>Indicates the single greatest pressure/aftertouch off all pressed notes</remarks>
        public byte Pressure => Data1;
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageChannelPressure(Pressure, Channel);
        /// <summary>
        /// Gets a string representation of this message
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Channel Pressure: {Pressure}, Channel: {Channel}";
    }
    
    /// <summary>
    /// Represents a MIDI channel pitch/pitch wheel message
    /// </summary>
    public partial class MessageChannelPitch : MessageWord
    {
        /// <summary>
        /// Creates a new MIDI channel pitch message
        /// </summary>
        /// <param name="pitch">The MIDI pitch (0-16383)</param>
        /// <param name="channel">The MIDI channel (0-15)</param>
        public MessageChannelPitch(short pitch, byte channel) : base(unchecked((byte)(0xE0 | channel)), BitConverter.IsLittleEndian ? /*MidiUtility.Swap*/(pitch) : pitch) //TODO: Fix this?
        {

        }
        internal MessageChannelPitch(byte pitch1, byte pitch2, byte channel) : base(unchecked((byte)(0xE0 | channel)), pitch1, pitch2)
        {

        }
        /// <summary>
        /// Indicates the pitch of the channel (pitch wheel position)
        /// </summary>
        public short Pitch => unchecked((short)(Data1 + Data2 * 128));
        /// <summary>
        /// When overridden in a derived class, implements Clone()
        /// </summary>
        /// <returns>The cloned MIDI message</returns>
        protected override Message CloneImpl() => new MessageChannelPitch(Data1, Data2, Channel);
        /// <summary>
        /// Gets a string representation of this message
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Channel Pitch: {Pitch}, Channel: {Channel}";
    }
    
    /// <summary>
    /// Represents a MIDI real-time message
    /// </summary>
    public abstract class MessageRealTime : Message
    {
        /// <summary>
        /// Creates a MIDI real-time message
        /// </summary>
        /// <param name="status"></param>
        protected MessageRealTime(byte status) : base(status)
        {

        }
    }
    
    /// <summary>
    /// Represents a MIDI reset message
    /// </summary>
    /// <remarks>This message shares the same status code with MIDI a meta-message. The meta-messages come from files but this comes over the wire whereas meta-messages do not.</remarks>
    public sealed class MessageRealTimeReset : MessageRealTime
    {
        /// <summary>
        /// Creates a new MIDI reset message
        /// </summary>
        public MessageRealTimeReset() : base(0xFF)
        {
        }
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageRealTimeReset();
        /// <summary>
        /// Indicates the payload of the message
        /// </summary>
        public override int PayloadLength => 0;
    }

    /// <summary>
    /// Represents a MIDI active sensing message
    /// </summary>
    public sealed class MessageRealTimeActiveSensing : MessageRealTime
    {
        /// <summary>
        /// Creates a new MIDI active sensing message
        /// </summary>
        public MessageRealTimeActiveSensing() : base(0xFE)
        {
        }
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageRealTimeActiveSensing();
        /// <summary>
        /// Indicates the payload of the message
        /// </summary>
        public override int PayloadLength => 0;
    }
    
    /// <summary>
    /// Represents a MIDI start message
    /// </summary>
    public sealed class MessageRealTimeStart : MessageRealTime
    {
        /// <summary>
        /// Creates a new MIDI start message
        /// </summary>
        public MessageRealTimeStart() : base(0xFA)
        {
        }
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageRealTimeStart();
        /// <summary>
        /// Indicates the payload of the message
        /// </summary>
        public override int PayloadLength => 0;
    }
    
    /// <summary>
    /// Represents a MIDI continue message
    /// </summary>
    public sealed class MessageRealTimeContinue : MessageRealTime
    {
        /// <summary>
        /// Creates a new MIDI continue message
        /// </summary>
        public MessageRealTimeContinue() : base(0xFB)
        {
        }
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageRealTimeContinue();
        /// <summary>
        /// Indicates the payload of the message
        /// </summary>
        public override int PayloadLength => 0;
    }
    
    public sealed class MessageRealTimeStop : MessageRealTime
    {
        /// <summary>
        /// Creates a new MIDI stop message
        /// </summary>
        public MessageRealTimeStop() : base(0xFC)
        {
        }
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageRealTimeStop();
        /// <summary>
        /// Indicates the payload of the message
        /// </summary>
        public override int PayloadLength => 0;
    }
    
    /// <summary>
    /// Represents a MIDI start message
    /// </summary>
    public sealed class MessageRealTimeTimingClock : MessageRealTime
    {
        /// <summary>
        /// Creates a new MIDI timing clock message
        /// </summary>
        public MessageRealTimeTimingClock() : base(0xF8)
        {
        }
        /// <summary>
        /// Clones the message
        /// </summary>
        /// <returns>The new message</returns>
        protected override Message CloneImpl() => new MessageRealTimeTimingClock();
        /// <summary>
        /// Indicates the payload of the message
        /// </summary>
        public override int PayloadLength => 0;
    }
}