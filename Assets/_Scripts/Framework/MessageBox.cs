using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

public class GrowLabel : Label
{
    private bool mGrowing;
    public GrowLabel()
    {
        this.AutoSize = false;
    }
    private void resizeLabel()
    {
        if (mGrowing) return;
        try
        {
            mGrowing = true;
            Size sz = new Size(this.Width, Int32.MaxValue);
            sz = TextRenderer.MeasureText(this.Text, this.Font, sz, TextFormatFlags.WordBreak);
            this.Height = sz.Height;
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
    private System.ComponentModel.IContainer components = null;

    private GrowLabel message = null;
    private PictureBox icon = null;
    private Button[] buttons = null;
    private FlowLayoutPanel buttonsPanel = null;

    public Action<string> OnButtonClick = null;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="title">Message box title.</param>
    /// <param name="message">Message box content.</param>
    /// <param name="buttonLabels">List of labels to put onto buttons. Selected option will be passed to OnButtonClick event handler.</param>
    /// <param name="iconPath">Optional icon path. Either absolute path, or one relative to StreamingAssets.</param>
    public MessageBox(string title, string message, string[] buttonLabels, string iconPath = null)
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

        this.buttons = new Button[buttonLabels.Length];
        for (int i = 0; i < buttonLabels.Length; ++i)
        {
            this.buttons[i] = new Button();
        }

        this.buttonsPanel = new FlowLayoutPanel();

        this.buttonsPanel.SuspendLayout();
        this.SuspendLayout();

        if (this.icon != null)
        {
            this.icon.Dock = DockStyle.Left;
            this.icon.ErrorImage = null;
            this.icon.InitialImage = null;
            this.icon.ImageLocation = iconPath;
            this.icon.Name = "icon";
            this.icon.MaximumSize = new Size(64, 64);
            this.icon.SizeMode = PictureBoxSizeMode.Zoom;
            this.icon.TabIndex = 0;
            this.icon.TabStop = false;
        }

        this.message.Dock = DockStyle.Fill;
        this.message.AutoSize = true;
        this.message.Padding = new Padding(20);
        this.message.Name = "message";
        this.message.TabIndex = 1;
        this.message.Text = message;

        this.buttonsPanel.AutoSize = true;
        this.buttonsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.buttonsPanel.Anchor = AnchorStyles.Bottom;
        this.buttonsPanel.Dock = DockStyle.Bottom;
        this.buttonsPanel.Margin = new Padding(10, 0, 10, 0);
        this.buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
        this.buttonsPanel.HorizontalScroll.Enabled = false;
        this.buttonsPanel.VerticalScroll.Enabled = false;

        for (int i = buttonLabels.Length - 1; i >= 0; ++i)
        {
            Button btn = buttons[i];
            string label = buttonLabels[i];

            btn.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            btn.Name = "btn_" + label;
            btn.Padding = new Padding(15, 1, 15, 1);
            btn.AutoSize = true;
            btn.TabIndex = 2 + i;
            btn.Text = label;
            btn.Click += new EventHandler((object sender, EventArgs args) =>
            {
                this.OnButtonClick?.Invoke(label);
                this.Dispose();
            });

            this.buttonsPanel.Controls.Add(btn);
        }

        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSize = true;
        this.Controls.Add(this.message);
        this.Controls.Add(this.buttonsPanel);
        this.Controls.Add(this.icon);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ControlBox = false;
        this.Name = "MessageForm";
        this.Padding = new System.Windows.Forms.Padding(15);
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.MaximumSize = new Size(600, 200);
        this.Text = title;
        if (this.icon != null)
        {
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.icon.Load();
        }
        this.buttonsPanel.ResumeLayout();
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

    private static string SelectRandomIcon()
    {
        var icons = new List<FileInfo>(new DirectoryInfo(UnityEngine.Application.streamingAssetsPath).GetFiles()).Where((file) => file.Name.EndsWith(".ico")).ToList();
        return icons[new System.Random().Next(0, icons.Count)].Name;
    }

    public static void Demo()
    {
        Action displayMessageBox = null;
        displayMessageBox = () =>
        {
            MessageBox box = new MessageBox("Łiii", "Jeszcze raz?", new string[] { "Tak", "Nie", "Może", "Niech będzie", "A co mi tam...", "Dajesz!", "No kurwa", "Wincyj", "tych", "przycisków", "daj", "AAAAAAA", "お前はもう死んでいる" }, SelectRandomIcon());
            box.OnButtonClick += (string button) =>
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