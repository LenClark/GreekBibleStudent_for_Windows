using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace GreekBibleStudent
{
    public class classGlobal
    {
        /*========================================================================================================================*
         *                                                                                                                        *
         *                                                       Global Variable                                                  * 
         *                                                       ---------------                                                  * 
         *                                                                                                                        *
         *  The purpose of this calss is to manage all variables that are global to the entire project in one, easily             *
         *  identifiable place.                                                                                                   *
         *                                                                                                                        *
         *========================================================================================================================*/

        bool isReady, isDisplayChapterCalled;

        public bool IsReady { get => isReady; set => isReady = value; }
        public bool IsDisplayChapterCalled { get => isDisplayChapterCalled; set => isDisplayChapterCalled = value; }

        /*------------------------------------------------------------------------------------------------------------------------*
         *                                                                                                                        *
         *                                 Variables controling form sizes and positions                                          *
         *                                 ---------------------------------------------                                          *
         *                                                                                                                        *
         *  windowX              The left pixel position in the main window/desktop                                               *
         *  windowY              The top position of the application form                                                         *
         *  windowHeight         The height of the main form                                                                      *
         *  windowWidth          The width of the main form                                                                       *
         *  windowState          Whether the window is maximised, minimised or normal                                             *
         *  splitPstn            The position of the split bar of the main, outer splitter container                              *     
         *  historyMax           The maximum number of past references that are retained by the application.  (This is a constant *
         *                       but including it as a global variable means it can be changed in the future)                     *    
         *  displayTextCode      ?                                                                                                *
         *  latestMousePosition  Stored whenever the user clicks on the main text area                                            *
         *  latestNotesPosition  Similar activity for notes                                                                       *
         *                                                                                                                        *
         *------------------------------------------------------------------------------------------------------------------------*/

        int windowX, windowY, windowWidth, windowHeight, windowState, splitPstn, historyMax, displayTextCode = 0, latestMousePosition = 0, latestNotesPosition = 0;

        public int WindowX { get => windowX; set => windowX = value; }
        public int WindowY { get => windowY; set => windowY = value; }
        public int WindowWidth { get => windowWidth; set => windowWidth = value; }
        public int WindowHeight { get => windowHeight; set => windowHeight = value; }
        public int WindowState { get => windowState; set => windowState = value; }
        public int SplitPstn { get => splitPstn; set => splitPstn = value; }
        public int HistoryMax { get => historyMax; set => historyMax = value; }
        public int DisplayTextCode { get => displayTextCode; set => displayTextCode = value; }
        public int LatestMousePosition { get => latestMousePosition; set => latestMousePosition = value; }
        public int LatestNotesPosition { get => latestNotesPosition; set => latestNotesPosition = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        *
         *                         Variables controling and storing references to main window controls                            *
         *                         -------------------------------------------------------------------                            *
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/


        /*------------------------------------------------------------------------------------------------------------------------*
         *                                                                                                                        *
         *  Richtext boxes     objectTypeCode = 1                                                                                 *
         *  --------------                                                                                                        *
         *                                                                                                                        *
         *  Index     Specific Richtext Control                                                                                   *
         *  -----     -------------------------                                                                                   *
         *                                                                                                                        *
         *    0         rtxtNTMainText                                                                                            *
         *    1         rtxtLxxMainText                                                                                           *
         *    2         rtxtParse                                                                                                 *
         *    3         rtxtLexicon                                                                                               *
         *    4         rtxtSearchResults                                                                                         *
         *    5         rtxtVocabList                                                                                             *
         *    6         rtxtNotes                                                                                                 *
         *                                                                                                                        *
         *------------------------------------------------------------------------------------------------------------------------*/

        int noOfRichtextBoxes = 0;
        SortedList<int, RichTextBox> rtxtCollection = new SortedList<int, RichTextBox>();

        public int NoOfRichtextBoxes { get => noOfRichtextBoxes; }

        public void addRichtextItem( RichTextBox newItem)
        {
            rtxtCollection.Add( noOfRichtextBoxes++, newItem );
        }

        public RichTextBox getRichtextItem( int index )
        {
            RichTextBox requiredRichTextBox;

            if( index >= noOfRichtextBoxes)
            {
                MessageBox.Show( "That RichText area doesn't exist", "RichTextBox error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return null;
            }
            rtxtCollection.TryGetValue( index, out requiredRichTextBox );
            return requiredRichTextBox;
        }

        /*------------------------------------------------------------------------------------------------------------------------*
         *                                                                                                                        *
         *  Combo Boxes      objectTypeCode = 2                                                                                   *
         *  -----------                                                                                                           *
         *                                                                                                                        *
         *  Index     Specific Combobox                        Index     Specific Combobox                                        *
         *  -----     -----------------                        -----     -----------------                                        *
         *                                                                                                                        *
         *    0         cbNTBook                                                                                                  *
         *    1         cbNTChapter                                                                                               *
         *    2         cbNTVerse                                                                                                 *
         *    3         cbLXXBook                                                                                                 *
         *    4         cbLXXChapter                                                                                              *
         *    5         cbLXXVerse                                                                                                *
         *    6         cbNTHistory                                                                                               *
         *    7         cbLXXHistory                                                                                              *
         *                                                                                                                        *
         *------------------------------------------------------------------------------------------------------------------------*/

        int noOfComboBoxes = 0;
        SortedList<int, ComboBox> cbCollection = new SortedList<int, ComboBox>();

        public int NoOfComboBoxes { get => noOfComboBoxes; }

        public void addComboBoxItem(ComboBox newItem)
        {
            cbCollection.Add(noOfComboBoxes++, newItem);
        }

        public ComboBox getComboBoxItem(int index)
        {
            ComboBox requiredComboBox;

            if (index >= noOfComboBoxes)
            {
                MessageBox.Show("That ComboBox doesn't exist", "ComboBox error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            cbCollection.TryGetValue(index, out requiredComboBox);
            return requiredComboBox;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Tab Controls     objectTypeCode = 3                                                                *
         *  ------------                                                                                       *
         *                                                                                                     *
         *    0         tabCtrlText (at the head of the main text area)                                        *
         *    1         tabCtrlTop (the top right tab area)                                                    *
         *    2         tabCtrlBottom (bottom right tab-area)                                                  *
         *    3         tabCtrlLSAppendices (very bottom right, probably not needed)                           *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfTabControls = 0;
        SortedList<int, TabControl> tabControlCollection = new SortedList<int, TabControl>();
        public int NoOfTabControls { get => noOfTabControls; set => noOfTabControls = value; }

        public void addTabControl(TabControl newItem)
        {
            tabControlCollection.Add(noOfTabControls++, newItem);
        }

        public TabControl getTabControl(int index)
        {
            TabControl requiredTabControl;

            if (index >= noOfTabControls)
            {
                MessageBox.Show("That Tab Control doesn't exist", "Tab Control error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            tabControlCollection.TryGetValue(index, out requiredTabControl);
            return requiredTabControl;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Tab control pages      objectTypeCode = 4                                                          *
         *  -----------------                                                                                  *
         *                                                                                                     *
         *  Index     Specific Page                                                                            *
         *                                                                                                     *
         *    0         tabPgeNT - Main Control - New Testament Text                                           *
         *    1         tabPgeLxx - Main Control - Septuagint Text                                             *
         *    2         tabPgeParse - Parse page                                                               *
         *    3         tabPgeLexicon - Lexicon page                                                           *
         *    4         tabPgeSearchResults - Search Results page                                              *
         *    5         tabPgeVocabList - Vocab List page                                                      *
         *    6         tabPgeNotes - Notes page                                                               *
         *    7         tabPgeLSAppendices - Liddell & Scott Appendices                                        *
         *    8         tabPgeSearch - Search Setup page                                                       *
         *    9         tabPgeVocabSetup - Setup Page for Vocab Lists                                          *
         *                                                                                                     *
         *   The six tabs withing the Liddell & Scott appendices do not need to be stored globally.            *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfTabPages = 0;
        SortedList<int, TabPage> tabPageCollection = new SortedList<int, TabPage>();
        public int NoOfTabPages { get => noOfTabPages; set => noOfTabPages = value; }

        public void addTabCtrlPage(TabPage newItem)
        {
            tabPageCollection.Add(noOfTabPages++, newItem);
        }

        public TabPage getTabCtrlPage(int index)
        {
            TabPage requiredTabCtrlPage;

            if (index >= noOfTabPages)
            {
                MessageBox.Show("That Tab Control Page doesn't exist", "Tab Control Page error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            tabPageCollection.TryGetValue(index, out requiredTabCtrlPage);
            return requiredTabCtrlPage;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Split Containers objectTypeCode = 5                                                                *
         *  ----------------                                                                                   *
         *                                                                                                     *
         *    0         splitMain (Covers effectively the whole form)                                          *
         *    1         splitLeft (Left main area)                                                             *
         *    3         splitRight (Right main area)                                                           *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfSplitContainers = 0;
        SortedList<int, SplitContainer> splitContainerCollection = new SortedList<int, SplitContainer>();
        public int NoOfSplitContainers { get => noOfSplitContainers; set => noOfSplitContainers = value; }

        public void addSplit(SplitContainer newItem)
        {
            splitContainerCollection.Add(noOfSplitContainers++, newItem);
        }

        public SplitContainer getSplit(int index)
        {
            SplitContainer requiredSplitContainer;

            if (index >= noOfSplitContainers)
            {
                MessageBox.Show("That Split Container doesn't exist", "Split Container error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            splitContainerCollection.TryGetValue(index, out requiredSplitContainer);
            return requiredSplitContainer;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Text boxes (needing global access) objectTypeCode = 6                                              *
         *  --------------------------                                                                         *
         *                                                                                                     *
         *  Index     Specific Textbox                                                                         *
         *  -----     ----------------                                                                         *
         *                                                                                                     *
         *    0      txtPrimaryWord (Search function)                                                          *
         *    1      txtSecondaryWord (Search function)                                                        *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfTextboxes = 0;
        SortedList<int, TextBox> textboxCollection = new SortedList<int, TextBox>();
        public int NoOfTextboxes { get => noOfTextboxes; set => noOfTextboxes = value; }

        public void addTextbox(TextBox newItem)
        {
            textboxCollection.Add(noOfTextboxes++, newItem);
        }

        public TextBox getTextbox(int index)
        {
            TextBox requiredTextbox;

            if (index >= noOfTextboxes)
            {
                MessageBox.Show("That Textbox doesn't exist", "Textbox error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            textboxCollection.TryGetValue(index, out requiredTextbox);
            return requiredTextbox;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  RadioButtons (needing global access)     objectTypeCode = 7                                        *
         *  ------------                                                                                       *
         *                                                                                                     *
         *  Index     Specific Radio Button                                                                    *
         *  -----     ---------------------                                                                    *
         *                                                                                                     *
         *    0      rbtnRoots (under "Search Options")                                                        *
         *    1      rbtnExact (under "Search Options")                                                        *
         *    2      rbtnUse                                                                                   *
         *    3      rbtnOmit                                                                                  *
         *    4      rbtnVocabVerse       |  From here onwards, radio buttons in the Vocab List setup          *
         *    5      rbtnByCatAlpha       v                                                                    *
         *    6      rbtnByTypeRandomnly                                                                       *
         *    7      rbtnMixedAlpha                                                                            *
         *    8      rbtnMixedRandomnly                                                                        *
         *    9      rbtnShowRoots                                                                             *
         *   10      rbtnShowAsUsed                                                                            *
         *   11      rbtnShowBothByRoot                                                                        *
         *   12      rbtnShowBothByWord                                                                        *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfRadioButtons = 0;
        SortedList<int, RadioButton> radioButtonCollection = new SortedList<int, RadioButton>();
        public int NoOfRadioButtons { get => noOfRadioButtons; set => noOfRadioButtons = value; }

        public void addRadioButton(RadioButton newItem)
        {
            radioButtonCollection.Add(noOfRadioButtons++, newItem);
        }

        public RadioButton getRadioButton(int index)
        {
            RadioButton requiredRadioButton;

            if (index >= noOfRadioButtons)
            {
                MessageBox.Show("That RadioButton doesn't exist", "RadioButton error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            radioButtonCollection.TryGetValue(index, out requiredRadioButton);
            return requiredRadioButton;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Web Browser controls                                            objectTypeCode = 8                 *
         *  ---------------------                                                                              *
         *                                                                                                     *
         *    0         webGeneral                                                                             *
         *    1         webAuthors                                                                             *
         *    2         webEpigraphy                                                                           *
         *    3         webPapyrology                                                                          *
         *    4         webPeriodicals                                                                         *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfWebBrowsers = 0;
        SortedList<int, WebBrowser> webBrowserCollection = new SortedList<int, WebBrowser>();

        public int NoOfWebBrowsers { get => noOfWebBrowsers; set => noOfWebBrowsers = value; }

        public void addWebBrowser(WebBrowser newItem)
        {
            webBrowserCollection.Add(noOfWebBrowsers++, newItem);
        }

        public WebBrowser getWebBrowser(int index)
        {
            WebBrowser requiredWebBrowser;

            if (index >= noOfWebBrowsers)
            {
                MessageBox.Show("That WebBrowser doesn't exist", "WebBrowser error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            webBrowserCollection.TryGetValue(index, out requiredWebBrowser);
            return requiredWebBrowser;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Web Content File Names                                            objectTypeCode = 9               *
         *  ---------------------                                                                              *
         *                                                                                                     *
         *    0         webGeneral                                                                             *
         *    1         webAuthors                                                                             *
         *    2         webEpigraphy                                                                           *
         *    3         webPapyrology                                                                          *
         *    4         webPeriodicals                                                                         *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfAppendixFiles = 0;
        SortedList<int, String> appendixFileNames = new SortedList<int, string>();

        public int NoOfAppendixFiles { get => noOfAppendixFiles; set => noOfAppendixFiles = value; }

        public void addWebFileName(String newItem)
        {
            appendixFileNames.Add(noOfAppendixFiles++, newItem);
        }

        public String getWebFileName(int index)
        {
            String requiredWebFileName;

            if (index >= noOfAppendixFiles)
            {
                MessageBox.Show("That Web File Nasme doesn't exist", "Web File Name error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            appendixFileNames.TryGetValue(index, out requiredWebFileName);
            return requiredWebFileName;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Labels (needing global access)     objectTypeCode = 10                                             *
         *  ------                                                                                             *
         *                                                                                                     *
         *  Index     Specific Textbox                                                                         *
         *  -----     ----------------                                                                         *
         *                                                                                                     *
         *    0      labWithinLbl                                                                              *
         *    1      labWordsOfLbl                                                                             *
         *    2      labLoadLbl                                                                                *
         *    3      labListMsg                                                                                *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfLabels = 0;
        SortedList<int, Label> labelCollection = new SortedList<int, Label>();

        public int NoOfLabels { get => noOfLabels; set => noOfLabels = value; }

        public void addLabel(Label newItem)
        {
            labelCollection.Add(noOfLabels++, newItem);
        }

        public Label getLabel(int index)
        {
            Label requiredLabel;

            if (index >= noOfLabels)
            {
                MessageBox.Show("That Label doesn't exist", "Label error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            labelCollection.TryGetValue(index, out requiredLabel);
            return requiredLabel;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  CheckBoxes (needing global access)     objectTypeCode = 11                                         *
         *  ------------                                                                                       *
         *                                                                                                     *
         *  Index     Specific Radio Button                     tag value                                      *
         *  -----     ---------------------                     ---------                                      *
         *                                                                                                     *
         *    0      chkMoses                                       1                                          *
         *    1      chkHistory                                     2                                          *
         *    2      chkMajorProphets                               3                                          *
         *    3      chkProphets                                    4                                          *
         *    4      chkKethubimHistory                             5                                          *
         *    5      chkKethubimPoetry                              6                                          *
         *    6      chkPseudepigrapha                              7                                          *
         *    7      chkNarratives                                  8                                          *
         *    8      chkPaul                                        9                                          *
         *    9      chkRest                                       10                                          *
         *   10      chBoxNouns          |  From here onwards, radio buttons in the Vocab List setup           *
         *   11      chBoxVerbs          v                         12                                          *
         *   12      chBoxAdjectives                               13                                          *
         *   13      chBoxAdverbs                                  14                                          *
         *   14      chBoxPrepositions                             15                                          *
         *   15      chBoxOthers                                   16                                          *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfCheckBoxes = 0;
        SortedList<int, CheckBox> checkBoxCollection = new SortedList<int, CheckBox>();
        public int NoOfCheckBoxes { get => noOfCheckBoxes; set => noOfCheckBoxes = value; }

        public void addCheckBox(CheckBox newItem)
        {
            checkBoxCollection.Add(noOfCheckBoxes++, newItem);
        }

        public CheckBox getCheckBox(int index)
        {
            CheckBox requiredCheckBox;

            if (index >= noOfCheckBoxes)
            {
                MessageBox.Show("That RadioButton doesn't exist", "RadioButton error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            checkBoxCollection.TryGetValue(index, out requiredCheckBox);
            return requiredCheckBox;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Status Labels                      objectTypeCode = 11                                             *
         *  -------------                                                                                      *
         *                                                                                                     *
         *  Index     Specific Textbox                                                                         *
         *  -----     ----------------                                                                         *
         *                                                                                                     *
         *    0        statLab2   (nb statLab1 doesn't change and therefore access is not needed)              *
         *    1        statLab3                                                                                *
         *    1        statLab4                                                                                *
         *    1        statLab5                                                                                *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/
        int noOfSearchMessages = 0;
        SortedList<int, ToolStripStatusLabel> searchMessages = new SortedList<int, ToolStripStatusLabel>();

        public int NoOfSearchMessages { get => noOfSearchMessages; set => noOfSearchMessages = value; }

        public void addStatusMessage(ToolStripStatusLabel newItem)
        {
            searchMessages.Add(noOfSearchMessages++, newItem);
        }

        public ToolStripStatusLabel getStatusMessage(int index)
        {
            ToolStripStatusLabel requiredStatusMessage;

            if (index >= noOfSearchMessages)
            {
                MessageBox.Show("That Label doesn't exist", "Status Label error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            searchMessages.TryGetValue(index, out requiredStatusMessage);
            return requiredStatusMessage;
        }

        /*-----------------------------------------------------------------------------------------------------*
         *                                                                                                     *
         *  Additional variables for Main Form controls that are not grouped                                   *
         *  ----------------------------------------------------------------                                   *
         *                                                                                                     *
         *  Index     Specific Textbox                                                                         *
         *  -----     ----------------                                                                         *
         *                                                                                                     *
         *    0        statLab2   (nb statLab1 doesn't change and therefore access is not needed)              *
         *    1        statLab3                                                                                *
         *    2        statLab4                                                                                *
         *    3        statLab5                                                                                *
         *                                                                                                     *
         *-----------------------------------------------------------------------------------------------------*/

        Button btnSearchType;
        ToolStripMenuItem parentMenuItem;
        StatusStrip statStrip;
        NumericUpDown udWordDistance;
        Panel pnlKeyboard, pnlFiller;

        public Button BtnSearchType { get => btnSearchType; set => btnSearchType = value; }
        public ToolStripMenuItem ParentMenuItem { get => parentMenuItem; set => parentMenuItem = value; }
        public StatusStrip StatStrip { get => statStrip; set => statStrip = value; }
        public NumericUpDown UdWordDistance { get => udWordDistance; set => udWordDistance = value; }
        public Panel PnlKeyboard { get => pnlKeyboard; set => pnlKeyboard = value; }
        public Panel PnlFiller { get => pnlFiller; set => pnlFiller = value; }

        /*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                               *
         *                                   Items managed by Preferences                                *
         *                                   ----------------------------                                *
         *                                                                                               *
         *  Each item is handled by a SortedList.  The key will be the index of the tab in Preferences.  *
         *                                                                                               *
         *+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        /*-----------------------------------------------------------------------------------------------*
         *                                                                                               *
         *                                         Text sizes                                            *
         *                                         ----------                                            *
         *                                                                                               *
         *  Each richtext box is capable of displaying text of varying size.  We will label text sizes   *
         *    as:                                                                                        *
         *                                                                                               *
         *  Id               Simple           Main                Primary                 Secondary      *
         *                   ------           ----                -------                 ---------      *
         *  0 NT Text    Verse numbering    Greek Text         Not applicable           Not applicable   *
         *  1 LXX Text   Verse numbering    Greek Text         Not applicable           Not applicable   *
         *  2 Parse area Main text          Title              Not applicable           Not applicable   *
         *  3 Lexicon    Main text          Title              Not applicable           Not applicable   *
         *  4 Search Res Bible Reference    Greek text         Primary match word       Secondary match  *
         *  5 Vocab List Main text          Not applicable     Not applicable           Not applicable   *
         *  6 Notes      Main text          Not applicable     Not applicable           Not applicable   *
         *                                                                                               *
         *-----------------------------------------------------------------------------------------------*/
        SortedList<int, float> simpleTextSize = new SortedList<int, float>();
        SortedList<int, float> mainTextSize = new SortedList<int, float>();
        SortedList<int, float> primaryTextSize = new SortedList<int, float>();
        SortedList<int, float> secondaryTextSize = new SortedList<int, float>();

        public void addTextSize(int index, float newSize, int textType)
        {
            /*===========================================================================*
             *                                                                           *
             *                            addTextSize                                    *
             *                                                                           *
             *  Generic addition method.  The parameter, textType is:                    *
             *                                                                           *
             *     Code           Meaning                                                *
             *      1           Simple Text Size                                         *
             *      2           Main Text Size                                           *
             *      3           Search text - Primary match text size                    *
             *      4           Search text - Secondary match text size                  *
             *                                                                           *
             *===========================================================================*/

            SortedList<int, float> genericTextSize;

            switch (textType)
            {
                case 1: genericTextSize = simpleTextSize; break;
                case 2: genericTextSize = mainTextSize; break;
                case 3: genericTextSize = primaryTextSize; break;
                case 4: genericTextSize = secondaryTextSize; break;
                default: genericTextSize = null; break;
            }
            if (genericTextSize == null) return;
            if (genericTextSize.ContainsKey(index)) genericTextSize.Remove(index);
            genericTextSize.Add(index, newSize);
        }

        public float getTextSize(int index, int textType)
        {
            float currentSize;
            SortedList<int, float> genericTextSize;

            switch (textType)
            {
                case 1: genericTextSize = simpleTextSize; break;
                case 2: genericTextSize = mainTextSize; break;
                case 3: genericTextSize = primaryTextSize; break;
                case 4: genericTextSize = secondaryTextSize; break;
                default: genericTextSize = null; break;
            }
            if (genericTextSize == null) return 0;
            if (!genericTextSize.ContainsKey(index)) return 0;
            genericTextSize.TryGetValue(index, out currentSize);
            return currentSize;
        }

        /*-----------------------------------------------------------------------------------------------*
         *                                                                                               *
         *                                        Text styles                                            *
         *                                        -----------                                            *
         *                                                                                               *
         *  By style is meant bold, italic and strikethrough text.                                       *
         *                                                                                               *
         *  These will be applied to Simple, Main, Primary, and Secondary text, as defined in Text Sizes *
         *    above.                                                                                     *
         *                                                                                               *
         *-----------------------------------------------------------------------------------------------*/

        SortedList<int, String> simpleStyle = new SortedList<int, String>();
        SortedList<int, String> mainStyle = new SortedList<int, String>();
        SortedList<int, String> primaryStyle = new SortedList<int, String>();
        SortedList<int, String> secondaryStyle = new SortedList<int, String>();

        public void addDefinedStyle(int index, String newStyle, int textType)
        {
            /*===========================================================================*
             *                                                                           *
             *                           addDefinedStyle                                 *
             *                                                                           *
             *  By style is meant bold, italic and strikethrough text.                   *
             *                                                                           *
             *===========================================================================*/

            SortedList<int, String> genericStyle = null;

            switch (textType)
            {
                case 1: genericStyle = simpleStyle; break;
                case 2: genericStyle = mainStyle; break;
                case 3: genericStyle = primaryStyle; break;
                case 4: genericStyle = secondaryStyle; break;
            }
            if (genericStyle == null) return;
            if (genericStyle.ContainsKey(index)) genericStyle.Remove(index);
            genericStyle.Add(index, newStyle);
        }

        public String getDefinedStyleByIndex(int index, int textType)
        {
            String newStyle;
            SortedList<int, String> genericStyle = null;

            switch (textType)
            {
                case 1: genericStyle = simpleStyle; break;
                case 2: genericStyle = mainStyle; break;
                case 3: genericStyle = primaryStyle; break;
                case 4: genericStyle = secondaryStyle; break;
            }
            if (genericStyle == null) return "";
            if (!genericStyle.ContainsKey(index)) return "";
            genericStyle.TryGetValue(index, out newStyle);
            return newStyle;
        }

        /*-----------------------------------------------------------------------------------------------*
         *                                                                                               *
         *                                        Font Names                                             *
         *                                        ----------                                             *
         *                                                                                               *
         *  Font names are names for fonts, as recognised by MS Windows, such as "Times New Roman" or    *
         *    "Arial". (The options wil be limited because they have to be Unicode compliant.)           *
         *                                                                                               *
         *  These will be applied to Simple, Main, Primary, and Secondary text, as defined in Text Sizes *
         *    above.                                                                                     *
         *                                                                                               *
         *-----------------------------------------------------------------------------------------------*/

        SortedList<int, String> simpleFontName = new SortedList<int, String>();
        SortedList<int, String> mainFontName = new SortedList<int, String>();
        SortedList<int, String> primaryFontName = new SortedList<int, String>();
        SortedList<int, String> secondaryFontName = new SortedList<int, String>();

        public void addFontName(int index, String fontName, int textType)
        {
            SortedList<int, String> genericFontName = null;

            switch (textType)
            {
                case 1: genericFontName = simpleFontName; break;
                case 2: genericFontName = mainFontName; break;
                case 3: genericFontName = primaryFontName; break;
                case 4: genericFontName = secondaryFontName; break;
            }
            if (genericFontName == null) return;
            if (genericFontName.ContainsKey(index)) genericFontName.Remove(index);
            genericFontName.Add(index, fontName);
        }

        public String getDefinedFontNameByIndex(int index, int textType)
        {
            String newFontName;
            SortedList<int, String> genericFontName = null;

            switch (textType)
            {
                case 1: genericFontName = simpleFontName; break;
                case 2: genericFontName = mainFontName; break;
                case 3: genericFontName = primaryFontName; break;
                case 4: genericFontName = secondaryFontName; break;
            }
            if (genericFontName == null) return "";
            if (!genericFontName.ContainsKey(index)) return "";
            genericFontName.TryGetValue(index, out newFontName);
            return newFontName;
        }

        /*-----------------------------------------------------------------------------------------------*
         *                                                                                               *
         *                                     Colour collections                                        *
         *                                     ------------------                                        *
         *                                                                                               *
         *  Each richtext box is capable of displaying text (and backgrounds) of various colours.  Any   *
         *    text area may display any of the following colours:                                        *
         *                                                                                               *
         *      0  Background colour;                                                                    *
         *      1  Simle text colour (as defined above)                                                  *
         *      2  Main text colour (e.g. for Greek or Headings)                                         *
         *      3  Primary text colour                                                                   *
         *      4  Secondary colour                                                                      *
         *                                                                                               *
         *  These colours may vary for each text area so the index represents the relevant text area to  *
         *    which the colour applies.  The colourCode identifies the above colour use.                 *
         *                                                                                               *
         *-----------------------------------------------------------------------------------------------*/
        SortedList<int, Color> backColour = new SortedList<int, Color>();
        SortedList<int, Color> foreSimpleColour = new SortedList<int, Color>();
        SortedList<int, Color> foreMainColour = new SortedList<int, Color>();
        SortedList<int, Color> forePrimaryColour = new SortedList<int, Color>();
        SortedList<int, Color> foreSecondaryColour = new SortedList<int, Color>();

        public SortedList<int, Color> BackColour { get => backColour; set => backColour = value; }
        public SortedList<int, Color> ForeSimpleColour { get => foreSimpleColour; set => foreSimpleColour = value; }
        public SortedList<int, Color> ForeMainColour { get => foreMainColour; set => foreMainColour = value; }
        public SortedList<int, Color> ForePrimaryColour { get => forePrimaryColour; set => forePrimaryColour = value; }
        public SortedList<int, Color> ForeSecondaryColour { get => foreSecondaryColour; set => foreSecondaryColour = value; }

        public void addColourSetting(int index, Color newColour, int colourType)
        {
            /*===========================================================================*
             *                                                                           *
             *                          addColourSetting                                 *
             *                                                                           *
             *  Generic addition method.  The parameter, textType is:                    *
             *                                                                           *
             *     Code           Meaning                                                *
             *      0           Background colour                                        *
             *      1           Sim0le text colour                                       *
             *      2           Main text colour                                         *
             *      3           Primary text colour                                      *
             *      4           Second text colour                                       *
             *                                                                           *
             *===========================================================================*/

            SortedList<int, Color> genericColour;

            switch (colourType)
            {
                case 0: genericColour = backColour; break;
                case 1: genericColour = foreSimpleColour; break;
                case 2: genericColour = foreMainColour; break;
                case 3: genericColour = forePrimaryColour; break;
                case 4: genericColour = foreSecondaryColour; break;
                default: genericColour = null; break;
            }
            if (genericColour == null) return;
            if (genericColour.ContainsKey(index)) genericColour.Remove(index);
            genericColour.Add(index, newColour);
        }

        public Color getColourSetting(int index, int colourType)
        {
            Color colourItem;
            SortedList<int, Color> genericColour;

            switch (colourType)
            {
                case 0: genericColour = backColour; break;
                case 1: genericColour = foreSimpleColour; break;
                case 2: genericColour = foreMainColour; break;
                case 3: genericColour = forePrimaryColour; break;
                case 4: genericColour = foreSecondaryColour; break;
                default: genericColour = null; break;
            }
            if (genericColour == null) return Color.Empty;
            if (!genericColour.ContainsKey(index)) return Color.Empty;
            genericColour.TryGetValue(index, out colourItem);
            return colourItem;
        }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        *
         *                             Variables managing information about file locations                                        *
         *                             ---------------------------------------------------                                        *
         *                                                                                                                        *
         *  baseDirectory        The root directory of all subsequent directories and files                                       *
         *  altBaseDirectory     This is used as baseDirectory in development.                                                    *
         *  sourceFolder         The name of the folder containing text information                                               *
         *  ntTitlesFile         The name of the file holding information about each NT book                                      *
         *  lxxTitlesFile        The name of the file holding information about each LXX book                                     *
         *  ntTextFolder         The folder (within Source) that contains NT text data                                            *
         *  helpPath             The folder containing the Help file and all dependant files                                      *
         *  notesPath            The folder forming the root of all notes storage                                                 *
         *  notesName            The name of the notes set (and folder) currently active                                          *
         *  lexiconFolder        The folder (within Source) that contains all lexical data                                        *
         *  keyboardFolder       The folder containing data supporting the presentation of the virtual keyboard                   *
         *  gkGroupsName         A file that provides additional decode information                                               *
         *  liddellAndScott      The name of the file containing appendices for the lexicon                                       *
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        int maxNoteRef = -1, currentNoteRef = -1;
        String baseDirectory, altBaseDirectory, ntTitlesFile = "NTTitles.txt", lxxTitlesFile = "LXXTitles.txt",
            ntTextFolder = "NT", lxxTextFolder = "LXX", helpFile = "Help", notesPath = "Notes", notesName = "default", lexiconFolder = "Lexicon",
            keyboardFolder = "Keyboard", liddellAndScott = "LandSSummary.txt";
        String fullGkAccute, fullGkCircumflex, fullGkDiaereses, fullGkGrave, fullGkIota, fullGkRough, fullGkSmooth, fullGkConv1, fullGkConv2;
        //        String sourceFolder = "Source", gkGroupsName = @"Gk\GkCharsGrouped.txt";

        public int CurrentNoteRef { get => currentNoteRef; set => currentNoteRef = value; }
        public int MaxNoteRef { get => maxNoteRef; set => maxNoteRef = value; }
        public string BaseDirectory { get => baseDirectory; set => baseDirectory = value; }
        public string AltBaseDirectory { get => altBaseDirectory; set => altBaseDirectory = value; }
//        public string SourceFolder { get => sourceFolder; set => sourceFolder = value; }
        public string NtTitlesFile { get => ntTitlesFile; set => ntTitlesFile = value; }
        public string LxxTitlesFile { get => lxxTitlesFile; set => lxxTitlesFile = value; }
        public string NtTextFolder { get => ntTextFolder; set => ntTextFolder = value; }
        public string LxxTextFolder { get => lxxTextFolder; set => lxxTextFolder = value; }
        public string HelpFile { get => helpFile; set => helpFile = value; }
        public string NotesPath { get => notesPath; set => notesPath = value; }
        public string NotesName { get => notesName; set => notesName = value; }
        public string LexiconFolder { get => lexiconFolder; set => lexiconFolder = value; }
        public string KeyboardFolder { get => keyboardFolder; set => keyboardFolder = value; }
//        public string GkGroupsName { get => gkGroupsName; set => gkGroupsName = value; }
        public string LiddellAndScott { get => liddellAndScott; set => liddellAndScott = value; }
        public string FullGkAccute { get => fullGkAccute; set => fullGkAccute = value; }
        public string FullGkCircumflex { get => fullGkCircumflex; set => fullGkCircumflex = value; }
        public string FullGkDiaereses { get => fullGkDiaereses; set => fullGkDiaereses = value; }
        public string FullGkGrave { get => fullGkGrave; set => fullGkGrave = value; }
        public string FullGkIota { get => fullGkIota; set => fullGkIota = value; }
        public string FullGkRough { get => fullGkRough; set => fullGkRough = value; }
        public string FullGkSmooth { get => fullGkSmooth; set => fullGkSmooth = value; }
        public string FullGkConv1 { get => fullGkConv1; set => fullGkConv1 = value; }
        public string FullGkConv2 { get => fullGkConv2; set => fullGkConv2 = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        *
         *                          Variables that assist the management of registry storage                                      *
         *                          --------------------------------------------------------                                      *
         *                                                                                                                        *
         *  registryKeyValue     An array containing registry key names                                                           *
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        String[] registryKeyValue = { "NT Text Colour", "NT Text Background Colour", "NT Text Font Size",
                                        "LXX Text Colour", "LXX Text Background Colour", "LXX Text Font Size",
                                          "Parse Area Text Colour", "Parse Area Background Colour", "Parse Area Font Size",
                                          "Lexical Area Text Colour", "Lexical Area Background Colour", "Lexical Area Font Size",
                                          "Search Results Text Colour", "Search Results Background Colour", "Search Results Font Size",
                                          "Notes Text Colour", "Notes Background Colour", "Notes Font Size",
                                          "Vocab List Text Colour", "Vocab List Background Colour", "Vocab List Font Size"  };

        public string[] RegistryKeyValue { get => registryKeyValue; set => registryKeyValue = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        *
         *                                  Variables that support the processing of Text                                         *
         *                                  ---------------------------------------------                                         *
         *                                                                                                                        *
         *  bookList            A dictionary storing the root of the book trees                                                   *
         *      Key:   a sequential integer identifying the specific book (see classBook for more details)                        *
         *      Value: a class instance holding information for the book                                                          *
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        int noOfNTBooks, noOfLXXBooks, noOfAllBooks;
        SortedDictionary<int, classBook> bookList = new SortedDictionary<int, classBook>();

        public int NoOfNTBooks { get => noOfNTBooks; set => noOfNTBooks = value; }
        public int NoOfLXXBooks { get => noOfLXXBooks; set => noOfLXXBooks = value; }
        public int NoOfAllBooks { get => noOfAllBooks; set => noOfAllBooks = value; }
        public SortedDictionary<int, classBook> BookList { get => bookList; set => bookList = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        * 
         *                                   Variables relating to the virtual keyboards                                          * 
         *                                   -------------------------------------------                                          *
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/
        RadioButton[] rbtnDestination;

        public RadioButton[] RbtnDestination { get => rbtnDestination; set => rbtnDestination = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        * 
         *                                       Variables relating to search processing                                          * 
         *                                       ---------------------------------------                                          * 
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/
        int primaryBookId, primaryWordSeq, secondaryBookId, secondaryWordSeq;
        int noOfBookGroups, noOfSearchObjects = 0, noOfResultsItems, rowIndex;
        String primaryWord, secondaryWord, primaryChapNo, primaryVNo, secondaryChapNo, secondaryVNo;
        String[] categoryName = { "Pentateuch", "History\nFormer Prophets", "Major Prophets", "Minor Prophets", 
                                     "Kethubim - Poetry", "Kethubim - History", "Pseudepigrapha", "Narratives", 
                                     "Paul's Letters", "Other Works" };
        Button btnStop;
        CheckBox[] chkBooks;
        classBook[,] differentiatedList;
        RichTextBox[,] searchText;
        ListBox lbAvailableBooks;

        public int PrimaryBookId { get => primaryBookId; set => primaryBookId = value; }
        public int PrimaryWordSeq { get => primaryWordSeq; set => primaryWordSeq = value; }
        public int SecondaryBookId { get => secondaryBookId; set => secondaryBookId = value; }
        public int SecondaryWordSeq { get => secondaryWordSeq; set => secondaryWordSeq = value; }
        public int NoOfBookGroups { get => noOfBookGroups; set => noOfBookGroups = value; }
        public int NoOfSearchObjects { get => noOfSearchObjects; set => noOfSearchObjects = value; }
        public int NoOfResultsItems { get => noOfResultsItems; set => noOfResultsItems = value; }
        public int RowIndex { get => rowIndex; set => rowIndex = value; }
        public string PrimaryWord { get => primaryWord; set => primaryWord = value; }
        public string SecondaryWord { get => secondaryWord; set => secondaryWord = value; }
        public string PrimaryChapNo { get => primaryChapNo; set => primaryChapNo = value; }
        public string PrimaryVNo { get => primaryVNo; set => primaryVNo = value; }
        public string SecondaryChapNo { get => secondaryChapNo; set => secondaryChapNo = value; }
        public string SecondaryVNo { get => secondaryVNo; set => secondaryVNo = value; }
        public string[] CategoryName { get => categoryName; set => categoryName = value; }
        public Button BtnStop { get => btnStop; set => btnStop = value; }
        public CheckBox[] ChkBooks { get => chkBooks; set => chkBooks = value; }
        public classBook[,] DifferentiatedList { get => differentiatedList; set => differentiatedList = value; }
        public RichTextBox[,] SearchText { get => searchText; set => searchText = value; }
        public ListBox LbAvailableBooks { get => lbAvailableBooks; set => lbAvailableBooks = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        * 
         *                                    Variables supporting the functioning of History                                     * 
         *                                    -----------------------------------------------                                     * 
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        String lastNTHistoryEntry = "", lastLXXHistoryEntry = "";

        public string LastNTHistoryEntry { get => lastNTHistoryEntry; set => lastNTHistoryEntry = value; }
        public string LastLXXHistoryEntry { get => lastLXXHistoryEntry; set => lastLXXHistoryEntry = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        * 
         *                                    Variables recording effect of mouse clicks                                          * 
         *                                    ------------------------------------------                                          * 
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        int selectedNTWordSequence, selectedLXXWordSequence;
        String lastSelectedNTVerse, lastSelectedNTWord, lastSelectedLXXVerse, lastSelectedLXXWord, 
            lastSelectedSearchVerse, lastSelectedSearchRef;
        classWord currentlySelectedNTWord, currentlySelectedLXXWord;

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        * 
         *                             Variables relating to frmCopyOptions (not needing global access)                           * 
         *                             ----------------------------------------------------------------                           * 
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/


        int noOfCopyOptions = 0;
        SortedList<int, int> copyDestinationOptions = new SortedList<int, int>();
        SortedList<int, int> copyReferenceOptions = new SortedList<int, int>();
        SortedList<int, int> copyAccentsOptions = new SortedList<int, int>();
        SortedList<int, int> copyRememberOptions = new SortedList<int, int>();

        public int SelectedNTWordSequence { get => selectedNTWordSequence; set => selectedNTWordSequence = value; }
        public int SelectedLXXWordSequence { get => selectedLXXWordSequence; set => selectedLXXWordSequence = value; }
        public string LastSelectedNTVerse { get => lastSelectedNTVerse; set => lastSelectedNTVerse = value; }
        public string LastSelectedNTWord { get => lastSelectedNTWord; set => lastSelectedNTWord = value; }
        public string LastSelectedLXXVerse { get => lastSelectedLXXVerse; set => lastSelectedLXXVerse = value; }
        public string LastSelectedLXXWord { get => lastSelectedLXXWord; set => lastSelectedLXXWord = value; }
        public string LastSelectedSearchVerse { get => lastSelectedSearchVerse; set => lastSelectedSearchVerse = value; }
        public string LastSelectedSearchRef { get => lastSelectedSearchRef; set => lastSelectedSearchRef = value; }
        public classWord CurrentlySelectedNTWord { get => currentlySelectedNTWord; set => currentlySelectedNTWord = value; }
        public classWord CurrentlySelectedLXXWord { get => currentlySelectedLXXWord; set => currentlySelectedLXXWord = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        * 
         *                                Variables controlling the resizing of the right pane                                    * 
         *                                ----------------------------------------------------                                    *
         *                                                                                                                        *
         *    The value of currentResizeAction determines (or controls) the timer's action.  Values are:                          *
         *                                                                                                                        *
         *    CurrentResizeAction                    Significance                                                                 *
         *    -------------------                    ------------                                                                 *
         *                                                                                                                        *
         *           1                     Maximise the top panel (i.e. reduce the bottom panel to almost zero)                   *
         *           2                     Maximise the bottom panel (i.e. the opposite action)                                   *
         *           3                     Equalise the two areas (return the state back to default)                              *
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        int currentResizeAction;

        public int CurrentResizeAction { get => currentResizeAction; set => currentResizeAction = value; }

        /*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*
         *                                                                                                                        *
         *                                         Other Variables (unclassified)                                                 *
         *                                         ------------------------------                                                 *
         *                                                                                                                        *
         *++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++*/

        String displayCodeName = "Book Display Code";
        String primaryKeyValue = "Primary Search Result Colour", secondaryKeyValue = "Secondary Search Result Colour";
        DataGridView dgvVocabList;

        public string DisplayCodeName { get => displayCodeName; set => displayCodeName = value; }
        public string PrimaryKeyValue { get => primaryKeyValue; set => primaryKeyValue = value; }
        public string SecondaryKeyValue { get => secondaryKeyValue; set => secondaryKeyValue = value; }
        public DataGridView DgvVocabList { get => dgvVocabList; set => dgvVocabList = value; }

        /*------------------------------------------------------------------------*
         *                                                                        * 
         *                    Globally Accessible Controls                        * 
         *                    ----------------------------                        * 
         *                                                                        *
         *------------------------------------------------------------------------*/

        Form masterForm;

        Color primaryColour, secondaryColour;
        Color[] foreColorRec, backColorRec;
        Font[] fontForArea;

        public Form MasterForm { get => masterForm; set => masterForm = value; }
        public Color PrimaryColour { get => primaryColour; set => primaryColour = value; }
        public Color SecondaryColour { get => secondaryColour; set => secondaryColour = value; }
        public Color[] ForeColorRec { get => foreColorRec; set => foreColorRec = value; }
        public Color[] BackColorRec { get => backColorRec; set => backColorRec = value; }
        public Font[] FontForArea { get => fontForArea; set => fontForArea = value; }

        /*------------------------------------------------------------------------*
         *                                                                        * 
         *          Greek character variables (required by text handling          * 
         *          ----------------------------------------------------          * 
         *                                                                        *
         *------------------------------------------------------------------------*/

        int mainCharStart, mainCharEnd, furtherCharStart, furtherCharEnd;
        String digammaUpper = (Convert.ToChar(0x03dc)).ToString(), digammaLower = (Convert.ToChar(0x03dd)).ToString();
        SortedDictionary<int, String> baseGkChars = new SortedDictionary<int, string>();
        SortedDictionary<int, String> furtherGkChars = new SortedDictionary<int, string>();

        public int MainCharStart { get => mainCharStart; set => mainCharStart = value; }
        public int MainCharEnd { get => mainCharEnd; set => mainCharEnd = value; }
        public int FurtherCharStart { get => furtherCharStart; set => furtherCharStart = value; }
        public int FurtherCharEnd { get => furtherCharEnd; set => furtherCharEnd = value; }
        public string DigammaUpper { get => digammaUpper; }
        public string DigammaLower { get => digammaLower; }
        public SortedDictionary<int, string> BaseGkChars { get => baseGkChars; set => baseGkChars = value; }
        public SortedDictionary<int, string> FurtherGkChars { get => furtherGkChars; set => furtherGkChars = value; }

        public classGlobal()
        {
            const int noOfBookCategories = 10, maxNoOfCategoryOccurrences = 20;

            int idx, jdx;
            RichTextBox tempRtxtbox;

            baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LFCConsulting\GreekBibleStudent\Files";
            altBaseDirectory = Path.GetFullPath(@"..\Files");
            historyMax = 99;
            for (idx = 0; idx < noOfRichtextBoxes; idx++)
            {
                rtxtCollection.TryGetValue(idx, out tempRtxtbox);
                foreColorRec[idx] = tempRtxtbox.ForeColor;
                backColorRec[idx] = tempRtxtbox.BackColor;
                fontForArea[idx] = tempRtxtbox.Font;
            }
            differentiatedList = new classBook[noOfBookCategories, maxNoOfCategoryOccurrences];
            for( idx = 0; idx < noOfBookCategories; idx++)
            {
                for( jdx = 0; jdx < maxNoOfCategoryOccurrences; jdx++)
                {
                    differentiatedList[idx, jdx] = null;
                }
            }
            chkBooks = new CheckBox[noOfBookCategories];
        }

        public void initialiseDefaultStore()
        {
            int idx;

            foreColorRec = new Color[noOfRichtextBoxes];
            backColorRec = new Color[noOfRichtextBoxes];
            fontForArea = new Font[noOfRichtextBoxes];
            for (idx = 0; idx < noOfRichtextBoxes; idx++)
            {
                foreColorRec[idx] = rtxtCollection[idx].ForeColor;
                backColorRec[idx] = rtxtCollection[idx].BackColor;
                fontForArea[idx] = rtxtCollection[idx].Font;
            }
        }

        public Font configureFont(String fontName, String styleName, float fontSize)
        {
            FontStyle selectedStyle = FontStyle.Regular;

            switch (styleName)
            {
                case "Regular": selectedStyle = FontStyle.Regular; break;
                case "Bold": selectedStyle = FontStyle.Bold; break;
                case "Italic": selectedStyle = FontStyle.Italic; break;
                case "Underline": selectedStyle = FontStyle.Underline; break;
                case "Strikeout": selectedStyle = FontStyle.Strikeout; break;
            }
            return new Font(fontName, fontSize, selectedStyle);
        }

        public void setCopyOptions(int optionCode, int copyTypeCode, int optionValue, int versionCode)
        {
            /*===========================================================================================================*
             *                                                                                                           *
             *                                                        setCopyOptions                                     *
             *                                                        --------------                                     *
             *                                                                                                           *
             *  When a user opts to copy a word, verse, etc. from the text, he has the option to elect for his latest    *
             *    options (e.g. whether to include accents or Bible references) so that the same options will be used    *
             *    automatically next time.  This method will manage the storage of those option.  The specific options   *
             *    are as follows:                                                                                        *
             *                                                                                                           *
             *          Dictionary                                Meaning                                                *
             *          ----------                                -------                                                *
             *     copyDestinationOptions   Where the copy will be directed                                              *
             *     copyReferenceOptions     Whether to include or exclude the Bible reference                            *
             *     copyAccentsOptions       Whether to include accents with the copy (or not)                            *
             *                                                                                                           *
             *                                                 Possible values and meaning                               *
             *          Dictionary                                   1             2                                     *
             *          ----------                             ------------- ------------                                *
             *     copyDestinationOptions                      Clipboard     Relevent note                               *
             *     copyReferenceOptions                        Include       Exclude                                     *
             *     copyAccentsOptions                          Include       Don't include                               *
             *                                                                                                           *
             *  If the user elects to remember any one of these options for future use an additional dictionary is       *
             *    populated:                                                                                             *
             *                                                                                                           *
             *          Variable                                  Meaning                                                *
             *          --------                                  -------                                                *
             *     copyRememberOptions      Indicates whether a specific copy is to be reused or not                     *
             *                                                                                                           *
             *                                                 Possible values and meaning                               *
             *          Variable                                     1             2                                     *
             *          --------                               ------------- ------------                                *
             *     copyRememberOptions                          Don't reuse   Reuse                                      *
             *                                                                                                           *
             *  Each of the above dictionaries are constructed as:                                                       *
             *     Key:   A copy option or type (specifically a word, verse, entire chapter or selection of text)        *
             *     Value: The possible value (and meaning) _for that copy option_, as defined above                      *
             *                                                                                                           *
             *  Key values are as follows:                                                                               *
             *                                       MT                                 LXX                              *
             *                                                                                                           *
             *                           Word  Verse  Chapter  Selection    Word  Verse  Chapter  Selection              *
             *                           ----  -----  -------  ---------    ----  -----  -------  ---------              *
             *                                                                                                           *
             *   dictionary:              0      1       2         3         10     11      12       13                  *
             *                                                                                                           *
             *  Parameters:                                                                                              *
             *  ==========                                                                                               *
             *                                                                                                           *
             *  optionCode     whether a word, verse, chapter or verse.  (These will be set to 0 to 3 for both MT and    *
             *                   LXX and adjusted in the method itself.)                                                 *
             *  copyTypeCode   whether it is a value for destination (0), reference (1), accents (2) or for that option  *
             *                   Code to be remembered (3)                                                               *
             *  optionValue    1 or 2, asdefined in "Possible values and meaning", above                                 *
             *  versionCode    if for MT, 0; if for LXX, 1                                                               *
             *                                                                                                           *
             *===========================================================================================================*/

            int keyCode;

            keyCode = optionCode;
            if (versionCode == 1) keyCode += 10;
            switch (copyTypeCode)
            {
                case 0:
                    if (copyDestinationOptions.ContainsKey(keyCode)) copyDestinationOptions.Remove(keyCode);
                    copyDestinationOptions.Add(keyCode, optionValue);
                    break;
                case 1:
                    if (copyReferenceOptions.ContainsKey(keyCode)) copyReferenceOptions.Remove(keyCode);
                    copyReferenceOptions.Add(keyCode, optionValue);
                    break;
                case 2:
                    if (copyAccentsOptions.ContainsKey(keyCode)) copyAccentsOptions.Remove(keyCode);
                    copyAccentsOptions.Add(keyCode, optionValue);
                    break;
                case 3:
                    if (copyRememberOptions.ContainsKey(keyCode)) copyRememberOptions.Remove(keyCode);
                    copyRememberOptions.Add(keyCode, optionValue);
                    break;
            }
        }


        public Tuple<int, int, int, int> getCopyOption(int optionCode, int versionCode)
        {
            /*===========================================================================================================*
             *                                                                                                           *
             *                                                        getCopyOptions                                     *
             *                                                        --------------                                     *
             *                                                                                                           *
             *  See setCopyOptions for a detailed description of the workings of this and the previous method.           *
             *  This method allows the user to submit an optionCode (e.g. 0 for word, 1 for verse, etc.)  and a value    *
             *    for whether it refers to the Hebrew/Aramaic text (versionCode = 0) or LXX (versionCode = 1) and the    *
             *    method will return a Tuple as follows:                                                                 *
             *                                                                                                           *
             *                                                                     Possible values and meaning           *
             *    items     significance                    value                        1             2                 *
             *    -----     ------------                    -----                  ------------- ------------            *
             *      1     Destination                       1 or 2                 Clipboard     Relevent note           *
             *      2     Reference                         1 or 2                 Include       Exclude                 *
             *      3     Accents                           1 or 2                 Include       Don't include           *
             *      4     Remember                          1 or 2                 Don't reuse   Reuse                   *
             *                                                                                                           *
             *  If no entry is found, a value of -1 is returned for that item.                                           *
             *                                                                                                           *
             *===========================================================================================================*/
            int keyCode, destVal, refVal, accVal, remVal;

            keyCode = optionCode;
            if (versionCode == 1) keyCode += 10;
            if (copyDestinationOptions.ContainsKey(keyCode)) copyDestinationOptions.TryGetValue(keyCode, out destVal);
            else destVal = 0;
            if (copyReferenceOptions.ContainsKey(keyCode)) copyReferenceOptions.TryGetValue(keyCode, out refVal);
            else refVal = 0;
            if (copyAccentsOptions.ContainsKey(keyCode)) copyAccentsOptions.TryGetValue(keyCode, out accVal);
            else accVal = 0;
            if (copyRememberOptions.ContainsKey(keyCode)) copyRememberOptions.TryGetValue(keyCode, out remVal);
            else remVal = 0;
            return new Tuple<int, int, int, int>(destVal, refVal, accVal, remVal);
        }
    }
}
