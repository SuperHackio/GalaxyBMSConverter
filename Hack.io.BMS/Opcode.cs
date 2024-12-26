using Hack.io.Interface;
using static Hack.io.BMS.BMS;

namespace Hack.io.BMS;

// 0x00 - 0x7F = Note On

// TODO: Figure out 0x80. Is it Wait? Is it still Note Off?

// 0x81 - 0x87 = Note Off

// TODO: Figure out 0x88 - 0x8F

// TODO: Figure out 0x90 - 0x9F

// 0xA0 = NULL
// 0xA1 = NULL
// 0xA2 = NULL
// 0xA3 = NULL
// 0xA4 = NULL
// 0xA5 = NULL
// 0xA6 = NULL
// 0xA7 = NULL
// 0xA8 = NULL
// 0xA9 = NULL
// 0xAA = NULL
// 0xAB = NULL
// 0xAC = NULL
// 0xAD = NULL
// 0xAE = NULL
// 0xAF = NULL
// 0xB0 = Extended Opcode Indicator
// 0xB1 = JASSeqParser::cmdNoteOn
// 0xB2 = JASSeqParser::cmdNoteOff
// 0xB3 = JASSeqParser::cmdNote
// 0xB4 = JASSeqParser::cmdSetLastNote
// 0xB5 = NULL
// 0xB6 = NULL
// 0xB7 = NULL
// 0xB8 = JASSeqParser::cmdParamE
// 0xB9 = JASSeqParser::cmdParamI
// 0xBA = JASSeqParser::cmdParamEI
// 0xBB = JASSeqParser::cmdParamII
// 0xBC = NULL
// 0xBD = NULL
// 0xBE = NULL
// 0xBF = NULL
// 0xC0 = NULL
// 0xC1 = JASSeqParser::cmdOpenTrack
// 0xC2 = JASSeqParser::cmdCloseTrack
// 0xC3 = JASSeqParser::cmdCall
// 0xC4 = JASSeqParser::cmdCallF
// 0xC5 = JASSeqParser::cmdRet
// 0xC6 = JASSeqParser::cmdRetF
// 0xC7 = JASSeqParser::cmdJmp
// 0xC8 = JASSeqParser::cmdJmpF
// 0xC9 = JASSeqParser::cmdJmpTable
// 0xCA = JASSeqParser::cmdCallTable
// 0xCB = JASSeqParser::cmdLoopS
// 0xCC = JASSeqParser::cmdLoopE
// 0xCD = NULL
// 0xCE = NULL
// 0xCF = NULL
// 0xD0 = JASSeqParser::cmdReadPort
// 0xD1 = JASSeqParser::cmdWritePort
// 0xD2 = JASSeqParser::cmdCheckPortImport
// 0xD3 = JASSeqParser::cmdCheckPortExport
// 0xD4 = JASSeqParser::cmdParentWritePort
// 0xD5 = JASSeqParser::cmdChildWritePort
// 0xD6 = JASSeqParser::cmdParentReadPort
// 0xD7 = JASSeqParser::cmdChildReadPort
// 0xD8 = JASSeqParser::cmdRegLoad
// 0xD9 = JASSeqParser::cmdReg
// 0xDA = JASSeqParser::cmdReg
// 0xDB = JASSeqParser::cmdRegUni
// 0xDC = JASSeqParser::cmdRegTblLoad
// 0xDD = NULL
// 0xDE = NULL
// 0xDF = NULL
// 0xE0 = JASSeqParser::cmdTempo
// 0xE1 = JASSeqParser::cmdBankPrg
// 0xE2 = JASSeqParser::cmdBank
// 0xE3 = JASSeqParser::cmdPrg
// 0xE4 = NULL
// 0xE5 = NULL
// 0xE6 = NULL
// 0xE7 = JASSeqParser::cmdEnvScaleSet
// 0xE8 = JASSeqParser::cmdEnvSet
// 0xE9 = JASSeqParser::cmdSimpleADSR
// 0xEA = JASSeqParser::cmdBusConnect
// 0xEB = JASSeqParser::cmdIIRCutOff
// 0xEC = JASSeqParser::cmdIIRSet
// 0xED = JASSeqParser::cmdFIRSet
// 0xEE = NULL
// 0xEF = NULL
// 0xF0 = JASSeqParser::cmdWait
// 0xF1 = JASSeqParser::cmdWaitByte
// 0xF2 = NULL
// 0xF3 = JASSeqParser::cmdSetIntTable
// 0xF4 = JASSeqParser::cmdSetInterrupt
// 0xF5 = JASSeqParser::cmdDisInterrupt
// 0xF6 = JASSeqParser::cmdRetI
// 0xF7 = JASSeqParser::cmdClrI
// 0xF8 = JASSeqParser::cmdIntTimer
// 0xF9 = JASSeqParser::cmdSyncCPU
// 0xFA = NULL
// 0xFB = NULL
// 0xFC = NULL
// 0xFD = JASSeqParser::cmdPrintf
// 0xFE = JASSeqParser::cmdNop
// 0xFF = JASSeqParser::cmdFinish

// Extended Opcodes
// 0xB000 = NULL
// 0xB001 = JASSeqParser::cmdDump
// The rest are all NULL


// Opcode Table Format
// 0x00 (Int32) = 0x00000000
// 0x04 (Int32) = Virtual Function Pointer (for Virtual member functions)
// 0x08 (Int32) = Concrete Function Pointer (for non-virtual member functions)
// 0x0C (Int16) = Element Count
// 0x0E (Int16) = 8 Data Type bitsets. each set is 2 bits, and they are put right next to each other, then right shifted 2 for Element Count
//                Data Types:
//                  0 = Int8
//                  1 = Int16
//                  2 = Int24
//                  3 = Read Register (Needs investigation)


public partial class BMS : List<(int ID, Track Track)>, ILoadSaveFile
{
    public abstract class OpcodeBase
    {
        public abstract byte TypeByte { get; }

        public override bool Equals(object? obj) => obj is OpcodeBase other && TypeByte == other.TypeByte;
        public override int GetHashCode() => HashCode.Combine(TypeByte);
    }

    /// <summary>
    /// Turns on a note
    /// </summary>
    /// <remarks>
    /// Opcodes 00 - 7F
    /// </remarks>
    public class OpcodeNoteOn : OpcodeBase
    {
        private byte mVoice;

        public override byte TypeByte => Note;
        public byte Note;
        public byte Voice
        {
            get => mVoice;
            set => mVoice = (byte)(value & 0x07);
        }
        public byte Velocity;

        public override bool Equals(object? obj) =>
            obj is OpcodeNoteOn on &&
                   base.Equals(obj) &&
                   mVoice == on.mVoice &&
                   Note == on.Note &&
                   Velocity == on.Velocity;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), mVoice, Note, Velocity);
    }

    /// <summary>
    /// Shuts off a Note
    /// </summary>
    /// <remarks>
    /// Opcodes 81 - 87. There's nothing in the code stopping 80 though, so that should be investigated
    /// </remarks>
    public class OpcodeNoteOff : OpcodeBase
    {
        private byte mVoice;

        public override byte TypeByte => (byte)(0x80 | mVoice);
        public byte Voice
        {
            get => mVoice;
            set => mVoice = (byte)(value & 0x07);
        }

        public override bool Equals(object? obj) => obj is OpcodeNoteOff off &&
                   base.Equals(obj) &&
                   mVoice == off.mVoice;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), mVoice);
    }




    public abstract class OpcodeExtendedOpcode : OpcodeBase
    {
        public override byte TypeByte => 0xB0;
        public abstract byte ExtendedTypeByte { get; }

        public override bool Equals(object? obj) => obj is OpcodeExtendedOpcode ext && base.Equals(obj) && ExtendedTypeByte == ext.ExtendedTypeByte;
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ExtendedTypeByte);
    }



    public class OpcodeSetParamInt8 : OpcodeBase
    {
        public override byte TypeByte => 0xB8;
        public ParamTarget Target;
        public byte Value;

        public override bool Equals(object? obj) => obj is OpcodeSetParamInt8 @int &&
                   base.Equals(obj) &&
                   Target == @int.Target &&
                   Value == @int.Value;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Target, Value);
    }

    public class OpcodeSetParamInt16 : OpcodeBase
    {
        public override byte TypeByte => 0xB9;
        public ParamTarget Target;
        public short Value;

        public override bool Equals(object? obj) => obj is OpcodeSetParamInt16 @int &&
                   base.Equals(obj) &&
                   Target == @int.Target &&
                   Value == @int.Value;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Target, Value);
    }

    public class OpcodeSetParamInt8Time : OpcodeBase
    {
        public override byte TypeByte => 0xBA;
        public ParamTarget Target;
        public byte Value;
        public short Time;

        public override bool Equals(object? obj) => obj is OpcodeSetParamInt8Time int8 &&
                   base.Equals(obj) &&
                   Target == int8.Target &&
                   Value == int8.Value &&
                   Time == int8.Time;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Target, Value, Time);
    }

    public class OpcodeSetParamInt16Time : OpcodeBase
    {
        public override byte TypeByte => 0xBB;
        public ParamTarget Target;
        public short Value;
        public short Time;

        public override bool Equals(object? obj) => obj is OpcodeSetParamInt16Time int16 &&
                   base.Equals(obj) &&
                   Target == int16.Target &&
                   Value == int16.Value &&
                   Time == int16.Time;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Target, Value, Time);
    }




    public class OpcodeOpenTrack : OpcodeBase
    {
        public override byte TypeByte => 0xC1;
        public int Id;
    }

    public class OpcodeCloseTrack : OpcodeBase
    {
        public override byte TypeByte => 0xC2;
        public int Id;
    }

    public class OpcodeCall : OpcodeBase
    {
        public override byte TypeByte => 0xC3;
        public int SubroutineID; //Index into a BMS' subroutine listing
    }

    public class OpcodeReturn : OpcodeBase
    {
        public override byte TypeByte => 0xC5;
    }

    public class OpcodeJump : OpcodeBase
    {
        public override byte TypeByte => 0xC7;
        public OpcodeBase? TargetOpcode;
    }




    public class OpcodeSetArticulation : OpcodeBase
    {
        public override byte TypeByte => 0xD8;
        public ArticulationTarget Target = ArticulationTarget.PPQN; //Default to PPQN
        public ushort Value = 120; //Default to 120
    }



    public class OpcodeSetTempo : OpcodeBase
    {
        public override byte TypeByte => 0xE0;
        public ushort Value = 120; //Default to 120
    }

    public class OpcodeSetBankProgram : OpcodeBase
    {
        public override byte TypeByte => 0xE1;
        public byte BankNo;
        public byte ProgramNo;

        public override bool Equals(object? obj) => obj is OpcodeSetBankProgram program &&
                   base.Equals(obj) &&
                   BankNo == program.BankNo &&
                   ProgramNo == program.ProgramNo;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), BankNo, ProgramNo);
    }

    public class OpcodeSetBank : OpcodeBase
    {
        public override byte TypeByte => 0xE2;
        public byte Value;

        public override bool Equals(object? obj) => obj is OpcodeSetBank bank &&
                   base.Equals(obj) &&
                   Value == bank.Value;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);
    }
    
    public class OpcodeSetProgram : OpcodeBase
    {
        public override byte TypeByte => 0xE3;
        public byte Value;

        public override bool Equals(object? obj) => obj is OpcodeSetProgram program &&
                   base.Equals(obj) &&
                   Value == program.Value;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);
    }



    public class OpcodeWaitVar : OpcodeBase
    {
        public override byte TypeByte => 0xF0;
        private int mValue = 0;
        public int Value
        {
            get => mValue;
            set => mValue = value & 0x1FFFFFFF;
        }

        public override bool Equals(object? obj) => obj is OpcodeWaitVar var &&
                   base.Equals(obj) &&
                   mValue == var.mValue;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), mValue);
    }

    public class OpcodeWaitInt8 : OpcodeBase
    {
        public override byte TypeByte => 0xF1;
        private byte mValue = 0;
        public byte Value
        {
            get => mValue;
            set => mValue = value;
        }

        public override bool Equals(object? obj) => obj is OpcodeWaitInt8 @int &&
                   base.Equals(obj) &&
                   mValue == @int.mValue;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), mValue);
    }

    public class OpcodePrintF : OpcodeBase
    {
        public override byte TypeByte => 0xFD;
        public string? Value; //If null, will not be saved to the file.
    }

    public class OpcodeNop : OpcodeBase
    {
        public override byte TypeByte => 0xFE; //Literally does nothing lol.
    }
    public class OpcodeFinish : OpcodeBase
    {
        public override byte TypeByte => 0xFF;
    }

    // SMG/2 Extended opcodes
    public class OpcodeExtDump : OpcodeExtendedOpcode
    {
        public override byte ExtendedTypeByte => 0x01;
    }


    // TODO: Research these
    public enum ArticulationTarget : byte
    {
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 0 in the switch statement</remarks>
        UNKNOWN_00 = 0x40,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 1 in the switch statement</remarks>
        UNKNOWN_01 = 0x41,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 2 in the switch statement</remarks>
        UNKNOWN_02 = 0x42,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 3 in the switch statement</remarks>
        UNKNOWN_03 = 0x43,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 4 in the switch statement</remarks>
        UNKNOWN_04 = 0x44,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 5 in the switch statement</remarks>
        UNKNOWN_05 = 0x45,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 6 in the switch statement</remarks>
        UNKNOWN_06 = 0x46,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 7 in the switch statement</remarks>
        UNKNOWN_07 = 0x47,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 8 in the switch statement</remarks>
        UNKNOWN_08 = 0x48,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 9 in the switch statement</remarks>
        UNKNOWN_09 = 0x49,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 10 in the switch statement</remarks>
        UNKNOWN_10 = 0x4A,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 11 in the switch statement</remarks>
        UNKNOWN_11 = 0x4B,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 12 in the switch statement</remarks>
        UNKNOWN_12 = 0x4C,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 13 in the switch statement</remarks>
        UNKNOWN_13 = 0x4D,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 14 in the switch statement</remarks>
        UNKNOWN_14 = 0x4E,
        /// <summary>
        /// Unknown
        /// </summary>
        /// <remarks>Case 15 in the switch statement</remarks>
        UNKNOWN_15 = 0x4F,

        // Cases 16 - 33 are just straight up empty...

        /// <summary>
        /// Sets the Pulses per Quarter Note (Timebase)
        /// </summary>
        /// <remarks>Case 34 in the switch statement</remarks>
        PPQN = 0x62,
        /// <summary>
        /// Transposes notes up or down. Signed Byte.
        /// </summary>
        /// <remarks>Case 35 in the switch statement</remarks>
        TRANSPOSE = 0x63,
        /// <summary>
        /// Unknown. Unable to determine an effect
        /// </summary>
        /// <remarks>Case 36 in the switch statement</remarks>
        UNKNOWN_36 = 0x64,
        /// <summary>
        /// Unknown. Unable to test - Related to Note Gates
        /// </summary>
        /// <remarks>Case 37 in the switch statement</remarks>
        UNKNOWN_37 = 0x65,
        /// <summary>
        /// Unknown. Maybe related to track children somehow?
        /// </summary>
        /// <remarks>Case 38 in the switch statement</remarks>
        UNKNOWN_38 = 0x66,
        /// <summary>
        /// Unknown. Maybe related to track children somehow?
        /// </summary>
        /// <remarks>Case 39 in the switch statement</remarks>
        UNKNOWN_39 = 0x67,
        /// <summary>
        /// Unknown. Maybe related to track children somehow?
        /// </summary>
        /// <remarks>Case 40 in the switch statement</remarks>
        UNKNOWN_40 = 0x68,
        /// <summary>
        /// Unknown. Seems to have no effect, but is a full range unsigned short
        /// </summary>
        /// <remarks>Case 41 in the switch statement</remarks>
        UNKNOWN_41 = 0x69,
        /// <summary>
        /// Unknown. Seems to set 2 values at once? splits the short in half seemingly.
        /// </summary>
        /// <remarks>Case 42 in the switch statement</remarks>
        UNKNOWN_42 = 0x6A,
        /// <summary>
        /// Unknown. Sets the second value of 41
        /// </summary>
        /// <remarks>Case 43 in the switch statement</remarks>
        UNKNOWN_43 = 0x6B,
        /// <summary>
        /// Unknown. Sets the second value of 41
        /// </summary>
        /// <remarks>Case 44 in the switch statement</remarks>
        UNKNOWN_44 = 0x6C,
        /// <summary>
        /// Unknown. Oscillator related maybe? Does not seem to be read...
        /// </summary>
        /// <remarks>Case 45 in the switch statement</remarks>
        UNKNOWN_45 = 0x6D,
        /// <summary>
        /// Sets the Vibrato effect strength.<para/>Divided by 1524
        /// </summary>
        /// <remarks>Case 46 in the switch statement</remarks>
        VIBRATO_DEPTH = 0x6E,
        /// <summary>
        /// Sets the Vibrato effect strength.<para/>Divided by 12192
        /// </summary>
        /// <remarks>Case 47 in the switch statement</remarks>
        VIBRATO_DEPTH_ALT = 0x6F,
        /// <summary>
        /// Sets the Tremolo effect strength.<para/>Multiplied by 0.00390625
        /// </summary>
        /// <remarks>Case 48 in the switch statement</remarks>
        TREMOLO_STRENGTH = 0x70,
        /// <summary>
        /// Changes the vibrato effect rate (how quickly the wobble occurs)<para/>Multiplied by 0.015625
        /// </summary>
        /// <remarks>Case 49 in the switch statement</remarks>
        VIBRATO_RATE = 0x71,
        /// <summary>
        /// Sets the Tremolo effect rate.<para/>Multiplied by 0.015625
        /// </summary>
        /// <remarks>Case 50 in the switch statement</remarks>
        TREMOLO_RATE = 0x72,
        /// <summary>
        /// Unknown. Possibly not used
        /// </summary>
        /// <remarks>Case 51 in the switch statement</remarks>
        UNKNOWN_51 = 0x73,
        /// <summary>
        /// Unknown. Possibly not used
        /// </summary>
        /// <remarks>Case 52 in the switch statement</remarks>
        UNKNOWN_52 = 0x74
    }

    public enum ParamTarget : byte
    {
        VOLUME = 0,
        PITCH = 1,
        REVERB = 2,
        PANNING = 3
            
        // I thought there would be more, but seemingly not.
    }
}
