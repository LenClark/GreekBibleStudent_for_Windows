using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    class classSearchResultLine
    {
        bool hasSecondaryMatch = false;
        int noOfWords = 0, noOfPrimaryMatches = 0, noOfSecondaryMatches = 0;
        String chapterNo, verseNo;
        SortedList<int, String> wordsInOrder = new SortedList<int, string>();
        List<int> primaryWordPosition = new List<int>();
        List<int> secondaryWordPosition = new List<int>();

        public bool HasSecondaryMatch { get => hasSecondaryMatch; set => hasSecondaryMatch = value; }
        public int NoOfWords { get => noOfWords; set => noOfWords = value; }
        public int NoOfPrimaryMatches { get => noOfPrimaryMatches; set => noOfPrimaryMatches = value; }
        public int NoOfSecondaryMatches { get => noOfSecondaryMatches; set => noOfSecondaryMatches = value; }
        public string ChapterNo { get => chapterNo; set => chapterNo = value; }
        public string VerseNo { get => verseNo; set => verseNo = value; }

        public void addWord(String inWord)
        {
            wordsInOrder.Add(noOfWords++, inWord);
        }

        public String getWordByIndex(int index)
        {
            String requiredWord = "";

            if (index < noOfWords) wordsInOrder.TryGetValue(index, out requiredWord);
            return requiredWord;
        }

        public void addPrimaryMatch(int wordPosition)
        {
            if (!primaryWordPosition.Contains(wordPosition))
            {
                primaryWordPosition.Add(wordPosition);
                noOfPrimaryMatches++;
            }
        }

        public void addSecondaryMatch(int wordPosition)
        {
            if (!secondaryWordPosition.Contains(wordPosition))
            {
                secondaryWordPosition.Add(wordPosition);
                noOfSecondaryMatches++;
            }
        }

        public int getPrimaryMatchByIndex(int index)
        {
            if (index < noOfPrimaryMatches) return primaryWordPosition[index];
            return -1;
        }

        public int getSecondaryMatchByIndex(int index)
        {
            if (index < noOfSecondaryMatches) return secondaryWordPosition[index];
            return -1;
        }
    }
}
