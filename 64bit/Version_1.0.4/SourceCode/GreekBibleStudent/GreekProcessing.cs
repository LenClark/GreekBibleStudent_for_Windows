using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    public class GreekProcessing
    {
        /***************************************************************************
         *                                                                         *
         *                       classGreekProcessing                              *
         *                       ====================                              *
         *                                                                         *
         *  All manipulations of Greek text will, where possible, be handled here. *
         *  Envisaged tasks are:                                                   *
         *                                                                         *
         *  1. Store bare vowels                                                   *
         *  2. Store bare consonants                                               *
         *  3. Store all base letters                                              *
         *  4. Handle the transition from a letter with "furniture" (e.g. accents, *
         *       diaraesis, etc - excluding breathings) to related letters with    *
         *       either no diacritics or with a breathing only;                    *
         *  5. Handle the transition from a letter with breathings to the base     *
         *       equivalent                                                        *
         *  6. Potentially, handle the addition of diacritics (not currently       *
         *       needed)                                                           *
         *  7. Potentially, handle the transition from transliterations to the     *
         *       real Greek character (not currently needed)                       *
         *                                                                         *
         ***************************************************************************/

        const int mainCharsStart = 0x0386, mainCharsEnd = 0x03ce, furtherCharsStart = 0x1f00, furtherCharsEnd = 0x1ffc;

        classGlobal globalVars;
        RichTextBox rtxtMain;

        // hexadecimal punctuation characters are: the middle dot, the ano teleia (partial stop, which is 
        //    functionally the same as the middle dot and the unicode documentation says that the middle
        //    dot is the "preferred character") and the erotimatiko (the Greek question mark, identical in 
        //    appearance to the semi colon).  So, elements 1 (zero-based) and 5 are equivalent and elements
        //    3 and 4 are equivalent.  Both are included in case source material has varied usage.
        String[] allowedPunctuation = { ".", ";", ",", "\u00b7", "\u0387", "\u037e" };
        SortedDictionary<int, String> allGkChars = new SortedDictionary<int, string>();
        SortedList<int, int> conversionWithBreathings = new SortedList<int, int>();

        SortedList<int, int> vowelCode = new SortedList<int, int>();
        SortedList<int, int> breathingCode = new SortedList<int, int>();
        SortedList<int, String> simpleGkMins = new SortedList<int, string>();
        SortedList<int, String> simpleGkMajs = new SortedList<int, string>();
        SortedList<int, String> baseVowels = new SortedList<int, string>();
        SortedList<int, String> baseSmoothVowels = new SortedList<int, string>();
        SortedList<int, String> baseRoughVowels = new SortedList<int, string>();
        SortedList<int, int> convertToSmooth = new SortedList<int, int>();
        SortedList<int, int> convertToRough = new SortedList<int, int>();
        SortedList<int, int> convertToNone = new SortedList<int, int>();

        public void initialiseGreekProcessing(classGlobal inConfig)
        {
            globalVars = inConfig;
        }

        public void constructGreekLists()
        {
            /**********************************************************************************
             *                                                                                *
             *                          constructGreekLists                                   *
             *                          ===================                                   *
             *                                                                                *
             *  The coding works on the following basis:                                      *
             *    a) Each base vowel has an integer value in ascending order.  So:            *
             *          α = 1                                                                 *
             *          ε = 2                                                                 *
             *          η = 3                                                                 *
             *          ι = 4                                                                 *
             *          ο = 5                                                                 *
             *          υ = 6                                                                 *
             *          ω = 7                                                                 *
             *       This value applies whether it is miniscule or majiscule                  *
             *    b) Add 10 if the vowel has a "grave" accent (varia)                         *
             *    c) Add 20 if the vowel has an "accute" accent (oxia)                        *
             *    d) Add 30 if the vowel has a "circumflex" accent (perispomeni)              *
             *    e) Add 40 if the vowel is in the base table and has an accute accent (oxia) *
             *         i.e. it is an alternative coding - it's actually a tonos, not an oxia  *
             *    f) Add 50 if the vowel has a vrachy (cannot have a breathing or accent)     *
             *    g) Add 60 if the vowel has a macron                                         *
             *    h) Add 100 if the vowel has a smooth breathing (psili)                      *
             *    i) Add 200 if the vowel has a rough breathing (dasia)                       *
             *    j) Add 300 if the vowel has dieresis (dialytika) - only ι and υ             *
             *    k) Add 1000 if the vowel has an iota subscript - only α, η and ω            *
             *    l) Add 10000 if a majuscule                                                 *
             *                                                                                *
             *    charCode1 (a and b):                                                        *
             *      purpose: to indicate whether a char is vowel (and, if so, which), another *
             *      extra character (i.e. rho or final sigma), a simple letter or puntuation. *
             *      Code values are:                                                          *
             *        -1   A null value - to be ignored                                       *
             *       0 - 6 alpha to omega respectively - not simple                           *
             *         7   rho - not simple                                                   *
             *         8   final sigma                                                        *
             *         9   simple alphabet                                                    *
             *        10   punctuation                                                        *
             *                                                                                *
             *    charCode2:                                                                  *
             *      purpose: identify whether char has a smooth breathing, rough breathing or *
             *               none.  All part a chars are without breathing, so only part b    *
             *               characters are coded. (Note, however, that 0x03ca and 0x03cb are *
             *               diereses (code value 3).                                         *
             *      Code values are:                                                          *
             *         0   No breathing                                                       *
             *         1   Smooth breathing                                                   *
             *         2   Rough breathing                                                    *
             *         3   Dieresis                                                           *
             *                                                                                *
             **********************************************************************************/

            int[] gkTable1 = { 0x03b1, -1, 0x03b5, 0x03b7, 0x03b9, -1, 0x03bf, -1, 0x03c5, 0x03c9, 0x03ca,
                           0x03b1, 0x03b2, 0x03b3, 0x03b4, 0x03b5, 0x03b6, 0x03b7, 0x03b8, 0x03b9, 0x03ba, 0x03bb, 0x03bc, 0x03bd, 0x03be, 0x03bf,
                           0x03c0, 0x03c1, -1, 0x03c3, 0x03c4, 0x03c5, 0x03c6, 0x03c7, 0x03c8, 0x03c9, 0x03ca, 0x03cb, 0x03b1, 0x03b5, 0x03b7, 0x03b9, 0x03cb,
                           0x03b1, 0x03b2, 0x03b3, 0x03b4, 0x03b5, 0x03b6, 0x03b7, 0x03b8, 0x03b9, 0x03ba, 0x03bb, 0x03bc, 0x03bd, 0x03be, 0x03bf,
                           0x03c0, 0x03c1, 0x03c2, 0x03c3, 0x03c4, 0x03c5, 0x03c6, 0x03c7, 0x03c8, 0x03c9, 0x03ca, 0x03cb, 0x03bf, 0x03c5, 0x03c9};
            int[] gkTable2 = { 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01,
                           0x1f10, 0x1f11, 0x1f10, 0x1f11, 0x1f10, 0x1f11, -1, -1, 0x1f10, 0x1f11, 0x1f10, 0x1f11, 0x1f10, 0x1f11, -1, -1,
                           0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21,
                           0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31,
                           0x1f40, 0x1f41, 0x1f40, 0x1f41, 0x1f40, 0x1f41, -1, -1, 0x1f40, 0x1f41, 0x1f40, 0x1f41, 0x1f40, 0x1f41, -1, -1,
                           0x1f50, 0x1f51, 0x1f50, 0x1f51, 0x1f50, 0x1f51, 0x1f50, 0x1f51, -1, 0x1f51, -1, 0x1f51, -1, 0x1f51, -1, 0x1f51,
                           0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61,
                           0x03b1, 0x03b1, 0x03b5, 0x03b5, 0x03b7, 0x03b7, 0x03b9, 0x03b9, 0x03bf, 0x03bf, 0x03c5, 0x03c5, 0x03c9, 0x03c9, -1, -1,
                           0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01,
                           0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21,
                           0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61,
                           0x03b1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, -1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, -1, -1, -1,
                           -1, -1, 0x03b7, 0x03b7, 0x03b7, -1, 0x03b7, 0x03b7, 0x03b5, 0x03b5, 0x03b7, 0x03b7, 0x03b7, -1, -1, -1,
                           0x03b9, 0x03b9, 0x03ca, 0x03ca, -1, -1, 0x03b9, 0x03ca, 0x03b9, 0x03b9, 0x03b9, 0x03b9, -1, -1, -1, -1,
                           0x03c5, 0x03c5, 0x03cb, 0x03cb, 0x1fe4, 0x1fe5, 0x03c5, 0x03cb, 0x03c5, 0x03c5, 0x03c5, 0x03c5, 0x1fe5, -1, -1, -1,
                           -1, -1, 0x03c9, 0x03c9, 0x03c9, -1, 0x03c9, 0x03c9, 0x03bf, 0x03bf, 0x03c9, 0x03c9, 0x03c9, -1, -1, -1 };
            int idx, mainCharIndex, furtherCharIndex;
            String charRepresentation;
            rtxtMain = globalVars.getRichtextControlByIndex(0);
            rtxtMain.Text = "";

            // Load the two Unicode tables into memory.  These are stored as:
            //           - base characters (and a few extras);
            //           - characters with accents, breathings, iota subscript, etc.
            // allGkChars: key = the code value, value = the code converted to a string character
            idx = 0;
            for (mainCharIndex = mainCharsStart; mainCharIndex <= mainCharsEnd; mainCharIndex++)
            {
                charRepresentation = (Convert.ToChar(mainCharIndex)).ToString();
                if (String.Compare(charRepresentation, "΋") == 0)
                {
                    idx++;
                    continue;
                }
                allGkChars.Add(mainCharIndex, charRepresentation);
                conversionWithBreathings.Add(mainCharIndex, gkTable1[idx]);
                idx++;
            }
            idx = 0;
            for (furtherCharIndex = furtherCharsStart; furtherCharIndex <= furtherCharsEnd; furtherCharIndex++)
            {
                charRepresentation = (Convert.ToChar(furtherCharIndex)).ToString();
                if (String.Compare(charRepresentation, "΋") == 0)
                {
                    idx++;
                    continue;
                }
                allGkChars.Add(furtherCharIndex, charRepresentation);
                conversionWithBreathings.Add(furtherCharIndex, gkTable2[idx]);
                idx++;
            }
            charRepresentation = (Convert.ToChar(0x03dc).ToString());  // Majuscule and miuscule digamma
            allGkChars.Add(0x03dc, charRepresentation);
            conversionWithBreathings.Add(0x03dc, 0x03dd);
            charRepresentation = (Convert.ToChar(0x03dd).ToString());
            allGkChars.Add(0x03dd, charRepresentation);
            conversionWithBreathings.Add(0x03dd, 0x03dd);
        }

        private void populateSmoothStore(int low, int high, int step, int gap1, int gap2, bool isSmooth)
        {
            int idx, simpleCount;

            simpleCount = 0;
            for (idx = low; idx <= high; idx += step)
            {
                if (isSmooth) convertToSmooth.Add(idx, simpleCount);
                else convertToRough.Add(idx, simpleCount);
                if (idx <= 0x1f8f) simpleCount += gap1;
                else simpleCount += gap2;
            }
        }

        private void populateNoneStore(int high, int low, int vowelMarker, int[] gaps)
        {
            int idx;

            for (idx = low; idx <= high; idx++)
            {
                if (gaps.Contains(idx)) continue;
                convertToNone.Add(idx, vowelMarker);
            }
        }

        public Tuple<String, String, String, String> removeNonGkChars(String source)
        {
            /*********************************************************************************
             *                                                                               *
             *                            removeNonGkChars                                   *
             *                            ================                                   *
             *                                                                               *
             *  The text comes with various characters derived from the Bible Society text   *
             *    that identifies variant readings.  Since we have no ability (or copyright  *
             *    agreement) to present these variant readings, they are redundant and even  *
             *    intrusive.  This method will remove them, where they occur.                *
             *                                                                               *
             *  It will allso:                                                               *
             *    a) preserve any ascii text before the Greek word;                          *
             *    b) preserve any ascii non-punctuation after the Greek word;hars and punct. *
             *    c) preserve any punctuation attached to the word.                          *
             *                                                                               *
             *  Returned value is a Tuple with:                                              *
             *      item1 = any ascii text found as in a) above                              *
             *      item2 = any non-punctuation, as in b) above                              *
             *      item3 = any punctuation                                                  *
             *      item4 = the Greek word without these spurious characters                 *
             *                                                                               *
             *********************************************************************************/

            String preChars = "", postChars = "", puntuation = "", clearGreek = "", copyOfSource;

            copyOfSource = source;
            while (copyOfSource.Length > 0)
            {
                if ((copyOfSource[0] >= '\u0386') && (copyOfSource[0] <= '\u03ce')) break;
                if ((copyOfSource[0] == '\u03dc') || (copyOfSource[0] == '\u03dd')) break;
                if ((copyOfSource[0] >= '\u1f00') && (copyOfSource[0] <= '\u1fff')) break;
                if (copyOfSource[0] <= '\u007f')
                {
                    preChars += copyOfSource.Substring(0, 1);
                }
                copyOfSource = copyOfSource.Substring(1, copyOfSource.Length - 1);
            }
            while (copyOfSource.Length > 0)
            {
                if (copyOfSource[copyOfSource.Length - 1] == '\u0386') break;
                if ((copyOfSource[copyOfSource.Length - 1] >= '\u0388') && (copyOfSource[copyOfSource.Length - 1] <= '\u03ce')) break;
                if ((copyOfSource[copyOfSource.Length - 1] == '\u03dc') || (copyOfSource[copyOfSource.Length - 1] == '\u03dd')) break;
                if ((copyOfSource[copyOfSource.Length - 1] >= '\u1f00') && (copyOfSource[copyOfSource.Length - 1] <= '\u1fff')) break;
                if (allowedPunctuation.Contains(copyOfSource.Substring(copyOfSource.Length - 1, 1)))
                {
                    puntuation = copyOfSource.Substring(copyOfSource.Length - 1, 1) + puntuation;
                }
                else
                {
                    if (copyOfSource[copyOfSource.Length - 1] <= '\u007f')
                    {
                        postChars = copyOfSource.Substring(copyOfSource.Length - 1, 1) + postChars;
                    }
                }
                copyOfSource = copyOfSource.Substring(0, copyOfSource.Length - 1);
            }
            clearGreek = copyOfSource;
            return new Tuple<string, string, string, string>(preChars, postChars, puntuation, clearGreek);
        }

        public String reduceToBareGreek(String source, bool nonGkIsAlreadyRemoved)
        {
            /*********************************************************************************
             *                                                                               *
             *                            reduceToBareGreek                                  *
             *                            =================                                  *
             *                                                                               *
             *  This will remove all accents, iota subscripts and length marks (it will      *
             *    retain breathings and diereses).  It will also:                            *
             *    - reduce any capital letters to minuscules (see below, however),           *
             *    - present final sigma as a normal sigma.                                   *
             *                                                                               *
             *  Note that any majuscule will also be reduced to a minuscule.                 *
             *  Care will be taken to ensure that any final sigma *is* a final sigma.        *
             *                                                                               *
             *  Parameters:                                                                  *
             *    source                 The starting text                                   *
             *    nonGkIsAlreadyRemoved  This should be set to true if it has already been   *
             *                           processed by removeNonGkChars                       *
             *                                                                               *
             *  Returned value:                                                              *
             *      String containing the suitably cleaned/stripped word                     *
             *                                                                               *
             *********************************************************************************/

            int idx, lengthOfString, characterValue, characterType;
            String tempWorkArea, strippedString, strippedChar;
            Tuple<String, String, String, String> CleanReturn;

            tempWorkArea = source;
            lengthOfString = tempWorkArea.Length;
            if (lengthOfString == 0) return source;
            strippedString = "";
            if (!nonGkIsAlreadyRemoved)
            {
                CleanReturn = removeNonGkChars(tempWorkArea);
                tempWorkArea = CleanReturn.Item4;
            }
            lengthOfString = tempWorkArea.Length;
            for (idx = 0; idx < lengthOfString; idx++)
            {
                // Get Hex value of character
                characterValue = (int)tempWorkArea[idx];
                if (characterValue == 0x2d)
                {
                    strippedString += "-";
                    continue;
                }
                // What character are we dealing with?
                conversionWithBreathings.TryGetValue(characterValue, out characterType);
                strippedChar = Convert.ToChar(characterType).ToString();
                strippedString += strippedChar;
            }
            // Check for final sigma
            lengthOfString = strippedString.Length;
            if (((int)strippedString[lengthOfString - 1]) == 0x03c3)
            {
                allGkChars.TryGetValue(0x03c2, out strippedChar);
                strippedString = strippedString.Substring(0, lengthOfString - 1) + strippedChar;
            }
            return strippedString;
        }

        public bool isCharMajiscule(String source)
        {
            int charCode;

            charCode = (int)source[0];
            if ((charCode >= 0x0391) && (charCode <= 0x03a9) && (charCode != 0x03a2)) return true;
            if (charCode % 16 > 7) return true;  // i.e. the least significant hex char is between 0x8 and 0xf
            return false;
        }

        public String leadingCharToUpper(String source)
        {
            int wordLen, charCode, index;
            String replacementWord, targetChar = null, currentChar = null;

            wordLen = source.Length;
            if (wordLen == 0) return "";
            if (isCharMajiscule(source)) return source;
            charCode = (int)source[0];
            allGkChars.TryGetValue(charCode, out currentChar);
            if (currentChar != null)
            {
                if (simpleGkMins.ContainsValue(currentChar))
                {
                    index = simpleGkMins.IndexOfValue(currentChar);
                    simpleGkMajs.TryGetValue(index, out targetChar);
                }
                else
                {
                    // Not in simple letters, so assume it's a vowel
                    if (convertToSmooth.ContainsKey(charCode)) // Is it a smooth breathing vowel?
                    {
                        convertToSmooth.TryGetValue(charCode, out index);
                        baseSmoothVowels.TryGetValue(index, out currentChar);
                        targetChar = (Convert.ToChar((int)currentChar[0]).ToString());
                    }
                    else
                    {
                        if (convertToRough.ContainsKey(charCode))
                        {
                            convertToRough.TryGetValue(charCode, out index);
                            baseRoughVowels.TryGetValue(index, out currentChar);
                            targetChar = (Convert.ToChar((int)currentChar[0]).ToString());
                        }
                        else
                        {
                            convertToNone.TryGetValue(charCode, out index);
                            baseVowels.TryGetValue(index, out currentChar);
                            targetChar = (Convert.ToChar((int)currentChar[0]).ToString());
                        }
                    }
                }
                replacementWord = targetChar + source.Substring(1);
                return replacementWord;
            }
            return "";
        }
    }
}
