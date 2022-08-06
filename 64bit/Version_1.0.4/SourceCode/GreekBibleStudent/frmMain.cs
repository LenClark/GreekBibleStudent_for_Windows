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
    /******************************************************************************************************************************
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
     *  Current revision: October 2021                                                                                            *
     *                                                                                                                            *
     ******************************************************************************************************************************/

    public partial class frmMain : Form
    {
        const int keybrdPanelHeight = 165;
        int rightPanesControl = 2, searchCursorPstn = 0;

        /*-----------------------------------------------------------------------*
         *                                                                       *
         *                                 Classes                               *
         *                                 =======                               *
         *                                                                       *
         *-----------------------------------------------------------------------*/

        int textCode = -1;
        classGlobal globalVars;
        GreekProcessing greekUtilities;
        classRegistry appRegistry;
        classText mainTextHandler;
        classLexicon lexicon;
        classKeyboard virtualKeyboard;
        classSearch searchProcedures;
        classNotes notesProcessing;

        bool isCbUpdate = false;

        public frmMain()
        {
            Label[] passLabels = new Label[8];

            InitializeComponent();

            /*-------------------------------------------------------------*
             *  Initialise global classes before anything else             *
             *-------------------------------------------------------------*/
            globalVars = new classGlobal();
            greekUtilities = new GreekProcessing();
            greekUtilities.initialiseGreekProcessing(globalVars);
            appRegistry = new classRegistry();
            appRegistry.initialiseRegistry(globalVars);
            lexicon = new classLexicon();
            mainTextHandler = new classText();
            mainTextHandler.initialiseText(globalVars, greekUtilities, lexicon);
            searchProcedures = new classSearch();
            searchProcedures.initialiseSearch(globalVars, mainTextHandler, greekUtilities);

            /*-------------------------------------------------------------*
             *  Allocation of values to globalVars (global Variables)      *
             *-------------------------------------------------------------*/
            globalVars.MasterForm = this;
            notesProcessing = new classNotes();
            notesProcessing.initialiseNotes(globalVars, mainTextHandler, appRegistry);

            /*-------------------------------------------------------------*
            *  Declare Richtext boxes to global configuration class       *
            *                                                             *
            *    0  rtxtMainText                                          *
            *    1  rtxtLxxMainText                                       *
            *    2  rtxtParse                                             *
            *    3  rtxtLexicon                                           *
            *    4  rtxtSearchResults                                     *
            *    5  rtxtNotes                                             *
            *    6  rtxtVocabList                                         *
            *-------------------------------------------------------------*/
            globalVars.addRichtextControl(rtxtMainText);
            globalVars.addRichtextControl(rtxtLxxMainText);
            globalVars.addRichtextControl(rtxtParse);
            globalVars.addRichtextControl(rtxtLexicon);
            globalVars.addRichtextControl(rtxtSearchResults);
            globalVars.addRichtextControl(rtxtVocabList);
            globalVars.addRichtextControl(rtxtNotes);
            /*-------------------------------------------------------------*
             *  Declare Text boxes to global configuration class           *
             *-------------------------------------------------------------*/
            globalVars.addSearchTextControl(txtPrimaryWord);
            globalVars.addSearchTextControl(txtSecondaryWord);
            /*-------------------------------------------------------------*
             *  Declare Combo boxes to global configuration class          *
             *                                                             *
             *    0  cbBook                                                *
             *    1  cbChapter                                             *
             *    2  cbVerse                                               *
             *    3  cbLxxBook                                             *
             *    4  cbLxxChapter                                          *
             *    5  cbLxxVerse                                            *
             *    6  cbHistory                                             *
             *    7  cbLxxHistory                                          *
             *                                                             *
             *-------------------------------------------------------------*/
            globalVars.addComboBoxControl(cbBook);
            globalVars.addComboBoxControl(cbChapter);
            globalVars.addComboBoxControl(cbVerse);
            globalVars.addComboBoxControl(cbLxxBook);
            globalVars.addComboBoxControl(cbLxxChapter);
            globalVars.addComboBoxControl(cbLxxVerse);
            globalVars.addComboBoxControl(cbHistory);
            globalVars.addComboBoxControl(cbLxxHistory);
            /*-------------------------------------------------------------*
             *  Declare Web browsers to global configuration class         *
             *-------------------------------------------------------------*/
            globalVars.addWebControl(webComments);
            globalVars.addWebControl(webAuthors);
            globalVars.addWebControl(webEpigraphy);
            globalVars.addWebControl(webPapyrology);
            globalVars.addWebControl(webPeriodicals);
            globalVars.addWebControl(webGeneral);
            /*-------------------------------------------------------------*
             *  Declare Web file names to global configuration class       *
             *-------------------------------------------------------------*/
            globalVars.addWebFileNames("Comments.html");
            globalVars.addWebFileNames("L1_authors_and_works.html");
            globalVars.addWebFileNames("L2_epigraphical_publications.html");
            globalVars.addWebFileNames("L3_papyrological_publications.html");
            globalVars.addWebFileNames("L4_periodicals.html");
            globalVars.addWebFileNames("L5_general_abbreviations.html");
            /*-------------------------------------------------------------*
             *  Declare Search related objects names to global             *
             *    configuration class                                      *
             *-------------------------------------------------------------*/
            globalVars.addSearchItemsForTextEntry(lblWithin);
            globalVars.addSearchItemsForTextEntry(lblWordsOf);
            globalVars.addSearchItemsForTextEntry(upDownWithin);
            globalVars.addSearchItemsForTextEntry(btnSearchType);
            globalVars.NtTabPge = tabPgeNT;
            globalVars.LxxTabPge = tabPgeLxx;
            globalVars.TabCtlText = tabCtlText;
            globalVars.NotesTabPage = tabPgeNotes;

            globalVars.SplitMain = splitMain;
            globalVars.TabCtrlBottomLeft = tabCtrlBottom;
            globalVars.SearchMessages.Add(1, statLab2);
            globalVars.SearchMessages.Add(2, statLab3);
            globalVars.SearchMessages.Add(3, statLab4);
            globalVars.SearchMessages.Add(4, statLab5);
            globalVars.StatStrip = statSearch;

            globalVars.HistoryMax = 99;

            /*-------------------------------------------------------------------*
             *                                                                   *
             *  Registry Management and cross-session values                     *
             *                                                                   *
             *-------------------------------------------------------------------*/

            appRegistry.MainForm = this;
            appRegistry.KeyString = @"software\LFCConsulting\GreekBibleStudent";
            appRegistry.initialiseRegistry();
            appRegistry.initialiseWindowDetails();
            appRegistry.initialiseFontsAndColour();
            globalVars.initialiseDefaultStore();
            appRegistry.closeKeys();
            greekUtilities.constructGreekLists();
            mainTextHandler.storeAllText();
            mainTextHandler.loadHistory();

            /*************************************************************************************
             *                                                                                   *
             *  Set up the list box contents for the search option.                              *
             *                                                                                   *
             *************************************************************************************/

            searchOptionCheckedChanged(null, null);

            /*************************************************************************************
             *                                                                                   *
             *  Check for the existence of unsaved notes and, if they exist, process them.       *
             *                                                                                   *
             *************************************************************************************/

            notesProcessing.processOnStartup();

            /*************************************************************************************
             *                                                                                   *
             * Load the Liddell & Scott Appendices                                               *
             *                                                                                   *
             *************************************************************************************/

            lexicon.initialiseLexicon(globalVars, greekUtilities);
            lexicon.populateAppendices();

            /*************************************************************************************
             *                                                                                   *
             * Step 7: Set up the pseudo-keyboard                                                *
             *                                                                                   *
             *************************************************************************************/

            splitLeft.SplitterDistance = splitLeft.Height - pnlHistory.Height;
            passLabels[0] = labNtLxx;
            passLabels[1] = labBookCode;
            passLabels[2] = labChap;
            passLabels[3] = labVerse;
            passLabels[4] = labNtLxx2ary;
            passLabels[5] = labBookCode2ary;
            passLabels[6] = labChap2ary;
            passLabels[7] = labVerse2ary;
            virtualKeyboard = new classKeyboard();
            virtualKeyboard.initialiseKeyboard(globalVars, pnlKeyboard, passLabels);
            virtualKeyboard.setupKeyboard();

            tabPgeNotes.Text = "Notes: " + globalVars.NotesName + " - " + cbBook.SelectedItem.ToString() + " " + cbChapter.SelectedItem.ToString() + ":" +
                cbVerse.SelectedItem.ToString();
        }

        private void cbBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCbUpdate)
            {
                isCbUpdate = true;
                mainTextHandler.handleChangeOfNTBook();
                isCbUpdate = false;
            }
        }

        private void cbChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCbUpdate)
            {
                isCbUpdate = true;
                mainTextHandler.handleChangeOfNTChapter();
                isCbUpdate = false;
            }
        }

        private void cbLxxBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCbUpdate)
            {
                isCbUpdate = true;
                mainTextHandler.handleChangeOfLxxBook();
                isCbUpdate = false;
            }
        }

        private void cbLxxChapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCbUpdate)
            {
                isCbUpdate = true;
                mainTextHandler.handleChangeOfLXXChapter();
                isCbUpdate = false;
            }
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            splitLeft.SplitterDistance = splitLeft.Height - pnlHistory.Height;
            appRegistry.updateWindowSize();
        }

        private void frmMain_Move(object sender, EventArgs e)
        {
            appRegistry.updateWindowPosition();
        }

        private void splitMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            appRegistry.updateSplitterDistance();
        }

        private void searchOptionCheckedChanged(object sender, EventArgs e)
        {
            /*=====================================================================================*
             *                                                                                     *
             *                           searchOptionCheckedChanged                                *
             *                           ==========================                                *
             *                                                                                     *
             *  This is all about managing selection of the radio buttons and check boxes that     *
             *    limit/activate groups of books.  A key control are the following constants and   *
             *    their explanations are:                                                          *
             *                                                                                     *
             *  The first three relate to groups of works in the New Testament, as follows:        *
             *    maxGospelsIdx  The *maximum* value for this group (i.e. the range from 1 to 5)   *
             *    maxPaulIdx     The *maximum* value for Paul's letters (so this, in effect, goes  *
             *                   from 6 (defined by maxGospelIdx) to 18)                           *
             *    maxRestIdx     Up to noOfNTBooks                                                 *
             *                                                                                     *
             *  Similarly, the following four define groups for the Septuagint:                    *
             *    maxMosesIdx    The maximum book value for the Pentateuch                         *
             *    maxHistoryIdx  Books designated as "history"                                     *
             *    maxWisdomIdx   Books in the Wisdom category                                      *
             *    maxProphetsIds The rest                                                          *
             *                                                                                     *
             *=====================================================================================*/
            const int maxGospelIdx = 5, maxPaulIdx = 18, maxMosesIdx = 5, maxHistoryIdx = 26, maxWisdomIdx = 35;

            int maxRestIdx = mainTextHandler.NoOfNTBooks, maxProphetsIdx = mainTextHandler.NoOfLxxBooks;
            bool isMoses, isHistory, isWisdom, isProphets, isGospels, isPaul, isRest;

            isMoses = chkMoses.Checked;
            isHistory = chkHistory.Checked;
            isWisdom = chkWisdom.Checked;
            isProphets = chkProphets.Checked;
            isGospels = chkGospels.Checked;
            isPaul = chkPaul.Checked;
            isRest = chkRest.Checked;
            lbAvailableBooks.Items.Clear();
            if (rbtnExclude.Checked)
            {
                if (!isGospels) addToListbox(0, maxGospelIdx, 0);
                if (!isPaul) addToListbox(maxGospelIdx, maxPaulIdx, 0);
                if (!isRest) addToListbox(maxPaulIdx, maxRestIdx, 0);
                if (!isMoses) addToListbox(0, maxMosesIdx, 1);
                if (!isHistory) addToListbox(maxMosesIdx, maxHistoryIdx, 1);
                if (!isWisdom) addToListbox(maxHistoryIdx, maxWisdomIdx, 1);
                if (!isProphets) addToListbox(maxWisdomIdx, maxProphetsIdx, 1);
            }
            else
            {
                if (isGospels) addToListbox(0, maxGospelIdx, 0);
                if (isPaul) addToListbox(maxGospelIdx, maxPaulIdx, 0);
                if (isRest) addToListbox(maxPaulIdx, maxRestIdx, 0);
                if (isMoses) addToListbox(0, maxMosesIdx, 1);
                if (isHistory) addToListbox(maxMosesIdx, maxHistoryIdx, 1);
                if (isWisdom) addToListbox(maxHistoryIdx, maxWisdomIdx, 1);
                if (isProphets) addToListbox(maxWisdomIdx, maxProphetsIdx, 1);
            }
            tabPgeSearch.Refresh();
        }

        private void addToListbox(int lowerIndex, int intUpperIndex, int code)
        {
            int idx;
            String bookName;
            SortedDictionary<int, classBookContent> listOfBooks;
            classBookContent currentBook;

            if (code == 0) listOfBooks = mainTextHandler.ListOfNTBooks;
            else listOfBooks = mainTextHandler.ListOfLxxBooks;
            for (idx = lowerIndex; idx < intUpperIndex; idx++)
            {
                listOfBooks.TryGetValue(idx, out currentBook);
                bookName = currentBook.BookName;
                lbAvailableBooks.Items.Add(bookName);
            }
        }

        private void tabCtlText_Selected(object sender, TabControlEventArgs e)
        {
            int bookId, tabVal;
            String formHeader = "Greek Bible Student - ";
            classBookContent newBook;

            tabVal = e.TabPageIndex;
            if (tabVal == 0)
            {
                bookId = cbBook.SelectedIndex;
                mainTextHandler.ListOfNTBooks.TryGetValue(bookId, out newBook);
                formHeader += cbBook.Text + " " + cbChapter.Text;
            }
            else
            {
                bookId = cbLxxBook.SelectedIndex;
                mainTextHandler.ListOfLxxBooks.TryGetValue(bookId, out newBook);
                formHeader += cbLxxBook.Text + " " + cbLxxChapter.Text;
            }
            this.Text = formHeader;
            notesProcessing.processNewNote(tabVal);
            /*            newChapter = newBook.getChapterBySeqNo(globalVars.getComboBoxControlByIndex(tabVal * 3 + 1).SelectedIndex);
                        newVerse = newChapter.getVerseBySeqNo(globalVars.getComboBoxControlByIndex(tabVal * 3 + 2).SelectedIndex);
                        rtxtNotes.Text = newVerse.NoteText; */
        }

        private void tmrKeyboard_Tick(object sender, EventArgs e)
        {
            int maxHeight;
            int heightCalculation;

            if (btnKeyboard.Text == "Show Keyboard")
            {
                heightCalculation = keybrdPanelHeight;
                maxHeight = splitLeft.Height;
                splitLeft.SplitterDistance -= 4;
                if (maxHeight - splitLeft.SplitterDistance >= heightCalculation + 5)
                {
                    btnKeyboard.Text = "Hide Keyboard";
                    tmrKeyboard.Enabled = false;
                    rtxtMainText.Refresh();
                }
            }
            else
            {
                splitLeft.SplitterDistance += 4;
                if (splitLeft.SplitterDistance >= splitLeft.Height - pnlHistory.Height)
                {
                    splitLeft.SplitterDistance = splitLeft.Height - pnlHistory.Height;
                    btnKeyboard.Text = "Show Keyboard";
                    tmrKeyboard.Enabled = false;
                }
            }
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            tmrKeyboard.Enabled = true;
        }

        private void historyBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            int tagVal;
            ComboBox currentCBox = (ComboBox)sender;

            if (!isCbUpdate)
            {
                isCbUpdate = true;
                tagVal = Convert.ToInt32(currentCBox.Tag);
                mainTextHandler.processSelectedHistory(tagVal);
                isCbUpdate = false;
            }
        }

        private void historyPrevNextClick(object sender, EventArgs e)
        {
            int tagVal;
            Button currentButton = (Button)sender;

            tagVal = Convert.ToInt32(currentButton.Tag);
            switch (tagVal)
            {
                case 1: mainTextHandler.advanceHistory(2, 0); break;
                case 2: mainTextHandler.advanceHistory(1, 0); break;
                case 3: mainTextHandler.advanceHistory(2, 1); break;
                case 4: mainTextHandler.advanceHistory(1, 1); break;
            }
        }

        private void textAreaMouseDown(object sender, MouseEventArgs e)
        {
            int nPstn, nStart, nEnd;
            String currentVerseText, currentRef;
            Char[] possibleDelimiters = { '\n' };
            RichTextBox currentTextArea;

            textCode = tabCtlText.SelectedIndex;
            if (textCode == 0) currentTextArea = rtxtMainText;
            else currentTextArea = rtxtLxxMainText;
            nPstn = currentTextArea.GetCharIndexFromPosition(new Point(e.X, e.Y));
            globalVars.LatestMousePosition = nPstn;
            if (currentTextArea.Text[nPstn] == '\n') nStart = currentTextArea.Text.LastIndexOf('\n', nPstn - 1);
            else nStart = currentTextArea.Text.LastIndexOf('\n', nPstn);
            nStart++;
            if (currentTextArea.Text[nPstn] == '\n') nEnd = nPstn;
            else nEnd = currentTextArea.Text.IndexOf('\n', nPstn);
            if (nEnd == -1) nEnd = currentTextArea.Text.Length;
            currentVerseText = currentTextArea.Text.Substring(nStart, nEnd - nStart);
            nEnd = currentVerseText.IndexOf(':');
            currentRef = currentVerseText.Substring(0, nEnd);
            if (textCode == 0) cbVerse.SelectedItem = currentRef;
            else cbLxxVerse.SelectedItem = currentRef;
        }

        private void rtxtMainText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void textAreaClick(object sender, EventArgs e)
        {
            String returnedVerse;
            RichTextBox currentTextArea = (RichTextBox)sender;

            returnedVerse = mainTextHandler.getVerseNumber(currentTextArea);
            if (tabCtlText.SelectedIndex == 0) cbVerse.SelectedItem = returnedVerse;
            else cbLxxVerse.SelectedItem = returnedVerse;
        }

        private void cMnuTextAnalyse_Click(object sender, EventArgs e)
        {
            mainTextHandler.performAnalysis(textCode);
            tabCtrlTop.SelectedIndex = 0;
            textCode = -1;
        }

        /********************************************************************************
         *                                                                              *
         *                         Context Menus - Text                                 *
         *                         ====================                                 *
         *                                                                              *
         ********************************************************************************/

        private void cMnuWordMemory_Click(object sender, EventArgs e)
        {
            int tagVal = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);

            mainTextHandler.copyCurrentWord(0, textCode, tagVal);
        }

        private void cMnuWordNotes_Click(object sender, EventArgs e)
        {
            int tagVal = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);

            mainTextHandler.copyCurrentWord(1, textCode, tagVal);
        }

        private void cMnuVerseMemory_Click(object sender, EventArgs e)
        {
            int tagVal = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);

            mainTextHandler.copyCurrentVerse(0, textCode, tagVal);
        }

        private void cMnuVerseNotes_Click(object sender, EventArgs e)
        {
            int tagVal = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);

            mainTextHandler.copyCurrentVerse(1, textCode, tagVal);
        }

        private void cMnuChapterMemory_Click(object sender, EventArgs e)
        {
            int tagVal = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);

            mainTextHandler.copyCurrentChapter(0, textCode, tagVal);
        }

        private void cMnuChapterNotes_Click(object sender, EventArgs e)
        {
            int tagVal = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);

            mainTextHandler.copyCurrentChapter(1, textCode, tagVal);
        }

        private void mnuSelectedMemory_Click(object sender, EventArgs e)
        {
            processSelectedCopy(1);
        }

        private void mnuSelectedNotes_Click(object sender, EventArgs e)
        {
            processSelectedCopy(2);
        }

        private void processSelectedCopy(int actionCode)
        {
            int nStart, nEnd;
            String selectedTextPortion;
            RichTextBox rtxtCurrent;

            if (tabCtlText.SelectedIndex == 0) rtxtCurrent = rtxtMainText;
            else rtxtCurrent = rtxtLxxMainText;
            if (rtxtCurrent.SelectionLength == 0) return;
            nStart = rtxtCurrent.SelectionStart;
            nEnd = rtxtCurrent.SelectionLength;
            selectedTextPortion = rtxtCurrent.Text.Substring(nStart, nEnd);
            if (actionCode == 1)
            {
                Clipboard.SetText(selectedTextPortion);
                MessageBox.Show("The selected text has been copied to the clipboard.", "Copy Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else mainTextHandler.insertTextToNotes(selectedTextPortion);
        }

        private void cMnuPrimarySearch_Click(object sender, EventArgs e)
        {
            int oldOrNew;

            oldOrNew = tabCtlText.SelectedIndex;
            labNtLxx.Text = oldOrNew.ToString();
            if (oldOrNew == 0)
            {
                labBookCode.Text = cbBook.SelectedIndex.ToString();
                labChap.Text = cbChapter.SelectedItem.ToString();
                labVerse.Text = cbVerse.SelectedItem.ToString();
            }
            else
            {
                labBookCode.Text = cbLxxBook.SelectedIndex.ToString();
                labChap.Text = cbLxxChapter.SelectedItem.ToString();
                labVerse.Text = cbLxxVerse.SelectedItem.ToString();
            }
            searchProcedures.copyToSearch(txtPrimaryWord, textCode);
            tabCtrlBottom.SelectedIndex = 2;
            tabCtrlTop.SelectedIndex = 2;
        }

        private void cMnuSecondarySearch_Click(object sender, EventArgs e)
        {
            int oldOrNew;

            oldOrNew = tabCtlText.SelectedIndex;
            labNtLxx2ary.Text = oldOrNew.ToString();
            if (oldOrNew == 0)
            {
                labBookCode2ary.Text = cbBook.SelectedIndex.ToString();
                labChap2ary.Text = cbChapter.SelectedItem.ToString();
                labVerse2ary.Text = cbVerse.SelectedItem.ToString();
            }
            else
            {
                labBookCode2ary.Text = cbLxxBook.SelectedIndex.ToString();
                labChap2ary.Text = cbLxxChapter.SelectedItem.ToString();
                labVerse2ary.Text = cbLxxVerse.SelectedItem.ToString();
            }
            searchProcedures.copyToSearch(txtSecondaryWord, textCode);
            tabCtrlBottom.SelectedIndex = 2;
            tabCtrlTop.SelectedIndex = 2;
            lblWithin.Visible = true;
            upDownWithin.Visible = true;
            lblWordsOf.Visible = true;
            txtSecondaryWord.Visible = true;
            btnSearchType.Text = "Basic Search";
        }

        /********************************************************************************
         *                                                                              *
         *               Context Menus - Grammatical & Lexical Areas                    *
         *               ===========================================                    *
         *                                                                              *
         ********************************************************************************/

        private void mnuGrammMemory_Click(object sender, EventArgs e)
        {
            mainTextHandler.copyParseAndLexiconData(0, false);
        }

        private void mnuGrammNotes_Click(object sender, EventArgs e)
        {
            mainTextHandler.copyParseAndLexiconData(0, true);
        }

        private void mnuLexMemory_Click(object sender, EventArgs e)
        {
            mainTextHandler.copyParseAndLexiconData(1, false);
        }

        private void mnuLexNotes_Click(object sender, EventArgs e)
        {
            mainTextHandler.copyParseAndLexiconData(1, true);
        }

        private void mnuBothMemory_Click(object sender, EventArgs e)
        {
            mainTextHandler.copyParseAndLexiconData(2, false);
        }

        private void mnuBothNotes_Click(object sender, EventArgs e)
        {
            mainTextHandler.copyParseAndLexiconData(2, true);
        }

        /********************************************************************************
         *                                                                              *
         *                         Context Menus - Notes                                *
         *                         =====================                                *
         *                                                                              *
         ********************************************************************************/

        private void notesAreaMouseDown(object sender, MouseEventArgs e)
        {
            globalVars.LatestNotesPosition = rtxtNotes.GetCharIndexFromPosition(new Point(e.X, e.Y));
        }

        private void mnuNotesCopyWord_Click(object sender, EventArgs e)
        {
            int nStart, nEnd;
            String noteWord;
            Char[] possibleDelimiters = { ' ', '\n' };

            nStart = rtxtNotes.Text.LastIndexOfAny(possibleDelimiters, globalVars.LatestNotesPosition);
            nStart++;
            nEnd = rtxtNotes.Text.IndexOfAny(possibleDelimiters, globalVars.LatestNotesPosition);
            if (nEnd == -1) nEnd = rtxtNotes.Text.Length;
            noteWord = rtxtNotes.Text.Substring(nStart, nEnd - nStart);
            Clipboard.SetText(noteWord);
            MessageBox.Show("The word \"" + noteWord + "\" has been copied to the clipboard.", "Copy Successful",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuNotesCopySelected_Click(object sender, EventArgs e)
        {
            String copiedText;

            if (rtxtNotes.SelectionLength == 0) return;
            copiedText = rtxtNotes.Text.Substring(rtxtNotes.SelectionStart, rtxtNotes.SelectionLength);
            Clipboard.SetText(copiedText);
            MessageBox.Show("The selected text has been copied to the clipboard.", "Copy Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuNotesPaste_Click(object sender, EventArgs e)
        {
            int currentPstn, lengthOfPaste;
            String clipboardContent, firstHalf, secondHalf;

            if (!Clipboard.ContainsText()) return;
            clipboardContent = Clipboard.GetText();
            lengthOfPaste = clipboardContent.Length;
            currentPstn = rtxtNotes.SelectionStart;
            if (currentPstn == 0)
            {
                rtxtNotes.Text = clipboardContent + rtxtNotes.Text;
            }
            else
            {
                if (currentPstn == rtxtNotes.Text.Length - 1)
                {
                    rtxtNotes.AppendText(clipboardContent);
                }
                else
                {
                    // This is the awkward option
                    firstHalf = rtxtNotes.Text.Substring(0, currentPstn);
                    secondHalf = rtxtNotes.Text.Substring(currentPstn);
                    rtxtNotes.Text = firstHalf + clipboardContent + secondHalf;
                }
            }
            rtxtNotes.SelectionStart = currentPstn + lengthOfPaste;
            rtxtNotes.SelectionLength = 0;
        }

        /*------------------------------------------------------------------------------*
         *                                                                              *
         *                         End of Context Menus                                 *
         *                         ====================                                 *
         *                                                                              *
         ********************************************************************************/

        private void cbVerse_SelectedIndexChanged(object sender, EventArgs e)
        {
            notesProcessing.processNewNote(0);
        }

        private void cbLxxVerse_SelectedIndexChanged(object sender, EventArgs e)
        {
            notesProcessing.processNewNote(1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int idx, radioButtonOption = 0, searchLbIndex, bookId = -1, code, simpleNTCount = 0, simpleLXXCount = 0;
            String /* returnedText, */ bookName;
            bool isComplex = false;
            SortedList<int, int> ntBooksToSearch = new SortedList<int, int>();
            SortedList<int, int> lxxBooksToSearch = new SortedList<int, int>();
            classBookContent currentBook;

            tabCtrlTop.SelectedIndex = 2;
            rtxtSearchResults.Text = "\n\nResearching your search results.\n\nPlease be patient.";
            rtxtSearchResults.Refresh();
            for (searchLbIndex = 0; searchLbIndex < lbAvailableBooks.Items.Count; searchLbIndex++)
            {
                // Confine the search to only those books in the list box (which can be both NT and LXX
                code = -1;
                bookName = lbAvailableBooks.Items[searchLbIndex].ToString();
                for (idx = 0; idx < mainTextHandler.NoOfNTBooks; idx++)
                {
                    // Find the id of the book currently under consideration
                    currentBook = mainTextHandler.getNTBookByIndex(idx);
                    if (currentBook == null) continue;
                    if (String.Compare(currentBook.BookName, bookName) == 0)
                    {
                        bookId = idx;
                        code = 0;
                        break;
                    }
                }
                if (code == -1) // We didn't find the book in the NT list
                {
                    // So, repeat the process for LXX
                    for (idx = 0; idx < mainTextHandler.NoOfLxxBooks; idx++)
                    {
                        currentBook = mainTextHandler.getLXXBookByIndex(idx);
                        if (currentBook == null) continue;
                        if (String.Compare(currentBook.BookName, bookName) == 0)
                        {
                            bookId = idx;
                            code = 1;
                            break;
                        }
                    }
                }
                if (code == 0) ntBooksToSearch.Add(simpleNTCount++, bookId);
                if (code == 1) lxxBooksToSearch.Add(simpleLXXCount++, bookId);
            }
            // By the end of this, we will have two populated lists: one of NT books and the other of LXX books to be searched
            // Now set up the various search parameters
            if (txtSecondaryWord.Visible) isComplex = true;
            if (rbtnRoots.Checked) radioButtonOption = 1;
            if (rbtnExact.Checked) radioButtonOption = 2;
            searchProcedures.performSearch(txtPrimaryWord.Text.Trim(), isComplex, radioButtonOption,
                txtSecondaryWord.Text.Trim(), (int)upDownWithin.Value, ntBooksToSearch, lxxBooksToSearch, Convert.ToInt32(labNtLxx.Text),
                Convert.ToInt32(labBookCode.Text), labChap.Text, labVerse.Text, Convert.ToInt32(labNtLxx2ary.Text),
                Convert.ToInt32(labBookCode2ary.Text), labChap2ary.Text, labVerse2ary.Text);
            displaySearchResults();
            tabCtrlTop.SelectedIndex = 2;
        }

        private void displaySearchResults()
        {
            int idx, jdx, wdx, noOfResults, noOfWords, primaryMatchCount, secondaryMatchCount;
            String bookName;
            classSearchResult currentResult;
            classSearchResultLine currentLine;
            Font mainFont;
            Color mainTextColour, primaryColour, secondaryColour;

            // Now use the info to set up results
            rtxtSearchResults.Text = "";
            mainFont = globalVars.FontForArea[4];
            mainTextColour = globalVars.ForeColorRec[4];
            primaryColour = globalVars.PrimaryColour;
            secondaryColour = globalVars.SecondaryColour;
            noOfResults = searchProcedures.NoOfResults;
            for (idx = 0; idx < noOfResults; idx++)
            {
                currentResult = searchProcedures.getSearchResultByIndex(idx);
                if (currentResult == null) continue;
                if (idx > 0)
                {
                    if (currentResult.IsFollowOn) rtxtSearchResults.AppendText("\n");
                    else rtxtSearchResults.AppendText("\n\n");
                }
                bookName = currentResult.BookName;
                for (jdx = 0; jdx < 5; jdx++)
                {
                    currentLine = currentResult.getResultByLine(jdx);
                    if (currentLine == null) continue;
                    rtxtSearchResults.SelectionFont = mainFont;
                    rtxtSearchResults.SelectionColor = mainTextColour;
                    rtxtSearchResults.SelectedText = bookName + " " + currentLine.ChapterNo + "." + currentLine.VerseNo + ": ";
                    noOfWords = currentLine.NoOfWords;
                    primaryMatchCount = 0;
                    secondaryMatchCount = 0;
                    for (wdx = 0; wdx < noOfWords; wdx++)
                    {
                        rtxtSearchResults.SelectionFont = mainFont;
                        if (wdx == currentLine.getPrimaryMatchByIndex(primaryMatchCount))
                        {
                            rtxtSearchResults.SelectionColor = primaryColour;
                            primaryMatchCount++;
                        }
                        else
                        {
                            if (wdx == currentLine.getSecondaryMatchByIndex(secondaryMatchCount))
                            {
                                rtxtSearchResults.SelectionColor = secondaryColour;
                                secondaryMatchCount++;
                            }
                            else rtxtSearchResults.SelectionColor = mainTextColour;
                        }
                        rtxtSearchResults.SelectedText = " " + currentLine.getWordByIndex(wdx);
                    }
                }
            }
        }
        private void TmrProgress_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnSearchType_Click(object sender, EventArgs e)
        {
            if (lblWithin.Visible)
            {
                lblWithin.Visible = false;
                upDownWithin.Visible = false;
                lblWordsOf.Visible = false;
                txtSecondaryWord.Visible = false;
                btnSearchType.Text = "Advanced Search";
            }
            else
            {
                lblWithin.Visible = true;
                upDownWithin.Visible = true;
                lblWordsOf.Visible = true;
                txtSecondaryWord.Visible = true;
                btnSearchType.Text = "Basic Search";
            };
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int idx, vdx, wordCount = 0, nStart, nEnd, bookId = 0, altBookId, chapSeq, noOfWords, noOfVerses, noOfBooks = 0;
            String verseNumber = "", bookName = "";
            int[] vocabTypes = { 0, 0, 0, 0, 0, 0 };
            SortedDictionary<int, classBookContent> listOfBooks;
            SortedDictionary<int, classWordContent> wordsInSequence = new SortedDictionary<int, classWordContent>();
            classBookContent currentBook = null;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            classWordContent currentWord;
            RichTextBox targetTextbox;

            if (tabCtlText.SelectedIndex == 0)
            {
                noOfBooks = mainTextHandler.NoOfNTBooks;
                listOfBooks = mainTextHandler.ListOfNTBooks;
                bookName = cbBook.SelectedItem.ToString();
                chapSeq = cbChapter.SelectedIndex;
                targetTextbox = globalVars.getRichtextControlByIndex(0);
            }
            else
            {
                noOfBooks = mainTextHandler.NoOfLxxBooks;
                listOfBooks = mainTextHandler.ListOfLxxBooks;
                bookName = cbLxxBook.SelectedItem.ToString();
                chapSeq = cbLxxChapter.SelectedIndex;
                targetTextbox = globalVars.getRichtextControlByIndex(1);
            }
            altBookId = -1;
            for (idx = 0; idx < noOfBooks; idx++)
            {
                listOfBooks.TryGetValue(idx, out currentBook);
                if (String.Compare(currentBook.BookName, bookName) == 0)
                {
                    altBookId = idx;
                    break;
                }
            }
            if (altBookId == -1) return;
            currentChapter = currentBook.getChapterBySeqNo(chapSeq);
            rtxtVocabList.Text = bookName + " " + currentBook.getChapterIdBySeqNo(chapSeq);
            if (rbtnVocabVerse.Checked)
            {
                nStart = targetTextbox.SelectionStart;
                if (nStart >= targetTextbox.Text.Length) nStart = targetTextbox.Text.Length - 1;
                nStart = targetTextbox.Text.LastIndexOf('\n', nStart);
                if (nStart == -1) nStart = 0;
                nEnd = targetTextbox.Text.IndexOf(':', nStart);
                if (nEnd == -1) nStart = targetTextbox.Text.LastIndexOf('\n', nStart);
                if (nStart == -1) nStart = 0;
                else
                {
                    if (nStart > 0) nStart++;
                }
                nEnd = targetTextbox.Text.IndexOf(':', nStart);
                if (nEnd == -1) return;
                if (nStart >= nEnd) return;
                try
                {
                    verseNumber = targetTextbox.Text.Substring(nStart, nEnd - nStart);
                }
                catch
                {
                    return;
                }
                currentVerse = currentChapter.getVerseByVerseNo(verseNumber);
                rtxtVocabList.AppendText(":" + verseNumber);
                noOfWords = currentVerse.WordCount;
                for (idx = 0; idx < noOfWords; idx++)
                {
                    currentWord = currentVerse.getWordBySeqNo(idx);
                    wordsInSequence.Add(wordCount++, currentWord);
                }
            }
            else  // We're dealing with the whole chapter
            {
                // All we do here is list the words *in sequence*, without worrying about verses or verse numbers
                noOfVerses = currentChapter.NoOfVersesInChapter;
                for (vdx = 0; vdx < noOfVerses; vdx++)
                {
                    currentVerse = currentChapter.getVerseBySeqNo(vdx);
                    noOfWords = currentVerse.WordCount;
                    for (idx = 0; idx < noOfWords; idx++)
                    {
                        currentWord = currentVerse.getWordBySeqNo(idx);
                        wordsInSequence.Add(wordCount++, currentWord);
                    }
                }
            }
            rtxtVocabList.AppendText("\n");
            processVocabulary(wordsInSequence, bookId, listOfBooks, noOfBooks);
            tabCtrlTop.SelectedIndex = 3;
        }

        /***************************************************************************************************
         *                                                                                                 *
         *                                        processVocabulary                                        *
         *                                        =================                                        *
         *                                                                                                 *
         *  This will create the vocabulary lists, according to options selected in the bottom pane.       *
         *                                                                                                 *
         *  Parameters:                                                                                    *
         *  ==========                                                                                     *
         *                                                                                                 *
         *  wordsInSequence       A list of word instances, stored in the order that they occur            *
         *  bookId                As is used throughout the application, an index of the book currently in *
         *                          view                                                                   *
         *  listOfBooks           The full list of books, to which bookId refers                           *
         *  noOfBooks             Fairly obvious                                                           *
         *                                                                                                 *
         *  Key variables (below):                                                                         *
         *  =====================                                                                          *
         *                                                                                                 *
         *  wordsInOrder          This provides a simple way of sorting the words, should a sorted option  *
         *                          be chosen                                                              *
         *  isFirstInCat                                                                                   *
         *  isPos                                                                                          *
         *  vocabTypes            Stores the specific grammatical options chosen - saves having to         *
         *                          constantly refer back to the radio buttons                             *
         *  parseOptions                                                                                   *
         *  parseLxxOptions       The string representations used to designated parts of speech as used in *
         *                          the source data                                                        *
         *  parseOptionNames      More orthodox part of speech descriptions, used in the actual lists      *
         *                                                                                                 *
         ***************************************************************************************************/

        private void processVocabulary(SortedDictionary<int, classWordContent> wordsInSequence, int bookId,
            SortedDictionary<int, classBookContent> listOfBooks, int noOfBooks)
        {
            bool isFirstInCat, isPos;
            int idx, jdx, vocabCheckCount, noOfPos, collationCode = 0;
            String wordUnderConsideration;
            int[] vocabTypes = { 0, 0, 0, 0, 0, 0 };
            String[] parseOptions = { "N-", "V-", "A-", "X-", "P-", "C-", "D-", "I-", "RA", "RD", "RI", "RP", "RR" };
            Char[] parseLxxOptions = { 'N', 'V', 'A', 'D', 'P', 'C', 'X', 'I', 'M', 'R' };
            String[] parseOptionNames = { "Nouns:", "Verbs:", "Adjectives:", "Adverbs:", "Prepositions:", "Other word types:" };
            SortedDictionary<String, classWordContent> wordsInOrder = new SortedDictionary<string, classWordContent>();

            //            rtxtVocabList.Text = "";
            for (idx = 0; idx < 6; idx++) vocabTypes[idx] = 0;
            // vocabCheckCount - used to check whether all checkboxes are left blank; if so, they are all set as if checked
            vocabCheckCount = 0;
            /*---------------------------------------------------------------------*
             *                         Populate vocabTypes                         * 
             *                                                                     * 
             *  Each vocab type represents a different part of speech.  So:        *
             *    Index       Part of Speech                                       *
             *      0           Noun                                               *
             *      1           Verb                                               *
             *      2           Adjective                                          *
             *      3           Adverb                                             *
             *      4           Preposition                                        *
             *      5           All other pos (true idx = 5 to 12)                 *
             *  In each case, if vocabTypes[x] = 1, that Pos has been selected;    *
             *    if 0, then omit.                                                 *
             *---------------------------------------------------------------------*/
            if (chBoxNouns.Checked) { vocabTypes[0] = 1; vocabCheckCount++; }
            if (chBoxVerbs.Checked) { vocabTypes[1] = 1; vocabCheckCount++; }
            if (chBoxAdjectives.Checked) { vocabTypes[2] = 1; vocabCheckCount++; }
            if (chBoxAdverbs.Checked) { vocabTypes[3] = 1; vocabCheckCount++; }
            if (chBoxPrepositions.Checked) { vocabTypes[4] = 1; vocabCheckCount++; }
            if (chBoxOthers.Checked) { vocabTypes[5] = 1; vocabCheckCount++; }
            if (vocabCheckCount == 0)
            {
                for (idx = 0; idx < 6; idx++) vocabTypes[idx] = 1;
            }
            /*---------------------------------------------------------------------*
             *                       Populate collationCode                        * 
             *                                                                     * 
             *  This code determines what word forms are shown in the list:        *
             *    Index       Part of Speech                                       *
             *      1           roots only                                         *
             *      2           the form of word as used in the passage            *
             *      3           both 1 and 2, with any ordering based on the root  *
             *      4           both 1 and 2, with usage determining sequence      *
             *---------------------------------------------------------------------*/
            if (rbtnShowRoots.Checked) collationCode = 1;
            if (rbtnShowAsUsed.Checked) collationCode = 2;
            if (rbtnShowBothByRoot.Checked) collationCode = 3;
            if (rbtnShowBothByWord.Checked) collationCode = 4;
            // noOfPos = number of items in parse???Options
            if (tabCtlText.SelectedIndex == 0) noOfPos = 13;
            else noOfPos = 10;
            if ((rbtnByCatAlpha.Checked) || (rbtnMixedAlpha.Checked))
            {
                // We need to work in alphabetical order
                // Which of the two options we use, we need our list recast in alphabetical order - which includes removing duplicate roots
                foreach (KeyValuePair<int, classWordContent> wordPair in wordsInSequence)
                {
                    if ((collationCode == 1) || (collationCode == 3)) wordUnderConsideration = wordPair.Value.RootWord;
                    else wordUnderConsideration = wordPair.Value.BareTextWord;
                    if (!wordsInOrder.ContainsKey(wordUnderConsideration)) wordsInOrder.Add(wordUnderConsideration, wordPair.Value);
                }
                if (rbtnByCatAlpha.Checked)
                {
                    for (idx = 0; idx < 6; idx++)
                    {
                        isFirstInCat = true;
                        if (vocabTypes[idx] == 1)
                        {
                            foreach (KeyValuePair<String, classWordContent> wordPair in wordsInOrder)
                            {
                                if (idx < 5)
                                {
                                    //  a. Treat the simpler Pos differently;
                                    //  b. Process each Pos in turn
                                    // Check that the pos category for this word is the current one
                                    if (tabCtlText.SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[idx]) == 0;
                                    else isPos = wordPair.Value.CatString[0] == parseLxxOptions[idx];
                                    if (isPos)
                                    {
                                        if (isFirstInCat)
                                        {
                                            if (rtxtVocabList.Text.Length == 0) rtxtVocabList.AppendText(parseOptionNames[idx]);
                                            else rtxtVocabList.AppendText("\n" + parseOptionNames[idx]);
                                            isFirstInCat = false;
                                        }
                                        switch (collationCode)
                                        {
                                            case 1: rtxtVocabList.AppendText("\n\t" + wordPair.Value.RootWord); break;
                                            case 2: rtxtVocabList.AppendText("\n\t" + wordPair.Value.TextWord); break;
                                            case 3: rtxtVocabList.AppendText("\n\t" + displayTwoWords(wordPair.Value.RootWord, wordPair.Value.TextWord)); break;
                                            case 4: rtxtVocabList.AppendText("\n\t" + displayTwoWords(wordPair.Value.TextWord, wordPair.Value.RootWord)); break;
                                        }
                                    }
                                }
                                else
                                {
                                    for (jdx = 5; jdx < noOfPos; jdx++)
                                    {
                                        if (tabCtlText.SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[jdx]) == 0;
                                        else isPos = wordPair.Value.CatString[0] == parseLxxOptions[jdx];
                                        if (isPos)
                                        {
                                            if (isFirstInCat)
                                            {
                                                if (rtxtVocabList.Text.Length == 0) rtxtVocabList.AppendText(parseOptionNames[idx]);
                                                else rtxtVocabList.AppendText("\n" + parseOptionNames[idx]);
                                                isFirstInCat = false;
                                            }
                                            switch (collationCode)
                                            {
                                                case 1: rtxtVocabList.AppendText("\n\t" + wordPair.Value.RootWord); break;
                                                case 2: rtxtVocabList.AppendText("\n\t" + wordPair.Value.TextWord); break;
                                                case 3: rtxtVocabList.AppendText("\n\t" + displayTwoWords(wordPair.Value.RootWord, wordPair.Value.TextWord)); break;
                                                case 4: rtxtVocabList.AppendText("\n\t" + displayTwoWords(wordPair.Value.TextWord, wordPair.Value.RootWord)); break;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else  // this means rbtnMixedAlpha is Checked
                {
                    foreach (KeyValuePair<String, classWordContent> wordPair in wordsInOrder)
                    {
                        jdx = 0;
                        while (jdx < noOfPos)
                        {
                            if (tabCtlText.SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[jdx]) == 0;
                            else isPos = wordPair.Value.CatString[0] == parseLxxOptions[jdx];
                            if (isPos) break;
                            jdx++;
                        }
                        if (jdx < noOfPos) // we found a match
                        {
                            if (jdx > 4) jdx = 5;
                            if (vocabTypes[jdx] == 1)
                            {
                                if (rtxtVocabList.Text.Length > 0) rtxtVocabList.AppendText("\n\t");
                                switch (collationCode)
                                {
                                    case 1: rtxtVocabList.AppendText(wordPair.Value.RootWord); break;
                                    case 2: rtxtVocabList.AppendText(wordPair.Value.TextWord); break;
                                    case 3: rtxtVocabList.AppendText(displayTwoWords(wordPair.Value.RootWord, wordPair.Value.TextWord)); break;
                                    case 4: rtxtVocabList.AppendText(displayTwoWords(wordPair.Value.TextWord, wordPair.Value.RootWord)); break;
                                }
                            }
                        }
                    }
                }
            }
            else //As they come rather than alphabetic
            {
                if (rbtnByTypeRandomnly.Checked)
                {
                    for (idx = 0; idx < 6; idx++)
                    {
                        isFirstInCat = true;
                        if (vocabTypes[idx] == 1)
                        {
                            foreach (KeyValuePair<int, classWordContent> wordPair in wordsInSequence)
                            {
                                if (idx < 5)
                                {
                                    if (tabCtlText.SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[idx]) == 0;
                                    else isPos = wordPair.Value.CatString[0] == parseLxxOptions[idx];
                                    if (isPos)
                                    {
                                        if (isFirstInCat)
                                        {
                                            if (rtxtVocabList.Text.Length == 0) rtxtVocabList.AppendText(parseOptionNames[idx]);
                                            else rtxtVocabList.AppendText("\n" + parseOptionNames[idx]);
                                            isFirstInCat = false;
                                        }
                                        if ((collationCode == 1) || (collationCode == 3)) wordUnderConsideration = wordPair.Value.RootWord;
                                        else wordUnderConsideration = wordPair.Value.TextWord;
                                        if (!wordsInOrder.ContainsKey(wordUnderConsideration))
                                        {
                                            switch (collationCode)
                                            {
                                                case 1: rtxtVocabList.AppendText("\n\t" + wordPair.Value.RootWord); break;
                                                case 2: rtxtVocabList.AppendText("\n\t" + wordPair.Value.TextWord); break;
                                                case 3: rtxtVocabList.AppendText("\n\t" + displayTwoWords(wordPair.Value.RootWord, wordPair.Value.TextWord)); break;
                                                case 4: rtxtVocabList.AppendText("\n\t" + displayTwoWords(wordPair.Value.TextWord, wordPair.Value.RootWord)); break;
                                            }
                                            wordsInOrder.Add(wordUnderConsideration, wordPair.Value);
                                        }
                                    }
                                }
                                else
                                {
                                    for (jdx = 5; jdx < noOfPos; jdx++)
                                    {
                                        if (tabCtlText.SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[jdx]) == 0;
                                        else isPos = wordPair.Value.CatString[0] == parseLxxOptions[jdx];
                                        if (isPos)
                                        {
                                            if (isFirstInCat)
                                            {
                                                if (rtxtVocabList.Text.Length == 0) rtxtVocabList.AppendText(parseOptionNames[idx]);
                                                else rtxtVocabList.AppendText("\n" + parseOptionNames[idx]);
                                                isFirstInCat = false;
                                            }
                                            if ((collationCode == 1) || (collationCode == 3)) wordUnderConsideration = wordPair.Value.RootWord;
                                            else wordUnderConsideration = wordPair.Value.TextWord;
                                            if (!wordsInOrder.ContainsKey(wordUnderConsideration))
                                            {
                                                switch (collationCode)
                                                {
                                                    case 1: rtxtVocabList.AppendText("\n\t" + wordPair.Value.RootWord); break;
                                                    case 2: rtxtVocabList.AppendText("\n\t" + wordPair.Value.TextWord); break;
                                                    case 3: rtxtVocabList.AppendText("\n\t" + displayTwoWords(wordPair.Value.RootWord, wordPair.Value.TextWord)); break;
                                                    case 4: rtxtVocabList.AppendText("\n\t" + displayTwoWords(wordPair.Value.TextWord, wordPair.Value.RootWord)); break;
                                                }
                                                wordsInOrder.Add(wordUnderConsideration, wordPair.Value);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else // mixed an as used
                {
                    foreach (KeyValuePair<int, classWordContent> wordPair in wordsInSequence)
                    {
                        jdx = 0;
                        while (jdx < noOfPos)
                        {
                            if (tabCtlText.SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[jdx]) == 0;
                            else isPos = wordPair.Value.CatString[0] == parseLxxOptions[jdx];
                            if (isPos) break;
                            jdx++;
                        }
                        if (jdx < noOfPos) // we found a match
                        {
                            if (jdx > 4) jdx = 5;
                            if (vocabTypes[jdx] == 1)
                            {
                                if ((collationCode == 1) || (collationCode == 3)) wordUnderConsideration = wordPair.Value.RootWord;
                                else wordUnderConsideration = wordPair.Value.TextWord;
                                if (!wordsInOrder.ContainsKey(wordUnderConsideration))
                                {
                                    if (rtxtVocabList.Text.Length > 0) rtxtVocabList.AppendText("\n\t");
                                    switch (collationCode)
                                    {
                                        case 1: rtxtVocabList.AppendText(wordPair.Value.RootWord); break;
                                        case 2: rtxtVocabList.AppendText(wordPair.Value.TextWord); break;
                                        case 3: rtxtVocabList.AppendText(displayTwoWords(wordPair.Value.RootWord, wordPair.Value.TextWord)); break;
                                        case 4: rtxtVocabList.AppendText(displayTwoWords(wordPair.Value.TextWord, wordPair.Value.RootWord)); break;
                                    }
                                    // We're using wordsInOrder simply as a check here
                                    wordsInOrder.Add(wordUnderConsideration, wordPair.Value);
                                }
                            }
                        }
                    }
                }
            }
        }

        private String displayTwoWords(String firstWord, String secondWord)
        {
            int idx, noOfSpaces;
            String resText;

            noOfSpaces = 30 - firstWord.Length;
            if (noOfSpaces < 1) noOfSpaces = 1;
            resText = firstWord;
            for (idx = 0; idx < noOfSpaces; idx++) resText += " ";
            resText += secondWord;
            return resText;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int idx, noOfSelections;
            int[] selectionArray;
            ListBox.SelectedIndexCollection selectedBooks;

            selectedBooks = lbAvailableBooks.SelectedIndices;
            noOfSelections = selectedBooks.Count;
            if (noOfSelections > 0)
            {
                selectionArray = new int[noOfSelections];
                selectedBooks.CopyTo(selectionArray, 0);
                for (idx = noOfSelections - 1; idx >= 0; idx--)
                {
                    lbAvailableBooks.Items.RemoveAt(selectionArray[idx]);
                }
            }
        }

        private void tmrRightPanes_Tick(object sender, EventArgs e)
        {
            int targetSize;

            switch (rightPanesControl)
            {
                case 0:
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
                case 1:
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
                case 2:
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

        private void cbRightPanes_SelectedIndexChanged(object sender, EventArgs e)
        {
            rightPanesControl = cbRightPanes.SelectedIndex;
            tmrRightPanes.Enabled = true;
        }

        /********************************************************************************
         *                                                                              *
         *                      Context Menus - Search Results                          *
         *                      ==============================                          *
         *                                                                              *
         ********************************************************************************/

        private void cMnuResultsMemory_Click(object sender, EventArgs e)
        {
            searchProcedures.copyAllResults(true);
        }

        private void cMnuResultsNotes_Click(object sender, EventArgs e)
        {
            searchProcedures.copyAllResults(false);
        }

        private void cMnuSingleMemory_Click(object sender, EventArgs e)
        {
            searchProcedures.copySingleResult(searchCursorPstn, true);
        }

        private void cMnuSingleNotes_Click(object sender, EventArgs e)
        {
            searchProcedures.copySingleResult(searchCursorPstn, false);
        }

        private void rtxtSearchResults_MouseDown(object sender, MouseEventArgs e)
        {
            searchCursorPstn = rtxtSearchResults.GetCharIndexFromPosition(new Point(e.X, e.Y));
        }

        private void cMnuResultsUpdateText_Click(object sender, EventArgs e)
        {
            searchProcedures.updateTextAreaWithSelectedChapter(searchCursorPstn);
        }

        private void mnuFilePrefs_Click(object sender, EventArgs e)
        {
            frmPreferences preferencesWindow = new frmPreferences();
            preferencesWindow.initialisePreferences(globalVars, appRegistry);

            if (preferencesWindow.ShowDialog() == DialogResult.OK)
            {
                rtxtMainText.Font = globalVars.FontForArea[0];
                rtxtMainText.BackColor = globalVars.BackColorRec[0];
                rtxtMainText.ForeColor = globalVars.ForeColorRec[0];

                rtxtLxxMainText.Font = globalVars.FontForArea[1];
                rtxtLxxMainText.BackColor = globalVars.BackColorRec[1];
                rtxtLxxMainText.ForeColor = globalVars.ForeColorRec[1];

                rtxtParse.Font = globalVars.FontForArea[2];
                rtxtParse.BackColor = globalVars.BackColorRec[2];
                rtxtParse.ForeColor = globalVars.ForeColorRec[2];

                rtxtLexicon.Font = globalVars.FontForArea[3];
                rtxtLexicon.BackColor = globalVars.BackColorRec[3];
                rtxtLexicon.ForeColor = globalVars.ForeColorRec[3];

                //                rtxtSearchResults.Font = globalVars.FontForArea[4];
                rtxtSearchResults.BackColor = globalVars.BackColorRec[4];
                //                rtxtSearchResults.ForeColor = globalVars.ForeColorRec[4];
                displaySearchResults();

                rtxtVocabList.Font = globalVars.FontForArea[5];
                rtxtVocabList.BackColor = globalVars.BackColorRec[5];
                rtxtVocabList.ForeColor = globalVars.ForeColorRec[5];

                rtxtNotes.Font = globalVars.FontForArea[6];
                rtxtNotes.BackColor = globalVars.BackColorRec[6];
                rtxtNotes.ForeColor = globalVars.ForeColorRec[6];
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            notesProcessing.processOldNote(tabCtlText.SelectedIndex);
            appRegistry.updateNotesSet();
            mainTextHandler.saveHistory();
        }

        private void mnuFileRelocate_Click(object sender, EventArgs e)
        {
            frmRelocate relocationForm;

            relocationForm = new frmRelocate();
            if( relocationForm.ShowDialog() == DialogResult.OK )
            {
                relocationForm.Dispose();
                if( appRegistry.relocateFiles(relocationForm.SelectedDestination) )
                    MessageBox.Show("Application files successfully relocated", "File Relocation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void rtxtNotes_Leave(object sender, EventArgs e)
        {
            notesProcessing.processOldNote(tabCtlText.SelectedIndex);
        }

        private void mnuNotesCreate_Click(object sender, EventArgs e)
        {
            frmNotes noteManagement = new frmNotes();
            noteManagement.initialiseNotesDialog(globalVars, mainTextHandler, 1);
            noteManagement.ShowDialog();
            noteManagement.Dispose();
        }

        private void mnuNotesDifferent_Click(object sender, EventArgs e)
        {
            frmNotes noteManagement = new frmNotes();
            noteManagement.initialiseNotesDialog(globalVars, mainTextHandler, 2);
            noteManagement.ShowDialog();
            noteManagement.Dispose();
        }

        private void mnuNotesDelete_Click(object sender, EventArgs e)
        {
            frmNotes noteManagement = new frmNotes();
            noteManagement.initialiseNotesDialog(globalVars, mainTextHandler, 3);
            noteManagement.ShowDialog();
            noteManagement.Dispose();
        }

        private void mnuNotesShowName_Click(object sender, EventArgs e)
        {
            frmNotes noteManagement = new frmNotes();
            noteManagement.initialiseNotesDialog(globalVars, mainTextHandler, 4);
            noteManagement.ShowDialog();
            noteManagement.Dispose();
        }

        private void mnuNotesShowLocation_Click(object sender, EventArgs e)
        {
            String message = globalVars.BaseDirectory + @"\" + globalVars.NotesPath + @"\" + globalVars.NotesName + "\n\n(This file location has been copied into memory\n" +
                "so that you can use it in a file manager)";
            Clipboard.SetText(globalVars.BaseDirectory + @"\" + globalVars.NotesPath + @"\" + globalVars.NotesName);
            MessageBox.Show(message, "Current Notes Set Location", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            aboutBox GBSAboutBox;

            GBSAboutBox = new aboutBox();
            GBSAboutBox.ShowDialog();
        }

        private void mnuHelpNTReader_Click(object sender, EventArgs e)
        {
            frmHelp gbsHelp;

            gbsHelp = new frmHelp();
            gbsHelp.initialiseHelp(globalVars.BaseDirectory + @"\" + globalVars.HelpPath + @"\Help.html");
            gbsHelp.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
