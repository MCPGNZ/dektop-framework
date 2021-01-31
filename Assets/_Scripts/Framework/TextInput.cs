using System.Drawing;
using System.Windows.Forms;

public class TextInput : Form
{
    private System.ComponentModel.IContainer components = null;

    private Label message;
    private FlowLayoutPanel layout;
    private Button okButton;
    private TextBox textInput;
    private PictureBox icon;

    public TextInput(string title, string message, string iconPath = null)
    {
        if (iconPath != null && !System.IO.Path.IsPathRooted(iconPath))
        {
            iconPath = UnityEngine.Application.streamingAssetsPath + System.IO.Path.DirectorySeparatorChar + iconPath;
        }

        this.message = new GrowLabel();
        if (iconPath != null)
        {
            this.icon = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
        }

        this.layout = new System.Windows.Forms.FlowLayoutPanel();
        this.okButton = new System.Windows.Forms.Button();
        this.textInput = new System.Windows.Forms.TextBox();
        this.layout.SuspendLayout();
        this.SuspendLayout();
        // 
        // icon
        // 
        if (this.icon != null)
        {
            this.icon.Dock = DockStyle.Left;
            this.icon.ErrorImage = null;
            this.icon.InitialImage = null;
            this.icon.ImageLocation = iconPath;
            this.icon.Name = "icon";
            this.icon.Size = new Size(96, 96);
            this.icon.SizeMode = PictureBoxSizeMode.Zoom;
            this.icon.TabIndex = 0;
            this.icon.TabStop = false;
        }
        // 
        // message
        // 
        this.message.AutoSize = true;
        this.message.Dock = System.Windows.Forms.DockStyle.Fill;
        this.message.Location = new System.Drawing.Point(96, 0);
        this.message.Margin = new System.Windows.Forms.Padding(20);
        this.message.Name = "message";
        this.message.Padding = new System.Windows.Forms.Padding(20);
        this.message.TabIndex = 1;
        this.message.Text = message;
        // 
        // layout
        // 
        this.layout.AutoSize = true;
        this.layout.Controls.Add(this.okButton);
        this.layout.Controls.Add(this.textInput);
        this.layout.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.layout.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
        this.layout.Name = "layout";
        this.layout.TabIndex = 2;
        this.layout.Padding = new Padding(0, 5, 15, 5);
        this.layout.MinimumSize = new Size(0, 40);
        // 
        // okButton
        // 
        this.okButton.AutoSize = true;
        this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        this.okButton.Padding = new Padding(15, 1, 15, 1);
        this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.okButton.Name = "okButton";
        this.okButton.TabIndex = 1;
        this.okButton.Text = "OK?";
        this.okButton.UseVisualStyleBackColor = true;
        // 
        // textInput
        // 
        this.textInput.AutoSize = true;
        this.textInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.textInput.MinimumSize = new Size(200, 10);
        this.textInput.Name = "textInput";
        this.textInput.TabIndex = 0;
        this.textInput.ReadOnly = false;
        // 
        // TextInput
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSize = true;
        this.Controls.Add(this.layout);
        this.Controls.Add(this.message);
        this.Controls.Add(this.icon);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ControlBox = false;
        this.Name = "TextInput";
        this.Padding = new System.Windows.Forms.Padding(15);
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Size = new Size(500, 200);
        this.MinimumSize = this.Size;
        this.MaximumSize = this.Size;
        this.Text = title;
        this.TopLevel = true;
        if (this.icon != null)
        {
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.icon.Load();
        }
        this.layout.ResumeLayout(false);
        this.layout.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

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

    public static string Show(string title, string message, string iconPath)
    {
        bool shouldRestartKeyInput = UnityRawInput.RawKeyInput.IsRunning;
        UnityRawInput.RawKeyInput.Stop();

        var input = new TextInput(title, message, iconPath);
        input.textInput.Focus();

        string result = "";
        if (input.ShowDialog() == DialogResult.OK)
            result = input.textInput.Text;

        input.Dispose();
        if (shouldRestartKeyInput)
            UnityRawInput.RawKeyInput.Start(workInBackround: true);

        return result;
    }
}
