namespace GreekBibleStudent
{
    partial class frmProgress
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
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.labProgressAction2Msg = new System.Windows.Forms.Label();
            this.labProgressAction1Msg = new System.Windows.Forms.Label();
            this.labProgressMessage3Lbl = new System.Windows.Forms.Label();
            this.labProgressMessage2Lbl = new System.Windows.Forms.Label();
            this.labProgressMessage1Lbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(13, 145);
            this.pbProgress.Maximum = 96;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(776, 5);
            this.pbProgress.Step = 1;
            this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbProgress.TabIndex = 11;
            // 
            // labProgressAction2Msg
            // 
            this.labProgressAction2Msg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labProgressAction2Msg.Location = new System.Drawing.Point(5, 226);
            this.labProgressAction2Msg.Name = "labProgressAction2Msg";
            this.labProgressAction2Msg.Size = new System.Drawing.Size(793, 23);
            this.labProgressAction2Msg.TabIndex = 10;
            this.labProgressAction2Msg.Text = "None";
            this.labProgressAction2Msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labProgressAction1Msg
            // 
            this.labProgressAction1Msg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labProgressAction1Msg.Location = new System.Drawing.Point(55, 203);
            this.labProgressAction1Msg.Name = "labProgressAction1Msg";
            this.labProgressAction1Msg.Size = new System.Drawing.Size(744, 23);
            this.labProgressAction1Msg.TabIndex = 9;
            this.labProgressAction1Msg.Text = "None";
            this.labProgressAction1Msg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labProgressMessage3Lbl
            // 
            this.labProgressMessage3Lbl.AutoSize = true;
            this.labProgressMessage3Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labProgressMessage3Lbl.Location = new System.Drawing.Point(13, 174);
            this.labProgressMessage3Lbl.Name = "labProgressMessage3Lbl";
            this.labProgressMessage3Lbl.Size = new System.Drawing.Size(113, 20);
            this.labProgressMessage3Lbl.TabIndex = 8;
            this.labProgressMessage3Lbl.Text = "Current action:";
            // 
            // labProgressMessage2Lbl
            // 
            this.labProgressMessage2Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labProgressMessage2Lbl.ForeColor = System.Drawing.Color.Red;
            this.labProgressMessage2Lbl.Location = new System.Drawing.Point(1, 93);
            this.labProgressMessage2Lbl.Name = "labProgressMessage2Lbl";
            this.labProgressMessage2Lbl.Size = new System.Drawing.Size(797, 31);
            this.labProgressMessage2Lbl.TabIndex = 7;
            this.labProgressMessage2Lbl.Text = "Please be patient";
            this.labProgressMessage2Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labProgressMessage1Lbl
            // 
            this.labProgressMessage1Lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labProgressMessage1Lbl.Location = new System.Drawing.Point(1, 40);
            this.labProgressMessage1Lbl.Name = "labProgressMessage1Lbl";
            this.labProgressMessage1Lbl.Size = new System.Drawing.Size(799, 23);
            this.labProgressMessage1Lbl.TabIndex = 6;
            this.labProgressMessage1Lbl.Text = "Performing Old Testament Student initialisation tasks";
            this.labProgressMessage1Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 288);
            this.ControlBox = false;
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.labProgressAction2Msg);
            this.Controls.Add(this.labProgressAction1Msg);
            this.Controls.Add(this.labProgressMessage3Lbl);
            this.Controls.Add(this.labProgressMessage2Lbl);
            this.Controls.Add(this.labProgressMessage1Lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmProgress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Greek Bible Student - Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label labProgressAction2Msg;
        private System.Windows.Forms.Label labProgressAction1Msg;
        private System.Windows.Forms.Label labProgressMessage3Lbl;
        private System.Windows.Forms.Label labProgressMessage2Lbl;
        private System.Windows.Forms.Label labProgressMessage1Lbl;
    }
}