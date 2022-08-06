using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    public partial class frmRelocate : Form
    {
        String selectedDestination;

        public frmRelocate()
        {
            InitializeComponent();
        }

        public string SelectedDestination { get => selectedDestination; set => selectedDestination = value; }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if( dlgLocation.ShowDialog() == DialogResult.OK)
            {
                rtxtSelectedDestination.Text = dlgLocation.SelectedPath;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            selectedDestination = rtxtSelectedDestination.Text;
            Close();
        }
    }
}
