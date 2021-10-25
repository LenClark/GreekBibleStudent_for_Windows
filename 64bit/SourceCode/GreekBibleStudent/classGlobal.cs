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
        /*------------------------------------------------------------------------------------------------------------------------*
         *                                                                                                                        *
         *                                                       Global Variable                                                  * 
         *                                                       ---------------                                                  * 
         *                                                                                                                        *
         *  The purpose of this calss is to manage all variables that are global to the entire project in one, easily             *
         *  identifiable place.                                                                                                   *
         *                                                                                                                        *
         *  Variables:                                                                                                            *
         *  =========                                                                                                             *
         *                                                                                                                        *
         *  1) Variables controling form sizes and positions                                                                      *
         *     ---------------------------------------------                                                                      *
         *                                                                                                                        *
         *  windowX              The left pixel position in the main window/desktop                                               *
         *  windowY              The top position of the application form                                                         *
         *  windowHeight         The height of the main form                                                                      *
         *  windowWidth          The width of the main form                                                                       *
         *  splitPstn            The position of the split bar of the main, outer splitter container                              *     
         *  historyMax           The maximum number of past references that are retained by the application.  (This is a constant *
         *                       but including it as a global variable means it can be changed in the future)                     *    
         *  displayTextCode      ?                                                                                                *
         *  latestMousePosition  Stored whenever the user clicks on the main text area                                            *
         *  latestNotesPosition  Similar activity for notes                                                                       *
         *                                                                                                                        *
         *  2) Variables controling and storing references to main window controls                                                *
         *     -------------------------------------------------------------------                                                *
         *                                                                                                                        *
         *  noOfRichtextBoxes    Identifies the number of areas in the application that use RichTextBox controls.  It sets the    *                                                    
         *                       maximums for the array, rtxtCollection.  These areas include:                                    *
         *                       a) the main bible text area;                                                                     *
         *                       b) the parse area;                                                                               *
         *                       c) the lexical analysis area;                                                                    *
         *                       d) the search results area;                                                                      *
         *                       e) the lists area;                                                                               *
         *                       f) the notes area.                                                                               *
         *  noOfComboBoxes       Identifies the number of ComboBox controls.  It sets the maximums for the array, cbCollection.   *
         *                       These include:                                                                                   *
         *                       a) the books pull-down list;                                                                     *
         *                       b) the chapters pull-down-list;                                                                  *
         *                       c) the verses pull-down list;                                                                    *
         *                       d) the NT history list;                                                                          *
         *                       e) the septuagint list.                                                                          *
         *  noOfWebBrowsers      Identifies the number of webBrowser controls used.  It applies to the presentation of Liddell &  *
         *                       Scott information.                                                                               *
         *  rtxtCollection       The array containing the RichTextBox control addresses                                           *
         *  cbCollection         The array containing the ComboBox control addresses                                              *
         *  webCollection        The array containing the WebBrowser control addresses                                            *
         *                                                                                                                        *
         *  3) Variables managing information about file locations                                                                *
         *     ---------------------------------------------------                                                                *
         *                                                                                                                        *
         *  baseDirectory        The root directory of all subsequent directories and files                                       *
         *  altBaseDirectory     This is used as baseDirectory in development.                                                    *
         *  sourceFolder         The name of the folder containing text information                                               *
         *  ntTextFolder         The folder (within Source) that contains NT text data                                            *
         *  lxxTextFolder        The folder (within Source) that contains LXX text data                                           *
         *  ntTitlesFile         The name of the file holding information about each NT book                                      *
         *  lxxTitlesFile        The name of the file holding information about each LXX book                                     *
         *  lexiconFolder        The folder (within Source) that contains all lexical data                                        *
         *  keyboardFolder       The folder containing data supporting the presentation of the virtual keyboard                   *
         *  liddellAndScott      The name of the file containing appendices for the lexicon                                       *
         *  helpPath             The folder containing the Help file and all dependant files                                      *
         *  notesPath            The folder forming the root of all notes storage                                                 *
         *  notesName            The name of the notes set (and folder) currently active                                          *
         *  gkGroupsName         A file that provides additional decode information                                               *
         *                                                                                                                        *
         *  4) Variables that assist the management of registry storage                                                           *
         *     --------------------------------------------------------                                                           *
         *                                                                                                                        *
         *  registryKeyValue     An array containing registry key names                                                           *
         *                                                                                                                        *
         *  noOfSearchItems      ?                                                                                                *
         *  noOfSearchObjects    ?                                                                                                *
         *                                                                                                                        *
         *------------------------------------------------------------------------------------------------------------------------*/

        int windowX, windowY, windowWidth, windowHeight, splitPstn, historyMax, displayTextCode = 0, latestMousePosition = 0, latestNotesPosition = 0;
        int noOfRichtextBoxes = 0, noOfSearchItems = 0, noOfComboBoxes = 0, noOfWebBrowsers = 0, workingInt = 0, noOfSearchObjects = 0;
        String baseDirectory, altBaseDirectory, sourceFolder = "Source", ntTitlesFile = "NTTitles.txt", lxxTitlesFile = "LXXTitles.txt",
            ntTextFolder = "NT", lxxTextFolder = "LXX", helpPath = "Help", notesPath = "Notes", notesName = "default", lexiconFolder = "Lexicon",
            keyboardFolder = "Keyboard", gkGroupsName = @"Gk\GkCharsGrouped.txt", liddellAndScott = "LandSSummary.txt";
        String displayCodeName = "Book Display Code";
        String[] registryKeyValue = { "NT Text Colour", "NT Text Background Colour", "NT Text Font Size",
                                        "LXX Text Colour", "LXX Text Background Colour", "LXX Text Font Size",
                                          "Parse Area Text Colour", "Parse Area Background Colour", "Parse Area Font Size",
                                          "Lexical Area Text Colour", "Lexical Area Background Colour", "Lexical Area Font Size",
                                          "Search Results Text Colour", "Search Results Background Colour", "Search Results Font Size",
                                          "Notes Text Colour", "Notes Background Colour", "Notes Font Size",
                                          "Vocab List Text Colour", "Vocab List Background Colour", "Vocab List Font Size"  };
        String primaryKeyValue = "Primary Search Result Colour", secondaryKeyValue = "Secondary Search Result Colour";
        SortedList<int, ComboBox> cbCollection = new SortedList<int, ComboBox>();
        SortedList<int, RichTextBox> rtxtCollection = new SortedList<int, RichTextBox>();
        SortedList<int, WebBrowser> webCollection = new SortedList<int, WebBrowser>();
        SortedList<int, String> appendixFileNames = new SortedList<int, string>();
        SortedList<int, TextBox> txtSearchItems = new SortedList<int, TextBox>();
        SortedList<int, Object> searchItemsForTextEntry = new SortedList<int, object>();
        SortedList<int, ToolStripStatusLabel> searchMessages = new SortedList<int, ToolStripStatusLabel>();

        Form masterForm;
        SplitContainer splitMain;
        TabControl tabCtrlBottomLeft, tabCtlText;

        TabPage ntTabPge, lxxTabPge, notesTabPage;
        Color primaryColour, secondaryColour;
        Color[] foreColorRec, backColorRec;
        Font[] fontForArea;
        ToolStripMenuItem parentMenuItem;
        StatusStrip statStrip;

        public int WindowX { get => windowX; set => windowX = value; }
        public int WindowY { get => windowY; set => windowY = value; }
        public int WindowWidth { get => windowWidth; set => windowWidth = value; }
        public int WindowHeight { get => windowHeight; set => windowHeight = value; }
        public int SplitPstn { get => splitPstn; set => splitPstn = value; }
        public int HistoryMax { get => historyMax; set => historyMax = value; }
        public int NoOfRichtextBoxes { get => noOfRichtextBoxes; }
        public int NoOfSearchItems { get => noOfSearchItems; }
        public int NoOfSearchObjects { get => noOfSearchObjects; }
        public int NoOfComboBoxes { get => noOfComboBoxes; }
        public int NoOfWebBrowsers { get => noOfWebBrowsers; }
        public int DisplayTextCode { get => displayTextCode; set => displayTextCode = value; }
        public string BaseDirectory { get => baseDirectory; set => baseDirectory = value; }
        public string AltBaseDirectory { get => altBaseDirectory; set => altBaseDirectory = value; }
        public string SourceFolder { get => sourceFolder; set => sourceFolder = value; }
        public string NtTitlesFile { get => ntTitlesFile; set => ntTitlesFile = value; }
        public string LxxTitlesFile { get => lxxTitlesFile; set => lxxTitlesFile = value; }
        public string NtTextFolder { get => ntTextFolder; set => ntTextFolder = value; }
        public string LxxTextFolder { get => lxxTextFolder; set => lxxTextFolder = value; }
        public string LexiconFolder { get => lexiconFolder; set => lexiconFolder = value; }
        public string HelpPath { get => helpPath; set => helpPath = value; }
        public string NotesPath { get => notesPath; set => notesPath = value; }
        public string NotesName { get => notesName; set => notesName = value; }
        public string KeyboardFolder { get => keyboardFolder; set => keyboardFolder = value; }
        public string GkGroupsName { get => gkGroupsName; set => gkGroupsName = value; }
        public string LiddellAndScott { get => liddellAndScott; set => liddellAndScott = value; }
        public string DisplayCodeName { get => displayCodeName; set => displayCodeName = value; }

        /*------------------------------------------------------------------------*
         *                                                                        * 
         *                    Globally Accessible Controls                        * 
         *                    ----------------------------                        * 
         *                                                                        *
         *------------------------------------------------------------------------*/

        public Form MasterForm { get => masterForm; set => masterForm = value; }
        public SplitContainer SplitMain { get => splitMain; set => splitMain = value; }
        public TabControl TabCtrlBottomLeft { get => tabCtrlBottomLeft; set => tabCtrlBottomLeft = value; }
        public TabControl TabCtlText { get => tabCtlText; set => tabCtlText = value; }
        public string[] RegistryKeyValue { get => registryKeyValue; set => registryKeyValue = value; }
        public Color[] ForeColorRec { get => foreColorRec; set => foreColorRec = value; }
        public Color[] BackColorRec { get => backColorRec; set => backColorRec = value; }
        public Font[] FontForArea { get => fontForArea; set => fontForArea = value; }
        public ToolStripMenuItem ParentMenuItem { get => parentMenuItem; set => parentMenuItem = value; }
        public SortedList<int, WebBrowser> WebCollection { get => webCollection; set => webCollection = value; }
        public SortedList<int, string> AppendixFileNames { get => appendixFileNames; set => appendixFileNames = value; }
        public TabPage NtTabPge { get => ntTabPge; set => ntTabPge = value; }
        public TabPage LxxTabPge { get => lxxTabPge; set => lxxTabPge = value; }
        public TabPage NotesTabPage { get => notesTabPage; set => notesTabPage = value; }

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
        public SortedDictionary<int, string> BaseGkChars { get => baseGkChars; set => baseGkChars = value; }
        public SortedDictionary<int, string> FurtherGkChars { get => furtherGkChars; set => furtherGkChars = value; }
        public string DigammaUpper { get => digammaUpper; }
        public string DigammaLower { get => digammaLower; }
        public SortedList<int, ToolStripStatusLabel> SearchMessages { get => searchMessages; set => searchMessages = value; }
        public StatusStrip StatStrip { get => statStrip; set => statStrip = value; }
        public int LatestMousePosition { get => latestMousePosition; set => latestMousePosition = value; }
        public int LatestNotesPosition { get => latestNotesPosition; set => latestNotesPosition = value; }
        public Color PrimaryColour { get => primaryColour; set => primaryColour = value; }
        public Color SecondaryColour { get => secondaryColour; set => secondaryColour = value; }
        public string PrimaryKeyValue { get => primaryKeyValue; set => primaryKeyValue = value; }
        public string SecondaryKeyValue { get => secondaryKeyValue; set => secondaryKeyValue = value; }

        public classGlobal()
        {
            int idx;
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
        }

        public void addRichtextControl(RichTextBox newControl)
        {
            rtxtCollection.Add(noOfRichtextBoxes++, newControl);
        }

        public RichTextBox getRichtextControlByIndex(int index)
        {
            RichTextBox currentRtxtBox;

            if (!rtxtCollection.ContainsKey(index)) return null;
            rtxtCollection.TryGetValue(index, out currentRtxtBox);
            return currentRtxtBox;
        }

        public void addSearchTextControl(TextBox newControl)
        {
            txtSearchItems.Add(noOfSearchItems++, newControl);
        }

        public TextBox getSearchTextControlByIndex(int index)
        {
            TextBox currentTxtBox;

            if (!txtSearchItems.ContainsKey(index)) return null;
            txtSearchItems.TryGetValue(index, out currentTxtBox);
            return currentTxtBox;
        }

        public void addComboBoxControl(ComboBox newControl)
        {
            cbCollection.Add(noOfComboBoxes++, newControl);
        }

        public ComboBox getComboBoxControlByIndex(int index)
        {
            ComboBox currentCbBox;

            if (!cbCollection.ContainsKey(index)) return null;
            cbCollection.TryGetValue(index, out currentCbBox);
            return currentCbBox;
        }

        public void addWebControl(WebBrowser newControl)
        {
            webCollection.Add(noOfWebBrowsers++, newControl);
        }

        public WebBrowser getWebControlByIndex(int index)
        {
            WebBrowser currentBrowser;

            if (!webCollection.ContainsKey(index)) return null;
            webCollection.TryGetValue(index, out currentBrowser);
            return currentBrowser;
        }

        public void addWebFileNames(String newText)
        {
            appendixFileNames.Add(workingInt++, newText);
        }

        public String getWebFileNamesByIndex(int index)
        {
            String currentBrowserFileName;

            if (!appendixFileNames.ContainsKey(index)) return null;
            appendixFileNames.TryGetValue(index, out currentBrowserFileName);
            return currentBrowserFileName;
        }

        public void addSearchItemsForTextEntry(Object newItem)
        {
            searchItemsForTextEntry.Add(noOfSearchObjects++, newItem);
        }

        public Object getSearchItemsForTextEntryByIndex(int index)
        {
            Object currentItem;

            if (!searchItemsForTextEntry.ContainsKey(index)) return null;
            searchItemsForTextEntry.TryGetValue(index, out currentItem);
            return currentItem;
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
    }
}
