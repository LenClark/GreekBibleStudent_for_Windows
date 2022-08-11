using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    public class classSearchVerse
    {
        /*===========================================================================================================*
         *                                                                                                           *
         *                                                classSearchVerse                                           *
         *                                                ================                                           *
         *                                                                                                           *
         *  A list of up to five verses can be defined as a SearchResult.  This record gives the following           *
         *    information:                                                                                           *
         *                                                                                                           *
         *  Variables:                                                                                               *
         *  =========                                                                                                *
         *                                                                                                           *
         *  isFollowed            This is set to true if the next verse is part of the match                         *
         *  chapterSeq            The sequence of the chapter in the book                                            *
         *  verseSeq              The sequenbce of the verse                                                         *
         *  noOfMatchingWords     Used to control the list of matchingWordPositions                                  *
         *  matchingWordPosition  The sequence value of the word matching the primary search word, if it exists      *
         *  impactedVerse         The instance of the verse, from which each word can be derived                     *
         *  matchingWordPositions The sequence numbewr of any primary or secondary matches in the verse              *
         *  matchingWordType      Indicates whether the match is Primary or secondary:                               *
         *                          Key:   The same integer index as used in matchingWordPositions                   *
         *                          Value: 1 = Primary, 2 = Secondary                                                *
         *                                                                                                           *
         *  Created by Len Clark                                                                                     *
         *  July 2022                                                                                                *
         *                                                                                                           *
         *===========================================================================================================*/
        bool isFollowed;
        int bookId, chapterSeq, verseSeq, noOfMatchingWords;
        String chapterReference, verseReference;
        /*--------------------------------------------------------------*
         *  matchingWordPositions:                                      *
         *  ---------------------                                       *
         *  Key:   NSInteger sequence                                   *
         *  Value: NSInteger position in verse                          *
         *--------------------------------------------------------------*/
        SortedList<int, int> matchingWordPositions = new SortedList<int, int>();
        /*--------------------------------------------------------------*
         *  matchingWordType:                                           *
         *  ----------------                                            *
         *  Key:   NSInteger sequence                                   *
         *  Value: NSInteger type code                                  *
         *--------------------------------------------------------------*/
        SortedList<int, int> matchingWordType = new SortedList<int, int>();

        classGlobal globalVars;
        classVerse impactedVerse;

        public bool IsFollowed { get => isFollowed; set => isFollowed = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public int ChapterSeq { get => chapterSeq; set => chapterSeq = value; }
        public int VerseSeq { get => verseSeq; set => verseSeq = value; }
        public int NoOfMatchingWords { get => noOfMatchingWords; set => noOfMatchingWords = value; }
        public string ChapterReference { get => chapterReference; set => chapterReference = value; }
        public string VerseReference { get => verseReference; set => verseReference = value; }
        public classVerse ImpactedVerse { get => impactedVerse; set => impactedVerse = value; }
        public SortedList<int, int> MatchingWordType { get => matchingWordType; set => matchingWordType = value; }
        public SortedList<int, int> MatchingWordPositions { get => matchingWordPositions; set => matchingWordPositions = value; }

        public classSearchVerse(classGlobal inGlobal)
        {
            globalVars = inGlobal;
            isFollowed = false;
            noOfMatchingWords = 0;
        }

        public void addWordPosition(int position, int wordType)
        {
            matchingWordPositions.Add(noOfMatchingWords, position);
            matchingWordType.Add(noOfMatchingWords++, wordType);
        }

        public int getWordPositionBySeq(int index)
        {
            int retrievedPosition = -1;

            matchingWordPositions.TryGetValue(index, out retrievedPosition);
            return retrievedPosition;
        }

        public int getWordTypeBySeq(int index)
        {
            int retrievedType = -1;

            matchingWordType.TryGetValue(index, out retrievedType);
            return retrievedType;
        }
    }
}
