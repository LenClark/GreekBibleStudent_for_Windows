using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    class classSearch
    {
        int noOfRefs, noOfResults = 0;

        classGlobal globalVars;
        classText textHandler;
        GreekProcessing greekProcessing;
        SortedDictionary<int, classReference> listOfRefs = new SortedDictionary<int, classReference>();
        SortedDictionary<String, int> refControl = new SortedDictionary<string, int>();
        SortedList<String, List<int>> listOfPrimaryMatches = new SortedList<string, List<int>>();     // New
        SortedDictionary<int, classSearchResult> listOfResults = new SortedDictionary<int, classSearchResult>();     // New

        public int NoOfResults { get => noOfResults; set => noOfResults = value; }

        /*-----------------------------------------------------------------------------------*
         *                                                                                   *
         *                     listOfRefs and refControl: explanation                        *
         *                     --------------------------------------                        *
         *                                                                                   *
         *  listOfRefs:                                                                      *
         *    key        a simple, sequential value (so we can subsequently step through in  *
         *                 order                                                             *
         *    value      the relevant class instance                                         *
         *                                                                                   *
         *  refControl:                                                                      *
         *    key        a compound of Old/New id (o), bookId (b), chapter seq (c) and       *
         *                 verse seq (v), in the format:                                     *
         *                    o-b-c-v                                                        *
         *    value      the integer key of listOfRefs                                       *
         *                                                                                   *
         *  Why?  So that we can easily see whether a given verse has already been           *
         *    referenced.  It probably has little additional benefit.                        *
         *                                                                                   *
         *-----------------------------------------------------------------------------------*/

        public void initialiseSearch(classGlobal inConfig, classText inTextClass, GreekProcessing inGk)
        {
            globalVars = inConfig;
            textHandler = inTextClass;
            greekProcessing = inGk;
        }

        public /* String */ void performSearch(String sourceString, bool isComplex, int rbOption, String secondaryWord, int contextCount,
            SortedList<int, int> ntBooksToSearch, SortedList<int, int> lxxBooksToSearch, int isPrimaryNTorLXX, int bookCode, String chapRef,
            String verseRef, int isSecondaryNTorLXX, int bookCode2ary, String chapRef2ary, String verseRef2ary)
        {
            /*************************************************************************************************************************
             *                                                                                                                       *
             *  This procedure creates a string that contains the results of the specified search.  It provides alternative options: *
             *                                                                                                                       *
             *  a) A simple search: all occurrences of a given lexime (i.e                                                           *
             *     i)   Find the root of the chosen word                                                                             *
             *     ii)  Perform a word by word comparison of the *roots* of all NT words for a match                                 *
             *     iii) Store the full text of those verses where a match occurs                                                     *
             *  b) A complex search: much as in a) but where the match occurs within n words (either way) of the root of the second  *
             *          specified word                                                                                               *
             *                                                                                                                       *
             *  It also recognises two types of search:                                                                              *
             *  a) Root comparison ( as described in a) above                                                                        *
             *  b) Exact word comparison, where both simple and comples searches are based on an exact match (including accents) of  *
             *          the word(s) as provided.                                                                                     *
             *                                                                                                                       *
             *  Parameters:                                                                                                          *
             *  ==========                                                                                                           *
             *                                                                                                                       *
             *  sourceString:        The word whose root is to be matched                                                            *
             *  isComplex:           If true, the procedure will perform a complex search and requires values for secondaryWord and  *
             *                         contextCount to be provided                                                                   *
             *                       If false, these two parameters are ignored                                                      *
             *  rbOption             The selected option in the main Search panel                                                    *
             *                          1 = comparison of all words by root                                                          *
             *                          2 = comparison by exact match of the given occurrence                                        *
             *  secondaryWord:       A second word; occurrences of the first lexeme must be within contextCount words of the second  *
             *                         lexeme                                                                                        *
             *  contextCount:        See above                                                                                       *
             *  ntBooksToSearch:     A list of NT books selected for the search (keyed on a simple 0-based integer)                  *
             *  lxxBooksToSearch:    A list of LXX books, as above                                                                   *
             *                                                                                                                       *
             *************************************************************************************************************************/

            int copyOfContextCount;
            String primaryString, secondaryString;
            Tuple<String, String, String, String> returnedWordData;
            Tuple<String, String> primaryAndSecondary;

            // Validate the provided fields
            copyOfContextCount = contextCount;
            if ((sourceString == null) || (sourceString.Length == 0))
            {
                MessageBox.Show("You must provide a word in which to base the search", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //                return "";
                return;    // New
            }
            if (sourceString.Contains(' '))
            {
                if (isComplex)
                {
                    MessageBox.Show("Bible Reader can only search for a single word in each text box",
                    "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Bible Reader can only search for a single word at a time\nConsider using the Advanced Search",
                    "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //                return "";
                return;    // New
            }
            if (isComplex)
            {
                if (copyOfContextCount > 10)
                {
                    MessageBox.Show("The search facility is limited to a maximum separation of ten words between the primary and secondary words\n" +
                    "The separation has been reset to 5", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    copyOfContextCount = 10;
                }
                if (copyOfContextCount < 1) copyOfContextCount = 1;
                if (secondaryWord.Contains(' '))
                {
                    MessageBox.Show("Bible Reader can only search for a single word in each text box", "Search Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //                return "";
                    return;    // New
                }
            }
            // Get rid of any spurious characters
            returnedWordData = greekProcessing.removeNonGkChars(sourceString);
            primaryString = returnedWordData.Item4;
            returnedWordData = greekProcessing.removeNonGkChars(secondaryWord);
            secondaryString = returnedWordData.Item4;
            // Now get rid of accents, etc
            primaryString = greekProcessing.reduceToBareGreek(primaryString, true);
            secondaryString = greekProcessing.reduceToBareGreek(secondaryString, true);
            if (rbOption != 2)
            {
                primaryAndSecondary = findRoot(primaryString, secondaryString, isComplex, rbOption, isPrimaryNTorLXX, bookCode, chapRef,
                    verseRef, isSecondaryNTorLXX, bookCode2ary, chapRef2ary, verseRef2ary);
                primaryString = primaryAndSecondary.Item1;
                secondaryString = primaryAndSecondary.Item2;
            }
            // Now we are in a position to perform the search - basically, simply scan
            //   Note: the word sequence(s) passed to simpleScan will either be the roots or the actual occurrences, depending on rbOption
            listOfResults.Clear();
            if (isComplex) complexScan(ntBooksToSearch, lxxBooksToSearch, primaryString, secondaryString, rbOption, contextCount);
            else /* return */ simpleScan(ntBooksToSearch, lxxBooksToSearch, primaryString, secondaryString, isComplex, rbOption, contextCount);

        }

        private Tuple<String, String> findRoot(String primaryOriginal, String secondaryOriginal, bool isComplex, int rbOption, int isPrimaryNTorLXX,
            int bookCode, String chapRef, String verseRef, int isSecondaryNTorLXX, int bookCode2ary, String chapRef2ary, String verseRef2ary)
        {
            /************************************************************************************************
             *                                                                                              *
             *                                         findRoot                                             *
             *                                         ========                                             *
             *                                                                                              *
             *  Purpose: to find the root form of the primary word and, if it is not null, the secondary    *
             *           word.                                                                              *
             *                                                                                              *
             *  We need to know an occurrence of the provided word (both primary and secondary) in order to *
             *    use that to get the root form.                                                            *
             *                                                                                              *
             *  In each case, if isPrimaryNTorLXX and isSecondaryNTorLXX > -1, then the target word(s)      *
             *    have been specified in a manner that also identifies the reference of a source example.   *
             *    This means that we *don't* have to scan the whole text in order to find an occurrence.    *
             *                                                                                              *
             *  If isPrimaryNTorLXX or isSecondaryNTorLXX = -1, this means that the word(s) was/were        *
             *    typed in and we don't have a set location for the word(s).  As a consequence, we will     *
             *    have to search *all* books to find a root for the word given.  It works on the basis that *
             *    the user has entered a valid non-root form of a lexeme and we compare it with occurrences *
             *    in the text in order to identify the root.                                                *
             *                                                                                              *
             *  If isComplex is true, it will do the same for the secondaryWord.                            *
             *                                                                                              *
             *  It will return the two forms that will be the base of the subsequent search in the Tuple.   *
             *                                                                                              *
             ************************************************************************************************/

            bool isSecondaryFound = false;
            int idx, bookIdx, chapIdx, verseIdx, wordIdx, noOfBooks, noOfChapters, noOfVerses, noOfWords;
            String candidateWord = "", primaryRoot = "", secondaryRoot = "";
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            classWordContent currentWord;

            if (rbOption == 2)
            {
                // We are using exact match comparison.  We have to assume that the words provided are correct,
                //   so there's nothing else to do.
                if (!isComplex)
                {
                    return new Tuple<string, string>(primaryRoot, "");
                }
                if (isSecondaryFound)
                {
                    return new Tuple<string, string>(primaryRoot, secondaryRoot);
                }
            }
            if (isPrimaryNTorLXX > -1)
            {
                // We are provided the source reference for the word
                if (isPrimaryNTorLXX == 0) currentBook = textHandler.getNTBookByIndex(bookCode);
                else currentBook = textHandler.getLXXBookByIndex(bookCode);
                currentChapter = currentBook.getChapterByChapterId(chapRef);
                currentVerse = currentChapter.getVerseByVerseNo(verseRef);
                noOfWords = currentVerse.WordCount;
                for (wordIdx = 0; wordIdx < noOfWords; wordIdx++)
                {
                    currentWord = currentVerse.getWordBySeqNo(wordIdx);
                    //                    candidateWord = greekProcessing.reduceToBareGreek( currentWord.RootWord, false );
                    candidateWord = currentWord.AccentlessTextWord;
                    if (String.Compare(candidateWord, primaryOriginal) == 0)
                    {
                        primaryRoot = currentWord.RootWord;
                        break;
                    }
                }
            }
            else
            {
                for (idx = 0; idx < 2; idx++)
                {
                    // Two scans: idx = 0 -> NT scan
                    //            idx = 1 -> Lxx scan
                    if (idx == 0) noOfBooks = textHandler.NoOfNTBooks;
                    else noOfBooks = textHandler.NoOfLxxBooks;
                    for (bookIdx = 0; bookIdx < noOfBooks; bookIdx++)
                    {
                        if (idx == 0) currentBook = textHandler.getNTBookByIndex(bookIdx);
                        else currentBook = textHandler.getLXXBookByIndex(bookIdx);
                        noOfChapters = currentBook.NoOfChaptersInBook;
                        for (chapIdx = 0; chapIdx < noOfChapters; chapIdx++)
                        {
                            currentChapter = currentBook.getChapterBySeqNo(chapIdx);
                            noOfVerses = currentChapter.NoOfVersesInChapter;
                            for (verseIdx = 0; verseIdx < noOfVerses; verseIdx++)
                            {
                                currentVerse = currentChapter.getVerseBySeqNo(verseIdx);
                                if (currentVerse != null)
                                {
                                    noOfWords = currentVerse.WordCount;
                                    for (wordIdx = 0; wordIdx < noOfWords; wordIdx++)
                                    {
                                        currentWord = currentVerse.getWordBySeqNo(wordIdx);
                                        candidateWord = currentWord.RootWord;
                                        // But we also need to remove accents
                                        candidateWord = greekProcessing.reduceToBareGreek(candidateWord, true);
                                        if (String.Compare(candidateWord, primaryOriginal) == 0)
                                        {
                                            primaryRoot = currentWord.RootWord;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isComplex)
            {
                if (isSecondaryNTorLXX > -1)
                {
                    // We are provided the source reference for the word
                    if (isSecondaryNTorLXX == 0) currentBook = textHandler.getNTBookByIndex(bookCode2ary);
                    else currentBook = textHandler.getLXXBookByIndex(bookCode2ary);
                    currentChapter = currentBook.getChapterByChapterId(chapRef2ary);
                    currentVerse = currentChapter.getVerseByVerseNo(verseRef2ary);
                    noOfWords = currentVerse.WordCount;
                    for (wordIdx = 0; wordIdx < noOfWords; wordIdx++)
                    {
                        currentWord = currentVerse.getWordBySeqNo(wordIdx);
                        candidateWord = currentWord.AccentlessTextWord;
                        if (String.Compare(candidateWord, secondaryOriginal) == 0)
                        {
                            secondaryRoot = currentWord.RootWord;
                            isSecondaryFound = true;
                            break;
                        }
                    }
                }
                else
                {
                    for (idx = 0; idx < 2; idx++)
                    {
                        // Two scans: idx = 0 -> NT scan
                        //            idx = 1 -> Lxx scan
                        if (idx == 0) noOfBooks = textHandler.NoOfNTBooks;
                        else noOfBooks = textHandler.NoOfLxxBooks;
                        for (bookIdx = 0; bookIdx < noOfBooks; bookIdx++)
                        {
                            if (idx == 0) currentBook = textHandler.getNTBookByIndex(bookIdx);
                            else currentBook = textHandler.getLXXBookByIndex(bookIdx);
                            noOfChapters = currentBook.NoOfChaptersInBook;
                            for (chapIdx = 0; chapIdx < noOfChapters; chapIdx++)
                            {
                                currentChapter = currentBook.getChapterBySeqNo(chapIdx);
                                noOfVerses = currentChapter.NoOfVersesInChapter;
                                for (verseIdx = 0; verseIdx < noOfVerses; verseIdx++)
                                {
                                    currentVerse = currentChapter.getVerseBySeqNo(verseIdx);
                                    if (currentVerse != null)
                                    {
                                        noOfWords = currentVerse.WordCount;
                                        for (wordIdx = 0; wordIdx < noOfWords; wordIdx++)
                                        {
                                            currentWord = currentVerse.getWordBySeqNo(wordIdx);
                                            candidateWord = currentWord.RootWord;
                                            // But we also need to remove accents
                                            candidateWord = greekProcessing.reduceToBareGreek(candidateWord, true);
                                            if (String.Compare(candidateWord, secondaryOriginal) == 0)
                                            {
                                                secondaryRoot = currentWord.RootWord;
                                                isSecondaryFound = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new Tuple<string, string>(primaryRoot, secondaryRoot);
        }

        private void primaryScan(SortedList<int, int> ntBooksToSearch, SortedList<int, int> lxxBooksToSearch, String primaryString, int rbOption)
        {
            /**************************************************************************************************************
             *                                                                                                            *
             *                                               primaryScan                                                  *
             *                                               ===========                                                  *
             *                                                                                                            *
             *  The purpose of this method is to scan for all occurrences of the primary word and populate a "list" of    *
             *    references where the word is found (including repeats).                                                 *
             *                                                                                                            *
             *  Parameters:                                                                                               *
             *  ==========                                                                                                *
             *                                                                                                            *
             *    ntBooksToSearch    A list of NT books to be included in the search                                      *
             *    lxxBooksToSearch   A list of LXX books to be included in the search                                     *
             *    primaryString      The word to be searched for                                                          *
             *    rbOption           Specifies whether we are looking fo an exact match or a root match                   *
             *                          1 = root match                                                                    *
             *                          2 = exact match                                                                   *
             *                                                                                                            *
             **************************************************************************************************************/

            int idx, bIndex, bookIdx, chapIdx, verseIdx, wordIdx, noOfBooks, noOfChapters, noOfVerses, noOfWords, refRef;
            String candidateWord = "", cleanedPrimary, refKey;
            SortedList<int, int> booksToSearch;
            List<int> listOfOccurrences;     // New
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            classWordContent currentWord;
            classReference currentRef;
            ToolStripStatusLabel tempLabel1, tempLabel2, tempLabel3, tempLabel4;

            // Prepare information storage
            noOfRefs = 0;
            listOfRefs.Clear();
            refControl.Clear();
            listOfPrimaryMatches.Clear();
            cleanedPrimary = primaryString;
            globalVars.SearchMessages.TryGetValue(1, out tempLabel1);
            globalVars.SearchMessages.TryGetValue(2, out tempLabel2);
            globalVars.SearchMessages.TryGetValue(3, out tempLabel3);
            globalVars.SearchMessages.TryGetValue(4, out tempLabel4);
            if (rbOption == 2) cleanedPrimary = greekProcessing.reduceToBareGreek(cleanedPrimary, false);
            for (idx = 0; idx < 2; idx++)
            {
                // Get the working list of books to be searched
                if (idx == 0) booksToSearch = ntBooksToSearch;
                else booksToSearch = lxxBooksToSearch;
                noOfBooks = booksToSearch.Count;
                if (noOfBooks == 0) continue;
                for (bIndex = 0; bIndex < noOfBooks; bIndex++)
                {
                    // Purely defensively ...
                    if (!booksToSearch.ContainsKey(bIndex)) continue;
                    booksToSearch.TryGetValue(bIndex, out bookIdx);
                    // Get the actual book instance
                    if (idx == 0) currentBook = textHandler.getNTBookByIndex(bookIdx);
                    else currentBook = textHandler.getLXXBookByIndex(bookIdx);
                    // Now dig down through all chapters, verses and words
                    noOfChapters = currentBook.NoOfChaptersInBook;
                    for (chapIdx = 0; chapIdx < noOfChapters; chapIdx++)
                    {
                        currentChapter = currentBook.getChapterBySeqNo(chapIdx);
                        noOfVerses = currentChapter.NoOfVersesInChapter;
                        for (verseIdx = 0; verseIdx < noOfVerses; verseIdx++)
                        {
                            currentVerse = currentChapter.getVerseBySeqNo(verseIdx);
                            if (currentVerse != null)
                            {
                                noOfWords = currentVerse.WordCount;
                                for (wordIdx = 0; wordIdx < noOfWords; wordIdx++)
                                {
                                    currentWord = currentVerse.getWordBySeqNo(wordIdx);
                                    if (rbOption == 2)
                                    {
                                        candidateWord = currentWord.AccentlessTextWord;
                                    }
                                    else
                                    {
                                        candidateWord = currentWord.RootWord;
                                    }
                                    if (rbOption == 2) candidateWord = greekProcessing.reduceToBareGreek(candidateWord, false);
                                    if (String.Compare(candidateWord, cleanedPrimary) == 0)
                                    {
                                        refKey = idx.ToString() + "-" + bookIdx.ToString() + "-" + chapIdx.ToString() + "-" + verseIdx.ToString();
                                        if (refControl.ContainsKey(refKey))
                                        {
                                            refControl.TryGetValue(refKey, out refRef);
                                            listOfRefs.TryGetValue(refRef, out currentRef);
                                            currentRef.NoOfMatches++;
                                            listOfPrimaryMatches.TryGetValue(refKey, out listOfOccurrences);     // New
                                            if (!listOfOccurrences.Contains(wordIdx)) listOfOccurrences.Add(wordIdx);     // New
                                        }
                                        else
                                        {
                                            currentRef = new classReference();
                                            currentRef.insertReference(idx, bookIdx, chapIdx, verseIdx);
                                            listOfRefs.Add(noOfRefs, currentRef);
                                            refControl.Add(refKey, noOfRefs++);
                                            listOfOccurrences = new List<int>();     // New
                                            listOfOccurrences.Add(wordIdx);     // New
                                            listOfPrimaryMatches.Add(refKey, listOfOccurrences);     // New

                                            tempLabel1.Text = "Performing initial primary search: " + currentBook.BookName;
                                            tempLabel2.Text = noOfRefs.ToString();
                                            if (noOfRefs == 1) tempLabel3.Text = "match";
                                            else tempLabel3.Text = "matches";
                                            tempLabel4.Text += " found so far; still working.";
                                            globalVars.StatStrip.Refresh();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            tempLabel1.Text = "Initial stage of search completed";
            tempLabel2.Text = "";
            tempLabel3.Text = "";
            tempLabel4.Text = "";
            globalVars.StatStrip.Refresh();
        }

        private /* String */ void simpleScan(SortedList<int, int> ntBooksToSearch, SortedList<int, int> lxxBooksToSearch, String primaryString, String secondaryString,
            bool isComplex, int rbOption, int contextCount)
        {
            /**************************************************************************************************************
             *                                                                                                            *
             *                                               simpleScan                                                   *
             *                                               ==========                                                   *
             *                                                                                                            *
             *  This method actually performs the scan.  Note that all words provided contain accents, etc.               *
             *                                                                                                            *
             *  Parameters:                                                                                               *
             *  ==========                                                                                                *
             *                                                                                                            *
             *  All parameters are described in performSearch above                                                       *
             *                                                                                                            *
             **************************************************************************************************************/

            bool isPrimaryMatch = false;     // New
            int idx, wdx, noOfWords;
            String cleanedPrimary, refKey /* New */;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            classWordContent currentWord;
            classReference currentRef;
            classSearchResult currentResult = null;
            ToolStripStatusLabel tempLabel1, tempLabel2, tempLabel3, tempLabel4;
            List<int> listOfOccurrences;     // New

            // Two scans, one for NT and one for LXX
            globalVars.SearchMessages.TryGetValue(1, out tempLabel1);
            globalVars.SearchMessages.TryGetValue(2, out tempLabel2);
            globalVars.SearchMessages.TryGetValue(3, out tempLabel3);
            globalVars.SearchMessages.TryGetValue(4, out tempLabel4);
            tempLabel1.Text = "Stage 2 - preparing the Search Results text";
            globalVars.StatStrip.Refresh();
            cleanedPrimary = primaryString;
            if (rbOption == 2) cleanedPrimary = greekProcessing.reduceToBareGreek(cleanedPrimary, false);
            primaryScan(ntBooksToSearch, lxxBooksToSearch, cleanedPrimary, rbOption);
            for (idx = 0; idx < noOfRefs; idx++)
            {
                listOfRefs.TryGetValue(idx, out currentRef);
                currentResult = new classSearchResult();     // New
                currentResult.setStoreRecord(2);   // New
                if (currentRef.NtOrLxxCode == 0) currentBook = textHandler.getNTBookByIndex(currentRef.BookId);
                else currentBook = textHandler.getLXXBookByIndex(currentRef.BookId);
                currentChapter = currentBook.getChapterBySeqNo(currentRef.ChapSeq);
                currentVerse = currentChapter.getVerseBySeqNo(currentRef.VerseSeq);
                currentResult.BookName = currentBook.BookName;
                currentResult.addChapterNo(currentBook.getChapterIdBySeqNo(currentRef.ChapSeq));
                currentResult.addVerseNo(currentChapter.getVerseIdBySeqNo(currentRef.VerseSeq));
                refKey = currentRef.NtOrLxxCode.ToString() + "-" + currentBook.BookId.ToString() + "-" +
                    currentRef.ChapSeq.ToString() + "-" + currentRef.VerseSeq.ToString();   // New
                noOfWords = currentVerse.WordCount;   // New
                for (wdx = 0; wdx < noOfWords; wdx++)   // New
                {   // New
                    currentWord = currentVerse.getWordBySeqNo(wdx);   // New
                    if (listOfPrimaryMatches.ContainsKey(refKey))   // New
                    {   // New
                        listOfPrimaryMatches.TryGetValue(refKey, out listOfOccurrences);   // New
                        if (listOfOccurrences.Contains(wdx)) isPrimaryMatch = true;   // New
                    }   // New
                    currentResult.addWord(currentWord.TextWord, isPrimaryMatch, false);   // New
                    isPrimaryMatch = false; // New
                }   // New
                listOfResults.Add(noOfResults++, currentResult);   // New
                                                                   //                resultingText += currentBook.BookName + " " + currentBook.getChapterIdBySeqNo(currentRef.ChapSeq) + ":" +
                                                                   //                                            currentChapter.getVerseIdBySeqNo(currentRef.VerseSeq) + "  " + currentVerse.getWholeText(idx == 0) + "\n\n";
                tempLabel1.Text = idx.ToString();
                if (idx == 1) tempLabel2.Text = "match";
                else tempLabel2.Text = "matches";
                tempLabel2.Text += " found so far";
                tempLabel3.Text = "Still working; currently processing:";
                tempLabel4.Text = currentBook.BookName;
                globalVars.StatStrip.Refresh();
            }
            tempLabel1.Text = idx.ToString();
            if (idx == 1) tempLabel2.Text = "match";
            else tempLabel2.Text = "matches";
            tempLabel2.Text += " found";
            tempLabel3.Text = "Search completed";
            if (rbOption == 1) tempLabel4.Text = "Criteria: all matches to root = " + cleanedPrimary;
            else tempLabel4.Text = "Criteria: exact matches to " + cleanedPrimary;
            globalVars.StatStrip.Refresh();
            //            return resultingText;
        }

        private String complexScan(SortedList<int, int> ntBooksToSearch, SortedList<int, int> lxxBooksToSearch, String primaryString, String secondaryString,
                int rbOption, int contextCount)
        {
            /*************************************************************************************************************
             *                                                                                                           *
             *   This method will look for all occurrences of secondaryWord within spanNo words (either way) of          *
             *     primaryWord (derived using currVerse and vIdx).                                                       *
             *                                                                                                           *
             *   This may require retrieving either the verse prior to the current one or the verse following the        *
             *     current one (or, in extreme, both).  No attempt will be made to cross into prior or following books.  *
             *                                                                                                           *
             *   This is, in effect, an integral part of performSearch but has been separated to relieve visual pressure *
             *     on that method and because it is of a particularly complex nature.                                    *
             *                                                                                                           *
             *   currBook:       The Book object holding data for the book currently in view                             *
             *   cIdx:           The chapter sequencenumber.  (Note, the related Chapters object can be derived, using   *
             *                       currBook and this value.)                                                           *
             *   vIdx:           The verse sequence number. (The Verses object can also be derived.)                     *
             *   wIdx:           This is the index of the identified word in the verse matching the primary search word. *
             *   spanNo:         The value set as a maximum separation between the primary and secondary search words to *
             *                       qualify as matching.                                                                *
             *   secondaryWord:  The secondary word                                                                      *
             *   isExact:        A flag indicating whether the search is simple (the occurrence of the primary word      *
             *                       only) or complex (searching for the co-occurrence of two words within a defined     *
             *                       range).  The variables, spanNo and secondaryWord are only needed if isExact is      *
             *                       true.                                                                               *
             *                                                                                                           *
             *   It will return:                                                                                         *
             *          a) true, if a complex match has been found, otherwise false;                                     *
             *          b) if true, the returnedText output parameter will contain any verses that contain the two       *
             *               source words.                                                                               *
             *                                                                                                           *
             *************************************************************************************************************/

            bool isMatch, isMatch2, isFinal = false, isPrimaryMatch = false, isSecondaryMatch = false;     // New
            int idx, jdx, verseIdx, wordIdx, noOfWords, occurrences, countDown, proximityCount = 0, index, matchCount;
            String resultingText = "", refKey /* New */, candidateWord, cleanedPrimary, candidateSecondary;
            String[] decompRef, fullDecomp;
            Char[] splitParams = { ' ' }, splitRef = { ':' };
            SortedList<int, classVerseContent> impactedVerse = new SortedList<int, classVerseContent>();
            SortedList<int, classVerseContent> impactedVerse2 = new SortedList<int, classVerseContent>();
            SortedList<int, classVerseContent> finalCollection = new SortedList<int, classVerseContent>();
            List<int> listOfOccurrences;     // New
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse, otherVerse;
            classWordContent currentWord, otherWord;
            classReference currentRef;
            classSearchResult currentResult = null;
            classSearchResultLine currentResultLine = null;
            ToolStripStatusLabel tempLabel1, tempLabel2, tempLabel3, tempLabel4;

            // Two scans, one for NT and one for LXX
            globalVars.SearchMessages.TryGetValue(1, out tempLabel1);
            globalVars.SearchMessages.TryGetValue(2, out tempLabel2);
            globalVars.SearchMessages.TryGetValue(3, out tempLabel3);
            globalVars.SearchMessages.TryGetValue(4, out tempLabel4);
            tempLabel1.Text = "Stage 2 - preparing the Search Results text";
            globalVars.StatStrip.Refresh();
            cleanedPrimary = primaryString;
            if (rbOption == 2) cleanedPrimary = greekProcessing.reduceToBareGreek(cleanedPrimary, false);
            primaryScan(ntBooksToSearch, lxxBooksToSearch, cleanedPrimary, rbOption);
            // At this point we have a series of references of all *primary* matches.  We now need to revisit each
            //   of these (and look at the verses either side, if need be), to see if the *secondary* word occurs
            for (idx = 0; idx < noOfRefs; idx++)
            {
                // This will refer to each of the found primary matches, one at a time
                isMatch = false;
                isMatch2 = false;
                matchCount = 0;
                impactedVerse.Clear();
                impactedVerse2.Clear();
                finalCollection.Clear();
                listOfRefs.TryGetValue(idx, out currentRef);   // This *is* one of the matches, so get the book, chapter and verse
                if (currentRef.NtOrLxxCode == 0) currentBook = textHandler.getNTBookByIndex(currentRef.BookId);
                else currentBook = textHandler.getLXXBookByIndex(currentRef.BookId);
                currentChapter = currentBook.getChapterBySeqNo(currentRef.ChapSeq);
                currentVerse = currentChapter.getVerseBySeqNo(currentRef.VerseSeq);
                occurrences = currentRef.NoOfMatches;  // What if the primary word occurs more than once?
                // Within the verse, find the primary word
                noOfWords = currentVerse.WordCount;
                for (jdx = 0; jdx < noOfWords; jdx++)
                {
                    otherVerse = currentVerse;
                    currentWord = currentVerse.getWordBySeqNo(jdx);
                    if (rbOption == 1) candidateWord = currentWord.RootWord;
                    else candidateWord = currentWord.AccentlessTextWord;
                    if (occurrences > 0)
                    {
                        if (String.Compare(primaryString, candidateWord) == 0)
                        {
                            // We've found a match.  So, start scanning back
                            countDown = contextCount - 1;
                            wordIdx = jdx - 1;
                            verseIdx = 10;
                            occurrences--;  // Make sure we register each find
                            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*
                             *                                                                                                 *
                             *                                        impactVerse                                              *
                             *                                        -----------                                              *
                             *                                                                                                 *
                             *  Summary of the content of impactVerse at the end of this step:                                 *
                             *                                                                                                 *
                             *  Initially, this will contain a reference to the verse containing the primary match.  In most   *
                             *    cases, this will be the only contents of impactVerse.  However, in the event that the        *
                             *    secondary match is in a prior verse, the key (verseIdx) will be *decremented* and the        *
                             *    additional verse added.                                                                      *
                             *                                                                                                 *
                             *  verseIdx has an initial value of 10 and is decreased for each earlier verse scanned.  The      *
                             *    value of 10 was chosen because the application allows a *word* gap between primary and       *
                             *    secondary of up to 10 words.  This actually means that a value of verseIdx of 10 is a        *
                             *    significant over-kill - but better safe than sorry.                                          *
                             *                                                                                                 *
                             *  So, we could end up with contents as follows:                                                  *
                             *                   key: 10, value: address of primary matching verse;                            *
                             *                   key: 9,  value: address of verse before the primary;                          *
                             *                   key: 8,  value: address of verse two before primary                           *
                             *    and so on.                                                                                   *
                             *                                                                                                 *
                             *  If no secondary match is found, *all* entries in impactedVerse will be removed.                *
                             *                                                                                                 *
                             *!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
                            if (!impactedVerse.ContainsKey(verseIdx)) impactedVerse.Add(verseIdx, otherVerse);
                            while (countDown >= 0)
                            {
                                if (wordIdx < 0)
                                {
                                    otherVerse = otherVerse.PreviousVerse;
                                    if (otherVerse == null) break;
                                    if (otherVerse.BookId != currentVerse.BookId) break;
                                    wordIdx = otherVerse.WordCount - 1;
                                    if (!impactedVerse.ContainsKey(--verseIdx)) impactedVerse.Add(verseIdx, otherVerse);
                                }
                                otherWord = otherVerse.getWordBySeqNo(wordIdx);
                                if (rbOption == 1) candidateSecondary = otherWord.RootWord;
                                else candidateSecondary = otherWord.AccentlessTextWord;
                                if (String.Compare(secondaryString, candidateSecondary) == 0)
                                {
                                    isMatch = true;
                                    break;
                                }
                                wordIdx--;
                                countDown--;
                            }
                            if (!isMatch) impactedVerse.Clear();
                            // So, we've scanned backwards, now scan forward
                            otherVerse = currentVerse;
                            countDown = contextCount - 1;
                            wordIdx = jdx + 1;
                            verseIdx = 10;
                            /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*
                             *                                                                                                 *
                             *                                        impactVerse2                                             *
                             *                                        ------------                                             *
                             *                                                                                                 *
                             *  Summary of the content of impactVerse2 at the end of this step:                                *
                             *                                                                                                 *
                             *  Initially, this will contain a reference to the verse containing the primary match.  In most   *
                             *    cases, this will be the only contents of impactVerse2.  However, in the event that the       *
                             *    secondary match is in a following verse, the key (verseIdx) will be *incremented* and the    *
                             *    additional verse added.                                                                      *
                             *                                                                                                 *
                             *  verseIdx has an initial value of 10 and is increased for each later verse scanned.  The        *
                             *    value of 10 was chosen because the application allows a *word* gap between primary and       *
                             *    secondary of up to 10 words.  This actually means that a value of verseIdx of 10 is a        *
                             *    significant over-kill - but better safe than sorry.                                          *
                             *                                                                                                 *
                             *  So, we could end up with contents as follows:                                                  *
                             *                   key: 10, value: address of primary matching verse;                            *
                             *                   key: 11,  value: address of verse immediately following the primary;          *
                             *                   key: 12,  value: address of verse two after primary                           *
                             *    and so on.                                                                                   *
                             *                                                                                                 *
                             *  If no secondary match is found, *all* entries in impactedVerse will be removed.                *
                             *                                                                                                 *
                             *!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/
                            if (!impactedVerse2.ContainsKey(verseIdx)) impactedVerse2.Add(verseIdx, otherVerse);
                            while (countDown >= 0)
                            {
                                if (wordIdx >= otherVerse.WordCount)
                                {
                                    otherVerse = otherVerse.NextVerse;
                                    if (otherVerse == null) break;
                                    if (otherVerse.BookId != currentVerse.BookId) break;
                                    wordIdx = 0;
                                    if (!impactedVerse2.ContainsKey(++verseIdx)) impactedVerse2.Add(verseIdx, otherVerse);
                                }
                                otherWord = otherVerse.getWordBySeqNo(wordIdx);
                                if (rbOption == 1) candidateSecondary = otherWord.RootWord;
                                else candidateSecondary = otherWord.AccentlessTextWord;
                                if (String.Compare(secondaryString, candidateSecondary) == 0)
                                {
                                    isMatch2 = true;
                                    break;
                                }
                                wordIdx++;
                                countDown--;
                            }
                            if (!isMatch2) impactedVerse2.Clear();
                            if (isMatch || isMatch2)
                            {
                                /*!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*
                                 *                                                                                                 *
                                 *  This extra level of complexity is required to handle multiple occurrences of the primary (and, *
                                 *  possibly, secondary) word in a single verse.  See 2 Cor 1:4 as a good example and work through *
                                 *  the logic.                                                                                     *
                                 *                                                                                                 *
                                 *  The contents of both impactedVerse and impactedVerse2 are now copied into finalCollection.     *
                                 *    Because of the use of a key which decreases for impactedVerse and increases for              *
                                 *    impactedVerse2, the only potential conflict will be where the key = 10, which will be        *
                                 *    effective duplicates anyway.                                                                 *
                                 *                                                                                                 *
                                 *!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!*/

                                foreach (KeyValuePair<int, classVerseContent> interimEntry in impactedVerse)
                                {
                                    if (!finalCollection.ContainsKey(interimEntry.Key)) finalCollection.Add(interimEntry.Key, interimEntry.Value);
                                }
                                foreach (KeyValuePair<int, classVerseContent> interimEntry in impactedVerse2)
                                {
                                    if (!finalCollection.ContainsKey(interimEntry.Key)) finalCollection.Add(interimEntry.Key, interimEntry.Value);
                                }
                                isMatch = false;
                                isMatch2 = false;
                                isFinal = true;
                            }
                        }
                    }
                }
                if (isFinal)
                {
                    /**************************************************************************
                     *                                                                        *
                     *  This will now step through the results that have been stored in the   *
                     *    finalCollection.  Because of the way we have keyed the collection,  *
                     *    we can be sure that they occur in the correct sequence.  The only   *
                     *    problem is that, in reality, the key is likely to start at a value  *
                     *    of 8 or more and finish at 12 - but, in theory, they could start at *
                     *    0 and end at 20.                                                    *
                     *                                                                        *
                     **************************************************************************/
                    for (jdx = 0; jdx <= 20; jdx++)
                    {
                        currentResult = new classSearchResult();     // New
                        currentResult.BookName = currentBook.BookName;
                        if (finalCollection.ContainsKey(jdx))
                        {
                            index = jdx - 8;  // Because the zeroth stored record in currentResult is the earliest
                            if ((index < 0) || (index > 4)) continue;  // I really don't expect this to happen
                            currentResult.setStoreRecord(index);   // New
                            finalCollection.TryGetValue(jdx, out otherVerse);
                            decompRef = otherVerse.BibleReference.Split(splitParams);
                            fullDecomp = decompRef[1].Split(splitRef);
                            currentResult.addChapterNo(fullDecomp[0]);
                            currentResult.addVerseNo(fullDecomp[1]);
                            refKey = currentRef.NtOrLxxCode.ToString() + "-" + currentBook.BookId.ToString() + "-" +
                                currentBook.getSeqNoByChapterId(Convert.ToInt32(fullDecomp[0]).ToString()) + "-" +
                                currentChapter.getSeqNoByVerseId(fullDecomp[1]);   // New
                                                                                   //                            refKey = currentRef.NtOrLxxCode.ToString() + "-" + currentBook.BookId.ToString() + "-" +
                                                                                   //                                currentRef.ChapSeq.ToString() + "-" + currentRef.VerseSeq.ToString();   // New
                            noOfWords = otherVerse.WordCount;   // New
                            for (wordIdx = 0; wordIdx < noOfWords; wordIdx++)   // New
                            {   // New
                                currentWord = otherVerse.getWordBySeqNo(wordIdx);   // New
                                if (listOfPrimaryMatches.ContainsKey(refKey))   // New
                                {   // New
                                    listOfPrimaryMatches.TryGetValue(refKey, out listOfOccurrences);   // New
                                    if (listOfOccurrences.Contains(wordIdx)) isPrimaryMatch = true;   // New
                                }   // New
                                if (rbOption == 1) candidateSecondary = currentWord.RootWord;   // New
                                else candidateSecondary = currentWord.AccentlessTextWord;   // New
                                if (String.Compare(secondaryString, candidateSecondary) == 0) isSecondaryMatch = true;   // New
                                currentResult.addWord(currentWord.TextWord, isPrimaryMatch, isSecondaryMatch);   // New
                                isPrimaryMatch = false; // New
                                isSecondaryMatch = false;   // New
                            }   // New
                        }
                        currentResultLine = currentResult.getResultByLine(jdx - 8);
                        if (currentResultLine != null)
                        {
                            if (matchCount++ > 0) currentResult.IsFollowOn = true;
                            listOfResults.Add(noOfResults++, currentResult);   // New
                        }
                        //                            resultingText += currentBook.BookName + " " + decompRef[1] + "  " + otherVerse.getWholeText(idx == 0) + "\n";

                    }
                    //                    resultingText += "\n";
                    proximityCount++;
                    isFinal = false;
                }
                tempLabel1.Text = proximityCount.ToString();
                if (proximityCount == 1) tempLabel2.Text = "match";
                else tempLabel2.Text = "matches";
                tempLabel2.Text += " found so far";
                tempLabel3.Text = "Still working; currently processing:";
                tempLabel4.Text = currentBook.BookName;
                globalVars.StatStrip.Refresh();
            }
            tempLabel1.Text = proximityCount.ToString();
            if (proximityCount == 1) tempLabel2.Text = "match";
            else tempLabel2.Text = "matches";
            tempLabel2.Text += " found";
            tempLabel3.Text = "Search completed";
            if (rbOption == 1) tempLabel4.Text = "Criteria: all matches to root = " + primaryString + " within " + contextCount.ToString() + " words of " +
                    secondaryString;
            else tempLabel4.Text = "Criteria: exact matches to " + primaryString + " within " + contextCount.ToString() + " words of " + secondaryString;
            globalVars.StatStrip.Refresh();
            return resultingText;
        }

        private String createRootLine(classVerseContent thisVerse)
        {
            int idx, noOfWords;
            String returnLine;
            classWordContent currentWord;

            returnLine = "";
            noOfWords = thisVerse.WordCount;
            for (idx = 0; idx < noOfWords; idx++)
            {
                currentWord = thisVerse.getWordBySeqNo(idx);
                if (idx == 0)
                {
                    returnLine = currentWord.AccentlessTextWord + currentWord.Punctuation;
                }
                else
                {
                    returnLine += " " + currentWord.AccentlessTextWord + currentWord.Punctuation;
                }
            }
            return returnLine;
        }

        private String populateReference(String bookName, String chapNo, String vNo)
        {
            return bookName + " " + chapNo + ":" + vNo + "  ";
        }

        public void copyToSearch(TextBox targetTextBox, int code)
        {
            String returnedWord;
            Tuple<String, String, String, String> returnedData;

            /*..........................................*
             *                                          *
             *  texthandler.copyCurrentWord:            *
             *  ---------------------------             *
             *                                          *
             *  Parameters:                             *
             *    1   A code that determines whether    *
             *        the word is copied to memory or   *
             *        to notes                          *
             *         value      means                 *
             *           0         memory               *
             *           1         notes                *
             *        So, a value of 2 means that it    *
             *        will be copied to neither.        *
             *    2   A code which identifies whether   *
             *        the word is from NT or LXX        *
             *         value      means                 *
             *           0         NT                   *
             *           1         LXX                  *
             *                                          *
             *..........................................*/
            returnedWord = textHandler.copyCurrentWord(2, code, 1);
            returnedData = greekProcessing.removeNonGkChars(returnedWord);
            returnedWord = returnedData.Item4;
            targetTextBox.Text = returnedWord;
        }

        public void copyAllResults(bool isToClipboard)
        {
            RichTextBox searchResultsArea;

            searchResultsArea = globalVars.getRichtextControlByIndex(4);
            if ((searchResultsArea.Text != null) && (searchResultsArea.Text.Length > 0))
            {
                if (isToClipboard)
                {
                    Clipboard.SetText(searchResultsArea.Text);
                    MessageBox.Show("Search Results have been copied", "Copy to Clipboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    textHandler.insertTextToNotes(searchResultsArea.Text);
                }
            }
        }

        public void copySingleResult(int searchCursorPstn, bool isToClipboard)
        {
            int csrPstn, startPstn, endPstn;
            String searchResult;
            RichTextBox searchResultsArea;

            searchResultsArea = globalVars.getRichtextControlByIndex(4);
            if ((searchResultsArea.Text == null) || (searchResultsArea.Text.Length == 0)) return;
            //            csrPstn = searchResultsArea.SelectionStart;
            csrPstn = searchCursorPstn;
            if (csrPstn == 0)
            {
                startPstn = 0;
            }
            else
            {
                // Find the start of the current line
                startPstn = searchResultsArea.Text.LastIndexOf('\n', csrPstn);
                // Adjust by one to move beyond the previous new line character
                startPstn++;
            }
            if (csrPstn > searchResultsArea.Text.Length - 1)
            {
                csrPstn = searchResultsArea.Text.Length - 1;
                endPstn = csrPstn;
            }
            else
            {
                // Now find the end of the current line
                endPstn = searchResultsArea.Text.IndexOf("\n", csrPstn);
                if (endPstn == -1) endPstn = searchResultsArea.Text.Length;
                else endPstn--;
            }
            searchResult = searchResultsArea.Text.Substring(startPstn, endPstn - startPstn);
            if (isToClipboard)
            {
                Clipboard.SetText(searchResult);
                MessageBox.Show("The search Result has been copied", "Copy to Clipboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                textHandler.insertTextToNotes(searchResult);
            }

        }

        public void updateTextAreaWithSelectedChapter(int searchCursorPstn)
        {
            bool isNTBook = true;
            int idx, noOfBooks, csrPstn, startPstn, endPstn;
            String refString, chapAndVerse, verseAlone, bookName;
            SortedDictionary<int, classBookContent> listOfNTBooks, listOfLXXBooks;
            classBookContent currentBook;
            RichTextBox mainTextArea, searchResultsArea;

            mainTextArea = globalVars.getRichtextControlByIndex(0);
            searchResultsArea = globalVars.getRichtextControlByIndex(4);
            if ((searchResultsArea.Text == null) || (searchResultsArea.Text.Length == 0)) return;
            // Retrieve the current "caret" position
            //            searchResultsArea.Focus();
            //            csrPstn = searchResultsArea.SelectionStart;
            csrPstn = searchCursorPstn;
            if (csrPstn > 0)
            {
                // Find the start of the current line
                startPstn = searchResultsArea.Text.LastIndexOf('\n', csrPstn);
                // Adjust by one to move beyond the previous new line character
                startPstn++;
                // Now find the marker of the end of the reference
                endPstn = searchResultsArea.Text.IndexOf("  ", startPstn);
                if (endPstn == -1) endPstn = searchResultsArea.Text.Length;
                // So, get the reference = it will be of the form: <book name> <chapter>.<verse>
                refString = searchResultsArea.Text.Substring(startPstn, endPstn - startPstn);
                endPstn = refString.LastIndexOf(' ');
                // So, a single space seperates the book name from chapter and verse
                chapAndVerse = refString.Substring(endPstn + 1);
                // refString becomes = book name
                refString = refString.Substring(0, endPstn);
                listOfNTBooks = textHandler.ListOfNTBooks;
                listOfLXXBooks = textHandler.ListOfLxxBooks;
                noOfBooks = listOfNTBooks.Count;
                idx = 0;
                while (idx < noOfBooks)
                {
                    listOfNTBooks.TryGetValue(idx, out currentBook);
                    if (currentBook == null)
                    {
                        idx++;
                        continue;
                    }
                    bookName = currentBook.BookName;
                    if (String.Compare(bookName, refString) == 0) break;
                    idx++;
                }
                if (idx >= noOfBooks)
                {
                    // The book is not in the NT.  Now try LXX
                    isNTBook = false;
                    noOfBooks = listOfLXXBooks.Count;
                    idx = 0;
                    while (idx < noOfBooks)
                    {
                        listOfLXXBooks.TryGetValue(idx, out currentBook);
                        if (currentBook == null)
                        {
                            idx++;
                            continue;
                        }
                        bookName = currentBook.BookName;
                        if (String.Compare(bookName, refString) == 0) break;
                        idx++;
                    }
                    if (idx > noOfBooks)
                    {
                        // We still haven't found it - it's nowhere to be found
                        return;
                    }
                }
                /*                switch (globalVars.DisplayTextCode)
                                {
                                    case 0:
                                    case 3: break;
                                    case 1: idx -= textHandler.NoOfNTBooks; break;
                                    case 2:
                                        if (idx > textHandler.NoOfNTBooks) idx -= textHandler.NoOfNTBooks;
                                        else idx += textHandler.NoOfNTBooks - textHandler.NoOfNTBooks; break;     // !!!
                                } */
                endPstn = chapAndVerse.IndexOf('.');
                verseAlone = chapAndVerse.Substring(endPstn + 1);
                // Actually chapAndVerse becomes simply chap
                chapAndVerse = chapAndVerse.Substring(0, endPstn);
                if (isNTBook) textHandler.displayNTChapter(idx, chapAndVerse, true);
                else textHandler.displayLXXChapter(idx, chapAndVerse, true);
            }
        }

        public classSearchResult getSearchResultByIndex(int index)
        {
            classSearchResult currentResult = null;

            if (index < noOfResults) listOfResults.TryGetValue(index, out currentResult);
            return currentResult;
        }
    }
}
