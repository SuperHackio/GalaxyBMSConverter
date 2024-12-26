namespace GalaxyBMSConverter
{
    partial class LabelTextBoxButtonControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            InfoLabel = new Label();
            InputTextBox = new TextBox();
            ActionButton = new Button();
            SuspendLayout();
            // 
            // InfoLabel
            // 
            InfoLabel.Dock = DockStyle.Left;
            InfoLabel.Location = new Point(0, 0);
            InfoLabel.Name = "InfoLabel";
            InfoLabel.Size = new Size(32, 23);
            InfoLabel.TabIndex = 0;
            InfoLabel.Text = "Text";
            InfoLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // InputTextBox
            // 
            InputTextBox.Dock = DockStyle.Fill;
            InputTextBox.Location = new Point(32, 0);
            InputTextBox.Name = "InputTextBox";
            InputTextBox.Size = new Size(154, 23);
            InputTextBox.TabIndex = 1;
            InputTextBox.TextChanged += InputTextBox_TextChanged;
            // 
            // ActionButton
            // 
            ActionButton.Dock = DockStyle.Right;
            ActionButton.Location = new Point(186, 0);
            ActionButton.Name = "ActionButton";
            ActionButton.Size = new Size(75, 23);
            ActionButton.TabIndex = 2;
            ActionButton.Text = "TEXT2";
            ActionButton.UseVisualStyleBackColor = true;
            ActionButton.Click += ActionButton_Click;
            // 
            // LabelTextBoxButtonControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(InputTextBox);
            Controls.Add(ActionButton);
            Controls.Add(InfoLabel);
            Name = "LabelTextBoxButtonControl";
            Size = new Size(261, 23);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label InfoLabel;
        private TextBox InputTextBox;
        private Button ActionButton;
    }
}
