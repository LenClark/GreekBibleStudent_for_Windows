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
    public partial class frmMove : Form
    {
        String selectedDestination;

        public string SelectedDestination { get => selectedDestination; set => selectedDestination = value; }

        public frmMove()
        {
            InitializeComponent();
        }

        public void registerDestination(String initialDestination)
        {
            selectedDestination = initialDestination;
            labSelectedDestinationMsg.Text = initialDestination;
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dlgLocation.ShowDialog() == DialogResult.OK)
            {
                labSelectedDestinationMsg.Text = dlgLocation.SelectedPath;
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
            selectedDestination = labSelectedDestinationMsg.Text;
            Close();
        }
    }
}
