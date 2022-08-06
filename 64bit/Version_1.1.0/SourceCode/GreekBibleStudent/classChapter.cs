using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    public class classChapter
    {
        /*============================================================================================================*
         *                                                                                                            *
         *                                              classChapter                                                  *
         *                                              ------------                                                  *
         *                                                                                                            *
         *  In essence, we want to identify each verse that belongs to a given chapter.  At root, information about   *
         *    the verse is provided by the class classVerse.  However, we need to cater for the possibility that the  *
         *    verse number is _not_ a simple integer but may contain alphanumerics (e.g. 12a).  So, we                *
         *    a) key the list of class instances on a sequential integer.  The sequence has no significance other     *
         *       than ensuring uniqueness.  (It will actually be generated in the sequence the verses are encountered *
         *       in the source data.)                                                                                 *
         *    b) we separately provide a lookup of this sequence number which gives the String-based version of the   *
         *       verse "number" (which we will call a verse reference"), which is recorded in the sequence listed in  *
         *       the source data.  This means that numbers may also be out of strict sequence.                        *
         *    c) we also provide an inverse lookup that allows us to find the sequence number, if we know the string- *
         *       based verse reference.  This is important because it allows us to find the verse details (the class  *
         *       instance) from the verse reference provided by the source data.                                      *
         *                                                                                                            *
         *  Note: 1. verse sequences will be zero-based (i.e. start at zero) while  verse references are (ostensibly) *
         *           meaningful and normally start at 1 (although they can be zero).                                  *
         *        2. noOfVersesInChapter will count the *sequence* of verses                                          *
         *                                                                                                            *
         *============================================================================================================*/

        int noOfVersesInChapter = 0, bookId, chapterNo;
        String chapterRef;


        /*=========================================================================================================================*
         *                                                                                                                         *
         *                                                  versesBySequence                                                       *
         *                                                  ----------------                                                       *
         *                                                                                                                         *
         *  A look-up list of verse class instances, keyed by a sequence no.                                                       *
         *      Key:   verse Sequence                                                                                              *
         *      Value: the class instance address                                                                                  *
         *                                                                                                                         *
         *=========================================================================================================================*/
        SortedDictionary<int, classVerse> versesBySequence = new SortedDictionary<int, classVerse>();

        /*=========================================================================================================================*
         *                                                                                                                         *
         *                                               verseReferenceBySequence                                                  *
         *                                               ------------------------                                                  *
         *                                                                                                                         *
         *  A list that will convert the simple verse sequence to the verse "reference", as given in the data.                     *
         *      Key:   verse sequence                                                                                              *
         *      Value: the verse number provided from data                                                                         *
         *                                                                                                                         *
         *=========================================================================================================================*/
        SortedDictionary<int, String> verseReferenceBySequence = new SortedDictionary<int, String>();

        /*=========================================================================================================================*
         *                                                                                                                         *
         *                                              sequenceForVerseReference                                                  *
         *                                              -------------------------                                                  *
         *                                                                                                                         *
         *  A reverse lookup to verseReferenceBySequence - i.e. given a data verse reference, this will give us the internal       *
         *    sequence number                                                                                                      *
         *      Key:   verse number (from data)                                                                                    *
         *      Value: verse sequence                                                                                              *
         *                                                                                                                         *
         *=========================================================================================================================*/
        SortedDictionary<String, int> sequenceForVerseReference = new SortedDictionary<String, int>();
        
        classChapter previousChapter, nextChapter;

        public int NoOfVersesInChapter { get => noOfVersesInChapter; }
        public int BookId { get => bookId; set => bookId = value; }
        public int ChapterNo { get => chapterNo; set => chapterNo = value; }
        public String ChapterRef { get => chapterRef; set => chapterRef = value; }
        public classChapter PreviousChapter { get => previousChapter; set => previousChapter = value; }
        public classChapter NextChapter { get => nextChapter; set => nextChapter = value; }

        public classVerse addVerseToChapter(String newVerseRef)
        {
            int seqNo = -1;
            classVerse newVerse;

            if (sequenceForVerseReference.ContainsKey(newVerseRef))
            {
                sequenceForVerseReference.TryGetValue(newVerseRef, out seqNo);
                versesBySequence.TryGetValue(seqNo, out newVerse);
            }
            else
            {
                newVerse = new classVerse();
                sequenceForVerseReference.Add(newVerseRef, noOfVersesInChapter);
                verseReferenceBySequence.Add(noOfVersesInChapter, newVerseRef);
                versesBySequence.Add(noOfVersesInChapter++, newVerse);
            }
            return newVerse;
        }

        public classVerse getVerseBySeqNo(int seqNo)
        {
            classVerse newVerse = null;

            versesBySequence.TryGetValue(seqNo, out newVerse);
            return newVerse;
        }

        public classVerse getVerseByVerseRef(String newVerseRef)
        {
            int seqNo;

            sequenceForVerseReference.TryGetValue(newVerseRef, out seqNo);
            if (seqNo == -1) return null;
            return getVerseBySeqNo(seqNo);
        }

        public String getVerseRefBySeqNo(int seqNo)
        {
            String newVerseRef = "";

            verseReferenceBySequence.TryGetValue(seqNo, out newVerseRef);
            return newVerseRef;
        }

        public int getSeqNoByVerseRef(String inVerseRef)
        {
            int vId = -1;

            sequenceForVerseReference.TryGetValue(inVerseRef, out vId);
            return vId;
        }
    }
}
