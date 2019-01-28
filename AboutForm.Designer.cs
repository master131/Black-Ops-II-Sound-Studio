namespace BlackOps2SoundStudio
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.closeButton = new System.Windows.Forms.Button();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.logoTextLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.nameVersionLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.nameLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.nameLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.versionLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.reservedLabel = new System.Windows.Forms.Label();
            this.aboutRichTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.mainLayoutPanel.SuspendLayout();
            this.logoTextLayoutPanel.SuspendLayout();
            this.nameVersionLayoutPanel.SuspendLayout();
            this.nameLayoutPanel.SuspendLayout();
            this.versionLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.closeButton.Location = new System.Drawing.Point(292, 327);
            this.closeButton.Margin = new System.Windows.Forms.Padding(4);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(91, 26);
            this.closeButton.TabIndex = 10;
            this.closeButton.Text = "OK";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.BackgroundImage")));
            this.logoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoPictureBox.Location = new System.Drawing.Point(4, 4);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(122, 130);
            this.logoPictureBox.TabIndex = 16;
            this.logoPictureBox.TabStop = false;
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.logoTextLayoutPanel, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.aboutRichTextBox, 0, 1);
            this.mainLayoutPanel.Controls.Add(this.closeButton, 0, 2);
            this.mainLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 3;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.54829F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.45171F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(387, 357);
            this.mainLayoutPanel.TabIndex = 17;
            // 
            // logoTextLayoutPanel
            // 
            this.logoTextLayoutPanel.ColumnCount = 2;
            this.logoTextLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.logoTextLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.logoTextLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
            this.logoTextLayoutPanel.Controls.Add(this.nameVersionLayoutPanel, 1, 0);
            this.logoTextLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logoTextLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.logoTextLayoutPanel.Name = "logoTextLayoutPanel";
            this.logoTextLayoutPanel.RowCount = 1;
            this.logoTextLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.logoTextLayoutPanel.Size = new System.Drawing.Size(381, 138);
            this.logoTextLayoutPanel.TabIndex = 0;
            // 
            // nameVersionLayoutPanel
            // 
            this.nameVersionLayoutPanel.ColumnCount = 1;
            this.nameVersionLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.nameVersionLayoutPanel.Controls.Add(this.nameLayoutPanel, 0, 0);
            this.nameVersionLayoutPanel.Controls.Add(this.versionLayoutPanel, 0, 1);
            this.nameVersionLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameVersionLayoutPanel.Location = new System.Drawing.Point(133, 3);
            this.nameVersionLayoutPanel.Name = "nameVersionLayoutPanel";
            this.nameVersionLayoutPanel.RowCount = 2;
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.21212F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.78788F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameVersionLayoutPanel.Size = new System.Drawing.Size(245, 132);
            this.nameVersionLayoutPanel.TabIndex = 17;
            // 
            // nameLayoutPanel
            // 
            this.nameLayoutPanel.Controls.Add(this.nameLabel);
            this.nameLayoutPanel.Controls.Add(this.versionLabel);
            this.nameLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.nameLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.nameLayoutPanel.Name = "nameLayoutPanel";
            this.nameLayoutPanel.Size = new System.Drawing.Size(239, 54);
            this.nameLayoutPanel.TabIndex = 0;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.nameLabel.Location = new System.Drawing.Point(3, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(230, 25);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Black Ops II Sound Studio";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.versionLabel.Location = new System.Drawing.Point(3, 30);
            this.versionLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(91, 20);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "Version 1.5.4";
            // 
            // versionLayoutPanel
            // 
            this.versionLayoutPanel.Controls.Add(this.copyrightLabel);
            this.versionLayoutPanel.Controls.Add(this.reservedLabel);
            this.versionLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.versionLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.versionLayoutPanel.Location = new System.Drawing.Point(3, 63);
            this.versionLayoutPanel.Name = "versionLayoutPanel";
            this.versionLayoutPanel.Size = new System.Drawing.Size(239, 66);
            this.versionLayoutPanel.TabIndex = 1;
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyrightLabel.Location = new System.Drawing.Point(3, 0);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(169, 40);
            this.copyrightLabel.TabIndex = 2;
            this.copyrightLabel.Text = "Copyright © 2013-2019 master131";
            // 
            // reservedLabel
            // 
            this.reservedLabel.AutoSize = true;
            this.reservedLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reservedLabel.Location = new System.Drawing.Point(3, 40);
            this.reservedLabel.Name = "reservedLabel";
            this.reservedLabel.Size = new System.Drawing.Size(131, 20);
            this.reservedLabel.TabIndex = 3;
            this.reservedLabel.Text = "All rights reserved.";
            // 
            // aboutRichTextBox
            // 
            this.aboutRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutRichTextBox.Location = new System.Drawing.Point(4, 148);
            this.aboutRichTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.aboutRichTextBox.Name = "aboutRichTextBox";
            this.aboutRichTextBox.ReadOnly = true;
            this.aboutRichTextBox.Size = new System.Drawing.Size(379, 171);
            this.aboutRichTextBox.TabIndex = 14;
            this.aboutRichTextBox.Text = "";
            this.aboutRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.aboutRichTextBox_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 376);
            this.Controls.Add(this.mainLayoutPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Black Ops II Sound Studio";
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.mainLayoutPanel.ResumeLayout(false);
            this.logoTextLayoutPanel.ResumeLayout(false);
            this.nameVersionLayoutPanel.ResumeLayout(false);
            this.nameLayoutPanel.ResumeLayout(false);
            this.nameLayoutPanel.PerformLayout();
            this.versionLayoutPanel.ResumeLayout(false);
            this.versionLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel logoTextLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel nameVersionLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel nameLayoutPanel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.FlowLayoutPanel versionLayoutPanel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label reservedLabel;
        private System.Windows.Forms.RichTextBox aboutRichTextBox;

    }
}