using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    internal class classPrimaryResult
    {
        /*================================================================================================================*
         *                                                                                                                *
         *                                               classSearchResult                                                *
         *                                               =================                                                *
         *                                                                                                                *
         *  This class is specific to the situation where a single word is being sought (i.e. no secondary word is        *
         *    involved.                                                                                                   *
         *                                                                                                                *
         *  Instance members:                                                                                             *
         *  ================                                                                                              *
         *                                                                                                                *
         *  isRepeatInVerse   Not clear of the significance of this variable                                              *
         *  bookId            The normal reference to the book from which the verse comes                                 *
         *  chapReference     The meaningful chapter number of the reference.                                             *
         *  chapSeq           The internal (meaningless) sequence                                                         *
         *  verseReference    The verse number                                                                            *
         *  verseSeq          The internal sequence                                                                       *
         *  noOfMatchingWords The number of words in the verse that match the search word.  (Probably not used.)          *
         *  impactedVerse     The verse itself (i.e. the address of the verse instance.)                                  *
         *                                                                                                                *
         *  matchingWordPositions    Relates to noOfMatchingWords, above                                                  *
         *                Key    A simple sequence, to allow enumeration                                                  *
         *                Value  The position in the verse (which allows retrieval by sequence)                           *
         *                                                                                                                *
         *================================================================================================================*/

        bool isRepeatInVerse = false;
        int bookId, chapSeq, verseSeq, noOfMatchingWords;
        String chapReference, verseReference;
        SortedList<int, int> matchingWordPositions = new SortedList<int, int>();
        classVerse impactedVerse;

        public bool IsRepeatInVerse { get => isRepeatInVerse; set => isRepeatInVerse = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public int ChapSeq { get => chapSeq; set => chapSeq = value; }
        public int VerseSeq { get => verseSeq; set => verseSeq = value; }
        public int NoOfMatchingWords { get => noOfMatchingWords; set => noOfMatchingWords = value; }
        public string ChapReference { get => chapReference; set => chapReference = value; }
        public string VerseReference { get => verseReference; set => verseReference = value; }
        public SortedList<int, int> MatchingWordPositions { get => matchingWordPositions; set => matchingWordPositions = value; }
        internal classVerse ImpactedVerse { get => impactedVerse; set => impactedVerse = value; }

        public void addWordPosition(int position)
        {
            matchingWordPositions.Add(noOfMatchingWords++, position);
        }

        public int getWordPositionBySeq(int index)
        {
            int retrievedPosition = -1;

            if (matchingWordPositions.ContainsKey(index)) matchingWordPositions.TryGetValue(index, out retrievedPosition);
            return retrievedPosition;
        }
    }
}
