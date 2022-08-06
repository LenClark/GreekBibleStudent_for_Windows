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
    public partial class frmCopyOptions : Form
    {
        /*========================================================================================================*
         *                                                                                                        *
         *                                           frmCopyOptions                                               *
         *                                           ==============                                               *
         *                                                                                                        *
         *  Controls and performs the various copying options, as provided in the context menu of the two main    *
         *    text areas.                                                                                         *
         *                                                                                                        *
         *  Parameters:                                                                                           *
         *  ==========                                                                                            *
         *                                                                                                        *
         *  typeOfCopy    Basically, which of the options was selected:                                           *
         *                                                                                                        *
         *       Code                          Significance                                                       *
         *         1         Copy the currently selected word (or the word the user has clicked on                *
         *         2         Copy the current verse                                                               *
         *         3         Copy the whole chapter                                                               *
         *         4         Copy the user-selected text (replaces ^C)                                            *
         *                                                                                                        *
         *  whichVersion  Identifies whether the copy is in the MT or LXX                                         *
         *         0         NT                                                                                   *
         *         1         LXX                                                                                  *
         *                                                                                                        *
         *========================================================================================================*/

        int typeOfCopy, whichVersion;

        /*-----------------------------------------------------------------------------------*
         *                                                                                   *
         *                                Controlling codes                                  *
         *                                 ----------------                                  *
         *                                                        Values                     *
         *     Code            Meaning                      1                 2              *
         *                                                                                   *
         *   destCode     copyDestination               Clipboard           Notes            *
         *   refCode      referenceIncluded             Include             Exclude          *
         *   accentCode   AccentsIncluded               Include             Exclude          *
         *   rememberCode Remember and use settings     Don't use           Use              *
         *   whichVersion Flags source as NT or LXX     NT = 0              LXX = 16         *
         *                                                                                   *
         *-----------------------------------------------------------------------------------*/
        int destCode, refCode, accentCode, rememberCode, rememberCount;
        String referenceText, selectedText;

        classGlobal globalVars;
        classGreek greekMethods;
        classNote noteProcs;

        public int TypeOfCopy { get => typeOfCopy; set => typeOfCopy = value; }
        public int WhichVersion { get => whichVersion; set => whichVersion = value; }
        public int DestCode { get => destCode; set => destCode = value; }
        public int RefCode { get => refCode; set => refCode = value; }
        public int AccentCode { get => accentCode; set => accentCode = value; }
        public int RememberCode { get => rememberCode; set => rememberCode = value; }
        public int RememberCount { get => rememberCount; set => rememberCount = value; }

        public frmCopyOptions(int copyCode, int mtOrLxx, classGlobal inGlobal, classGreek inGkProcs, classNote inNote)
        {
            InitializeComponent();
            typeOfCopy = copyCode;
            whichVersion = mtOrLxx;
            globalVars = inGlobal;
            greekMethods = inGkProcs;
            noteProcs = inNote;
            referenceText = formReference();
            getRelevantText();
            switch (copyCode)
            {
                case 1:
                case 2: this.Text = "Copying " + referenceText + " - " + selectedText; break;
                case 3: this.Text = "Copying " + referenceText; break;
                case 4: this.Text = "Copying a selection from " + referenceText; break;
            }
        }

        public DialogResult processMenuClick()
        {
            int idx;
            Tuple<int, int, int, int> optionsReturned;
            DialogResult result;

            result = DialogResult.Cancel;
            optionsReturned = globalVars.getCopyOption(typeOfCopy - 1, whichVersion);
            if (optionsReturned.Item4 == 2)
            {
                destCode = optionsReturned.Item1;
                refCode = optionsReturned.Item2;
                accentCode = optionsReturned.Item3;
                rememberCode = optionsReturned.Item4;
                performCopy();
            }
            else result = ShowDialog();
            for (idx = 0; idx < 4; idx++)
            {
                optionsReturned = globalVars.getCopyOption(idx, 0);
                if (optionsReturned.Item4 == 2) rememberCount++;
                optionsReturned = globalVars.getCopyOption(idx, 1);
                if (optionsReturned.Item4 == 2) rememberCount++;
            }
            return result;
        }

        private String formReference()
        {
            ComboBox cbBook;
            String refText = "", bookName, chapNo, vNo;
            classBook currentNTBook, currentLXXBook;

            switch (whichVersion)
            {
                case 0:
                    cbBook = globalVars.getComboBoxItem(0);
                    globalVars.BookList.TryGetValue(cbBook.SelectedIndex + 59, out currentNTBook);
                    bookName = currentNTBook.BookName;
                    chapNo = globalVars.getComboBoxItem(1).SelectedItem.ToString();
                    vNo = globalVars.getComboBoxItem(2).SelectedItem.ToString();
                    switch (typeOfCopy)
                    {
                        case 1:
                        case 2: refText = bookName + " " + chapNo + "." + vNo; break;
                        case 3:
                        case 4: refText = bookName + " " + chapNo; break;
                    }
                    break;
                case 1:
                    cbBook = globalVars.getComboBoxItem(3);
                    globalVars.BookList.TryGetValue(cbBook.SelectedIndex, out currentLXXBook);
                    bookName = currentLXXBook.BookName;
                    chapNo = globalVars.getComboBoxItem(4).SelectedItem.ToString();
                    vNo = globalVars.getComboBoxItem(5).SelectedItem.ToString();
                    switch (typeOfCopy)
                    {
                        case 1:
                        case 2: refText = bookName + " " + chapNo + "." + vNo; break;
                        case 3:
                        case 4: refText = bookName + " " + chapNo; break;
                    }
                    break;
            }
            return refText;
        }

        private void getRelevantText()
        {
            int nStart, nLength;
            RichTextBox targetTextArea = null;

            targetTextArea = globalVars.getRichtextItem(0);
            switch (typeOfCopy)
            {
                case 1:
                    if (whichVersion == 0) selectedText = globalVars.LastSelectedNTWord;
                    else selectedText = globalVars.LastSelectedLXXWord;
                    break;
                case 2:
                    if (whichVersion == 0) selectedText = globalVars.LastSelectedNTVerse;
                    else selectedText = globalVars.LastSelectedLXXVerse;
                    if ((selectedText != null) && (selectedText.Length > 0))
                    {
                        nStart = selectedText.IndexOf(":");
                        while (selectedText[++nStart] == ' ') ;
                        selectedText = selectedText.Substring(nStart);
                    }
                    break;
                case 3: selectedText = targetTextArea.Text; break;
                case 4:
                    nStart = targetTextArea.SelectionStart;
                    nLength = targetTextArea.SelectionLength;
                    if (nLength == 0) selectedText = "";
                    else selectedText = targetTextArea.Text.Substring(nStart, nLength); break;
            }
        }

        private void copyWord(int destCode, int refCode, int accentCode)
        {
            const char zeroWidthSpace = '\u200b', zeroWidthNonJoiner = '\u200d';

            String copyWord = "", modifiedWord, informationMessage = "";

            if (whichVersion == 0) copyWord = globalVars.LastSelectedNTWord;
            else copyWord = globalVars.LastSelectedLXXWord;
            modifiedWord = copyWord.Replace(zeroWidthSpace.ToString(), "");
            modifiedWord = modifiedWord.Replace(zeroWidthNonJoiner.ToString(), "");
            if (accentCode == 2) modifiedWord = greekMethods.reduceToBareGreek(modifiedWord, true);
            if (destCode == 1)
            {
                if (refCode == 1) Clipboard.SetText(referenceText + ":  " + modifiedWord);
                else Clipboard.SetText(modifiedWord);
                informationMessage = referenceText + ", " + modifiedWord + "has been copied to the clipboard";
                MessageBox.Show(informationMessage, "Copy of " + copyWord + " successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void setupCodes()
        {
            if (rbtnCopyToMemory.Checked) destCode = 1;
            else destCode = 2;
            if (rbtnIncludeReference.Checked) refCode = 1;
            else refCode = 2;
            if (rbtnIncludeAccents.Checked) accentCode = 1;
            else accentCode = 2;
            if (chkRemember.Checked) rememberCode = 2;
            else rememberCode = 1;
        }

        public void performCopy()
        {
            const char zeroWidthSpace = '\u200b', zeroWidthNonJoiner = '\u200d';

            int idx, noOfWords;
            String modifiedWord, finalText = "", tempText = "", messageText = "";
            Char[] splitter = { ' ' };
            String[] brokenText;

            if (whichVersion < 0) return;
            if ((selectedText == null) || (selectedText.Length == 0))
            {
                switch (typeOfCopy)
                {
                    case 1: messageText = "No word has been selected"; break;
                    case 2: messageText = "No verse has been selected"; break;
                    case 3: messageText = "No chapter has been selected"; break;
                    case 4: messageText = "No selection has been made"; break;
                }
                MessageBox.Show(messageText, "Copy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            globalVars.setCopyOptions((typeOfCopy - 1), 0, destCode, whichVersion);
            globalVars.setCopyOptions((typeOfCopy - 1), 1, refCode, whichVersion);
            globalVars.setCopyOptions((typeOfCopy - 1), 2, accentCode, whichVersion);
            globalVars.setCopyOptions((typeOfCopy - 1), 3, rememberCode, whichVersion);
            modifiedWord = selectedText.Replace(zeroWidthSpace.ToString(), "");
            modifiedWord = modifiedWord.Replace(zeroWidthNonJoiner.ToString(), "");
            switch (accentCode)
            {
                case 1: finalText = modifiedWord; break;
                case 2:
                    brokenText = modifiedWord.Split(splitter);
                    noOfWords = brokenText.Length;
                    for (idx = 0; idx < noOfWords; idx++)
                    {
                        tempText = greekMethods.reduceToBareGreek(brokenText[idx], true);
                        if (idx == 0) finalText = tempText;
                        else finalText += " " + tempText;
                    }
                    break;
            }
            if (refCode == 1) finalText = referenceText + ": " + finalText;
            switch (typeOfCopy)
            {
                case 1: messageText = selectedText; break;
                case 2: messageText = "the verse"; break;
                case 3: messageText = "the chapter"; break;
                case 4: messageText = "the selected text"; break;
            }
            switch (destCode)
            {
                case 1:
                    if (finalText == null)
                    {
                        MessageBox.Show("Your copy string was empty\nThis isn't allowed.", "Copy failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Clipboard.SetText(finalText);
                    MessageBox.Show(referenceText + ": " + messageText + " has been copied to the clipboard", "Copy successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2: noteProcs.insertTextIntoNote(finalText); break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            setupCodes();
            performCopy();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
