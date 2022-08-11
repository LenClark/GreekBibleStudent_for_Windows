using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    public class classLXXText
    {
        const char zeroWidthSpace = '\u200b', zeroWidthNonJoiner = '\u200d';

        classGlobal globalVars;
        frmProgress progressForm;
        classHistory historyProcesses;
        classLexicon lexicon;

        public void initialiseText(classGlobal inGlobal, frmProgress inForm, classHistory inHistory, classLexicon inLex)
        {
            globalVars = inGlobal;
            progressForm = inForm;
            historyProcesses = inHistory;
            lexicon = inLex;
        }

        private delegate void performProgressAdvance(String primaryMessage, String secondaryMessage, bool useSecondary);
        private delegate void performComboBoxUpdate(ComboBox cbCombo, String item);
        private delegate void performComboBoxSelection(ComboBox cbCombo, int indexPstn);
        private delegate void performMainFormLabelChange(Label labLabelLbl, String newText);

        private void updateProgress(String mainMessage, String secondaryMessage, bool useSecondary)
        {
            progressForm.incrementProgress(mainMessage, secondaryMessage, useSecondary);
        }

        private void addComboItem(ComboBox cbCombo, String item)
        {
            cbCombo.Items.Add(item);
        }

        private void selectComboItem(ComboBox cbCombo, int indexPstn)
        {
            cbCombo.SelectedIndex = indexPstn;
        }

        private void changeLabel(Label labLabelLbl, String newText)
        {
            labLabelLbl.Text = newText;
        }

        public void loadText()
        {
            int idx, bookNo, maxCategory = 0;
            String titlesFileName, sourceFileName, fileBuffer, bookName, chapterRef, prevChapRef = "?", verseRef, prevVerseRef = "?";
            int[] noInEachCategory;
            Char[] fieldSeparator = { '\t' };
            String[] fields;
            StreamReader srSource;
            classBook currentBook = null;
            classChapter currentChapter = null, prevChapter;
            classVerse currentVerse = null, previousVerse = null;
            classWord currentWord = null;

            /*--------------------------------------------------------*
             *                                                        *
             *  Step 1: Load the titles                               *
             *  ------  ---------------                               *
             *                                                        *
             *--------------------------------------------------------*/
            progressForm.Invoke(new performProgressAdvance(updateProgress), "Loading the names of OT books", "", false);
            titlesFileName = globalVars.LxxTitlesFile;
            srSource = new StreamReader(titlesFileName);
            fileBuffer = srSource.ReadLine();
            bookNo = 0;
            while (fileBuffer != null)
            {
                fields = fileBuffer.Split(fieldSeparator);
                if ((fileBuffer[0] != ';') && (fields.Length > 3))
                {
                    currentBook = new classBook();
                    globalVars.BookList.Add(bookNo, currentBook);
                    bookName = fields[1];
                    currentBook.BookName = bookName;
                    currentBook.BookId = bookNo;
                    currentBook.ShortName = fields[0];
                    currentBook.FileName = fields[3];
                    currentBook.Category = Convert.ToInt32(fields[4]);
                    currentBook.IsNT = false;
                    globalVars.getComboBoxItem(3).Invoke(new performComboBoxUpdate(addComboItem), globalVars.getComboBoxItem(3), currentBook.BookName);
                    bookNo++;
                    if (currentBook.Category > maxCategory) maxCategory = currentBook.Category;
                }
                fileBuffer = srSource.ReadLine();
            }
            globalVars.NoOfLXXBooks = bookNo;
            globalVars.NoOfAllBooks = bookNo;
            srSource.Close();

            /*---------------------------------------------------------------------------------------------------*
             *                                                                                                   *
             *   Step 1b:                                                                                        *
             *   ======                                                                                          *
             *                                                                                                   *
             *   Sort out the categories for search options                                                      *
             *                                                                                                   *
             *   This is a bit squiffy so let's explain.                                                         *
             *                                                                                                   *
             *   Index 1 of DifferentiatedList is the book category - 1, where book category = Pentateuch,       *
             *     minor prophets, etc.                                                                          *
             *   Index 2 is a simple zero-based sequence.                                                        *
             *                                                                                                   *
             *   But, how do we get a sequence, when they occur randomly?  noInEachCategory is an array that     *
             *     keeps a count of each category as we go.  So, the main for loop scans through the books in    *
             *     sequence (i.e. the class instances of books in sequence) and extracts the code for the        *
             *     category.  This is used for index 1.  It uses the current count of noInEachCategory as index  *
             *     2 and records the instance address for the book.                                              *
             *                                                                                                   *
             *---------------------------------------------------------------------------------------------------*/

            noInEachCategory = new int[maxCategory];
            for (idx = 0; idx < maxCategory; idx++) noInEachCategory[idx] = 0;
            for( idx = 0; idx < globalVars.NoOfLXXBooks; idx++)
            {
                globalVars.BookList.TryGetValue(idx, out currentBook);
                globalVars.DifferentiatedList[currentBook.Category - 1, noInEachCategory[currentBook.Category - 1]] = currentBook;
                noInEachCategory[currentBook.Category - 1]++;
            }

            /*---------------------------------------------------------------------------------------------------*
             *                                                                                                   *
             *   Step 2:                                                                                         *
             *   ======                                                                                          *
             *                                                                                                   *
             *   Now load the relevant LXX text data.                                                            *
             *                                                                                                   *
             *---------------------------------------------------------------------------------------------------*/

            // Now for the  text data. Let's process and store it
            for (idx = 0; idx < bookNo; idx++)
            {
                globalVars.BookList.TryGetValue(idx, out currentBook);
                progressForm.Invoke(new performProgressAdvance(updateProgress), "Loading source text for the Septuagint", currentBook.BookName, true);
                currentChapter = null;
                currentVerse = null;
                prevChapRef = "?";
                sourceFileName = globalVars.LxxTextFolder + @"\" + currentBook.FileName;
                srSource = new StreamReader(sourceFileName);
                fileBuffer = srSource.ReadLine();
                while (fileBuffer != null)
                {
                    /**************************************************************************************
                     *                                                                                    *
                     *  Split the line as follows:                                                        *
                     *                                                                                    *
                     *   Field                    Contents                                                *
                     *   -----  ------------------------------------------------------------------------  *
                     *     1	Chapter number                                                            *
                     *     2	Verse number (note: may = 0 or 12b)                                       *
                     *     3	Initial Parse code                                                        *
                     *     4	Detailed Parse code                                                       *
                     *     5	A unique grammatical identifier                                           *
                     *     6	Word as it is to be displayed in the text                                 *
                     *     7	Word a) all lower case, b) stripped of accents and related furniture      *
                     *     8	Word, as in field 7 but also with breathings and iota subscripts removed  *
                     *     9	Immediate root of word in field 6                                         *
                     *     10	Pre-word characters                                                       *
                     *     11	Post-word non-punctuation characters                                      *
                     *     12	Punctuation                                                               *
                     *                                                                                    *
                     *  However, fields 1 and 2 are as supplied by the source file.  In addition, we will *
                     *  create a simple, sequential index for chapters and verses.  This will allow for:  *
                     *  a) out-of-sequence chapters (in a few books, there are gaps and, even, chapters   *
                     *     transposed;                                                                    *
                     *  b) verses that include text as well as digits (e.g. 19b);                         *
                     *  c) unnumbered verses (in our data, given the index 0)                             *
                     *                                                                                    *
                     **************************************************************************************/
                    fields = fileBuffer.Split(fieldSeparator);
                    chapterRef = fields[0];
                    // Handle the chapter
                    if (String.Compare(chapterRef, prevChapRef) != 0)
                    {
                        prevChapter = currentChapter;
                        currentChapter = currentBook.addNewChapterToBook(chapterRef);
                        if (prevChapter != null) prevChapter.NextChapter = currentChapter;
                        currentChapter.PreviousChapter = prevChapter;
                        currentChapter.BookId = idx;
                        currentChapter.ChapterRef = chapterRef;
                        currentChapter.ChapterNo = currentBook.NoOfChaptersInBook - 1;
                        prevChapRef = chapterRef;
                        prevVerseRef = "?";
                    }
                    // Handle the verse
                    verseRef = fields[1];
                    if (String.Compare(verseRef, prevVerseRef) != 0)
                    {
                        currentVerse = currentChapter.addVerseToChapter(verseRef);
                        if (previousVerse != null)
                        {
                            previousVerse.NextVerse = currentVerse;
                        }
                        currentVerse.PreviousVerse = previousVerse;
                        prevVerseRef = verseRef;
                        previousVerse = currentVerse;
                        currentVerse.ChapSeq = currentChapter.ChapterNo;
                        currentVerse.ChapRef = currentChapter.ChapterRef;
                        currentVerse.VerseSeq = currentChapter.NoOfVersesInChapter - 1;
                        currentVerse.VerseRef = currentChapter.getVerseRefBySeqNo(currentVerse.VerseSeq);
                    }
                    currentWord = currentVerse.addWordToVerse();
                    currentWord.CatString = fields[2];
                    currentWord.ParseString = fields[3];
                    currentWord.UniqueValue = fields[4];
                    currentWord.TextWord = fields[5];
                    currentWord.AccentlessTextWord = fields[6];
                    currentWord.BareTextWord = fields[7];
                    currentWord.RootWord = fields[8];
                    currentWord.Punctuation = fields[11];
                    currentWord.PreWordChars = fields[9];
                    currentWord.PostWordChars = fields[10];
                    fileBuffer = srSource.ReadLine();
                }
                srSource.Close();
            }
            globalVars.getComboBoxItem(3).Invoke(new performComboBoxSelection(selectComboItem), globalVars.getComboBoxItem(3), 0);
            globalVars.getLabel(2).Invoke(new performMainFormLabelChange(changeLabel), globalVars.getLabel(2), "LXX Load Complete");
        }

        public void displayChapter(int bookIdx, String chapIdx, bool isSameBook)
        {
            /*=======================================================================================================*
             *                                                                                                       *
             *                                        displayChapter                                                 *
             *                                        ==============                                                 *
             *                                                                                                       *
             *  Controls the display of a specified chapter                                                          *
             *                                                                                                       *
             *  Parameters:                                                                                          *
             *    bookIdx:      the book index (a zero based index)                                                  *
             *    chapIdx:      chapter number (real chapter, which must be converted to it's equivalent sequence    *
             *                  number                                                                               *
             *                                                                                                       *
             *=======================================================================================================*/

            int idx, cdx, wdx, noOfChapters, noOfVerses, noOfWords;
            String newBookName, displayString = "", realChapNo, realVNo;
            classBook currentBook;
            classChapter currentChapter;
            classVerse currentVerse;
            classWord currentWord;
            RichTextBox targetTextArea;
            ComboBox cbChapter, cbVerse;
            ComboBox cbBook;
            Font engFont, greekFont;
            Color backgroundColour, engTextColour, gkTextColour;

            greekFont = globalVars.configureFont(globalVars.getDefinedFontNameByIndex(1, 2), globalVars.getDefinedStyleByIndex(1, 2), globalVars.getTextSize(1, 2));
            engFont = globalVars.configureFont(globalVars.getDefinedFontNameByIndex(1, 1), globalVars.getDefinedStyleByIndex(1, 1), globalVars.getTextSize(1, 1));
            backgroundColour = globalVars.getColourSetting(1, 0);
            engTextColour = globalVars.getColourSetting(1, 1);
            gkTextColour = globalVars.getColourSetting(1, 2);
            // Get the Rich Text area in which the text occurs
            targetTextArea = globalVars.getRichtextItem(1);
            // Get the combo boxes for the Book, Chapter and Verse
            cbBook = globalVars.getComboBoxItem(3);
            cbChapter = globalVars.getComboBoxItem(4);
            cbVerse = globalVars.getComboBoxItem(5);
            // If any of them don't exist, abort
            if ((cbBook == null) || (cbChapter == null) || (cbVerse == null) || (targetTextArea == null)) return;
            // Get the name of the new book - and, BTW the class instance for the book
            globalVars.BookList.TryGetValue(bookIdx, out currentBook);
            if (currentBook == null) return;
            newBookName = currentBook.BookName;
            if (cbBook.Items.Count == 0) return;
            if (currentBook.NoOfChaptersInBook == 0) return;
            /*=====================================================================================================================*
             *                                                                                                                     *
             *  From this point on, we are in danger of recursion - i.e. the line                                                  *
             *      cbBook.SelectedIndex = bookIdx - 1;                                                                            *
             *    will kick off cbBook_SelectedIndexChanged in frmMain, which, in turn, will invoke mainText.respondToNewBook.     *
             *    Since this will finally invoke displayChapter, we are going to have an unhelpful loop.                           *
             *                                                                                                                     *
             *  We resolve this by flagging the fact that we are _in_ displayChapter by setting globalVars.IsDisplayChapterCalled  *
             *    to true.  While this flag is true, SelectedIndexChanged events will not call displayChapter.                     *
             *                                                                                                                     *
             *  There is a problem with this, however: if we                                                                       *
             *                                                                                                                     *
             *                                                                                                                     *
             *                                                                                                                     *
             *                                                                                                                     *
             *=====================================================================================================================*/
            globalVars.IsDisplayChapterCalled = true;
            if (!isSameBook) cbBook.SelectedIndex = bookIdx;
            // Get the specified chapter from the current book -
            currentChapter = currentBook.getChapterByChapterRef(chapIdx);
            if (!isSameBook)
            {
                cbChapter.Items.Clear();
                noOfChapters = currentBook.NoOfChaptersInBook;
                for (cdx = 0; cdx < noOfChapters; cdx++)
                {
                    realChapNo = currentBook.getChapterRefBySequence(cdx);
                    cbChapter.Items.Add(realChapNo);
                }
            }
            //            globalVars.IsOnHold = true;
            cbChapter.SelectedItem = chapIdx.ToString();
            //            globalVars.IsOnHold = false;
            noOfVerses = currentChapter.NoOfVersesInChapter;
            targetTextArea.BackColor = backgroundColour;
            targetTextArea.Text = "";
            cbVerse.Items.Clear();
            for (idx = 0; idx < noOfVerses; idx++)
            {
                currentVerse = currentChapter.getVerseBySeqNo(idx);
                if (currentVerse == null) continue;
                realVNo = currentChapter.getVerseRefBySeqNo(idx);
                cbVerse.Items.Add(realVNo.ToString());
                if (idx > 0)
                {
                    targetTextArea.AppendText("\n");
                }
                targetTextArea.SelectionColor = engTextColour;
                targetTextArea.SelectionFont = engFont;
                targetTextArea.SelectedText = realVNo.ToString() + ": ";
                noOfWords = currentVerse.WordCount;
                for (wdx = 0; wdx < noOfWords; wdx++)
                {
                    targetTextArea.SelectionColor = gkTextColour;
                    currentWord = currentVerse.getWordBySeqNo(wdx);
                    targetTextArea.SelectionFont = greekFont;
                    targetTextArea.SelectedText = " " + currentWord.PreWordChars;
                    targetTextArea.SelectedText = zeroWidthSpace.ToString() + currentWord.TextWord;
                    targetTextArea.SelectedText = zeroWidthNonJoiner + currentWord.PostWordChars + currentWord.Punctuation;
                }
            }
            cbVerse.SelectedIndex = 0;
            if (globalVars.getTabControl(0).SelectedTab == globalVars.getTabCtrlPage(1))
            {
                displayString = newBookName + " " + chapIdx;
                globalVars.MasterForm.Text = "Greek Bible Student - Septuagint: " + displayString;
                addEntryToHistory(displayString);
            }
            globalVars.IsDisplayChapterCalled = false;
        }

        public void backOrForwardOneChapter(int forwardBack)
        {
            /*================================================================================================*
             *                                                                                                *
             *                                    backOrForwardOneChapter                                     *
             *                                    =======================                                     *
             *                                                                                                *
             *  Simply handles moving backwards or forwards from the present chapter.                         *
             *                                                                                                *
             *  Parameters:                                                                                   *
             *  ==========                                                                                    *
             *    forwardBack: 1 = previous chapter                                                           *
             *                 2 = next chapter                                                               *
             *                                                                                                *
             *================================================================================================*/

            int bookIdx = 0, actualIdx, chapIdx;
            String chapNo;
            ComboBox cbBooks, cbChapters;
            classBook currentBook;
            classChapter currentChapter, advChapter;

            cbBooks = globalVars.getComboBoxItem(3);
            cbChapters = globalVars.getComboBoxItem(4);
            actualIdx = cbBooks.SelectedIndex;
            chapIdx = cbChapters.SelectedIndex;
            globalVars.BookList.TryGetValue(actualIdx, out currentBook);
            currentChapter = currentBook.getChapterBySequence(chapIdx);
            if (forwardBack == 2) advChapter = currentChapter.NextChapter;
            else advChapter = currentChapter.PreviousChapter;
            if (advChapter == null) return;
            bookIdx = advChapter.BookId;
            chapNo = advChapter.ChapterRef;
            displayChapter(bookIdx, chapNo, true);
        }

        public void respondToNewBook()
        {
            int bookId, cdx, noOfChapters;
            String chapterRef;
            ComboBox cbBook, cbChapter;
            classBook currentBook;

            cbBook = globalVars.getComboBoxItem(3);
            cbChapter = globalVars.getComboBoxItem(4);
            bookId = cbBook.SelectedIndex;
            globalVars.BookList.TryGetValue(bookId, out currentBook);
            noOfChapters = currentBook.NoOfChaptersInBook;
            cbChapter.Items.Clear();
            for (cdx = 0; cdx < noOfChapters; cdx++)
            {
                chapterRef = currentBook.getChapterRefBySequence(cdx);
                cbChapter.Items.Add(chapterRef);
            }
            if (cbChapter.Items.Count > 0) cbChapter.SelectedIndex = 0;
            //            if (isChapUpdateActive) displayChapter(bookId, 1);
        }

        public void processSelectedHistory()
        {
            /*================================================================================================*
             *                                                                                                *
             *                                    processSelectedHistory                                      *
             *                                    ======================                                      *
             *                                                                                                *
             *  Called when a book/chapter is changed using the history combo box.                            *
             *                                                                                                *
             *================================================================================================*/

            int idx, bookIdx, nPstn;
            String historyEntry, bookName, chapNo;
            ComboBox cbHistory;
            classBook currentBook;

            cbHistory = (ComboBox)globalVars.getComboBoxItem(7);
            if (cbHistory.Items.Count == 0) return;
            historyEntry = cbHistory.SelectedItem.ToString();
            if (String.Compare(globalVars.LastLXXHistoryEntry, historyEntry) == 0) return;
            nPstn = historyEntry.LastIndexOf(' ');
            if (nPstn < 0) return;
            bookName = historyEntry.Substring(0, nPstn);
            chapNo = historyEntry.Substring(nPstn + 1);
            bookIdx = -1;
            for (idx = 0; idx < globalVars.NoOfLXXBooks; idx++)
            {
                globalVars.BookList.TryGetValue(idx, out currentBook);
                if (String.Compare(currentBook.BookName, bookName) == 0)
                {
                    bookIdx = idx;
                    break;
                }
            }
            if (bookIdx > -1)
            {
                globalVars.LastLXXHistoryEntry = historyEntry;
                displayChapter(bookIdx, chapNo, false);
            }
        }

        private void addEntryToHistory(String displayString)
        {
            ComboBox cbHistory;

            globalVars.IsReady = false;
            cbHistory = globalVars.getComboBoxItem(7);
            if (cbHistory.Items.Contains(displayString)) cbHistory.Items.Remove(displayString);
            cbHistory.Items.Insert(0, displayString);
            if (cbHistory.Items.Count > globalVars.HistoryMax) cbHistory.Items.RemoveAt(cbHistory.Items.Count - 1);
            cbHistory.SelectedIndex = 0;
            globalVars.IsReady = true;
        }

        public void recordMouseEffects(MouseEventArgs e)
        {
            /*========================================================================================================*
             *                                                                                                        *
             *                                          recordMouseEffects                                            *
             *                                          ==================                                            *
             *                                                                                                        *
             *  See registerMouseDown in frmMain.cs for details.                                                      *
             *                                                                                                        *
             *========================================================================================================*/

            int nStart, nPstn = 0, nEnd, verseCharPosition, wordSeq = 0, bookId;
            String currentVerseText, selectedWord, currentRef, subjectText;
            RichTextBox currentRtxt;
            ComboBox cbBook, cbChapter, cbVerse;
            classBook currentBook = null;
            classChapter currentChapter;
            classVerse currentVerse;
            classWord currentWord;

            cbBook = globalVars.getComboBoxItem(3);
            cbChapter = globalVars.getComboBoxItem(4);
            cbVerse = globalVars.getComboBoxItem(5);
            currentRtxt = globalVars.getRichtextItem(1);
            nPstn = currentRtxt.GetCharIndexFromPosition(new Point(e.X, e.Y));
            // This seems to be true when clicked *beyond* a line - find the start of the current line
            subjectText = currentRtxt.Text;
            if (subjectText[nPstn] == '\n') nStart = subjectText.LastIndexOf('\n', nPstn - 1);
            else nStart = subjectText.LastIndexOf('\n', nPstn);
            nStart++;
            // Now the end of the line
            if (subjectText[nPstn] == '\n') nEnd = nPstn;
            else nEnd = subjectText.IndexOf('\n', nPstn);
            if (nEnd == -1) nEnd = subjectText.Length;
            // We can now identify the verse and the line of text associated with it
            currentVerseText = subjectText.Substring(nStart, nEnd - nStart);
            // Now to get the verse number
            nEnd = currentVerseText.IndexOf(':');
            currentRef = currentVerseText.Substring(0, nEnd);
            // Update cbVerse
            cbVerse.SelectedItem = currentRef;
            // What about the specific word?
            // Let's adjust the nPstn so that it relates to the single line of text, currentVerseText
            verseCharPosition = nPstn - nStart;
            if (verseCharPosition > currentVerseText.Length - 1) verseCharPosition = currentVerseText.Length - 1;
            if (currentVerseText[verseCharPosition] == zeroWidthSpace) nStart = verseCharPosition;
            else nStart = currentVerseText.LastIndexOf(zeroWidthSpace, verseCharPosition);
            if (currentVerseText[verseCharPosition] == '\n') nEnd = verseCharPosition - 1;
            else nEnd = currentVerseText.IndexOf(zeroWidthSpace, verseCharPosition);
            if (nEnd == -1) nEnd = currentVerseText.Length - 1;
            // Count the words up to the current position
            nPstn = 0;
            nPstn = currentVerseText.IndexOf(zeroWidthSpace, nPstn);
            while (nPstn > -1)
            {
                if (nPstn > verseCharPosition)
                {
                    // The previous word is the word in which we clicked
                    break;
                }
                // So, at the *start* of the first word, wordSeq = 1, at the *start* of the 2nd word, it = 2, and so on
                wordSeq++;
                nPstn = currentVerseText.IndexOf(zeroWidthSpace, ++nPstn);
            }
            // But wordSeq would be too great:
            //   If we clicked on the verse no, wordSeq would = 0;
            //   If we clicked on the first word, wordSeq would = 1, etc.  Sp ...
            wordSeq--;
            selectedWord = "";
            if (nStart > -1)
            {
                selectedWord = currentVerseText.Substring(nStart, nEnd - nStart).Trim();
                if (selectedWord.Contains(zeroWidthNonJoiner.ToString()))
                {
                    nEnd = selectedWord.IndexOf(zeroWidthNonJoiner);
                    selectedWord = selectedWord.Substring(0, nEnd).Trim();
                }
            }
            bookId = cbBook.SelectedIndex;
            if (globalVars.BookList.ContainsKey(bookId)) globalVars.BookList.TryGetValue(bookId, out currentBook);
            currentChapter = currentBook.getChapterByChapterRef(cbChapter.SelectedItem.ToString());
            currentVerse = currentChapter.getVerseByVerseRef(cbVerse.SelectedItem.ToString());
            currentWord = currentVerse.getWordBySeqNo(wordSeq);
            globalVars.LastSelectedLXXVerse = currentVerseText;
            globalVars.LastSelectedLXXWord = selectedWord;
            globalVars.SelectedLXXWordSequence = wordSeq;
            globalVars.CurrentlySelectedLXXWord = currentWord;
        }

        public void Analysis()
        {
            int bookId;
            String ChapNo, VerseNo, rootWord, parseString;
            Char[] parseSplit = { '.' };
            ComboBox cbBook, cbChapter, cbVerse;
            RichTextBox rtxtParse;
            classBook currentBook = null;
            classChapter currentChapter;
            classVerse currentVerse;
            classWord currentWord;
            Font largeFont, mainFont;
            Color textColour, headerColour, backgroundColour;

            largeFont = globalVars.configureFont(globalVars.getDefinedFontNameByIndex(2, 2), globalVars.getDefinedStyleByIndex(2, 2), globalVars.getTextSize(2, 2));
            mainFont = globalVars.configureFont(globalVars.getDefinedFontNameByIndex(2, 1), globalVars.getDefinedStyleByIndex(2, 1), globalVars.getTextSize(2, 1));
            backgroundColour = globalVars.getColourSetting(2, 0);
            // First, find the word that has been selected
            if (globalVars.LastSelectedLXXWord.Length == 0)
            {
                MessageBox.Show("You need to actively select a word before this action", "Analyse Word", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            cbBook = globalVars.getComboBoxItem(3);
            cbChapter = globalVars.getComboBoxItem(4);
            cbVerse = globalVars.getComboBoxItem(5);
            bookId = cbBook.SelectedIndex;
            ChapNo = cbChapter.SelectedItem.ToString();
            VerseNo = cbVerse.SelectedItem.ToString();
            if (globalVars.BookList.ContainsKey(bookId)) globalVars.BookList.TryGetValue(bookId, out currentBook);
            currentChapter = currentBook.getChapterByChapterRef(ChapNo);
            currentVerse = currentChapter.getVerseByVerseRef(VerseNo);
            currentWord = currentVerse.getWordBySeqNo(globalVars.SelectedLXXWordSequence);
            // Now we have two tasks.  Intially, get and present the parse details
            rtxtParse = globalVars.getRichtextItem(2);
            rtxtParse.Clear();
            rtxtParse.BackColor = backgroundColour;
            textColour = globalVars.getColourSetting(2, 1);
            headerColour = globalVars.getColourSetting(2, 2);
            rtxtParse.SelectionFont = mainFont;
            rtxtParse.SelectionColor = headerColour;
            rtxtParse.SelectedText = "\t\t";
            rtxtParse.SelectionFont = largeFont;
            rtxtParse.SelectionColor = headerColour;
            rtxtParse.SelectedText = currentWord.TextWord;
            rtxtParse.SelectionFont = mainFont;
            rtxtParse.SelectionColor = textColour;
            rtxtParse.SelectedText = "\n\n";
            rootWord = currentWord.RootWord;
            parseString = lexicon.parseLXXGrammar(currentWord.CatString, currentWord.ParseString);
            if (parseString.Length > 0)
            {
                rtxtParse.SelectionFont = mainFont;
                rtxtParse.SelectionColor = textColour;
                rtxtParse.SelectedText = parseString;
                rtxtParse.SelectedText = " - root: " + rootWord;
            }
            lexicon.getLexiconEntry(rootWord);
            // finally, make sure the parse page is visible
            globalVars.getTabControl(1).SelectedIndex = 0;
        }
    }
}
