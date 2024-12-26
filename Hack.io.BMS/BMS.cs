using Hack.io.Interface;
using Hack.io.Utility;
using static Hack.io.BMS.BMS;

namespace Hack.io.BMS;

public partial class BMS : List<(int ID, Track Track)>, ILoadSaveFile
{
    public List<List<OpcodeBase>> Subroutines = [];

    public void Load(Stream Strm)
    {
        throw new NotImplementedException();
    }

    public void Save(Stream Strm)
    {
        SaveTracks(Strm, this, true); //We want the subroutines placed after the root track, but before the Parent. This is because the call instruction only takes 24 bits as address.

        //Resolve all subroutine calls
        foreach ((OpcodeBase Opcode, long OffsetPosition) in SubroutineAddressBook)
        {
            int? idx = null;
            if (Opcode is OpcodeCall opc)
                idx = opc.SubroutineID;

            if (idx is null)
                throw new NullReferenceException($"Unable to find Subroutine with Index {idx}");

            Strm.Position = OffsetPosition;
            uint final = (uint)(SubroutineOffsets[idx.Value] ?? throw new NullReferenceException($"Function with id {idx} was not written to the file."));
            final |= (uint)(Opcode.TypeByte << 24); //Subroutines only support 24 bit calls
            Strm.WriteUInt32(final);
        }
    }

    public class Track : List<OpcodeBase>
    {
        public List<(int ID, Track Track)> Children = [];
    }

    // These are only to be used during Save
    private long?[] SubroutineOffsets = [];
    private List<(OpcodeBase Opcode, long OffsetPosition)> SubroutineAddressBook = [];
    private void SaveTracks(Stream Strm, List<(int ID, Track Track)> Source, bool AppendSubsNow = false)
    {
        SubroutineOffsets = new long?[Subroutines.Count];
        SubroutineAddressBook = [];
        List<(long OffsetLocation, int TrackID)> TrackOffsets = new(Source.Count);
        for (int i = 0; i < Source.Count; i++)
        {
            TrackOffsets.Clear();
            SaveTrack(Source[i].Track);

            if (AppendSubsNow)
            {
                // pretty sure we can just write all of this verbatim
                for (int x = 0; x < Subroutines.Count; x++)
                {
                    SubroutineOffsets[x] = Strm.Position;
                    SaveTrack(Subroutines[x]); // Sneaky Sneaky
                }
            }

            foreach ((long OffsetLocation, int TrackID) in TrackOffsets)
            {
                long Offset = Strm.Position;
                bool HasWrittenChild = false;
                for (int j = 0; j < Source[i].Track.Children.Count; j++)
                {
                    if (Source[i].Track.Children[j].ID == TrackID)
                    {
                        List<(int, Track)> WhyDidIDoThisToMyself = [Source[i].Track.Children[j]];
                        SaveTracks(Strm, WhyDidIDoThisToMyself);
                        HasWrittenChild = true;
                        break;
                    }
                }
                if (!HasWrittenChild)
                    throw new MissingMemberException($"Failed to find the requested BMS Track {TrackID}");

                long pausepos = Strm.Position;
                Strm.Position = OffsetLocation;
                WriteOpenTrack((uint)TrackID, Offset);
                Strm.Position = pausepos;
            }
        }

        void WriteOpenTrack(uint track, long pos)
        {
            Strm.WriteByte(0xC1); // Open Track
            uint final = (uint)pos;
            final |= track << 24;
            Strm.WriteUInt32(final);
        }

        void SaveTrack(List<OpcodeBase> Trk)
        {
            List<long> OpPos = [];
            List<(OpcodeBase Opcode, long OffsetPosition)> AddressBook = []; //Write the locations of where all the jump instructions are supposed to be, and set the Opcode to the jump instruction
            foreach (OpcodeBase item in Trk)
            {
                OpPos.Add(Strm.Position);
                if (item is OpcodeNoteOn OpNoteOn)
                {
                    Strm.WriteByte(OpNoteOn.TypeByte);
                    Strm.WriteByte(OpNoteOn.Voice);
                    Strm.WriteByte(OpNoteOn.Velocity);
                }
                else if (item is OpcodeNoteOff OpNoteOff)
                {
                    Strm.WriteByte(OpNoteOff.TypeByte); //TypeByte merges them for us. Hooray!
                }
                else if (item is OpcodeExtendedOpcode OpExtOp)
                {
                    Strm.WriteByte(OpExtOp.TypeByte);
                    Strm.WriteByte(OpExtOp.ExtendedTypeByte);
                    // SMG1/2 do not support any Extended opcodes that read beyond these 2 bytes
                }
                else if (item is OpcodeOpenTrack OpOpenTrack)
                {
                    TrackOffsets.Add((Strm.Position, OpOpenTrack.Id));
                    Strm.WritePlaceholder(5); //Open Tracks
                }
                else if (item is OpcodeCall OpCall)
                {
                    SubroutineAddressBook.Add((OpCall, Strm.Position));
                    Strm.WritePlaceholder(4);
                }
                else if (item is OpcodeReturn)
                {
                    Strm.WriteByte(item.TypeByte);
                }
                else if (item is OpcodeJump OpJump)
                {
                    AddressBook.Add((OpJump, Strm.Position));
                    Strm.WritePlaceholder(4);
                }
                else if (item is OpcodeSetArticulation OpSetVar)
                {
                    Strm.WriteByte(OpSetVar.TypeByte); // Set variable
                    Strm.WriteEnum<ArticulationTarget, byte>(OpSetVar.Target, StreamUtil.WriteUInt8);
                    Strm.WriteUInt16(OpSetVar.Value); // to the user value
                }
                else if (item is OpcodeSetTempo OpSetBPM)
                {
                    Strm.WriteByte(OpSetBPM.TypeByte); // Set BPM
                    Strm.WriteUInt16(OpSetBPM.Value); // to the user value
                }
                else if (item is OpcodeSetBank OpSetBank)
                {
                    Strm.WriteByte(OpSetBank.TypeByte);
                    Strm.WriteByte(OpSetBank.Value);
                }
                else if (item is OpcodeSetProgram OpSetProgram)
                {
                    Strm.WriteByte(OpSetProgram.TypeByte);
                    Strm.WriteByte(OpSetProgram.Value);
                }
                else if (item is OpcodeSetParamInt8 OpSetParamInt8)
                {
                    Strm.WriteByte(OpSetParamInt8.TypeByte);
                    Strm.WriteEnum<ParamTarget, byte>(OpSetParamInt8.Target, StreamUtil.WriteUInt8);
                    Strm.WriteByte(OpSetParamInt8.Value);
                }
                else if (item is OpcodeSetParamInt16 OpSetParamInt16)
                {
                    Strm.WriteByte(OpSetParamInt16.TypeByte);
                    Strm.WriteEnum<ParamTarget, byte>(OpSetParamInt16.Target, StreamUtil.WriteUInt8);
                    Strm.WriteInt16(OpSetParamInt16.Value);
                }
                else if (item is OpcodeSetParamInt8Time OpSetParamInt8Time)
                {
                    Strm.WriteByte(OpSetParamInt8Time.TypeByte);
                    Strm.WriteEnum<ParamTarget, byte>(OpSetParamInt8Time.Target, StreamUtil.WriteUInt8);
                    Strm.WriteByte(OpSetParamInt8Time.Value);
                    Strm.WriteInt16(OpSetParamInt8Time.Time);
                }
                else if (item is OpcodeSetParamInt16Time OpSetParamInt16Time)
                {
                    Strm.WriteByte(OpSetParamInt16Time.TypeByte);
                    Strm.WriteEnum<ParamTarget, byte>(OpSetParamInt16Time.Target, StreamUtil.WriteUInt8);
                    Strm.WriteInt16(OpSetParamInt16Time.Value);
                    Strm.WriteInt16(OpSetParamInt16Time.Time);
                }
                else if (item is OpcodeWaitVar OpWaitVar)
                {
                    Strm.WriteByte(OpWaitVar.TypeByte); // Wait
                    Strm.WriteVariableLength(OpWaitVar.Value);
                }
                else if (item is OpcodeWaitInt8 OpWaitInt8)
                {
                    Strm.WriteByte(OpWaitInt8.TypeByte); // Wait
                    Strm.WriteByte(OpWaitInt8.Value);
                }
                else if (item is OpcodePrintF OpPrintF && !string.IsNullOrWhiteSpace(OpPrintF.Value))
                {
                    Strm.WriteByte(OpPrintF.TypeByte);
                    Strm.WriteStringJIS(OpPrintF.Value);
                }
                else if (item is OpcodeNop or OpcodeFinish)
                    Strm.WriteByte(item.TypeByte);
            }

            long PausePosition = Strm.Position;

            // Now we fill in the jump holes
            foreach ((OpcodeBase Opcode, long OffsetPosition) in AddressBook)
            {
                Strm.Position = OffsetPosition;
                OpcodeBase? Addr = null;
                int Shift = 0;

                if (Opcode is OpcodeJump OpJump)
                {
                    Addr = OpJump.TargetOpcode;
                    Shift = 24;
                }
                // TODO: Support other opcodes?

                if (Addr is null)
                    throw new NullReferenceException("Jump opcode does not have a target opcode to jump to, or it was not found to be written to the file!");

                // Because opcodes have Equals implemented, I cannot just use the normal IndexOf
                int idx = Trk.IndexOfReference(Addr);
                if (idx == -1)
                    throw new NullReferenceException("Jump opcode does not have a target opcode to jump to, or it was not found to be written to the file!");

                uint final = (uint)OpPos[idx];
                final |= (uint)(Opcode.TypeByte << Shift); //I don't like this very much, but also don't support any other case so this is fine for now...
                Strm.WriteUInt32(final);
            }

            Strm.Position = PausePosition;
        }
    }
}
