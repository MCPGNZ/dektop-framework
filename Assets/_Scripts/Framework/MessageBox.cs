using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UnityEngine;

public class MessageBox : Form
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private Label message = null;
    private PictureBox icon = null;
    private Button[] buttons = null;

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

        this.message = new Label();
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

        this.SuspendLayout();

        this.message.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                    | AnchorStyles.Left)
                    | AnchorStyles.Right)));
        this.message.AutoSize = true;
        this.message.Location = new System.Drawing.Point(85, 15);
        this.message.MaximumSize = new System.Drawing.Size(294, 0);
        this.message.Name = "message";
        this.message.Size = new System.Drawing.Size(28, 13);
        this.message.TabIndex = 0;
        this.message.Text = message;

        if (this.icon != null)
        {
            this.icon.ErrorImage = null;
            this.icon.InitialImage = null;
            this.icon.ImageLocation = iconPath;
            this.icon.Location = new System.Drawing.Point(15, 15);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(64, 64);
            this.icon.SizeMode = PictureBoxSizeMode.Zoom;
            this.icon.TabIndex = 0;
            this.icon.TabStop = false;
        }

        for (int i = 0; i < buttonLabels.Length; ++i)
        {
            Button btn = buttons[i];
            string label = buttonLabels[i];

            btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //btn.FocusDuesEnabled = false;
            btn.Location = new System.Drawing.Point(139 + i * 81, 88);
            btn.Name = "btn_" + label;
            btn.Size = new System.Drawing.Size(75, 23);
            btn.TabIndex = 2;
            btn.Text = label;
            //btn.Tooltip = "";
            btn.UseVisualStyleBackColor = true;
            btn.Click += new EventHandler((object sender, EventArgs args) => {
                this.OnButtonClick?.Invoke(label);
                this.Dispose();
            });
        }

        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(this.buttons.Length * 81 + 151, 129);
        this.Controls.Add(this.message);
        this.Controls.Add(this.icon);
        foreach (Button button in buttons)
        {
            this.Controls.Add(button);
        }
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ControlBox = false;
        this.Name = "MessageForm";
        this.Padding = new System.Windows.Forms.Padding(15);
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = title;
        if (this.icon != null)
        {
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.icon.Load();
        }
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