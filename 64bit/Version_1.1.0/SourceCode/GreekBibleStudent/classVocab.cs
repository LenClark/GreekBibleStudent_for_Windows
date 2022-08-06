using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    internal class classVocab
    {
        classGlobal globalVars;
        classNote note;
        DataGridView dgvTarget;

        public classVocab( classGlobal inGlobal, classNote inNote )
        {
            globalVars = inGlobal;
            note = inNote;
            dgvTarget = globalVars.DgvVocabList;
        }

        public void setupVocabList()
        {
            int idx, vdx, wordCount = 0, bookId = 0, chapSeq, verseSeq, noOfWords, noOfVerses;
            String chapterRef, verseNumber = "", bookName = "";
            int[] vocabTypes = { 0, 0, 0, 0, 0, 0 };
            SortedDictionary<int, classWord> wordsInSequence = new SortedDictionary<int, classWord>();
            classBook currentBook = null;
            classChapter currentChapter;
            classVerse currentVerse;
            classWord currentWord;
            RichTextBox targetTextbox /*, rtxtVocabList */;

            dgvTarget.RowCount = 0;
            if (globalVars.getTabControl(0).SelectedIndex == 0)
            {
                bookId = globalVars.getComboBoxItem(0).SelectedIndex + globalVars.NoOfLXXBooks;
                globalVars.BookList.TryGetValue( bookId, out currentBook );
                chapSeq = globalVars.getComboBoxItem(1).SelectedIndex;
                verseSeq = globalVars.getComboBoxItem(2).SelectedIndex;
                targetTextbox = globalVars.getRichtextItem(0);
            }
            else
            {
                bookId = globalVars.getComboBoxItem(3).SelectedIndex;
                chapSeq = globalVars.getComboBoxItem(4).SelectedIndex;
                verseSeq = globalVars.getComboBoxItem(5).SelectedIndex;
                targetTextbox = globalVars.getRichtextItem(1);
            }
            globalVars.BookList.TryGetValue(bookId, out currentBook);
            bookName = currentBook.BookName;
            currentChapter = currentBook.getChapterBySequence(chapSeq);
            chapterRef = currentBook.getChapterRefBySequence(chapSeq);
            globalVars.getLabel(3).Text = bookName + " " + chapterRef;
            if (globalVars.getRadioButton(4).Checked)  // List based only on the current verse
            {
                currentVerse = currentChapter.getVerseBySeqNo(verseSeq);
                verseNumber = currentChapter.getVerseRefBySeqNo(verseSeq);
                globalVars.getLabel(3).Text = bookName + " " + chapterRef + ":" + verseNumber;
                noOfWords = currentVerse.WordCount;
                for (idx = 0; idx < noOfWords; idx++)
                {
                    currentWord = currentVerse.getWordBySeqNo(idx);
                    wordsInSequence.Add(wordCount++, currentWord);
                }
            }
            else  // We're dealing with the whole chapter
            {
                // All we do here is list the words *in sequence*, without worrying about verses or verse numbers
                noOfVerses = currentChapter.NoOfVersesInChapter;
                for (vdx = 0; vdx < noOfVerses; vdx++)
                {
                    currentVerse = currentChapter.getVerseBySeqNo(vdx);
                    noOfWords = currentVerse.WordCount;
                    for (idx = 0; idx < noOfWords; idx++)
                    {
                        currentWord = currentVerse.getWordBySeqNo(idx);
                        wordsInSequence.Add(wordCount++, currentWord);
                    }
                }
            }
            processVocabulary(wordsInSequence);
            globalVars.getTabControl(1).SelectedIndex = 3;
        }

        private void processVocabulary(SortedDictionary<int, classWord> wordsInSequence)
        {
            /*=================================================================================================*
             *                                                                                                 *
             *                                        processVocabulary                                        *
             *                                        =================                                        *
             *                                                                                                 *
             *  This will create the vocabulary lists, according to options selected in the bottom pane.       *
             *                                                                                                 *
             *  Parameters:                                                                                    *
             *  ==========                                                                                     *
             *                                                                                                 *
             *  wordsInSequence       A list of word instances, stored in the order that they occur            *
             *                                                                                                 *
             *  Key variables (below):                                                                         *
             *  =====================                                                                          *
             *                                                                                                 *
             *  wordsInOrder          This provides a simple way of sorting the words, should a sorted option  *
             *                          be chosen                                                              *
             *  isFirstInCat                                                                                   *
             *  isPos                                                                                          *
             *  vocabTypes            Stores the specific grammatical options chosen - saves having to         *
             *                          constantly refer back to the radio buttons                             *
             *  parseOptions                                                                                   *
             *  parseLxxOptions       The string representations used to designated parts of speech as used in *
             *                          the source data                                                        *
             *  parseOptionNames      More orthodox part of speech descriptions, used in the actual lists      *
             *                                                                                                 *
             *=================================================================================================*/

            bool isFirstInCat, isPos;
            int idx, jdx, vocabCheckCount, noOfPos, collationCode = 0, rowNumber, widthCalc;
            String wordUnderConsideration, declaredStyle;
            int[] vocabTypes = { 0, 0, 0, 0, 0, 0 };
            String[] parseOptions = { "N-", "V-", "A-", "X-", "P-", "C-", "D-", "I-", "RA", "RD", "RI", "RP", "RR" };
            Char[] parseLxxOptions = { 'N', 'V', 'A', 'D', 'P', 'C', 'X', 'I', 'M', 'R' };
            String[] parseOptionNames = { "Nouns:", "Verbs:", "Adjectives:", "Adverbs:", "Prepositions:", "Other word types:" };
            SortedDictionary<String, classWord> wordsInOrder = new SortedDictionary<string, classWord>();
            SortedList<int, String> col1Text = new SortedList<int, string>();
            SortedList<int, String> col2Text = new SortedList<int, string>();
            SortedList<int, String> col3Text = new SortedList<int, string>();
            SortedList<int, String> col4Text = new SortedList<int, string>();
            System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;
            //            RichTextBox rtxtVocabList;

            //            rtxtVocabList = globalVars.getRichtextItem(5);
            dgvTarget = globalVars.DgvVocabList;
            dgvTarget.DefaultCellStyle.BackColor = globalVars.getColourSetting(5, 0);
            dgvTarget.DefaultCellStyle.ForeColor = globalVars.getColourSetting(5, 1);
            declaredStyle = globalVars.getDefinedStyleByIndex(5, 1);
            switch(declaredStyle)
            {
                case "Regular": fontStyle = System.Drawing.FontStyle.Regular; break;
                case "Bold": fontStyle = System.Drawing.FontStyle.Bold; break;
                case "Italic": fontStyle = System.Drawing.FontStyle.Italic; break;
                case "Bold and Italic": fontStyle = System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic; break;
            }
            dgvTarget.DefaultCellStyle.Font = new System.Drawing.Font(globalVars.getDefinedFontNameByIndex(5, 1), globalVars.getTextSize(5, 1), fontStyle);
            widthCalc = dgvTarget.Width / 3;
            if( widthCalc < 101)
            {
                dgvTarget.Columns[0].Width = 100;
                dgvTarget.Columns[1].Width = 100;
                dgvTarget.Columns[2].Width = 100;
            }
            else
            {
                dgvTarget.Columns[0].Width = widthCalc;
                dgvTarget.Columns[1].Width = widthCalc;
                dgvTarget.Columns[2].Width = widthCalc;
            }
            for (idx = 0; idx < 6; idx++) vocabTypes[idx] = 0;
            // vocabCheckCount - used to check whether all checkboxes are left blank; if so, they are all set as if checked
            vocabCheckCount = 0;
            /*---------------------------------------------------------------------*
             *                         Populate vocabTypes                         * 
             *                                                                     * 
             *  Each vocab type represents a different part of speech.  So:        *
             *    Index       Part of Speech                                       *
             *      0           Noun                                               *
             *      1           Verb                                               *
             *      2           Adjective                                          *
             *      3           Adverb                                             *
             *      4           Preposition                                        *
             *      5           All other pos (true idx = 5 to 12)                 *
             *  In each case, if vocabTypes[x] = 1, that Pos has been selected;    *
             *    if 0, then omit.                                                 *
             *---------------------------------------------------------------------*/
            if (globalVars.getCheckBox(10).Checked) { vocabTypes[0] = 1; vocabCheckCount++; }
            if (globalVars.getCheckBox(11).Checked) { vocabTypes[1] = 1; vocabCheckCount++; }
            if (globalVars.getCheckBox(12).Checked) { vocabTypes[2] = 1; vocabCheckCount++; }
            if (globalVars.getCheckBox(13).Checked) { vocabTypes[3] = 1; vocabCheckCount++; }
            if (globalVars.getCheckBox(14).Checked) { vocabTypes[4] = 1; vocabCheckCount++; }
            if (globalVars.getCheckBox(15).Checked) { vocabTypes[5] = 1; vocabCheckCount++; }
            if (vocabCheckCount == 0)
            {
                for (idx = 0; idx < 6; idx++) vocabTypes[idx] = 1;
            }
            /*---------------------------------------------------------------------*
             *                       Populate collationCode                        * 
             *                                                                     * 
             *  This code determines what word forms are shown in the list:        *
             *  collationCode       Part of Speech                                 *
             *      1           roots only                                         *
             *      2           the form of word as used in the passage            *
             *      3           both 1 and 2, with any ordering based on the root  *
             *      4           both 1 and 2, with usage determining sequence      *
             *---------------------------------------------------------------------*/
            if (globalVars.getRadioButton(9).Checked) collationCode = 1;
            if (globalVars.getRadioButton(10).Checked) collationCode = 2;
            if (globalVars.getRadioButton(11).Checked) collationCode = 3;
            if (globalVars.getRadioButton(12).Checked) collationCode = 4;
            // noOfPos = number of items in parse???Options
            switch( collationCode)
            {
                case 1: 
                    dgvTarget.Columns[1].HeaderText = "Root Word";
                    dgvTarget.Columns[2].Visible = false;
                    break;
                case 2: 
                    dgvTarget.Columns[1].HeaderText = "Word Used"; 
                    dgvTarget.Columns[2].Visible = false;
                    break;
                case 3: 
                    dgvTarget.Columns[1].HeaderText = "Root Word";
                    dgvTarget.Columns[2].Visible = true;
                    dgvTarget.Columns[2].HeaderText = "Word Used"; break;
                case 4: 
                    dgvTarget.Columns[1].HeaderText = "Word Used";
                    dgvTarget.Columns[2].Visible = true;
                    dgvTarget.Columns[2].HeaderText = "Root Word"; break;
            }
            if (globalVars.getTabControl(0).SelectedIndex == 0) noOfPos = 13;
            else noOfPos = 10;
            rowNumber = 0;
            if ((globalVars.getRadioButton(5).Checked) || (globalVars.getRadioButton(7).Checked))
            {
                // We need to work in alphabetical order
                // Which of the two options we use, we need our list recast in alphabetical order - which includes removing duplicate roots
                foreach (KeyValuePair<int, classWord> wordPair in wordsInSequence)
                {
                    if ((collationCode == 1) || (collationCode == 3)) wordUnderConsideration = wordPair.Value.RootWord;
                    else wordUnderConsideration = wordPair.Value.BareTextWord;
                    if (!wordsInOrder.ContainsKey(wordUnderConsideration)) wordsInOrder.Add(wordUnderConsideration, wordPair.Value);
                }
                if (globalVars.getRadioButton(5).Checked)
                {
                    for (idx = 0; idx < 6; idx++)
                    {
                        isFirstInCat = true;
                        if (vocabTypes[idx] == 1)
                        {
                            foreach (KeyValuePair<String, classWord> wordPair in wordsInOrder)
                            {
                                if (idx < 5)
                                {
                                    //  a. Treat the simpler Pos differently;
                                    //  b. Process each Pos in turn
                                    // Check that the pos category for this word is the current one
                                    if (globalVars.getTabControl(0).SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[idx]) == 0;
                                    else isPos = wordPair.Value.CatString[0] == parseLxxOptions[idx];
                                    if (isPos)
                                    {
                                        if (isFirstInCat)
                                        {
                                            col1Text.Add(rowNumber, parseOptionNames[idx] );
                                            isFirstInCat = false;
                                        }
                                        switch (collationCode)
                                        {
                                            case 1: col2Text.Add(rowNumber, wordPair.Value.RootWord ); break;
                                            case 2: col2Text.Add(rowNumber, wordPair.Value.TextWord ); break;
                                            case 3:
                                                col2Text.Add(rowNumber, wordPair.Value.RootWord);
                                                col4Text.Add(rowNumber, wordPair.Value.TextWord);
                                                break;
                                            case 4:
                                                col2Text.Add(rowNumber, wordPair.Value.TextWord);
                                                col4Text.Add(rowNumber, wordPair.Value.RootWord);
                                                break;
                                        }
                                        rowNumber++;
                                    }
                                }
                                else
                                {
                                    for (jdx = 5; jdx < noOfPos; jdx++)
                                    {
                                        if (globalVars.getTabControl(0).SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[jdx]) == 0;
                                        else isPos = wordPair.Value.CatString[0] == parseLxxOptions[jdx];
                                        if (isPos)
                                        {
                                            if (isFirstInCat)
                                            {
                                                col1Text.Add(rowNumber, parseOptionNames[idx]);
                                                isFirstInCat = false;
                                            }
                                            switch (collationCode)
                                            {
                                                case 1: col2Text.Add(rowNumber, wordPair.Value.RootWord); break;
                                                case 2: col2Text.Add(rowNumber, wordPair.Value.TextWord); break;
                                                case 3:
                                                    col2Text.Add(rowNumber, wordPair.Value.RootWord);
                                                    col4Text.Add(rowNumber, wordPair.Value.TextWord);
                                                    break;
                                                case 4:
                                                    col2Text.Add(rowNumber, wordPair.Value.TextWord);
                                                    col4Text.Add(rowNumber, wordPair.Value.RootWord);
                                                    break;
                                            }
                                            rowNumber++;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else  // this means rbtnMixedAlpha is Checked
                {
                    foreach (KeyValuePair<String, classWord> wordPair in wordsInOrder)
                    {
                        jdx = 0;
                        while (jdx < noOfPos)
                        {
                            if (globalVars.getTabControl(0).SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[jdx]) == 0;
                            else isPos = wordPair.Value.CatString[0] == parseLxxOptions[jdx];
                            if (isPos) break;
                            jdx++;
                        }
                        if (jdx < noOfPos) // we found a match
                        {
                            if (jdx > 4) jdx = 5;
                            if (vocabTypes[jdx] == 1)
                            {
                                switch (collationCode)
                                {
                                    case 1: col2Text.Add(rowNumber, wordPair.Value.RootWord); break;
                                    case 2: col2Text.Add(rowNumber, wordPair.Value.TextWord); break;
                                    case 3:
                                        col2Text.Add(rowNumber, wordPair.Value.RootWord);
                                        col4Text.Add(rowNumber, wordPair.Value.TextWord);
                                        break;
                                    case 4:
                                        col2Text.Add(rowNumber, wordPair.Value.TextWord);
                                        col4Text.Add(rowNumber, wordPair.Value.RootWord);
                                        break;
                                }
                                rowNumber++;
                            }
                        }
                    }
                }
            }
            else //As they come rather than alphabetic
            {
                if (globalVars.getRadioButton(6).Checked)
                {
                    for (idx = 0; idx < 6; idx++)
                    {
                        isFirstInCat = true;
                        if (vocabTypes[idx] == 1)
                        {
                            foreach (KeyValuePair<int, classWord> wordPair in wordsInSequence)
                            {
                                if (idx < 5)
                                {
                                    if (globalVars.getTabControl(0).SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[idx]) == 0;
                                    else isPos = wordPair.Value.CatString[0] == parseLxxOptions[idx];
                                    if (isPos)
                                    {
                                        if (isFirstInCat)
                                        {
                                            col1Text.Add(rowNumber, parseOptionNames[idx]);
                                            isFirstInCat = false;
                                        }
                                        if ((collationCode == 1) || (collationCode == 3)) wordUnderConsideration = wordPair.Value.RootWord;
                                        else wordUnderConsideration = wordPair.Value.TextWord;
                                        if (!wordsInOrder.ContainsKey(wordUnderConsideration))
                                        {
                                            switch (collationCode)
                                            {
                                                case 1: col2Text.Add(rowNumber, wordPair.Value.RootWord); break;
                                                case 2: col2Text.Add(rowNumber, wordPair.Value.TextWord); break;
                                                case 3:
                                                    col2Text.Add(rowNumber, wordPair.Value.RootWord);
                                                    col4Text.Add(rowNumber, wordPair.Value.TextWord);
                                                    break;
                                                case 4:
                                                    col2Text.Add(rowNumber, wordPair.Value.TextWord);
                                                    col4Text.Add(rowNumber, wordPair.Value.RootWord);
                                                    break;
                                            }
                                            rowNumber++;
                                            wordsInOrder.Add(wordUnderConsideration, wordPair.Value);
                                        }
                                    }
                                }
                                else
                                {
                                    for (jdx = 5; jdx < noOfPos; jdx++)
                                    {
                                        if (globalVars.getTabControl(0).SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[jdx]) == 0;
                                        else isPos = wordPair.Value.CatString[0] == parseLxxOptions[jdx];
                                        if (isPos)
                                        {
                                            if (isFirstInCat)
                                            {
                                                col1Text.Add(rowNumber, parseOptionNames[idx]);
                                                isFirstInCat = false;
                                            }
                                            if ((collationCode == 1) || (collationCode == 3)) wordUnderConsideration = wordPair.Value.RootWord;
                                            else wordUnderConsideration = wordPair.Value.TextWord;
                                            if (!wordsInOrder.ContainsKey(wordUnderConsideration))
                                            {
                                                switch (collationCode)
                                                {
                                                    case 1: col2Text.Add(rowNumber, wordPair.Value.RootWord); break;
                                                    case 2: col2Text.Add(rowNumber, wordPair.Value.TextWord); break;
                                                    case 3:
                                                        col2Text.Add(rowNumber, wordPair.Value.RootWord);
                                                        col4Text.Add(rowNumber, wordPair.Value.TextWord);
                                                        break;
                                                    case 4:
                                                        col2Text.Add(rowNumber, wordPair.Value.TextWord);
                                                        col4Text.Add(rowNumber, wordPair.Value.RootWord);
                                                        break;
                                                }
                                                rowNumber++;
                                                wordsInOrder.Add(wordUnderConsideration, wordPair.Value);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else // mixed an as used
                {
                    foreach (KeyValuePair<int, classWord> wordPair in wordsInSequence)
                    {
                        jdx = 0;
                        while (jdx < noOfPos)
                        {
                            if (globalVars.getTabControl(0).SelectedIndex == 0) isPos = String.Compare(wordPair.Value.CatString, parseOptions[jdx]) == 0;
                            else isPos = wordPair.Value.CatString[0] == parseLxxOptions[jdx];
                            if (isPos) break;
                            jdx++;
                        }
                        if (jdx < noOfPos) // we found a match
                        {
                            if (jdx > 4) jdx = 5;
                            if (vocabTypes[jdx] == 1)
                            {
                                if ((collationCode == 1) || (collationCode == 3)) wordUnderConsideration = wordPair.Value.RootWord;
                                else wordUnderConsideration = wordPair.Value.TextWord;
                                if (!wordsInOrder.ContainsKey(wordUnderConsideration))
                                {
                                    switch (collationCode)
                                    {
                                        case 1: col2Text.Add(rowNumber, wordPair.Value.RootWord); break;
                                        case 2: col2Text.Add(rowNumber, wordPair.Value.TextWord); break;
                                        case 3:
                                            col2Text.Add(rowNumber, wordPair.Value.RootWord);
                                            col4Text.Add(rowNumber, wordPair.Value.TextWord);
                                            break;
                                        case 4:
                                            col2Text.Add(rowNumber, wordPair.Value.TextWord);
                                            col4Text.Add(rowNumber, wordPair.Value.RootWord);
                                            break;
                                    }
                                    // We're using wordsInOrder simply as a check here
                                    rowNumber++;
                                    wordsInOrder.Add(wordUnderConsideration, wordPair.Value);
                                }
                            }
                        }
                    }
                }
            }
            dgvTarget.RowCount = rowNumber;
            for( idx = 0; idx < rowNumber; idx++ )
            {
                populateCell(0, idx, col1Text);
                populateCell(1, idx, col2Text);
                populateCell(2, idx, col4Text);
            }
        }

        private void populateCell( int colNum, int rowNum, SortedList<int, String> currentCol)
        {
            String currentWord;

            currentCol.TryGetValue(rowNum, out currentWord );
            if ((currentWord != null) && (currentWord.Length > 0))
                dgvTarget.Rows[rowNum].Cells[colNum].Value = currentWord;
        }

        public void copyVocabList( bool isToText, bool isToNotes)
        {
            int idx, noOfRows, maxLen1 = 0, maxLen2 = 0, maxLen3 = 0;
            String output = "";
            String[] col1, col2, col3;
            DataGridView dgvSource;

            dgvSource = globalVars.DgvVocabList;
            noOfRows = dgvSource.Rows.Count;
            col1 = new String[noOfRows];
            col2 = new String[noOfRows];
            col3 = new String[noOfRows];
            for (idx = 0; idx < noOfRows; idx++ )
            {
                if(dgvSource.Rows[idx].Cells[0].Value == null) col1[idx] = "";
                else col1[idx] = dgvSource.Rows[idx].Cells[0].Value.ToString();
                if (dgvSource.Rows[idx].Cells[1].Value == null) col2[idx] = "";
                else col2[idx] = dgvSource.Rows[idx].Cells[1].Value.ToString();
                if (dgvSource.Rows[idx].Cells[2].Value == null) col3[idx] = "";
                else col3[idx] = dgvSource.Rows[idx].Cells[2].Value.ToString();
                if( col1[idx].Length > maxLen1 ) maxLen1 = col1[idx].Length;
                if (col2[idx].Length > maxLen2) maxLen2 = col2[idx].Length;
                if (col3[idx].Length > maxLen3) maxLen3 = col3[idx].Length;
            }
            if( isToText)
            {
                for( idx = 0; idx < noOfRows; idx++ )
                {
                    if (idx > 0) output += "\n";
                    output += col1[idx].PadRight(maxLen1, ' ') + " " + col2[idx].PadRight(maxLen2, ' ') + " " + col3[idx];
                }
            }
            else
            {
                for (idx = 0; idx < noOfRows; idx++)
                {
                    if (idx > 0) output += "\n";
                    output += col1[idx] + "\t" + col2[idx] + "\t" + col3[idx];
                }
            }
            if( ( output == null ) || ( output.Length == 0 ) )
            {
                MessageBox.Show( "You have not populated a Vocabulary List", "Vocab List Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            if( isToNotes )
            {
                note.insertTextIntoNote(output);
            }
            else
            {
                Clipboard.SetText(output);
                MessageBox.Show( "Current Vocabulary List copied to memory", "Vocab List Copy", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
        }
    }
}
