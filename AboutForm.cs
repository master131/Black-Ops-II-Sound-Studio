using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using BlackOps2SoundStudio.Properties;

namespace BlackOps2SoundStudio
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            aboutRichTextBox.Rtf = Resources.About;

            // DPI clipping fix.
            using (var g = CreateGraphics())
                if (g.DpiX > 96)
                    Recurse(this);

            var v = typeof(AboutForm).Assembly.GetName().Version;
            versionLabel.Text = "Version " + v.Major + "." + v.Minor + "." + v.Build;
        }

        private void Recurse(Control control)
        {
            foreach (Control child in control.Controls)
                Recurse(child);

            var label = control as Label;
            if (label != null)
                label.Font = new Font(label.Font.FontFamily, label.Font.Size * 0.75f, label.Font.Style);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
