namespace GreekBibleStudent
{
    partial class frmCopyReset
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
            this.pnlBase = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkLXXSelection = new System.Windows.Forms.CheckBox();
            this.chkLXXChapter = new System.Windows.Forms.CheckBox();
            this.chkLXXVerse = new System.Windows.Forms.CheckBox();
            this.chkLXXWord = new System.Windows.Forms.CheckBox();
            this.labLXXLbl = new System.Windows.Forms.Label();
            this.labNTLbl = new System.Windows.Forms.Label();
            this.labExplanationFull = new System.Windows.Forms.Label();
            this.chkNTSelection = new System.Windows.Forms.CheckBox();
            this.chkNTChapter = new System.Windows.Forms.CheckBox();
            this.chkNTVerse = new System.Windows.Forms.CheckBox();
            this.chkNTWord = new System.Windows.Forms.CheckBox();
            this.labExplanationLbl = new System.Windows.Forms.Label();
            this.pnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.btnCancel);
            this.pnlBase.Controls.Add(this.btnOK);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBase.Location = new System.Drawing.Point(0, 192);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(519, 30);
            this.pnlBase.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(432, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkLXXSelection
            // 
            this.chkLXXSelection.AutoSize = true;
            this.chkLXXSelection.Location = new System.Drawing.Point(291, 122);
            this.chkLXXSelection.Name = "chkLXXSelection";
            this.chkLXXSelection.Size = new System.Drawing.Size(193, 17);
            this.chkLXXSelection.TabIndex = 24;
            this.chkLXXSelection.Text = "copying the selection portion of text";
            this.chkLXXSelection.UseVisualStyleBackColor = true;
            // 
            // chkLXXChapter
            // 
            this.chkLXXChapter.AutoSize = true;
            this.chkLXXChapter.Location = new System.Drawing.Point(291, 99);
            this.chkLXXChapter.Name = "chkLXXChapter";
            this.chkLXXChapter.Size = new System.Drawing.Size(149, 17);
            this.chkLXXChapter.TabIndex = 23;
            this.chkLXXChapter.Text = "copying the entire chapter";
            this.chkLXXChapter.UseVisualStyleBackColor = true;
            // 
            // chkLXXVerse
            // 
            this.chkLXXVerse.AutoSize = true;
            this.chkLXXVerse.Location = new System.Drawing.Point(291, 76);
            this.chkLXXVerse.Name = "chkLXXVerse";
            this.chkLXXVerse.Size = new System.Drawing.Size(101, 17);
            this.chkLXXVerse.TabIndex = 22;
            this.chkLXXVerse.Text = "copying a verse";
            this.chkLXXVerse.UseVisualStyleBackColor = true;
            // 
            // chkLXXWord
            // 
            this.chkLXXWord.AutoSize = true;
            this.chkLXXWord.Location = new System.Drawing.Point(291, 53);
            this.chkLXXWord.Name = "chkLXXWord";
            this.chkLXXWord.Size = new System.Drawing.Size(128, 17);
            this.chkLXXWord.TabIndex = 21;
            this.chkLXXWord.Text = "copying a single word";
            this.chkLXXWord.UseVisualStyleBackColor = true;
            // 
            // labLXXLbl
            // 
            this.labLXXLbl.AutoSize = true;
            this.labLXXLbl.Location = new System.Drawing.Point(352, 32);
            this.labLXXLbl.Name = "labLXXLbl";
            this.labLXXLbl.Size = new System.Drawing.Size(82, 13);
            this.labLXXLbl.TabIndex = 20;
            this.labLXXLbl.Text = "Septuagint Text";
            // 
            // labNTLbl
            // 
            this.labNTLbl.AutoSize = true;
            this.labNTLbl.Location = new System.Drawing.Point(70, 32);
            this.labNTLbl.Name = "labNTLbl";
            this.labNTLbl.Size = new System.Drawing.Size(106, 13);
            this.labNTLbl.TabIndex = 19;
            this.labNTLbl.Text = "New Testament Text";
            // 
            // labExplanationFull
            // 
            this.labExplanationFull.Location = new System.Drawing.Point(12, 151);
            this.labExplanationFull.Name = "labExplanationFull";
            this.labExplanationFull.Size = new System.Drawing.Size(482, 35);
            this.labExplanationFull.TabIndex = 18;
            this.labExplanationFull.Text = "If a check box (above) is \"checked\", then that menu option will use previously co" +
    "nfigured options.  To enable you to reset those options, uncheck the box.";
            // 
            // chkNTSelection
            // 
            this.chkNTSelection.AutoSize = true;
            this.chkNTSelection.Location = new System.Drawing.Point(38, 122);
            this.chkNTSelection.Name = "chkNTSelection";
            this.chkNTSelection.Size = new System.Drawing.Size(193, 17);
            this.chkNTSelection.TabIndex = 17;
            this.chkNTSelection.Text = "copying the selection portion of text";
            this.chkNTSelection.UseVisualStyleBackColor = true;
            // 
            // chkNTChapter
            // 
            this.chkNTChapter.AutoSize = true;
            this.chkNTChapter.Location = new System.Drawing.Point(38, 99);
            this.chkNTChapter.Name = "chkNTChapter";
            this.chkNTChapter.Size = new System.Drawing.Size(149, 17);
            this.chkNTChapter.TabIndex = 16;
            this.chkNTChapter.Text = "copying the entire chapter";
            this.chkNTChapter.UseVisualStyleBackColor = true;
            // 
            // chkNTVerse
            // 
            this.chkNTVerse.AutoSize = true;
            this.chkNTVerse.Location = new System.Drawing.Point(38, 76);
            this.chkNTVerse.Name = "chkNTVerse";
            this.chkNTVerse.Size = new System.Drawing.Size(101, 17);
            this.chkNTVerse.TabIndex = 15;
            this.chkNTVerse.Text = "copying a verse";
            this.chkNTVerse.UseVisualStyleBackColor = true;
            // 
            // chkNTWord
            // 
            this.chkNTWord.AutoSize = true;
            this.chkNTWord.Location = new System.Drawing.Point(38, 53);
            this.chkNTWord.Name = "chkNTWord";
            this.chkNTWord.Size = new System.Drawing.Size(128, 17);
            this.chkNTWord.TabIndex = 14;
            this.chkNTWord.Text = "copying a single word";
            this.chkNTWord.UseVisualStyleBackColor = true;
            // 
            // labExplanationLbl
            // 
            this.labExplanationLbl.AutoSize = true;
            this.labExplanationLbl.Location = new System.Drawing.Point(12, 9);
            this.labExplanationLbl.Name = "labExplanationLbl";
            this.labExplanationLbl.Size = new System.Drawing.Size(171, 13);
            this.labExplanationLbl.TabIndex = 13;
            this.labExplanationLbl.Text = "Use the stored, default settings for:";
            // 
            // frmCopyReset
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(519, 222);
            this.ControlBox = false;
            this.Controls.Add(this.chkLXXSelection);
            this.Controls.Add(this.chkLXXChapter);
            this.Controls.Add(this.chkLXXVerse);
            this.Controls.Add(this.chkLXXWord);
            this.Controls.Add(this.labLXXLbl);
            this.Controls.Add(this.labNTLbl);
            this.Controls.Add(this.labExplanationFull);
            this.Controls.Add(this.chkNTSelection);
            this.Controls.Add(this.chkNTChapter);
            this.Controls.Add(this.chkNTVerse);
            this.Controls.Add(this.chkNTWord);
            this.Controls.Add(this.labExplanationLbl);
            this.Controls.Add(this.pnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmCopyReset";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reset default copy options";
            this.pnlBase.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkLXXSelection;
        private System.Windows.Forms.CheckBox chkLXXChapter;
        private System.Windows.Forms.CheckBox chkLXXVerse;
        private System.Windows.Forms.CheckBox chkLXXWord;
        private System.Windows.Forms.Label labLXXLbl;
        private System.Windows.Forms.Label labNTLbl;
        private System.Windows.Forms.Label labExplanationFull;
        private System.Windows.Forms.CheckBox chkNTSelection;
        private System.Windows.Forms.CheckBox chkNTChapter;
        private System.Windows.Forms.CheckBox chkNTVerse;
        private System.Windows.Forms.CheckBox chkNTWord;
        private System.Windows.Forms.Label labExplanationLbl;
    }
}