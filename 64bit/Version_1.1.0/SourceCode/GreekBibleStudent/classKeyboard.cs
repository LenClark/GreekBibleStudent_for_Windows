using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace GreekBibleStudent
{
    internal class classKeyboard
    {
        bool isGkMiniscule, isCarriageReturnDown = false, isShiftDown = false;
        Panel keyboardPanel;
        Button[,] gkKeys;
        SortedList<int, String> minisculeKeyFace = new SortedList<int, string>();
        SortedList<int, String> majisculeKeyFace = new SortedList<int, string>();
        Label labEnteredTextLbl;
        Button btnUse;
        TextBox txtEnteredText;
        GroupBox gbTextDestination;

        private delegate void performPanelAddition(Panel targetPanel, Button currentButton);
        private delegate void performPanelGroup(Panel targetPanel, GroupBox currentGB);
        private delegate void performAddLabel(Panel targetPanel, Label currentLabel);
        private delegate void performAddTextbox(Panel targetPanel, TextBox currentTextBox);
        private delegate void performGroupAddition(GroupBox currentGB, RadioButton currentButton);
        private delegate void performToolTipAddition(ToolTip ttTarget, Control ctrlTarget, String hint);
        private delegate void performProgressAdvance(String primaryMessage, String secondaryMessage, bool useSecondary);
        private delegate void performMainFormLabelChange(Label labLabelLbl, String newText);

        classGlobal globalVars;
        frmProgress progressForm;
        classGreek greekOrthography;

        private void addToPanel(Panel targetPanel, Button currentButton)
        {
            targetPanel.Controls.Add(currentButton);
        }

        private void addGroupToPanel(Panel targetPanel, GroupBox currentGB)
        {
            targetPanel.Controls.Add(currentGB);
        }

        private void addLabelToPanel(Panel targetPanel, Label currentLabel)
        {
            targetPanel.Controls.Add(currentLabel);
        }

        private void addTextboxToPanel(Panel targetPanel, TextBox currentTextBox)
        {
            targetPanel.Controls.Add(currentTextBox);
        }

        private void updateTooltip(ToolTip ttTarget, Control ctrlTarget, String hint)
        {
            ttTarget.SetToolTip(ctrlTarget, hint);
        }

        private void addRButtonToGroup(GroupBox currentGB, RadioButton currentButton)
        {
            currentGB.Controls.Add(currentButton);
        }

        private void updateProgress(String mainMessage, String secondaryMessage, bool useSecondary)
        {
            progressForm.incrementProgress(mainMessage, secondaryMessage, useSecondary);
        }

        private void changeLabel(Label labLabelLbl, String newText)
        {
            labLabelLbl.Text = newText;
        }

        public void initialiseKeyboard(classGlobal inConfig, frmProgress inProgress, classGreek inGkOrthog)
        {
            globalVars = inConfig;
            progressForm = inProgress;
            greekOrthography = inGkOrthog;
            keyboardPanel = (Panel)globalVars.PnlKeyboard;
            setupKeyboard();
        }

        private String[,] loadFileData(String fileName, int noOfRows, int noOfCols)
        {
            /*=============================================================================================================*
             *                                                                                                             *
             *                                                loadFileData                                                 *
             *                                                ============                                                 *
             *                                                                                                             *
             *  An integral part of the method, setupHebKeyboard (above).  Used as a somewhat generic mechanism for        *
             *    loading:                                                                                                 *
             *                                                                                                             *
             *    a) Gk and Heb characters;                                                                                *
             *    b) Information for key hints                                                                             *
             *    c) Heb accents                                                                                           *
             *                                                                                                             *
             *  It can be used in a number of files (hence the parameter, fileName).                                       *
             *                                                                                                             *
             *=============================================================================================================*/
            int idx, jdx;
            String fileBuffer;
            String[,] targetArray;
            StreamReader srSource;

            srSource = new StreamReader(fileName);
            targetArray = new String[noOfRows, noOfCols];
            fileBuffer = srSource.ReadLine();
            idx = 0; jdx = 0;
            while (fileBuffer != null)
            {
                if ((fileBuffer.Length > 0) && (fileBuffer[0] == '\\'))
                {
                    fileBuffer = String.Format("{0:C}", (char)int.Parse(fileBuffer.Substring(2), System.Globalization.NumberStyles.HexNumber));
                }
                targetArray[idx, jdx] = fileBuffer;
                if (++jdx == noOfCols)
                {
                    jdx = 0;
                    if (++idx == noOfRows) break;
                }
                fileBuffer = srSource.ReadLine();
            }
            srSource.Close();
            return targetArray;
        }

        private int[,] loadKeyWidths(String fullFileName, int noOfRows, int noOfCols)
        {
            /*=============================================================================================================*
             *                                                                                                             *
             *                                               loadKeyWidths                                                 *
             *                                               =============                                                 *
             *                                                                                                             *
             *  An integral part of the method, setupHebKeyboard (above).  Much as loadFileData (above) but it has to be   *
             *    a different method because the values stored are of a different data type (integer rather than string).  *
             *                                                                                                             *
             *=============================================================================================================*/
            int idx, jdx;
            String fileBuffer;
            int[,] targetArray;
            StreamReader srSource;

            srSource = new StreamReader(fullFileName);
            targetArray = new int[noOfRows, noOfCols];
            fileBuffer = srSource.ReadLine();
            idx = 0; jdx = 0;
            while (fileBuffer != null)
            {
                targetArray[idx, jdx] = Convert.ToInt32(fileBuffer);
                fileBuffer = srSource.ReadLine();
                if (++jdx == noOfCols)
                {
                    jdx = 0;
                    if (++idx == noOfRows) break;
                }
            }
            srSource.Close();
            return targetArray;
        }

        public void setupKeyboard()
        {
            /*=============================================================================================================*
             *                                                                                                             *
             *                                              setupKeyboard                                                  *
             *                                              =============                                                  *
             *                                                                                                             *
             *=============================================================================================================*/
            const int noOfRows = 4, noOfCols = 13, keyGap = 4, keyHeight = 30;

            int idx, keyRow, keyCol, maxForRow, keyWidth, accummulativeWidth, tagCount = 1, baseHeight, rbtnTop = 20, leftRBtn = 15;
            String keyFaceMinName = "gkKeyFaceMin.txt", keyFaceMajName = "gkKeyFaceMaj.txt", keyHintMinName = "gkKeyHintMin.txt", keyHintMajName = "gkKeyHintMaj.txt",
                keyWidthName = "keyWidths.txt", fullFileName, fileBuffer;
            int[,] keyWidths, gkKeyCode;
            //            int[] rbtnLeft = { 15, 90, 224, 300 };
            int[] rbtnLeft;
            String[] radioButtonText = { "Notes", "Primary Search Word", "Secondary Search Word" };
            String[,] gkKeyFace, gkKeyHint, gkKeyVal;
            StreamReader srSource;
            ToolTip[,] gkToolTips = new ToolTip[noOfRows, noOfCols];
            RadioButton rbtnTemp;
            Font typicalFont;

            progressForm.Invoke(new performProgressAdvance(updateProgress), "Creating the Virtual Keyboard", "", false);
            isGkMiniscule = true;
            fullFileName = globalVars.KeyboardFolder + @"\" + keyWidthName;
            keyWidths = loadKeyWidths(fullFileName, noOfRows, noOfCols);
            /***************************************
             * 
             * gkKeys: a global array containing all references to each key
             */
            gkKeys = new Button[noOfRows, noOfCols];

            /***************************************
             * 
             * keyCode: a global array containing the physical key data for each key (if scanned)
             */
            gkKeyCode = new int[noOfRows, noOfCols];
            gkKeyVal = new String[noOfRows, noOfCols];
            /****************************************
             * 
             * Now actually create the two sets of keys
             */
            fullFileName = globalVars.KeyboardFolder + @"\" + keyFaceMinName;
            gkKeyFace = loadFileData(fullFileName, noOfRows, noOfCols);
            fullFileName = globalVars.KeyboardFolder + @"\" + keyHintMinName;
            gkKeyHint = loadFileData(fullFileName, noOfRows, noOfCols);
            initialiseGkKeyCode(noOfRows, noOfCols, ref gkKeyCode, ref gkKeyVal);
            maxForRow = 0;
            baseHeight = 8;
            for (keyRow = 0; keyRow < noOfRows; keyRow++)
            {
                switch (keyRow)
                {
                    case 0:
                    case 1:
                    case 2: maxForRow = noOfCols; break;
                    case 3: maxForRow = 13; break;
                    case 4: maxForRow = 8; break;
                }
                accummulativeWidth = 16;
                for (keyCol = 0; keyCol < maxForRow; keyCol++)
                {
                    keyWidth = keyWidths[keyRow, keyCol];
                    gkKeys[keyRow, keyCol] = new Button();
                    if( String.Compare( gkKeyFace[keyRow, keyCol], "*" ) == 0 )
                    {
                        gkKeys[keyRow, keyCol].Image = Image.FromFile(globalVars.KeyboardFolder + @"\CapsLock.png");
                    }
                    else
                    {
                        if (String.Compare(gkKeyFace[keyRow, keyCol], "^") == 0)
                        {
                            gkKeys[keyRow, keyCol].Image = Image.FromFile(globalVars.KeyboardFolder + @"\shift.png");
                        }
                        else
                        {
                            gkKeys[keyRow, keyCol].Text = gkKeyFace[keyRow, keyCol];
                        }
                    }
                    minisculeKeyFace.Add(tagCount, gkKeyFace[keyRow, keyCol]);
                    gkKeys[keyRow, keyCol].TextAlign = ContentAlignment.MiddleCenter;
                    gkKeys[keyRow, keyCol].Left = accummulativeWidth;
                    gkKeys[keyRow, keyCol].Top = baseHeight + (keyRow * (keyHeight + keyGap));
                    gkKeys[keyRow, keyCol].Height = keyHeight;
                    gkKeys[keyRow, keyCol].Width = keyWidth;
                    gkKeys[keyRow, keyCol].Font = new Font("Times New Roman", 10);
                    gkKeys[keyRow, keyCol].Tag = tagCount;
                    gkKeys[keyRow, keyCol].Click += gkKeyboard_button_click;
                    keyboardPanel.Invoke(new performPanelAddition(addToPanel), keyboardPanel, gkKeys[keyRow, keyCol]);

                    gkToolTips[keyRow, keyCol] = new ToolTip();
                    gkToolTips[keyRow, keyCol].AutomaticDelay = 200;
                    gkToolTips[keyRow, keyCol].AutoPopDelay = 2147483647;
                    gkToolTips[keyRow, keyCol].ToolTipTitle = "Key value";
                    keyboardPanel.Invoke(new performToolTipAddition(updateTooltip), gkToolTips[keyRow, keyCol], gkKeys[keyRow, keyCol], gkKeyHint[keyRow, keyCol]);

                    accummulativeWidth += keyWidth + keyGap;
                    tagCount++;
                }
            }
            rbtnLeft = new int[4];
            typicalFont = new Font("Microsoft Sans Serif", 8.25F);
            for (idx = 0; idx < 3; idx++)
            {
                rbtnLeft[idx] = leftRBtn;
                leftRBtn += TextRenderer.MeasureText(radioButtonText[idx], typicalFont).Width + 25;
            }
            gbTextDestination = new GroupBox();
            gbTextDestination.Left = 12;
            gbTextDestination.Top = baseHeight + (noOfRows * (keyHeight + keyGap));
            gbTextDestination.Height = 50;
            gbTextDestination.Width = leftRBtn;
            gbTextDestination.Text = "Direct Greek text to: ";
            //            mtKeyboardPanel.Controls.Add(gbTextDestination);
            keyboardPanel.Invoke(new performPanelGroup(addGroupToPanel), keyboardPanel, gbTextDestination);
            leftRBtn = 15;
            globalVars.RbtnDestination = new RadioButton[3];
            for (idx = 0; idx < 3; idx++)
            {
                rbtnTemp = new RadioButton();
                rbtnTemp.Left = rbtnLeft[idx];
                rbtnTemp.Top = rbtnTop;
                rbtnTemp.AutoSize = true;
                rbtnTemp.Text = radioButtonText[idx];
                if (idx == 0) rbtnTemp.Checked = true;
                gbTextDestination.Invoke(new performGroupAddition(addRButtonToGroup), gbTextDestination, rbtnTemp);
                globalVars.RbtnDestination[idx] = rbtnTemp;
                leftRBtn += TextRenderer.MeasureText(radioButtonText[idx], rbtnTemp.Font).Width + 25;
            }
            labEnteredTextLbl = new Label();
            labEnteredTextLbl.Left = gbTextDestination.Left + gbTextDestination.Width + 10; ;
            labEnteredTextLbl.Top = gbTextDestination.Top + (gbTextDestination.Height / 2) - 4;
            labEnteredTextLbl.AutoSize = true;
            labEnteredTextLbl.Text = "Entered text:";
            keyboardPanel.Invoke(new performAddLabel(addLabelToPanel), keyboardPanel, labEnteredTextLbl);
            txtEnteredText = new TextBox();
            txtEnteredText.Left = labEnteredTextLbl.Left + labEnteredTextLbl.Width + 5;
            txtEnteredText.Top = labEnteredTextLbl.Top - 5;
            txtEnteredText.Width = 120;
            txtEnteredText.Font = new Font("Times New Roman", 12F, FontStyle.Regular);
            keyboardPanel.Invoke(new performAddTextbox(addTextboxToPanel), keyboardPanel, txtEnteredText);
            btnUse = new Button();
            btnUse.Left = labEnteredTextLbl.Left + labEnteredTextLbl.Width + 25;
            btnUse.Top = labEnteredTextLbl.Top - keyHeight - 15;
            btnUse.Height = keyHeight;
            btnUse.Width = keyWidths[3, 11];
            btnUse.Text = "Use word";
            btnUse.Tag = 2;
            btnUse.Click += respondToUseButton;
            keyboardPanel.Invoke(new performPanelAddition(addToPanel), keyboardPanel, btnUse);
            // Load the majiscule key-face text, ready;
            tagCount = 1;
            fullFileName = globalVars.KeyboardFolder + @"\" + keyFaceMajName;
            srSource = new StreamReader(fullFileName);
            fileBuffer = srSource.ReadLine();
            while (fileBuffer != null)
            {
                if ((fileBuffer.Length > 0) && (fileBuffer[0] == '\\'))
                {
                    fileBuffer = String.Format("{0:C}", (char)int.Parse(fileBuffer.Substring(2), System.Globalization.NumberStyles.HexNumber));
                }
                majisculeKeyFace.Add(tagCount++, fileBuffer);
                fileBuffer = srSource.ReadLine();
            }
            srSource.Close();
            progressForm.Invoke(new performProgressAdvance(updateProgress), "Virtual Keyboard Created", "", false);
            globalVars.getLabel(2).Invoke(new performMainFormLabelChange(changeLabel), globalVars.getLabel(2), "Virtual Keyboard Created");
        }

        private void respondToUseButton(object sender, EventArgs e)
        {
            if (globalVars.RbtnDestination[0].Checked) updateNotes(globalVars.getRichtextItem(6));
            if (globalVars.RbtnDestination[1].Checked) updateSearchBox(globalVars.getTextbox(0), false);
            if (globalVars.RbtnDestination[2].Checked) updateSearchBox(globalVars.getTextbox(1), true);
        }


        private void initialiseGkKeyCode(int x, int y, ref int[,] gkKeyCode, ref String[,] gkKeyVal)
        {
            /*=============================================================================================================*
             *                                                                                                             *
             *                                             initialiseGkKeyCode                                             *
             *                                             ===================                                             *
             *                                                                                                             *
             *  An integral part of the method, setupGkKeyboard (above).                                                   *
             *                                                                                                             *
             *=============================================================================================================*/
            int idx, jdx;

            gkKeyCode = new int[x, y];
            gkKeyVal = new string[x, y];
            for (idx = 0; idx < x; idx++)
            {
                for (jdx = 0; jdx < y; jdx++)
                {
                    gkKeyCode[idx, jdx] = -1;
                    gkKeyVal[idx, jdx] = "";
                }
            }
        }
        private void gkKeyboard_button_click(object sender, EventArgs e)
        {
            /*==========================================================================================================*
             *                                                                                                          *
             *                                        gkKeyboard_button_click                                           *
             *                                        =======================                                           *
             *                                                                                                          *
             *  Specifically handles key presses from the Greek virtual keyboard.  (see hebKeyboard_button_click for    *
             *    responses to the Hebrew keyboard).                                                                    *
             *                                                                                                          *
             *==========================================================================================================*/
            bool isNormalChar = false, isModifyingChar = false, isNormalNonGk = false;
            int clickedTag;
            String keyVal = "";
            Button clickedButton;

            clickedButton = (Button)sender;
            clickedTag = Convert.ToInt32(clickedButton.Tag.ToString());
            if (isGkMiniscule)
            {
                if ((clickedTag > 0) && (clickedTag < 11)) isNormalNonGk = true;
                if ((clickedTag == 11) || (clickedTag == 12) || (clickedTag == 14)) isModifyingChar = true;
                if ((clickedTag > 14) && (clickedTag < 24)) isNormalChar = true;
                if ((clickedTag == 24) || (clickedTag == 25)) isModifyingChar = true;
                if ((clickedTag > 27) && (clickedTag < 37)) isNormalChar = true;
                if ((clickedTag == 37) || (clickedTag == 38)) isModifyingChar = true;
                if ((clickedTag > 40) && (clickedTag < 48)) isNormalChar = true;
                if ((clickedTag == 48) || (clickedTag == 49) || (clickedTag == 51)) isNormalChar = true;
                if (isNormalChar || isNormalNonGk) keyVal = clickedButton.Text;
                if (clickedTag == 51) keyVal = " ";
            }
            if ((clickedTag == 27) || (clickedTag == 40) || (clickedTag == 50)) handleShiftAndCapsLoc(clickedTag);
            else
            {
                updateTextbox(keyVal, clickedTag, isModifyingChar, isNormalChar);
                /*        if (globalVars.RbtnLXXDestination[0].Checked)
                            updateGkNotes(keyVal, clickedTag, isModifyingChar, isNormalChar, (RichTextBox)globalVars.getGroupedControl(globalVars.RichtextBoxCode, 3));
                        if (globalVars.RbtnLXXDestination[1].Checked)
                            updateGkSearchBox(keyVal, clickedTag, isModifyingChar, isNormalNonGk, (TextBox)globalVars.getGroupedControl(globalVars.TextboxCode, 2), false);
                        if (globalVars.RbtnLXXDestination[2].Checked)
                            updateGkSearchBox(keyVal, clickedTag, isModifyingChar, isNormalNonGk, (TextBox)globalVars.getGroupedControl(globalVars.TextboxCode, 3), true); */
            }
            if (isNormalChar)
            {
                if (isShiftDown && (!isCarriageReturnDown))
                {
                    resetGkKeyboard(true);
                    isShiftDown = false;
                }
            }
        }

        private void handleShiftAndCapsLoc(int keyCode)
        {
            if (keyCode == 27)
            {
                if (isCarriageReturnDown)
                {
                    resetGkKeyboard(true);
                    isCarriageReturnDown = false;
                }
                else
                {
                    resetGkKeyboard(false);
                    isCarriageReturnDown = true;
                }
            }
            else
            {
                if (!isCarriageReturnDown)
                {
                    if (isShiftDown)
                    {
                        resetGkKeyboard(true);
                        isShiftDown = false;
                    }
                    else
                    {
                        resetGkKeyboard(false);
                        isShiftDown = true;
                    }
                }
            }
        }

        private void resetGkKeyboard(bool isMiniscule)
        {
            const int noOfRows = 4, noOfCols = 13;

            int keyRow, keyCol, maxForRow = 0, tagCount = 1;
            String keyText;

            for (keyRow = 0; keyRow < noOfRows; keyRow++)
            {
                switch (keyRow)
                {
                    case 0:
                    case 1:
                    case 2: maxForRow = noOfCols; break;
                    case 3: maxForRow = 13; break;
                    case 4: maxForRow = 8; break;
                }
                for (keyCol = 0; keyCol < maxForRow; keyCol++)
                {
                    if (isMiniscule) minisculeKeyFace.TryGetValue(tagCount, out keyText);
                    else majisculeKeyFace.TryGetValue(tagCount, out keyText);
                    if (String.Compare( keyText, "*") == 0)
                    {
                        gkKeys[keyRow, keyCol].Image = Image.FromFile(globalVars.KeyboardFolder + @"\CapsLock.png");
                    }
                    else
                    {
                        if (String.Compare(keyText, "^") == 0)
                        {
                            gkKeys[keyRow, keyCol].Image = Image.FromFile(globalVars.KeyboardFolder + @"\shift.png");
                        }
                        else
                        {
                            gkKeys[keyRow, keyCol].Text = keyText;
                        }
                    }
                    tagCount++;
                }
            }
        }

        private void updateTextbox(String keyVal, int clickCode, bool isModifyingChar, bool isNormalChar)
        {
            int currPstn;
            String fullNote, previousChar, replacementChar = null, beforeCursor, afterCursor;

            fullNote = txtEnteredText.Text;
            currPstn = txtEnteredText.SelectionStart;
            if (isModifyingChar)
            {
                if ((fullNote == null) || (fullNote.Length == 0)) return;
                if (currPstn == 0) return;
                if (currPstn == fullNote.Length - 1)
                {
                    previousChar = txtEnteredText.Text.Substring(txtEnteredText.Text.Length - 1);
                    replacementChar = modifyCharacter(previousChar, clickCode);
                    txtEnteredText.Text = txtEnteredText.Text.Substring(0, txtEnteredText.Text.Length - 1) + replacementChar;
                }
                else
                {
                    beforeCursor = fullNote.Substring(0, currPstn);
                    afterCursor = fullNote.Substring(currPstn);
                    previousChar = beforeCursor.Substring(beforeCursor.Length - 1);
                    replacementChar = modifyCharacter(previousChar, clickCode);
                    txtEnteredText.Text = beforeCursor.Substring(0, beforeCursor.Length - 1) + replacementChar + afterCursor;
                    currPstn = beforeCursor.Length;
                    txtEnteredText.SelectionStart = currPstn;
                }
            }
            else
            {
                if (isNormalChar)
                {
                    if ((fullNote == null) || (fullNote.Length == 0))
                    {
                        txtEnteredText.Text = keyVal;
                        txtEnteredText.SelectionStart = 1;
                        return;
                    }
                    if (currPstn == 0)
                    {
                        txtEnteredText.Text = keyVal + fullNote;
                        txtEnteredText.SelectionStart = 1;
                        return;
                    }
                    if (currPstn == fullNote.Length - 1)
                    {
                        txtEnteredText.Text = fullNote + keyVal;
                        txtEnteredText.SelectionStart = ++currPstn;
                        return;
                    }
                    beforeCursor = fullNote.Substring(0, currPstn);
                    afterCursor = fullNote.Substring(currPstn);
                    txtEnteredText.Text = beforeCursor + keyVal + afterCursor;
                    txtEnteredText.SelectionStart = ++currPstn;
                }
                else
                {
                    switch (clickCode)
                    {
                        case 13: // Backspace
                        case 26: // Delete - treated as backspace
                            if ((fullNote == null) || (fullNote.Length == 0)) return;
                            if (currPstn == 0) return;
                            if (currPstn == fullNote.Length - 1)
                            {
                                txtEnteredText.Text = fullNote.Substring(0, fullNote.Length - 1);
                                txtEnteredText.SelectionStart = --currPstn;
                            }
                            else
                            {
                                beforeCursor = fullNote.Substring(0, currPstn);
                                afterCursor = fullNote.Substring(currPstn);
                                txtEnteredText.Text = beforeCursor.Substring(0, beforeCursor.Length - 1) + afterCursor;
                                currPstn = beforeCursor.Length - 1;
                                txtEnteredText.SelectionStart = currPstn;
                            }
                            break;
                        case 39: txtEnteredText.Text = ""; break;
                    }
                }
            }
        }

        private String modifyCharacter(String startingCharacter, int clickCode)
        {
            String replacementChar = null;

            switch (clickCode)
            {
                case 11: replacementChar = greekOrthography.getCharacterWithDieresis(startingCharacter); break;
                case 12: replacementChar = greekOrthography.getCharacterWithAccuteAccent(startingCharacter); break;
                case 14: replacementChar = greekOrthography.getCharacterWithIotaSubscript(startingCharacter); break;
                case 24: replacementChar = greekOrthography.getCharacterWithCircumflexAccent(startingCharacter); break;
                case 25: replacementChar = greekOrthography.getCharacterWithGraveAccent(startingCharacter); break;
                case 37: replacementChar = greekOrthography.getCharacterWithRoughBreathing(startingCharacter); break;
                case 38: replacementChar = greekOrthography.getCharacterWithSmoothBreathing(startingCharacter); break;
            }
            if (replacementChar == null) return startingCharacter;
            return replacementChar;
        }

        private void updateNotes(RichTextBox rtxtNotes)
        {
            int currPstn;
            String newWord, fullNote, beforeCursor, afterCursor;

                newWord = txtEnteredText.Text;
                globalVars.getTabControl(2).SelectedIndex = 0;
            fullNote = rtxtNotes.Text;
            currPstn = rtxtNotes.SelectionStart;

            if ((fullNote == null) || (fullNote.Length == 0))
            {
                rtxtNotes.Text = newWord;
                rtxtNotes.SelectionStart = rtxtNotes.Text.Length;
                return;
            }
            if (currPstn == 0)
            {
                rtxtNotes.Text = newWord + fullNote;
                rtxtNotes.SelectionStart = newWord.Length;
                return;
            }
            if (currPstn == fullNote.Length - 1)
            {
                rtxtNotes.Text = fullNote + newWord;
                rtxtNotes.SelectionStart = rtxtNotes.Text.Length;
                return;
            }
            beforeCursor = fullNote.Substring(0, currPstn);
            afterCursor = fullNote.Substring(currPstn);
            rtxtNotes.Text = beforeCursor + newWord + afterCursor;
            rtxtNotes.SelectionStart = currPstn + newWord.Length;

        }

        private void updateSearchBox(TextBox txtTarget, bool isSecondary)
        {
            Label lblFirst = null, lblSecond = null;
            Button btnSecondarySearch = null;
            NumericUpDown udSearch = null;

                globalVars.getTabControl(2).SelectedIndex = 2;
                if (isSecondary)
                {
                    lblFirst = globalVars.getLabel(0);  // within
                    lblSecond = globalVars.getLabel(1);  // wordsOf
                    udSearch = globalVars.UdWordDistance;
                    btnSecondarySearch = globalVars.BtnSearchType;
                    lblFirst.Visible = true;
                    lblSecond.Visible = true;
                    udSearch.Visible = true;
                    txtTarget.Visible = true;
                    btnSecondarySearch.Text = "Basic Search";
                    globalVars.SecondaryBookId = -1;
                    globalVars.SecondaryChapNo = "";
                    globalVars.SecondaryVNo = "";
                    globalVars.SecondaryWordSeq = -1;
                    globalVars.SecondaryWord = txtEnteredText.Text;
                }
                else
                {
                    globalVars.PrimaryBookId = -1;
                    globalVars.PrimaryChapNo = "";
                    globalVars.PrimaryVNo = "";
                    globalVars.PrimaryWordSeq = -1;
                    globalVars.PrimaryWord = txtEnteredText.Text;
                }
                txtTarget.Text = txtEnteredText.Text;
        }

        public bool getDestinationButtonStatus(int whichButton)
        {
            return globalVars.RbtnDestination[whichButton].Checked;
        }
    }
}
