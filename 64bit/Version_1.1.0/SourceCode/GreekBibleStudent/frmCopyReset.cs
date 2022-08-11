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
    public partial class frmCopyReset : Form
    {
        int isNTWordChecked = 0, isNTVerseChecked = 0, isNTChapterChecked = 0, isNTSelectionChecked = 0,
            isLXXWordChecked = 0, isLXXVerseChecked = 0, isLXXChapterChecked = 0, isLXXSelectionChecked = 0;
        classGlobal globalVars;

        public int IsNTWordChecked { get => isNTWordChecked; set => isNTWordChecked = value; }
        public int IsNTVerseChecked { get => isNTVerseChecked; set => isNTVerseChecked = value; }
        public int IsNTChapterChecked { get => isNTChapterChecked; set => isNTChapterChecked = value; }
        public int IsNTSelectionChecked { get => isNTSelectionChecked; set => isNTSelectionChecked = value; }
        public int IsLXXWordChecked { get => isLXXWordChecked; set => isLXXWordChecked = value; }
        public int IsLXXVerseChecked { get => isLXXVerseChecked; set => isLXXVerseChecked = value; }
        public int IsLXXChapterChecked { get => isLXXChapterChecked; set => isLXXChapterChecked = value; }
        public int IsLXXSelectionChecked { get => isLXXSelectionChecked; set => isLXXSelectionChecked = value; }

        public frmCopyReset( classGlobal inGlobal)
        {
            InitializeComponent();
            globalVars = inGlobal;
        }

        public void populateCheckboxes()
        {
            if (isNTWordChecked == 0) chkNTWord.Enabled = false;
            else
            {
                chkNTWord.Enabled = true;
                chkNTWord.Checked = (isNTWordChecked == 2);
            }
            if (isNTVerseChecked == 0) chkNTVerse.Enabled = false;
            else
            {
                chkNTVerse.Enabled = true;
                chkNTVerse.Checked = (isNTVerseChecked == 2);
            }
            if (isNTChapterChecked == 0) chkNTChapter.Enabled = false;
            else
            {
                chkNTChapter.Enabled = true;
                chkNTChapter.Checked = (isNTChapterChecked == 2);
            }
            if (isNTSelectionChecked == 0) chkNTSelection.Enabled = false;
            else
            {
                chkNTSelection.Enabled = true;
                chkNTSelection.Checked = (isNTSelectionChecked == 2);
            }
            if (isLXXWordChecked == 0) chkLXXWord.Enabled = false;
            else
            {
                chkLXXWord.Enabled = true;
                chkLXXWord.Checked = (isLXXWordChecked == 2);
            }
            if (isLXXVerseChecked == 0) chkLXXVerse.Enabled = false;
            else
            {
                chkLXXVerse.Enabled = true;
                chkLXXVerse.Checked = (isLXXVerseChecked == 2);
            }
            if (isLXXChapterChecked == 0) chkLXXChapter.Enabled = false;
            else
            {
                chkLXXChapter.Enabled = true;
                chkLXXChapter.Checked = (isLXXChapterChecked == 2);
            }
            if (isLXXSelectionChecked == 0) chkLXXSelection.Enabled = false;
            else
            {
                chkLXXSelection.Enabled = true;
                chkLXXSelection.Checked = (isLXXSelectionChecked == 2);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (chkNTWord.Enabled)
            {
                if (chkNTWord.Checked) isNTWordChecked = 2;
                else isNTWordChecked = 1;
            }
            else isNTWordChecked = 0;
            if (chkNTVerse.Enabled)
            {
                if (chkNTVerse.Checked) isNTVerseChecked = 2;
                else isNTVerseChecked = 1;
            }
            else isNTVerseChecked = 0;
            if (chkNTChapter.Enabled)
            {
                if (chkNTChapter.Checked) isNTChapterChecked = 2;
                else isNTChapterChecked = 1;
            }
            else isNTChapterChecked = 0;
            if (chkNTSelection.Enabled)
            {
                if (chkNTSelection.Checked) isNTSelectionChecked = 2;
                else isNTSelectionChecked = 1;
            }
            else isNTSelectionChecked = 0;
            if (chkLXXWord.Enabled)
            {
                if (chkLXXWord.Checked) isLXXWordChecked = 2;
                else isLXXWordChecked = 1;
            }
            else isLXXWordChecked = 0;
            if (chkLXXVerse.Enabled)
            {
                if (chkLXXVerse.Checked) isLXXVerseChecked = 2;
                else isLXXVerseChecked = 1;
            }
            else isLXXVerseChecked = 0;
            if (chkLXXChapter.Enabled)
            {
                if (chkLXXChapter.Checked) isLXXChapterChecked = 2;
                else isLXXChapterChecked = 1;
            }
            else isLXXChapterChecked = 0;
            if (chkLXXSelection.Enabled)
            {
                if (chkLXXSelection.Checked) isLXXSelectionChecked = 2;
                else isLXXSelectionChecked = 1;
            }
            else isLXXSelectionChecked = 0;
            DialogResult = DialogResult.OK;
            Close();
        }

        public int enableReset()
        {
            bool isUseful = false;
            int rememberCount = 0;

            isNTWordChecked = globalVars.getCopyOption(0, 0).Item4;
            isNTVerseChecked = globalVars.getCopyOption(1, 0).Item4;
            isNTChapterChecked = globalVars.getCopyOption(2, 0).Item4;
            isNTSelectionChecked = globalVars.getCopyOption(3, 0).Item4;
            isLXXWordChecked = globalVars.getCopyOption(0, 1).Item4;
            isLXXVerseChecked = globalVars.getCopyOption(1, 1).Item4;
            isLXXChapterChecked = globalVars.getCopyOption(2, 1).Item4;
            isLXXSelectionChecked = globalVars.getCopyOption(3, 1).Item4;
            if (isNTWordChecked + isNTVerseChecked + isNTChapterChecked + isNTSelectionChecked +
                isLXXWordChecked + isLXXVerseChecked + isLXXChapterChecked + isLXXSelectionChecked > 0) isUseful = true;
            if (isUseful)
            {
                populateCheckboxes();
                if (ShowDialog() == DialogResult.OK)
                {
                    globalVars.setCopyOptions(0, 3, isNTWordChecked, 0);
                    globalVars.setCopyOptions(1, 3, isNTVerseChecked, 0);
                    globalVars.setCopyOptions(2, 3, isNTChapterChecked, 0);
                    globalVars.setCopyOptions(3, 3, isNTSelectionChecked, 0);
                    globalVars.setCopyOptions(0, 3, isLXXWordChecked, 1);
                    globalVars.setCopyOptions(1, 3, isLXXVerseChecked, 1);
                    globalVars.setCopyOptions(2, 3, isLXXChapterChecked, 1);
                    globalVars.setCopyOptions(3, 3, isLXXSelectionChecked, 1);
                    if (isNTWordChecked == 2) rememberCount++;
                    if (isNTVerseChecked == 2) rememberCount++;
                    if (isNTChapterChecked == 2) rememberCount++;
                    if (isNTSelectionChecked == 2) rememberCount++;
                    if (isLXXWordChecked == 2) rememberCount++;
                    if (isLXXVerseChecked == 2) rememberCount++;
                    if (isLXXChapterChecked == 2) rememberCount++;
                    if (isLXXSelectionChecked == 2) rememberCount++;
                }
            }
            return rememberCount;
        }
    }
}
