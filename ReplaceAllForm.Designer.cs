namespace Black_Ops_II_Sound_Studio_Extended
{
    partial class ReplaceAllForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.folderTextBox = new System.Windows.Forms.TextBox();
            this.stopWhenReplaceFailsComboBox = new System.Windows.Forms.CheckBox();
            this.stopWhenNoMatchCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.exportButton = new System.Windows.Forms.Button();
            this.reportRichTextBox = new System.Windows.Forms.RichTextBox();
            this.conversionProgressBar = new System.Windows.Forms.ProgressBar();
            this.overallProgressBar = new System.Windows.Forms.ProgressBar();
            this.conversionProgressLabel = new System.Windows.Forms.Label();
            this.overallProgressLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.adaptNamesCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sourceComboBox = new System.Windows.Forms.ComboBox();
            this.targetComboBox = new System.Windows.Forms.ComboBox();
            this.dupFixCheckBox = new System.Windows.Forms.CheckBox();
            this.nameAdaptingGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.nameAdaptingGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.openFolderButton);
            this.groupBox1.Controls.Add(this.folderTextBox);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(586, 44);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Folder selection";
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(521, 18);
            this.openFolderButton.Margin = new System.Windows.Forms.Padding(2);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(56, 21);
            this.openFolderButton.TabIndex = 1;
            this.openFolderButton.Text = "Open";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // folderTextBox
            // 
            this.folderTextBox.Location = new System.Drawing.Point(4, 19);
            this.folderTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.folderTextBox.Name = "folderTextBox";
            this.folderTextBox.Size = new System.Drawing.Size(513, 20);
            this.folderTextBox.TabIndex = 0;
            // 
            // stopWhenReplaceFailsComboBox
            // 
            this.stopWhenReplaceFailsComboBox.AutoSize = true;
            this.stopWhenReplaceFailsComboBox.Checked = true;
            this.stopWhenReplaceFailsComboBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stopWhenReplaceFailsComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopWhenReplaceFailsComboBox.Location = new System.Drawing.Point(16, 42);
            this.stopWhenReplaceFailsComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.stopWhenReplaceFailsComboBox.Name = "stopWhenReplaceFailsComboBox";
            this.stopWhenReplaceFailsComboBox.Size = new System.Drawing.Size(164, 20);
            this.stopWhenReplaceFailsComboBox.TabIndex = 7;
            this.stopWhenReplaceFailsComboBox.Text = "Stop when replace fails";
            this.stopWhenReplaceFailsComboBox.UseVisualStyleBackColor = true;
            // 
            // stopWhenNoMatchCheckBox
            // 
            this.stopWhenNoMatchCheckBox.AutoSize = true;
            this.stopWhenNoMatchCheckBox.Checked = true;
            this.stopWhenNoMatchCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stopWhenNoMatchCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopWhenNoMatchCheckBox.Location = new System.Drawing.Point(16, 18);
            this.stopWhenNoMatchCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.stopWhenNoMatchCheckBox.Name = "stopWhenNoMatchCheckBox";
            this.stopWhenNoMatchCheckBox.Size = new System.Drawing.Size(145, 20);
            this.stopWhenNoMatchCheckBox.TabIndex = 6;
            this.stopWhenNoMatchCheckBox.Text = "Stop when no match";
            this.stopWhenNoMatchCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.exportButton);
            this.groupBox3.Controls.Add(this.reportRichTextBox);
            this.groupBox3.Controls.Add(this.conversionProgressBar);
            this.groupBox3.Controls.Add(this.overallProgressBar);
            this.groupBox3.Controls.Add(this.conversionProgressLabel);
            this.groupBox3.Controls.Add(this.overallProgressLabel);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(9, 183);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(586, 324);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Progress";
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportButton.Location = new System.Drawing.Point(5, 284);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(121, 23);
            this.exportButton.TabIndex = 6;
            this.exportButton.Text = "Export results report";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // reportRichTextBox
            // 
            this.reportRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportRichTextBox.Location = new System.Drawing.Point(5, 126);
            this.reportRichTextBox.Name = "reportRichTextBox";
            this.reportRichTextBox.ReadOnly = true;
            this.reportRichTextBox.Size = new System.Drawing.Size(576, 152);
            this.reportRichTextBox.TabIndex = 5;
            this.reportRichTextBox.Text = "";
            // 
            // conversionProgressBar
            // 
            this.conversionProgressBar.Location = new System.Drawing.Point(4, 91);
            this.conversionProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.conversionProgressBar.Name = "conversionProgressBar";
            this.conversionProgressBar.Size = new System.Drawing.Size(390, 19);
            this.conversionProgressBar.TabIndex = 5;
            // 
            // overallProgressBar
            // 
            this.overallProgressBar.Location = new System.Drawing.Point(4, 42);
            this.overallProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.overallProgressBar.Name = "overallProgressBar";
            this.overallProgressBar.Size = new System.Drawing.Size(390, 19);
            this.overallProgressBar.TabIndex = 4;
            // 
            // conversionProgressLabel
            // 
            this.conversionProgressLabel.AutoSize = true;
            this.conversionProgressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conversionProgressLabel.Location = new System.Drawing.Point(398, 91);
            this.conversionProgressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.conversionProgressLabel.Name = "conversionProgressLabel";
            this.conversionProgressLabel.Size = new System.Drawing.Size(80, 13);
            this.conversionProgressLabel.TabIndex = 3;
            this.conversionProgressLabel.Text = "No files loaded ";
            // 
            // overallProgressLabel
            // 
            this.overallProgressLabel.AutoSize = true;
            this.overallProgressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overallProgressLabel.Location = new System.Drawing.Point(398, 42);
            this.overallProgressLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.overallProgressLabel.Name = "overallProgressLabel";
            this.overallProgressLabel.Size = new System.Drawing.Size(77, 13);
            this.overallProgressLabel.TabIndex = 2;
            this.overallProgressLabel.Text = "No files loaded";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 73);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(195, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "Current file conversion progress";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 24);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "General progress";
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(487, 511);
            this.startButton.Margin = new System.Windows.Forms.Padding(2);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(101, 22);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Location = new System.Drawing.Point(428, 511);
            this.stopButton.Margin = new System.Windows.Forms.Padding(2);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(56, 22);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.adaptNamesCheckBox);
            this.groupBox4.Controls.Add(this.stopWhenNoMatchCheckBox);
            this.groupBox4.Controls.Add(this.stopWhenReplaceFailsComboBox);
            this.groupBox4.Location = new System.Drawing.Point(9, 59);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(337, 121);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Options";
            // 
            // adaptNamesCheckBox
            // 
            this.adaptNamesCheckBox.AutoSize = true;
            this.adaptNamesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.adaptNamesCheckBox.Location = new System.Drawing.Point(16, 66);
            this.adaptNamesCheckBox.Name = "adaptNamesCheckBox";
            this.adaptNamesCheckBox.Size = new System.Drawing.Size(126, 20);
            this.adaptNamesCheckBox.TabIndex = 8;
            this.adaptNamesCheckBox.Text = "Adapt file names";
            this.adaptNamesCheckBox.UseVisualStyleBackColor = true;
            this.adaptNamesCheckBox.CheckedChanged += new System.EventHandler(this.adaptNamesCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source platform:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Target platform:";
            // 
            // sourceComboBox
            // 
            this.sourceComboBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.sourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceComboBox.FormattingEnabled = true;
            this.sourceComboBox.Items.AddRange(new object[] {
            "PC",
            "Xbox 360"});
            this.sourceComboBox.Location = new System.Drawing.Point(121, 16);
            this.sourceComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.sourceComboBox.Name = "sourceComboBox";
            this.sourceComboBox.Size = new System.Drawing.Size(92, 21);
            this.sourceComboBox.TabIndex = 4;
            // 
            // targetComboBox
            // 
            this.targetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetComboBox.FormattingEnabled = true;
            this.targetComboBox.Items.AddRange(new object[] {
            "PC"});
            this.targetComboBox.Location = new System.Drawing.Point(121, 41);
            this.targetComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.targetComboBox.Name = "targetComboBox";
            this.targetComboBox.Size = new System.Drawing.Size(92, 21);
            this.targetComboBox.TabIndex = 5;
            // 
            // dupFixCheckBox
            // 
            this.dupFixCheckBox.AutoSize = true;
            this.dupFixCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dupFixCheckBox.Location = new System.Drawing.Point(16, 67);
            this.dupFixCheckBox.Name = "dupFixCheckBox";
            this.dupFixCheckBox.Size = new System.Drawing.Size(159, 20);
            this.dupFixCheckBox.TabIndex = 10;
            this.dupFixCheckBox.Text = "Apply dup voiceline fix";
            this.dupFixCheckBox.UseVisualStyleBackColor = true;
            // 
            // nameAdaptingGroupBox
            // 
            this.nameAdaptingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameAdaptingGroupBox.Controls.Add(this.dupFixCheckBox);
            this.nameAdaptingGroupBox.Controls.Add(this.targetComboBox);
            this.nameAdaptingGroupBox.Controls.Add(this.sourceComboBox);
            this.nameAdaptingGroupBox.Controls.Add(this.label2);
            this.nameAdaptingGroupBox.Controls.Add(this.label1);
            this.nameAdaptingGroupBox.Enabled = false;
            this.nameAdaptingGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameAdaptingGroupBox.Location = new System.Drawing.Point(351, 58);
            this.nameAdaptingGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.nameAdaptingGroupBox.Name = "nameAdaptingGroupBox";
            this.nameAdaptingGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.nameAdaptingGroupBox.Size = new System.Drawing.Size(244, 121);
            this.nameAdaptingGroupBox.TabIndex = 1;
            this.nameAdaptingGroupBox.TabStop = false;
            this.nameAdaptingGroupBox.Text = "Name Adapting";
            // 
            // ReplaceAllForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(602, 538);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.nameAdaptingGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ReplaceAllForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Replace All Manager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.nameAdaptingGroupBox.ResumeLayout(false);
            this.nameAdaptingGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.TextBox folderTextBox;
        private System.Windows.Forms.CheckBox stopWhenReplaceFailsComboBox;
        private System.Windows.Forms.CheckBox stopWhenNoMatchCheckBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.ProgressBar conversionProgressBar;
        private System.Windows.Forms.ProgressBar overallProgressBar;
        private System.Windows.Forms.Label conversionProgressLabel;
        private System.Windows.Forms.Label overallProgressLabel;
        private System.Windows.Forms.RichTextBox reportRichTextBox;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox adaptNamesCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox sourceComboBox;
        private System.Windows.Forms.ComboBox targetComboBox;
        private System.Windows.Forms.CheckBox dupFixCheckBox;
        private System.Windows.Forms.GroupBox nameAdaptingGroupBox;
    }
}