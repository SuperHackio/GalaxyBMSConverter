namespace GalaxyBMSConverter
{
    partial class OptimizeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptimizeForm));
            OptimizeCheckedListBox = new CheckedListBox();
            SaveButton = new Button();
            InfoLabel = new Label();
            SuspendLayout();
            // 
            // OptimizeCheckedListBox
            // 
            OptimizeCheckedListBox.Dock = DockStyle.Fill;
            OptimizeCheckedListBox.FormattingEnabled = true;
            OptimizeCheckedListBox.IntegralHeight = false;
            OptimizeCheckedListBox.Location = new Point(0, 78);
            OptimizeCheckedListBox.Name = "OptimizeCheckedListBox";
            OptimizeCheckedListBox.Size = new Size(294, 339);
            OptimizeCheckedListBox.TabIndex = 0;
            OptimizeCheckedListBox.ItemCheck += OptimizeCheckedListBox_ItemCheck;
            // 
            // SaveButton
            // 
            SaveButton.Dock = DockStyle.Bottom;
            SaveButton.Location = new Point(0, 417);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(294, 23);
            SaveButton.TabIndex = 1;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // InfoLabel
            // 
            InfoLabel.Dock = DockStyle.Top;
            InfoLabel.Location = new Point(0, 0);
            InfoLabel.Name = "InfoLabel";
            InfoLabel.Size = new Size(294, 78);
            InfoLabel.TabIndex = 2;
            InfoLabel.Text = resources.GetString("InfoLabel.Text");
            InfoLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // OptimizeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(294, 440);
            Controls.Add(OptimizeCheckedListBox);
            Controls.Add(InfoLabel);
            Controls.Add(SaveButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(310, 360);
            Name = "OptimizeForm";
            Text = "Select Optimizations";
            ResumeLayout(false);
        }

        #endregion

        private CheckedListBox OptimizeCheckedListBox;
        private Button SaveButton;
        private Label InfoLabel;
    }
}