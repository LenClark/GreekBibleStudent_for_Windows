namespace GreekBibleStudent
{
    partial class frmCopyOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbtnExcludeAccents = new System.Windows.Forms.RadioButton();
            this.rbtnIncludeAccents = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnExcludeReference = new System.Windows.Forms.RadioButton();
            this.rbtnIncludeReference = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnCopyToNotes = new System.Windows.Forms.RadioButton();
            this.rbtnCopyToMemory = new System.Windows.Forms.RadioButton();
            this.pnlBase.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.btnCancel);
            this.pnlBase.Controls.Add(this.btnOK);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBase.Location = new System.Drawing.Point(0, 164);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(674, 30);
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
            this.btnOK.Location = new System.Drawing.Point(587, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(519, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "(If this is selected, you need to select \"Change Copy Options\" from the context m" +
    "enu to change the settings.)";
            // 
            // chkRemember
            // 
            this.chkRemember.AutoSize = true;
            this.chkRemember.Location = new System.Drawing.Point(32, 109);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(444, 17);
            this.chkRemember.TabIndex = 9;
            this.chkRemember.Text = "Remember these settings and don\'t show this dialog in future (only applies to thi" +
    "s session)";
            this.chkRemember.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbtnExcludeAccents);
            this.groupBox3.Controls.Add(this.rbtnIncludeAccents);
            this.groupBox3.Location = new System.Drawing.Point(423, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(236, 73);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Accents (and the like): ";
            // 
            // rbtnExcludeAccents
            // 
            this.rbtnExcludeAccents.AutoSize = true;
            this.rbtnExcludeAccents.Location = new System.Drawing.Point(16, 42);
            this.rbtnExcludeAccents.Name = "rbtnExcludeAccents";
            this.rbtnExcludeAccents.Size = new System.Drawing.Size(212, 17);
            this.rbtnExcludeAccents.TabIndex = 5;
            this.rbtnExcludeAccents.Text = "Don\'t include accents in the copied text";
            this.rbtnExcludeAccents.UseVisualStyleBackColor = true;
            // 
            // rbtnIncludeAccents
            // 
            this.rbtnIncludeAccents.AutoSize = true;
            this.rbtnIncludeAccents.Checked = true;
            this.rbtnIncludeAccents.Location = new System.Drawing.Point(16, 19);
            this.rbtnIncludeAccents.Name = "rbtnIncludeAccents";
            this.rbtnIncludeAccents.Size = new System.Drawing.Size(185, 17);
            this.rbtnIncludeAccents.TabIndex = 4;
            this.rbtnIncludeAccents.TabStop = true;
            this.rbtnIncludeAccents.Text = "Include accents in the copied text";
            this.rbtnIncludeAccents.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnExcludeReference);
            this.groupBox2.Controls.Add(this.rbtnIncludeReference);
            this.groupBox2.Location = new System.Drawing.Point(188, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 82);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reference: ";
            // 
            // rbtnExcludeReference
            // 
            this.rbtnExcludeReference.AutoSize = true;
            this.rbtnExcludeReference.Location = new System.Drawing.Point(20, 46);
            this.rbtnExcludeReference.Name = "rbtnExcludeReference";
            this.rbtnExcludeReference.Size = new System.Drawing.Size(142, 17);
            this.rbtnExcludeReference.TabIndex = 3;
            this.rbtnExcludeReference.Text = "Don\'t include in the copy";
            this.rbtnExcludeReference.UseVisualStyleBackColor = true;
            // 
            // rbtnIncludeReference
            // 
            this.rbtnIncludeReference.AutoSize = true;
            this.rbtnIncludeReference.Checked = true;
            this.rbtnIncludeReference.Location = new System.Drawing.Point(20, 23);
            this.rbtnIncludeReference.Name = "rbtnIncludeReference";
            this.rbtnIncludeReference.Size = new System.Drawing.Size(115, 17);
            this.rbtnIncludeReference.TabIndex = 2;
            this.rbtnIncludeReference.TabStop = true;
            this.rbtnIncludeReference.Text = "Include in the copy";
            this.rbtnIncludeReference.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnCopyToNotes);
            this.groupBox1.Controls.Add(this.rbtnCopyToMemory);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 70);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Copy the word to: ";
            // 
            // rbtnCopyToNotes
            // 
            this.rbtnCopyToNotes.AutoSize = true;
            this.rbtnCopyToNotes.Location = new System.Drawing.Point(20, 42);
            this.rbtnCopyToNotes.Name = "rbtnCopyToNotes";
            this.rbtnCopyToNotes.Size = new System.Drawing.Size(53, 17);
            this.rbtnCopyToNotes.TabIndex = 1;
            this.rbtnCopyToNotes.Text = "Notes";
            this.rbtnCopyToNotes.UseVisualStyleBackColor = true;
            // 
            // rbtnCopyToMemory
            // 
            this.rbtnCopyToMemory.AutoSize = true;
            this.rbtnCopyToMemory.Checked = true;
            this.rbtnCopyToMemory.Location = new System.Drawing.Point(20, 19);
            this.rbtnCopyToMemory.Name = "rbtnCopyToMemory";
            this.rbtnCopyToMemory.Size = new System.Drawing.Size(115, 17);
            this.rbtnCopyToMemory.TabIndex = 0;
            this.rbtnCopyToMemory.TabStop = true;
            this.rbtnCopyToMemory.Text = "Memory (Clipboard)";
            this.rbtnCopyToMemory.UseVisualStyleBackColor = true;
            // 
            // frmCopyOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(674, 194);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkRemember);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmCopyOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copy Options";
            this.pnlBase.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkRemember;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbtnExcludeAccents;
        private System.Windows.Forms.RadioButton rbtnIncludeAccents;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnExcludeReference;
        private System.Windows.Forms.RadioButton rbtnIncludeReference;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnCopyToNotes;
        private System.Windows.Forms.RadioButton rbtnCopyToMemory;
    }
}