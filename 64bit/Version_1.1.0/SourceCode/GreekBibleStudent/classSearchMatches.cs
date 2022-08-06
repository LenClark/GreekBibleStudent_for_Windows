using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    internal class classSearchMatches
    {
        bool isValid = true;
        int bookId, primaryChapterSeq, primaryVerseSeq, primaryWordSeq, secondaryChapterSeq, secondaryVerseSeq, secondaryWordSeq;
        String primaryChapterRef, primaryVerseRef, secondaryChapterRef, secondaryVerseRef;
        classWord primaryScanWord, secondaryScanWord;

        public bool IsValid { get => isValid; set => isValid = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public int PrimaryChapterSeq { get => primaryChapterSeq; set => primaryChapterSeq = value; }
        public int PrimaryVerseSeq { get => primaryVerseSeq; set => primaryVerseSeq = value; }
        public int PrimaryWordSeq { get => primaryWordSeq; set => primaryWordSeq = value; }
        public int SecondaryChapterSeq { get => secondaryChapterSeq; set => secondaryChapterSeq = value; }
        public int SecondaryVerseSeq { get => secondaryVerseSeq; set => secondaryVerseSeq = value; }
        public int SecondaryWordSeq { get => secondaryWordSeq; set => secondaryWordSeq = value; }
        public string PrimaryChapterRef { get => primaryChapterRef; set => primaryChapterRef = value; }
        public string PrimaryVerseRef { get => primaryVerseRef; set => primaryVerseRef = value; }
        public string SecondaryChapterRef { get => secondaryChapterRef; set => secondaryChapterRef = value; }
        public string SecondaryVerseRef { get => secondaryVerseRef; set => secondaryVerseRef = value; }
        public classWord PrimaryScanWord { get => primaryScanWord; set => primaryScanWord = value; }
        public classWord SecondaryScanWord { get => secondaryScanWord; set => secondaryScanWord = value; }
    }
}
