using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BlackOps2SoundStudio.Converter;
using BlackOps2SoundStudio.Decoders;
using BlackOps2SoundStudio.Format;
using NAudio.Wave;

namespace BlackOps2SoundStudio
{
    public sealed partial class MainForm : Form
    {
        private SndAliasBank _sndAliasBank;
        private WaveOut _player;
        private Stream _audioStream;
        private DataGridViewRow _lastSelectedRow;
        private int _currentPlayIndex = -1;
        private string _currentPath;

        private WaveFileReader _wavFileReader;
        private Mp3FileReader _mp3FileReader;

        public MainForm()
        {
            InitializeComponent();

#if BUILD_XML
            SndAliasNameDatabase.BuildXML();
#endif

            Font = SystemFonts.MessageBoxFont;
            audioEntryOffsetColumn.ValueType = typeof (int);
            audioEntrySizeColumn.ValueType = typeof (int);
            headerPropertyGrid.ViewForeColor = Color.FromArgb(1, 0, 0);
            headerPropertyGrid.LineColor = Color.FromArgb(214, 219, 233);
            mainMenuStrip.Renderer = new ToolStripAeroRenderer(ToolbarTheme.Toolbar);
            audioEntryContextMenuStrip.Renderer = new ToolStripAeroRenderer(ToolbarTheme.Toolbar);
            audioEntriesDataGridView.DoubleBuffered(true);
        }

        private static void SetLabelColumnWidth(PropertyGrid grid, int width)
        {
            if (grid == null)
                return;

            FieldInfo fi = grid.GetType().GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                return;

            var view = fi.GetValue(grid) as Control;
            if (view == null)
                return;

            MethodInfo mi = view.GetType().GetMethod("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mi == null)
                return;

            mi.Invoke(view, new object[] { width });
        }

        private static string FormatTimeSpan(TimeSpan ts)
        {
            return string.Format("{0:D2}:{1:D2}", (int)ts.TotalMinutes, ts.Seconds);
        }

        private static string GetSizeReadable(long i)
        {
            string sign = (i < 0 ? "-" : "");
            double readable = (i < 0 ? -i : i);
            string suffix;
            if (i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = i >> 20;
            }
            else if (i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = i >> 10;
            }
            else if (i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString(sign + "0 B"); // Byte
            }
            readable = readable / 1024;

            return sign + readable.ToString("0.## ") + suffix;
        }

        private static string FormatDuration(TimeSpan duration)
        {
            if (duration.Hours >= 1)
                return string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);
            return string.Format("{0:D2}:{1:D2}", duration.Minutes, duration.Seconds);
        }

        private static string FormatSampleRate(int sampleRate)
        {
            return string.Format("{0} kHz", sampleRate/1000);
        }

        private void UpdateHeaderInformation()
        {
            var headerProperty = new HeaderProperties {
                Magic = _sndAliasBank.Magic,
                Version = _sndAliasBank.Version,
                SizeOfAudioEntry = _sndAliasBank.SizeOfAudioEntry,
                SizeOfHashEntry = _sndAliasBank.SizeOfChecksumEntry,
                SizeOfStringEntry = _sndAliasBank.SizeOfDependencyEntry,
                NumberOfAudioEntries = _sndAliasBank.Entries.Count,
                AssetLinkID = BitConverter.ToString(_sndAliasBank.AssetLinkIdentifier).Replace("-", ""),
            };

            var assetReferences = new List<string>();
            foreach (var assetReference in _sndAliasBank.AssetReferences)
            {
                if (string.IsNullOrEmpty(assetReference))
                    break;
                assetReferences.Add(assetReference);
            }
            headerProperty.AssetReferences = assetReferences;
            headerPropertyGrid.SelectedObject = headerProperty;
        }

        private void UpdateAudioEntries()
        {
            audioEntriesDataGridView.Rows.Clear();

            // Set autosize modes.
            for (int i = 0; i < audioEntriesDataGridView.Columns.Count - 1; i++)
            {
                if (audioEntriesDataGridView.Columns[i].HeaderText != "Sample Rate")
                    audioEntriesDataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                else
                    audioEntriesDataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            // Add the entries.
            DataGridViewRow[] rows = new DataGridViewRow[_sndAliasBank.Entries.Count];
            for (int i = 0; i < _sndAliasBank.Entries.Count; i++)
            {
                var entry = _sndAliasBank.Entries[i];
                string name;
                if (SndAliasNameDatabase.Names.TryGetValue(entry.Identifier, out name))
                    name = showFullNameToolStripMenuItem.Checked ? name : Path.GetFileName(name);
                else
                    name = "Sound #" + (i + 1) + SndAliasBankHelper.GetExtensionFromFormat(entry.Format);
                var row = new DataGridViewRow();
                row.CreateCells(audioEntriesDataGridView,
                    name,
                    entry.Offset,
                    entry.Size,
                    SndAliasBankHelper.GetFormatName(entry.Format),
                    entry.Loop,
                    entry.ChannelCount,
                    FormatSampleRate(entry.SampleRate),
                    BitConverter.ToString(_sndAliasBank.Checksums[i]).Replace("-", ""));
                row.ContextMenuStrip = audioEntryContextMenuStrip;
                row.Tag = entry;
                rows[i] = row;
            }

            audioEntriesDataGridView.Rows.AddRange(rows);

            // Fix to allow user to manually size.
            for (int i = 0; i < audioEntriesDataGridView.Columns.Count - 1; i++)
            {
                var column = audioEntriesDataGridView.Columns[i];
                if (column.AutoSizeMode != DataGridViewAutoSizeColumnMode.AllCells) continue;
                int width = column.GetPreferredWidth(column.AutoSizeMode, true);
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet;
                column.Width = width;
            }
        }

        private void Cleanup()
        {
#if BUILD_BO3
            SndAliasNameDatabase.BuildBO3();
#endif

            stopToolStripButton.PerformClick();
            if (_sndAliasBank == null)
                return;

            foreach (var entry in _sndAliasBank.Entries)
                entry.Data.Dispose();

            _sndAliasBank.Dispose();
            ConvertHelper.Cleanup();
        }

        private void LoadSound(string path)
        {
            try
            {
                // Read the specified file.
                Cleanup();
                audioEntriesDataGridView.Rows.Clear();
                headerPropertyGrid.SelectedObject = null;

                _sndAliasBank = SndAliasBankReader.Read(path);
                _currentPath = path;

                // Update the UI.
                Text = "Black Ops II Sound Studio by master131 - " + Path.GetFileName(path);
                UpdateHeaderInformation();
                UpdateAudioEntries();

                // Make the main controls visible.
                welcomeLabel.Visible = false;
                mainSplitContainer.Visible = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                refreshToolStripMenuItem.Enabled = true;
                SetLabelColumnWidth(headerPropertyGrid, 130);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Black Ops II Sound Studio", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Black Ops II/III Audio Files|*.sabs;*.sabl";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    LoadSound(openFileDialog.FileName);
            }
        }

        private void audioEntriesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            playToolStripButton.Enabled = true;
        }

        private void playToolStripButton_Click(object sender, EventArgs e)
        {
            // Resume the song if the song was paused.
            if (_player != null && _lastSelectedRow == audioEntriesDataGridView.SelectedRows[0])
            {
                if (_player.PlaybackState == PlaybackState.Paused)
                    _player.Play();
                return;
            }

            // Stop the current song and free resources.
            stopToolStripButton.PerformClick();
            audioEntriesDataGridView.Enabled = false;
            playToolStripButton.Enabled = false;
            mainMenuStrip.Enabled = false;

            // Find the current selected entry.
            var entry = (SndAssetBankEntry) (_lastSelectedRow = audioEntriesDataGridView.SelectedRows[0]).Tag;

            ThreadPool.QueueUserWorkItem(x =>
            {
                // Begin decoding or create a compatible IWaveProvider interface.
                IWaveProvider provider = null;

                if (entry.Format == AudioFormat.FLAC)
                {
                    _audioStream = DecodeFLAC(entry);

                    if (_audioStream != null)
                    {
                        _wavFileReader = new WaveFileReader(_audioStream);
                        provider = _wavFileReader;
                    }
                    else
                    {
                        MessageBox.Show("Something happened while trying to decode/prepare the audio.",
                            "Black Ops 2 Sound Studio", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        _currentPlayIndex = -1;
                        Invoke(new MethodInvoker(() =>
                        {
                            currentTimetoolStripLabel.Text = "";
                            playToolStripButton.Enabled = true;
                            audioEntriesDataGridView.Enabled = true;
                            mainMenuStrip.Enabled = true;
                        }));
                        return;
                    }
                }
                else if (entry.Format == AudioFormat.PCMS16)
                {
                    _audioStream = SndAliasBankHelper.DecodeHeaderlessWav(entry);
                    _wavFileReader = new WaveFileReader(_audioStream);
                    provider = _wavFileReader;
                }
                else if (entry.Format == AudioFormat.MP3)
                {
                    _audioStream = new MemoryStream(entry.Data.Get());
                    _mp3FileReader = new Mp3FileReader(_audioStream);
                    provider = _mp3FileReader;
                }
                else if (entry.Format == AudioFormat.XMA4)
                {
                    Invoke(new MethodInvoker(() => currentTimetoolStripLabel.Text = "Please wait, decoding audio..."));
                    
                    using (var xmaStream = SndAliasBankHelper.AddXMAHeader(entry))
                        _audioStream = ConvertHelper.ConvertXMAToWAV(xmaStream);

                    if (_audioStream != null)
                    {
                        _wavFileReader = new WaveFileReader(_audioStream);
                        provider = _wavFileReader;
                    }
                }
                
                // Begin playing the audio on the UI thread.
                Invoke(new MethodInvoker(() =>
                {
                    currentTimetoolStripLabel.Text = "";
                    playToolStripButton.Enabled = true;
                    audioEntriesDataGridView.Enabled = true;
                    mainMenuStrip.Enabled = true;

                    // Ensure a valid provided was provided.
                    if (provider != null)
                    {
                        pauseToolStripButton.Enabled = true;
                        stopToolStripButton.Enabled = true;

                        _player = new WaveOut();
                        _player.Init(provider);
                        playerTimeTimer.Start();
                        _player.PlaybackStopped += _player_PlaybackStopped;
                        _player.Play();
                    }
                    else
                    {
                        MessageBox.Show("Playback for the selected format is not supported or audio decoding failed.",
                                        "Black Ops 2 Sound Studio", MessageBoxButtons.OK,
                                        MessageBoxIcon.Exclamation);
                    }
                }));
            });
        }

        void _player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            // Stop and free resources on playback completion.
            var index = _currentPlayIndex;
            stopToolStripButton.PerformClick();

            // Play the next audio if supported.
            if (index != -1 && index < audioEntriesDataGridView.Rows.Count - 1)
            {
                audioEntriesDataGridView.Rows[index].Selected = false;
                audioEntriesDataGridView.Rows[++index].Selected = true;
                _currentPlayIndex = index;
                playToolStripButton.PerformClick();
            }
        }

        private Stream DecodeFLAC(SndAssetBankEntry entry)
        {
            try
            {
                var wavOutput = new MemoryStream();
                using (var input = new MemoryStream(entry.Data.Get()))
                using (var wavWriter = new WavWriter(wavOutput))
                using (var reader = new FlacReader(input, wavWriter))
                {
                    reader.SampleProcessed += (s, args) =>
                        BeginInvoke(new MethodInvoker(() =>
                            currentTimetoolStripLabel.Text = "Please wait, decoding audio... " + args.Progress + "%"));
                    reader.Process();
                }
                wavOutput.Position = 0;
                return wavOutput;
            }
            catch
            {
                return null;
            }
        }

        private void pauseToolStripButton_Click(object sender, EventArgs e)
        {
            _player.Pause();
        }

        private void stopToolStripButton_Click(object sender, EventArgs e)
        {
            // Disable updates of song timings.
            playerTimeTimer.Stop();
            _currentPlayIndex = -1;

            // Free resources.
            if (_player != null)
            {
                _player.Stop();
                _player.Dispose();
                _player = null;
            }

            if (_wavFileReader != null)
            {
                _wavFileReader.Close();
                _wavFileReader = null;
            }

            if (_mp3FileReader != null)
            {
                _mp3FileReader.Close();
                _mp3FileReader = null;
            }

            if (_audioStream != null)
            {
                _audioStream.Close();
                _audioStream = null;
            }

            // Update UI.
            stopToolStripButton.Enabled = false;
            pauseToolStripButton.Enabled = false;
            currentTimetoolStripLabel.Text = "";
            totalTimeToolStripLabel.Text = "";
        }

        private void playerTimeTimer_Tick(object sender, EventArgs e)
        {
            if (_wavFileReader != null)
            {
                currentTimetoolStripLabel.Text = "Current Time: " + FormatTimeSpan(_wavFileReader.CurrentTime);
                totalTimeToolStripLabel.Text = "Total Time: " + FormatTimeSpan(_wavFileReader.TotalTime);
            }
            else if (_mp3FileReader != null)
            {
                currentTimetoolStripLabel.Text = "Current Time: " + FormatTimeSpan(_mp3FileReader.CurrentTime);
                totalTimeToolStripLabel.Text = "Total Time: " + FormatTimeSpan(_mp3FileReader.TotalTime);
            }
        }

        private void audioEntriesDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
                return;

            // Format the size and offsets here to allow proper column sorting.
            if (e.ColumnIndex == 1)
            {
                e.Value = "0x" + ((long)e.Value).ToString("X");
                e.FormattingApplied = true;
            }
            else if (e.ColumnIndex == 2)
            {
                e.Value = GetSizeReadable((int)e.Value);
                e.FormattingApplied = true;
            }
        }

        private void headerPropertyGrid_Resize(object sender, EventArgs e)
        {
            SetLabelColumnWidth(headerPropertyGrid, 130);
        }

        private void audioEntriesDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
                audioEntriesDataGridView.Rows[e.RowIndex].Selected = true;
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playToolStripButton.PerformClick();
        }

        private void playAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentPlayIndex = audioEntriesDataGridView.SelectedRows[0].Index;
            playToolStripButton.PerformClick();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new AboutForm())
                form.ShowDialog();
        }

        private void OnAudioReplaced(SndAssetBankEntry entry, Stream newData)
        {
            if (newData == null || newData.Length == 0)
            {
                MessageBox.Show("Conversion failed, ensure the file you provided is valid.",
                                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (newData != null) newData.Close();
                return;
            }

            if (entry.Format == AudioFormat.PCMS16)
            {
                var sampleCount = newData.Length / (entry.ChannelCount * 2);
                var duration = TimeSpan.FromMilliseconds(1000 * (float) sampleCount / entry.SampleRate);
                if (duration > entry.Duration && duration.Subtract(entry.Duration).TotalSeconds >= 1)
                {
                    if (MessageBox.Show(
                        "Duration mismatch detected. This is known to cause loading problems on single-player missions " +
                        "and has not been tested on multiplayer.\n\n" +
                        "Original Duration: " + FormatDuration(entry.Duration) + "\n" +
                        "New Duration: " + FormatDuration(duration) + "\n\n" +
                        "Are you sure you want to continue?",
                        "Black Ops II Sound Studio", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        newData.Close();
                        return;
                    }
                }
                entry.Duration = duration;
            }
            else
            {
                var helper = new ConvertHelper();
                entry.Duration = helper.GetDuration(newData);
            }

            entry.Data = new AudioDataStream(newData, (int) newData.Length);

            foreach (DataGridViewRow row in audioEntriesDataGridView.Rows)
            {
                if (row.Tag != entry) continue;
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style.BackColor = Color.LightBlue;
                MessageBox.Show("Audio data has been successfully replaced.",
                                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
            }
        }

        private void ReplaceAudio(SndAssetBankEntry entry, string inputFile)
        {
            // Begin conversion here.
            var options = new ConvertOptions { AudioChannels = entry.ChannelCount, SampleRate = entry.SampleRate };
            OnAudioReplaced(entry, ConvertProgressForm.Convert(inputFile, entry.Format, options));
        }

        private void replaceAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ensure FFmpeg binaries are present before attempting to continue.
            if (!ConvertHelper.CanConvert())
            {
                MessageBox.Show("FFmpeg was not found, please re-download Black Ops II Sound Studio.",
                                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var entry = (SndAssetBankEntry) audioEntriesDataGridView.SelectedRows[0].Tag;

            if (entry.Format == AudioFormat.XMA4)
            {
                MessageBox.Show("Audio replacement for this format is currently not supported.",
                                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All supported formats|*.mp3;*.wav;*.flac;*.ogg;*.m4a;*.wma;*.avi;*.flv;*.mp4;*.webm;*.mkv;*.wmv;*.3gp|All files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    ReplaceAudio(entry, openFileDialog.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_sndAliasBank.Version == SndAliasConstants.Version_T7)
            {
                MessageBox.Show("Save support has been disabled for Black Ops III.",
                    "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                _sndAliasBank.Save(true, fixChecksumsToolStripMenuItem.Checked);
                MessageBox.Show("File has been successfully saved.", "Black Ops II Sound Studio",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Black Ops II Sound Studio",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRow = audioEntriesDataGridView.SelectedRows[0];
            var name = selectedRow.Cells[0].Value.ToString();
            var entry = (SndAssetBankEntry) selectedRow.Tag;

            using (var saveFileDialog = new SaveFileDialog())
            {
                var extension = SndAliasBankHelper.GetExtensionFromFormat(entry.Format);
                if (string.IsNullOrEmpty(extension))
                    saveFileDialog.Filter = "All Files|*.*";
                else
                    saveFileDialog.Filter = extension.Substring(1).ToUpperInvariant() + " Files|*" + extension;

                if (!name.EndsWith(extension)) name += extension;
                saveFileDialog.FileName = name;

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var audioStream = SndAliasBankHelper.GetAudioStreamFromEntry(entry))
                    using (var fs = new FileStream(saveFileDialog.FileName, FileMode.Create,
                        FileAccess.Write, FileShare.Read))
                        audioStream.CopyTo(fs);

                    MessageBox.Show(Path.GetFileName(saveFileDialog.FileName) + " has been exported successfully.",
                                    "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Black Ops II Sound Studio",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void exportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool useOriginalTreeStructure = MessageBox.Show("Do you want to use the original tree structure used by Treyarch?",
                "Black Ops II Sound Studio", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes;

            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select a directory to export to:";
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;

                // Export.
                ExportProgressForm.Export(_sndAliasBank, useOriginalTreeStructure, folderBrowserDialog.SelectedPath);
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadSound(_currentPath);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cleanup();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_sndAliasBank.Version == SndAliasConstants.Version_T7)
            {
                MessageBox.Show("Save support has been disabled for Black Ops III.",
                    "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                var extension = Path.GetExtension(_currentPath);
                if (string.IsNullOrEmpty(extension))
                    saveFileDialog.Filter = "All Files|*.*";
                else
                    saveFileDialog.Filter = extension.Substring(1).ToUpperInvariant() + " Files|*" + extension;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    _sndAliasBank.Save(saveFileDialog.FileName, true);
                    _currentPath = saveFileDialog.FileName;
                    Text = "Black Ops II Sound Studio by master131 - " + Path.GetFileName(_currentPath);
                    MessageBox.Show("File has been successfully saved.", "Black Ops II Sound Studio",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Black Ops II Sound Studio",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.None;
                LoadSound(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
            }
        }

        private void showFullNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in audioEntriesDataGridView.Rows)
            {
                var entry = (SndAssetBankEntry) row.Tag;
                string name;
                if (SndAliasNameDatabase.Names.TryGetValue(entry.Identifier, out name))
                    row.Cells[0].Value = showFullNameToolStripMenuItem.Checked ? name : Path.GetFileName(name);
            }
        }
    }
}
