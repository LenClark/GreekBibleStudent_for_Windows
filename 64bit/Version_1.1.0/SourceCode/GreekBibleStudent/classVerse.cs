using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    public class classVerse
    {
        int wordCount = 0, bookId, chapSeq, verseSeq;
        String noteText = "", bibleReference, chapRef, verseRef;
        SortedDictionary<int, classWord> wordIndex = new SortedDictionary<int, classWord>();
        classVerse previousVerse, nextVerse;

        public int WordCount { get => wordCount; }
        public int BookId { get => bookId; set => bookId = value; }
        public int ChapSeq { get => chapSeq; set => chapSeq = value; }
        public int VerseSeq { get => verseSeq; set => verseSeq = value; }
        public string NoteText { get => noteText; set => noteText = value; }
        public string BibleReference { get => bibleReference; set => bibleReference = value; }
        public string ChapRef { get => chapRef; set => chapRef = value; }
        public string VerseRef { get => verseRef; set => verseRef = value; }
        public classVerse PreviousVerse { get => previousVerse; set => previousVerse = value; }
        public classVerse NextVerse { get => nextVerse; set => nextVerse = value; }

        public classWord addWordToVerse()
        {
            classWord newWord;

            newWord = new classWord();
            wordIndex.Add(wordCount++, newWord);
            return newWord;
        }

        public void setBibleReference(String bookName, String Chapter, String Verse)
        {
            bibleReference = bookName + " " + Chapter + ":" + Verse;
        }

        public classWord getWordBySeqNo(int seqNo)
        {
            classWord newWord;

            if (seqNo < wordCount)
            {
                wordIndex.TryGetValue(seqNo, out newWord);
                return newWord;
            }
            return null;
        }

        public String getWholeText(bool isNT)
        {
            int idx;
            String wholeText, singleWord, punctuationItem, preWord, postWord;
            classWord currentWord;

            wholeText = "";
            for (idx = 0; idx < wordCount; idx++)
            {
                wordIndex.TryGetValue(idx, out currentWord);
                if (currentWord == null) continue;
                singleWord = currentWord.TextWord;
                preWord = currentWord.PreWordChars;
                if (preWord == null) preWord = "";
                if (isNT) postWord = "";
                else
                {
                    postWord = currentWord.PostWordChars;
                    if (postWord == null) postWord = "";
                }
                punctuationItem = currentWord.Punctuation;
                if (punctuationItem == null) punctuationItem = "";
                if ((singleWord != null) && (singleWord.Length > 0))
                {
                    if ((wholeText == null) || (wholeText.Length == 0))
                    {
                        wholeText = preWord + singleWord + postWord + punctuationItem;
                    }
                    else
                    {
                        wholeText += " " + preWord + singleWord + postWord + punctuationItem;
                    }
                }
            }
            return wholeText;
        }
    }
}
