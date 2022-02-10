using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using BlackOps2SoundStudio.Format;

namespace BlackOps2SoundStudio
{
    public partial class ExportProgressForm : Form
    {
        private SndAliasBank _sndAliasBank;
        private bool _useOriginalTreeStructure;
        private string _outputDirectory;

        public ExportProgressForm()
        {
            InitializeComponent();
        }

        private void SetSndAliasBank(SndAliasBank sndAliasBank, bool useOriginalTreeStructure, string outputDirectory)
        {
            _sndAliasBank = sndAliasBank;
            _useOriginalTreeStructure = useOriginalTreeStructure;
            _outputDirectory = outputDirectory;
        }

        internal static void Export(SndAliasBank sndAliasBank, bool useOriginalTreeStructure, string outputDirectory)
        {
            var form = new ExportProgressForm();
            form.SetSndAliasBank(sndAliasBank, useOriginalTreeStructure, outputDirectory);
            form.ShowDialog();
        }

        private void exportBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < _sndAliasBank.Entries.Count; i++)
            {
                if (exportBackgroundWorker.CancellationPending)
                    return;

                var entry = _sndAliasBank.Entries[i];
                string relativePath;
                if (!_useOriginalTreeStructure || !SndAliasNameDatabase.Names.TryGetValue(entry.Identifier, out relativePath))
                    relativePath = "Sound #" + (i + 1) + SndAliasBankHelper.GetExtensionFromFormat(entry.Format);
                else
                    relativePath += SndAliasBankHelper.GetExtensionFromFormat(entry.Format);

                using (var audioStream = SndAliasBankHelper.GetAudioStreamFromEntry(entry))
                {
                    try
                    {
                        var fullPath = Path.Combine(_outputDirectory, relativePath);
                        string directoryPath;
                        if (relativePath.IndexOf('\\') != -1 && !Directory.Exists((directoryPath = Path.GetDirectoryName(fullPath))))
                            Directory.CreateDirectory(directoryPath);
                        using (var fs = new FileStream(fullPath, FileMode.Create,
                            FileAccess.Write, FileShare.Read))
                            audioStream.CopyTo(fs);
                        exportBackgroundWorker.ReportProgress((i + 1) * 100 / _sndAliasBank.Entries.Count, Path.GetFileName(fullPath));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Black Ops II Sound Studio",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            MessageBox.Show("All audio entries have been exported successfully.",
                            "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportProgressForm_Load(object sender, EventArgs e)
        {
            exportBackgroundWorker.RunWorkerAsync();
        }

        private void exportBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            exportProgressBar.Value = e.ProgressPercentage;
            exportLabel.Text = "Exporting " + e.UserState + "... " + e.ProgressPercentage + "%";
        }

        private void ExportProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                exportBackgroundWorker.CancelAsync();
        }

        private void exportBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }
    }
}
