using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UnityRawInput;
using Application = UnityEngine.Application;

public class TextInput : Form
{
    private IContainer components = null;

    private Label message;
    private FlowLayoutPanel layout;
    private Button okButton;
    private TextBox textInput;
    private PictureBox icon;

    public TextInput(string title, string message, string iconPath = null)
    {
        if (iconPath != null && !Path.IsPathRooted(iconPath))
        {
            iconPath = Application.streamingAssetsPath + Path.DirectorySeparatorChar + iconPath;
        }

        this.message = new GrowLabel();
        if (iconPath != null)
        {
            icon = new PictureBox();
            ((ISupportInitialize)icon).BeginInit();
        }

        layout = new FlowLayoutPanel();
        okButton = new Button();
        textInput = new TextBox();
        layout.SuspendLayout();
        SuspendLayout();
        //
        // icon
        //
        if (icon != null)
        {
            icon.Dock = DockStyle.Left;
            icon.ErrorImage = null;
            icon.InitialImage = null;
            icon.ImageLocation = iconPath;
            icon.Name = "icon";
            icon.Size = new Size(96, 96);
            icon.SizeMode = PictureBoxSizeMode.Zoom;
            icon.TabIndex = 0;
            icon.TabStop = false;
        }
        //
        // message
        //
        this.message.AutoSize = true;
        this.message.Dock = DockStyle.Fill;
        this.message.Location = new Point(96, 0);
        this.message.Margin = new Padding(20);
        this.message.Name = "message";
        this.message.Padding = new Padding(20);
        this.message.TabIndex = 1;
        this.message.Text = message;
        //
        // layout
        //
        layout.AutoSize = true;
        layout.Controls.Add(okButton);
        layout.Controls.Add(textInput);
        layout.Dock = DockStyle.Bottom;
        layout.FlowDirection = FlowDirection.RightToLeft;
        layout.Name = "layout";
        layout.TabIndex = 2;
        layout.Padding = new Padding(0, 5, 15, 5);
        layout.MinimumSize = new Size(0, 40);
        //
        // okButton
        //
        okButton.AutoSize = true;
        okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        okButton.Padding = new Padding(15, 1, 15, 1);
        okButton.DialogResult = DialogResult.OK;
        okButton.Name = "okButton";
        okButton.TabIndex = 1;
        okButton.Text = "OK?";
        okButton.UseVisualStyleBackColor = true;
        //
        // textInput
        //
        textInput.AutoSize = true;
        textInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        textInput.MinimumSize = new Size(250, 50);
        textInput.Multiline = false;
        textInput.WordWrap = false;
        textInput.Name = "textInput";
        textInput.TabIndex = 0;
        textInput.ReadOnly = false;
        //
        // TextInput
        //
        AutoScaleDimensions = new SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        Controls.Add(layout);
        Controls.Add(this.message);
        Controls.Add(icon);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ControlBox = false;
        Name = "TextInput";
        Padding = new Padding(15);
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(550, 250);
        MinimumSize = Size;
        MaximumSize = Size;
        Text = title;
        TopMost = true;
        if (icon != null)
        {
            ((ISupportInitialize)icon).EndInit();
            icon.Load();
        }
        layout.ResumeLayout(false);
        layout.PerformLayout();
        ResumeLayout(false);
        PerformLayout();

    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    public static string Show(string title, string message, string iconPath)
    {
        bool shouldRestartKeyInput = RawKeyInput.IsRunning;
        RawKeyInput.Stop();

        var input = new TextInput(title, message, iconPath);
        input.textInput.Focus();

        string result = "";
        if (input.ShowDialog() == DialogResult.OK)
            result = input.textInput.Text;

        input.Dispose();
        if (shouldRestartKeyInput)
            RawKeyInput.Start(true);

        return result;
    }
}