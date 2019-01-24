namespace BlackOps2SoundStudio
{
    partial class ConvertProgressForm
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
            this.conversionLabel = new System.Windows.Forms.Label();
            this.conversionProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // conversionLabel
            // 
            this.conversionLabel.Location = new System.Drawing.Point(12, 9);
            this.conversionLabel.Name = "conversionLabel";
            this.conversionLabel.Size = new System.Drawing.Size(439, 25);
            this.conversionLabel.TabIndex = 0;
            this.conversionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // conversionProgressBar
            // 
            this.conversionProgressBar.Location = new System.Drawing.Point(15, 39);
            this.conversionProgressBar.Name = "conversionProgressBar";
            this.conversionProgressBar.Size = new System.Drawing.Size(436, 23);
            this.conversionProgressBar.TabIndex = 1;
            // 
            // ConvertProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 76);
            this.Controls.Add(this.conversionProgressBar);
            this.Controls.Add(this.conversionLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConvertProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Conversion Progress";
            this.Load += new System.EventHandler(this.ConvertProgressForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label conversionLabel;
        private System.Windows.Forms.ProgressBar conversionProgressBar;
    }
}