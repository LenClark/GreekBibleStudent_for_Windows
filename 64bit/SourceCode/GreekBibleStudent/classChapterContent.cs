using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    public class classChapterContent
    {
        int noOfVersesInChapter = 0, bookId;
        String chapterId;
        /*--------------------------------------------------------------------------------*
         *                                                                                *
         *                                Storage of Verses                               *
         *                                -----------------                               *
         *                                                                                *
         *  Verses, as seen by users, will be stored as text (strings) in much the same   *
         *    way as chapters (see the relevant comment in Books).  This will enable us   *
         *    to use (e.g) names in lieu of verses - or even a null string.               *
         *                                                                                *
         *  To facilitate sensible access to Verses we will also identify them by a       *
         *    non-visible sequence number.  So:                                           *
         *                                                                                *
         *  versesInChapter: a look-up list of verse instances, keyed by a sequence no.   *
         *  verseLookup:     the text-based, visible verse associated with the same       *
         *                   sequence key as used in versesInChapter                      *
         *  verseSequence:   a look-up list keyed on the meaningful, text-based verse     *
         *                   which returns the associated sequence number                 *
         *                                                                                *
         *--------------------------------------------------------------------------------*/
        SortedDictionary<int, classVerseContent> versesInChapter = new SortedDictionary<int, classVerseContent>();
        SortedDictionary<int, String> verseLookup = new SortedDictionary<int, String>();
        SortedDictionary<String, int> verseSequence = new SortedDictionary<string, int>();
        classChapterContent previousChapter, nextChapter;

        public int NoOfVersesInChapter { get => noOfVersesInChapter; }
        public int BookId { get => bookId; set => bookId = value; }
        public String ChapterId { get => chapterId; set => chapterId = value; }
        public classChapterContent PreviousChapter { get => previousChapter; set => previousChapter = value; }
        public classChapterContent NextChapter { get => nextChapter; set => nextChapter = value; }

        public classVerseContent addVerseToChapter(String verseId)
        {
            classVerseContent newVerse;

            newVerse = new classVerseContent();
            versesInChapter.Add(noOfVersesInChapter, newVerse);
            verseLookup.Add(noOfVersesInChapter, verseId);
            verseSequence.Add(verseId, noOfVersesInChapter++);
            return newVerse;
        }

        public classVerseContent getVerseBySeqNo(int seqNo)
        {
            classVerseContent newVerse;

            versesInChapter.TryGetValue(seqNo, out newVerse);
            return newVerse;
        }

        public classVerseContent getVerseByVerseNo(String verseId)
        {
            int seqNo;

            verseSequence.TryGetValue(verseId, out seqNo);
            return getVerseBySeqNo(seqNo);
        }

        public String getVerseIdBySeqNo(int seqNo)
        {
            String verseId = "";

            verseLookup.TryGetValue(seqNo, out verseId);
            return verseId;
        }

        public String getSeqNoByVerseId(String inVerse)
        {
            int vId = -1;

            verseSequence.TryGetValue(inVerse, out vId);
            return vId.ToString();
        }
    }
}
