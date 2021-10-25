using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    class classSearchResult
    {
        /**********************************************************************
         *                                                                    *
         *                        classSearchResult                           *
         *                        =================                           *
         *                                                                    *
         *  Originally, the results of the search (simple or complex) were    *
         *    simply stored in a String, which was returned to the main       *
         *    method and displayed in the results richtextbox.  The purpose   *
         *    of this class is to store the contents of that text in a more   *
         *    accessible format.  This will allow the colour coding of        *
         *    primary and secondary matches.                                  *
         *                                                                    *
         *  This class will be used by both simple and complex scans.  For    *
         *    complex scans, the secondary match may be in a preceding or     *
         *    following verse, so we need to cater for the situation where    *
         *    the result spreads over two verses.  In fact, in extreme (but   *
         *    unlikely) situations, the spread may be over *three* verses.    *
         *    Since this may involve either the two verses *before* the main  *
         *    occurrence (i.e. the verse containing the primary match) or the *
         *    two verses after it, we need to provide five                    *
         *    "SearchResultLines".  In simple scans, only the middle of the   *
         *    five verses will ever be used.                                  *
         *                                                                    *
         *  Parameters:                                                       *
         *  ==========                                                        *
         *                                                                    *
         *  currentRecordIndex  Value             Significance                *
         *                        2    The verse containing the primary match *
         *                        1    The immediately preceding verse        *
         *                        0    The verse before that                  *
         *                        3    The verse immediately *following* the  *
         *                               primary match                        *
         *                        4    The verse following that               *
         *  bookName            Occurs here rather than in the line detail    *
         *                        because all matches must be in a single     *
         *                        book                                        *
         *  searchResultStore   A lookup list of lines by index               *
         *  currentLineReference  The current line - retained across accesses *
         *                                                                    *
         **********************************************************************/

        bool isFollowOn = false;
        int currentRecordIndex = -1;
        String bookName;
        SortedList<int, classSearchResultLine> searchResultStore = new SortedList<int, classSearchResultLine>();
        classSearchResultLine currentLineReference = null;

        public string BookName { get => bookName; set => bookName = value; }
        public bool IsFollowOn { get => isFollowOn; set => isFollowOn = value; }

        public void setStoreRecord(int lineNo)
        {
            if (!searchResultStore.ContainsKey(lineNo))
            {
                currentLineReference = new classSearchResultLine();
                searchResultStore.Add(lineNo, currentLineReference);
            }
            else
            {
                searchResultStore.TryGetValue(lineNo, out currentLineReference);
            }
            currentRecordIndex = lineNo;
        }

        public void addChapterNo(String inChap)
        {
            currentLineReference.ChapterNo = inChap;
        }

        public void addVerseNo(String inVerse)
        {
            currentLineReference.VerseNo = inVerse;
        }

        public void addWord(String inWord, bool isPrimary, bool isSecondary)
        {
            if (isPrimary) currentLineReference.addPrimaryMatch(currentLineReference.NoOfWords);
            if (isSecondary)
            {
                currentLineReference.addSecondaryMatch(currentLineReference.NoOfWords);
                currentLineReference.HasSecondaryMatch = true;
            }
            currentLineReference.addWord(inWord);
        }

        public classSearchResultLine getResultByLine(int lineNo)
        {
            classSearchResultLine currentLine = null;

            if (searchResultStore.ContainsKey(lineNo)) searchResultStore.TryGetValue(lineNo, out currentLine);
            return currentLine;
        }
    }
}
