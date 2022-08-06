using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    class classReference
    {
        int ntOrLxxCode, bookId, chapSeq, verseSeq, noOfMatches = 0;

        public int NtOrLxxCode { get => ntOrLxxCode; set => ntOrLxxCode = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public int ChapSeq { get => chapSeq; set => chapSeq = value; }
        public int VerseSeq { get => verseSeq; set => verseSeq = value; }
        public int NoOfMatches { get => noOfMatches; set => noOfMatches = value; }

        public void insertReference(int oldOrNew, int bookCode, int chapCode, int vCode)
        {
            ntOrLxxCode = oldOrNew;
            bookId = bookCode;
            chapSeq = chapCode;
            verseSeq = vCode;
            noOfMatches++;
        }
    }
}
