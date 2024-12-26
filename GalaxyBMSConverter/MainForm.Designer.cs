namespace GalaxyBMSConverter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            InstrumentListLabelTextBoxButtonControl = new LabelTextBoxButtonControl();
            LookupLabelTextBoxButtonControl = new LabelTextBoxButtonControl();
            MidiLabelTextBoxButtonControl = new LabelTextBoxButtonControl();
            ConvertButton = new Button();
            TimingGroupBox = new GroupBox();
            ChordDataGridView = new DataGridView();
            ChordStartTickColumn = new DataGridViewTextBoxColumn();
            BassColumn = new DataGridViewTextBoxColumn();
            NoteColumn = new DataGridViewTextBoxColumn();
            ScaleUpColumn = new DataGridViewTextBoxColumn();
            ScaleDownColumn = new DataGridViewTextBoxColumn();
            ChordBPMColumn = new DataGridViewTextBoxColumn();
            splitContainer1 = new SplitContainer();
            TimingComboBox = new ComboBox();
            splitContainer5 = new SplitContainer();
            LoadChordCsvButton = new Button();
            SaveChordCsvButton = new Button();
            ConvertGroupBox = new GroupBox();
            splitContainer2 = new SplitContainer();
            splitContainer4 = new SplitContainer();
            OptimizationMenuButton = new Button();
            LoopStartTickNumericUpDown = new NumericUpDown();
            label26 = new Label();
            LoopEndTickNumericUpDown = new NumericUpDown();
            label25 = new Label();
            LoopCloseVoiceCheckBox = new CheckBox();
            splitContainer3 = new SplitContainer();
            MidiChannel00ComboBox = new ComboBox();
            label2 = new Label();
            MidiChannel01ComboBox = new ComboBox();
            label12 = new Label();
            MidiChannel07ComboBox = new ComboBox();
            MidiChannel02ComboBox = new ComboBox();
            label5 = new Label();
            MidiChannel03ComboBox = new ComboBox();
            MidiChannel06ComboBox = new ComboBox();
            MidiChannel04ComboBox = new ComboBox();
            MidiChannel05ComboBox = new ComboBox();
            label7 = new Label();
            label16 = new Label();
            label18 = new Label();
            label10 = new Label();
            label14 = new Label();
            MidiChannel08ComboBox = new ComboBox();
            label11 = new Label();
            label3 = new Label();
            MidiChannel15ComboBox = new ComboBox();
            MidiChannel09ComboBox = new ComboBox();
            label13 = new Label();
            label4 = new Label();
            MidiChannel14ComboBox = new ComboBox();
            MidiChannel10ComboBox = new ComboBox();
            label15 = new Label();
            label6 = new Label();
            MidiChannel13ComboBox = new ComboBox();
            MidiChannel11ComboBox = new ComboBox();
            label17 = new Label();
            label9 = new Label();
            MidiChannel12ComboBox = new ComboBox();
            TimingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ChordDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer5).BeginInit();
            splitContainer5.Panel1.SuspendLayout();
            splitContainer5.Panel2.SuspendLayout();
            splitContainer5.SuspendLayout();
            ConvertGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LoopStartTickNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LoopEndTickNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            SuspendLayout();
            // 
            // InstrumentListLabelTextBoxButtonControl
            // 
            InstrumentListLabelTextBoxButtonControl.ButtonText = "Browse...";
            InstrumentListLabelTextBoxButtonControl.Dock = DockStyle.Top;
            InstrumentListLabelTextBoxButtonControl.LabelSize = new Size(86, 23);
            InstrumentListLabelTextBoxButtonControl.LabelText = "Instrument List";
            InstrumentListLabelTextBoxButtonControl.Location = new Point(0, 0);
            InstrumentListLabelTextBoxButtonControl.Name = "InstrumentListLabelTextBoxButtonControl";
            InstrumentListLabelTextBoxButtonControl.Size = new Size(414, 23);
            InstrumentListLabelTextBoxButtonControl.TabIndex = 0;
            InstrumentListLabelTextBoxButtonControl.ButtonClick += InstrumentListLabelTextBoxButtonControl_ButtonClick;
            // 
            // LookupLabelTextBoxButtonControl
            // 
            LookupLabelTextBoxButtonControl.ButtonText = "Browse...";
            LookupLabelTextBoxButtonControl.Dock = DockStyle.Top;
            LookupLabelTextBoxButtonControl.LabelSize = new Size(86, 23);
            LookupLabelTextBoxButtonControl.LabelText = "Lookup List";
            LookupLabelTextBoxButtonControl.Location = new Point(0, 23);
            LookupLabelTextBoxButtonControl.Name = "LookupLabelTextBoxButtonControl";
            LookupLabelTextBoxButtonControl.Size = new Size(414, 23);
            LookupLabelTextBoxButtonControl.TabIndex = 1;
            LookupLabelTextBoxButtonControl.ButtonClick += LookupLabelTextBoxButtonControl_ButtonClick;
            // 
            // MidiLabelTextBoxButtonControl
            // 
            MidiLabelTextBoxButtonControl.ButtonText = "Browse...";
            MidiLabelTextBoxButtonControl.Dock = DockStyle.Top;
            MidiLabelTextBoxButtonControl.LabelSize = new Size(86, 23);
            MidiLabelTextBoxButtonControl.LabelText = "Midi File";
            MidiLabelTextBoxButtonControl.Location = new Point(0, 46);
            MidiLabelTextBoxButtonControl.Name = "MidiLabelTextBoxButtonControl";
            MidiLabelTextBoxButtonControl.Size = new Size(414, 23);
            MidiLabelTextBoxButtonControl.TabIndex = 2;
            MidiLabelTextBoxButtonControl.ButtonClick += MidiLabelTextBoxButtonControl_ButtonClick;
            // 
            // ConvertButton
            // 
            ConvertButton.Dock = DockStyle.Bottom;
            ConvertButton.Location = new Point(0, 578);
            ConvertButton.Name = "ConvertButton";
            ConvertButton.Size = new Size(414, 23);
            ConvertButton.TabIndex = 3;
            ConvertButton.Text = "Convert";
            ConvertButton.UseVisualStyleBackColor = true;
            ConvertButton.Click += ConvertButton_Click;
            // 
            // TimingGroupBox
            // 
            TimingGroupBox.Controls.Add(ChordDataGridView);
            TimingGroupBox.Controls.Add(splitContainer1);
            TimingGroupBox.Dock = DockStyle.Fill;
            TimingGroupBox.Location = new Point(0, 69);
            TimingGroupBox.Name = "TimingGroupBox";
            TimingGroupBox.Size = new Size(414, 194);
            TimingGroupBox.TabIndex = 4;
            TimingGroupBox.TabStop = false;
            TimingGroupBox.Text = "Timing Track Generation";
            // 
            // ChordDataGridView
            // 
            ChordDataGridView.AllowUserToResizeRows = false;
            ChordDataGridView.Columns.AddRange(new DataGridViewColumn[] { ChordStartTickColumn, BassColumn, NoteColumn, ScaleUpColumn, ScaleDownColumn, ChordBPMColumn });
            ChordDataGridView.Dock = DockStyle.Fill;
            ChordDataGridView.Location = new Point(3, 42);
            ChordDataGridView.Name = "ChordDataGridView";
            ChordDataGridView.Size = new Size(408, 149);
            ChordDataGridView.TabIndex = 2;
            // 
            // ChordStartTickColumn
            // 
            ChordStartTickColumn.HeaderText = "Chord Start Tick";
            ChordStartTickColumn.MaxInputLength = 12;
            ChordStartTickColumn.Name = "ChordStartTickColumn";
            ChordStartTickColumn.Resizable = DataGridViewTriState.False;
            ChordStartTickColumn.Width = 115;
            // 
            // BassColumn
            // 
            BassColumn.HeaderText = "Bass Note";
            BassColumn.Name = "BassColumn";
            BassColumn.Width = 90;
            // 
            // NoteColumn
            // 
            NoteColumn.HeaderText = "Chord Notes";
            NoteColumn.Name = "NoteColumn";
            // 
            // ScaleUpColumn
            // 
            ScaleUpColumn.HeaderText = "Scale Up Notes";
            ScaleUpColumn.Name = "ScaleUpColumn";
            // 
            // ScaleDownColumn
            // 
            ScaleDownColumn.HeaderText = "Scale Down Notes";
            ScaleDownColumn.Name = "ScaleDownColumn";
            // 
            // ChordBPMColumn
            // 
            ChordBPMColumn.HeaderText = "BPM Override";
            ChordBPMColumn.Name = "ChordBPMColumn";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Top;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(3, 19);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(TimingComboBox);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer5);
            splitContainer1.Size = new Size(408, 23);
            splitContainer1.SplitterDistance = 202;
            splitContainer1.TabIndex = 3;
            // 
            // TimingComboBox
            // 
            TimingComboBox.Dock = DockStyle.Fill;
            TimingComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TimingComboBox.FormattingEnabled = true;
            TimingComboBox.Items.AddRange(new object[] { "NONE", "Track 00 (Standard)", "Track 01", "Track 02", "Track 03", "Track 04", "Track 05", "Track 06", "Track 07", "Track 08", "Track 09", "Track 10", "Track 11", "Track 12", "Track 13", "Track 14", "Track 15" });
            TimingComboBox.Location = new Point(0, 0);
            TimingComboBox.Name = "TimingComboBox";
            TimingComboBox.Size = new Size(202, 23);
            TimingComboBox.TabIndex = 0;
            TimingComboBox.SelectedIndexChanged += TimingComboBox_SelectedIndexChanged;
            // 
            // splitContainer5
            // 
            splitContainer5.Dock = DockStyle.Fill;
            splitContainer5.Location = new Point(0, 0);
            splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            splitContainer5.Panel1.Controls.Add(LoadChordCsvButton);
            // 
            // splitContainer5.Panel2
            // 
            splitContainer5.Panel2.Controls.Add(SaveChordCsvButton);
            splitContainer5.Size = new Size(202, 23);
            splitContainer5.SplitterDistance = 101;
            splitContainer5.TabIndex = 0;
            // 
            // LoadChordCsvButton
            // 
            LoadChordCsvButton.Dock = DockStyle.Fill;
            LoadChordCsvButton.Location = new Point(0, 0);
            LoadChordCsvButton.Name = "LoadChordCsvButton";
            LoadChordCsvButton.Size = new Size(101, 23);
            LoadChordCsvButton.TabIndex = 0;
            LoadChordCsvButton.Text = "Load";
            LoadChordCsvButton.UseVisualStyleBackColor = true;
            LoadChordCsvButton.Click += LoadChordCsvButton_Click;
            // 
            // SaveChordCsvButton
            // 
            SaveChordCsvButton.Dock = DockStyle.Fill;
            SaveChordCsvButton.Location = new Point(0, 0);
            SaveChordCsvButton.Name = "SaveChordCsvButton";
            SaveChordCsvButton.Size = new Size(97, 23);
            SaveChordCsvButton.TabIndex = 0;
            SaveChordCsvButton.Text = "Save";
            SaveChordCsvButton.UseVisualStyleBackColor = true;
            SaveChordCsvButton.Click += SaveChordCsvButton_Click;
            // 
            // ConvertGroupBox
            // 
            ConvertGroupBox.Controls.Add(splitContainer2);
            ConvertGroupBox.Dock = DockStyle.Bottom;
            ConvertGroupBox.Location = new Point(0, 263);
            ConvertGroupBox.Name = "ConvertGroupBox";
            ConvertGroupBox.Size = new Size(414, 315);
            ConvertGroupBox.TabIndex = 5;
            ConvertGroupBox.TabStop = false;
            ConvertGroupBox.Text = "Conversion Settings";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.FixedPanel = FixedPanel.Panel1;
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Location = new Point(3, 19);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.AutoScaleMode = AutoScaleMode.Font;
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(splitContainer3);
            splitContainer2.Size = new Size(408, 293);
            splitContainer2.SplitterDistance = 54;
            splitContainer2.TabIndex = 105;
            // 
            // splitContainer4
            // 
            splitContainer4.Dock = DockStyle.Fill;
            splitContainer4.Location = new Point(0, 0);
            splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.Controls.Add(OptimizationMenuButton);
            splitContainer4.Panel1.Controls.Add(LoopStartTickNumericUpDown);
            splitContainer4.Panel1.Controls.Add(label26);
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(LoopEndTickNumericUpDown);
            splitContainer4.Panel2.Controls.Add(label25);
            splitContainer4.Panel2.Controls.Add(LoopCloseVoiceCheckBox);
            splitContainer4.Size = new Size(408, 54);
            splitContainer4.SplitterDistance = 202;
            splitContainer4.TabIndex = 0;
            // 
            // OptimizationMenuButton
            // 
            OptimizationMenuButton.Dock = DockStyle.Bottom;
            OptimizationMenuButton.Location = new Point(0, 31);
            OptimizationMenuButton.Name = "OptimizationMenuButton";
            OptimizationMenuButton.Size = new Size(202, 23);
            OptimizationMenuButton.TabIndex = 71;
            OptimizationMenuButton.Text = "Optimizations...";
            OptimizationMenuButton.UseVisualStyleBackColor = true;
            OptimizationMenuButton.Click += OptimizationMenuButton_Click;
            // 
            // LoopStartTickNumericUpDown
            // 
            LoopStartTickNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LoopStartTickNumericUpDown.Location = new Point(102, 3);
            LoopStartTickNumericUpDown.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            LoopStartTickNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            LoopStartTickNumericUpDown.Name = "LoopStartTickNumericUpDown";
            LoopStartTickNumericUpDown.Size = new Size(97, 23);
            LoopStartTickNumericUpDown.TabIndex = 70;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(3, 5);
            label26.Name = "label26";
            label26.Size = new Size(85, 15);
            label26.TabIndex = 68;
            label26.Text = "Loop Start Tick";
            // 
            // LoopEndTickNumericUpDown
            // 
            LoopEndTickNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LoopEndTickNumericUpDown.Location = new Point(99, 3);
            LoopEndTickNumericUpDown.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            LoopEndTickNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            LoopEndTickNumericUpDown.Name = "LoopEndTickNumericUpDown";
            LoopEndTickNumericUpDown.Size = new Size(100, 23);
            LoopEndTickNumericUpDown.TabIndex = 71;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(3, 7);
            label25.Name = "label25";
            label25.Size = new Size(81, 15);
            label25.TabIndex = 69;
            label25.Text = "Loop End Tick";
            // 
            // LoopCloseVoiceCheckBox
            // 
            LoopCloseVoiceCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            LoopCloseVoiceCheckBox.AutoSize = true;
            LoopCloseVoiceCheckBox.Location = new Point(55, 32);
            LoopCloseVoiceCheckBox.Name = "LoopCloseVoiceCheckBox";
            LoopCloseVoiceCheckBox.Size = new Size(138, 19);
            LoopCloseVoiceCheckBox.TabIndex = 72;
            LoopCloseVoiceCheckBox.Text = "Close Voices on Loop";
            LoopCloseVoiceCheckBox.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 0);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(MidiChannel00ComboBox);
            splitContainer3.Panel1.Controls.Add(label2);
            splitContainer3.Panel1.Controls.Add(MidiChannel01ComboBox);
            splitContainer3.Panel1.Controls.Add(label12);
            splitContainer3.Panel1.Controls.Add(MidiChannel07ComboBox);
            splitContainer3.Panel1.Controls.Add(MidiChannel02ComboBox);
            splitContainer3.Panel1.Controls.Add(label5);
            splitContainer3.Panel1.Controls.Add(MidiChannel03ComboBox);
            splitContainer3.Panel1.Controls.Add(MidiChannel06ComboBox);
            splitContainer3.Panel1.Controls.Add(MidiChannel04ComboBox);
            splitContainer3.Panel1.Controls.Add(MidiChannel05ComboBox);
            splitContainer3.Panel1.Controls.Add(label7);
            splitContainer3.Panel1.Controls.Add(label16);
            splitContainer3.Panel1.Controls.Add(label18);
            splitContainer3.Panel1.Controls.Add(label10);
            splitContainer3.Panel1.Controls.Add(label14);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(MidiChannel08ComboBox);
            splitContainer3.Panel2.Controls.Add(label11);
            splitContainer3.Panel2.Controls.Add(label3);
            splitContainer3.Panel2.Controls.Add(MidiChannel15ComboBox);
            splitContainer3.Panel2.Controls.Add(MidiChannel09ComboBox);
            splitContainer3.Panel2.Controls.Add(label13);
            splitContainer3.Panel2.Controls.Add(label4);
            splitContainer3.Panel2.Controls.Add(MidiChannel14ComboBox);
            splitContainer3.Panel2.Controls.Add(MidiChannel10ComboBox);
            splitContainer3.Panel2.Controls.Add(label15);
            splitContainer3.Panel2.Controls.Add(label6);
            splitContainer3.Panel2.Controls.Add(MidiChannel13ComboBox);
            splitContainer3.Panel2.Controls.Add(MidiChannel11ComboBox);
            splitContainer3.Panel2.Controls.Add(label17);
            splitContainer3.Panel2.Controls.Add(label9);
            splitContainer3.Panel2.Controls.Add(MidiChannel12ComboBox);
            splitContainer3.Size = new Size(408, 235);
            splitContainer3.SplitterDistance = 202;
            splitContainer3.TabIndex = 0;
            // 
            // MidiChannel00ComboBox
            // 
            MidiChannel00ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel00ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel00ComboBox.FormattingEnabled = true;
            MidiChannel00ComboBox.Location = new Point(102, 4);
            MidiChannel00ComboBox.Name = "MidiChannel00ComboBox";
            MidiChannel00ComboBox.Size = new Size(97, 23);
            MidiChannel00ComboBox.TabIndex = 73;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 8);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 74;
            label2.Text = "MIDI Channel 00";
            // 
            // MidiChannel01ComboBox
            // 
            MidiChannel01ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel01ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel01ComboBox.FormattingEnabled = true;
            MidiChannel01ComboBox.Location = new Point(102, 33);
            MidiChannel01ComboBox.Name = "MidiChannel01ComboBox";
            MidiChannel01ComboBox.Size = new Size(97, 23);
            MidiChannel01ComboBox.TabIndex = 77;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(3, 211);
            label12.Name = "label12";
            label12.Size = new Size(94, 15);
            label12.TabIndex = 102;
            label12.Text = "MIDI Channel 07";
            // 
            // MidiChannel07ComboBox
            // 
            MidiChannel07ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel07ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel07ComboBox.FormattingEnabled = true;
            MidiChannel07ComboBox.Location = new Point(102, 207);
            MidiChannel07ComboBox.Name = "MidiChannel07ComboBox";
            MidiChannel07ComboBox.Size = new Size(97, 23);
            MidiChannel07ComboBox.TabIndex = 101;
            // 
            // MidiChannel02ComboBox
            // 
            MidiChannel02ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel02ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel02ComboBox.FormattingEnabled = true;
            MidiChannel02ComboBox.Location = new Point(102, 62);
            MidiChannel02ComboBox.Name = "MidiChannel02ComboBox";
            MidiChannel02ComboBox.Size = new Size(97, 23);
            MidiChannel02ComboBox.TabIndex = 81;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(3, 37);
            label5.Name = "label5";
            label5.Size = new Size(94, 15);
            label5.TabIndex = 78;
            label5.Text = "MIDI Channel 01";
            // 
            // MidiChannel03ComboBox
            // 
            MidiChannel03ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel03ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel03ComboBox.FormattingEnabled = true;
            MidiChannel03ComboBox.Location = new Point(102, 91);
            MidiChannel03ComboBox.Name = "MidiChannel03ComboBox";
            MidiChannel03ComboBox.Size = new Size(97, 23);
            MidiChannel03ComboBox.TabIndex = 85;
            // 
            // MidiChannel06ComboBox
            // 
            MidiChannel06ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel06ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel06ComboBox.FormattingEnabled = true;
            MidiChannel06ComboBox.Location = new Point(102, 178);
            MidiChannel06ComboBox.Name = "MidiChannel06ComboBox";
            MidiChannel06ComboBox.Size = new Size(97, 23);
            MidiChannel06ComboBox.TabIndex = 97;
            // 
            // MidiChannel04ComboBox
            // 
            MidiChannel04ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel04ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel04ComboBox.FormattingEnabled = true;
            MidiChannel04ComboBox.Location = new Point(102, 120);
            MidiChannel04ComboBox.Name = "MidiChannel04ComboBox";
            MidiChannel04ComboBox.Size = new Size(97, 23);
            MidiChannel04ComboBox.TabIndex = 89;
            // 
            // MidiChannel05ComboBox
            // 
            MidiChannel05ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel05ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel05ComboBox.FormattingEnabled = true;
            MidiChannel05ComboBox.Location = new Point(102, 149);
            MidiChannel05ComboBox.Name = "MidiChannel05ComboBox";
            MidiChannel05ComboBox.Size = new Size(97, 23);
            MidiChannel05ComboBox.TabIndex = 93;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(3, 66);
            label7.Name = "label7";
            label7.Size = new Size(94, 15);
            label7.TabIndex = 82;
            label7.Text = "MIDI Channel 02";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(3, 153);
            label16.Name = "label16";
            label16.Size = new Size(94, 15);
            label16.TabIndex = 94;
            label16.Text = "MIDI Channel 05";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(3, 124);
            label18.Name = "label18";
            label18.Size = new Size(94, 15);
            label18.TabIndex = 90;
            label18.Text = "MIDI Channel 04";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(3, 95);
            label10.Name = "label10";
            label10.Size = new Size(94, 15);
            label10.TabIndex = 86;
            label10.Text = "MIDI Channel 03";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(3, 182);
            label14.Name = "label14";
            label14.Size = new Size(94, 15);
            label14.TabIndex = 98;
            label14.Text = "MIDI Channel 06";
            // 
            // MidiChannel08ComboBox
            // 
            MidiChannel08ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel08ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel08ComboBox.FormattingEnabled = true;
            MidiChannel08ComboBox.Location = new Point(99, 4);
            MidiChannel08ComboBox.Name = "MidiChannel08ComboBox";
            MidiChannel08ComboBox.Size = new Size(100, 23);
            MidiChannel08ComboBox.TabIndex = 75;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(-1, 211);
            label11.Name = "label11";
            label11.Size = new Size(94, 15);
            label11.TabIndex = 104;
            label11.Text = "MIDI Channel 15";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(-1, 8);
            label3.Name = "label3";
            label3.Size = new Size(94, 15);
            label3.TabIndex = 76;
            label3.Text = "MIDI Channel 08";
            // 
            // MidiChannel15ComboBox
            // 
            MidiChannel15ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel15ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel15ComboBox.FormattingEnabled = true;
            MidiChannel15ComboBox.Location = new Point(99, 207);
            MidiChannel15ComboBox.Name = "MidiChannel15ComboBox";
            MidiChannel15ComboBox.Size = new Size(100, 23);
            MidiChannel15ComboBox.TabIndex = 103;
            // 
            // MidiChannel09ComboBox
            // 
            MidiChannel09ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel09ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel09ComboBox.FormattingEnabled = true;
            MidiChannel09ComboBox.Location = new Point(99, 33);
            MidiChannel09ComboBox.Name = "MidiChannel09ComboBox";
            MidiChannel09ComboBox.Size = new Size(100, 23);
            MidiChannel09ComboBox.TabIndex = 79;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(-1, 182);
            label13.Name = "label13";
            label13.Size = new Size(94, 15);
            label13.TabIndex = 100;
            label13.Text = "MIDI Channel 14";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(-1, 37);
            label4.Name = "label4";
            label4.Size = new Size(94, 15);
            label4.TabIndex = 80;
            label4.Text = "MIDI Channel 09";
            // 
            // MidiChannel14ComboBox
            // 
            MidiChannel14ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel14ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel14ComboBox.FormattingEnabled = true;
            MidiChannel14ComboBox.Location = new Point(99, 178);
            MidiChannel14ComboBox.Name = "MidiChannel14ComboBox";
            MidiChannel14ComboBox.Size = new Size(100, 23);
            MidiChannel14ComboBox.TabIndex = 99;
            // 
            // MidiChannel10ComboBox
            // 
            MidiChannel10ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel10ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel10ComboBox.FormattingEnabled = true;
            MidiChannel10ComboBox.Location = new Point(99, 62);
            MidiChannel10ComboBox.Name = "MidiChannel10ComboBox";
            MidiChannel10ComboBox.Size = new Size(100, 23);
            MidiChannel10ComboBox.TabIndex = 83;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(-1, 153);
            label15.Name = "label15";
            label15.Size = new Size(94, 15);
            label15.TabIndex = 96;
            label15.Text = "MIDI Channel 13";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(-1, 66);
            label6.Name = "label6";
            label6.Size = new Size(94, 15);
            label6.TabIndex = 84;
            label6.Text = "MIDI Channel 10";
            // 
            // MidiChannel13ComboBox
            // 
            MidiChannel13ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel13ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel13ComboBox.FormattingEnabled = true;
            MidiChannel13ComboBox.Location = new Point(99, 149);
            MidiChannel13ComboBox.Name = "MidiChannel13ComboBox";
            MidiChannel13ComboBox.Size = new Size(100, 23);
            MidiChannel13ComboBox.TabIndex = 95;
            // 
            // MidiChannel11ComboBox
            // 
            MidiChannel11ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel11ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel11ComboBox.FormattingEnabled = true;
            MidiChannel11ComboBox.Location = new Point(99, 91);
            MidiChannel11ComboBox.Name = "MidiChannel11ComboBox";
            MidiChannel11ComboBox.Size = new Size(100, 23);
            MidiChannel11ComboBox.TabIndex = 87;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(-1, 124);
            label17.Name = "label17";
            label17.Size = new Size(94, 15);
            label17.TabIndex = 92;
            label17.Text = "MIDI Channel 12";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(-1, 95);
            label9.Name = "label9";
            label9.Size = new Size(94, 15);
            label9.TabIndex = 88;
            label9.Text = "MIDI Channel 11";
            // 
            // MidiChannel12ComboBox
            // 
            MidiChannel12ComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MidiChannel12ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MidiChannel12ComboBox.FormattingEnabled = true;
            MidiChannel12ComboBox.Location = new Point(99, 120);
            MidiChannel12ComboBox.Name = "MidiChannel12ComboBox";
            MidiChannel12ComboBox.Size = new Size(100, 23);
            MidiChannel12ComboBox.TabIndex = 91;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(414, 601);
            Controls.Add(TimingGroupBox);
            Controls.Add(ConvertGroupBox);
            Controls.Add(ConvertButton);
            Controls.Add(MidiLabelTextBoxButtonControl);
            Controls.Add(LookupLabelTextBoxButtonControl);
            Controls.Add(InstrumentListLabelTextBoxButtonControl);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(430, 640);
            Name = "MainForm";
            Text = "Galaxy BMS Converter";
            TimingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ChordDataGridView).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer5.Panel1.ResumeLayout(false);
            splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer5).EndInit();
            splitContainer5.ResumeLayout(false);
            ConvertGroupBox.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel1.PerformLayout();
            splitContainer4.Panel2.ResumeLayout(false);
            splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)LoopStartTickNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)LoopEndTickNumericUpDown).EndInit();
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel1.PerformLayout();
            splitContainer3.Panel2.ResumeLayout(false);
            splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private LabelTextBoxButtonControl InstrumentListLabelTextBoxButtonControl;
        private LabelTextBoxButtonControl LookupLabelTextBoxButtonControl;
        private LabelTextBoxButtonControl MidiLabelTextBoxButtonControl;
        private Button ConvertButton;
        private GroupBox TimingGroupBox;
        private GroupBox ConvertGroupBox;
        private CheckBox LoopCloseVoiceCheckBox;
        private NumericUpDown LoopEndTickNumericUpDown;
        private NumericUpDown LoopStartTickNumericUpDown;
        private Label label26;
        private Label label25;
        private ComboBox TimingComboBox;
        private Label label2;
        private ComboBox MidiChannel00ComboBox;
        private Label label3;
        private ComboBox MidiChannel08ComboBox;
        private Label label11;
        private ComboBox MidiChannel15ComboBox;
        private Label label12;
        private ComboBox MidiChannel07ComboBox;
        private Label label13;
        private ComboBox MidiChannel14ComboBox;
        private Label label14;
        private ComboBox MidiChannel06ComboBox;
        private Label label15;
        private ComboBox MidiChannel13ComboBox;
        private Label label16;
        private ComboBox MidiChannel05ComboBox;
        private Label label17;
        private ComboBox MidiChannel12ComboBox;
        private Label label18;
        private ComboBox MidiChannel04ComboBox;
        private Label label9;
        private ComboBox MidiChannel11ComboBox;
        private Label label10;
        private ComboBox MidiChannel03ComboBox;
        private Label label6;
        private ComboBox MidiChannel10ComboBox;
        private Label label7;
        private ComboBox MidiChannel02ComboBox;
        private Label label4;
        private ComboBox MidiChannel09ComboBox;
        private Label label5;
        private ComboBox MidiChannel01ComboBox;
        private DataGridView ChordDataGridView;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainer4;
        private SplitContainer splitContainer3;
        private SplitContainer splitContainer5;
        private Button LoadChordCsvButton;
        private Button SaveChordCsvButton;
        private Button OptimizationMenuButton;
        private DataGridViewTextBoxColumn ChordStartTickColumn;
        private DataGridViewTextBoxColumn BassColumn;
        private DataGridViewTextBoxColumn NoteColumn;
        private DataGridViewTextBoxColumn ScaleUpColumn;
        private DataGridViewTextBoxColumn ScaleDownColumn;
        private DataGridViewTextBoxColumn ChordBPMColumn;
    }
}
