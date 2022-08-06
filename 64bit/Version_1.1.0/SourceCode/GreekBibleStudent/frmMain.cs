using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GreekBibleStudent
{
    public partial class frmMain : Form
    {
        /*============================================================================================================================*
         *                                                                                                                            *
         *                                                   GreekBibleStudent                                                        *
         *                                                   =================                                                        *
         *                                                                                                                            *
         *  Greek Bible Student brings together a number of diverse tools that *can* be found out and about but:                      *
         *    a) not all in one place, and                                                                                            *
         *    b) mainly online.                                                                                                       *
         *    So, this allows you to continue to function off-line.  We have also added some extra functionality, such as a           *
         *    reasonably comprehensive search facility (which functions like a concordance) and the ability to create word lists to   *
         *    help learn vocabulary.                                                                                                  *
         *                                                                                                                            *
         *  Effort has been made to keep the code readable and to honour the principals of object-oriented programming.  In           *
         *  particular, specific areas of activity have been allocated to dedicated classes.                                          *
         *                                                                                                                            *
         *  Note, a single class, classConfig (instantiated as globalVars), handles global variables                                  *
         *                                                                                                                            *
         *  Developed: Len Clark                                                                                                      *
         *  Current revision: July 2022                                                                                               *
         *                                                                                                                            *
         *============================================================================================================================*/

        const int keybrdPanelHeight = 225;

        bool isActivated = false;
        int rightPanesControl = 2, searchCursorPstn = 0, textCode = -1, showButtonTag = -1;

        /*-----------------------------------------------------------------------*
         *                                                                       *
         *                                 Classes                               *
         *                                 =======                               *
         *                                                                       *
         *-----------------------------------------------------------------------*/

        classGlobal globalVars;
        classRegistry appRegistry;
        classGreek greekUtilities;
        classNTText ntText;
        classLXXText lxxText;
        classHistory historyProcessing;
        classLexicon greekLexicon;
        classKeyboard keyboard;
        classSearch searchMain;
        classNote note;
        classVocab vocabSetup;

        frmProgress progressForm;

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *   Thread management                                                                                 *
         *   -----------------                                                                                 *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        Thread initialisationThread;
        private delegate void performKeyboardInit(SplitContainer targetSplit);

        private void setKeyboardPosition(SplitContainer targetSplit)
        {
            targetSplit.SplitterDistance = targetSplit.Height - globalVars.PnlFiller.Height;
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            /*===========================================================================================================================*
             *                                                                                                                           *
             *                                                     frmMain_Activated                                                     *
             *                                                     =================                                                     *
             *                                                                                                                           *
             *  This is the controling method for building the form.  We have used form activation rather than the class instantiation   *
             *  because the latter functions *before* the visible form is available and makes the display of progress impossible (or, at *
             *  least, difficult).  We ensure that the form creation elements of the activation method is only called once by means of   *
             *  the variable, isActivated.                                                                                               *
             *                                                                                                                           *
             *===========================================================================================================================*/

            if (!isActivated)
            {
                this.Visible = false;
                progressForm = new frmProgress();
                progressForm.Show();

                /*-------------------------------------------------------------*
                 *  Initialise global classes before anything else             *
                 *-------------------------------------------------------------*/
                globalVars = new classGlobal();
                greekUtilities = new classGreek();
                appRegistry = new classRegistry();
                note = new classNote();

                makeMainFormControlsGlobal();
                vocabSetup = new classVocab(globalVars, note);
                /*-------------------------------------------------------------------*
                  *                                                                   *
                  *  Registry Management and cross-session values                     *
                  *                                                                   *
                  *-------------------------------------------------------------------*/

                appRegistry.MainForm = this;
                appRegistry.initialiseRegistry( globalVars, @"software\LFCConsulting\GBS");
                appRegistry.initialiseWindowDetails();
                appRegistry.initialiseFontsAndColour();
                greekUtilities.initialiseGreekOrthography(globalVars);
                initialisationThread = new Thread(new ThreadStart(InitialLoad));
                initialisationThread.IsBackground = true;
                initialisationThread.Start();
                isActivated = true;
            }
        }

        private void makeMainFormControlsGlobal()
        {
            globalVars.MasterForm = this;
            globalVars.IsReady = false;
            globalVars.IsDisplayChapterCalled = false;
            /*-------------------------------------------------------------*
             *  Declare Richtext boxes to global configuration class       *
             *-------------------------------------------------------------*/
            globalVars.addRichtextItem(rtxtNTMainText);
            globalVars.addRichtextItem(rtxtLxxMainText);
            globalVars.addRichtextItem(rtxtParse);
            globalVars.addRichtextItem(rtxtLexicon);
            globalVars.addRichtextItem(rtxtSearchResults);
            globalVars.addRichtextItem(rtxtVocabList);
            globalVars.addRichtextItem(rtxtNotes);

            /*-------------------------------------------------------------*
             *  Declare Combo boxes to global configuration class          *
             *-------------------------------------------------------------*/
            globalVars.addComboBoxItem(cbNTBook);
            globalVars.addComboBoxItem(cbNTChapter);
            globalVars.addComboBoxItem(cbNTVerse);
            globalVars.addComboBoxItem(cbLXXBook);
            globalVars.addComboBoxItem(cbLXXChapter);
            globalVars.addComboBoxItem(cbLXXVerse);
            globalVars.addComboBoxItem(cbNTHistory);
            globalVars.addComboBoxItem(cbLXXHistory);

            /*-------------------------------------------------------------*
             *  Declare Tab Controls to global configuration class         *
             *-------------------------------------------------------------*/
            globalVars.addTabControl(tabCtlText);
            globalVars.addTabControl(tabCtrlTop);
            globalVars.addTabControl(tabCtrlBottom);
            globalVars.addTabControl(tabCtrlLSAppendices);

            /*-------------------------------------------------------------*
             *  Declare Tab Pages to global configuration class            *
             *-------------------------------------------------------------*/
            globalVars.addTabCtrlPage(tabPgeNT);
            globalVars.addTabCtrlPage(tabPgeLxx);
            globalVars.addTabCtrlPage(tabPgeParse);
            globalVars.addTabCtrlPage(tabPgeLexicon);
            globalVars.addTabCtrlPage(tabPgeSearchResults);
            globalVars.addTabCtrlPage(tabPgeVocabList);
            globalVars.addTabCtrlPage(tabPgeNotes);
            globalVars.addTabCtrlPage(tabPgeLSAppendices);
            globalVars.addTabCtrlPage(tabPgeSearch);
            globalVars.addTabCtrlPage(tabPgeVocabSetup);

            /*-------------------------------------------------------------*
             *  Declare Split Containers to global configuration class     *
             *-------------------------------------------------------------*/
            globalVars.addSplit(splitMain);
            globalVars.addSplit(splitLeft);
            globalVars.addSplit(splitRight);

            /*-------------------------------------------------------------*
             *  Declare Text boxes to global configuration class           *
             *-------------------------------------------------------------*/
            globalVars.addTextbox(txtPrimaryWord);
            globalVars.addTextbox(txtSecondaryWord);

            /*-------------------------------------------------------------*
             *  Declare Radio Buttons to global configuration class        *
             *-------------------------------------------------------------*/
            globalVars.addRadioButton(rbtnRoots);
            globalVars.addRadioButton(rbtnExact);
            globalVars.addRadioButton(rbtnUse);
            globalVars.addRadioButton(rbtnOmit);
            globalVars.addRadioButton(rbtnVocabVerse);
            globalVars.addRadioButton(rbtnByCatAlpha);
            globalVars.addRadioButton(rbtnByTypeRandomnly);
            globalVars.addRadioButton(rbtnMixedAlpha);
            globalVars.addRadioButton(rbtnMixedRandomnly);
            globalVars.addRadioButton(rbtnShowRoots);
            globalVars.addRadioButton(rbtnShowAsUsed);
            globalVars.addRadioButton(rbtnShowBothByRoot);
            globalVars.addRadioButton(rbtnShowBothByWord);

            /*-------------------------------------------------------------*
             *  Declare Web browsers to global configuration class         *
             *-------------------------------------------------------------*/
            globalVars.addWebBrowser(webGeneral);
            globalVars.addWebBrowser(webAuthors);
            globalVars.addWebBrowser(webEpigraphy);
            globalVars.addWebBrowser(webPapyrology);
            globalVars.addWebBrowser(webPeriodicals);

            /*-------------------------------------------------------------*
             *  Declare Web file names to global configuration class       *
             *-------------------------------------------------------------*/
            globalVars.addWebFileName("L5_general_abbreviations.html");
            globalVars.addWebFileName("L1_authors_and_works.html");
            globalVars.addWebFileName("L2_epigraphical_publications.html");
            globalVars.addWebFileName("L3_papyrological_publications.html");
            globalVars.addWebFileName("L4_periodicals.html");

            /*-------------------------------------------------------------*
             *  Declare Labels to global configuration class               *
             *-------------------------------------------------------------*/
            globalVars.addLabel(labWithinLbl);
            globalVars.addLabel(labWordsOfLbl);
            globalVars.addLabel(labNTLoadLbl);
            globalVars.addLabel(labListMsg);

            /*-------------------------------------------------------------*
             *  Declare CheckBoxes to global configuration class           *
             *-------------------------------------------------------------*/

            globalVars.addCheckBox(chkMoses);
            globalVars.addCheckBox(chkHistory);
            globalVars.addCheckBox(chkMajorProphets);
            globalVars.addCheckBox(chkProphets);
            globalVars.addCheckBox(chkKethubimHistory);
            globalVars.addCheckBox(chkKethubimPoetry);
            globalVars.addCheckBox(chkPseudepigrapha);
            globalVars.addCheckBox(chkNarratives);
            globalVars.addCheckBox(chkPaul);
            globalVars.addCheckBox(chkRest);
            globalVars.addCheckBox(chBoxNouns);
            globalVars.addCheckBox(chBoxVerbs);
            globalVars.addCheckBox(chBoxAdjectives);
            globalVars.addCheckBox(chBoxAdverbs);
            globalVars.addCheckBox(chBoxPrepositions);
            globalVars.addCheckBox(chBoxOthers);

            /*-------------------------------------------------------------*
             *  Declare Status Labels to global configuration class        *
             *-------------------------------------------------------------*/
            globalVars.addStatusMessage(statLab2);
            globalVars.addStatusMessage(statLab3);
            globalVars.addStatusMessage(statLab4);
            globalVars.addStatusMessage(statLab5);

            /*-------------------------------------------------------------*
             *  Declare miscellaneous objects                              *
             *-------------------------------------------------------------*/

            globalVars.UdWordDistance = upDownWithin;
            globalVars.BtnSearchType = btnSearchType;
            globalVars.BtnStop = btnStop;

//            globalVars.ParentMenuItem = mnuMain;
            globalVars.StatStrip = statSearch;
            globalVars.PnlKeyboard = pnlKeyboard;
            globalVars.PnlFiller = pnlFiller;
            globalVars.LbAvailableBooks = lbAvailableBooks;
            globalVars.DgvVocabList = dgvVocabList;
            //            Color[] foreColorRec, backColorRec;
            //            Font[] fontForArea;

            globalVars.HistoryMax = 99;
        }

        private void InitialLoad()
        {
            // We need to define some classes early so that a non-null value is stored in the text classes - the sequence is vital
            lxxText = new classLXXText();
            ntText = new classNTText();
            historyProcessing = new classHistory(globalVars, ntText, lxxText, progressForm);
            greekLexicon = new classLexicon();
            greekLexicon.initialiseLexicon(globalVars, greekUtilities, progressForm);

            /*===================================================================*
             *                                                                   *
             *  Load the LXX text                                                *
             *    (Note: the order of execution is significant because of the    *
             *           population of book counts.)                             *
             *                                                                   *
             *===================================================================*/

            lxxText.initialiseText(globalVars, progressForm, historyProcessing, greekLexicon);
            lxxText.loadText();
        }

        private void ntLoad()
        {
            /*===================================================================*
             *                                                                   *
             *  Load the NT text                                                 *
             *                                                                   *
             *===================================================================*/

            ntText.initialiseText(globalVars, progressForm, historyProcessing, greekLexicon);
            ntText.loadText();
        }

        private void appendixLoad()
        {
            /*************************************************************************************
             *                                                                                   *
             * Load the Liddell & Scott Appendices                                               *
             *                                                                                   *
             *************************************************************************************/

            greekLexicon.populateAppendices();
        }

        private void keyboardLoad()
        {
            /*===================================================================*
             *                                                                   *
             *  Set up the virtual keyboard                                      *
             *                                                                   *
             *===================================================================*/
            keyboard = new classKeyboard();
            keyboard.initialiseKeyboard(globalVars, progressForm, greekUtilities);
            splitLeft.Invoke(new performKeyboardInit(setKeyboardPosition), splitLeft);
        }

        private void notesSetup()
        {
            /*===================================================================*
             *                                                                   *
             *  Initialise the notes facilities                                  *
             *                                                                   *
             *===================================================================*/
            note.initialiseNotes(globalVars, ntText, lxxText, appRegistry, progressForm);
            note.processOnStartup();
        }

        private void initialiseSearch()
        {
            /*===================================================================*
             *                                                                   *
             *  Initialise the search form                                       *
             *                                                                   *
             *===================================================================*/

            searchMain = new classSearch(this, globalVars, greekLexicon, greekUtilities, ntText, lxxText, note, progressForm);
        }

        private void loadHistory()
        {
            /*===================================================================*
             *                                                                   *
             *  Initialise the two history lists                                 *
             *                                                                   *
             *===================================================================*/
            historyProcessing.loadHistory();
        }

        private void labNTLoadLbl_TextChanged(object sender, EventArgs e)
        {
            Tuple<int, String> bookDetails;

            if( String.Compare( labNTLoadLbl.Text, "LXX Load Complete" ) == 0 )
            {
                labNTLoadLbl.Text = "NT Load beginning";
                initialisationThread = new Thread(new ThreadStart(ntLoad));
                initialisationThread.IsBackground = true;
                initialisationThread.Start();
            }
            if (String.Compare(labNTLoadLbl.Text, "NT Load Complete") == 0)
            {
                initialisationThread = new Thread(new ThreadStart(appendixLoad));
                initialisationThread.IsBackground = true;
                initialisationThread.Start();
            }
            if (String.Compare(labNTLoadLbl.Text, "Appendix Load Complete") == 0)
            {
                initialisationThread = new Thread(new ThreadStart(keyboardLoad));
                initialisationThread.IsBackground = true;
                initialisationThread.Start();
            }
            if (String.Compare(labNTLoadLbl.Text, "Virtual Keyboard Created") == 0)
            {
                initialisationThread = new Thread(new ThreadStart(notesSetup));
                initialisationThread.IsBackground = true;
                initialisationThread.Start();
            }
            if (String.Compare(labNTLoadLbl.Text, "Notes Processing Complete") == 0)
            {
                initialisationThread = new Thread(new ThreadStart(initialiseSearch));
                initialisationThread.IsBackground = true;
                initialisationThread.Start();
            }
            if (String.Compare(labNTLoadLbl.Text, "Search Initialisation Complete") == 0)
            {
                initialisationThread = new Thread(new ThreadStart(loadHistory));
                initialisationThread.IsBackground = true;
                initialisationThread.Start();
            }
            
            if (String.Compare(labNTLoadLbl.Text, "History Load Complete") == 0)
            {
                labNTLoadLbl.Visible = false;
                progressForm.Close();
                bookDetails = historyProcessing.getBookAndChapter(2);
                if (bookDetails.Item1 > -1) lxxText.displayChapter(bookDetails.Item1, bookDetails.Item2, false);
                bookDetails = historyProcessing.getBookAndChapter(1);
                if( bookDetails.Item1 > -1) ntText.displayChapter( bookDetails.Item1, bookDetails.Item2, false );
                this.Visible = true;
                globalVars.IsReady = true;
            }
        }

        private void tmrKeyboard_Tick(object sender, EventArgs e)
        {
            int maxHeight, heightCalculation, tagVal;
            Button clickedButton, otherButton;

            switch(showButtonTag)
            {
                case 1: clickedButton = btnNTShowKeyboard; otherButton = btnLXXKeyboard; break;
                case 2: clickedButton = btnLXXKeyboard; otherButton = btnNTShowKeyboard; break;
                default: return;
            }
            if (clickedButton.Text == "Show Keyboard")
            {
                heightCalculation = keybrdPanelHeight;
                maxHeight = splitLeft.Height;
                splitLeft.SplitterDistance -= 8;
                if (maxHeight - splitLeft.SplitterDistance >= heightCalculation + 5)
                {
                    clickedButton.Text = "Hide Keyboard";
                    otherButton.Text = "Hide Keyboard";
                    tmrKeyboard.Enabled = false;
                }
            }
            else
            {
                splitLeft.SplitterDistance += 8;
                if (splitLeft.SplitterDistance >= splitLeft.Height - globalVars.PnlFiller.Height - 6)
                {
                    splitLeft.SplitterDistance = splitLeft.Height - globalVars.PnlFiller.Height;
                    clickedButton.Text = "Show Keyboard";
                    otherButton.Text = "Show Keyboard";
                    tmrKeyboard.Enabled = false;
                }
            }
        }

        private void advanceOrGoBackOneChapter(object sender, EventArgs e)
        {
            int tagVal;
            Button currentButton;

            currentButton = (Button)sender;
            tagVal = Convert.ToInt32(currentButton.Tag);
            switch (tagVal)
            {
                case 1: ntText.backOrForwardOneChapter(1); break;
                case 2: ntText.backOrForwardOneChapter(2); break;
                case 3: lxxText.backOrForwardOneChapter(1); break;
                case 4: lxxText.backOrForwardOneChapter(2); break;
            }
        }

        private void cbBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbBook;

            if (! globalVars.IsReady) return;
            if (globalVars.IsDisplayChapterCalled) return;
            cbBook = (ComboBox)sender;
            if (cbBook.Items.Count == 0) return;
            if (cbBook == globalVars.getComboBoxItem(0)) ntText.respondToNewBook();
            else lxxText.respondToNewBook(); 
        }

        private void cbChapter_SelectedValueChanged(object sender, EventArgs e)
        {
            int bookId;
            String chapterId;
            ComboBox cbChapter;

            if (!globalVars.IsReady) return;
            if (globalVars.IsDisplayChapterCalled) return;
            cbChapter = (ComboBox)sender;
            if (cbChapter.Items.Count == 0) return;
            if (cbChapter == globalVars.getComboBoxItem(1))
            {
                bookId = globalVars.getComboBoxItem(0).SelectedIndex;
                chapterId = globalVars.getComboBoxItem(1).SelectedItem.ToString();
                ntText.displayChapter(bookId + globalVars.NoOfLXXBooks, chapterId, true);
            }
            if (cbChapter == globalVars.getComboBoxItem(4))
            {
                bookId = globalVars.getComboBoxItem(3).SelectedIndex;
                chapterId = globalVars.getComboBoxItem(4).SelectedItem.ToString();
                lxxText.displayChapter(bookId, chapterId, true);
            }
        }
        private void changeReferenceByHistory(object sender, EventArgs e)
        {
            int tagVal, idx, noOfEntries;
            String fileName = "";
            ComboBox currentCB;
            StreamWriter swHistory = null;

//            if (!globalVars.IsReady) return;
            currentCB = (ComboBox)sender;
            tagVal = Convert.ToInt32(currentCB.Tag);
            {
                switch (tagVal)
                {
                    case 1: ntText.processSelectedHistory(); break;
                    case 2: lxxText.processSelectedHistory(); break;
                }
            }
            fileName = globalVars.NotesPath;
            switch (tagVal)
            {
                case 1: swHistory = new StreamWriter(fileName + @"\NTHistory.txt"); break;
                case 2: swHistory = new StreamWriter(fileName + @"\LXXHistory.txt"); break;
            } 
            noOfEntries = currentCB.Items.Count;
            for (idx = 0; idx < noOfEntries; idx++)
            {
                if (idx < noOfEntries - 1) swHistory.WriteLine(currentCB.Items[idx].ToString());
                else swHistory.Write(currentCB.Items[idx].ToString());
            }
            swHistory.Close();
        }

        private void registerMouseDown(object sender, MouseEventArgs e)
        {
            /*========================================================================================================*
             *                                                                                                        *
             *                                           registerMouseDown                                            *
             *                                           =================                                            *
             *                                                                                                        *
             *  Purpose:                                                                                              *
             *  =======                                                                                               *
             *                                                                                                        *
             *  To identify the following whenever *any* mouse click is made:                                         *
             *    a) The text for the verse in which the selection is made                                            *
             *    b) The word clicked                                                                                 *
             *    c) The sequence number of the word in the verse                                                     *
             *  Point c) will allow us to identify the classWord instance for the selected word.                      *
             *                                                                                                        *
             *  Note: if the content of the rich text area has been changed by selecting from one of the combo boxes, *
             *    a) and b) will be set to null values.                                                               *
             *                                                                                                        *
             *========================================================================================================*/

            RichTextBox currentRtxt;

            currentRtxt = (RichTextBox) sender;
            if( currentRtxt == globalVars.getRichtextItem(0)) ntText.recordMouseEffects(e);
            if (currentRtxt == globalVars.getRichtextItem(1)) lxxText.recordMouseEffects(e);
            if (currentRtxt == globalVars.getRichtextItem(4)) searchMain.recordMouseEffects(e);
        }

        private void cMnuCopy_Click(object sender, EventArgs e)
        {
            int tagVal, versionFlag;
            ToolStripMenuItem currentMenuItem;
            frmCopyOptions copyOptions;

            currentMenuItem = (ToolStripMenuItem)sender;
            tagVal = Convert.ToInt32(currentMenuItem.Tag);
            versionFlag = globalVars.getTabControl(0).SelectedIndex;
            copyOptions = new frmCopyOptions(tagVal, versionFlag, globalVars, greekUtilities, note);
            if (copyOptions.processMenuClick() == DialogResult.OK)
            {
                if (copyOptions.RememberCount > 0)
                {
                    mnuTextReset.Enabled = true;
                    mnuEditReset.Enabled = true;
                }
                else
                {
                    mnuTextReset.Enabled = false;
                    mnuEditReset.Enabled = false;
                }
            }
        }

        private void mnuTextReset_Click(object sender, EventArgs e)
        {
            frmCopyReset copyReset;

            copyReset = new frmCopyReset(globalVars);
            if (copyReset.enableReset() > 0)
            {
                mnuTextReset.Enabled = true;
                mnuEditReset.Enabled = true;
            }
            else
            {
                mnuTextReset.Enabled = false;
                mnuEditReset.Enabled = false;
            }
        }

        private void searchSetup_Click(object sender, EventArgs e)
        {
            int tagVal;
            ToolStripMenuItem cMnuCurrent = (ToolStripMenuItem)sender;

            tagVal = Convert.ToInt32(cMnuCurrent.Tag);
            searchMain.searchSetup(tagVal, globalVars.getTabControl(0).SelectedIndex);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchMain.initiateSearch();
        }

        private void cMnuTextAnalyse_Click(object sender, EventArgs e)
        {
            if (tabCtlText.SelectedTab == tabPgeNT) ntText.Analysis();
            else lxxText.Analysis();
        }

        private void CheckboxChanged(object sender, EventArgs e)
        {
            searchMain.populateAvailableBoolsListbox();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int idx = 0;
            List<int> indexList = new List<int>();
            List<int> reversedList = new List<int>();
            ListBox.SelectedIndexCollection indexCollection;

            indexCollection = lbAvailableBooks.SelectedIndices;
            foreach ( int i in indexCollection)
            {
                indexList.Add(i);
            }
            reversedList = Enumerable.Reverse(indexList).ToList();
            foreach ( int i in reversedList)
            {
                lbAvailableBooks.Items.RemoveAt(i);
            }
        }

        private void btnReinstate_Click(object sender, EventArgs e)
        {
            rbtnUse.Checked = true;
            chkMoses.Checked = true;
            chkHistory.Checked = true;
            chkMajorProphets.Checked = true;
            chkProphets.Checked = true;
            chkKethubimHistory.Checked = true;
            chkKethubimPoetry.Checked = true;
            chkPseudepigrapha.Checked = true;
            chkNarratives.Checked = true;
            chkPaul.Checked = true;
            chkRest.Checked = true;
            searchMain.populateAvailableBoolsListbox();
        }

        private void btnSearchType_Click(object sender, EventArgs e)
        {
            searchMain.setSearchType();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            searchMain.stopSearch();
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            showButtonTag = Convert.ToInt32(((Button)sender).Tag);
            tmrKeyboard.Enabled = true;
        }

        private void cMnuResultsUpdateText_Click(object sender, EventArgs e)
        {
            searchMain.updateTextAreaWithSelectedChapter();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            vocabSetup.setupVocabList();
        }

        private void copySearchResults_Click(object sender, EventArgs e)
        {
            int tagVal;
            ToolStripMenuItem currentMenuItem;

            currentMenuItem = (ToolStripMenuItem)sender;
            tagVal = Convert.ToInt32(currentMenuItem.Tag);
            switch (tagVal)
            {
                case 1: searchMain.copyAllResults(true); break;
                case 2: searchMain.copyAllResults(false); break;
                case 3: searchMain.copySingleResult(true); break;
                case 4: searchMain.copySingleResult(false); break;
            }
        }

        private void resizeRightPanes(object sender, EventArgs e)
        {
            int tagVal;
            ToolStripMenuItem menuItem;

            menuItem = (ToolStripMenuItem)sender;
            tagVal=Convert.ToInt32(menuItem.Tag);
            globalVars.CurrentResizeAction = tagVal;
            tmrRightPanes.Enabled = true;
        }

        private void tmrRightPanes_Tick(object sender, EventArgs e)
        {
            int targetSize;

            switch (globalVars.CurrentResizeAction)
            {
                case 1:
                    {
                        targetSize = splitMain.Panel2.Height - 32;
                        splitRight.SplitterDistance += 4;
                        if (splitRight.SplitterDistance >= targetSize)
                        {
                            tmrRightPanes.Enabled = false;
                            rightPanesControl = -1;
                        }
                    }
                    break;
                case 2:
                    {
                        targetSize = 4;
                        splitRight.SplitterDistance -= 4;
                        if (splitRight.SplitterDistance <= targetSize)
                        {
                            tmrRightPanes.Enabled = false;
                            rightPanesControl = -1;
                        }
                    }
                    break;
                case 3:
                    {
                        targetSize = splitMain.Panel2.Height / 2;
                        if (splitRight.SplitterDistance < targetSize)
                        {
                            splitRight.SplitterDistance += 4;
                        }
                        else
                        {
                            splitRight.SplitterDistance -= 4;
                        }
                        if (Math.Abs(splitRight.SplitterDistance - targetSize) < 3)
                        {
                            tmrRightPanes.Enabled = false;
                            rightPanesControl = -1;
                        }
                    }
                    break;
                default: break;
            }
        }

        private void splitMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            appRegistry.updateSplitterDistance();
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            appRegistry.updateWindowSize();
        }

        private void mnuFilePrefs_Click(object sender, EventArgs e)
        {
            int noOfAreas;
            bool[] hasChanged;
            frmPreferences preferences;

            preferences = new frmPreferences(globalVars, appRegistry);
            preferences.initialiseForm();
            if( preferences.ShowDialog() == DialogResult.OK )
            {
                noOfAreas = preferences.NoOfAreas;
                hasChanged = preferences.HasChanged;
                if (hasChanged[0])
                {
                    ntText.displayChapter(cbNTBook.SelectedIndex + globalVars.NoOfLXXBooks, cbNTChapter.SelectedItem.ToString(), true);
                    appRegistry.updateSingleAreaFontsAndColour(0);
                }
                if (hasChanged[1])
                {
                    lxxText.displayChapter(cbLXXBook.SelectedIndex, cbLXXChapter.SelectedItem.ToString(), true);
                    appRegistry.updateSingleAreaFontsAndColour(1);
                }
                if (hasChanged[2]) appRegistry.updateSingleAreaFontsAndColour(2);
                if (hasChanged[3]) appRegistry.updateSingleAreaFontsAndColour(3);
                if (hasChanged[4]) appRegistry.updateSingleAreaFontsAndColour(4);
                if (hasChanged[5]) appRegistry.updateSingleAreaFontsAndColour(5);
                if (hasChanged[6]) appRegistry.updateSingleAreaFontsAndColour(6);
            }
            preferences.Close();
            preferences.Dispose();
        }

        private void notesMainMenu(object sender, EventArgs e)
        {
            int tagVal;
            ToolStripMenuItem currentMenuItem;
            frmNotes noteManagement;

            currentMenuItem = (ToolStripMenuItem)sender;
            tagVal = Convert.ToInt32(currentMenuItem.Tag);
            noteManagement = new frmNotes();
            noteManagement.initialiseNotesDialog(globalVars, note, tagVal);
            noteManagement.ShowDialog();
            noteManagement.Dispose();
        }

        private void relocateSourceFiles(object sender, EventArgs e)
        {
            frmMove relocationForm;

            relocationForm = new frmMove();
            relocationForm.registerDestination(globalVars.BaseDirectory);
            if (relocationForm.ShowDialog() == DialogResult.OK)
            {
                relocationForm.Dispose();
                if (appRegistry.relocateFiles(relocationForm.SelectedDestination))
                    MessageBox.Show("Application files successfully relocated", "File Relocation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void copyParseAndLexText(object sender, EventArgs e)
        {
            int tagValue;
            ToolStripMenuItem menuItem;

            menuItem = (ToolStripMenuItem)sender;
            tagValue = Convert.ToInt32(menuItem.Tag);
            switch( tagValue)
            {
                case 1: greekLexicon.copyParseAndLexiconData(0, false); break;
                case 2: greekLexicon.copyParseAndLexiconData(0, true); break;
                case 3: greekLexicon.copyParseAndLexiconData(1, false); break;
                case 4: greekLexicon.copyParseAndLexiconData(1, true); break;
                case 5: greekLexicon.copyParseAndLexiconData(2, false); break;
                case 6: greekLexicon.copyParseAndLexiconData(2, true); break;
            }
        }

        private void copyVocabList(object sender, EventArgs e)
        {
            int tagVal;
            ToolStripMenuItem menuItem;

            menuItem = (ToolStripMenuItem)sender;
            tagVal = Convert.ToInt32(menuItem.Tag);
            switch ( tagVal)
            {
                case 1: vocabSetup.copyVocabList( true, false); break;
                case 2: vocabSetup.copyVocabList(true, true); break;
                case 3: vocabSetup.copyVocabList(false, false); break;
                case 4: vocabSetup.copyVocabList(false, true); break;
            }
        }

        private void displayHelp(object sender, EventArgs e)
        {
            frmHelp helpForm;

            helpForm = new frmHelp();
            helpForm.initialiseHelp(globalVars.HelpFile);
            helpForm.Show();
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            AboutGBS aboutGBS = new AboutGBS();

            aboutGBS.ShowDialog();
        }

        private void frmMain_Move(object sender, EventArgs e)
        {
            if (appRegistry != null) appRegistry.updateWindowPosition();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
