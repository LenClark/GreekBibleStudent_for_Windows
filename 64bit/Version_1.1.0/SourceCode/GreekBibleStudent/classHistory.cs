using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GreekBibleStudent
{
    public class classHistory
    {
        classGlobal globalVars;
        classNTText ntText;
        classLXXText lxxText;
        frmProgress progressForm;

        private delegate void performComboBoxUpdate(ComboBox targetCB, String comboItem);
        private delegate void performComboBoxSelection(ComboBox targetCB, int itemIndex);
        private delegate void performComboBoxClear(ComboBox targetCB);
        private delegate void performProgressAdvance(String primaryMessage, String secondaryMessage, bool useSecondary);
        private delegate void performMainFormLabelChange(Label labLabelLbl, String newText);

        private void addComboItem(ComboBox targetCB, String cbItem)
        {
            targetCB.Items.Add(cbItem);
        }

        private void selectComboItem(ComboBox targetCB, int itemIndex)
        {
            if (targetCB.Items.Count > itemIndex)
            {
                targetCB.Text = targetCB.Items[0].ToString();
                //targetCB.SelectedIndex = itemIndex;
            }
        }

        private void clearCombobox(ComboBox targetCB)
        {
            targetCB.Items.Clear();
        }

        private void updateProgress(String mainMessage, String secondaryMessage, bool useSecondary)
        {
            progressForm.incrementProgress(mainMessage, secondaryMessage, useSecondary);
        }

        private void changeLabel(Label labLabelLbl, String newText)
        {
            labLabelLbl.Text = newText;
        }

        public classHistory(classGlobal inGlobal, classNTText inNtText, classLXXText inLxxText, frmProgress inForm)
        {
            globalVars = inGlobal;
            ntText = inNtText;
            lxxText = inLxxText;
            progressForm = inForm;
        }

        public void loadHistory()
        {
            /*============================================================================================*
             *                                                                                            *
             *                                       loadHistory                                          *
             *                                       ===========                                          *
             *                                                                                            *
             *  This is called only once, at the start of the application.  Because it is called in a     *
             *    background thread, we cannot use addEntryToHistory, which is a method designed for use  *
             *    throughout the life of the application. However, we _can_ assume that the target        *
             *    Combo boxes are currently empty.                                                        *
             *                                                                                            *
             *============================================================================================*/

            int idx;
            String historyFileName, fileBuffer;
            FileInfo fiHistory;
            StreamReader srHistory;
            ComboBox cbHistory;

            progressForm.Invoke(new performProgressAdvance(updateProgress), "Loading History", "", false);
            for (idx = 0; idx < 2; idx++)
            {
                if (idx == 0) historyFileName = globalVars.NotesPath + @"\NTHistory.txt";
                else historyFileName = globalVars.NotesPath + @"\LXXHistory.txt";
                fiHistory = new FileInfo(historyFileName);
                cbHistory = globalVars.getComboBoxItem(idx + 6);
                if (fiHistory.Exists)
                {
                    srHistory = new StreamReader(historyFileName);
                    fileBuffer = srHistory.ReadLine();
                    if ((fileBuffer != null) && (fileBuffer[0] == ';')) fileBuffer = srHistory.ReadLine();
                    while (fileBuffer != null)
                    {
                        cbHistory.Invoke(new performComboBoxUpdate(addComboItem), cbHistory, fileBuffer);
                        fileBuffer = srHistory.ReadLine();
                    }
                    srHistory.Close();
                    srHistory.Dispose();
                    if (cbHistory.Items.Count == 0)
                    {
                        if (idx == 0) fileBuffer = "Matthew 1";
                        else fileBuffer = "Genesis 1";
                        cbHistory.Invoke(new performComboBoxUpdate(addComboItem), cbHistory, fileBuffer);
                    }
                    else
                    {
                        cbHistory.Invoke(new performComboBoxSelection(selectComboItem), cbHistory, 0);
                    }
                }
                else
                {
                    if (idx == 0) cbHistory.Invoke(new performComboBoxUpdate(addComboItem), cbHistory, "Matthew 1");
                    else cbHistory.Invoke(new performComboBoxUpdate(addComboItem), cbHistory, "Genesis 1");
                    cbHistory.Invoke(new performComboBoxSelection(selectComboItem), cbHistory, 0);
                }
            }
            progressForm.Invoke(new performProgressAdvance(updateProgress), "History Load Complete", "", false);
            globalVars.getLabel(2).Invoke(new performMainFormLabelChange(changeLabel), globalVars.getLabel(2), "History Load Complete");
        }

        public Tuple<int, String> getBookAndChapter( int ntOrLxx)
        {
            int nPstn, bookIndex = -1;
            String fullRef, bookRef, chapRef;
            ComboBox currentTarget;

            switch( ntOrLxx)
            {
                case 1: currentTarget = globalVars.getComboBoxItem(6); break;
                case 2: currentTarget = globalVars.getComboBoxItem(7); break;
                default: return new Tuple<int, string>(-1, "");
            }
            fullRef = currentTarget.Text;
            if (fullRef.Length > 0)
            {
                nPstn = fullRef.LastIndexOf(' ');
                if (nPstn > -1)
                {
                    bookRef = fullRef.Substring(0, nPstn);
                    chapRef = fullRef.Substring(nPstn + 1);
                }
                else
                {
                    bookRef = "";
                    chapRef = "";
                }
            }
            else
            {
                bookRef = "";
                chapRef = "";
            }
            if (bookRef.Length == 0) return new Tuple<int, string>(-1, "");
            foreach( KeyValuePair<int, classBook> bookPair in globalVars.BookList)
            {
                if( String.Compare( bookPair.Value.BookName, bookRef ) == 0 )
                {
                    bookIndex = bookPair.Key;
                    break;
                }
            }
            return new Tuple<int, string>(bookIndex, chapRef);
        }

        public void addEntryToHistory(String newEntry, int actionCode, int ntOrLxx)
        {
            /*======================================================================================================*
             *                                                                                                      *
             *                                           addEntryToHistory                                          *
             *                                           -----------------                                          *
             *                                                                                                      *
             *  This adds an entry to either of the history combo boxes.                                            *
             *                                                                                                      *
             *  Parameters:                                                                                         *
             *  ----------                                                                                          *
             *                                                                                                      *
             *  newEntry          The string that will be entered                                                   *
             *  actionCode        An integer value specifying whether to add the entry at the head of the list or   *
             *                    at the tail;                                                                      *
             *                      0 = head         1 - tail                                                       *
             *  ntOrLxx           Identifies whether the entry is from the NT or LXX                                *
             *                      0 = NT           1 = LXX                                                        *
             *                                                                                                      *
             *  The statement:                                                                                      *
             *             addEntryToHistory(displayString, 0, false);                                              *
             *  will remove a pre-existing entry for the book and chapter, if it exists, and, whether it existed    *
             *  before or not, it will add i.  This means that the event method, cbHistory_SelectedIndexChanged,    *
             *  will be invoked and this, in turn, will call mainText.processSelectedHistory.  This will finish by  *
             *  calling displayChapter which, as before, sets up a loop.  So, once again, we can use                *
             *  isChapUpdateActive to prevent the loop occurring                                                    *
             *                                                                                                      *
             *======================================================================================================*/
            ComboBox cbHistory;

            if (ntOrLxx == 0) cbHistory = globalVars.getComboBoxItem(6);
            else cbHistory = globalVars.getComboBoxItem(7);
            if (cbHistory.Items.Contains(newEntry))
            {
                cbHistory.Items.Remove(newEntry);
            }
            if (cbHistory.Items.Count >= globalVars.HistoryMax)
            {
                cbHistory.Items.RemoveAt(cbHistory.Items.Count - 1);
            }
            if (actionCode == 0) cbHistory.Items.Insert(0, newEntry);
            else cbHistory.Items.Add(newEntry);
            globalVars.IsReady = false;
            cbHistory.SelectedIndex = 0;
            globalVars.IsReady = true;
        }
    }
}
