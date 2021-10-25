using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    public class classBookContent
    {
        bool isNT;
        int noOfChaptersInBook = 0, bookId;
        String bookName, shortName, fileName;
        /*--------------------------------------------------------------------------------*
         *                                                                                *
         *                               Storage of Chapters                              *
         *                               -------------------                              *
         *                                                                                *
         *  Chapters occur in a specific sequence, which is provided to us as a sequence  *
         *    of similarly named (or numbered chapters).  Because chapters are somethimes *
         *    not strictly logical in LXX we will adopt the following strategy:           *
         *    a) we will handle Proverbs differently from other books;                    *
         *    b) as a consequence, we will store a bookId when we create the instance;    *
         *    c) these will be as follows:                                                *
         *         -1   all NT books will have this value;                                *
         *         0-58 the LXX book id (the filenumber - 1)                              *
         *         28   will indicate the book is Proverbs and must be handled            *
         *                differently                                                     *
         *                                                                                *
         *  chaptersInBook:  a look-up list of chapter instances, keyed by a sequence no. *
         *  chapterLookup:   the text-based, visible chapter associated with the same     *
         *                   sequence key as used in chaptersInBook                       *
         *  chapterSequence: a look-up list keyed on the meaningful, text-based chapter   *
         *                   which returns the associated sequence number                 *
         *                                                                                *
         *--------------------------------------------------------------------------------*/
        SortedDictionary<int, classChapterContent> chaptersInBook = new SortedDictionary<int, classChapterContent>();
        SortedDictionary<int, String> chapterLookup = new SortedDictionary<int, String>();
        SortedDictionary<String, int> chapterSequence = new SortedDictionary<String, int>();
        /*--------------------------------------------------------------------------------*
         *                                                                                *
         *                              secondLowerBound                                  *
         *                              ----------------                                  *
         *                                                                                *
         *  Key:   the chapter, as provided within the text.                              *
         *  Value: an array of all verse values that are found in the *second* occurrence *
         *         of the chapter.                                                        *
         *                                                                                *
         *--------------------------------------------------------------------------------*/
        SortedDictionary<String, String[]> secondLowerBound = new SortedDictionary<String, String[]>();

        public bool IsNT { get => isNT; set => isNT = value; }
        public int NoOfChaptersInBook { get => noOfChaptersInBook; }
        public int BookId { get => bookId; set => bookId = value; }
        public string BookName { get => bookName; set => bookName = value; }
        public string ShortName { get => shortName; set => shortName = value; }
        public string FileName { get => fileName; set => fileName = value; }

        public classBookContent()
        {
            String[] chap24 = { "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34" };
            String[] chap30 = { "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33" };
            String[] chap31 = { "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
                                "30", "31" };

            secondLowerBound.Add("24", chap24);
            secondLowerBound.Add("30", chap30);
            secondLowerBound.Add("31", chap31);
        }

        public classChapterContent addNewChapterToBook(String chapterId)
        {
            /*************************************************************************************
             *                                                                                   *
             *                               addNewChapterToBook                                 *
             *                               ===================                                 *
             *                                                                                   *
             *  This method will:                                                                *
             *  a) create a new instance of the chapter;
             *  b) accept the text chapter designation of the source file (= chapterId);         *
             *  c) generate a sequential chapter reference number (= noOfChaptersInBook);        *
             *  d) add the chapter instance to a List, keyed on the sequential value;            *
             *  e) add the text chapter designation to a list, keyed by the sequential value;    *
             *  f) create a reverse reference list to the sequential value so that we can        *
             *     retrieve the sequence number if we know the chapter designation               *
             *                                                                                   *
             *  This approach has been used because the LXX sometimes has some non-sequential    *
             *    chapters (and even some chapters that aren't actual numbers).  However, there  *
             *    is a problem: in Proverbs, Rahlfs chapter designation is as follows:           *
             *        1 - 24 (v22e), 30 (vv 1-14), 24 again (vv 23-34), 30 again (vv 15-33),     *
             *        31 (vv 1-9), 32 - 36, 31 (vv 11-31).                                       *
             *    As a result, the reverse list (keyed on the chapter designation) is not unique *
             *    for chapters 24, 30 and 31.                                                    *
             *                                                                                   *
             *  To enable this, we have added a wierd extra:                                     *
             *    occurrenceList will be keyed on the chapter reference with a value of the      *
             *    last occurrence of the chapter.  So, most will have a value of 0 but, for      *
             *    example, the second time we hit 24, the occurrence will be adjusted up to 1.   *
             *    chapterSequence will then *not* be keyed on the chapter designation alone but  *
             *    a concatenation of chapter designation and occurrence (seperated by "-").      *
             *    This will ensure uniqueness.  Retrieving the chapter designation will          *
             *    require the removal of this occurrence value.                                  *
             *                                                                                   *
             *************************************************************************************/

            String substituteChapterId;
            classChapterContent newChapter;

            newChapter = new classChapterContent();
            chaptersInBook.Add(noOfChaptersInBook, newChapter);
            if (bookId == 28)
            {
                substituteChapterId = chapterId;
                if (chapterSequence.ContainsKey(chapterId))
                {
                    substituteChapterId += "b";
                }
                chapterLookup.Add(noOfChaptersInBook, substituteChapterId);
                chapterSequence.Add(substituteChapterId, noOfChaptersInBook++);
            }
            else
            {
                chapterLookup.Add(noOfChaptersInBook, chapterId);
                chapterSequence.Add(chapterId, noOfChaptersInBook++);
            }
            return newChapter;
        }

        public classChapterContent getChapterBySeqNo(int seqNo)
        {
            classChapterContent newChapter;

            chaptersInBook.TryGetValue(seqNo, out newChapter);
            return newChapter;
        }

        public classChapterContent getChapterByChapterId(String chapterId)
        {
            int seqNo;


            chapterSequence.TryGetValue(chapterId, out seqNo);
            return getChapterBySeqNo(seqNo);
        }

        public int getSeqNoByChapterId(String chapterId)
        {
            int seqNo;

            chapterSequence.TryGetValue(chapterId, out seqNo);
            return seqNo;
        }

        public String getChapterIdBySeqNo(int seqNo)
        {
            String chapterId = "";

            chapterLookup.TryGetValue(seqNo, out chapterId);
            return chapterId;
        }
    }
}
