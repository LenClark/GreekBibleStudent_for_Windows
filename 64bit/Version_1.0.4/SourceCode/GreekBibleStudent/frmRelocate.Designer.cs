
namespace GreekBibleStudent
{
    partial class frmRelocate
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labExplanation = new System.Windows.Forms.Label();
            this.labExplanation2 = new System.Windows.Forms.Label();
            this.labExplanation3 = new System.Windows.Forms.Label();
            this.labExplanation4 = new System.Windows.Forms.Label();
            this.labExplanation5 = new System.Windows.Forms.Label();
            this.labExplanation6 = new System.Windows.Forms.Label();
            this.labExplanation7 = new System.Windows.Forms.Label();
            this.labReason1 = new System.Windows.Forms.Label();
            this.labReason2 = new System.Windows.Forms.Label();
            this.labReason3 = new System.Windows.Forms.Label();
            this.labExplanation8 = new System.Windows.Forms.Label();
            this.labDestinationLbl = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.labDestinationSelectedLbl = new System.Windows.Forms.Label();
            this.dlgLocation = new System.Windows.Forms.FolderBrowserDialog();
            this.rtxtSelectedDestination = new System.Windows.Forms.RichTextBox();
            this.pnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.btnCancel);
            this.pnlBase.Controls.Add(this.btnOK);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBase.Location = new System.Drawing.Point(0, 331);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(560, 30);
            this.pnlBase.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(473, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Move";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Don\'t move";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labExplanation
            // 
            this.labExplanation.AutoSize = true;
            this.labExplanation.Location = new System.Drawing.Point(12, 20);
            this.labExplanation.Name = "labExplanation";
            this.labExplanation.Size = new System.Drawing.Size(263, 13);
            this.labExplanation.TabIndex = 1;
            this.labExplanation.Text = "The application depends on a large number of files for:";
            // 
            // labExplanation2
            // 
            this.labExplanation2.AutoSize = true;
            this.labExplanation2.Location = new System.Drawing.Point(40, 38);
            this.labExplanation2.Name = "labExplanation2";
            this.labExplanation2.Size = new System.Drawing.Size(181, 13);
            this.labExplanation2.TabIndex = 2;
            this.labExplanation2.Text = "a)  the Greek text (both NT and LXX)";
            // 
            // labExplanation3
            // 
            this.labExplanation3.AutoSize = true;
            this.labExplanation3.Location = new System.Drawing.Point(40, 54);
            this.labExplanation3.Name = "labExplanation3";
            this.labExplanation3.Size = new System.Drawing.Size(97, 13);
            this.labExplanation3.TabIndex = 3;
            this.labExplanation3.Text = "b)  the lexicon data";
            // 
            // labExplanation4
            // 
            this.labExplanation4.AutoSize = true;
            this.labExplanation4.Location = new System.Drawing.Point(40, 70);
            this.labExplanation4.Name = "labExplanation4";
            this.labExplanation4.Size = new System.Drawing.Size(44, 13);
            this.labExplanation4.TabIndex = 4;
            this.labExplanation4.Text = "c)  Help";
            // 
            // labExplanation5
            // 
            this.labExplanation5.AutoSize = true;
            this.labExplanation5.Location = new System.Drawing.Point(40, 86);
            this.labExplanation5.Name = "labExplanation5";
            this.labExplanation5.Size = new System.Drawing.Size(210, 13);
            this.labExplanation5.TabIndex = 5;
            this.labExplanation5.Text = "d)  a few additional elements of information.";
            // 
            // labExplanation6
            // 
            this.labExplanation6.AutoSize = true;
            this.labExplanation6.Location = new System.Drawing.Point(12, 106);
            this.labExplanation6.Name = "labExplanation6";
            this.labExplanation6.Size = new System.Drawing.Size(346, 13);
            this.labExplanation6.TabIndex = 6;
            this.labExplanation6.Text = "If you have created any notes, they are also stored in the same location.";
            // 
            // labExplanation7
            // 
            this.labExplanation7.AutoSize = true;
            this.labExplanation7.Location = new System.Drawing.Point(12, 124);
            this.labExplanation7.Name = "labExplanation7";
            this.labExplanation7.Size = new System.Drawing.Size(519, 13);
            this.labExplanation7.TabIndex = 7;
            this.labExplanation7.Text = "By default, all these files are stored in your User profile.  You may want to mov" +
    "e these for a variety of reasons:";
            // 
            // labReason1
            // 
            this.labReason1.AutoSize = true;
            this.labReason1.Location = new System.Drawing.Point(40, 142);
            this.labReason1.Name = "labReason1";
            this.labReason1.Size = new System.Drawing.Size(222, 13);
            this.labReason1.TabIndex = 8;
            this.labReason1.Text = "a)  Simply to make the notes more accessible;";
            // 
            // labReason2
            // 
            this.labReason2.AutoSize = true;
            this.labReason2.Location = new System.Drawing.Point(40, 160);
            this.labReason2.Name = "labReason2";
            this.labReason2.Size = new System.Drawing.Size(213, 13);
            this.labReason2.TabIndex = 9;
            this.labReason2.Text = "b)  To make them accessible to other users;";
            // 
            // labReason3
            // 
            this.labReason3.AutoSize = true;
            this.labReason3.Location = new System.Drawing.Point(40, 178);
            this.labReason3.Name = "labReason3";
            this.labReason3.Size = new System.Drawing.Size(170, 13);
            this.labReason3.TabIndex = 10;
            this.labReason3.Text = "c)  To move them off the C:\\ drive.";
            // 
            // labExplanation8
            // 
            this.labExplanation8.AutoSize = true;
            this.labExplanation8.Location = new System.Drawing.Point(12, 196);
            this.labExplanation8.Name = "labExplanation8";
            this.labExplanation8.Size = new System.Drawing.Size(162, 13);
            this.labExplanation8.TabIndex = 11;
            this.labExplanation8.Text = "This facility allows you to do that.";
            // 
            // labDestinationLbl
            // 
            this.labDestinationLbl.AutoSize = true;
            this.labDestinationLbl.Location = new System.Drawing.Point(15, 253);
            this.labDestinationLbl.Name = "labDestinationLbl";
            this.labDestinationLbl.Size = new System.Drawing.Size(285, 13);
            this.labDestinationLbl.TabIndex = 12;
            this.labDestinationLbl.Text = "Select the place to which you would like to move your files:";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(306, 248);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 13;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // labDestinationSelectedLbl
            // 
            this.labDestinationSelectedLbl.AutoSize = true;
            this.labDestinationSelectedLbl.Location = new System.Drawing.Point(43, 281);
            this.labDestinationSelectedLbl.Name = "labDestinationSelectedLbl";
            this.labDestinationSelectedLbl.Size = new System.Drawing.Size(106, 13);
            this.labDestinationSelectedLbl.TabIndex = 14;
            this.labDestinationSelectedLbl.Text = "Selected destination:";
            // 
            // dlgLocation
            // 
            this.dlgLocation.Description = "Select the folder to which you want to move the Application files.";
            this.dlgLocation.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // rtxtSelectedDestination
            // 
            this.rtxtSelectedDestination.Location = new System.Drawing.Point(150, 278);
            this.rtxtSelectedDestination.Name = "rtxtSelectedDestination";
            this.rtxtSelectedDestination.ReadOnly = true;
            this.rtxtSelectedDestination.Size = new System.Drawing.Size(398, 47);
            this.rtxtSelectedDestination.TabIndex = 15;
            this.rtxtSelectedDestination.Text = "Not chosen yet";
            // 
            // frmRelocate
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 361);
            this.ControlBox = false;
            this.Controls.Add(this.rtxtSelectedDestination);
            this.Controls.Add(this.labDestinationSelectedLbl);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.labDestinationLbl);
            this.Controls.Add(this.labExplanation8);
            this.Controls.Add(this.labReason3);
            this.Controls.Add(this.labReason2);
            this.Controls.Add(this.labReason1);
            this.Controls.Add(this.labExplanation7);
            this.Controls.Add(this.labExplanation6);
            this.Controls.Add(this.labExplanation5);
            this.Controls.Add(this.labExplanation4);
            this.Controls.Add(this.labExplanation3);
            this.Controls.Add(this.labExplanation2);
            this.Controls.Add(this.labExplanation);
            this.Controls.Add(this.pnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmRelocate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Move the location of Application files";
            this.pnlBase.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labExplanation;
        private System.Windows.Forms.Label labExplanation2;
        private System.Windows.Forms.Label labExplanation3;
        private System.Windows.Forms.Label labExplanation4;
        private System.Windows.Forms.Label labExplanation5;
        private System.Windows.Forms.Label labExplanation6;
        private System.Windows.Forms.Label labExplanation7;
        private System.Windows.Forms.Label labReason1;
        private System.Windows.Forms.Label labReason2;
        private System.Windows.Forms.Label labReason3;
        private System.Windows.Forms.Label labExplanation8;
        private System.Windows.Forms.Label labDestinationLbl;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label labDestinationSelectedLbl;
        private System.Windows.Forms.FolderBrowserDialog dlgLocation;
        private System.Windows.Forms.RichTextBox rtxtSelectedDestination;
    }
}