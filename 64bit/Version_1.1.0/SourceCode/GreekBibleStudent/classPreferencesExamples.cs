using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    internal class classPreferencesExamples
    {
        /*==========================================================================================*
         *                                                                                          *
         *                               classPreferencesExamples                                   *
         *                               ========================                                   *
         *                                                                                          *
         *  Stores the text on a word-by-word basis for the various examples.  The nature of each   *
         *    word (whether it is a verse number or reference, main text, primary or secondary      *
         *    matching word) is identified by typeCode.  Its value is as follows:                   *
         *                                                                                          *
         *  typeCode     Value                         Meaning                                      *
         *               =====                         =======                                      *
         *                 1       The text is in the "simple" font                                 *
         *                 2       The text is the "main"  font                                     *
         *                 3       Primary match                                                    *
         *                 4       Secondary match                                                  *
         *                 5       End of entry but the next entry is linked                        *
         *                 6       End of entry and the next is _not_ linked                        *
         *                                                                                          *
         *                 Values 5 and 6 are only used in the preferences module.                  *
         *                                                                                          *
         *==========================================================================================*/

        int typeCode;
        String text;

        public int TypeCode { get => typeCode; set => typeCode = value; }
        public string Text { get => text; set => text = value; }
    }
}
