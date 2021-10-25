using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    public class classText
    {
        /****************************************************************************************
         *                                                                                      *
         *                                      classText                                       *
         *                                      =========                                       *
         *                                                                                      *
         *  Point of entry to storage of text and methods for handling the text.                *
         *                                                                                      *
         ****************************************************************************************/

        bool isChapUpdateActive = true;
        int noOfLxxBooks = 0, noOfNTBooks = 0, ntBookProgressCount = 0, lxxBookProgressCount = 0;
        String lastNTHistoryEntry = "", lastLxxHistoryEntry = "";
        SortedDictionary<int, classBookContent> listOfNTBooks = new SortedDictionary<int, classBookContent>();
        SortedDictionary<int, classBookContent> listOfLxxBooks = new SortedDictionary<int, classBookContent>();
        Object ntBookAcquisitionLock = new object(), lxxBookAcquisitionLock = new object();
        Thread[] threads;

        classGlobal globalVars;
        GreekProcessing greekUtilities;
        classLexicon mainLexicon;

        public int NoOfLxxBooks { get => noOfLxxBooks; }
        public int NoOfNTBooks { get => noOfNTBooks; set => noOfNTBooks = value; }
        public SortedDictionary<int, classBookContent> ListOfNTBooks { get => listOfNTBooks; set => listOfNTBooks = value; }
        public SortedDictionary<int, classBookContent> ListOfLxxBooks { get => listOfLxxBooks; set => listOfLxxBooks = value; }
        public string LastNTHistoryEntry { get => lastNTHistoryEntry; set => lastNTHistoryEntry = value; }
        public string LastLxxHistoryEntry { get => lastLxxHistoryEntry; set => lastLxxHistoryEntry = value; }

        public void initialiseText(classGlobal inConfig, GreekProcessing utilityClass, classLexicon inLexicon)
        {
            globalVars = inConfig;
            greekUtilities = utilityClass;
            mainLexicon = inLexicon;
        }

        public void storeAllText()
        {
            /*-------------------------------------------------------------------------------------------------*
             *                                                                                                 *
             *                                           storeAllText                                          *
             *                                           ============                                          *
             *                                                                                                 *
             *  Handles the initial transfer of NT and LXX text from files to memory (i.e. the class 
             *    hierarchy).                                                                                  *
             *                                                                                                 *
             *  Indexing:                                                                                      *
             *  --------                                                                                       *
             *  Books will be indexed from 0 onwards.  (The file book code starts at 1, so be careful when     *
             *     handling data from the files.)                                                              *
             *  Chapters and Verses will be indexed sequentially on a zero based count also but the "real"     *
             *     chapter and verse number will also be retained and key accesses will be provided by both    *
             *     sequence number and chapter/verse number.  (This allows simple access - by sequence - and   *
             *     real world access - by chapter/verse = even where the chapters or [more likely] verses are  *
             *     *not* sequential - i.e. one or more are omitted.)                                           *
             *                                                                                                 *
             *-------------------------------------------------------------------------------------------------*/

            int bookCount = 0, idx, noOfCores;
            /*................*
             *  File handling *
             *................*/
            String fullFileName, fileBuffer;
            /*.........................*
             *  Manipulating the Text  *
             *.........................*/
            String[] bookDetails = new String[7];
            Char[] splitParams = { '\t' };
            Char[] possibleCaps = { 'Α', 'Ἀ', 'Ἁ', 'Ἂ', 'Ἃ', 'Ἄ', 'Ἅ', 'Β', 'Γ', 'Δ', 'Ε', 'Ἐ', 'Ἑ', 'Ἒ', 'Ἓ', 'Ἔ', 'Ἕ', 'Ζ', 'Η', 'Ἠ', 'Ἡ', 'Ἢ', 'Ἣ', 'Ἤ', 'Ἥ', 'Θ',
                                  'Ι', 'Ἰ', 'Ἱ', 'Ἲ', 'Ἳ', 'Ἴ', 'Ἵ', 'Κ', 'Λ', 'Μ', 'Ν', 'Ξ', 'Ο', 'Ὀ', 'Ὁ', 'Ὂ', 'Ὃ', 'Ὄ', 'Ὅ', 'Π', 'Ρ', 'Σ', 'Τ',
                                  'Υ', 'Ὑ', 'Ὓ', 'Ὕ', 'Φ', 'Χ', 'Ψ', 'Ω', 'Ὠ', 'Ὡ', 'Ὢ', 'Ὣ', 'Ὤ', 'Ὥ' };
            Char[] replacementMins = { 'α', 'ἀ', 'ἁ', 'ἂ', 'ἃ', 'ἄ', 'ἅ', 'β', 'γ', 'δ', 'ε', 'ἐ', 'ἑ', 'ἒ', 'ἓ', 'ἔ', 'ἕ', 'ζ', 'η', 'ἠ', 'ἡ', 'ἢ', 'ἣ', 'ἤ', 'ἥ', 'θ',
                                     'ι', 'ἰ', 'ἱ', 'ἲ', 'ἳ', 'ἴ', 'ἵ', 'κ', 'λ', 'μ', 'ν', 'ξ', 'ο', 'ὀ', 'ὁ', 'ὂ', 'ὃ', 'ὄ', 'ὅ', 'π', 'ρ', 'σ', 'τ',
                                     'υ', 'ὑ', 'ὓ', 'ὕ', 'φ', 'χ', 'ψ', 'ω', 'ὠ', 'ὡ', 'ὢ', 'ὣ', 'ὤ', 'ὥ' };

            SortedSet<String> parseList = new SortedSet<string>();

            noOfCores = System.Environment.ProcessorCount - 1;  // One less, just to make sure we're not swamped
            threads = new Thread[noOfCores];

            /*********************************************************************************
             * 
             *   Step 1: Get NT details from the file: 
             * 
             *********************************************************************************/

            {
                StreamReader srTitles;
                classBookContent currentBook;

                currentBook = null;
                fullFileName = globalVars.BaseDirectory + @"\" + globalVars.SourceFolder + @"\" + globalVars.NtTitlesFile;
                srTitles = new StreamReader(fullFileName);
                fileBuffer = srTitles.ReadLine();
                while (fileBuffer != null)
                {
                    bookDetails = fileBuffer.Split(splitParams);
                    currentBook = new classBookContent();
                    currentBook.BookId = bookCount;
                    ListOfNTBooks.Add(bookCount++, currentBook);
                    currentBook.IsNT = true;
                    currentBook.BookName = bookDetails[0];
                    currentBook.ShortName = bookDetails[1];
                    currentBook.FileName = bookDetails[2];
                    currentBook.IsNT = true;
                    fileBuffer = srTitles.ReadLine();
                }
                noOfNTBooks = bookCount;
                srTitles.Close();
                srTitles.Dispose();
            }

            /*********************************************************************************
             * 
             *   Step 2: Now recurse through the NT files to load all text
             * 
             *   File Structure:
             *   --------------
             *     Field          Significance
             *        1     Chapter (there is no book code; it can be generated at run-time)
             *        2     Verse
             *        3     Parse Code 1
             *        4     Parse code 2
             *        3     Abbreviated part of speech Id
             *        4     Blank field
             *        5     Unique code id (when combined with field 3)
             *        6     Word, as used in the text
             *        7     Word used with accents and iota subscripts removed
             *        8     Word used, as field 7, but breathings and diereses also removed
             *        9     Root word
             *       10     Pre-word non-Greek characters (if any)
             *       11     Post-word non-Greek non-punctuation characters (if any)
             *       12     Final punctuation (if any)
             *       
             *********************************************************************************/

            {
                bool isAllDone = false;
                int gotToCount = -1;

                for (idx = 0; idx < noOfCores; idx++)
                {
                    threads[idx] = new Thread(new ThreadStart(getNTData));
                    threads[idx].IsBackground = true;
                    threads[idx].Start();
                }
                while( ! isAllDone )
                {
                    for (idx = 0; idx < noOfCores; idx++)
                    {
                        if (threads[idx].IsAlive) break;
                        gotToCount = idx;
                    }
                    if (gotToCount >= noOfCores - 1) isAllDone = true;
                }
            }

            /*********************************************************************************
             * 
             *   Step 3: Get LXX details from the file: 
             * 
             *********************************************************************************/

            {
                StreamReader srTitles;
                classBookContent currentBook;

                currentBook = null;
                bookCount = 0;
                fullFileName = globalVars.BaseDirectory + @"\" + globalVars.SourceFolder + @"\" + globalVars.LxxTitlesFile;
                srTitles = new StreamReader(fullFileName);
                fileBuffer = srTitles.ReadLine();
                while (fileBuffer != null)
                {
                    bookDetails = fileBuffer.Split(splitParams);
                    currentBook = new classBookContent();
                    ListOfLxxBooks.Add(bookCount, currentBook);
                    currentBook.BookName = bookDetails[1];
                    currentBook.ShortName = bookDetails[2];
                    currentBook.FileName = bookDetails[3];
                    currentBook.IsNT = false;
                    currentBook.BookId = bookCount++;
                    fileBuffer = srTitles.ReadLine();
                }
                noOfLxxBooks = bookCount;
                srTitles.Close();
                srTitles.Dispose();
            }

            /*********************************************************************************
             * 
             *   Step 4: Now recurse through the LXX files to load all text
             * 
             *********************************************************************************/

            {
                bool isAllDone = false;
                int gotToCount = -1;

                for (idx = 0; idx < noOfCores; idx++)
                {
                    threads[idx] = new Thread(new ThreadStart(getLxxData));
                    threads[idx].IsBackground = true;
                    threads[idx].Start();
                }
                while (!isAllDone)
                {
                    for (idx = 0; idx < noOfCores; idx++)
                    {
                        if (threads[idx].IsAlive) break;
                        gotToCount = idx;
                    }
                    if (gotToCount >= noOfCores - 1) isAllDone = true;
                }
            }

            /*********************************************************************************
             * 
             *   Step 5: Finally, populate the combo box
             * 
             *********************************************************************************/

            populateBookCBList();
        }

        private void getNTData()
        {
            /*********************************************************************************************
             *                                                                                           *
             *                                          getNTData                                        *
             *                                          =========                                        *
             *                                                                                           *
             *  Each thread will call this to process the contents of one book at a time.  It is to be   *
             *    hoped that multithreading will speed up the load process on all but old machines.      *
             *                                                                                           *
             *   File Structure:                                                                         *
             *   --------------                                                                          *
             *     Field          Significance                                                           *
             *        1     Chapter (there is no book code; it can be generated at run-time)             *
             *        2     Verse                                                                        *
             *        3     Parse Code 1                                                                 *
             *        4     Parse code 2                                                                 *
             *        3     Abbreviated part of speech Id                                                *
             *        4     Blank field                                                                  *
             *        5     Unique code id (when combined with field 3)                                  *
             *        6     Word, as used in the text                                                    *
             *        7     Word used with accents and iota subscripts removed                           *
             *        8     Word used, as field 7, but breathings and diereses also removed              *
             *        9     Root word                                                                    *
             *       10     Pre-word non-Greek characters (if any)                                       *
             *       11     Post-word non-Greek non-punctuation characters (if any)                      *
             *       12     Final punctuation (if any)                                                   *
             *                                                                                           *
             *********************************************************************************************/
            int bookIndex = 0;
            String verseId, prevVerseId, chapterId, prevChapterId, fullFileName, fileBuffer;
            //            Object bookAcquisitionLock = new object();
            String[] bookDetails = new String[7];
            Char[] splitParams = { '\t' };
            StreamReader srBooks;
            classBookContent currentBook;
            classChapterContent currentChapter, prevChapter;
            classVerseContent currentVerse, previousVerse;
            classWordContent currentWord;

            while (bookIndex < noOfNTBooks)
            {
                currentChapter = null;
                currentVerse = null;
                lock (ntBookAcquisitionLock)
                {
                    bookIndex = ntBookProgressCount++;
                }
                if (bookIndex >= noOfNTBooks) return;
                ListOfNTBooks.TryGetValue(bookIndex, out currentBook);
                if (currentBook == null) return;
                fullFileName = globalVars.BaseDirectory + @"\" + globalVars.SourceFolder + @"\" + globalVars.NtTextFolder + @"\" + currentBook.FileName;
                srBooks = new StreamReader(fullFileName);
                fileBuffer = srBooks.ReadLine();
                prevChapterId = "";
                prevVerseId = "x";
                previousVerse = null;
                while (fileBuffer != null)
                {
                    bookDetails = fileBuffer.Split(splitParams);
                    // Handle the chapter
                    chapterId = bookDetails[0];
                    if (String.Compare(chapterId, prevChapterId) != 0)
                    {
                        prevChapter = currentChapter;
                        currentChapter = currentBook.addNewChapterToBook(chapterId);
                        if (prevChapter != null) prevChapter.NextChapter = currentChapter;
                        currentChapter.PreviousChapter = prevChapter;
                        currentChapter.BookId = bookIndex;
                        currentChapter.ChapterId = chapterId;
                        prevChapterId = chapterId;
                        prevVerseId = "x";
                    }
                    // Handle the verse
                    verseId = bookDetails[1];
                    if (String.Compare(verseId, prevVerseId) != 0)
                    {
                        currentVerse = currentChapter.addVerseToChapter(verseId);
                        if (previousVerse != null)
                        {
                            previousVerse.NextVerse = currentVerse;
                        }
                        currentVerse.PreviousVerse = previousVerse;
                        currentVerse.BookId = bookIndex;
                        prevVerseId = verseId;
                        previousVerse = currentVerse;
                        currentVerse.setBibleReference(currentBook.ShortName, chapterId, verseId);
                    }
                    currentWord = currentVerse.addWordToVerse();
                    currentWord.CatString = bookDetails[2];
                    currentWord.ParseString = bookDetails[3];
                    currentWord.UniqueValue = bookDetails[4];
                    currentWord.TextWord = bookDetails[5];
                    currentWord.AccentlessTextWord = bookDetails[6];
                    currentWord.BareTextWord = bookDetails[7];
                    currentWord.RootWord = bookDetails[8];
                    currentWord.Punctuation = bookDetails[11];
                    currentWord.PreWordChars = bookDetails[9];
                    currentWord.PostWordChars = bookDetails[10];
                    fileBuffer = srBooks.ReadLine();
                }
                srBooks.Close();
            }
        }

        private void getLxxData()
        {

            /*********************************************************************************************
             *                                                                                           *
             *                                          getLxxData                                       *
             *                                          ==========                                       *
             *                                                                                           *
             *  See getNTData for comments                                                               *
             *                                                                                           *
             *  The File Structure is the same as for NT source files (see getNTData).                   *
             *                                                                                           *
             *********************************************************************************************/
            int bookIndex = 0;
            String verseId, prevVerseId, chapterId, prevChapterId, fullFileName, fileBuffer;
            String[] bookDetails = new String[7];
            Char[] splitParams = { '\t' };
            StreamReader srBooks;
            classBookContent currentBook;
            classChapterContent currentChapter, prevChapter;
            classVerseContent currentVerse, previousVerse;
            classWordContent currentWord;

            while (bookIndex < noOfLxxBooks)
            {
                currentChapter = null;
                currentVerse = null;
                lock (ntBookAcquisitionLock)
                {
                    bookIndex = lxxBookProgressCount++;
                }
                if (bookIndex >= noOfLxxBooks) return;
                ListOfLxxBooks.TryGetValue(bookIndex, out currentBook);
                if (currentBook == null) continue;
                fullFileName = globalVars.BaseDirectory + @"\" + globalVars.SourceFolder + @"\" + globalVars.LxxTextFolder + @"\" + currentBook.FileName;
                srBooks = new StreamReader(fullFileName);
                fileBuffer = srBooks.ReadLine();
                prevChapterId = "";
                prevVerseId = "x";
                previousVerse = null;
                while (fileBuffer != null)
                {
                    bookDetails = fileBuffer.Split(splitParams);
                    // Handle the chapter
                    chapterId = bookDetails[0];
                    if (String.Compare(chapterId, prevChapterId) != 0)
                    {
                        prevChapter = currentChapter;
                        currentChapter = currentBook.addNewChapterToBook(chapterId);
                        if (prevChapter != null) prevChapter.NextChapter = currentChapter;
                        currentChapter.PreviousChapter = prevChapter;
                        currentChapter.BookId = bookIndex;
                        currentChapter.ChapterId = chapterId;
                        prevChapterId = chapterId;
                        prevVerseId = "x";
                    }
                    // Handle the verse
                    verseId = bookDetails[1];
                    if (String.Compare(verseId, prevVerseId) != 0)
                    {
                        currentVerse = currentChapter.addVerseToChapter(verseId);
                        if (previousVerse != null)
                        {
                            previousVerse.NextVerse = currentVerse;
                        }
                        currentVerse.PreviousVerse = previousVerse;
                        prevVerseId = verseId;
                        previousVerse = currentVerse;
                        currentVerse.setBibleReference(currentBook.ShortName, chapterId, verseId);
                    }
                    currentWord = currentVerse.addWordToVerse();
                    currentWord.CatString = bookDetails[2];
                    currentWord.ParseString = bookDetails[3];
                    currentWord.UniqueValue = bookDetails[4];
                    currentWord.TextWord = bookDetails[5];
                    currentWord.AccentlessTextWord = bookDetails[6];
                    currentWord.BareTextWord = bookDetails[7];
                    currentWord.RootWord = bookDetails[8];
                    currentWord.Punctuation = bookDetails[11];
                    currentWord.PreWordChars = bookDetails[9];
                    currentWord.PostWordChars = bookDetails[10];
                    fileBuffer = srBooks.ReadLine();
                }
                srBooks.Close();
            }
        }

    public void populateBookCBList()
        {
            int idx;
            classBookContent currentBook;
            ComboBox targetCBox;

            targetCBox = globalVars.getComboBoxControlByIndex(0);
            if (targetCBox == null) return;
            targetCBox.Items.Clear();
            for (idx = 0; idx < noOfNTBooks; idx++)
            {
                ListOfNTBooks.TryGetValue(idx, out currentBook);
                if (currentBook == null) continue;
                targetCBox.Items.Add(currentBook.BookName);
            }
            if (targetCBox.Items.Count > 0) targetCBox.SelectedIndex = 0;

            targetCBox = globalVars.getComboBoxControlByIndex(3);
            if (targetCBox == null) return;
            targetCBox.Items.Clear();
            for (idx = 0; idx < noOfLxxBooks; idx++)
            {
                ListOfLxxBooks.TryGetValue(idx, out currentBook);
                if (currentBook == null) continue;
                targetCBox.Items.Add(currentBook.BookName);
            }
            if (targetCBox.Items.Count > 0) targetCBox.SelectedIndex = 0;
        }

        public void displayNTChapter(int bookIdx, String chapIdx, bool isNewBook)
        {
            /********************************************************************
             *                                                                  *
             *                        displayNTChapter                          *
             *                        ==============                            *
             *                                                                  *
             *  Controls the display of a specified chapter                     *
             *                                                                  *
             *  Parameters:                                                     *
             *    bookIdx: the book index (a zero based index)                  *
             *    chapIdx: chapter number (real chapter, which must be          *
             *             converted to it's equivalent sequence number         *
             *                                                                  *
             ********************************************************************/

            int idx, noOfVerses, noOfChapters, chapNo;
            String newBookName, displayString, realVNo;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            RichTextBox targetTextArea;
            ComboBox cbChapter, cbVerse;
            ComboBox cbBook;

            targetTextArea = globalVars.getRichtextControlByIndex(0);
            cbBook = globalVars.getComboBoxControlByIndex(0);
            cbChapter = globalVars.getComboBoxControlByIndex(1);
            cbVerse = globalVars.getComboBoxControlByIndex(2);
            if ((cbBook == null) || (cbChapter == null) || (cbVerse == null) || (targetTextArea == null)) return;
            // Get the name of the new book
            ListOfNTBooks.TryGetValue(bookIdx, out currentBook);
            newBookName = currentBook.BookName;
            // No, so change the book reference

            if (cbBook.Items.Count == 0) return;
            cbBook.SelectedIndex = bookIdx;
            // Recreate cbChapters if it is a new book
            if (isNewBook)
            {
                // Inhibit duplicate updates
                isChapUpdateActive = false;
                noOfChapters = currentBook.NoOfChaptersInBook;
                if (noOfChapters == 0) return;
                cbChapter.Items.Clear();
                for (idx = 0; idx < noOfChapters; idx++)
                {
                    cbChapter.Items.Add(currentBook.getChapterIdBySeqNo(idx).ToString());
                }
                // The real chapter must be submitted, so make sure we select the *sequence*
                if (cbChapter.Items.Count > 0)
                {
                    chapNo = currentBook.getSeqNoByChapterId(chapIdx);
                    cbChapter.SelectedIndex = chapNo;
                }
                isChapUpdateActive = true;
            }
            // Get the specified chapter from the current book -
            //   which will be the new book, if it has changed
            currentChapter = currentBook.getChapterByChapterId(chapIdx);
            // Now modify the verse combo box *and* display the new chapter
            noOfVerses = currentChapter.NoOfVersesInChapter;
            targetTextArea.Text = "";
            cbVerse.Items.Clear();
            for (idx = 0; idx < noOfVerses; idx++)
            {
                currentVerse = currentChapter.getVerseBySeqNo(idx);
                if (currentVerse == null) continue;
                realVNo = currentChapter.getVerseIdBySeqNo(idx);
                cbVerse.Items.Add(realVNo.ToString());
                if (idx > 0)
                {
                    targetTextArea.AppendText("\n");
                }
                targetTextArea.AppendText(realVNo.ToString() + ":");
                targetTextArea.AppendText(" " + currentVerse.getWholeText(true));
            }
            cbVerse.SelectedIndex = 0;
            displayString = newBookName + " " + chapIdx;
            globalVars.NtTabPge.Text = "New Testament - " + displayString;
            if (globalVars.TabCtlText.SelectedIndex == 0) globalVars.MasterForm.Text = "Greek Bible Student - " + displayString;
            addEntryToHistory(displayString, 0, 0);
        }

        public void displayLXXChapter(int bookIdx, String chapIdx, bool isNewBook)
        {
            /********************************************************************
             *                                                                  *
             *                        displayLXXChapter                         *
             *                        =================                         *
             *                                                                  *
             *  Controls the display of a specified chapter                     *
             *                                                                  *
             *  Parameters:                                                     *
             *    bookIdx: the book index (a zero based index)                  *
             *    chapIdx: chapter number (real chapter, which must be          *
             *             converted to it's equivalent sequence number         *
             *                                                                  *
             ********************************************************************/

            int idx, noOfVerses, noOfChapters, chapNo;
            String newBookName, displayString, realVNo;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            RichTextBox targetTextArea;
            ComboBox cbChapter, cbVerse;
            ComboBox cbBook;

            targetTextArea = globalVars.getRichtextControlByIndex(1);
            cbBook = globalVars.getComboBoxControlByIndex(3);
            cbChapter = globalVars.getComboBoxControlByIndex(4);
            cbVerse = globalVars.getComboBoxControlByIndex(5);
            if ((cbBook == null) || (cbChapter == null) || (cbVerse == null) || (targetTextArea == null)) return;
            // Get the name of the new book
            listOfLxxBooks.TryGetValue(bookIdx, out currentBook);
            newBookName = currentBook.BookName;
            // No, so change the book reference

            if (cbBook.Items.Count == 0) return;
            cbBook.SelectedIndex = bookIdx;
            // Recreate cbChapters if it is a new book
            if (isNewBook)
            {
                // Inhibit duplicate updates
                isChapUpdateActive = false;
                noOfChapters = currentBook.NoOfChaptersInBook;
                if (noOfChapters == 0) return;
                cbChapter.Items.Clear();
                for (idx = 0; idx < noOfChapters; idx++)
                {
                    cbChapter.Items.Add(currentBook.getChapterIdBySeqNo(idx).ToString());
                }
                // The real chapter must be submitted, so make sure we select the *sequence*
                if (cbChapter.Items.Count > 0)
                {
                    chapNo = currentBook.getSeqNoByChapterId(chapIdx);
                    cbChapter.SelectedIndex = chapNo;
                }
                isChapUpdateActive = true;
            }
            // Get the specified chapter from the current book -
            //   which will be the new book, if it has changed
            currentChapter = currentBook.getChapterByChapterId(chapIdx);
            // Now modify the verse combo box *and* display the new chapter
            noOfVerses = currentChapter.NoOfVersesInChapter;
            targetTextArea.Text = "";
            cbVerse.Items.Clear();
            // Temporary
            String copyOfText = "";
            for (idx = 0; idx < noOfVerses; idx++)
            {
                currentVerse = currentChapter.getVerseBySeqNo(idx);
                if (currentVerse == null) continue;
                realVNo = currentChapter.getVerseIdBySeqNo(idx);
                cbVerse.Items.Add(realVNo.ToString());
                if (idx > 0)
                {
                    targetTextArea.AppendText("\n");
                    copyOfText += "\n";
                }
                targetTextArea.AppendText(realVNo.ToString() + ":");
                copyOfText += realVNo.ToString() + ":";
                targetTextArea.AppendText(" " + currentVerse.getWholeText(false));
                copyOfText += " " + currentVerse.getWholeText(false);
            }
            cbVerse.SelectedIndex = 0;
            displayString = newBookName + " " + chapIdx;
            globalVars.LxxTabPge.Text = "Septuagint (Old Testament) - " + displayString;
            if (globalVars.TabCtlText.SelectedIndex == 1) globalVars.MasterForm.Text = "Greek Bible Student - " + displayString;
            addEntryToHistory(displayString, 1, 0);
        }

        public void handleChangeOfNTBook()
        {
            /*---------------------------------------------------------------------------------------*
             *  This will be triggered by a change of selection in the NT Books combobox, so the     *
             *    bookIdx will be a genuine index but we want to use the *real* chapter number       *
             *---------------------------------------------------------------------------------------*/
            int bookIdx;
            String chapterNo;
            ComboBox bookCBox;
            classBookContent currentBook;

            bookCBox = globalVars.getComboBoxControlByIndex(0);
            if (bookCBox == null) return;
            bookIdx = bookCBox.SelectedIndex;
            if (bookIdx < 0) return;
            listOfNTBooks.TryGetValue(bookIdx, out currentBook);
            chapterNo = currentBook.getChapterIdBySeqNo(0);
            displayNTChapter(bookIdx, chapterNo, true);
        }

        public void handleChangeOfLxxBook()
        {
            /*---------------------------------------------------------------------------------------*
             *  This will be triggered by a change of selection in the LXX Books combobox, so the    *
             *    bookIdx will be a genuine index but we want to use the *real* chapter number       *
             *---------------------------------------------------------------------------------------*/
            int bookIdx;
            String chapterNo;
            ComboBox bookCBox;
            classBookContent currentBook;

            bookCBox = globalVars.getComboBoxControlByIndex(3);
            if (bookCBox == null) return;
            bookIdx = bookCBox.SelectedIndex;
            if (bookIdx < 0) return;
            listOfLxxBooks.TryGetValue(bookIdx, out currentBook);
            chapterNo = currentBook.getChapterIdBySeqNo(0);
            displayLXXChapter(bookIdx, chapterNo, true);
        }

        public void handleChangeOfNTChapter()
        {
            /*---------------------------------------------------------------------------------------*
             *  This will be triggered by a change of selection in the Chapters combobox, so the     *
             *    chapter number (taken from the combo box) must be assumed to be the actual chapter *
             *    number.                                                                            *
             *---------------------------------------------------------------------------------------*/
            int bookIdx;
            String chapterNo;
            ComboBox chapterCBox;
            ComboBox bookCBox;
            classBookContent currentBook;

            if (!isChapUpdateActive) return;
            bookCBox = globalVars.getComboBoxControlByIndex(0);
            chapterCBox = globalVars.getComboBoxControlByIndex(1);
            if ((bookCBox == null) || (chapterCBox == null)) return;
            bookIdx = bookCBox.SelectedIndex;
            if (bookIdx < 0) return;
            listOfNTBooks.TryGetValue(bookIdx, out currentBook);
            // Let's get the real chapter number from the combo box
            chapterNo = chapterCBox.SelectedItem.ToString();
            displayNTChapter(bookIdx, chapterNo, false);
            //            addEntryToHistory(currentBook.BookName + " " + chapterNo);
        }

        public void handleChangeOfLXXChapter()
        {
            /*---------------------------------------------------------------------------------------*
             *  This will be triggered by a change of selection in the Chapters combobox, so the     *
             *    chapter number (taken from the combo box) must be assumed to be the actual chapter *
             *    number.                                                                            *
             *---------------------------------------------------------------------------------------*/
            int bookIdx;
            String chapterNo;
            ComboBox chapterCBox;
            ComboBox bookCBox;
            classBookContent currentBook;

            if (!isChapUpdateActive) return;
            bookCBox = globalVars.getComboBoxControlByIndex(3);
            chapterCBox = globalVars.getComboBoxControlByIndex(4);
            if ((bookCBox == null) || (chapterCBox == null)) return;
            bookIdx = bookCBox.SelectedIndex;
            if (bookIdx < 0) return;
            listOfLxxBooks.TryGetValue(bookIdx, out currentBook);
            // Let's get the real chapter number from the combo box
            chapterNo = chapterCBox.SelectedItem.ToString();
            displayLXXChapter(bookIdx, chapterNo, false);
            //            addEntryToHistory(currentBook.BookName + " " + chapterNo);
        }

        public classBookContent getNTBookByIndex(int index)
        {
            classBookContent tempBook;

            if (!listOfNTBooks.ContainsKey(index)) return null;
            listOfNTBooks.TryGetValue(index, out tempBook);
            return tempBook;
        }

        public classBookContent getLXXBookByIndex(int index)
        {
            classBookContent tempBook;

            if (!listOfLxxBooks.ContainsKey(index)) return null;
            listOfLxxBooks.TryGetValue(index, out tempBook);
            return tempBook;
        }

        public String getSelectedWord(bool isNT)
        {
            /*****************************************************
             *                                                   *
             *                  getSelectedWord                  *
             *                  ===============                  *
             *                                                   *
             *  Returns the word from the main text area (either *
             *    NT or LXX) that is currently under the mouse   *
             *    click.                                         *
             *                                                   *
             *****************************************************/
            int nPstn, nStart, nEnd;
            Char[] setOfDeliniators = { '\n', ' ' };
            RichTextBox mainText;

            if (isNT) mainText = globalVars.getRichtextControlByIndex(0);
            else mainText = globalVars.getRichtextControlByIndex(1);

            // Where are we now?
            //            nPstn = mainText.SelectionStart;
            nPstn = globalVars.LatestMousePosition;
            if (nPstn > mainText.Text.Length - 1) nPstn = mainText.Text.Length - 1;
            if (mainText.Text[nPstn] == ' ') nPstn--;
            if (mainText.Text[nPstn] == '\n') nPstn--;
            // Now find the potential start of the word
            nStart = mainText.Text.LastIndexOfAny(setOfDeliniators, nPstn);
            if (nStart == -1)
            {
                nStart = 0;
            }
            else
            {
                nStart++;
            }
            // And also the potential end of the word
            nEnd = mainText.Text.IndexOfAny(setOfDeliniators, nPstn);
            if (nEnd == -1) nEnd = mainText.Text.Length - 1;
            if (nEnd < mainText.Text.Length - 1)
            {
                nEnd--;
            }
            // Store the word
            while (checkIsNonGreekCharacter(mainText.Text, nStart))
            {
                nStart++;
                if (nStart == nPstn) break;
            }
            while (checkIsNonGreekCharacter(mainText.Text, nEnd))
            {
                nEnd--;
                if (nEnd == nPstn) break;
            }
            return mainText.Text.Substring(nStart, nEnd - nStart + 1);
        }

        private bool checkIsNonGreekCharacter(String sourceText, int csrPstn)
        {
            Char firstOrLastChar;

            firstOrLastChar = sourceText[csrPstn];
            if ((firstOrLastChar >= 0x0374) && (firstOrLastChar <= 0x03dd)) return false;
            if ((firstOrLastChar >= 0x1f00) && (firstOrLastChar <= 0x1fff)) return false;
            return true;
        }

        public void performAnalysis(int code)
        {
            /***********************************************************************************************
             *                                                                                             *
             *  The purpose of this method is to identify the word being clicked on (then provide its      *
             *     meaning)                                                                                *
             *                                                                                             *
             ***********************************************************************************************/
            bool isNT;
            int idx, wordLength, pstnBefore, pstnAfter, currPstn, newPstn, pseudoBookIdx, wordCount, noOfBooks = -1;
            String ourBook, ourChapNo, outputText, rootWord;
            String clickedOnLine, clickedOnWord, selVerse;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            classWordContent currentWord;

            Form masterForm;
            RichTextBox mainText, parseTextArea, lexiconTextArea;
            ComboBox cbVerse, cbChapter, cbBook;
            Tuple<String, String, String, String> cleanResults;

            isNT = code == 0;

            /*----------------------------------------------------------------------------------------------*
             *                                                                                              *
             *    1. Identify the word at the cursor                                                        *
             *                                                                                              *
             *----------------------------------------------------------------------------------------------*/

            /*..............................................................................................*
             *                                                                                              *
             *        a) Isolate and store                                                                  *
             *                                                                                              *
             *..............................................................................................*/

            masterForm = globalVars.MasterForm;
            if (isNT) mainText = globalVars.getRichtextControlByIndex(0);
            else mainText = globalVars.getRichtextControlByIndex(1);
            // This actually tells us where the caret currently is
            currPstn = mainText.SelectionStart;
            // Assume that, if we actually have a selected word, it is a whole word (or more)
            clickedOnWord = mainText.SelectedText.Trim();
            if (clickedOnWord.Length > 0)
            {
                // Check whether it is multiple words
                if ((clickedOnWord.Contains(' ')) || (clickedOnWord.Contains('\n')))
                {
                    MessageBox.Show("You appear to have selected more than one word.\nYou must limit your selection to a single word",
                        "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    if (Convert.ToInt32(clickedOnWord) > 0)
                    {
                        MessageBox.Show("You appear to have selected the verse number.\nYou must select an actual word",
                            "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch
                {
                    // No action.
                }
            }
            else
            {
                // Okay, we don't have a selection but we do have a click
                clickedOnWord = findCurrentWord(mainText.Text, currPstn);
            }
            // Check we actually do have a word
            if (clickedOnWord.Length == 0) return;
            // Check we haven't clicked on the verse number
            if (clickedOnWord[clickedOnWord.Length - 1] == ':')
            {
                clickedOnWord = clickedOnWord.Substring(0, clickedOnWord.Length - 1);
                try
                {
                    if (Convert.ToInt32(clickedOnWord) > 0)
                    {
                        MessageBox.Show("You cannot analyse a verse number", "Analysis Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                catch
                {
                    // No action required: it isn't a number
                }
            }
            // Finally, in this section, tidy up the word from any extraneous characters
            cleanResults = greekUtilities.removeNonGkChars(clickedOnWord);
            clickedOnWord = cleanResults.Item4;

            /*..............................................................................................*
             *                                                                                              *
             *        b. Get the verse number (we need this for interrogating the lexicon file)             *
             *                                                                                              *
             *..............................................................................................*/

            // Get the current line (again!)
            clickedOnLine = isolateCurrentLine(mainText.Text, currPstn, out newPstn);
            // The verse should be the first "word" in this line
            pstnAfter = clickedOnLine.IndexOf(' ');
            if (pstnAfter <= 0)
            {
                // We have a problem
                selVerse = clickedOnLine;
            }
            else
            {
                pstnAfter--;
                selVerse = clickedOnLine.Substring(0, pstnAfter + 1);
            }
            if (selVerse[selVerse.Length - 1] == ':')
            {
                selVerse = selVerse.Substring(0, selVerse.Length - 1);
            }
            // So, selVerse should now provide the verse number

            /*..............................................................................................*
             *                                                                                              *
             *        c. Set the verse number in the verse combo box and get the "word index" value (We'll  *
             *             need this for section 2).                                                        *
             *                                                                                              *
             *..............................................................................................*/

            if (isNT) cbVerse = globalVars.getComboBoxControlByIndex(2);
            else cbVerse = globalVars.getComboBoxControlByIndex(5);
            cbVerse.SelectedItem = selVerse;
            pstnBefore = 0;
            wordCount = 0;
            while (pstnBefore < newPstn)
            {
                pstnAfter = clickedOnLine.IndexOf(' ', pstnBefore);
                if ((pstnAfter > -1) && (pstnAfter < newPstn))
                {
                    wordCount++;
                    // We have to add a wrinkle because the text has *2* spaces after the verse number
                    if (wordCount == 1)
                    {
                        if (clickedOnLine[pstnAfter + 1] == ' ')
                        {
                            if (++pstnAfter == newPstn) break;
                        }
                    }
                }
                else
                {
                    break;
                }
                pstnBefore = pstnAfter + 1;
            }
            wordCount--;

            /*----------------------------------------------------------------------------------------------*
             *                                                                                              *
             *    2. Record the meaning and grammatical details of the selected word                        *
             *                                                                                              *
             *----------------------------------------------------------------------------------------------*/

            // Get the Book name and the currentChapter
            if (isNT) cbBook = globalVars.getComboBoxControlByIndex(0);
            else cbBook = globalVars.getComboBoxControlByIndex(3);
            ourBook = cbBook.SelectedItem.ToString();
            if (isNT) cbChapter = globalVars.getComboBoxControlByIndex(1);
            else cbChapter = globalVars.getComboBoxControlByIndex(4);
            ourChapNo = cbChapter.SelectedItem.ToString();
            // And the book index
            pseudoBookIdx = cbBook.SelectedIndex;
            // To get the parse details, we need to obtain the parse fields from the verse
            if (isNT) ListOfNTBooks.TryGetValue(pseudoBookIdx, out currentBook);
            else ListOfLxxBooks.TryGetValue(pseudoBookIdx, out currentBook);
            currentChapter = currentBook.getChapterByChapterId(ourChapNo);
            currentVerse = currentChapter.getVerseByVerseNo(selVerse);
            currentWord = currentVerse.getWordBySeqNo(wordCount);
            rootWord = currentWord.RootWord;
            outputText = clickedOnWord + ":\n";
            wordLength = clickedOnWord.Length;
            for (idx = 0; idx < wordLength; idx++)
            {
                outputText += "=";
            }
            outputText += "\n\n";
            if (isNT) noOfBooks = noOfNTBooks;
            else noOfBooks = noOfLxxBooks;
            if (code == 0) outputText += mainLexicon.parseGrammar(currentWord.CatString, currentWord.ParseString, true);
            else outputText += mainLexicon.parseGrammar(currentWord.CatString, currentWord.ParseString, false);
            outputText += "\n\nRoot of word: " + rootWord;
            parseTextArea = globalVars.getRichtextControlByIndex(2);
            parseTextArea.Text = outputText;
            outputText = mainLexicon.getLexiconEntry(rootWord);
            lexiconTextArea = globalVars.getRichtextControlByIndex(3);
            lexiconTextArea.Text = outputText;
        }

        private String findCurrentWord(String textStream, int cursorPosition)
        {
            /*****************************************************************************************************
             *                                                                                                   *
             *  Given a specific position in the text, find the word in which the position occurs.               *
             *                                                                                                   *
             *  In practical terms, this translates to the word in which a cursor appears.  As with text in a    *
             *    rich text box, we assume line returns can exist.                                               *
             *                                                                                                   *
             *****************************************************************************************************/

            int ourCursorPosition, pstnBefore, pstnAfter;
            String workingLine;

            // Let's start by isolating the line of text in which the word occurs
            // Because we're using isolateCurrentLine, we can assume the following:
            //  a) The cursor has a value between 0 and textStream.Length - 1, inclusive
            //  b) The string isn't bounded by carriage returns
            workingLine = isolateCurrentLine(textStream, cursorPosition, out ourCursorPosition);
            if (workingLine.Length == 0) return "";
            // Find the start of the word we have clicked on
            pstnBefore = workingLine.LastIndexOf(' ', ourCursorPosition);
            if (pstnBefore == ourCursorPosition)
            {
                // The user clicked on a space
                pstnBefore = workingLine.LastIndexOf(' ', pstnBefore - 1);
            }
            if (pstnBefore == -1)
            {
                pstnBefore = 0;
            }
            else
            {
                //Move off the space, one forward to the next character
                pstnBefore++;
            }
            // Now find the end of the word
            pstnAfter = workingLine.IndexOf(' ', pstnBefore);
            //What if it's the last word?
            if (pstnAfter == -1)
            {
                pstnAfter = workingLine.Length - 1;
            }
            else
            {
                // Again, adjust backwards off the space
                pstnAfter--;
            }
            return workingLine.Substring(pstnBefore, pstnAfter - pstnBefore + 1);
        }

        public String getVerseNumber(RichTextBox currentTextArea)
        {
            int selectionPoint, revisedPosition, startPstn;
            String textStream, returnedLine;

            textStream = currentTextArea.Text;
            selectionPoint = currentTextArea.SelectionStart;
            returnedLine = isolateCurrentLine(textStream, selectionPoint, out revisedPosition);

            startPstn = returnedLine.IndexOf(' ');
            if (startPstn > -1)
            {
                returnedLine = returnedLine.Substring(0, startPstn);
                if (returnedLine[returnedLine.Length - 1] == ':')
                {
                    returnedLine = returnedLine.Substring(0, returnedLine.Length - 1);
                }
                return returnedLine;
            }
            return "";
        }

        private String isolateCurrentLine(String textStream, int cursorPosition, out int revisedPosition)
        {
            /*****************************************************************************************************
             *                                                                                                   *
             *  Given a specific position in the text, find the line in which the position occurs.               *
             *                                                                                                   *
             *  The assumption here is that the text stream is a multi-line text (i.e. contains carriage returns *
             *    - although it must also cater for the fact that they don't exist).  This process is useful in  *
             *    itself but also as the first part of identifying the word in which the cursor currently exists *
             *                                                                                                   *
             *  Returns: the current line (with no carriage returns).                                            *
             *           revisedPosition contains either:                                                        *
             *               a) the same as cursorPosition but revised so that it works in the returned string,  *
             *               b) an error code (which will always be negative)                                    *
             *                                                                                                   *
             *****************************************************************************************************/

            int pstnBefore, pstnAfter, ourCursorPosition;
            String resultingLine;
            Char[] trimParams = { '\n' };

            // First check that the provided information is not duff.  If cursorPosition is outside the string range
            //   a) if <0, position at the start of the string;
            //   b) if > the length of the string, position it at the end of the string.
            if ((textStream == null) || (textStream.Length == 0))
            {
                MessageBox.Show("You have asked for a word search on a null string", "Text Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                revisedPosition = -1;
                return "";
            }
            ourCursorPosition = cursorPosition;
            if (cursorPosition < 0) ourCursorPosition = 0;
            if (cursorPosition >= textStream.Length) ourCursorPosition = textStream.Length - 1;
            // Find the start of the current line, which may be marked by a \n or nothing
            pstnBefore = textStream.LastIndexOf('\n', ourCursorPosition);
            if (pstnBefore == ourCursorPosition)
            {
                // This can happen when the mouse is clicked in the white space to the right of a line
                if (ourCursorPosition == 0)
                {
                    //Maybe the mouse clicked an area with multiple carriage returns
                    revisedPosition = 0;
                    return "";
                }
                pstnBefore = textStream.LastIndexOf('\n', ourCursorPosition - 1);
            }
            if (pstnBefore < 0)
            {
                // We're probably on the first line of text
                pstnBefore = 0;
            }
            else
            {
                // We've found a carriage return but we want to exclude it
                pstnBefore++;
            }
            // Now go the other way
            pstnAfter = textStream.IndexOf('\n', ourCursorPosition);
            if (pstnAfter < 0)
            {
                // We are at the end of text
                pstnAfter = textStream.Length - 1;
            }
            else
            {
                //Once again, exclude the carriage return
                pstnAfter--;
            }
            // Check that we still have a real string
            if (pstnBefore >= pstnAfter)
            {
                revisedPosition = 0;
                return "";
            }
            resultingLine = textStream.Substring(pstnBefore, pstnAfter - pstnBefore + 1);
            // Just to be sure, lets clear any starting or trailing carriage returns
            resultingLine = resultingLine.Trim(trimParams);
            // Adjust relative cursor position
            revisedPosition = ourCursorPosition - pstnBefore;
            if (revisedPosition >= resultingLine.Length) revisedPosition--;
            return resultingLine;
        }

        public String copyCurrentWord(int optionFlag, int code, int refCode)
        {
            bool isNT;
            String wordCopy;

            // Where are we now?
            isNT = code == 0;
            wordCopy = getSelectedWord(isNT);
            coreOfCopy(2, wordCopy, optionFlag, code, refCode);
            return wordCopy;
        }

        public void copyCurrentVerse(int optionFlag, int code, int refCode)
        {
            coreOfCopy(1, "", optionFlag, code, refCode);
        }

        public void copyCurrentChapter(int optionFlag, int code, int refCode)
        {
            coreOfCopy(0, "", optionFlag, code, refCode);
        }

        private void coreOfCopy(int copyType, String specificWord, int optionFlag, int code, int refCode)
        {
            int bookIdx, chapIdx, verseIdx, noOfVerses;
            String chapterCopy, bookName, relevantRef;
            classBookContent currBook;
            classChapterContent currChapter;
            classVerseContent currVerse;
            ComboBox cbBook, cbChapter, cbVerse;

            chapterCopy = "";
            if (code == 0)
            {
                cbBook = globalVars.getComboBoxControlByIndex(0);
                cbChapter = globalVars.getComboBoxControlByIndex(1);
            }
            else
            {
                cbBook = globalVars.getComboBoxControlByIndex(3);
                cbChapter = globalVars.getComboBoxControlByIndex(4);
            }
            bookIdx = cbBook.SelectedIndex;
            chapIdx = cbChapter.SelectedIndex;
            if (code == 0) ListOfNTBooks.TryGetValue(bookIdx, out currBook);
            else ListOfLxxBooks.TryGetValue(bookIdx, out currBook);
            bookName = currBook.BookName;
            currChapter = currBook.getChapterBySeqNo(chapIdx);
            if (copyType == 0)
            {
                noOfVerses = currChapter.NoOfVersesInChapter;
                for (verseIdx = 0; verseIdx < noOfVerses; verseIdx++)
                {
                    currVerse = currChapter.getVerseBySeqNo(verseIdx);
                    if (verseIdx == 0)
                    {
                        chapterCopy = currChapter.getVerseIdBySeqNo(0) + ":  " + currVerse.getWholeText(true);
                    }
                    else
                    {
                        chapterCopy += "\n" + currChapter.getVerseIdBySeqNo(verseIdx) + ":  " + currVerse.getWholeText(true);
                    }
                }
                relevantRef = bookName + " " + currBook.getChapterIdBySeqNo(chapIdx);
                if (refCode == 1) chapterCopy = relevantRef + ":\n\n" + chapterCopy;
            }
            else
            {
                if (code == 0) cbVerse = globalVars.getComboBoxControlByIndex(2);
                else cbVerse = globalVars.getComboBoxControlByIndex(5);
                verseIdx = cbVerse.SelectedIndex; ;
                currVerse = currChapter.getVerseBySeqNo(verseIdx);
                relevantRef = bookName + " " + currBook.getChapterIdBySeqNo(chapIdx) + ":" + currChapter.getVerseIdBySeqNo(verseIdx);
                if (copyType == 1)
                {
                    if (refCode == 1) chapterCopy = relevantRef + "\n\n" + currVerse.getWholeText(true);
                    else chapterCopy = currVerse.getWholeText(true);
                }
                else
                {
                    if (refCode == 1) chapterCopy = relevantRef + ": " + specificWord;
                    else chapterCopy = specificWord;
                    relevantRef = relevantRef + ": " + specificWord;
                }
            }
            if (optionFlag == 0) Clipboard.SetText(chapterCopy);
            else
            {
                if (optionFlag == 1) insertTextToNotes(chapterCopy);
            }
            if (optionFlag == 0) MessageBox.Show(relevantRef + " has been copied to the clipboard.", "Copy Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void copyParseAndLexiconData(int actionType, bool isToNotes)
        {
            bool isContent = false;
            String confirmingMessage = "", managedText = "";
            RichTextBox activeTextBox;

            switch (actionType)
            {
                case 0: // Grammatical Information only
                    activeTextBox = globalVars.getRichtextControlByIndex(1);
                    if ((activeTextBox.Text == null) || (activeTextBox.Text.Length == 0)) return;
                    managedText = activeTextBox.Text;
                    confirmingMessage = "Grammatical Summary";
                    break;
                case 1: // Lexical Information only
                    activeTextBox = globalVars.getRichtextControlByIndex(2);
                    if ((activeTextBox.Text == null) || (activeTextBox.Text.Length == 0)) return;
                    managedText = activeTextBox.Text;
                    confirmingMessage = "Lexical Summary";
                    break;
                case 2:
                    activeTextBox = globalVars.getRichtextControlByIndex(1);
                    if ((activeTextBox.Text != null) && (activeTextBox.Text.Length > 0))
                    {
                        managedText = "Grammatical Summary\n-------------------\n\n" + activeTextBox.Text;
                        confirmingMessage = "Grammatical Summary";
                        isContent = true;
                    }
                    activeTextBox = globalVars.getRichtextControlByIndex(2);
                    if ((activeTextBox.Text != null) && (activeTextBox.Text.Length > 0))
                    {
                        if (managedText.Length == 0)
                        {
                            managedText = "Lexical Summary\n-------------------\n\n" + activeTextBox.Text;
                            confirmingMessage = "Lexical Summary";
                        }
                        else
                        {
                            managedText += "\n\nLexical Summary\n-------------------\n\n" + activeTextBox.Text;
                            confirmingMessage += " and Lexical Summary";
                        }
                        isContent = true;
                    }
                    if (!isContent) return;
                    break;
            }
            if (isToNotes)
            {
                insertTextToNotes(managedText);
            }
            else
            {
                Clipboard.SetText(managedText);
                MessageBox.Show(confirmingMessage + " has been copied to the clipboard.", "Copy Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void insertTextToNotes(String textToBeInserted)
        {
            int nPstn;
            RichTextBox notesText;

            notesText = globalVars.getRichtextControlByIndex(6);
            nPstn = notesText.SelectionStart;
            if ((notesText.Text == null) || (notesText.Text.Length == 0))
            {
                notesText.Text = textToBeInserted;
            }
            else
            {
                if (nPstn > notesText.Text.Length - 1) nPstn = notesText.Text.Length - 1;
                if (nPstn == notesText.Text.Length - 1)
                {
                    notesText.AppendText(textToBeInserted);
                }
                else
                {
                    if (nPstn == 0)
                    {
                        notesText.Text = textToBeInserted + notesText.Text;
                        notesText.SelectionStart = textToBeInserted.Length;
                    }
                    else
                    {
                        notesText.Text = notesText.Text.Substring(0, nPstn) + textToBeInserted + notesText.Text.Substring(nPstn);
                        notesText.SelectionStart = nPstn + textToBeInserted.Length;
                    }
                }
            }
            notesText.Focus();
        }

        private void addEntryToHistory(String newEntry, int whichHistoryCode, int actionCode)
        {
            /*------------------------------------------------------------*
             *                                                            *
             *                      addEntryToHistory                     *
             *                      -----------------                     *
             *                                                            *
             *  This adds an entry to either of the history combo boxes.  *
             *                                                            *
             *  Parameters:                                               *
             *  ----------                                                *
             *                                                            *
             *  newEntry          The string that will be entered         *
             *  whichHistoryCode  An integer value specifying whether NT  *
             *                    or LXX:                                 *
             *                      0 = NT           1 = LXX              *
             *  actionCode        An integer value specifying whether to  *
             *                    add the entry at the head of the list   *
             *                    or at the tail;                         *
             *                      0 = head         1 - tail             *
             *                                                            *
             *------------------------------------------------------------*/
            ComboBox cbHistory;

            if (whichHistoryCode == 0) cbHistory = globalVars.getComboBoxControlByIndex(6);
            else cbHistory = globalVars.getComboBoxControlByIndex(7);
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
            cbHistory.SelectedIndex = 0;
        }

        public void loadHistory()
        {
            int idx, bookIdx, noOfBooks, epochCode = 6, nPstn, noOfChapters;
            Char discriminant = ' ';
            String historyFileName, fileBuffer, bookName, chapNo, interimResult, chapIdx;
            FileInfo fiHistory;
            StreamReader srHistory;
            ComboBox cbHistory;
            classBookContent currentBook;
            SortedDictionary<int, classBookContent> listOfBooks;

            historyFileName = globalVars.BaseDirectory + @"\" + globalVars.NotesPath + @"\history.txt";
            fiHistory = new FileInfo(historyFileName);
            if (fiHistory.Exists)
            {
                cbHistory = globalVars.getComboBoxControlByIndex(6);
                srHistory = new StreamReader(historyFileName);
                fileBuffer = srHistory.ReadLine();
                if ((fileBuffer != null) && (fileBuffer[0] == ';')) fileBuffer = srHistory.ReadLine();
                while (fileBuffer != null)
                {
                    addEntryToHistory(fileBuffer, epochCode - 6, 1);
                    //                    cbHistory.Items.Add(fileBuffer);
                    fileBuffer = srHistory.ReadLine();
                    if ((fileBuffer != null) && (fileBuffer[0] == ';'))
                    {
                        fileBuffer = srHistory.ReadLine();
                        cbHistory = globalVars.getComboBoxControlByIndex(7);
                        epochCode = 7;
                    }
                }
                srHistory.Close();
                srHistory.Dispose();
                for (epochCode = 6; epochCode < 8; epochCode++)
                {
                    cbHistory = globalVars.getComboBoxControlByIndex(epochCode);
                    if (cbHistory.Items.Count == 0)
                    {
                        if (epochCode == 6) fileBuffer = "Matthew 1";
                        else fileBuffer = "Genesis 1";
                        addEntryToHistory(fileBuffer, epochCode, 1);
                        return;
                    }
                    cbHistory.SelectedIndex = 0;
                    fileBuffer = cbHistory.SelectedItem.ToString();
                    nPstn = fileBuffer.LastIndexOf(discriminant);
                    if (nPstn < 0) return;   // This would be a problem
                    bookName = fileBuffer.Substring(0, nPstn);
                    chapNo = fileBuffer.Substring(nPstn + 1);
                    bookIdx = -1;
                    currentBook = null;
                    if (epochCode == 6) listOfBooks = listOfNTBooks;
                    else listOfBooks = listOfLxxBooks;
                    noOfBooks = listOfBooks.Count;
                    for (idx = 0; idx < noOfBooks; idx++)
                    {
                        listOfBooks.TryGetValue(idx, out currentBook);
                        if (String.Compare(bookName, currentBook.BookName) == 0)
                        {
                            bookIdx = idx;
                            break;
                        }
                    }
                    if (bookIdx < 0) return; // This would be a problem too
                                             // We now know the book, so ???? confirm that selected chapter exists ???
                    noOfChapters = currentBook.NoOfChaptersInBook;
                    chapIdx = "";
                    for (idx = 0; idx < noOfChapters; idx++)
                    {
                        interimResult = currentBook.getChapterIdBySeqNo(idx);
                        if (String.Compare(interimResult, chapNo) == 0)
                        {
                            chapIdx = interimResult;
                            break;
                        }
                    }
                    if (chapIdx.Length == 0) return;  // Yet another problem
                    if (epochCode == 6) displayNTChapter(bookIdx, chapIdx, true);
                    else displayLXXChapter(bookIdx, chapIdx, true);
                }
            }
            else
            {
                switch (globalVars.DisplayTextCode)
                {
                    case 0: displayNTChapter(0, "1", true); break;
                    case 1: displayNTChapter(27, "1", true); break;
                    case 2: displayNTChapter(27, "1", true); break;
                    case 3: displayNTChapter(0, "1", true); break;
                }
            }
        }

        public void saveHistory()
        {
            String historyFileName;
            StreamWriter swHistory;
            ComboBox cbHistory;

            historyFileName = globalVars.BaseDirectory + @"\" + globalVars.NotesPath + @"\history.txt";
            swHistory = new StreamWriter(historyFileName);
            cbHistory = globalVars.getComboBoxControlByIndex(6);
            swHistory.WriteLine(";NT");
            foreach (String historyItem in cbHistory.Items)
            {
                swHistory.WriteLine(historyItem);
            }
            swHistory.WriteLine(";LXX");
            cbHistory = globalVars.getComboBoxControlByIndex(7);
            foreach (String historyItem in cbHistory.Items)
            {
                swHistory.WriteLine(historyItem);
            }
            swHistory.Close();
            swHistory.Dispose();
        }

        public void advanceHistory(int forwardBack, int ntLxxCode)
        {
            /**********************************************************
             *                                                        *
             *                    advanceHistory                      *
             *                    ==============                      *
             *                                                        *
             *  Simply handles moving backwards or forwards from the  *
             *    present chapter.                                    *
             *                                                        *
             *  Parameters:                                           *
             *    forwardBack: 1 = previous chapter                   *
             *                 2 = next chapter                       *
             *                                                        *
             **********************************************************/

            int bookIdx = 0, actualIdx, chapIdx;
            String chapNo;
            ComboBox cbBooks, cbChapters;
            classBookContent currentBook;
            classChapterContent currentChapter, advChapter;
            SortedDictionary<int, classBookContent> listOfBooks = null;

            if (ntLxxCode == 0)
            {
                listOfBooks = listOfNTBooks;
                cbBooks = globalVars.getComboBoxControlByIndex(0);
                cbChapters = globalVars.getComboBoxControlByIndex(1);
            }
            else
            {
                listOfBooks = listOfLxxBooks;
                cbBooks = globalVars.getComboBoxControlByIndex(3);
                cbChapters = globalVars.getComboBoxControlByIndex(4);
            }
            actualIdx = cbBooks.SelectedIndex;
            chapIdx = cbChapters.SelectedIndex;
            listOfBooks.TryGetValue(actualIdx, out currentBook);
            currentChapter = currentBook.getChapterBySeqNo(chapIdx);
            if (forwardBack == 1) advChapter = currentChapter.NextChapter;
            else advChapter = currentChapter.PreviousChapter;
            if (advChapter == null) return;
            bookIdx = advChapter.BookId;
            chapNo = advChapter.ChapterId;
            if (ntLxxCode == 0) displayNTChapter(bookIdx, chapNo, true);
            else displayLXXChapter(bookIdx, chapNo, true);
        }

        public void processSelectedHistory(int historyCode)
        {
            int idx, noOfBooks, bookIdx, nPstn;
            String historyEntry, lastHistoryEntry, bookName, chapNo;
            ComboBox cbBooks, cbChapters, cbHistory;
            classBookContent currentBook;
            SortedDictionary<int, classBookContent> listOfBooks = null;

            if (historyCode == 0)
            {
                listOfBooks = listOfNTBooks;
                cbBooks = globalVars.getComboBoxControlByIndex(0);
                cbChapters = globalVars.getComboBoxControlByIndex(1);
                cbHistory = globalVars.getComboBoxControlByIndex(6);
                lastHistoryEntry = lastNTHistoryEntry;
                noOfBooks = noOfNTBooks;
            }
            else
            {
                listOfBooks = listOfLxxBooks;
                cbBooks = globalVars.getComboBoxControlByIndex(3);
                cbChapters = globalVars.getComboBoxControlByIndex(4);
                cbHistory = globalVars.getComboBoxControlByIndex(7);
                lastHistoryEntry = lastLxxHistoryEntry;
                noOfBooks = noOfLxxBooks;
            }
            if (cbHistory.Items.Count == 0) return;
            historyEntry = cbHistory.SelectedItem.ToString();
            if (String.Compare(lastHistoryEntry, historyEntry) == 0) return;
            nPstn = historyEntry.LastIndexOf(' ');
            if (nPstn < 0) return;
            bookName = historyEntry.Substring(0, nPstn);
            chapNo = historyEntry.Substring(nPstn + 1);
            bookIdx = -1;
            for (idx = 0; idx < noOfBooks; idx++)
            {
                listOfBooks.TryGetValue(idx, out currentBook);
                if (String.Compare(currentBook.BookName, bookName) == 0)
                {
                    bookIdx = idx;
                    break;
                }
            }
            if (bookIdx > -1)
            {
                if (historyCode == 0)
                {
                    lastNTHistoryEntry = historyEntry;
                    displayNTChapter(bookIdx, chapNo, true);
                }
                else
                {
                    lastLxxHistoryEntry = historyEntry;
                    displayLXXChapter(bookIdx, chapNo, true);
                }
            }
        }
    }
}
