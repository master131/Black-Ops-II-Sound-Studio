using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BlackOps2SoundStudio.Converter;
using BlackOps2SoundStudio.Decoders;
using BlackOps2SoundStudio.Encoders;
using BlackOps2SoundStudio.Format;
using NAudio.Wave;
using Microsoft.WindowsAPICodePack.Dialogs;
using Black_Ops_II_Sound_Studio_Extended;

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
        private Boolean skipConversion = false;

        private WaveFileReader _wavFileReader;
        private Mp3FileReader _mp3FileReader;

        public MainForm()
        {
            InitializeComponent();

#if BUILD_XML
            SndAliasNameDatabase.BuildXML();
#endif

            Font = SystemFonts.MessageBoxFont;
            audioEntryOffsetColumn.ValueType = typeof(int);
            audioEntrySizeColumn.ValueType = typeof(int);
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
            return string.Format("{0} kHz", sampleRate / 1000);
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

        private string GetNameForEntry(SndAssetBankEntry entry, int index) {
            string name;
            if (SndAliasNameDatabase.Names.TryGetValue(entry.Identifier, out name))
                name = showFullNameToolStripMenuItem.Checked ? name : Path.GetFileName(name);
            else
                name = "Sound #" + (index + 1) + SndAliasBankHelper.GetExtensionFromFormat(entry.Format);
            return name;
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
                var name = GetNameForEntry(entry, i);
                var row = new DataGridViewRow();
                row.CreateCells(audioEntriesDataGridView,
                    name,
                    entry.Offset,
                    entry.Size,
                    SndAliasBankHelper.GetFormatName(entry.Format),
                    entry.Loop,
                    entry.ChannelCount,
                    FormatSampleRate(entry.SampleRate),
                    BitConverter.ToString(_sndAliasBank.Checksums[i]).Replace("-", ""),
                    entry.replaced);
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
                forceChangeFormatToolStripMenuItem.Enabled = true;
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
            var entry = (SndAssetBankEntry)(_lastSelectedRow = audioEntriesDataGridView.SelectedRows[0]).Tag;

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
            var wavOutput = new MemoryStream();
            try
            {
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
            catch (TypeInitializationException e)
            {
                Exception inner1 = e.InnerException;
                if (inner1 != null)
                {
                    Exception inner2 = inner1.InnerException;
                    if (inner2 != null)
                        MessageBox.Show(inner2.Message,
                                    "Black Ops 2 Sound Studio", MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                }
                return null;
            }
            catch (Exception e)
            {
                // Ignore exception for headerless FLAC
                if (e.Message.IndexOf("until eof", StringComparison.InvariantCulture) != -1 &&
                    entry.Data is AudioDataStream && ((AudioDataStream)entry.Data).Stream is HeaderlessFLACStream)
                {
                    wavOutput.Position = 0;
                    return wavOutput;
                }
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
                var duration = TimeSpan.FromMilliseconds(1000 * (float)sampleCount / entry.SampleRate);
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

            entry.Data = new AudioDataStream(newData, (int)newData.Length);
            entry.replaced = true;

            foreach (DataGridViewRow row in audioEntriesDataGridView.Rows)
            {
                if (row.Tag != entry) continue;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = Color.LightBlue;
                }
                row.Cells[audioEntryReplacedColumn.Index].Value = entry.replaced;
                MessageBox.Show("Audio data has been successfully replaced.",
                                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
            }
        }

        private void ReplaceAudio(SndAssetBankEntry entry, string inputFile)
        {
            if (skipConversion)
            {
                using (var reader = new AudioFileReader(inputFile))
                {
                    // Get audio info
                    String inputExtension = (new FileInfo(inputFile)).Extension;
                    int inputSampleRate = reader.WaveFormat.SampleRate;
                    int inputChannels = reader.WaveFormat.Channels;

                    // Check if input file matches target entry
                    if (!inputExtension.Equals("." + SndAssetBankEntry.formatToString(entry.Format)))
                    {
                        MessageBox.Show("Input audio file has a different format than the target audio. Disable Skip Conversion or convert it manually.",
                                    "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (inputSampleRate != entry.SampleRate)
                    {
                        MessageBox.Show("Input audio file has a different sample rate than the target audio. Disable Skip Conversion or convert it manually.",
                                    "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (inputChannels != entry.ChannelCount)
                    {
                        MessageBox.Show("Input audio file has a different channel count than the target audio. Disable Skip Conversion or convert it manually.",
                                    "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Create audio stream and replace
                using (FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    Stream audioCopy = copyFile(fs);
                    OnAudioReplaced(entry, audioCopy);
                }
            }
            else
            {
                // Begin conversion here.
                var options = new ConvertOptions { AudioChannels = entry.ChannelCount, SampleRate = entry.SampleRate };
                OnAudioReplaced(entry, ConvertProgressForm.Convert(inputFile, entry.Format, options));
            }
        }

        private Stream copyFile(FileStream reader)
        {
            var temp = Path.GetTempFileName();
            var fs = File.Open(temp, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            ConvertHelper.TemporaryFiles.Add(temp);
            int bytesRead;
            var buffer = new byte[1024 * 1024];
            //var buffer = new byte[1 * reader.WaveFormat.AverageBytesPerSecond];
            //byte[] bytes = new byte[1 * reader.WaveFormat.AverageBytesPerSecond];

            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }

            return fs;
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

            var entry = (SndAssetBankEntry)audioEntriesDataGridView.SelectedRows[0].Tag;

            if (entry.Format == AudioFormat.XMA4)
            {
                MessageBox.Show("Audio replacement for this format is currently not supported.",
                                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All supported formats|*.mp3;*.wav;*.flac;*.ogg;*.m4a;*.wma;*.avi;*.flv;*.mp4;*.webm;*.mkv;*.wmv;*.3gp;*.xma|All files|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    ReplaceAudio(entry, openFileDialog.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (_sndAliasBank.Version != SndAliasConstants.Version_T6)
            //{
            //    MessageBox.Show("Save is only supported for Black Ops II.",
            //        "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

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
            var entry = (SndAssetBankEntry)selectedRow.Tag;

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
            //if (_sndAliasBank.Version != SndAliasConstants.Version_T6)
            //{
            //    MessageBox.Show("Save is only supported for Black Ops II.",
            //        "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

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
                var entry = (SndAssetBankEntry)row.Tag;
                string name;
                if (SndAliasNameDatabase.Names.TryGetValue(entry.Identifier, out name))
                    row.Cells[0].Value = showFullNameToolStripMenuItem.Checked ? name : Path.GetFileName(name);
            }
        }

        private void forceFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to change the format recognised by Black Ops II Sound Studio? " +
                                "This is usually something you do not want to do unless you are working with pre-production sound files.",
                "Black Ops II Sound Studio", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) {
                var targetFormat = ((ToolStripMenuItem)sender).Text;
                AudioFormat format;
                switch (targetFormat) {
                    case "PCM":
                        format = AudioFormat.PCMS16;
                        break;
                    case "XMA":
                        format = AudioFormat.XMA4;
                        break;
                    case "MP3":
                        format = AudioFormat.MP3;
                        break;
                    case "FLAC":
                        format = AudioFormat.FLAC;
                        break;
                    default:
                        return;
                }

                var entries = audioEntriesDataGridView.Rows
                    .Cast<DataGridViewRow>()
                    .Select(r => (SndAssetBankEntry)r.Tag)
                    .OrderBy(r => r.Offset)
                    .ToList();

                foreach (DataGridViewRow row in audioEntriesDataGridView.Rows) {
                    var entry = (SndAssetBankEntry)row.Tag;
                    entry.Format = format;
                    row.Cells[0].Value = GetNameForEntry(entry, entries.IndexOf(entry));
                    row.Cells[3].Value = SndAliasBankHelper.GetFormatName(format);
                }

                audioEntriesDataGridView.Refresh();
            }
        }

        public void startReplaceAll(ReplaceAllForm replaceAllManager)
        {
            // unpack Replace Manager attributes
            string path = replaceAllManager.path;
            string source = replaceAllManager.source;
            string target = replaceAllManager.target;
            bool stopWhenNoMatch = replaceAllManager.stopWhenNoMatch;
            bool stopWhenReplaceFails = replaceAllManager.stopWhenReplaceFails;
            bool applyDupFix = replaceAllManager.applyDupFix;
            bool adaptNameFiles = replaceAllManager.adaptFileNames;
            IProgress<int> overallProgressValue = replaceAllManager.overallProgress;
            IProgress<string> reportConsole = replaceAllManager.reportConsole;
            IProgress<int> matchCountReporter = replaceAllManager.matchCountReporter;

            // load all file paths
            IEnumerable<string> files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);
            int fileCount = files.Count();
            reportConsole.Report("Files found: " + fileCount + "\n");

            // find matching snd files
            reportConsole.Report("--------------------------\n"); 
            reportConsole.Report("MATCHING RESULTS\n");
            reportConsole.Report("Matching... (this may take long)\n");
            Dictionary<SndAssetBankEntry, string> matchedEntries = new Dictionary<SndAssetBankEntry, string>();
            Dictionary< string, string> unmatchedEntries = new Dictionary<string, string>();

            int duplicatedFiles = 0;
            foreach (string filepath in files)
            {
                // set cancelling condition, only possible to apply before a matching step starts
                if (replaceAllManager.tokenSource.Token.IsCancellationRequested)
                {
                    reportConsole.Report("Cancelling...\n");
                    replaceAllManager.tokenSource.Token.ThrowIfCancellationRequested();
                }

                
                string matchingName = Path.GetFileNameWithoutExtension(filepath); // remove filepath, only leaving the filename
                SndAssetBankEntry snd = null;
                bool thereAreEntriesLeft = matchedEntries.Count < _sndAliasBank.Entries.Count;

                // perform lookup (only when there are still entries left)
                if (adaptNameFiles && thereAreEntriesLeft)
                {
                    // adjust the filepath to match the target file pattern
                    matchingName = matchingName.Replace("." + source, "." + target); // change the platform
                        //.Replace(Path.GetExtension(filepath), ""); // remove file extension
                    snd = this.GetEntryForName(matchingName);
                } 
                else if (thereAreEntriesLeft)
                {
                    snd = this.GetEntryForNameOnTable(matchingName);
                }
                else
                {
                    reportConsole.Report("All sound files were already matched. Skipping all files left.\n");
                    break;
                }

                // skip duplicated files
                if (snd != null && (matchedEntries.ContainsKey(snd) || unmatchedEntries.ContainsKey(filepath)))
                {
                    reportConsole.Report($"Skipping duplicated file: {filepath}\n");
                    duplicatedFiles++;
                    continue;
                }

                if (snd != null)
                {
                    matchedEntries.Add(snd, filepath);
                    //reportConsole.Report(Path.GetFileName(filepath) + " -> snd(" + snd.Identifier + "," + this.GetNameForEntry(snd, 0) + ")\n");
                }
                else
                {
                    reportConsole.Report("Couldn't match " + matchingName + "\n");
                    //unmatchedEntries.Add(filepath, matchingName);
                }
            }

            reportConsole.Report("Finished matching files.\n");

            // create extra entry for duplicated voicelines (not to confuse with duplicated files)
            int dupCount = 0;
            int skippedDups = 0;
            if (adaptNameFiles && applyDupFix)
            {
                reportConsole.Report("Dup fix will be applied. All files containing '_m_' will have repeated entries using '_s_' instead.\n");
                reportConsole.Report("Creating matching dups...\n");
                // look for files that can be duplicated
                foreach (string filepath in files)
                {
                    if (filepath.Contains("_m_"))
                    {
                        // set cancel condition, only possible before a matching process starts
                        if (replaceAllManager.tokenSource.Token.IsCancellationRequested)
                        {
                            reportConsole.Report("Cancelling...\n");
                            replaceAllManager.tokenSource.Token.ThrowIfCancellationRequested();
                        }

                        // create fake filepath and adjust it to match a real sound file
                        string dupFilepath = filepath.Replace("_m_", "_s_");
                        string dupMatchingName = Path.GetFileName(dupFilepath) // remove filepath, only leaving the filename
                            .Replace("." + source, "." + target) // change the platform
                            .Replace(Path.GetExtension(dupFilepath), ""); // remove file extension

                        // perform lookup and save (only when there are still entries left)
                        SndAssetBankEntry dupSnd = null;
                        if (matchedEntries.Count < _sndAliasBank.Entries.Count)
                            dupSnd = this.GetEntryForName(dupMatchingName);
                        else
                        {
                            reportConsole.Report("All sound files were already matched. Skipping all dups left.\n");
                            break;
                        }

                        // check if the real file has already been tested to avoid duplicated entry conflicts (ironic...)
                        if (dupSnd != null && (matchedEntries.ContainsKey(dupSnd) || unmatchedEntries.ContainsKey(filepath)))
                        {
                            skippedDups++;
                            reportConsole.Report("Dup file already exists -> ");
                            reportConsole.Report($"Generated \"{Path.GetFileName(dupFilepath)}\" from \"{filepath}\". Skipping...\n");
                            continue;
                        }

                        if (dupSnd != null)
                        {
                            dupCount++;
                            // add new entry but using the unmodified filepath (the "_m_" version) so it can be duplicated
                            matchedEntries.Add(dupSnd, filepath);
                            //reportConsole.Report(Path.GetFileName(filepath) + " -> snd(" + dupSnd.Identifier + "," + this.GetNameForEntry(dupSnd, 0) + ")\n");
                        }
                        else
                            unmatchedEntries.Add(filepath, dupMatchingName);
                    }
                }
            }

            if (adaptNameFiles && applyDupFix)
                reportConsole.Report("Finished generating and matching dups.\n");

            // report unmatched entries
            if (unmatchedEntries.Count > 0)
            {
                reportConsole.Report("Not all entries matched.\n");
                if (matchedEntries.Count < _sndAliasBank.Entries.Count)
                    reportConsole.Report($"Unmatched entries: {unmatchedEntries.Count}\n");
                else
                    reportConsole.Report($"Unmatched entries (thus far, some were skipped): {unmatchedEntries.Count}\n");
                foreach (string filepath in unmatchedEntries.Keys)
                {
                    reportConsole.Report($"{Path.GetFileName(filepath)} :: Generated name: {unmatchedEntries[filepath]}\n");
                }
                if (stopWhenNoMatch)
                {
                    reportConsole.Report("Stopping...\n");
                    MessageBox.Show("Not all files matched. Stopping...",
                                    "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            // display matching results
            if (dupCount > 0)
                reportConsole.Report($"Matched {matchedEntries.Count} of {fileCount + dupCount} valid entries (files + generated dups).\n");
            else
                reportConsole.Report($"Matched {matchedEntries.Count} of {fileCount} files.\n");
            if (duplicatedFiles > 0)
                reportConsole.Report($"Skipped {duplicatedFiles} duplicated files.\n");
            if (skippedDups > 0)
                reportConsole.Report($"Skipped {skippedDups} generated dup(s).\n");
            matchCountReporter.Report(matchedEntries.Count);

            // replace
            reportConsole.Report("--------------------------\n");
            reportConsole.Report("REPLACING RESULTS\n");
            reportConsole.Report("Replacing sound files...\n");
            if(skipConversion) reportConsole.Report("Skipping conversion for all files.");
            int replaceCount = 0;
            foreach (SndAssetBankEntry entry in matchedEntries.Keys)
            {
                // set cancel condition, only possible before a conversion process starts
                if (replaceAllManager.tokenSource.Token.IsCancellationRequested)
                {
                    reportConsole.Report("Cancelling...\n");
                    replaceAllManager.tokenSource.Token.ThrowIfCancellationRequested();
                }

                // replace
                bool success = this.ReplaceManually(entry, matchedEntries[entry], replaceAllManager);

                // check success result
                if (!success)
                {
                    reportConsole.Report("Could not replace: ");
                    //reportConsole.Report(Path.GetFileName(matchedEntries[entry]) + " -> " + this.GetNameForEntry(entry, 0) + "\n");
                    reportConsole.Report(Path.GetFileName(matchedEntries[entry]) + " -> " + this.GetNameForEntryOnTable(entry) + "\n");

                    if (stopWhenReplaceFails)
                    {
                        reportConsole.Report("Stopping...\n");
                        MessageBox.Show("A file could not be replaced. Stopping...",
                                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        if (overallProgressValue != null)
                        {
                            overallProgressValue.Report(0);
                        }
                        return;
                    }
                }
                else
                {
                    replaceCount++;
                    //reportConsole.Report($"Replaced {replaceCount} of {fileCount} files\n");
                }

                overallProgressValue.Report(replaceCount);
            }
            reportConsole.Report("Finished replacing sound files.\n");

            reportConsole.Report("--------------------------\n");
            reportConsole.Report("All files were processed!\n");
            MessageBox.Show("All files were processed!",
                            "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void replaceAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_sndAliasBank == null)
            {
                MessageBox.Show("You need to open a .sabs or .sabl file first.", "Replace Manager", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } 
            else
                using (var form = new ReplaceAllForm(this))
                {
                    form.ShowDialog();
                }
        }

        private SndAssetBankEntry GetEntryForName(string name)
        {
            // find matching snd identifier
            int identifier = SndAliasNameDatabase.Names.FirstOrDefault(x => Path.GetFileName(x.Value) == name).Key;
            // get snd entry based on identifier
            SndAssetBankEntry snd = _sndAliasBank.Entries.FirstOrDefault(y => y.Identifier == identifier);
            return snd;
        }

        private SndAssetBankEntry GetEntryForNameOnTable(string name)
        {
            for (int i = 0; i < audioEntriesDataGridView.Rows.Count; i++)
            {
                DataGridViewRow row = audioEntriesDataGridView.Rows[i];
                string entryName = (string)row.Cells[0].Value;
                entryName = entryName.Replace(Path.GetExtension(entryName), ""); // remove file extension
                if (entryName == name)
                    return (SndAssetBankEntry)row.Tag;
            }
            return null;
        }

        private string GetNameForEntryOnTable(SndAssetBankEntry entry)
        {
            for (int i = 0; i < audioEntriesDataGridView.Rows.Count; i++)
            {
                DataGridViewRow row = audioEntriesDataGridView.Rows[i];
                if (row.Tag == entry)
                    return (string)row.Cells[0].Value;
            }
            return null;
        }

        private bool ReplaceManually(SndAssetBankEntry entry, string filename, ReplaceAllForm replaceAllManager)
        {
            // unpack Replace Manager attributes
            IProgress<string> reportConsole = replaceAllManager.reportConsole;
            //Label conversionLabel = replaceAllManager.conversionProgressLabel;

            // Ensure FFmpeg binaries are present before attempting to continue.
            if (!ConvertHelper.CanConvert())
            {
                //MessageBox.Show("FFmpeg was not found, please re-download Black Ops II Sound Studio.",
                //                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                reportConsole.Report("FFmpeg was not found, please re-download Black Ops II Sound Studio.\n");
                return false;
            }

            if (entry.Format == AudioFormat.XMA4)
            {
                //MessageBox.Show("Audio replacement for this format is currently not supported.",
                //                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                reportConsole.Report("Audio replacement for this format is currently not supported.\n");
                return false;
            }

            bool success = ReplaceAudioNoDialog(entry, filename, replaceAllManager);
            return success;
        }

        private bool ReplaceAudioNoDialog(SndAssetBankEntry entry, string inputFile, ReplaceAllForm replaceAllManager)
        {
            if (skipConversion)
            {
                using (var reader = new AudioFileReader(inputFile))
                {
                    // Get audio info
                    String inputExtension = (new FileInfo(inputFile)).Extension;
                    int inputSampleRate = reader.WaveFormat.SampleRate;
                    int inputChannels = reader.WaveFormat.Channels;

                    // Check if input file matches target entry
                    if (!inputExtension.Equals("." + SndAssetBankEntry.formatToString(entry.Format)))
                    {
                        replaceAllManager.reportConsole.Report("Input audio file has a different format than the target audio. Disable Skip Conversion or convert it manually.");
                        return false;
                    }
                    if (inputSampleRate != entry.SampleRate)
                    {
                        replaceAllManager.reportConsole.Report("Input audio file has a different sample rate than the target audio. Disable Skip Conversion or convert it manually.");
                        return false;
                    }
                    if (inputChannels != entry.ChannelCount)
                    {
                        replaceAllManager.reportConsole.Report("Input audio file has a different channel count than the target audio. Disable Skip Conversion conversion or convert it manually.");
                        return false;
                    }
                }

                // Create audio stream and replace
                using (FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    Stream audioCopy = copyFile(fs);
                    return OnAudioReplacedNoDialog(entry, audioCopy, replaceAllManager);
                }
            }
            else
            {
                // Begin conversion here.
                var options = new ConvertOptions { AudioChannels = entry.ChannelCount, SampleRate = entry.SampleRate };
                bool success = OnAudioReplacedNoDialog(entry, ConvertProgressForm.ConvertExternalProgressBar(inputFile, entry.Format, options, replaceAllManager), replaceAllManager);
                return success;
            }
        }

        private bool OnAudioReplacedNoDialog(SndAssetBankEntry entry, Stream newData, ReplaceAllForm replaceAllManager)
        {
            // unpack Replace Manager attributes
            IProgress<string> reportConsole = replaceAllManager.reportConsole;

            if (newData == null || newData.Length == 0)
            {
                //MessageBox.Show("Conversion failed, ensure the file you provided is valid.",
                //                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                reportConsole.Report("Conversion failed, ensure the file you provided is valid.\n");
                if (newData != null) newData.Close();
                return false;
            }


            if (entry.Format == AudioFormat.PCMS16)
            {
                var sampleCount = newData.Length / (entry.ChannelCount * 2);
                var duration = TimeSpan.FromMilliseconds(1000 * (float)sampleCount / entry.SampleRate);
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
                        return false;
                    }
                }
                entry.Duration = duration;
            }
            else
            {
                var helper = new ConvertHelper();
                entry.Duration = helper.GetDuration(newData);
            }

            entry.Data = new AudioDataStream(newData, (int)newData.Length);
            entry.replaced = true; 

            foreach (DataGridViewRow row in audioEntriesDataGridView.Rows)
            {
                if (row.Tag != entry) continue;
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style.BackColor = Color.LightBlue;
                row.Cells[audioEntryReplacedColumn.Index].Value = entry.replaced;

                //MessageBox.Show("Audio data has been successfully replaced.",
                //                "Black Ops II Sound Studio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
            }

            return true;
        }

        private void skipConversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skipConversionToolStripMenuItem.Checked = !skipConversionToolStripMenuItem.Checked;
            skipConversion = skipConversionToolStripMenuItem.Checked;
        }
    }
}
