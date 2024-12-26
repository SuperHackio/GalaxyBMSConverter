using System.ComponentModel;

namespace GalaxyBMSConverter;

public partial class OptimizeForm : Form
{
    private bool IsShowing = false;
    private int ToolTipIndex = -1;
    private readonly ToolTip DescriptionToolTip;
    private readonly List<OptimizationFlag> OptimizationList = [];
    private readonly Dictionary<OptimizationFlag, bool> OptimizationCaptureList = []; //Basically this is for allowing the user to discard changes
    
    public OptimizeForm()
    {
        InitializeComponent();

        // Add optimizations
        foreach ((string Name, string Description, bool Default, string? WarningOnEnable, string? WarningOnDisable) in Optimizations)
        {
            OptimizationFlag of = new(Name, Description, Default);
            if (WarningOnEnable is not null)
                of.WarningOnEnable = WarningOnEnable;
            if (WarningOnDisable is not null)
                of.WarningOnDisable = WarningOnDisable;
            OptimizationList.Add(of);
            OptimizationCaptureList.Add(of, Default);
        }

        DescriptionToolTip = new() {
            ToolTipTitle = "Optimization Information",
            ToolTipIcon = ToolTipIcon.Info,
            AutoPopDelay = 20000,  // Warning! MSDN states this is Int32, but anything over 32767 will fail.
            ShowAlways = true,
            InitialDelay = 200,
            ReshowDelay = 200,
            UseAnimation = true
        };
        OptimizeCheckedListBox.DataSource = new BindingList<OptimizationFlag>(OptimizationList);
        OptimizeCheckedListBox.DisplayMember = nameof(OptimizationFlag.Name); //Never knew you could use NameOf for this until now
        OptimizeCheckedListBox.ValueMember = nameof(OptimizationFlag.Enabled);
        OptimizeCheckedListBox.MouseMove += new MouseEventHandler(ShowCheckBoxToolTip);
    }

    protected override void OnShown(EventArgs e)
    {
        IsShowing = true;
        UpdateChecks();
        IsShowing = false;
        base.OnShown(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        UpdateValues();
        base.OnClosed(e);
    }

    public void Recenter() => CenterToParent();

    public void SaveCurrent()
    {
        for (int i = 0; i < OptimizationList.Count; i++)
        {
            OptimizationFlag of = OptimizationList[i];
            OptimizationCaptureList[of] = of.Enabled;
        }
    }

    public void Cancel()
    {
        for (int i = 0; i < OptimizationList.Count; i++)
        {
            OptimizationFlag of = OptimizationList[i];
            of.Enabled = OptimizationCaptureList[of];
        }
    }

    private void UpdateChecks()
    {
        for (int i = 0; i < OptimizationList.Count; i++)
            OptimizeCheckedListBox.SetItemChecked(i, OptimizationList[i].Enabled);
    }

    private void UpdateValues()
    {
        for (int i = 0; i < OptimizationList.Count; i++)
            OptimizationList[i].Enabled = OptimizeCheckedListBox.GetItemChecked(i);
    }

    // wacky function. shouldn't be called too often I hope...
    public void Reset()
    {
        for (int i = 0; i < OptimizationList.Count; i++)
            foreach ((string Name, string unused1, bool Default, string? unused2, string? unused3) in Optimizations)
                if (Name.Equals(OptimizationList[i].Name))
                {
                    OptimizationList[i].Enabled = Default;
                    break;
                }
    }

    public bool? GetOptimizationEnabled(string name)
    {
        int idx = GetIndexOfOptimization(name);
        if (idx < 0)
            return null;
        return OptimizationList[idx].Enabled;
    }

    private int GetIndexOfOptimization(string name)
    {
        for (int i = 0; i < OptimizationList.Count; i++)
            if (OptimizationList[i].Name.Equals(name))
                return i;
        return -1;
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void OptimizeCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        if (IsShowing)
            return; // Do not display warnings when opening the form bruh

        OptimizationFlag of = OptimizationList[e.Index];
        string? warning = null;
        if (e.NewValue == CheckState.Checked && of.WarningOnEnable is not null)
            warning = of.WarningOnEnable;
        if (e.NewValue == CheckState.Unchecked && of.WarningOnDisable is not null)
            warning = of.WarningOnDisable;

        if (warning is not null && MessageBox.Show(warning + Environment.NewLine + Environment.NewLine + "Do you still wish to change this?", "Optimization Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            e.NewValue = e.CurrentValue; //Cancel the operation
    }

    private void ShowCheckBoxToolTip(object? sender, MouseEventArgs e)
    {
        int pos = OptimizeCheckedListBox.IndexFromPoint(e.Location);
        if (ToolTipIndex != pos)
        {
            ToolTipIndex = OptimizeCheckedListBox.IndexFromPoint(OptimizeCheckedListBox.PointToClient(MousePosition));
            if (ToolTipIndex > -1)
                DescriptionToolTip.SetToolTip(OptimizeCheckedListBox, OptimizationList[ToolTipIndex].Description);
        }
        if (pos == -1)
            DescriptionToolTip.SetToolTip(OptimizeCheckedListBox, null);
    }



    public class OptimizationFlag(string n, string d, bool def)
    {
        public string Name { get; set; } = n;
        public string Description { get; set; } = d;
        public bool Enabled { get; set; } = def;

        public string? WarningOnEnable = null;
        public string? WarningOnDisable = null;
    }



    public const string OptPPQNTo120 = "[BMS] Convert PPQN to 120 (SMG/2 Standard)";
    public const string OptCombineBankProg = "[BMS] Use Combined Bank Programs";
    public const string OptGroupLinearControlChange = "[BMS] Create Linear Control Changes";
    public const string OptBMSRepeatOpcodes = "[BMS] Optimize Repeated Opcodes";
    public const string OptBMSMultistack = "[BMS] Sub-Optimize Repeated Opcodes";
    public const string OptSmallCIT = "[CIT] Merge identical chords/scales";

    private static readonly (string Name, string Description, bool Default, string? WarningOnEnable, string? WarningOnDisable)[] Optimizations = [
        (OptPPQNTo120, "Recalculates the Timebase (PPQN) of the input to be 120.", true, null, "PPQN other than the standard 120 may cause Beat Blocks or other Rhythm based functionality to stop working properly."),
        (OptCombineBankProg, "Makes \"Patch Change\" Midi Messages\nuse the combined Bank Program opcode\ninstead of the 2 separate Bank & Program opcodes.", false, null, null),
        //(OptGroupLinearControlChange, "Detects linear Control Changes and wraps\nthem into the Linear Change opcodes, lowering filesize.\nOnly affects the following types:\n- Volume\n- Pitch\n- Reverb\n- Panning", false, $"This setting may cause value discrepancies\nat or around the loop points.\nIt may also produce undesired results when using\n\"{OptBMSRepeatOpcodes}\"\non its own.\nThis is not guarenteed to cause problems.", null),
        //(OptBMSRepeatOpcodes, "Goes through the BMS data and creates Subroutines\nfor frequently used sequences of opcodes.\nIdeally, this reduces the final BMS file size.", false, null, null),
        //(OptBMSMultistack, $"Requires \"{OptBMSRepeatOpcodes}\" to be ENABLED.\nWhen used, larger subroutines will be created,\nand theh Sub-Subroutines will be created from those.\nAn alternate way to reduce the final BMS file size.", false, null, null),
        (OptSmallCIT, "Recycles duplicate Chord/Scale data where applicable,\nreducing the size of the final CIT file.", true, null, null),
    ];
}