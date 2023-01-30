namespace BlackOps2SoundStudio
{
    sealed partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.headerGroupBox = new System.Windows.Forms.GroupBox();
            this.headerPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.audioEntriesGroupBox = new System.Windows.Forms.GroupBox();
            this.audioEntriesDataGridView = new System.Windows.Forms.DataGridView();
            this.audioEntryContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.replaceAudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomToolStrip = new System.Windows.Forms.ToolStrip();
            this.playToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pauseToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.stopToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.currentTimetoolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.totalTimeToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.playerTimeTimer = new System.Windows.Forms.Timer(this.components);
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixChecksumsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFullNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceChangeFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pcmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xmaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mp3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flacToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skipConversionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioEntryNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.audioEntryOffsetColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.audioEntrySizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.audioEntryFormatColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.audioEntryLoopColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.audioEntryChannelsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.audioEntrySampleRateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.audioEntryHashColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.audioEntryReplacedColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.headerGroupBox.SuspendLayout();
            this.audioEntriesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.audioEntriesDataGridView)).BeginInit();
            this.audioEntryContextMenuStrip.SuspendLayout();
            this.bottomToolStrip.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.welcomeLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.welcomeLabel.ForeColor = System.Drawing.Color.Gray;
            this.welcomeLabel.Location = new System.Drawing.Point(215, 168);
            this.welcomeLabel.MinimumSize = new System.Drawing.Size(259, 30);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(259, 69);
            this.welcomeLabel.TabIndex = 3;
            this.welcomeLabel.Text = "Black Ops II Sound Studio";
            this.welcomeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.mainSplitContainer.Location = new System.Drawing.Point(12, 27);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.headerGroupBox);
            this.mainSplitContainer.Panel1MinSize = 130;
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.audioEntriesGroupBox);
            this.mainSplitContainer.Size = new System.Drawing.Size(665, 349);
            this.mainSplitContainer.SplitterDistance = 213;
            this.mainSplitContainer.SplitterWidth = 3;
            this.mainSplitContainer.TabIndex = 4;
            this.mainSplitContainer.Visible = false;
            // 
            // headerGroupBox
            // 
            this.headerGroupBox.Controls.Add(this.headerPropertyGrid);
            this.headerGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerGroupBox.Location = new System.Drawing.Point(0, 0);
            this.headerGroupBox.Name = "headerGroupBox";
            this.headerGroupBox.Size = new System.Drawing.Size(213, 349);
            this.headerGroupBox.TabIndex = 6;
            this.headerGroupBox.TabStop = false;
            this.headerGroupBox.Text = "Header Information";
            // 
            // headerPropertyGrid
            // 
            this.headerPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPropertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.headerPropertyGrid.HelpVisible = false;
            this.headerPropertyGrid.Location = new System.Drawing.Point(6, 20);
            this.headerPropertyGrid.Name = "headerPropertyGrid";
            this.headerPropertyGrid.Size = new System.Drawing.Size(201, 323);
            this.headerPropertyGrid.TabIndex = 0;
            this.headerPropertyGrid.ToolbarVisible = false;
            this.headerPropertyGrid.Resize += new System.EventHandler(this.headerPropertyGrid_Resize);
            // 
            // audioEntriesGroupBox
            // 
            this.audioEntriesGroupBox.Controls.Add(this.audioEntriesDataGridView);
            this.audioEntriesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioEntriesGroupBox.Location = new System.Drawing.Point(0, 0);
            this.audioEntriesGroupBox.Name = "audioEntriesGroupBox";
            this.audioEntriesGroupBox.Size = new System.Drawing.Size(449, 349);
            this.audioEntriesGroupBox.TabIndex = 5;
            this.audioEntriesGroupBox.TabStop = false;
            this.audioEntriesGroupBox.Text = "Audio Entries";
            // 
            // audioEntriesDataGridView
            // 
            this.audioEntriesDataGridView.AllowUserToAddRows = false;
            this.audioEntriesDataGridView.AllowUserToDeleteRows = false;
            this.audioEntriesDataGridView.AllowUserToResizeRows = false;
            this.audioEntriesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audioEntriesDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.audioEntriesDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.audioEntriesDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.audioEntriesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.audioEntriesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.audioEntryNameColumn,
            this.audioEntryOffsetColumn,
            this.audioEntrySizeColumn,
            this.audioEntryFormatColumn,
            this.audioEntryLoopColumn,
            this.audioEntryChannelsColumn,
            this.audioEntrySampleRateColumn,
            this.audioEntryHashColumn,
            this.audioEntryReplacedColumn});
            this.audioEntriesDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.audioEntriesDataGridView.Location = new System.Drawing.Point(6, 20);
            this.audioEntriesDataGridView.MultiSelect = false;
            this.audioEntriesDataGridView.Name = "audioEntriesDataGridView";
            this.audioEntriesDataGridView.RowHeadersVisible = false;
            this.audioEntriesDataGridView.RowTemplate.ContextMenuStrip = this.audioEntryContextMenuStrip;
            this.audioEntriesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.audioEntriesDataGridView.Size = new System.Drawing.Size(437, 323);
            this.audioEntriesDataGridView.TabIndex = 0;
            this.audioEntriesDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.audioEntriesDataGridView_CellFormatting);
            this.audioEntriesDataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.audioEntriesDataGridView_CellMouseDown);
            this.audioEntriesDataGridView.SelectionChanged += new System.EventHandler(this.audioEntriesDataGridView_SelectionChanged);
            // 
            // audioEntryContextMenuStrip
            // 
            this.audioEntryContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.audioEntryContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.playAllToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exportToolStripMenuItem,
            this.exportAllToolStripMenuItem,
            this.toolStripMenuItem4,
            this.replaceAudioToolStripMenuItem});
            this.audioEntryContextMenuStrip.Name = "audioEntryContextMenuStrip";
            this.audioEntryContextMenuStrip.Size = new System.Drawing.Size(151, 126);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // playAllToolStripMenuItem
            // 
            this.playAllToolStripMenuItem.Name = "playAllToolStripMenuItem";
            this.playAllToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.playAllToolStripMenuItem.Text = "Play All";
            this.playAllToolStripMenuItem.Click += new System.EventHandler(this.playAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(147, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // exportAllToolStripMenuItem
            // 
            this.exportAllToolStripMenuItem.Name = "exportAllToolStripMenuItem";
            this.exportAllToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.exportAllToolStripMenuItem.Text = "Export All";
            this.exportAllToolStripMenuItem.Click += new System.EventHandler(this.exportAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(147, 6);
            // 
            // replaceAudioToolStripMenuItem
            // 
            this.replaceAudioToolStripMenuItem.Name = "replaceAudioToolStripMenuItem";
            this.replaceAudioToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.replaceAudioToolStripMenuItem.Text = "Replace Audio";
            this.replaceAudioToolStripMenuItem.Click += new System.EventHandler(this.replaceAudioToolStripMenuItem_Click);
            // 
            // bottomToolStrip
            // 
            this.bottomToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bottomToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bottomToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripButton,
            this.pauseToolStripButton,
            this.stopToolStripButton,
            this.currentTimetoolStripLabel,
            this.totalTimeToolStripLabel});
            this.bottomToolStrip.Location = new System.Drawing.Point(0, 377);
            this.bottomToolStrip.Name = "bottomToolStrip";
            this.bottomToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.bottomToolStrip.Size = new System.Drawing.Size(689, 27);
            this.bottomToolStrip.TabIndex = 6;
            this.bottomToolStrip.Text = "toolStrip1";
            // 
            // playToolStripButton
            // 
            this.playToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.playToolStripButton.Enabled = false;
            this.playToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("playToolStripButton.Image")));
            this.playToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playToolStripButton.Name = "playToolStripButton";
            this.playToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.playToolStripButton.Click += new System.EventHandler(this.playToolStripButton_Click);
            // 
            // pauseToolStripButton
            // 
            this.pauseToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pauseToolStripButton.Enabled = false;
            this.pauseToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pauseToolStripButton.Image")));
            this.pauseToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pauseToolStripButton.Name = "pauseToolStripButton";
            this.pauseToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.pauseToolStripButton.Click += new System.EventHandler(this.pauseToolStripButton_Click);
            // 
            // stopToolStripButton
            // 
            this.stopToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopToolStripButton.Enabled = false;
            this.stopToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("stopToolStripButton.Image")));
            this.stopToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopToolStripButton.Name = "stopToolStripButton";
            this.stopToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.stopToolStripButton.Click += new System.EventHandler(this.stopToolStripButton_Click);
            // 
            // currentTimetoolStripLabel
            // 
            this.currentTimetoolStripLabel.Name = "currentTimetoolStripLabel";
            this.currentTimetoolStripLabel.Size = new System.Drawing.Size(0, 24);
            // 
            // totalTimeToolStripLabel
            // 
            this.totalTimeToolStripLabel.Name = "totalTimeToolStripLabel";
            this.totalTimeToolStripLabel.Size = new System.Drawing.Size(0, 24);
            // 
            // playerTimeTimer
            // 
            this.playerTimeTimer.Tick += new System.EventHandler(this.playerTimeTimer_Tick);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(689, 24);
            this.mainMenuStrip.TabIndex = 7;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsToolStripMenuItem.Text = "Save As..";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.fixChecksumsToolStripMenuItem,
            this.showFullNameToolStripMenuItem,
            this.forceChangeFormatToolStripMenuItem,
            this.replaceAllToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Enabled = false;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // fixChecksumsToolStripMenuItem
            // 
            this.fixChecksumsToolStripMenuItem.CheckOnClick = true;
            this.fixChecksumsToolStripMenuItem.Name = "fixChecksumsToolStripMenuItem";
            this.fixChecksumsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.fixChecksumsToolStripMenuItem.Text = "Fix Checksums";
            // 
            // showFullNameToolStripMenuItem
            // 
            this.showFullNameToolStripMenuItem.CheckOnClick = true;
            this.showFullNameToolStripMenuItem.Name = "showFullNameToolStripMenuItem";
            this.showFullNameToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.showFullNameToolStripMenuItem.Text = "Show Full Name";
            this.showFullNameToolStripMenuItem.Click += new System.EventHandler(this.showFullNameToolStripMenuItem_Click);
            // 
            // forceChangeFormatToolStripMenuItem
            // 
            this.forceChangeFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pcmToolStripMenuItem,
            this.xmaToolStripMenuItem,
            this.mp3ToolStripMenuItem,
            this.flacToolStripMenuItem});
            this.forceChangeFormatToolStripMenuItem.Enabled = false;
            this.forceChangeFormatToolStripMenuItem.Name = "forceChangeFormatToolStripMenuItem";
            this.forceChangeFormatToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.forceChangeFormatToolStripMenuItem.Text = "Force Change Format";
            // 
            // pcmToolStripMenuItem
            // 
            this.pcmToolStripMenuItem.Name = "pcmToolStripMenuItem";
            this.pcmToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.pcmToolStripMenuItem.Text = "PCM";
            this.pcmToolStripMenuItem.Click += new System.EventHandler(this.forceFormatToolStripMenuItem_Click);
            // 
            // xmaToolStripMenuItem
            // 
            this.xmaToolStripMenuItem.Name = "xmaToolStripMenuItem";
            this.xmaToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.xmaToolStripMenuItem.Text = "XMA";
            this.xmaToolStripMenuItem.Click += new System.EventHandler(this.forceFormatToolStripMenuItem_Click);
            // 
            // mp3ToolStripMenuItem
            // 
            this.mp3ToolStripMenuItem.Name = "mp3ToolStripMenuItem";
            this.mp3ToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.mp3ToolStripMenuItem.Text = "MP3";
            this.mp3ToolStripMenuItem.Click += new System.EventHandler(this.forceFormatToolStripMenuItem_Click);
            // 
            // flacToolStripMenuItem
            // 
            this.flacToolStripMenuItem.Name = "flacToolStripMenuItem";
            this.flacToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.flacToolStripMenuItem.Text = "FLAC";
            this.flacToolStripMenuItem.Click += new System.EventHandler(this.forceFormatToolStripMenuItem_Click);
            // 
            // replaceAllToolStripMenuItem
            // 
            this.replaceAllToolStripMenuItem.Name = "replaceAllToolStripMenuItem";
            this.replaceAllToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.replaceAllToolStripMenuItem.Text = "Replace Manager";
            this.replaceAllToolStripMenuItem.Click += new System.EventHandler(this.replaceAllToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.skipConversionToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // skipConversionToolStripMenuItem
            // 
            this.skipConversionToolStripMenuItem.Name = "skipConversionToolStripMenuItem";
            this.skipConversionToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.skipConversionToolStripMenuItem.Text = "Skip Conversion";
            this.skipConversionToolStripMenuItem.Click += new System.EventHandler(this.skipConversionToolStripMenuItem_Click);
            // 
            // audioEntryNameColumn
            // 
            this.audioEntryNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.audioEntryNameColumn.HeaderText = "Name";
            this.audioEntryNameColumn.Name = "audioEntryNameColumn";
            this.audioEntryNameColumn.ReadOnly = true;
            this.audioEntryNameColumn.Width = 60;
            // 
            // audioEntryOffsetColumn
            // 
            this.audioEntryOffsetColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.audioEntryOffsetColumn.HeaderText = "Offset";
            this.audioEntryOffsetColumn.MinimumWidth = 80;
            this.audioEntryOffsetColumn.Name = "audioEntryOffsetColumn";
            this.audioEntryOffsetColumn.ReadOnly = true;
            this.audioEntryOffsetColumn.Width = 80;
            // 
            // audioEntrySizeColumn
            // 
            this.audioEntrySizeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.audioEntrySizeColumn.HeaderText = "Size";
            this.audioEntrySizeColumn.MinimumWidth = 70;
            this.audioEntrySizeColumn.Name = "audioEntrySizeColumn";
            this.audioEntrySizeColumn.ReadOnly = true;
            this.audioEntrySizeColumn.Width = 70;
            // 
            // audioEntryFormatColumn
            // 
            this.audioEntryFormatColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.audioEntryFormatColumn.HeaderText = "Format";
            this.audioEntryFormatColumn.Name = "audioEntryFormatColumn";
            this.audioEntryFormatColumn.ReadOnly = true;
            this.audioEntryFormatColumn.Width = 64;
            // 
            // audioEntryLoopColumn
            // 
            this.audioEntryLoopColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.audioEntryLoopColumn.HeaderText = "Loop";
            this.audioEntryLoopColumn.Name = "audioEntryLoopColumn";
            this.audioEntryLoopColumn.Width = 56;
            // 
            // audioEntryChannelsColumn
            // 
            this.audioEntryChannelsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.audioEntryChannelsColumn.HeaderText = "Channels";
            this.audioEntryChannelsColumn.Name = "audioEntryChannelsColumn";
            this.audioEntryChannelsColumn.ReadOnly = true;
            this.audioEntryChannelsColumn.Width = 76;
            // 
            // audioEntrySampleRateColumn
            // 
            this.audioEntrySampleRateColumn.HeaderText = "Sample Rate";
            this.audioEntrySampleRateColumn.Name = "audioEntrySampleRateColumn";
            this.audioEntrySampleRateColumn.Width = 60;
            // 
            // audioEntryHashColumn
            // 
            this.audioEntryHashColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.audioEntryHashColumn.HeaderText = "Hash";
            this.audioEntryHashColumn.Name = "audioEntryHashColumn";
            this.audioEntryHashColumn.ReadOnly = true;
            // 
            // audioEntryReplacedColumn
            // 
            this.audioEntryReplacedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.audioEntryReplacedColumn.HeaderText = "Replaced";
            this.audioEntryReplacedColumn.Name = "audioEntryReplacedColumn";
            this.audioEntryReplacedColumn.ReadOnly = true;
            this.audioEntryReplacedColumn.ToolTipText = "If this audio entry was replaced.";
            this.audioEntryReplacedColumn.Width = 78;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 404);
            this.Controls.Add(this.bottomToolStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.welcomeLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(704, 423);
            this.Name = "MainForm";
            this.Text = "Black Ops II Sound Studio by master131";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.headerGroupBox.ResumeLayout(false);
            this.audioEntriesGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.audioEntriesDataGridView)).EndInit();
            this.audioEntryContextMenuStrip.ResumeLayout(false);
            this.bottomToolStrip.ResumeLayout(false);
            this.bottomToolStrip.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.GroupBox headerGroupBox;
        private System.Windows.Forms.GroupBox audioEntriesGroupBox;
        private System.Windows.Forms.DataGridView audioEntriesDataGridView;
        private System.Windows.Forms.ToolStrip bottomToolStrip;
        private System.Windows.Forms.ToolStripButton playToolStripButton;
        private System.Windows.Forms.ToolStripButton pauseToolStripButton;
        private System.Windows.Forms.ToolStripButton stopToolStripButton;
        private System.Windows.Forms.ToolStripLabel currentTimetoolStripLabel;
        private System.Windows.Forms.ToolStripLabel totalTimeToolStripLabel;
        private System.Windows.Forms.Timer playerTimeTimer;
        private System.Windows.Forms.PropertyGrid headerPropertyGrid;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip audioEntryContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem replaceAudioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixChecksumsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFullNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceChangeFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pcmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mp3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flacToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skipConversionToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntryNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntryOffsetColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntrySizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntryFormatColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntryLoopColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntryChannelsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntrySampleRateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntryHashColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn audioEntryReplacedColumn;
    }
}

