namespace BlackOps2SoundStudio
{
    partial class ExportProgressForm
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
            this.exportProgressBar = new System.Windows.Forms.ProgressBar();
            this.exportLabel = new System.Windows.Forms.Label();
            this.exportBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // exportProgressBar
            // 
            this.exportProgressBar.Location = new System.Drawing.Point(15, 39);
            this.exportProgressBar.Name = "exportProgressBar";
            this.exportProgressBar.Size = new System.Drawing.Size(436, 23);
            this.exportProgressBar.TabIndex = 3;
            // 
            // exportLabel
            // 
            this.exportLabel.Location = new System.Drawing.Point(12, 9);
            this.exportLabel.Name = "exportLabel";
            this.exportLabel.Size = new System.Drawing.Size(439, 25);
            this.exportLabel.TabIndex = 2;
            this.exportLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exportBackgroundWorker
            // 
            this.exportBackgroundWorker.WorkerReportsProgress = true;
            this.exportBackgroundWorker.WorkerSupportsCancellation = true;
            this.exportBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.exportBackgroundWorker_DoWork);
            this.exportBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.exportBackgroundWorker_ProgressChanged);
            this.exportBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.exportBackgroundWorker_RunWorkerCompleted);
            // 
            // ExportProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 76);
            this.Controls.Add(this.exportProgressBar);
            this.Controls.Add(this.exportLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Export All";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportProgressForm_FormClosing);
            this.Load += new System.EventHandler(this.ExportProgressForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar exportProgressBar;
        private System.Windows.Forms.Label exportLabel;
        private System.ComponentModel.BackgroundWorker exportBackgroundWorker;
    }
}