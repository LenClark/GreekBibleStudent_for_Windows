using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GreekBibleStudent
{
    public class classGreek
    {
        /*================================================================================================*
         *                                                                                                *
         *                                  classGreekProcessing                                          *
         *                                  ====================                                          *
         *                                                                                                *
         *  All manipulations of Greek text will, where possible, be handled here.                        *
         *  Envisaged tasks are:                                                                          *
         *                                                                                                *
         *  1. Store bare vowels                                                                          *
         *  2. Store bare consonants                                                                      *
         *  3. Store all base letters                                                                     *
         *  4. Handle the transition from a letter with "furniture" (e.g. accents, diaraesis, etc -       *
         *       excluding breathings) to related letters with either no diacritics or with a breathing   *
         *       only;                                                                                    *
         *  5. Handle the transition from a letter with breathings to the base equivalent                 *
         *  6. Potentially, handle the addition of diacritics (not currently needed)                      *
         *  7. Potentially, handle the transition from transliterations to the real Greek character (not  *
         *       currently needed)                                                                        *
         *                                                                                                *
         *================================================================================================*/

        /*---------------------------------------------------------------------------------------------------------*
          *                                                                                                         *
          *                                             allGkChars                                                  *
          *                                             ----------                                                  *
          *                                                                                                         *
          *   Load the two Unicode tables into memory.  This will identify the integer value of a character         *
          *     (provided in hexadecimal) with its String form.                                                     *
          *                                                                                                         *
          *       Key = the integer value of the character                                                          *
          *       Value = the hex code converted to a string character (i.e. the actual character)                  *
          *                                                                                                         *
          *   Note that the table will contain some characters of no interest to us, including some unprintable     *
          *     characters.  These will simply be ignored.                                                          *
          *                                                                                                         *
          *---------------------------------------------------------------------------------------------------------*/
        int noOfGkChars = 0;
        SortedDictionary<int, String> allGkChars = new SortedDictionary<int, string>();

        /*---------------------------------------------------------------------------------------------------------*
         *                                                                                                         *
         *                                          addRoughBreathing                                              *
         *                                          -----------------                                              *
         *                                                                                                         *
         *   This allows us to convert any Greek character that is cabable of carrying a rough breathing from its  *
         *     non-rough breathing state to one with a rough breathing.                                            *
         *                                                                                                         *
         *       Key = the character without a rough breathing                                                     *
         *       Value = the "same" character _with_ a rough breathing                                             *
         *                                                                                                         *
         *---------------------------------------------------------------------------------------------------------*/
        SortedDictionary<String, String> addRoughBreathing = new SortedDictionary<String, String>();

        /*---------------------------------------------------------------------------------------------------------*
         *                                                                                                         *
         *            addSmoothBreathing, addAccute, addGrave, addCirc, addDiaeresis, addIotaSub                   *
         *            --------------------------------------------------------------------------                   *
         *                                                                                                         *
         *   These function much as addRoughBreathing: it converts a character from a state without the accent,    *
         *     breathing or other element to one that _does_ contain it.                                           *
         *                                                                                                         *
         *       Key = the character without the accent, breathing, etc.                                           *
         *       Value = the "same" character _with_ the relevant element.                                         *
         *                                                                                                         *
         *---------------------------------------------------------------------------------------------------------*/
        SortedDictionary<String, String> addSmoothBreathing = new SortedDictionary<String, String>();
        SortedDictionary<String, String> addAccute = new SortedDictionary<String, String>();
        SortedDictionary<String, String> addGrave = new SortedDictionary<String, String>();
        SortedDictionary<String, String> addCirc = new SortedDictionary<String, String>();
        SortedDictionary<String, String> addDiaeresis = new SortedDictionary<String, String>();
        SortedDictionary<String, String> addIotaSub = new SortedDictionary<String, String>();

        /*---------------------------------------------------------------------------------------------------------*
         *                                                                                                         *
         *                                         conversionWithBreathings                                        *
         *                                         ------------------------                                        *
         *                                                                                                         *
         *---------------------------------------------------------------------------------------------------------*/
        SortedList<String, String> conversionWithBreathings = new SortedList<String, String>();

        String[] allowedPunctuation = { ".", ";", ",", "\u00b7", "\u0387", "\u037e" };

        classGlobal globalVars;

        public void initialiseGreekOrthography(classGlobal inGlobal)
        {
            globalVars = inGlobal;
            setupAllChars();
            addRoughBreathing = getRelevantGreek(globalVars.FullGkRough);
            addSmoothBreathing = getRelevantGreek(globalVars.FullGkSmooth);
            addAccute = getRelevantGreek(globalVars.FullGkAccute);
            addGrave = getRelevantGreek(globalVars.FullGkGrave);
            addCirc = getRelevantGreek(globalVars.FullGkCircumflex);
            addDiaeresis = getRelevantGreek(globalVars.FullGkDiaereses);
            addIotaSub = getRelevantGreek(globalVars.FullGkIota);
        }

        private void setupAllChars()
        {
            const int mainCharsStart = 0x0386, mainCharsEnd = 0x03ce, furtherCharsStart = 0x1f00, furtherCharsEnd = 0x1ffc;

            int idx = 0, mainCharIndex, furtherCharIndex, storedValue;
            String charRepresentation, convertedRepresentation, fileName, buffer;
            StreamReader srGreek;

            /*---------------------------------------------------------------------------------------------------------*
             *                                                                                                         *
             *                                        convTable1 and convTable2                                        *
             *                                        -------------------------                                        *
             *                                                                                                         *
             *   These are used as temporary stores for creating breathing conversions.                                *
             *                                                                                                         *
             *---------------------------------------------------------------------------------------------------------*/
            SortedDictionary<int, String> convTable1 = new SortedDictionary<int, String>();
            SortedDictionary<int, String> convTable2 = new SortedDictionary<int, String>();

            // First, populate convTables 1 and 2
            fileName = globalVars.FullGkConv1;
            srGreek = new StreamReader(fileName);
            buffer = srGreek.ReadLine();
            while (buffer != null)
            {
                if (buffer.Length > 0)
                {
                    if (buffer[0] == '-') storedValue = -1;
                    else storedValue = hexStringToInt(buffer);
                    if (storedValue == -1) convertedRepresentation = "";
                    else convertedRepresentation = (Convert.ToChar(storedValue)).ToString();
                    convTable1.Add(idx++, convertedRepresentation);
                }
                buffer = srGreek.ReadLine();
            }
            srGreek.Close();

            fileName = globalVars.FullGkConv2;
            idx = 0;
            srGreek = new StreamReader(fileName);
            buffer = srGreek.ReadLine();
            while (buffer != null)
            {
                if (buffer.Length > 0)
                {
                    if (buffer[0] == '-') storedValue = -1;
                    else storedValue = hexStringToInt(buffer);
                    if (storedValue == -1) convertedRepresentation = "";
                    else convertedRepresentation = (Convert.ToChar(storedValue)).ToString();
                    convTable2.Add(idx++, convertedRepresentation);
                }
                buffer = srGreek.ReadLine();
            }
            srGreek.Close();

            idx = 0;
            for (mainCharIndex = mainCharsStart; mainCharIndex <= mainCharsEnd; mainCharIndex++)
            {
                charRepresentation = (Convert.ToChar(mainCharIndex)).ToString();
                allGkChars.Add(mainCharIndex, charRepresentation);
                convTable1.TryGetValue(idx++, out convertedRepresentation);
                if (!conversionWithBreathings.ContainsKey(charRepresentation)) conversionWithBreathings.Add(charRepresentation, convertedRepresentation);
                noOfGkChars++;
            }
            idx = 0;
            for (furtherCharIndex = furtherCharsStart; furtherCharIndex <= furtherCharsEnd; furtherCharIndex++)
            {
                charRepresentation = (Convert.ToChar(furtherCharIndex)).ToString();
                allGkChars.Add(furtherCharIndex, charRepresentation);
                convTable2.TryGetValue(idx++, out convertedRepresentation);
                if (!conversionWithBreathings.ContainsKey(charRepresentation)) conversionWithBreathings.Add(charRepresentation, convertedRepresentation);
                noOfGkChars++;
            }
            charRepresentation = (Convert.ToChar(0x03dc).ToString());  // Majuscule and miuscule digamma
            allGkChars.Add(0x03dc, charRepresentation);
            conversionWithBreathings.Add(charRepresentation, (Convert.ToChar(hexStringToInt("0x03dd"))).ToString());
            charRepresentation = (Convert.ToChar(0x03dd).ToString());
            allGkChars.Add(0x03dd, charRepresentation);
            conversionWithBreathings.Add(charRepresentation, charRepresentation);
            noOfGkChars += 2;
        }

        private SortedDictionary<String, String> getRelevantGreek(String fileName)
        {
            /*==================================================================================================*
             *                                                                                                  *
             *                                      getRelevantGreek                                            *
             *                                      ================                                            *
             *                                                                                                  *
             *  Services initialiseGreekOrthography by simply loading data from files.                          *
             *                                                                                                  *
             *==================================================================================================*/

            String buffer, startingChar, finalChar;
            Char[] splitChar = { ',' };
            String[] components;
            StreamReader srGreek;
            SortedDictionary<String, String> tempDictionary = new SortedDictionary<string, string>();

            srGreek = new StreamReader(fileName);
            buffer = srGreek.ReadLine();
            while (buffer != null)
            {
                if (buffer.Length > 0)
                {
                    components = buffer.Split(splitChar);
                    if (components[0][0] == '0') startingChar = (Convert.ToChar(hexStringToInt(components[0]))).ToString();
                    else startingChar = components[0];
                    if (components[1][0] == '0') finalChar = (Convert.ToChar(hexStringToInt(components[1]))).ToString();
                    else finalChar = components[1];
                    if (!tempDictionary.ContainsKey(startingChar)) tempDictionary.Add(startingChar, finalChar);
                }
                buffer = srGreek.ReadLine();
            }
            srGreek.Close();
            return tempDictionary;
        }

        private int hexStringToInt(String hexString)
        {
            int idx, hexLength, multiplier, runningTotal = 0;
            String activeString;

            // This assumes a string of the form 0xnnnn
            activeString = hexString.Substring(2);
            hexLength = activeString.Length;
            multiplier = 1;
            for (idx = hexLength - 1; idx >= 0; idx--)
            {
                if ((activeString[idx] >= '0') && (activeString[idx] <= '9')) runningTotal += Convert.ToInt32(activeString.Substring(idx, 1)) * multiplier;
                else
                {
                    switch (activeString[idx])
                    {
                        case 'a':
                        case 'A': runningTotal += 10 * multiplier; break;
                        case 'b':
                        case 'B': runningTotal += 11 * multiplier; break;
                        case 'c':
                        case 'C': runningTotal += 12 * multiplier; break;
                        case 'd':
                        case 'D': runningTotal += 13 * multiplier; break;
                        case 'e':
                        case 'E': runningTotal += 14 * multiplier; break;
                        case 'f':
                        case 'F': runningTotal += 15 * multiplier; break;
                    }
                }
                multiplier *= 16;
            }
            return runningTotal;
        }

        public String reduceToBareGreek(String source, bool nonGkIsAlreadyRemoved)
        {
            /*===========================================================================================================*
             *                                                                                                           *
             *                                           reduceToBareGreek                                               *
             *                                           =================                                               *
             *                                                                                                           *
             *  There are times when we want the basic word (specifically for accent-independent comparisons).  We have  *
             *    already included accentless versions of words in classLXXWord but we may need to strip the extra       *
             *    elements from words from other sources.                                                                *
             *                                                                                                           *
             *  This will remove all accents, iota subscripts and length marks (it will retain breathings and diereses). *
             *    It will also:                                                                                          *
             *    - reduce any capital letters to minuscules (see below, however),                                       *
             *    - present final sigma as a normal sigma.                                                               *
             *                                                                                                           *
             *  Note that _any_ majuscule will also be reduced to a minuscule.                                           *
             *  Care will be taken to ensure that any final sigma *is* a final sigma.                                    *
             *                                                                                                           *
             *  Parameters:                                                                                              *
             *    source                 The starting text                                                               *
             *    nonGkIsAlreadyRemoved  This should be set to true if it has already been processed by removeNonGkChars *
             *                                                                                                           *
             *  Returned value:                                                                                          *
             *      String containing the suitably cleaned/stripped word                                                 *
             *                                                                                                           *
             *===========================================================================================================*/

            int idx, lengthOfString, characterValue;
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
                conversionWithBreathings.TryGetValue(Convert.ToChar(characterValue).ToString(), out strippedChar);
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

        public Tuple<String, String, String, String> removeNonGkChars(String source)
        {
            /*===========================================================================================================*
             *                                                                                                           *
             *                                              removeNonGkChars                                             *
             *                                              ================                                             *
             *                                                                                                           *
             *  The text comes with various characters derived from the Bible Society text that identifies variant       *
             *    readings.  Since we have no ability (or copyright agreement) to present these variant readings, they   *
             *    are redundant and even intrusive.  This method will remove them, where they occur.                     *
             *                                                                                                           *
             *  It will allso:                                                                                           *
             *    a) preserve any ascii text before the Greek word;                                                      *
             *    b) preserve any ascii non-punctuation after the Greek word;hars and punct.                             *
             *    c) preserve any punctuation attached to the word.                                                      *
             *                                                                                                           *
             *  Returned value is a Tuple with:                                                                          *
             *      item1 = any ascii text found as in a) above                                                          *
             *      item2 = any non-punctuation, as in b) above                                                          *
             *      item3 = any punctuation                                                                              *
             *      item4 = the Greek word without these spurious characters                                             *
             *                                                                                                           *
             *===========================================================================================================*/

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

        public String getCharacterWithRoughBreathing(String previousChar)
        {
            String replacementChar = null;

            addRoughBreathing.TryGetValue(previousChar, out replacementChar);
            if (replacementChar == null) return previousChar;
            return replacementChar;
        }

        public String getCharacterWithSmoothBreathing(String previousChar)
        {
            String replacementChar = null;

            addSmoothBreathing.TryGetValue(previousChar, out replacementChar);
            if (replacementChar == null) return previousChar;
            return replacementChar;
        }

        public String getCharacterWithAccuteAccent(String previousChar)
        {
            String replacementChar = null;

            addAccute.TryGetValue(previousChar, out replacementChar);
            if (replacementChar == null) return previousChar;
            return replacementChar;
        }

        public String getCharacterWithGraveAccent(String previousChar)
        {
            String replacementChar = null;

            addGrave.TryGetValue(previousChar, out replacementChar);
            if (replacementChar == null) return previousChar;
            return replacementChar;
        }

        public String getCharacterWithCircumflexAccent(String previousChar)
        {
            String replacementChar = null;

            addCirc.TryGetValue(previousChar, out replacementChar);
            if (replacementChar == null) return previousChar;
            return replacementChar;
        }

        public String getCharacterWithDieresis(String previousChar)
        {
            String replacementChar = null;

            addDiaeresis.TryGetValue(previousChar, out replacementChar);
            if (replacementChar == null) return previousChar;
            return replacementChar;
        }

        public String getCharacterWithIotaSubscript(String previousChar)
        {
            String replacementChar = null;

            addIotaSub.TryGetValue(previousChar, out replacementChar);
            if (replacementChar == null) return previousChar;
            return replacementChar;
        }
    }
}
