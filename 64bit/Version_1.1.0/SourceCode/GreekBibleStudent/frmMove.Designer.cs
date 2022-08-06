namespace GreekBibleStudent
{
    partial class frmMove
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMove));
            this.pnlBase = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.labSelectedDestinationMsg = new System.Windows.Forms.Label();
            this.labDestinationSelectedLbl = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.labDestinationLbl = new System.Windows.Forms.Label();
            this.labExplanationLbl = new System.Windows.Forms.Label();
            this.dlgLocation = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.btnCancel);
            this.pnlBase.Controls.Add(this.btnOK);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBase.Location = new System.Drawing.Point(0, 325);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(800, 30);
            this.pnlBase.TabIndex = 2;
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
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(713, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Move";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pbIcon
            // 
            this.pbIcon.Image = ((System.Drawing.Image)(resources.GetObject("pbIcon.Image")));
            this.pbIcon.Location = new System.Drawing.Point(548, 12);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(240, 258);
            this.pbIcon.TabIndex = 21;
            this.pbIcon.TabStop = false;
            // 
            // labSelectedDestinationMsg
            // 
            this.labSelectedDestinationMsg.AutoSize = true;
            this.labSelectedDestinationMsg.Location = new System.Drawing.Point(152, 290);
            this.labSelectedDestinationMsg.Name = "labSelectedDestinationMsg";
            this.labSelectedDestinationMsg.Size = new System.Drawing.Size(33, 13);
            this.labSelectedDestinationMsg.TabIndex = 26;
            this.labSelectedDestinationMsg.Text = "None";
            // 
            // labDestinationSelectedLbl
            // 
            this.labDestinationSelectedLbl.AutoSize = true;
            this.labDestinationSelectedLbl.Location = new System.Drawing.Point(40, 290);
            this.labDestinationSelectedLbl.Name = "labDestinationSelectedLbl";
            this.labDestinationSelectedLbl.Size = new System.Drawing.Size(106, 13);
            this.labDestinationSelectedLbl.TabIndex = 25;
            this.labDestinationSelectedLbl.Text = "Selected destination:";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(303, 257);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 24;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // labDestinationLbl
            // 
            this.labDestinationLbl.AutoSize = true;
            this.labDestinationLbl.Location = new System.Drawing.Point(12, 262);
            this.labDestinationLbl.Name = "labDestinationLbl";
            this.labDestinationLbl.Size = new System.Drawing.Size(285, 13);
            this.labDestinationLbl.TabIndex = 23;
            this.labDestinationLbl.Text = "Select the place to which you would like to move your files:";
            // 
            // labExplanationLbl
            // 
            this.labExplanationLbl.Location = new System.Drawing.Point(12, 12);
            this.labExplanationLbl.Name = "labExplanationLbl";
            this.labExplanationLbl.Size = new System.Drawing.Size(499, 198);
            this.labExplanationLbl.TabIndex = 22;
            this.labExplanationLbl.Text = resources.GetString("labExplanationLbl.Text");
            // 
            // dlgLocation
            // 
            this.dlgLocation.Description = "Select the folder to which you want to move the Application files.";
            this.dlgLocation.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // frmMove
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 355);
            this.ControlBox = false;
            this.Controls.Add(this.labSelectedDestinationMsg);
            this.Controls.Add(this.labDestinationSelectedLbl);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.labDestinationLbl);
            this.Controls.Add(this.labExplanationLbl);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.pnlBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMove";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Move Application Source Files";
            this.pnlBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label labSelectedDestinationMsg;
        private System.Windows.Forms.Label labDestinationSelectedLbl;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label labDestinationLbl;
        private System.Windows.Forms.Label labExplanationLbl;
        private System.Windows.Forms.FolderBrowserDialog dlgLocation;
    }
}