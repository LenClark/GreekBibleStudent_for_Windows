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
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
        }

        public void incrementProgress(String mainMessage, String secondaryMessage, bool useSecondary)
        {
            labProgressAction1Msg.Text = mainMessage;
            if (useSecondary) labProgressAction2Msg.Text = secondaryMessage;
            pbProgress.Increment(1);
        }
    }
}
