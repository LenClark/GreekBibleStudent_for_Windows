using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    public class classVerseContent
    {
        int wordCount = 0, bookId;
        String noteText = "", bibleReference;
        SortedDictionary<int, classWordContent> wordIndex = new SortedDictionary<int, classWordContent>();
        classVerseContent previousVerse, nextVerse;

        public int WordCount { get => wordCount; }
        public int BookId { get => bookId; set => bookId = value; }
        public string NoteText { get => noteText; set => noteText = value; }
        public string BibleReference { get => bibleReference; set => bibleReference = value; }
        public classVerseContent PreviousVerse { get => previousVerse; set => previousVerse = value; }
        public classVerseContent NextVerse { get => nextVerse; set => nextVerse = value; }

        public classWordContent addWordToVerse()
        {
            classWordContent newWord;

            newWord = new classWordContent();
            wordIndex.Add(wordCount++, newWord);
            return newWord;
        }

        public void setBibleReference(String bookName, String Chapter, String Verse)
        {
            bibleReference = bookName + " " + Chapter + ":" + Verse;
        }

        public classWordContent getWordBySeqNo(int seqNo)
        {
            classWordContent newWord;

            wordIndex.TryGetValue(seqNo, out newWord);
            return newWord;
        }

        public String getWholeText(bool isNT)
        {
            int idx;
            String wholeText, singleWord, punctuationItem, preWord, postWord;
            classWordContent currentWord;

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
