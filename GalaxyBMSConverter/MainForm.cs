using Hack.io.BMS;
using Hack.io.CIT;
using Hack.io.MIDI;
using Hack.io.Utility;
using Hack.io.YAZ0;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace GalaxyBMSConverter;

public partial class MainForm : Form
{
    private readonly OpenFileDialog CSVOFD = new() { Filter = "Comma Separated Values|*.csv|All Files|*.*" };
    private readonly OpenFileDialog MIDIOFD = new() { Filter = "Musical Instrument Digital Interface|*.mid|All Files|*.*" };

    private readonly SaveFileDialog CSVSFD = new() { Filter = "Comma Separated Values|*.csv|All Files|*.*" };
    private readonly SaveFileDialog BMSSFD = new() { Filter = "Nintendo JAudio2 BMS|*.bms" };

    private readonly ComboBox[] ChannelToTrack;
    private readonly string[] TrackText;
    private readonly BindingList<string>[] TrackBindinglist;

    private readonly OptimizeForm OPForm = new();

    public MainForm()
    {
        InitializeComponent();
        CenterToScreen();

        ChannelToTrack = [
            MidiChannel00ComboBox,
            MidiChannel01ComboBox,
            MidiChannel02ComboBox,
            MidiChannel03ComboBox,
            MidiChannel04ComboBox,
            MidiChannel05ComboBox,
            MidiChannel06ComboBox,
            MidiChannel07ComboBox,
            MidiChannel08ComboBox,
            MidiChannel09ComboBox,
            MidiChannel10ComboBox,
            MidiChannel11ComboBox,
            MidiChannel12ComboBox,
            MidiChannel13ComboBox,
            MidiChannel14ComboBox,
            MidiChannel15ComboBox,
        ];

        TrackText = new string[ChannelToTrack.Length + 1];

        for (int i = 0; i < ChannelToTrack.Length; i++)
            TrackText[i] = $"BMS Track {i:00}";
        TrackText[ChannelToTrack.Length] = "<Do Not Convert>";

        TrackBindinglist = new BindingList<string>[ChannelToTrack.Length];

        for (int i = 0; i < ChannelToTrack.Length; i++)
        {
            TrackBindinglist[i] = new(TrackText);
            BindingSource bSource = new()
            {
                DataSource = TrackBindinglist[i]
            };

            ChannelToTrack[i].DataSource = bSource;
            ChannelToTrack[i].SelectedIndex = i;
        }

        TimingComboBox.SelectedIndex = 0;
    }

    void RebindMidiChannelComboBox()
    {
        SuspendLayout();
        for (int i = 0; i < ChannelToTrack.Length; i++)
        {
            TrackBindinglist[i].RaiseListChangedEvents = true;
            for (int j = 0; j < ChannelToTrack.Length; j++)
                TrackBindinglist[i].ResetItem(j);
        }
        ResumeLayout();


    }

    private void InstrumentListLabelTextBoxButtonControl_ButtonClick(object sender, EventArgs e)
    {
        if (CSVOFD.ShowDialog() != DialogResult.OK)
            return;
        InstrumentListLabelTextBoxButtonControl.Text = CSVOFD.FileName;
    }

    private void LookupLabelTextBoxButtonControl_ButtonClick(object sender, EventArgs e)
    {
        if (CSVOFD.ShowDialog() != DialogResult.OK)
            return;
        LookupLabelTextBoxButtonControl.Text = CSVOFD.FileName;
    }

    private void MidiLabelTextBoxButtonControl_ButtonClick(object sender, EventArgs e)
    {
        if (MIDIOFD.ShowDialog() != DialogResult.OK)
            return;
        MidiLabelTextBoxButtonControl.Text = MIDIOFD.FileName;
        FileInfo fi = new(MIDIOFD.FileName);
        BMSSFD.FileName = Path.Combine(fi.DirectoryName ?? "", fi.Name.Replace(".mid", "_midi_cnv.bms"));
    }

    private void TimingComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selected = TimingComboBox.SelectedIndex - 1;
        for (int i = 0; i < ChannelToTrack.Length; i++)
            TrackText[i] = i == selected ? "(Timing Track)" : $"BMS Track {i:00}";
        TrackText[ChannelToTrack.Length] = "<Do Not Convert>";

        RebindMidiChannelComboBox();

        if (TimingComboBox.SelectedIndex < 1)
        {
            ChordDataGridView.Enabled = false;
            return;
        }
        ChordDataGridView.Enabled = true;
    }

    private void LoadChordCsvButton_Click(object sender, EventArgs e)
    {
        if (ChordDataGridView.Rows.Count == 0 && MessageBox.Show("Loading a chord CSV will erase the data currently in the table.\nProceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.OK)
            return;

        if (CSVOFD.ShowDialog() != DialogResult.OK)
            return;

        string[] lines = File.ReadAllLines(CSVOFD.FileName);

        if (lines.Length <= 1) // Just one row would mean we only have the header, which isn't good.
            return;

        int TickIndex = -1;
        int BassIndex = -1;
        int ChordIndex = -1;
        int ScaleUpIndex = -1;
        int ScaleDownIndex = -1;
        int BPMOverrideIndex = -1;
        string[] HeaderSplit = lines[0].Split(',');
        if (HeaderSplit.Length < 5)
            goto Failure;

        // After writing this, I realized I probably should've just used IndexOf..... maybe this is faster somehow (doubt)
        for (int i = 0; i < HeaderSplit.Length; i++)
        {
            if (HeaderSplit[i].Equals("Tick"))
                TickIndex = i;
            else if (HeaderSplit[i].Equals("Bass"))
                BassIndex = i;
            else if (HeaderSplit[i].Equals("Chord"))
                ChordIndex = i;
            else if (HeaderSplit[i].Equals("ScaleUp"))
                ScaleUpIndex = i;
            else if (HeaderSplit[i].Equals("ScaleDown"))
                ScaleDownIndex = i;
            else if (HeaderSplit[i].Equals("BPMOverride"))
                BPMOverrideIndex = i;
        }

        if (TickIndex == -1 || BassIndex == -1 || ChordIndex == -1 || ScaleUpIndex == -1 || ScaleDownIndex == -1)
            goto Failure;

        ChordDataGridView.Rows.Clear();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            string ovr = values.Length < 6 ? "" : values[BPMOverrideIndex];
            ChordDataGridView.Rows.Add(values[TickIndex], values[BassIndex], values[ChordIndex], values[ScaleUpIndex], values[ScaleDownIndex], ovr);
        }

        return;

    // You know, spaghetti and meatballs is pretty tasty.
    Failure:
        MessageBox.Show("Failed to read the Chord CSV", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void SaveChordCsvButton_Click(object sender, EventArgs e)
    {
        if (CSVSFD.ShowDialog() != DialogResult.OK)
            return;

        StringBuilder SB = new();
        SB.AppendLine("Tick,Bass,Chord,ScaleUp,ScaleDown,BPMOverride");
        foreach (DataGridViewRow row in ChordDataGridView.Rows)
        {
            string? TickNum = row.Cells[0].Value?.ToString();
            string? BassNote = row.Cells[1].Value?.ToString();
            string? ChordNotes = row.Cells[2].Value?.ToString();
            string? ScaleUpNotes = row.Cells[3].Value?.ToString();
            string? ScaleDownNotes = row.Cells[4].Value?.ToString();
            string? BPMOverride = row.Cells[5].Value?.ToString();
            SB.AppendLine($"{TickNum},{BassNote},{ChordNotes},{ScaleUpNotes},{ScaleDownNotes},{BPMOverride}");
        }
        File.WriteAllText(CSVSFD.FileName, SB.ToString());
    }

    private void OptimizationMenuButton_Click(object sender, EventArgs e)
    {
        OPForm.SaveCurrent();
        OPForm.Recenter();
        if (OPForm.ShowDialog() != DialogResult.OK)
            OPForm.Cancel();
    }

    private void ConvertButton_Click(object sender, EventArgs e)
    {
        bool IsNoMidi = MidiLabelTextBoxButtonControl.Text.Equals("_");

        if (!IsNoMidi && !File.Exists(MidiLabelTextBoxButtonControl.Text))
        {
            MessageBox.Show("No Midi File selected!\nIf you just want to generate chords, type '_' as the Midi path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!IsNoMidi && !File.Exists(InstrumentListLabelTextBoxButtonControl.Text))
        {
            MessageBox.Show("No Instrument List selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!IsNoMidi && !File.Exists(LookupLabelTextBoxButtonControl.Text))
        {
            MessageBox.Show("No Lookup List selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }


        if (IsNoMidi && TimingComboBox.SelectedIndex <= 0)
        {
            MessageBox.Show("In order to generate only a timing track, you need to assign a BMS Track to use.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (BMSSFD.ShowDialog() != DialogResult.OK)
            return;

        Dictionary<string, long> WarningList = [];
        void AddWarning(string msg)
        {
            if (!WarningList.ContainsKey(msg))
                WarningList.Add(msg, 0);
            WarningList[msg]++;
        }

        // Load the instrument data
        List<InstrumentInfoTag> InstrumentInfo = [];
        List<MIDIRemapEntry> RemapInfo = [];

        if (!IsNoMidi)
        {
            string[] Lines = File.ReadAllLines(InstrumentListLabelTextBoxButtonControl.Text);
            for (int i = 1; i < Lines.Length; i++) // Ignore the first row since it's just the header definitions
            {
                string[] splitter = Lines[i].Split(',');
                InstrumentInfoTag t = new()
                {
                    Name = splitter[0],
                    BankProg = (byte.Parse(splitter[1], System.Globalization.NumberStyles.HexNumber), byte.Parse(splitter[2], System.Globalization.NumberStyles.HexNumber)),
                    IsPercussion = !string.IsNullOrWhiteSpace(splitter[3])
                };
                InstrumentInfo.Add(t);
            }

            Lines = File.ReadAllLines(LookupLabelTextBoxButtonControl.Text);
            for (int i = 1; i < Lines.Length; i++) // Ignore the first row since it's just the header definitions
            {
                string[] splitter = Lines[i].Split(',');
                MIDIRemapEntry r = new();
                int instid = -1;
                if (!int.TryParse(splitter[0], out instid))
                    instid = 128; //Percussion
                r.ID = instid;
                if (byte.TryParse(splitter[2], out byte midikey))
                    r.MidiKey = midikey;
                r.BMSName = splitter[3];
                if (byte.TryParse(splitter[4], out byte bmskey))
                    r.BMSKey = bmskey;

                RemapInfo.Add(r);
            }
        }

        // Create & Save BMS
        MIDI CurrentConversionTarget = [];
        StreamUtil.SetEndianBig();
        if (!MidiLabelTextBoxButtonControl.Text.Equals("_"))
            FileUtil.LoadFile(MidiLabelTextBoxButtonControl.Text, CurrentConversionTarget.Load);
        int PPQN = CurrentConversionTarget.TimeBase; // Defaults to 120, so only generating a timing track will work just fine

        int LoopStart = (int)(LoopStartTickNumericUpDown.Value < 0 ? 0 : LoopStartTickNumericUpDown.Value);
        int LoopEnd = (int)(LoopEndTickNumericUpDown.Value < 0 ? CurrentConversionTarget.Length : LoopEndTickNumericUpDown.Value);

        bool NoLooping = LoopStart == 0 && LoopEnd == 0;
        bool IsGenerateChords = ChordDataGridView.Enabled;

        // Optimizations
        bool IsNormalizePPQN = IsOptimizeOn(OptimizeForm.OptPPQNTo120);
        bool IsCombineBankProg = IsOptimizeOn(OptimizeForm.OptCombineBankProg);
        bool IsGroupLinCC = false;// IsOptimizeOn(OptimizeForm.OptGroupLinearControlChange);
        bool IsOptimizeBMS = false;// IsOptimizeOn(OptimizeForm.OptBMSRepeatOpcodes);
        bool IsWideOptimizeBMS = false;// IsOptimizeOn(OptimizeForm.OptBMSMultistack) && IsOptimizeBMS; // Both must be enabled for this to work
        bool IsSmallCIT = IsOptimizeOn(OptimizeForm.OptSmallCIT);

        bool IsOptimizeOn(string name) => OPForm.GetOptimizationEnabled(name) ?? throw new MissingMemberException($"Optimization name {name} could not be found");
        // -------------

        if (!NoLooping && LoopStart >= LoopEnd)
        {
            MessageBox.Show("The Loop Start must be less than the Loop End.\nAlternatively, you can set both to 0 to disable looping, or set both to -1 to use the start and end as looping points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        if (NoLooping)
            LoopEnd = CurrentConversionTarget.Length;
        else
        {
            // Warnings for if the loop points are off measure
            int PPQNx4 = PPQN * 4;
            if (LoopStart % PPQNx4 != 0)
                AddWarning("The Loop Start position does not align to a Measure");
            if (LoopEnd % PPQNx4 != 0)
                AddWarning("The Loop End position does not align to a Measure");
        }

        MIDI.Sequence[] AllChannels = new MIDI.Sequence[16];
        Dictionary<int, List<MIDI.Event>>[] AllEventsByChannel = new Dictionary<int, List<MIDI.Event>>[AllChannels.Length];
        MIDI.Sequence TempoChannel = [];

        for (int i = 0; i < AllChannels.Length; i++)
        {
            AllChannels[i] = [];
            AllEventsByChannel[i] = [];
        }

        foreach (var seq in CurrentConversionTarget)
        {
            var absseq = seq.AbsoluteEvents;
            foreach (var evt in absseq)
            {
                int ChannelID = evt.Message.Channel;
                int Position = ChangeTickPPQN(evt.Position);

                Dictionary<int, List<MIDI.Event>> Current = AllEventsByChannel[ChannelID];
                if (!Current.ContainsKey(Position))
                    Current.Add(Position, []);
                Current[Position].Add(new MIDI.Event(Position, evt.Message));
            }
        }
        for (int i = 0; i < AllChannels.Length; i++)
        {
            Dictionary<int, List<MIDI.Event>> Current = AllEventsByChannel[i];
            //Sort keys
            List<int> Keys = [.. Current.Keys];
            Keys.Sort();
            for (int k = 0; k < Keys.Count; k++)
            {
                List<MIDI.Event> CurrentEvents = Current[Keys[k]];
                for (int x = 0; x < CurrentEvents.Count; x++)
                {
                    var evt = CurrentEvents[x];
                    if (evt.Message is MIDI.MessageMetaTempo)
                    {
                        TempoChannel.Add(evt);
                        continue;
                    }
                    if (evt.Message.Status != 0xFF || evt.Message is MIDI.MessageMetaMarker)
                        AllChannels[i].Add(evt);
                }
            }
        }

        for (int i = 0; i < AllChannels.Length; i++)
            if (AllChannels[i].Count != 0 && IsGenerateChords)
            {
                //Check that the user isn't overwriting the timing track
                int CurDestBMSTrack = ChannelToTrack[i].SelectedIndex;
                int CurBMSChordTrack = TimingComboBox.SelectedIndex - 1;
                if (CurBMSChordTrack == CurDestBMSTrack)
                {
                    MessageBox.Show($"MIDI Channel {i} cannot be mapped to BMS Track {CurBMSChordTrack:00} because BMS Track {CurBMSChordTrack:00} is occupied by the Chord Generator\n\nPlease change its target BMS track.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        // Allow the chord table to override the BPM
        List<(int Tick, int BPM)> AllBPMOverride = [];
        foreach (DataGridViewRow row in ChordDataGridView.Rows)
        {
            string? TickNum = row.Cells[0].Value?.ToString();
            string? Override = row.Cells[5].Value?.ToString();
            if (!int.TryParse(TickNum, out int RealTickNum) || !int.TryParse(Override, out int RealOverride))
                continue;
            AllBPMOverride.Add((RealTickNum, RealOverride));
        }
        if (AllBPMOverride.Count > 0 || IsNoMidi)
        {
            AllBPMOverride = [.. AllBPMOverride.OrderBy(o => o.Tick)];
            if (!IsNoMidi)
                AddWarning("The MIDI's Tempo Changes were overridden.");
            TempoChannel.Clear();
            bool IsFoundZeroTick = false;
            foreach ((int Tick, int BPM) in AllBPMOverride)
            {
                if (Tick == 0)
                    IsFoundZeroTick = true;
                MIDI.MessageMetaTempo m = new((double)BPM);
                TempoChannel.Add(new MIDI.Event(ChangeTickPPQN(Tick), m));
            }
            if (!IsFoundZeroTick)
            {
                MessageBox.Show("Tick 0 is missing a BPM Override entry.\nAdd one for Tick 0 and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        LoopStart = ChangeTickPPQN(LoopStart);
        LoopEnd = ChangeTickPPQN(LoopEnd);
        PPQN = ChangeTickPPQN(CurrentConversionTarget.TimeBase); // Magically, the math works out so that this will be 120

        // The Initial result BMS file
        BMS InitialResult = [], FinalResult = [];

        // Galaxy BMS are setup like this:
        // - ROOT
        //  - Parent
        //   - Track 00
        //   - Track 01
        //   - Track 02
        //   - Track 03
        //   - Track 04
        //   - Track 05
        //   - Track 06
        //   - Track 07
        //   - Track 08
        //   - Track 09
        //   - Track 10
        //   - Track 11
        //   - Track 12
        //   - Track 13
        //   - Track 14
        //   - Track 15

        BMS.Track ROOT = [];
        CreateBMSRoot(ROOT);

        BMS.Track PARENT = [];
        CreateBMSParent(PARENT);

        ROOT.Children.Add((0, PARENT));

        for (int i = 0; i < ChannelToTrack.Length; i++)
        {
            if (AllChannels[i].Count == 0 || ChannelToTrack[i].SelectedIndex >= AllChannels.Length)
                continue;
            BMS.Track CHILD = [];
            GenerateTrack(AllChannels[i], CHILD);
            PARENT.Children.Add((ChannelToTrack[i].SelectedIndex, CHILD));
        }

        // Create and save the CIT
        if (IsGenerateChords)
            GenerateCIT();

        InitialResult.Add((0, ROOT));

        // Optimize the BMS
        FinalResult = OptimizeBMS(InitialResult);

        FileUtil.SaveFile(BMSSFD.FileName, FinalResult.Save);
        byte[] NotEncoded = File.ReadAllBytes(BMSSFD.FileName);
        long MaxSeqDataSize = 56000;
        bool IsOversized = NotEncoded.Length > MaxSeqDataSize;
        if (IsOversized)
            AddWarning($"The BMS is too large ({NotEncoded.Length} > {MaxSeqDataSize})");
        else
        {
            byte[] comp = YAZ0.Compress(NotEncoded);
            File.WriteAllBytes(BMSSFD.FileName.Replace(".bms", ".szs"), comp);
        }

        if (WarningList.Count == 0)
            MessageBox.Show("Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
        {
            StringBuilder WarningBuilder = new();
            WarningBuilder.AppendLine("The Conversion finished, but warnings were thrown.");
            WarningBuilder.AppendLine("The output file may or may not work as intended.");
            WarningBuilder.AppendLine();
            foreach (var item in WarningList)
                WarningBuilder.AppendLine($"{item.Key} (x{item.Value})");
            MessageBox.Show(WarningBuilder.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ==================== Locals

        void CreateBMSRoot(BMS.Track Dest)
        {
            Dest.Add(new BMS.OpcodeOpenTrack() { Id = 0 });
            Dest.Add(new BMS.OpcodeSetArticulation() { Value = (ushort)PPQN });

            GenerateTrack(TempoChannel, Dest);
        }
        void CreateBMSParent(BMS.Track Dest)
        {
            for (int i = 0; i < ChannelToTrack.Length; i++)
            {
                if (IsGenerateChords && i == (TimingComboBox.SelectedIndex - 1))
                {
                    Dest.Add(new BMS.OpcodeOpenTrack() { Id = TimingComboBox.SelectedIndex - 1 });
                }

                if (AllChannels[i].Count == 0 || ChannelToTrack[i].SelectedIndex >= AllChannels.Length)
                    continue;
                Dest.Add(new BMS.OpcodeOpenTrack() { Id = ChannelToTrack[i].SelectedIndex });
            }
            GenerateTrack([], Dest);
        }

        void GenerateTrack(MIDI.Sequence Source, BMS.Track Dest)
        {
            //Debug
            List<byte> UsedPercussionNotes = [];

            var absolute = Source;
            byte?[] VoiceData = new byte?[7];
            byte[] KeyPressure = CollectionUtil.InitilizeArray<byte>(127, 0x80);
            int TargetLoopOpcodeIndex = Dest.Count;
            int CurrentTime = 0;
            int CurrentChannel = 0;
            int CurrentPatch = -1;
            byte? LastBank = null, LastProgram = null;
            byte CurrentVolume = 127, CurrentExpression = 127;
            bool HasProcessedLoop = LoopStart == 0 || NoLooping;
            double LastUsedTempo = 0;

            foreach (MIDI.Event item in absolute)
            {
                if (!HasProcessedLoop && item.Position >= LoopStart)
                {
                    CreateWaitOpcode(LoopStart - CurrentTime);
                    CurrentTime = LoopStart;
                    TargetLoopOpcodeIndex = Dest.Count;
                    HasProcessedLoop = true;
                }

                if (item.Position >= LoopEnd)
                    break;

                if (item.Position > CurrentTime)
                {
                    // Proceed to the next time first
                    CreateWaitOpcode(item.Position - CurrentTime);
                    CurrentTime = item.Position;
                }


                if (item.Message is MIDI.MessageNoteOn mON)
                {
                    if (mON.Velocity > 0)
                        WriteNoteOn(mON.NoteId, mON.Velocity);
                }
                else if (item.Message is MIDI.MessageNoteOff mOFF)
                {
                    WriteNoteOff(mOFF.NoteId);
                }
                else if (item.Message is MIDI.MessageMetaTempo mMetaTempo)
                {
                    if (LastUsedTempo != mMetaTempo.Tempo)
                    {
                        Dest.Add(new BMS.OpcodeSetTempo() { Value = (ushort)Math.Round(mMetaTempo.Tempo, 0) });
                        LastUsedTempo = mMetaTempo.Tempo;
                    }
                }
                else if (item.Message is MIDI.MessagePatchChange mPatchChange)
                {
                    CurrentChannel = mPatchChange.Channel;

                    byte p = mPatchChange.PatchId;
                    if (CurrentChannel == 9)
                        p = 128;
                    WriteProgramChange(p);
                }
                else if (item.Message is MIDI.MessageChannelPitch mChannelPitch)
                {
                    var p = mChannelPitch.Pitch - 0x2000;
                    Dest.Add(new BMS.OpcodeSetParamInt16() { Target = BMS.ParamTarget.PITCH, Value = (short)p });
                }
                else if (item.Message is MIDI.MessageControlChange mControllerChange)
                {
                    if (mControllerChange.ControlId == 1) // Modulation (We are using for Vibrato Depth)
                    {
                        Dest.Add(new BMS.OpcodeSetArticulation() { Target = BMS.ArticulationTarget.VIBRATO_DEPTH, Value = mControllerChange.Value });
                    }
                    else if (mControllerChange.ControlId == 2) // Breath Control (We are using for Vibrato Rate)
                    {
                        Dest.Add(new BMS.OpcodeSetArticulation() { Target = BMS.ArticulationTarget.VIBRATO_RATE, Value = mControllerChange.Value });
                    }
                    if (mControllerChange.ControlId == 7 || mControllerChange.ControlId == 11) // Volume & Expression. Apparently Expression is a % of the volume.
                    {
                        if (mControllerChange.ControlId == 7)
                            CurrentVolume = mControllerChange.Value;
                        else
                            CurrentExpression = mControllerChange.Value;

                        // Calculate volume wowie
                        float CurVol = CurrentVolume / 127f;
                        float CurExp = CurrentExpression / 127f;
                        float FinalVol = CurVol * CurExp;

                        Dest.Add(new BMS.OpcodeSetParamInt8() { Target = BMS.ParamTarget.VOLUME, Value = (byte)(FinalVol * 127) });
                    }
                    else if (mControllerChange.ControlId == 10) // Panning
                    {
                        Dest.Add(new BMS.OpcodeSetParamInt8() { Target = BMS.ParamTarget.PANNING, Value = mControllerChange.Value });
                    }
                    else if (mControllerChange.ControlId == 91) // Effects Channel 1 (We are using for Reverb)
                    {
                        Dest.Add(new BMS.OpcodeSetParamInt8() { Target = BMS.ParamTarget.REVERB, Value = mControllerChange.Value });
                    }
                    else if (mControllerChange.ControlId == 92) // Effects Channel 2 (We are using for Tremolo Strength)
                    {
                        Dest.Add(new BMS.OpcodeSetArticulation() { Target = BMS.ArticulationTarget.TREMOLO_STRENGTH, Value = mControllerChange.Value });
                    }
                    else if (mControllerChange.ControlId == 93) // Effects Channel 3 (We are using for Tremolo Rate)
                    {
                        Dest.Add(new BMS.OpcodeSetArticulation() { Target = BMS.ArticulationTarget.TREMOLO_RATE, Value = mControllerChange.Value });
                    }
                    else if (mControllerChange.ControlId == 94) // Effects Channel 4 (We are using for Quick Transpose)
                    {
                        Dest.Add(new BMS.OpcodeSetArticulation() { Target = BMS.ArticulationTarget.TRANSPOSE, Value = mControllerChange.Value });
                    }
                }
                else if (item.Message is MIDI.MessageMetaMarker mMarker)
                {
                    string msg = mMarker.Text;
                    string id = "OSReport::";
                    string dmp = "ExtDump";
                    if (msg.Length < 126 + id.Length && msg.StartsWith(id))
                    {
                        Dest.Add(new BMS.OpcodePrintF() { Value = msg[id.Length..] + "\n" }); //All OSReport calls need \n at the end... right?
                        AddWarning("OSReport messages need a memory patch in order to be seen.");
                    }
                    else if (msg.Equals(dmp))
                    {
                        Dest.Add(new BMS.OpcodeExtDump());
                        AddWarning("Debug Dump needs a memory patch in order to be seen.");
                    }
                }
                //else if (item.Message is MIDI.MessageKeyPressure mKeyPressure)
                //{
                //    KeyPressure[mKeyPressure.NoteId] = mKeyPressure.Pressure;
                //}
                else
                {
                    // Unsupported bruh :skull:
                }
            }

            if (!HasProcessedLoop) //If the loop hasn't been processed by this point.... huh???
            {
                CreateWaitOpcode(LoopStart - CurrentTime);
                CurrentTime = LoopStart;
                TargetLoopOpcodeIndex = Dest.Count;
                HasProcessedLoop = true;
            }

            CreateWaitOpcode(LoopEnd - CurrentTime);
            CurrentTime = LoopEnd;


            if (LoopCloseVoiceCheckBox.Checked)
                for (int i = 0; i < VoiceData.Length; i++)
                {
                    if (VoiceData[i] is null)
                        continue;
                    Dest.Add(new BMS.OpcodeNoteOff() { Voice = (byte)(i + 1) });
                    VoiceData[i] = null; //Probably not needed...
                }

            if (!NoLooping)
                Dest.Add(new BMS.OpcodeJump() { TargetOpcode = Dest[TargetLoopOpcodeIndex] });

            Dest.Add(new BMS.OpcodeFinish()); // Add a finisher.

            for (int i = 0; i < UsedPercussionNotes.Count; i++)
            {
                Debug.WriteLine($"Perc Note: {UsedPercussionNotes[i]:00}");
            }

            void CreateWaitOpcode(int delay) => Dest.Add(new BMS.OpcodeWaitVar() { Value = delay });
            void WriteNoteOn(byte note, byte velocity)
            {
                //Get a free voice slot
                int i = 0;
                for (; i < VoiceData.Length; i++)
                {
                    if (VoiceData[i] is null)
                        goto JumpLoc;
                }

                AddWarning("Failed to find open voice");
                return;

            JumpLoc:
                if (CurrentPatch < 0)
                    AddWarning("Note On was called before Patch Change");

                VoiceData[i] = note;

                bool isPerc = false;
                string? BMSName = null;
                for (int x = 0; x < RemapInfo.Count; x++)
                {
                    if (RemapInfo[x].ID == CurrentPatch)
                    {
                        BMSName = RemapInfo[x].BMSName;
                        break;
                    }
                }
                if (BMSName is not null)
                    for (int j = 0; j < InstrumentInfo.Count; j++)
                    {
                        if (InstrumentInfo[j].Name.Equals(BMSName) && InstrumentInfo[j].IsPercussion)
                        {
                            isPerc = true;
                            break;
                        }
                    }

                if (isPerc) //Percussion moment
                {
                    if (!UsedPercussionNotes.Contains(note))
                        UsedPercussionNotes.Add(note);
                    WriteProgramChange(128, note);
                    for (int x = 0; x < RemapInfo.Count; x++)
                    {
                        if (RemapInfo[x].ID == 128 && RemapInfo[x].MidiKey == note && RemapInfo[x].BMSKey is byte b)
                        {
                            note = b;
                            goto SuccessPerc;
                        }
                    }

                    //Failed to find suitable percussion note. Cannot write note
                    AddWarning("Failed to remap Percussion note");
                    VoiceData[i] = null; //Free the voice
                    return;
                SuccessPerc:;
                }

                Dest.Add(new BMS.OpcodeNoteOn() { Note = note, Voice = (byte)(i + 1), Velocity = velocity });
            }
            void WriteNoteOff(byte note)
            {
                int i = 0;
                for (; i < VoiceData.Length; i++)
                {
                    if (VoiceData[i] is null)
                        continue;
                    if (VoiceData[i] == note)
                        goto JumpLoc;
                }
                AddWarning("Failed to find voice to close");
                return;
            JumpLoc:
                Dest.Add(new BMS.OpcodeNoteOff() { Voice = (byte)(i + 1) });
                VoiceData[i] = null;
            }
            void WriteProgramChange(byte patch, byte? note = null)
            {
                CurrentPatch = patch;
                (byte BankNo, byte ProgramNo) BankProg;

                string? BMSName = null;
                for (int i = 0; i < RemapInfo.Count; i++)
                {
                    if (note is null)
                    {
                        if (RemapInfo[i].ID == patch)
                        {
                            BMSName = RemapInfo[i].BMSName;
                            break;
                        }
                    }
                    else
                    {
                        if (RemapInfo[i].ID == patch && RemapInfo[i].MidiKey == note && RemapInfo[i].BMSKey is byte b)
                        {
                            BMSName = RemapInfo[i].BMSName;
                            break;
                        }
                    }
                }
                if (BMSName is not null)
                    for (int i = 0; i < InstrumentInfo.Count; i++)
                    {
                        if (InstrumentInfo[i].Name.Equals(BMSName))
                        {
                            BankProg = InstrumentInfo[i].BankProg;
                            goto applyProgramChange;
                        }
                    }

                BankProg = (0, 7); //Default to Piano
                AddWarning("Failed to map a Bank Prog change");

            applyProgramChange:

                if (IsCombineBankProg && BankProg.BankNo != LastBank && BankProg.ProgramNo != LastProgram)
                    Dest.Add(new BMS.OpcodeSetBankProgram() { BankNo = BankProg.BankNo, ProgramNo = BankProg.ProgramNo });
                else
                {
                    if (BankProg.BankNo != LastBank)
                        Dest.Add(new BMS.OpcodeSetBank() { Value = BankProg.BankNo });
                    if (BankProg.ProgramNo != LastProgram)
                        Dest.Add(new BMS.OpcodeSetProgram() { Value = BankProg.ProgramNo });
                }
                LastBank = BankProg.BankNo;
                LastProgram = BankProg.ProgramNo;
            }
        }

        int ChangeTickPPQN(int curpos) => !IsNormalizePPQN ? curpos : (int)((curpos / (float)CurrentConversionTarget.TimeBase) * 120f);



        void GenerateCIT()
        {
            // Lets gather the chords the user wanted
            CIT ChordInfoTable = new();
            List<(int Tick, int ChordID, int ScaleID)> Assignments = [];

            foreach (DataGridViewRow row in ChordDataGridView.Rows)
            {
                string? TickNum = row.Cells[0].Value?.ToString();
                string? BassNote = row.Cells[1].Value?.ToString();
                string? ChordNotes = row.Cells[2].Value?.ToString();
                string? ScaleUpNotes = row.Cells[3].Value?.ToString();
                string? ScaleDownNotes = row.Cells[4].Value?.ToString();

                if (!int.TryParse(TickNum, out int RealTickNum) || BassNote is null || ChordNotes is null || ScaleUpNotes is null || ScaleDownNotes is null)
                    continue;

                RealTickNum = ChangeTickPPQN(RealTickNum);

                string[] ChordSplit = ChordNotes.Split();
                CIT.Note[] RealChords = new CIT.Note[7];
                for (int i = 0; i < RealChords.Length; i++)
                {
                    if (i >= ChordSplit.Length)
                        RealChords[i] = CIT.Note.NONE;
                    else
                        RealChords[i] = CIT.NoteFromString(ChordSplit[i]);
                }

                CIT.Chord newChord = new(CIT.NoteFromString(BassNote), RealChords[0], RealChords[1], RealChords[2], RealChords[3], RealChords[4], RealChords[5], RealChords[6]);
                int ChordID = IsSmallCIT ? 0 : ChordInfoTable.Chords.Count;
                for (; ChordID < ChordInfoTable.Chords.Count; ChordID++)
                {
                    if (ChordInfoTable.Chords[ChordID].Equals(newChord))
                        break;
                }
                if (ChordID == ChordInfoTable.Chords.Count)
                    ChordInfoTable.Chords.Add(newChord);

                //-------------------------------

                string[] ScaleUpSplit = ScaleUpNotes.Split();
                string[] ScaleDownSplit = ScaleDownNotes.Split();
                int twelve = 12; //lol
                CIT.Note[] RealScaleUp = new CIT.Note[twelve];
                CIT.Note[] RealScaleDown = new CIT.Note[twelve];

                for (int i = 0; i < twelve; i++)
                {
                    if (i >= ScaleUpSplit.Length)
                        RealScaleUp[i] = CIT.Note.NONE;
                    else
                        RealScaleUp[i] = CIT.NoteFromString(ScaleUpSplit[i]);

                    if (i >= ScaleDownSplit.Length)
                        RealScaleDown[i] = CIT.Note.NONE;
                    else
                        RealScaleDown[i] = CIT.NoteFromString(ScaleDownSplit[i]);
                }

                CIT.Scale newScaleUp = new(RealScaleUp[0],
                    RealScaleUp[1],
                    RealScaleUp[2],
                    RealScaleUp[3],
                    RealScaleUp[4],
                    RealScaleUp[5],
                    RealScaleUp[6],
                    RealScaleUp[7],
                    RealScaleUp[8],
                    RealScaleUp[9],
                    RealScaleUp[10],
                    RealScaleUp[11]);
                CIT.Scale newScaleDown = new(RealScaleDown[0],
                    RealScaleDown[1],
                    RealScaleDown[2],
                    RealScaleDown[3],
                    RealScaleDown[4],
                    RealScaleDown[5],
                    RealScaleDown[6],
                    RealScaleDown[7],
                    RealScaleDown[8],
                    RealScaleDown[9],
                    RealScaleDown[10],
                    RealScaleDown[11]);
                int ScaleID = IsSmallCIT ? 0 : ChordInfoTable.Scales.Count;
                for (; ScaleID < ChordInfoTable.Scales.Count; ScaleID++)
                {
                    if (ChordInfoTable.Scales[ScaleID].Up.Equals(newScaleUp) && ChordInfoTable.Scales[ScaleID].Down.Equals(newScaleDown))
                        break;
                }
                if (ScaleID == ChordInfoTable.Scales.Count)
                    ChordInfoTable.Scales.Add((newScaleUp, newScaleDown));

                Assignments.Add((RealTickNum, ChordID, ScaleID));
            }
            // if no chords were generated by this point, just slap a default in here
            if (ChordInfoTable.Chords.Count == 0) //there will never be a situation where there's no chords, but at least one Scale, or vise versa.
            {
                // According to the game, there's no scales assigned when no CIT is used.
                // The default data itself was copied from Nintendo's default values for SMG.
                AddWarning("No Chord data was defined. A default was used.");
                CIT.Chord newChord = new(CIT.Note.C, CIT.Note.C, CIT.Note.E, CIT.Note.G, CIT.Note.NONE, CIT.Note.NONE, CIT.Note.NONE, CIT.Note.NONE);
                CIT.Scale newScale = new(CIT.Note.C, CIT.Note.D, CIT.Note.E, CIT.Note.G, CIT.Note.A, CIT.Note.NONE, CIT.Note.NONE, CIT.Note.NONE, CIT.Note.NONE, CIT.Note.NONE, CIT.Note.NONE, CIT.Note.NONE);
                ChordInfoTable.Chords.Add(newChord);
                ChordInfoTable.Scales.Add((newScale, newScale));
                Assignments.Add((0, 0, 0));
            }

            Assignments = [.. Assignments.OrderBy(o => o.Tick)];

            // Time to write out the timing track... oh boy...
            BMS.Track Dest = [];

            int CurrentTime = 0;
            int TargetLoopOpcodeIndex = Dest.Count;
            bool HasProcessedLoop = LoopStart == 0 || NoLooping;

            foreach ((int Tick, int ChordID, int ScaleID) in Assignments)
            {
                while (CurrentTime < Tick)
                {
                    WriteMeasure();

                    if (!HasProcessedLoop && CurrentTime >= LoopStart)
                    {
                        TargetLoopOpcodeIndex = Dest.Count;
                        HasProcessedLoop = true;
                    }

                    if (CurrentTime >= LoopEnd)
                        goto OuterBreak;
                }
                WriteSelection(ChordID, ScaleID);
            }
        OuterBreak:
            if (!HasProcessedLoop) //If the loop hasn't been processed by this point.... Guess that there weren't chords assigned past the loop point
            {
                while (CurrentTime < LoopStart)
                {
                    WriteMeasure();
                }
                TargetLoopOpcodeIndex = Dest.Count;
                HasProcessedLoop = true;
            }
            while (CurrentTime < LoopEnd)
            {
                WriteMeasure();
            }

            if (!NoLooping)
                Dest.Add(new BMS.OpcodeJump() { TargetOpcode = Dest[TargetLoopOpcodeIndex] });

            Dest.Add(new BMS.OpcodeFinish()); // Add a finisher.

            PARENT.Children.Add((TimingComboBox.SelectedIndex - 1, Dest));

            FileUtil.SaveFile(BMSSFD.FileName.Replace(".bms", ".cit"), ChordInfoTable.Save);

            // Unfortunately, the Rhythm system doesn't support the Combined BankProg opcodes
            void WriteSelection(int ChordID, int ScaleID)
            {
                Dest.Add(new BMS.OpcodeSetBank() { Value = (byte)ScaleID });
                Dest.Add(new BMS.OpcodeSetProgram() { Value = (byte)ChordID });
            }

            void WriteMeasure()
            {
                WriteBeat();
                WriteBeat();
                WriteBeat();
                WriteBeat();

                void WriteBeat()
                {
                    List<BMS.OpcodeBase>[] AllBeatTicks = new List<BMS.OpcodeBase>[PPQN];
                    int Voice1Num = 1;
                    int Voice2Num = 8;
                    int Voice3Num = 4;
                    int Voice4Num = 2;

                    for (int i = 0; i < AllBeatTicks.Length; i++)
                        AllBeatTicks[i] = [];

                    for (int x = 0; x < Voice1Num; x++)
                    {
                        int idx = PPQN / Voice1Num;
                        AllBeatTicks[idx * x].Add(CreateNoteOn(0x18, 1, (byte)(x == 0 ? 0x0E : 0x6E)));
                        AllBeatTicks[(idx * (x + 1)) - 1].Add(CreateNoteOff(1));
                    }
                    for (int x = 0; x < Voice2Num; x++)
                    {
                        int idx = PPQN / Voice2Num;
                        AllBeatTicks[idx * x].Add(CreateNoteOn(0x19, 2, (byte)(x == 0 ? 0x22 : 0x6E)));
                        AllBeatTicks[(idx * (x + 1)) - 1].Add(CreateNoteOff(2));
                    }
                    for (int x = 0; x < Voice3Num; x++)
                    {
                        int idx = PPQN / Voice3Num;
                        AllBeatTicks[idx * x].Add(CreateNoteOn(0x1B, 3, (byte)(x == 0 ? 0x1A : 0x6E)));
                        AllBeatTicks[(idx * (x + 1)) - 1].Add(CreateNoteOff(3));
                    }
                    for (int x = 0; x < Voice4Num; x++)
                    {
                        int idx = PPQN / Voice4Num;
                        AllBeatTicks[idx * x].Add(CreateNoteOn(0x1E, 4, (byte)(x == 0 ? 0x12 : 0x6E)));
                        AllBeatTicks[(idx * (x + 1)) - 1].Add(CreateNoteOff(4));
                    }

                    int EmptyCounter = 0;
                    foreach (var item in AllBeatTicks)
                    {
                        if (item.Count == 0)
                        {
                            EmptyCounter++;
                            continue;
                        }
                        if (EmptyCounter > 0)
                            CreateWaitOpcode(EmptyCounter);

                        Dest.AddRange(item);
                        if (EmptyCounter > 0)
                            CreateWaitOpcode(2); //is it always 2?
                        EmptyCounter = 0;
                    }

                    CurrentTime += PPQN;
                }

                BMS.OpcodeBase CreateNoteOn(byte NoteID, byte VoiceID, byte VelocityValue) => new BMS.OpcodeNoteOn() { Note = NoteID, Voice = VoiceID, Velocity = VelocityValue };
                BMS.OpcodeBase CreateNoteOff(byte VoiceID) => new BMS.OpcodeNoteOff() { Voice = VoiceID };
                void CreateWaitOpcode(int delay) => Dest.Add(new BMS.OpcodeWaitVar() { Value = delay });
            }
        }


        // Hiiii so if you're reading this, you're reading my currently unimplemented failures
        // I was completely unable to figure out a way to make these happen after 3 whole days so I just gave up.
        BMS OptimizeBMS(BMS Source)
        {
            if (!IsOptimizeBMS && !IsGroupLinCC)
                return Source;

            BMS Optimized = [];
            if (IsOptimizeBMS) //IsOptimizeWideBMS requires this to be enabled so we can just check this
            {

            }
            else
                Optimized = Source;

            if (IsGroupLinCC)
            {
                //for (int i = 0; i < Optimized.Count; i++)
                //{
                //    var trk = Optimized[i].Track;
                //    GroupLinearCC(trk);
                //}



                //void GroupLinearCC(BMS.Track Track)
                //{
                //    List<BMS.OpcodeSetParamInt8>[] ParamSet8Listing = new List<BMS.OpcodeSetParamInt8>[4];
                //    for (int i = 0; i < ParamSet8Listing.Length; i++)
                //        ParamSet8Listing[i] = [];

                //    List<BMS.OpcodeBase> JustDeleteTheseOnes = [];

                //    foreach (var opcode in Track)
                //    {
                //        if (opcode is BMS.OpcodeSetParamInt8 opcode8)
                //            ParamSet8Listing[(byte)opcode8.Target].Add(opcode8);

                //        // TODO: param16
                //        if (opcode is BMS.OpcodeSetParamInt16 opcode16)
                //        {

                //        }
                //    }

                //    foreach (var item in ParamSet8Listing)
                //        ProcessParam8Group(item);

                //    DeleteExcessWaits();

                //    for (int i = 0; i < Track.Children.Count; i++)
                //    {
                //        var trk = Track.Children[i].Track;
                //        GroupLinearCC(trk);
                //    }

                //    void ProcessParam8Group(List<BMS.OpcodeSetParamInt8> list)
                //    {
                //        if (list.Count == 0)
                //            return;

                //        List<BMS.OpcodeBase> TrashCan = []; //Anything in here just gets deleted
                //        List<List<BMS.OpcodeSetParamInt8>> Groupings = [];

                //        int LastDirChangeIndex = 0;
                //        int LastChangeDelta = 0;

                //        for (int i = 1; i < list.Count; i++) //Always keep the first one
                //        {
                //            int diff = list[i].Value - list[i - 1].Value;
                //            if (diff == 0 && LastChangeDelta == 0) // We have a useless entry...
                //                TrashCan.Add(list[i-1]);
                //            else
                //            {
                //                if (LastChangeDelta != diff)
                //                {
                //                    if (LastChangeDelta != 0)
                //                        Groupings.Add(list.Slice(LastDirChangeIndex, i - LastDirChangeIndex));
                //                    LastDirChangeIndex = i - 1;
                //                }

                //                LastChangeDelta = diff;
                //            }
                //        }

                //        foreach (var item in Groupings)
                //        {
                //            if (item.Count < 2)
                //                continue; //Should be impossible...

                //            int IndexOfStart = Track.IndexOf(item[0]);
                //            int IndexOfEnd = Track.IndexOf(item[item.Count - 1]);
                //            if (IndexOfStart == -1 || IndexOfEnd == -1)
                //                continue; //Should also be impossible...

                //            int TickCounter = 0;
                //            for (int i = IndexOfStart; i < IndexOfEnd; i++)
                //            {
                //                if (Track[i] is BMS.OpcodeWaitInt8 OpWait8)
                //                    TickCounter += OpWait8.Value;
                //                else if (Track[i] is BMS.OpcodeWaitVar OpWait)
                //                    TickCounter += OpWait.Value;
                //            }
                //            Track.Insert(IndexOfStart, new BMS.OpcodeSetParamInt8Time() { Target = item[0].Target, Value = item[item.Count-1].Value, Time = (short)TickCounter });
                //            foreach (var v in item)
                //                Track.Remove(v);
                //            foreach (var v in TrashCan)
                //                Track.Remove(v);
                //        }
                //    }
                
                //    void DeleteExcessWaits()
                //    {
                //        List<BMS.OpcodeBase> TrashCan = [];
                //        List<(int idx, BMS.OpcodeBase b)> NewOpcodes = [];
                //        int StartIndex = -1;
                //        int Delta = 0;
                //        for (int i = 0; i < Track.Count; i++)
                //        {
                //            if (Track[i] is BMS.OpcodeWaitInt8 or BMS.OpcodeWaitVar)
                //            {
                //                if (StartIndex < 0)
                //                {
                //                    Delta = 0;
                //                    StartIndex = i;
                //                }
                //                if (Track[i] is BMS.OpcodeWaitInt8 OpWait8)
                //                    Delta += OpWait8.Value;
                //                else if (Track[i] is BMS.OpcodeWaitVar OpWaitV)
                //                    Delta += OpWaitV.Value;
                //            }
                //            else
                //            {
                //                if (StartIndex < 0)
                //                    continue; //Nothing to do

                //                int EndIndex = i;
                //                for (int j = StartIndex; j < EndIndex; j++)
                //                {
                //                    if (Track[j] is BMS.OpcodeWaitInt8 or BMS.OpcodeWaitVar)
                //                        TrashCan.Add(Track[j]);
                //                }

                //                NewOpcodes.Add((StartIndex, new BMS.OpcodeWaitVar() { Value = Delta }));
                //                StartIndex = -1;
                //            }
                //        }
                //        for (int i = 0; i < NewOpcodes.Count; i++)
                //            Track.Insert(NewOpcodes[i].idx, NewOpcodes[i].b);
                //        foreach (var item in TrashCan)
                //            Track.Remove(item);
                //    }
                //}
            }

            return Optimized;
        }
    
    

    }

    private struct InstrumentInfoTag
    {
        public string Name;
        public (byte BankNo, byte ProgramNo) BankProg;
        public bool IsPercussion;

        public override readonly string ToString() => Name;
    }
    private struct MIDIRemapEntry
    {
        public int ID;
        public byte? MidiKey;
        public string BMSName;
        public byte? BMSKey;

        public override readonly string ToString() => $"{ID:000} ({MidiKey:00}) -> {BMSName} ({BMSKey:00})";
    }
}
