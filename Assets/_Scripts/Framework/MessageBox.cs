using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Mcpgnz.DesktopFramework;
using Application = UnityEngine.Application;

public class GrowLabel : Label
{
    private bool mGrowing;
    public GrowLabel()
    {
        AutoSize = false;
    }
    private void resizeLabel()
    {
        if (mGrowing) return;
        try
        {
            mGrowing = true;
            Size sz = new Size(Width, int.MaxValue);
            sz = TextRenderer.MeasureText(Text, Font, sz, TextFormatFlags.WordBreak);
            Height = sz.Height;
        }
        finally
        {
            mGrowing = false;
        }
    }
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        resizeLabel();
    }
    protected override void OnFontChanged(EventArgs e)
    {
        base.OnFontChanged(e);
        resizeLabel();
    }
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        resizeLabel();
    }
}

public class MessageBox : Form
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    private GrowLabel message;
    private PictureBox icon;
    private Button[] buttons;
    private FlowLayoutPanel buttonsPanel;

    public Action<string> OnButtonClick;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="title">Message box title.</param>
    /// <param name="message">Message box content.</param>
    /// <param name="buttonLabels">List of labels to put onto buttons. Selected option will be passed to OnButtonClick event handler.</param>
    /// <param name="iconPath">Optional icon path. Either absolute path, or one relative to StreamingAssets.</param>
    public MessageBox(string title, string message, string[] buttonLabels, string iconPath = null)
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

        buttons = new Button[buttonLabels.Length];
        for (int i = 0; i < buttonLabels.Length; ++i)
        {
            buttons[i] = new Button();
        }

        buttonsPanel = new FlowLayoutPanel();

        //  buttonsPanel.SuspendLayout();
        // SuspendLayout();

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

        this.message.Dock = DockStyle.Fill;
        this.message.AutoSize = true;
        this.message.Padding = new Padding(20);
        this.message.Name = "message";
        this.message.TabIndex = 1;
        this.message.Text = message;

        buttonsPanel.WrapContents = false;
        buttonsPanel.AutoSize = true;
        buttonsPanel.AutoSizeMode = AutoSizeMode.GrowOnly;
        buttonsPanel.Anchor = AnchorStyles.Bottom;
        buttonsPanel.Dock = DockStyle.Bottom;
        buttonsPanel.Margin = new Padding(10, 0, 10, 0);
        buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonsPanel.HorizontalScroll.Enabled = false;
        buttonsPanel.VerticalScroll.Enabled = false;

        for (int i = buttonLabels.Length - 1; i >= 0; --i
        )
        {
            Button btn = buttons[i];
            string label = buttonLabels[i];

            btn.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            btn.Name = "btn_" + label;
            btn.AutoSizeMode = AutoSizeMode.GrowOnly;
            btn.Padding = new Padding(15, 1, 15, 1);
            btn.AutoSize = true;
            btn.TabIndex = 2 + i;
            btn.Text = label;
            btn.Update();
            btn.Click += (sender, args) =>
            {
                OnButtonClick?.Invoke(label);
                Dispose();
            };

            buttonsPanel.Controls.Add(btn);
        }

        AutoScaleDimensions = new SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSizeMode = AutoSizeMode.GrowOnly;
        AutoSize = true;
        Controls.Add(this.message);
        Controls.Add(buttonsPanel);
        Controls.Add(icon);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ControlBox = false;
        Name = "MessageForm";
        Padding = new Padding(15);
        StartPosition = FormStartPosition.CenterScreen;
        MaximumSize = new Size(1280, 200);
        MinimumSize = new Size(buttons.Length * Config.MinResponseSize, 32);
        Text = title;
        TopLevel = true;
        if (icon != null)
        {
            ((ISupportInitialize)icon).EndInit();
            icon.Load();
        }
        buttonsPanel.ResumeLayout();
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

    private static string SelectRandomIcon()
    {
        var icons = new List<FileInfo>(new DirectoryInfo(Application.streamingAssetsPath).GetFiles()).Where(file => file.Name.EndsWith(".ico")).ToList();
        return icons[new Random().Next(0, icons.Count)].Name;
    }

    public static void Demo()
    {
        Action displayMessageBox = null;
        displayMessageBox = () =>
        {
            MessageBox box = new MessageBox("Łiii", "Jeszcze raz?", new[] { "Tak", "Nie", "Może", "Niech będzie", "A co mi tam...", "Dajesz!", "No kurwa", "Wincyj", "tych", "przycisków", "daj", "AAAAAAA", "お前はもう死んでいる" }, SelectRandomIcon());
            box.OnButtonClick += button =>
            {
                switch (button)
                {
                    case "Nie": break;
                    case "Może": if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f) displayMessageBox(); break;
                    default: displayMessageBox(); break;
                }
            };
            box.Show();
        };

        displayMessageBox();
    }
}
