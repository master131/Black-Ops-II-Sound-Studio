using BlackOps2SoundStudio;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Black_Ops_II_Sound_Studio_Extended
{
    public partial class ReplaceAllForm : Form
    {
        private MainForm mainForm;
        public CancellationTokenSource tokenSource;
        public IProgress<int> overallProgress;
        public IProgress<int> conversionProgress;
        public IProgress<string> reportConsole;
        public IProgress<int> matchCountReporter;

        public string path;
        public string source;
        public string target;
        public string extension;
        public bool stopWhenNoMatch;
        public bool stopWhenReplaceFails;
        public bool applyDupFix;
        public bool adaptFileNames;

        Task replaceTask;

        public ReplaceAllForm()
        {
            InitializeComponent();
        }

        public ReplaceAllForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(ManagerClosing);
        }

        private void openFolderButton_Click(object sender, EventArgs e)
        {
            bool running = replaceTask != null && !replaceTask.IsCompleted;
            if (!running)
            {
                string dir = this.openDirectory();
                if (!String.IsNullOrEmpty(dir))
                {
                    this.folderTextBox.Text = dir;
                    this.sourceComboBox.SelectedIndex = 1;
                    this.targetComboBox.SelectedIndex = 0;
                    //this.extensionComboBox.SelectedIndex = 13;
                }
            }
        }

        private string openDirectory()
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                //dialog.InitialDirectory = "C:\\Users";
                //dialog.Filters.Add(new CommonFileDialogFilter("Audio", "*.mp3;*.wav;*.flac;*.ogg;*.m4a;*.wma;*.xma"));
                //dialog.Filters.Add(new CommonFileDialogFilter("Video", "*.avi;*.flv;*.mp4;*.webm;*.mkv;*.wmv;*.3gp"));
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    return dialog.FileName;
                else
                    return null;
            }
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            // retrieve options for matching and replacing
            stopWhenNoMatch = this.stopWhenNoMatchCheckBox.Checked;
            stopWhenReplaceFails = this.stopWhenReplaceFailsComboBox.Checked;
            applyDupFix = this.dupFixCheckBox.Checked;
            adaptFileNames = this.adaptNamesCheckBox.Checked;

            // check if a folder was selected
            path = this.folderTextBox.Text;
            if (String.IsNullOrEmpty(path.Trim()))
            {
                MessageBox.Show("Choose a folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // retrieve audio files
            IEnumerable<string> files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);
            int filesCount = files.Count();
            if (filesCount == 0)
            {
                MessageBox.Show("No audio files found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // check if the source platform was selected
            if (adaptFileNames && (this.sourceComboBox.SelectedItem == null))
            {
                MessageBox.Show("Choose a source platform.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 
            else
            {
                // adapt to internal naming
                source = this.sourceComboBox.SelectedItem as string;
                switch (source)
                {
                    case "PC":
                        source = "pc";
                        break;
                    case "Xbox 360":
                        source = "xenon";
                        break;
                }
            }

            // check if the target platform was selected
            if (adaptFileNames && (this.targetComboBox.SelectedItem == null))
            {
                MessageBox.Show("Choose a target platform.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else
            {
                // adapt to internal naming
                target = this.targetComboBox.SelectedItem as string;
                switch (target)
                {
                    case "PC":
                        target = "pc";
                        break;
                    case "Xbox 360":
                        target = "xenon";
                        break;
                }
            }


            // prepare token for cancellation so the process can be stopped
            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            // prepare variables for the progress bars
            overallProgressBar.Minimum = 0;
            overallProgressBar.Value = 0;
            conversionProgressBar.Minimum = 0;
            conversionProgressBar.Maximum = 100;
            conversionProgressBar.Value = 0;
            conversionProgressLabel.Text = "0%";
            overallProgress = new Progress<int>(replaceCount =>
            {
                overallProgressBar.Value = replaceCount;
                overallProgressLabel.Text = $"Replaced {replaceCount} of {overallProgressBar.Maximum} files";
            });
            matchCountReporter = new Progress<int>(fileCount =>
            {
                overallProgressBar.Maximum = fileCount;
            });
            conversionProgress = new Progress<int>(percent =>
            {
                percent = percent > 100 ? 100 : percent;
                conversionProgressBar.Value = percent;
                conversionProgressLabel.Text = $"{percent}%";
            });
            reportConsole = new Progress<string>(text =>
            {
                if (!this.IsDisposed)
                {
                    reportRichTextBox.AppendText(text);
                    //reportRichTextBox.ScrollToCaret();
                }
            });

            // disable start button until all process is finished
            startButton.Enabled = false;

            // init replacing process
            try
            {
                replaceTask = Task.Run(() => mainForm.startReplaceAll(this), token);
                await replaceTask;
                overallProgressBar.Value = 0;
                conversionProgressBar.Value = 0;
            } 
            catch (OperationCanceledException)
            {
                MessageBox.Show("Operation cancelled.", "Cancelled.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
            finally
            {
                tokenSource.Dispose();
                startButton.Enabled = true;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            bool running = replaceTask != null && !replaceTask.IsCompleted;
            if (running)
            {
                DialogResult r = MessageBox.Show("Are you sure you want to stop?",
                    "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (r == DialogResult.OK)
                    stopTask();
            }
        }

        private void stopTask()
        {
            try
            {
                if (tokenSource != null)
                    tokenSource.Cancel();
            }
            catch (ObjectDisposedException)
            {

            }
        }

        private void ManagerClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool running = replaceTask != null && !replaceTask.IsCompleted;
            if (running)
            {
                DialogResult r = MessageBox.Show("There's a conversion in progress. Are you sure you want to cancel it?",
                    "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (r == DialogResult.OK)
                {
                    stopTask();
                }
                else
                    e.Cancel = true;
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Title = "Export to a text file.";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, reportRichTextBox.Text.Replace("\n", Environment.NewLine));
            }
        }

        private void adaptNamesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            nameAdaptingGroupBox.Enabled = adaptNamesCheckBox.Checked;
        }
    }
}
