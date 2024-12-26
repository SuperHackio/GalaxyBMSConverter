using System.ComponentModel;

namespace GalaxyBMSConverter;

public partial class LabelTextBoxButtonControl : UserControl
{
    public string LabelText
    {
        get => InfoLabel.Text;
        set => InfoLabel.Text = value;
    }

    public Size LabelSize
    {
        get => InfoLabel.Size;
        set => InfoLabel.Size = value;
    }

    public new string Text
    {
        get => InputTextBox.Text;
        set => InputTextBox.Text = value;
    }

    public string ButtonText
    {
        get => ActionButton.Text;
        set => ActionButton.Text = value;
    }

    [Category("Action")]
    [Description("Occurs when the Action Button specifically is clicked")]
    public event EventHandler? ButtonClick
    {
        add => Events.AddHandler(ActionButton, value);
        remove => Events.RemoveHandler(ActionButton, value);
    }

    public LabelTextBoxButtonControl()
    {
        InitializeComponent();
    }

    private void InputTextBox_TextChanged(object sender, EventArgs e)
    {
        OnTextChanged(e);
    }

    private void ActionButton_Click(object sender, EventArgs e)
    {
        if (Events[ActionButton] is EventHandler eh)
            eh(this, e);
    }
}
