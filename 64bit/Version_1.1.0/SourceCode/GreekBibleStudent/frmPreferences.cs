using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    public partial class frmPreferences : Form
    {
        const char zeroWidthSpace = '\u200b', zeroWidthNonJoiner = '\u200d';

        int noOfAreas, controlTops = 10, textFont = 12;
        bool[] hasChanged;
        ComboBox[,] cbSimpleFontSizes, cbMainFontSizes, cbPrimaryFontSizes, cbSecondaryFontSizes;
        TextBox txtPrimary, txtSecondary;
        TextBox[] txtText, txtBg;
        RichTextBox[] rtxtExample;
        RichTextBox[,] rtxtReference, rtxtVerse;
        DataGridView dgvSearchResults;
        TabPage[] tabCollection;
        TabControl[] tabCtlSubMaster;
        TabPage[,] tabSubGroups;
        PictureBox[,] colourBoxes;
        Font largerFont, smallerFont;
        Button[] btnReset;
        int[] noOfWordsInExample;
        String[] preferenceAreas = { "New Testament", "Septuagint", "Grammatical Breakdown", "Lexical Area", "Search Results", "Vocab Lists",
                                     "Notes" };
        SortedList<int, classPreferencesExamples>[] exampleText = new SortedList<int, classPreferencesExamples>[7];


        String[] mtParse = { "׳דְּבָרַי", "", "noun: masculine plural absolute" };
        String[] lxxParse = { "xδέδωκα", "", "1st person Singular Perfect Active Indicative  - root: δίδωμι" };


        String[,] subTabHeaders = { { "Verse Numbers", "Greek Text", "", ""}, { "Verse Numbers", "Greek Text", "", "" },
                                    { "Main Text", "Heading Text", "", "" }, { "Main Text", "Heading Text", "", "" },
                                    { "Text for Refrerences", "Main Text", "The Primary word sought", "The Secondary word sought" },
                                    { "Text", "", "", ""}, { "Text", "", "", ""} };


        /*-----------------------------------------------------------------------------------------------*
         *                                                                                               *
         *                                         Text sizes                                            *
         *                                         ----------                                            *
         *                                                                                               *
         *-----------------------------------------------------------------------------------------------*/
        SortedList<int, float> prefSimpleTextSize = new SortedList<int, float>();
        SortedList<int, float> prefMainTextSize = new SortedList<int, float>();
        SortedList<int, float> prefPrimaryTextSize = new SortedList<int, float>();
        SortedList<int, float> prefSecondaryTextSize = new SortedList<int, float>();

        /*-----------------------------------------------------------------------------------------------*
         *                                                                                               *
         *                                         Text style                                            *
         *                                         ----------                                            *
         *                                                                                               *
         *-----------------------------------------------------------------------------------------------*/
        SortedList<int, String> prefSimpleStyle = new SortedList<int, String>();
        SortedList<int, String> prefMainStyle = new SortedList<int, String>();
        SortedList<int, String> prefPrimaryStyle = new SortedList<int, String>();
        SortedList<int, String> prefSecondaryStyle = new SortedList<int, String>();

        /*-----------------------------------------------------------------------------------------------*
         *                                                                                               *
         *                                         Font name                                             *
         *                                         ---------                                             *
         *                                                                                               *
         *-----------------------------------------------------------------------------------------------*/
        SortedList<int, String> prefSimpleFontName = new SortedList<int, String>();
        SortedList<int, String> prefMainFontName = new SortedList<int, String>();
        SortedList<int, String> prefPrimaryFontName = new SortedList<int, String>();
        SortedList<int, String> prefSecondaryFontName = new SortedList<int, String>();

        /*-----------------------------------------------------------------------------------------------*
         *                                                                                               *
         *                                     Colour collections                                        *
         *                                     ------------------                                        *
         *                                                                                               *
         *-----------------------------------------------------------------------------------------------*/
        SortedList<int, Color> prefBackColour = new SortedList<int, Color>();
        SortedList<int, Color> prefForeSimpleColour = new SortedList<int, Color>();
        SortedList<int, Color> prefForeMainColour = new SortedList<int, Color>();
        SortedList<int, Color> prefForePrimaryColour = new SortedList<int, Color>();
        SortedList<int, Color> prefForeSecondaryColour = new SortedList<int, Color>();

        classGlobal globalVars;
        classRegistry appRegistry;

        public int NoOfAreas { get => noOfAreas; set => noOfAreas = value; }
        public bool[] HasChanged { get => hasChanged; set => hasChanged = value; }

        public frmPreferences(classGlobal inConfig, classRegistry inRegistry)
        {
            InitializeComponent();

            /*-------------------------------------------------------------------------*
             *  Global Classes used                                                    *
             *-------------------------------------------------------------------------*/

            globalVars = inConfig;
            appRegistry = inRegistry;
        }

        public void initialiseForm()
        {
            /*=====================================================================================================================*
             *                                                                                                                     *
             *                                     frmPreferences - Initialisation                                                 *
             *                                     ===============================                                                 *
             *                                                                                                                     *
             *  The Preferences dialog is created programmatically and is quite fiddly.  The basic architecture is:                *
             *                                                                                                                     *
             *                                             frmPreferences                                                          *
             *                                                   |                                                                 *
             *                                                   v                                                                 *
             *                                            tabCtlPreferences (created in the Designer)                              *
             *                                                   |                                                                 *
             *                                                   v                                                                 *
             *                                              tabCollection (one per area, defined by preferenceAreas)               *
             *                                                   |                                                                 *
             *                                                   v                                                                 *
             *                                            tabCtlSubMaster (tab controls added to each tabCollection)               *
             *                                                   |                                                                 *
             *                                                   v                                                                 *
             *                                              tabSubGroups (pages added to each tabCtlSubMaster)                     *
             *                                                                                                                     *
             *                                                                                                                     *
             *                                                                                                                     *
             *                                                                                                                     *
             *=====================================================================================================================*/

            int idx;
            String[] fontSizes;
            String[] fontStyles;
            Color tempColour = Color.Empty;
            Label fontInfo, textInfo, bgroundInfo;

            noOfAreas = preferenceAreas.Length;

            /*-------------------------------------------------------------------------*
             *  Get current values from classGlobal                                    *
             *-------------------------------------------------------------------------*/

            preparePopulateSourceData();
            for (idx = 0; idx < noOfAreas; idx++)
            {
                populateSourceData(idx);
            }

            /*-------------------------------------------------------------------------*
             *  Set up the actual Dialog                                               *
             *-------------------------------------------------------------------------*/

            fontSizes = new string[] { "6", "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "32", "36", "48", "60", "72" };
            fontStyles = new string[] { "Regular", "Bold", "Italic", "Bold and Italic" };
            largerFont = new Font(FontFamily.GenericSansSerif, 12F, FontStyle.Regular);
            smallerFont = new Font(FontFamily.GenericSansSerif, 10F, FontStyle.Regular);
            tabCollection = new TabPage[noOfAreas];
            tabCtlSubMaster = new TabControl[noOfAreas];
            tabSubGroups = new TabPage[noOfAreas, 4];
            rtxtExample = new RichTextBox[noOfAreas];

            rtxtReference = new RichTextBox[2, 25];
            rtxtVerse = new RichTextBox[2, 25];
            dgvSearchResults = new DataGridView();
            fontInfo = new Label();
            textInfo = new Label();
            bgroundInfo = new Label();
            cbSimpleFontSizes = new ComboBox[noOfAreas, 4];
            cbMainFontSizes = new ComboBox[noOfAreas, 4];
            cbPrimaryFontSizes = new ComboBox[noOfAreas, 4];
            cbSecondaryFontSizes = new ComboBox[noOfAreas, 4];
            colourBoxes = new PictureBox[noOfAreas, 5];
            txtText = new TextBox[noOfAreas];
            txtBg = new TextBox[noOfAreas];
            hasChanged = new bool[noOfAreas];
            btnReset = new Button[noOfAreas];
            noOfWordsInExample = new int[noOfAreas];

            exampleText[0] = populateNTText();
            exampleText[1] = populateLXXText();
            exampleText[2] = populateParseText();
            exampleText[3] = populateLexicon();
            exampleText[4] = populateSearchResult();
//            exampleText[5] = populateVocabList();
            exampleText[6] = populateNoteText();
            buildDialog();
            for (idx = 0; idx < noOfAreas; idx++)
            {
                displayExampleText(idx);
            }
        }

        private void buildDialog()
        {
            int idx, jdx, textLength, newLeft;
            Color tempColour = Color.Empty;
            Label textInfo, exampleLbl, resetLbl;
            PictureBox pboxTemp;
            Button btnFont, btnTemp;

            for (idx = 0; idx < noOfAreas; idx++)
            {
                /*----------------------------------------------------------------------------------------*
                 *  An area-based flag indicating whether one or more elements for that area has/have     *
                 *    changed.                                                                            *
                 *----------------------------------------------------------------------------------------*/
                hasChanged[idx] = false;

                /*----------------------------------------------------------------------------------------*
                 *  Create the main tabs (tabCollection - one for each area)                              *
                 *----------------------------------------------------------------------------------------*/
                tabCollection[idx] = createNewTabPage(preferenceAreas[idx]);
                tabCollection[idx].Height = tabCtlPreferences.ClientRectangle.Height;
                tabCollection[idx].Width = tabCtlPreferences.ClientSize.Width - 4;
                tabCtlPreferences.Controls.Add(tabCollection[idx]);

                /*----------------------------------------------------------------------------------------*
                 *  Create the TabControl for the area tab that will house the sub-tabs                   *
                 *----------------------------------------------------------------------------------------*/
                tabCtlSubMaster[idx] = new TabControl();
                tabCtlSubMaster[idx].Height = TextRenderer.MeasureText("Masoretic", tabCollection[idx].Font).Height + controlTops + 50;
                tabCtlSubMaster[idx].Width = tabCollection[idx].ClientSize.Width - 4;
                tabCollection[idx].Controls.Add(tabCtlSubMaster[idx]);

                /*----------------------------------------------------------------------------------------*
                 *  Create the sub-tabs.                                                                  *
                 *    Each area will have up to four sub-tabs.  The exact number is determined by the     *
                 *    array of sub-tab titles, subTabHeaders.                                             *
                 *----------------------------------------------------------------------------------------*/
                for (jdx = 0; jdx < 4; jdx++)
                {
                    if (subTabHeaders[idx, jdx].Length == 0) continue;
                    tabSubGroups[idx, jdx] = createNewTabPage(subTabHeaders[idx, jdx]);
                    tabSubGroups[idx, jdx].Width = tabCollection[idx].ClientSize.Width - 8;
                    tabSubGroups[idx, jdx].Height = tabCtlSubMaster[idx].ClientSize.Height;
                    tabCtlSubMaster[idx].Controls.Add(tabSubGroups[idx, jdx]);

                    /*----------------------------------------------------------------------------------------*
                     *  We now add the various Controls, starting with the colour label.                      *
                     *----------------------------------------------------------------------------------------*/
                    if (tabSubGroups[idx, jdx] == null) continue;
                    textInfo = new Label();
                    textInfo.Text = "Text Colour:";
                    textInfo.Left = 16;
                    textInfo.Top = controlTops + 3;
                    textInfo.Font = largerFont;
                    textInfo.AutoSize = true;
                    tabSubGroups[idx, jdx].Controls.Add(textInfo);
                    textLength = TextRenderer.MeasureText("Text Colour:", largerFont).Width;
                    newLeft = textLength + 21;  // textInfo.Left plus a small spacing value
                    /*----------------------------------------------------------------------------------------*
                     *  Now create the picture box for that text's colour.                                    *
                     *----------------------------------------------------------------------------------------*/
                    pboxTemp = new PictureBox();
                    pboxTemp.Top = controlTops;
                    pboxTemp.Left = newLeft;
                    pboxTemp.Size = new Size(75, 28);
                    switch (jdx)
                    {
                        case 0:
                            prefForeSimpleColour.TryGetValue(idx, out tempColour);
                            pboxTemp.BackColor = tempColour;
                            break;
                        case 1:
                            prefForeMainColour.TryGetValue(idx, out tempColour);
                            pboxTemp.BackColor = tempColour;
                            break;
                        case 2:
                            prefForePrimaryColour.TryGetValue(idx, out tempColour);
                            pboxTemp.BackColor = tempColour;
                            break;
                        case 3:
                            prefForeSecondaryColour.TryGetValue(idx, out tempColour);
                            pboxTemp.BackColor = tempColour;
                            break;

                    }
                    pboxTemp.Tag = idx * 10 + jdx + 1;  // A distinctive tag value that allows us to decode it when clicked
                    pboxTemp.MouseClick += pictureBoxMouseClick;
                    tabSubGroups[idx, jdx].Controls.Add(pboxTemp);
                    colourBoxes[idx, jdx + 1] = pboxTemp;

                    /*----------------------------------------------------------------------------------------*
                     *  Now add the label for the background colour, if we are on the zeroth sub-tab.         *
                     *----------------------------------------------------------------------------------------*/
                    newLeft += 90;
                    if (jdx == 0)
                    {
                        textInfo = new Label();
                        textInfo.Text = "Background Colour:";
                        textInfo.Left = newLeft;
                        textInfo.Top = controlTops + 3;
                        textInfo.Font = largerFont;
                        textInfo.AutoSize = true;
                        tabSubGroups[idx, jdx].Controls.Add(textInfo);
                        newLeft += TextRenderer.MeasureText("Background Colour:", largerFont).Width + 5;

                        /*----------------------------------------------------------------------------------------*
                         *  Add the associated picture box for the background colour.                             *
                         *----------------------------------------------------------------------------------------*/
                        pboxTemp = new PictureBox();
                        pboxTemp.Top = controlTops;
                        pboxTemp.Left = newLeft;
                        pboxTemp.Size = new Size(75, 28);
                        prefBackColour.TryGetValue(idx, out tempColour);
                        pboxTemp.BackColor = tempColour;
                        pboxTemp.Tag = idx * 10;
                        pboxTemp.MouseClick += pictureBoxMouseClick;
                        tabSubGroups[idx, jdx].Controls.Add(pboxTemp);
                        colourBoxes[idx, 0] = pboxTemp;
                        newLeft += 90;
                    }

                    /*----------------------------------------------------------------------------------------*
                     *  Now add the label and button for modifying the relevant font.                         *
                     *----------------------------------------------------------------------------------------*/

                    textInfo = new Label();
                    textInfo.Text = "Font details:";
                    textInfo.Left = newLeft;
                    textInfo.Top = controlTops + 3;
                    textInfo.Font = smallerFont;
                    textInfo.AutoSize = true;
                    tabSubGroups[idx, jdx].Controls.Add(textInfo);
                    newLeft += TextRenderer.MeasureText("Font details:", smallerFont).Width + 5;

                    btnFont = new Button();
                    btnFont.Left = newLeft;
                    btnFont.Top = controlTops;
                    btnFont.Font = smallerFont;
                    btnFont.Height = 28;
                    btnFont.Text = "Modify";
                    btnFont.Tag = idx * 10 + jdx;
                    btnFont.Click += BtnFont_Click;
                    tabSubGroups[idx, jdx].Controls.Add(btnFont);
                }

                /*----------------------------------------------------------------------------------------*
                 *  We are now targeting the area _below_ the sub-tabs, common to the main tab.           *
                 *  First, the label for the RichTextBox or set of RichTextBoxes, below                   *
                 *----------------------------------------------------------------------------------------*/
                exampleLbl = new Label();
                exampleLbl.Top = controlTops + tabCtlSubMaster[idx].Height + 8;
                exampleLbl.Left = 10;
                exampleLbl.Text = "Example text:";
                exampleLbl.Font = largerFont;
                exampleLbl.AutoSize = true;
                tabCollection[idx].Controls.Add(exampleLbl);

                /*----------------------------------------------------------------------------------------*
                 *  Further over, the label explaining the reset button.                                  *
                 *----------------------------------------------------------------------------------------*/
                textLength = TextRenderer.MeasureText("Example text:", largerFont).Width;
                newLeft = textLength + 225;  // Space the new text well away from the richtextbox label
                resetLbl = new Label();
                resetLbl.Top = controlTops + tabCtlSubMaster[idx].Height + 8;
                resetLbl.Left = newLeft;
                resetLbl.Text = "Restore all colours and font metrics:";
                resetLbl.Font = largerFont;
                resetLbl.AutoSize = true;
                tabCollection[idx].Controls.Add(resetLbl);

                /*----------------------------------------------------------------------------------------*
                 *  And the reset button itself.                                                          *
                 *----------------------------------------------------------------------------------------*/
                newLeft += TextRenderer.MeasureText("Restore all colours and font metrics:", largerFont).Width + 5;
                btnTemp = new Button();
                btnTemp.Left = newLeft;
                btnTemp.Top = controlTops + tabCtlSubMaster[idx].Height + 3;
                btnTemp.Font = largerFont;
                btnTemp.Height = 28;
                btnTemp.Text = "...";
                btnTemp.Enabled = false;
                btnTemp.Tag = idx;
                btnTemp.Click += resetButton;
                tabCollection[idx].Controls.Add(btnTemp);
                btnReset[idx] = btnTemp;

                if (idx == 5)
                {
                    /*----------------------------------------------------------------------------------------*
                     *  For area 5, we add a DataGridView rather than a RichTextBox                           *
                     *----------------------------------------------------------------------------------------*/
                    dgvSearchResults.Top = exampleLbl.Top + TextRenderer.MeasureText("Example text:", largerFont).Height + 16;
                    dgvSearchResults.Left = 16;
                    dgvSearchResults.Width = tabCtlPreferences.ClientRectangle.Width - 32;
                    dgvSearchResults.Height = tabCollection[idx].ClientRectangle.Height - 
                        (controlTops + tabCtlSubMaster[idx].Height + 16 + exampleLbl.Height + pnlBase.Height);
                    dgvSearchResults.ColumnCount = 3;
                    dgvSearchResults.Columns[0].Width = 300;
                    dgvSearchResults.Columns[1].Width = 300;
                    dgvSearchResults.Columns[2].Width = 300;
                    dgvSearchResults.RowHeadersVisible = false;
                    tabCollection[idx].Controls.Add(dgvSearchResults);
                }
                else
                {
                    /*----------------------------------------------------------------------------------------*
                     *  For all other areas, a simple RichTextBox.                                            *
                     *----------------------------------------------------------------------------------------*/
                    rtxtExample[idx] = new RichTextBox();
                    rtxtExample[idx].Top = exampleLbl.Top + TextRenderer.MeasureText("Example text:", largerFont).Height + 16;
                    rtxtExample[idx].Left = 16;
                    rtxtExample[idx].Width = tabCtlPreferences.ClientRectangle.Width - 32;
                    rtxtExample[idx].Height = tabCtlPreferences.ClientRectangle.Height - 
                        (controlTops + tabCtlSubMaster[idx].Height + 16 + exampleLbl.Height + pnlBase.Height);
                    tabCollection[idx].Controls.Add(rtxtExample[idx]);
                }
            }
        }

        private void preparePopulateSourceData()
        {
            prefSimpleTextSize.Clear();
            prefMainTextSize.Clear();
            prefPrimaryTextSize.Clear();
            prefSecondaryTextSize.Clear();

            prefSimpleStyle.Clear();
            prefMainStyle.Clear();
            prefPrimaryStyle.Clear();
            prefSecondaryStyle.Clear();

            prefSimpleFontName.Clear();
            prefMainFontName.Clear();
            prefPrimaryFontName.Clear();
            prefSecondaryFontName.Clear();

            prefBackColour.Clear();
            prefForeSimpleColour.Clear();
            prefForeMainColour.Clear();
            prefForePrimaryColour.Clear();
            prefForeSecondaryColour.Clear();
        }

        private void populateSourceData( int areaCode)
        {
            /*--------------------------------------------------------------------------------------------------*
             *                                                                                                  *
             *  Get current values from classGlobal                                                             *
             *                                                                                                  *
             *  In all that follows, the indexes representing different application areas function as follows:  *
             *                                                                                                  *
             *                  areaCode              Refers to                                                 *
             *                  --------              ---------                                                 *
             *                     0            Main NT Text area                                               *
             *                     1            Main LXX Text area                                              *
             *                     2            Parse area                                                      *
             *                     3            Lexical Area                                                    *
             *                     4            Search Results Area                                             *
             *                     5            Vocab Lists                                                     *
             *                     6            Notes                                                           *
             *                                                                                                  *
             *--------------------------------------------------------------------------------------------------*/

            prefSimpleTextSize.Add(areaCode, globalVars.getTextSize(areaCode, 1));
            if( areaCode < 5) prefMainTextSize.Add(areaCode, globalVars.getTextSize(areaCode, 2));
            if (areaCode == 4) prefPrimaryTextSize.Add(4, globalVars.getTextSize(4, 3));
            if (areaCode == 4) prefSecondaryTextSize.Add(4, globalVars.getTextSize(4, 4));

            prefSimpleStyle.Add(areaCode, globalVars.getDefinedStyleByIndex(areaCode, 1));
            if (areaCode < 5) prefMainStyle.Add(areaCode, globalVars.getDefinedStyleByIndex(areaCode, 2));
            if (areaCode == 4) prefPrimaryStyle.Add(4, globalVars.getDefinedStyleByIndex(4, 3));
            if (areaCode == 4) prefSecondaryStyle.Add(4, globalVars.getDefinedStyleByIndex(4, 4));

            prefSimpleFontName.Add(areaCode, globalVars.getDefinedFontNameByIndex(areaCode, 1));
            if (areaCode < 5) prefMainFontName.Add(areaCode, globalVars.getDefinedFontNameByIndex(areaCode, 2));
            if (areaCode == 4) prefPrimaryFontName.Add(4, globalVars.getDefinedFontNameByIndex(4, 3));
            if (areaCode == 4) prefSecondaryFontName.Add(4, globalVars.getDefinedFontNameByIndex(4, 4));

            prefBackColour.Add(areaCode, globalVars.getColourSetting(areaCode, 0));
            prefForeSimpleColour.Add(areaCode, globalVars.getColourSetting(areaCode, 1));
            if (areaCode < 5) prefForeMainColour.Add(areaCode, globalVars.getColourSetting(areaCode, 2));
            if (areaCode == 4) prefForePrimaryColour.Add(4, globalVars.getColourSetting(4, 3));
            if (areaCode == 4) prefForeSecondaryColour.Add(4, globalVars.getColourSetting(4, 4));
        }

        private SortedList<int, classPreferencesExamples> populateNTText()
        {
            /*=============================================================================================*
             *                                                                                             *
             *                                   populateNTText                                            *
             *                                   ==============                                            *
             *                                                                                             *
             *   We arbitrarily choose 3 John as a sample text because it is short                         *
             *                                                                                             *
             *=============================================================================================*/

            int bookId = 83;              // Id for 3 John
            int wdx, vdx, noOfWords, noOfVerses, itemCount = 0;
            String verseRef;
            classBook currentBook;
            classChapter currentChapter;
            classVerse currentVerse;
            classWord currentWord;
            SortedList<int, classPreferencesExamples> exampleText = new SortedList<int, classPreferencesExamples>();

            globalVars.BookList.TryGetValue(bookId, out currentBook);
            currentChapter = currentBook.getChapterBySequence(0);
            noOfVerses = currentChapter.NoOfVersesInChapter;
            for (vdx = 0; vdx < noOfVerses; vdx++)
            {
                if (vdx > 0) addToClass("\n", 2, itemCount++, exampleText); // New line
                verseRef = currentChapter.getVerseRefBySeqNo(vdx);
                addToClass(verseRef + ": ", 1, itemCount++, exampleText);
                currentVerse = currentChapter.getVerseBySeqNo(vdx);
                noOfWords = currentVerse.WordCount;
                for (wdx = 0; wdx < noOfWords; wdx++)
                {
                    currentWord = currentVerse.getWordBySeqNo(wdx);
                    addToClass(currentWord.TextWord, 2, itemCount++, exampleText);
                }
            }
            noOfWordsInExample[0] = itemCount;
            return exampleText;
        }

        private SortedList<int, classPreferencesExamples> populateLXXText()
        {
            /*=============================================================================================*
             *                                                                                             *
             *                                   populateNTText                                            *
             *                                   ==============                                            *
             *                                                                                             *
             *   We arbitrarily choose Psalm 1 as a sample text because it is short                        *
             *                                                                                             *
             *=============================================================================================*/

            int bookId = 26;              // Id for Psalms
            int wdx, vdx, noOfWords, noOfVerses, itemCount = 0;
            String verseRef;
            classBook currentBook;
            classChapter currentChapter;
            classVerse currentVerse;
            classWord currentWord;
            SortedList<int, classPreferencesExamples> exampleText = new SortedList<int, classPreferencesExamples>();

            globalVars.BookList.TryGetValue(bookId, out currentBook);
            currentChapter = currentBook.getChapterBySequence(0);
            noOfVerses = currentChapter.NoOfVersesInChapter;
            for (vdx = 0; vdx < noOfVerses; vdx++)
            {
                if (vdx > 0) addToClass("\n", 2, itemCount++, exampleText); // New line
                verseRef = currentChapter.getVerseRefBySeqNo(vdx);
                addToClass(verseRef + ": ", 1, itemCount++, exampleText);
                currentVerse = currentChapter.getVerseBySeqNo(vdx);
                noOfWords = currentVerse.WordCount;
                for (wdx = 0; wdx < noOfWords; wdx++)
                {
                    currentWord = currentVerse.getWordBySeqNo(wdx);
                    addToClass(currentWord.TextWord, 2, itemCount++, exampleText);
                }
            }
            noOfWordsInExample[1] = itemCount;
            return exampleText;
        }

        private SortedList<int, classPreferencesExamples> populateParseText()
        {
            int noOfLines, lineIndex, itemCount;
            String[] mtParse = { "ἐγήγερται", "3rd person Singular Perfect Passive Indicative\n\nRoot of word: ἐγείρω" };
            SortedList<int, classPreferencesExamples> exampleText = new SortedList<int, classPreferencesExamples>();

            itemCount = 0;
            noOfLines = mtParse.Length;
            for (lineIndex = 0; lineIndex < noOfLines; lineIndex++)
            {
                if (lineIndex == 0) addToClass(mtParse[0] + "\n\n", 2, itemCount++, exampleText);
                else addToClass(mtParse[lineIndex], 1, itemCount++, exampleText);
            }
            noOfWordsInExample[2] = itemCount;
            return exampleText;
        }

        private SortedList<int, classPreferencesExamples> populateLexicon()
        {
            int noOfLines, lineIndex, itemCount;
            String[] lxxLex = { "δίδωμι\n\n",
                "Etymology: Redupl. from Root  δΟ , Lat.  do, dare.\n\n",
                "        I Orig. sense, to give,  τί τινι Hom., etc.; in pres.and imperf.to be ready to give, to offer, id= Hom.\n",
                "			2 of the gods, to grant, κῠδος, νίκην, and of evils, δ.ἄλγεα, ἄτας, κήδεα id = Hom.; later,  εὖ διδόναι τινί to provide well for . . , Soph., Eur.",
                "			3 to offer to the gods, Hom., etc.\n",
                "			4 with an inf.added,  δῶκε τεύχεα θεράποντι φορῆναι gave him the arms to carry, Il.; διδοῐ πιεῐν gives to drink, Hdt., etc.\n",
                "			5 Prose phrases, δ. ὅρκον, opp.to λαμβάνειν, to tender an oath;  δ.χάριν,  χαρίζεσθαι, as  ὀργῆι χάριν δούς having indulged his anger, Soph.;" +
                "   λόγον τινὶ δ.to give one leave to speak, Xen.; but,  δ.λόγον ἑαυτῶι to deliberate, Hdt.\n\n",
                "        II c.acc.pers.to give over, deliver up, Hom., etc.\n",
                "			2 of parents, to give their daughter to wife, id= Hom.\n",
                "			3 in attic, διδόναι τινά τινι to grant any one to entreaties, pardon him, Xen.:   διδόναι τινί τι to forgive one a thing, remit its punishment, Eur., Dem.\n",
                "			4  διδόναι ἑαυτόν τινι to give oneself up, Hdt., etc.\n",
                "			5  δδ.ίκην, v.δίκη IV. 3.\n\n",
                "        III in vows and prayers, c.acc.pers.et inf. to grant, allow, bring about that, Hom., Trag.\n",
                "        IV seemingly intr.to give oneself up, devote oneself, τινί Eur." };
            SortedList<int, classPreferencesExamples> exampleText = new SortedList<int, classPreferencesExamples>();

            itemCount = 0;
            noOfLines = lxxLex.Length;
            for (lineIndex = 0; lineIndex < noOfLines; lineIndex++)
            {
                if (lineIndex == 0) addToClass(lxxLex[0], 2, itemCount++, exampleText);
                else addToClass(lxxLex[lineIndex], 1, itemCount++, exampleText);
            }
            noOfWordsInExample[3] = itemCount;
            return exampleText;
        }

        private SortedList<int, classPreferencesExamples> populateSearchResult()
        {
            int noOfItems, noOfLines, itemIndex, lineIndex, itemCount, nPstn;
            Char[] textSplitter = { ' ' };
            String[] individualItem, splitWord;
            String[] searchExample = { "Numbers 20.16:",
                "καὶ ἀνεβοήσαμεν πρὸς κύριον, καὶ εἰσήκουσεν κύριος τῆς yφωνῆς ἡμῶν καὶ ἀποστείλας xἄγγελον ἐξήγαγεν ἡμᾶς ἐξ Αἰγύπτου, " +
                               "καὶ νῦν ἐσμεν ἐν Καδης, πόλει ἐκ μέρους τῶν ὁρίων σου·", "",
                               "Judges(Codex Alexandrinus) 6.10:",
                "καὶ εἶπα ὑμῖν Ἐγὼ κύριος ὁ θεὸς ὑμῶν, οὐ φοβηθήσεσθε τοὺς θεοὺς τοῦ Αμορραίου, " +
                               "ἐν οἷς ὑμεῖς ἐνοικεῖτε ἐν τῇ γῇ αὐτῶν· καὶ οὐκ εἰσηκούσατε τῆς yφωνῆς μου.", "Judges(Codex Alexandrinus) 6.11:",
                "Καὶ ἦλθεν xἄγγελος κυρίου καὶ " +
                               "ἐκάθισεν ὑπὸ τὴν δρῦν τὴν οὖσαν ἐν Εφραθα τὴν τοῦ Ιωας πατρὸς Αβιεζρι, καὶ Γεδεων ὁ υἱὸς αὐτοῦ ἐρράβδιζεν πυροὺς ἐν ληνῷ τοῦ ἐκφυγεῖν ἐκ προσώπου Μαδιαμ.",
                               "", "Judges(Codex Alexandrinus) 13.9:",
                "καὶ ἐπήκουσεν ὁ θεὸς τῆς yφωνῆς Μανωε, καὶ παρεγένετο ὁ xἄγγελος τοῦ θεοῦ ἔτι πρὸς τὴν γυναῖκα αὐτῆς " +
                               "καθημένης ἐν τῷ ἀγρῷ, καὶ Μανωε ὁ ἀνὴρ αὐτῆς οὐκ ἦν μετ αὐτῆς.", "",
                               "Judges(Codex Vaticanus) 6.10:",
                "καὶ εἶπα ὑμῖν Ἐγὼ κύριος ὁ θεὸς ὑμῶν, οὐ φοβηθήσεσθε τοὺς θεοὺς τοῦ Αμορραίου, " +
                               "ἐν οἷς ὑμεῖς καθήσεσθε ἐν τῇ γῇ αὐτῶν· καὶ οὐκ εἰσηκούσατε τῆς yφωνῆς μου.", "Judges(Codex Vaticanus) 6.11:",
                "Καὶ ἦλθεν xἄγγελος κυρίου καὶ ἐκάθισεν " +
                               "ὑπὸ τὴν τερέμινθον τὴν ἐν Εφραθα τὴν Ιωας πατρὸς τοῦ Εσδρι, καὶ Γεδεων υἱὸς αὐτοῦ ῥαβδίζων σῖτον ἐν ληνῷ εἰς ἐκφυγεῖν ἀπὸ προσώπου τοῦ Μαδιαμ." };
            SortedList<int, classPreferencesExamples> exampleText = new SortedList<int, classPreferencesExamples>();

            itemCount = 0;
            noOfLines = searchExample.Length;
            for (lineIndex = 0; lineIndex < noOfLines; lineIndex++)
            {
                if (searchExample[lineIndex].Length == 0)
                {
                    addToClass("", 6, itemCount++, exampleText);
                    continue;
                }
                if (searchExample[lineIndex].Contains(':'))
                {
                    nPstn = searchExample[lineIndex].IndexOf(':');
                    addToClass(searchExample[lineIndex].Substring(0, nPstn), 1, itemCount++, exampleText); // The word is a reference - use simple font
                }
                else
                {
                    individualItem = searchExample[lineIndex].Split(textSplitter);
                    noOfItems = individualItem.Length;
                    for (itemIndex = 0; itemIndex < noOfItems; itemIndex++)
                    {
                        if (individualItem[itemIndex].Contains('x')) // The word is a Primary match
                        {
                            splitWord = individualItem[itemIndex].Split('x');
                            if (splitWord[0].Length > 0)
                            {
                                addToClass(splitWord[0], 2, itemCount++, exampleText);
                                addToClass(splitWord[1] + " ", 3, itemCount++, exampleText);
                            }
                            else addToClass(splitWord[1] + " ", 3, itemCount++, exampleText);
                        }
                        else
                        {
                            if (individualItem[itemIndex].Contains('y')) // The word is a secondary match
                            {
                                splitWord = individualItem[itemIndex].Split('y');
                                if (splitWord[0].Length > 0)
                                {
                                    addToClass(splitWord[0], 2, itemCount++, exampleText);
                                    addToClass(splitWord[1] + " ", 4, itemCount++, exampleText);
                                }
                                else addToClass(splitWord[1] + " ", 4, itemCount++, exampleText);
                            }
                            else addToClass(individualItem[itemIndex] + " ", 2, itemCount++, exampleText);  // Normal Greek word
                        }
                    }
                    addToClass("", 5, itemCount++, exampleText);
                }
            }
            noOfWordsInExample[4] = itemCount;
            return exampleText;
        }

        private SortedList<int, classPreferencesExamples> populateNoteText()
        {
            String exampleNote = "My notes tell me that this:\n\n" +
                                        "Τοῦ δὲ Ἰησοῦ χριστοῦ ἡ γένεσις οὕτως ἦν. μνηστευθείσης τῆς μητρὸς αὐτοῦ Μαρίας τῷ Ἰωσήφ, πρὶν ἢ συνελθεῖν αὐτοὺς " +
                                        "εὑρέθη ἐν γαστρὶ ἔχουσα ἐκ πνεύματος ἁγίου.\n\nis a verse which ...";
            SortedList<int, classPreferencesExamples> exampleText = new SortedList<int, classPreferencesExamples>();

            addToClass(exampleNote, 1, 0, exampleText);
            noOfWordsInExample[6] = 1;
            return exampleText;
        }

        private RichTextBox initialSetupOfRText(Color backgroundColour, int colWidth, RightToLeft rightToLeft)
        {
            RichTextBox rtxtCurrent;

            rtxtCurrent = new RichTextBox();
            rtxtCurrent.BackColor = backgroundColour;
            rtxtCurrent.Width = colWidth;
            rtxtCurrent.Height = 120;
            rtxtCurrent.RightToLeft = rightToLeft;
            rtxtCurrent.Visible = false;
            return rtxtCurrent;
        }

        private int getNewRTXHeight(RichTextBox rtxCurrent, Font fontUsed)
        {
            int noOfLines, noOfRTXLines;
            float fontHeight;

            noOfRTXLines = 0;
            if (rtxCurrent != null)
            {
                noOfLines = rtxCurrent.GetLineFromCharIndex(rtxCurrent.Text.Length) + 1;
                fontHeight = fontUsed.Height;
                noOfRTXLines = noOfLines * ((int)fontHeight) + rtxCurrent.Margin.Vertical;
            }
            return noOfRTXLines;
        }

        private void resetRTX(RichTextBox rtxtCurrent, int newHeight)
        {
            if (rtxtCurrent == null) return;
            rtxtCurrent.Height = newHeight;
            rtxtCurrent.Visible = true;
        }

        private void displayExampleText(int areaCode)
        {
            int typeCode, noOfLines, lineIndex;
            float fontSize = 12F;
            String fontName = "", fontStyle;
            String[,] vocabSample = { { "Nouns:", "ἀνήρ", "ἄνδρα" }, { "", "νεανίσκος", "νεανίσκοι" }, { "", "πούς", "πόδας" },
                                      { "Verbs:", "εἰσέρχομαι", "εἰσελθόντες" }, { "", "ἐκφέρω", "ἐξενέγκαντες" }, { "", "ἐκψύχω", "ἐξέψυξεν" },
                                      { "", "εὑρίσκω", "εὗρον" }, { "", "θάπτω", "ἔθαψαν" }, { "", "πίπτω", "ἔπεσεν" },
                                      { "Adjectives:", "νεκρός", "νεκράν" }, { "Prepositions:", "πρός", "πρὸς" },
                                      { "Other word types:", "αὐτός", "αὐτοῦ" }, { "", "δέ", "δὲ" }, { "", "ἑαυτοῦ", "αὐτὴν" }, { "", "καί", "καὶ" },
                                      { "", "ὁ", "τοὺς" }, { "", "παραχρῆμα", "παραχρῆμα" } };
            Color tempColour = Color.Empty;
            RichTextBox rtxtCurrent;
            FontStyle actualStyle = FontStyle.Regular;
            SortedList<int, classPreferencesExamples> specificText;

            if (areaCode == 5)
            {
                SortedList<int, classPreferencesExamples> exampleText = new SortedList<int, classPreferencesExamples>();

                prefBackColour.TryGetValue(5, out tempColour);
                dgvSearchResults.DefaultCellStyle.BackColor = tempColour;
                prefForeSimpleColour.TryGetValue(5, out tempColour);
                dgvSearchResults.DefaultCellStyle.ForeColor = tempColour;
                prefForeSimpleColour.TryGetValue(5, out tempColour);
                prefSimpleTextSize.TryGetValue(5, out fontSize);
                prefSimpleFontName.TryGetValue(5, out fontName);
                prefSimpleStyle.TryGetValue(5, out fontStyle);
                switch (fontStyle)
                {
                    case "Regular": actualStyle = FontStyle.Regular; break;
                    case "Bold": actualStyle = FontStyle.Bold; break;
                    case "Italic": actualStyle = FontStyle.Italic; break;
                    case "Bold and Italic": actualStyle = FontStyle.Bold | FontStyle.Italic; break;
                }
                dgvSearchResults.DefaultCellStyle.Font = new Font(fontName, fontSize, actualStyle);
                noOfLines = vocabSample.Length / 3;
//                dgvSearchResults.ColumnCount = 3;
                dgvSearchResults.Columns[0].HeaderText = "Grammar Type";
                dgvSearchResults.Columns[1].HeaderText = "Root Word";
                dgvSearchResults.Columns[2].HeaderText = "Word used";
                dgvSearchResults.RowCount = noOfLines;
                dgvSearchResults.SelectAll();
                dgvSearchResults.ClearSelection();
                for (lineIndex = 0; lineIndex < noOfLines; lineIndex++)
                {
                    if (vocabSample[lineIndex, 0].Length > 0) dgvSearchResults.Rows[lineIndex].Cells[0].Value = vocabSample[lineIndex, 0];
                    dgvSearchResults.Rows[lineIndex].Cells[1].Value = vocabSample[lineIndex, 1];
                    dgvSearchResults.Rows[lineIndex].Cells[2].Value = vocabSample[lineIndex, 2];
                }
                noOfWordsInExample[5] = 0;
            }
            else
            {
                specificText = exampleText[areaCode];
                rtxtCurrent = rtxtExample[areaCode];
                rtxtCurrent.Clear();
                prefBackColour.TryGetValue(areaCode, out tempColour);
                rtxtCurrent.BackColor = tempColour;
                foreach (KeyValuePair<int, classPreferencesExamples> egPair in specificText)
                {
                    typeCode = egPair.Value.TypeCode;
                    switch (typeCode)
                    {
                        case 1:
                            prefForeSimpleColour.TryGetValue(areaCode, out tempColour);
                            prefSimpleTextSize.TryGetValue(areaCode, out fontSize);
                            prefSimpleFontName.TryGetValue(areaCode, out fontName);
                            prefSimpleStyle.TryGetValue(areaCode, out fontStyle);
                            switch (fontStyle)
                            {
                                case "Regular": actualStyle = FontStyle.Regular; break;
                                case "Bold": actualStyle = FontStyle.Bold; break;
                                case "Italic": actualStyle = FontStyle.Italic; break;
                                case "Bold and Italic": actualStyle = FontStyle.Bold | FontStyle.Italic; break;
                            }
                            break;
                        case 2:
                            prefForeMainColour.TryGetValue(areaCode, out tempColour);
                            prefMainTextSize.TryGetValue(areaCode, out fontSize);
                            prefMainFontName.TryGetValue(areaCode, out fontName);
                            prefMainStyle.TryGetValue(areaCode, out fontStyle);
                            switch (fontStyle)
                            {
                                case "Regular": actualStyle = FontStyle.Regular; break;
                                case "Bold": actualStyle = FontStyle.Bold; break;
                                case "Italic": actualStyle = FontStyle.Italic; break;
                                case "Bold and Italic": actualStyle = FontStyle.Bold | FontStyle.Italic; break;
                            }
                            break;
                        case 3:
                            prefForePrimaryColour.TryGetValue(areaCode, out tempColour);
                            prefPrimaryTextSize.TryGetValue(areaCode, out fontSize);
                            prefPrimaryFontName.TryGetValue(areaCode, out fontName);
                            prefPrimaryStyle.TryGetValue(areaCode, out fontStyle);
                            switch (fontStyle)
                            {
                                case "Regular": actualStyle = FontStyle.Regular; break;
                                case "Bold": actualStyle = FontStyle.Bold; break;
                                case "Italic": actualStyle = FontStyle.Italic; break;
                                case "Bold and Italic": actualStyle = FontStyle.Bold | FontStyle.Italic; break;
                            }
                            break;
                        case 4:
                            prefForeSecondaryColour.TryGetValue(areaCode, out tempColour);
                            prefSecondaryTextSize.TryGetValue(areaCode, out fontSize);
                            prefSecondaryFontName.TryGetValue(areaCode, out fontName);
                            prefSecondaryStyle.TryGetValue(areaCode, out fontStyle);
                            switch (fontStyle)
                            {
                                case "Regular": actualStyle = FontStyle.Regular; break;
                                case "Bold": actualStyle = FontStyle.Bold; break;
                                case "Italic": actualStyle = FontStyle.Italic; break;
                                case "Bold and Italic": actualStyle = FontStyle.Bold | FontStyle.Italic; break;
                            }
                            break;
                        case 5:
                            prefForeMainColour.TryGetValue(areaCode, out tempColour);
                            prefMainTextSize.TryGetValue(areaCode, out fontSize);
                            prefMainFontName.TryGetValue(areaCode, out fontName);
                            prefMainStyle.TryGetValue(areaCode, out fontStyle);
                            switch (fontStyle)
                            {
                                case "Regular": actualStyle = FontStyle.Regular; break;
                                case "Bold": actualStyle = FontStyle.Bold; break;
                                case "Italic": actualStyle = FontStyle.Italic; break;
                                case "Bold and Italic": actualStyle = FontStyle.Bold | FontStyle.Italic; break;
                            }
                            break;
                        case 6:
                            prefForeMainColour.TryGetValue(areaCode, out tempColour);
                            prefMainTextSize.TryGetValue(areaCode, out fontSize);
                            prefMainFontName.TryGetValue(areaCode, out fontName);
                            prefMainStyle.TryGetValue(areaCode, out fontStyle);
                            switch (fontStyle)
                            {
                                case "Regular": actualStyle = FontStyle.Regular; break;
                                case "Bold": actualStyle = FontStyle.Bold; break;
                                case "Italic": actualStyle = FontStyle.Italic; break;
                                case "Bold and Italic": actualStyle = FontStyle.Bold | FontStyle.Italic; break;
                            }
                            break;
                    }
                    rtxtCurrent.SelectionColor = tempColour;
                    rtxtCurrent.SelectionFont = new Font(fontName, fontSize, actualStyle);
                    switch (typeCode)
                    {
                        case 1:
                            if (areaCode == 4) rtxtCurrent.SelectedText = egPair.Value.Text + ": ";
                            else rtxtCurrent.SelectedText = egPair.Value.Text; break;
                        case 2:
                        case 3:
                        case 4:
                            if ((areaCode == 0) || (areaCode == 1) || (areaCode == 4))
                                rtxtCurrent.SelectedText = " " + egPair.Value.Text;
                            else rtxtCurrent.SelectedText = egPair.Value.Text;
                            break;
                        case 5: rtxtCurrent.SelectedText = "\n"; break;
                        case 6: rtxtCurrent.SelectedText = "\n\n"; break;
                    }
                }
            }
        }

        private void addToClass(String text, int code, int noOfItems, SortedList<int, classPreferencesExamples> textSample)
        {
            classPreferencesExamples currentExampleClass;

            currentExampleClass = new classPreferencesExamples();
            currentExampleClass.Text = text;
            currentExampleClass.TypeCode = code;
                textSample.Add(noOfItems, currentExampleClass);
        }

        private TabPage createNewTabPage(String name)
        {
            TabPage tempPage;

            tempPage = new TabPage();
            tempPage.Text = name;
            return tempPage;
        }

        private void pictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            int tagVal, areaVal, pboxType;
            Color tempColour = Color.Empty;
            PictureBox currentPBox;
            SortedList<int, Color> selectedPBox = null;

            currentPBox = (PictureBox)sender;
            tagVal = Convert.ToInt32(currentPBox.Tag);
            areaVal = tagVal / 10;
            pboxType = tagVal % 10;
            switch (pboxType)
            {
                case 0: selectedPBox = prefBackColour; break;
                case 1: selectedPBox = prefForeSimpleColour; break;
                case 2: selectedPBox = prefForeMainColour; break;
                case 3: selectedPBox = prefForePrimaryColour; break;
                case 4: selectedPBox = prefForeSecondaryColour; break;
            }
            selectedPBox.TryGetValue(areaVal, out tempColour);
            if (tempColour != Color.Empty)
            {
                dlgBgColour.Color = tempColour;
                if (dlgBgColour.ShowDialog() == DialogResult.OK)
                {
                    selectedPBox.Remove(areaVal);
                    selectedPBox.Add(areaVal, dlgBgColour.Color);
                    currentPBox.BackColor = dlgBgColour.Color;
                    displayExampleText(areaVal);
                    hasChanged[areaVal] = true;
                    btnReset[areaVal].Enabled = true;
                }
            }
        }

        private void BtnFont_Click(object sender, EventArgs e)
        {
            int tagVal, areaVal, subVal;
            float actualSize = 12F;
            String styleName = "", actualFont = "";
            FontStyle actualFontStyle = FontStyle.Regular;
            Font newFont;
            Button btnClicked;
            SortedList<int, float> textSize = null;
            SortedList<int, String> textStyle = null;
            SortedList<int, String> fontName = null;

            btnClicked = (Button)sender;
            tagVal = Convert.ToInt32(btnClicked.Tag);
            areaVal = tagVal / 10;
            subVal = tagVal % 10;
            switch (subVal)
            {
                case 0:
                    textSize = prefSimpleTextSize;
                    textStyle = prefSimpleStyle;
                    fontName = prefSimpleFontName;
                    break;
                case 1:
                    textSize = prefMainTextSize;
                    textStyle = prefMainStyle;
                    fontName = prefMainFontName;
                    break;
                case 2:
                    textSize = prefPrimaryTextSize;
                    textStyle = prefPrimaryStyle;
                    fontName = prefPrimaryFontName;
                    break;
                case 3:
                    textSize = prefSecondaryTextSize;
                    textStyle = prefSecondaryStyle;
                    fontName = prefSecondaryFontName;
                    break;
            }
            textSize.TryGetValue(areaVal, out actualSize);
            textStyle.TryGetValue(areaVal, out styleName);
            fontName.TryGetValue(areaVal, out actualFont);
            switch (styleName)
            {
                case "Regular": actualFontStyle = FontStyle.Regular; break;
                case "Bold": actualFontStyle = FontStyle.Bold; break;
                case "Italic": actualFontStyle = FontStyle.Italic; break;
                case "Bold and Italic": actualFontStyle = FontStyle.Bold | FontStyle.Italic; break;
            }
            dlgFont.Font = new Font(actualFont, actualSize, actualFontStyle);
            if (dlgFont.ShowDialog() == DialogResult.OK)
            {
                newFont = dlgFont.Font;
                actualSize = newFont.Size;
                actualFont = newFont.Name;
                actualFontStyle = newFont.Style;
                switch (actualFontStyle)
                {
                    case FontStyle.Regular: styleName = "Regular"; break;
                    case FontStyle.Bold: styleName = "Bold"; break;
                    case FontStyle.Italic: styleName = "Italic"; break;
                    case FontStyle.Bold | FontStyle.Italic: styleName = "Bold and Italic"; break;
                }
                textSize.Remove(areaVal);
                textStyle.Remove(areaVal);
                fontName.Remove(areaVal);
                textSize.Add(areaVal, actualSize);
                textStyle.Add(areaVal, styleName);
                fontName.Add(areaVal, actualFont);
                displayExampleText(areaVal);
                hasChanged[areaVal] = true;
                btnReset[areaVal].Enabled = true;
            } 
        }

        private void resetButton(object sender, EventArgs e)
        {
            int tagVal;
            Button btnDoReset;

            btnDoReset = (Button)sender;
            tagVal = Convert.ToInt32(btnDoReset.Tag);

            prefSimpleTextSize.Remove(tagVal);
            if (tagVal < 5) prefMainTextSize.Remove(tagVal);
            if (tagVal == 4) prefPrimaryTextSize.Remove(4);
            if (tagVal == 4) prefSecondaryTextSize.Remove(4);

            prefSimpleStyle.Remove(tagVal);
            if (tagVal < 5) prefMainStyle.Remove(tagVal);
            if (tagVal == 4) prefPrimaryStyle.Remove(4);
            if (tagVal == 4) prefSecondaryStyle.Remove(4);

            prefSimpleFontName.Remove(tagVal);
            if (tagVal < 5) prefMainFontName.Remove(tagVal);
            if (tagVal == 4) prefPrimaryFontName.Remove(4);
            if (tagVal == 4) prefSecondaryFontName.Remove(4);

            prefBackColour.Remove(tagVal);
            prefForeSimpleColour.Remove(tagVal);
            if (tagVal < 5) prefForeMainColour.Remove(tagVal);
            if (tagVal == 4) prefForePrimaryColour.Remove(4);
            if (tagVal == 4) prefForeSecondaryColour.Remove(4);
            populateSourceData(tagVal);
            displayExampleText(tagVal);
            hasChanged[tagVal] = false;
            btnReset[tagVal].Enabled = false;
        }

        private void processReset(int tagVal, int actionType,
                                  SortedList<int, Color> colourList,
                                  SortedList<int, float> sizeList,
                                  SortedList<int, String> styleList,
                                  SortedList<int, String> fontList)
        {
            Color newColour;

            newColour = globalVars.getColourSetting(tagVal, actionType);
            colourList.Remove(tagVal);
            colourList.Add(tagVal, newColour);
            colourBoxes[tagVal, actionType].BackColor = newColour;
            sizeList.Remove(tagVal);
            sizeList.Add(tagVal, globalVars.getTextSize(tagVal, actionType));
            styleList.Remove(tagVal);
            styleList.Add(tagVal, globalVars.getDefinedStyleByIndex(tagVal, actionType));
            fontList.Remove(tagVal);
            fontList.Add(tagVal, globalVars.getDefinedFontNameByIndex(tagVal, actionType));
        }

        private void populateGlobalData()
        {
            /*--------------------------------------------------------------------------------------------------*
             *                                                                                                  *
             *  Get current values from classGlobal                                                             *
             *                                                                                                  *
             *  In all that follows, the indexes representing different application areas function as follows:  *
             *                                                                                                  *
             *                   Value              Refers to                                                   *
             *                   -----              ---------                                                   *
             *                     0            Main MT Text area                                               *
             *                     1            Main LXX Text area                                              *
             *                     2            Parse area (for both MT and LXX)                                *
             *                     3            Lexical Area (for LXX)                                          *
             *                     4            Search Results Area when using MT                               *
             *                     5            Search Results Area when using LXX                              *
             *                     6            Notes Area for MT                                               *
             *                     7            Motes area for LXX                                              *
             *                                                                                                  *
             *--------------------------------------------------------------------------------------------------*/

            int idx, jdx, limit = 0;
            Color tempColour;

            for (idx = 0; idx < noOfAreas; idx++)
            {
                if (hasChanged[idx])
                {
                    prefBackColour.TryGetValue(idx, out tempColour);
                    globalVars.addColourSetting(idx, tempColour, 0);
                    switch (idx)
                    {
                        case 0: limit = 2; break;
                        case 1: limit = 2; break;
                        case 2: limit = 2; break;
                        case 3: limit = 2; break;
                        case 4: limit = 4; break;
                        case 5: limit = 1; break;
                        case 6: limit = 1; break;
                    }
                    for (jdx = 1; jdx <= limit; jdx++)
                    {
                        switch (jdx)
                        {
                            case 1: modifyGlobalData(idx, jdx, prefForeSimpleColour, prefSimpleTextSize, prefSimpleStyle, prefSimpleFontName); break;
                            case 2: modifyGlobalData(idx, jdx, prefForeMainColour, prefMainTextSize, prefMainStyle, prefMainFontName); break;
                            case 3: modifyGlobalData(idx, jdx, prefForePrimaryColour, prefPrimaryTextSize, prefPrimaryStyle, prefPrimaryFontName); break;
                            case 4: modifyGlobalData(idx, jdx, prefForeSecondaryColour, prefSecondaryTextSize, prefSecondaryStyle, prefSecondaryFontName); break;
                        }
                    }
                }
            }
        }

        private void modifyGlobalData(int tagVal, int actionType,
                                  SortedList<int, Color> colourList,
                                  SortedList<int, float> sizeList,
                                  SortedList<int, String> styleList,
                                  SortedList<int, String> fontList)
        {
            float newSize = 12F;
            String newStyle = "", newFont = "";
            Color newColour = Color.Empty;

            colourList.TryGetValue(tagVal, out newColour);
            globalVars.addColourSetting(tagVal, newColour, actionType);
            sizeList.TryGetValue(tagVal, out newSize);
            globalVars.addTextSize(tagVal, newSize, actionType);
            styleList.TryGetValue(tagVal, out newStyle);
            globalVars.addDefinedStyle(tagVal, newStyle, actionType);
            fontList.TryGetValue(tagVal, out newFont);
            globalVars.addFontName(tagVal, newFont, actionType);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            populateGlobalData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
