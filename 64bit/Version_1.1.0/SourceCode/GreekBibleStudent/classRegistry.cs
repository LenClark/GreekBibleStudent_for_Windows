using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace GreekBibleStudent
{
    public class classRegistry
    {
        const int noOfAreas = 8;

        bool isInitialising = false;
        int defaultSplitPosition, defaultState, defaultHeight, defaultWidth, defaultX, defaultY;
        String keyString, cautionFileName = "deferredAction.txt";
        String[] keyColourNames = { "NT Text Background Colour", "NT Verse No. Colour", "NT Text Colour", "", "",
                                        "LXX Text Background Colour", "LXX Verse No. Colour", "LXX Text Colour", "", "",
                                          "Parse Area Background Colour", "Parse Area Text Colour", "Parse header Colour", "", "",
                                          "Lexical Area Background Colour", "Lexical Area Text Colour", "Lexical Area header Colour", "", "",
                                          "Search Results Background Colour", "Search Reference Colour", "Search Results Text Colour", "Search Results Primary Colour", "Search Results Secondary Colour",
                                          "Vocab List Background Colour", "Vocab List Text Colour", "", "", "",
                                          "Notes Background Colour", "Notes Text Colour", "", "", "" };
        String[] keyFontSizeNames = { "NT English Text Font Size", "NT Greek Text Font Size", "", "",
                                          "LXX English Text Font Size", "LXX Greek Text Font Size", "", "",
                                          "Parse Area Font Size", "Parse Title Font Size", "", "",
                                          "Lexical Area Font Size", "Lexical Title Font Size", "", "",
                                          "Search Results English Font Size", "Search Results Greek Font Size", "Search Primary Match Size", "Search Secondary Match Size",
                                          "Vocab List Font Size", "", "", "",
                                          "Notes Font Size", "", "", "" };
        String[] keyFontStyleNames = { "NT English Text Style", "NT Greek Text Style", "", "",
                                          "LXX English Text Style", "LXX Greek Text Style", "", "",
                                          "Parse Area Style", "Parse Title Style", "", "",
                                          "Lexical Area Style", "Lexical Title Style", "", "",
                                          "Search Results English Style", "Search Results Greek Style", "Search Primary Match Style", "Search Secondary Match Style",
                                          "Vocab List Style", "", "", "",
                                          "Notes Style", "", "", "" };
        String[] keyFontNameNames = { "NT English Text Font Name", "NT Greek Text Font Name", "", "",
                                          "LXX English Text Font Name", "LXX Greek Text Font Name", "", "",
                                          "Parse Area Font Name", "Parse Title Font Name", "", "",
                                          "Lexical Area Font Name", "Lexical Title Font Name", "", "",
                                          "Search Results English Font Name", "Search Results Greek Font Name", "Search Primary Font Name", "Search Secondary Font Name",
                                          "Vocab List Font Name", "", "", "",
                                          "Notes Font Name", "", "", "" };
        String[] keyNames = { "Base Directory" /*, "Source Folder" */, "NT Titles", "LXX Titles", "NT Folder", "LXX Folder", "Notes Folder",
                                   "Notes Name", "Help File", "Lexicon Path", "Keyboard Path", "Lexicon File" };
        RegistryKey baseKey;
        Form mainForm;

        classGlobal globalVars;

        public Form MainForm { get => mainForm; set => mainForm = value; }
        public int DefaultSplitPosition { get => defaultSplitPosition; set => defaultSplitPosition = value; }
        public int DefaultState { get => defaultState; set => defaultState = value; }
        public int DefaultHeight { get => defaultHeight; set => defaultHeight = value; }
        public int DefaultWidth { get => defaultWidth; set => defaultWidth = value; }
        public int DefaultX { get => defaultX; set => defaultX = value; }
        public int DefaultY { get => defaultY; set => defaultY = value; }
        public string KeyString { get => keyString; set => keyString = value; }

        public void initialiseRegistry(classGlobal inConfig, String inKey)
        {
            globalVars = inConfig;
            keyString = inKey;
            openRegistry();
            initialisePaths();
            initialiseWindowDetails();
        }

        public void openRegistry()
        {
            baseKey = Registry.CurrentUser.OpenSubKey(keyString, true);
        }

        public void closeKeys()
        {
            baseKey.Close();
            baseKey.Dispose();
        }

        private void initialisePaths()
        {
            /*===================================================================================*
             *                                                                                   *
             *   Manage File location details                                                    *
             *   ----------------------------                                                    *
             *                                                                                   *
             * Note: The actual directory names (Notes, Source) are "hard coded"                 *
             *                                                                                   *
             *===================================================================================*/

            String registryString;
            String baseDirectory;
            String ntTitlesFile = "NTTitles.txt", lxxTitlesFile = "LXXTitles.txt";
            String ntSourceFolder = "NT", lxxTextFolder = "LXX";
            String notesPath = "Notes", notesName = "Default";
            String helpPath = "Help", helpFile = "Help.html";
            String lexiconFolder = "Lexicon", mainLexFile = "LandSSummary.txt";
            String fullNtTitlesFile, fullLxxTitlesFile, fullNtTextFolder, fullLxxTextFolder, fullNotesPath, fullNotesName, fullHelpFile, fullLexiconFolder, 
                fullKeyboardFolder, fullLiddellAndScott;

            String keyboardFolder = "Keyboard", greekControlFolder = "Greek";
            String gkAccute = "accuteAccents.txt", gkCircumflex = "circumflexAccents.txt", gkDiaereses = "diaereses.txt", gkGrave = "graveAccents.txt",
                gkIota = "iotaSubscripts.txt", gkRough = "roughBreathings.txt", gkSmooth = "smoothBreathings.txt",
                gkConv1 = "breathingConversion1.txt", gkConv2 = "breathingConversion2.txt";

            String fileBuffer;
            StreamReader srCaution;

            isInitialising = true;
            // Now get file location details from the registry - and create if they don't exist
            if (baseKey == null)
            {
                // Go here if the base registry key doesn't exist
                baseDirectory = Path.GetFullPath(@"..\Files");
                globalVars.BaseDirectory = baseDirectory;
                globalVars.NtTitlesFile = baseDirectory + @"\Source\" + ntTitlesFile;
                globalVars.LxxTitlesFile = baseDirectory + @"\Source\" + lxxTitlesFile;
                globalVars.NtTextFolder = baseDirectory + @"\Source\" + ntSourceFolder;
                globalVars.LxxTextFolder = baseDirectory + @"\Source\" + lxxTextFolder;
                globalVars.NotesPath = baseDirectory + @"\" + notesPath;
                globalVars.NotesName = notesName;
                globalVars.HelpFile = baseDirectory + @"\" + helpPath + @"\" + helpFile;
                globalVars.LexiconFolder = baseDirectory + @"\Source\" + lexiconFolder;
                globalVars.KeyboardFolder = baseDirectory + @"\Source\" + keyboardFolder;
                globalVars.LiddellAndScott = mainLexFile;

                baseKey = Registry.CurrentUser.CreateSubKey(keyString);
                baseKey.SetValue(keyNames[0], baseDirectory, RegistryValueKind.String);
                baseKey.SetValue(keyNames[1], globalVars.NtTitlesFile, RegistryValueKind.String);
                baseKey.SetValue(keyNames[2], globalVars.LxxTitlesFile, RegistryValueKind.String);
                baseKey.SetValue(keyNames[3], globalVars.NtTextFolder, RegistryValueKind.String);
                baseKey.SetValue(keyNames[4], globalVars.LxxTextFolder, RegistryValueKind.String);
                baseKey.SetValue(keyNames[5], globalVars.NotesPath, RegistryValueKind.String);
                baseKey.SetValue(keyNames[6], globalVars.NotesName, RegistryValueKind.String);
                baseKey.SetValue(keyNames[7], globalVars.HelpFile, RegistryValueKind.String);
                baseKey.SetValue(keyNames[8], globalVars.LexiconFolder, RegistryValueKind.String);
                baseKey.SetValue(keyNames[9], globalVars.KeyboardFolder, RegistryValueKind.String);
                baseKey.SetValue(keyNames[10], globalVars.LiddellAndScott, RegistryValueKind.String);
            }
            else
            {
                registryString = registrySetting(keyNames[0], globalVars.BaseDirectory);
                if (registryString.Length > 0)
                {
                    baseDirectory = registryString;
                }
                else
                {
                    baseDirectory = Path.GetFullPath(@"..\Source");
                    baseKey.SetValue(keyNames[0], baseDirectory, RegistryValueKind.String);
                }
                globalVars.BaseDirectory = baseDirectory;
                fullNtTitlesFile = baseDirectory + @"\Source\" + ntTitlesFile;
                registryString = registrySetting(keyNames[1], fullNtTitlesFile);
                if (registryString.Length > 0)
                {
                    fullNtTitlesFile = registryString;
                }
                else baseKey.SetValue(keyNames[1], fullNtTitlesFile, RegistryValueKind.String);
                globalVars.NtTitlesFile = fullNtTitlesFile;
                fullLxxTitlesFile = baseDirectory + @"\Source\" + lxxTitlesFile;
                registryString = registrySetting(keyNames[2], fullLxxTitlesFile);
                if (registryString.Length > 0)
                {
                    fullLxxTitlesFile = registryString;
                }
                else baseKey.SetValue(keyNames[2], fullLxxTitlesFile, RegistryValueKind.String);
                globalVars.LxxTitlesFile = fullLxxTitlesFile;
                fullNtTextFolder = baseDirectory + @"\Source\" + ntSourceFolder;
                registryString = registrySetting(keyNames[3], fullNtTextFolder);
                if (registryString.Length > 0)
                {
                    fullNtTextFolder = registryString;
                }
                else baseKey.SetValue(keyNames[3], fullNtTextFolder, RegistryValueKind.String);
                globalVars.NtTextFolder = fullNtTextFolder;
                fullLxxTextFolder = baseDirectory + @"\Source\" + lxxTextFolder;
                registryString = registrySetting(keyNames[4], fullLxxTextFolder);
                if (registryString.Length > 0)
                {
                    fullLxxTextFolder = registryString;
                }
                else baseKey.SetValue(keyNames[4], fullLxxTextFolder, RegistryValueKind.String);
                globalVars.LxxTextFolder = fullLxxTextFolder;
                fullNotesPath = baseDirectory + @"\" + notesPath;
                registryString = registrySetting(keyNames[5], fullNotesPath);
                if (registryString.Length > 0)
                {
                    fullNotesPath = registryString;
                }
                else baseKey.SetValue(keyNames[5], fullNotesPath, RegistryValueKind.String);
                globalVars.NotesPath = fullNotesPath;
                fullNotesName = notesName;
                registryString = registrySetting(keyNames[6], fullNotesName);
                if (registryString.Length > 0)
                {
                    fullNotesName = registryString;
                }
                else baseKey.SetValue(keyNames[6], fullNotesName, RegistryValueKind.String);
                globalVars.NotesName = fullNotesName;
                fullHelpFile = baseDirectory + @"\" + helpPath + @"\" + helpFile;
                registryString = registrySetting(keyNames[7], fullHelpFile);
                if (registryString.Length > 0)
                {
                    fullHelpFile = registryString;
                }
                else baseKey.SetValue(keyNames[7], fullHelpFile, RegistryValueKind.String);
                globalVars.HelpFile = fullHelpFile;
                fullLexiconFolder = baseDirectory + @"\Source\" + lexiconFolder;
                registryString = registrySetting(keyNames[8], fullLexiconFolder);
                if (registryString.Length > 0)
                {
                    fullLexiconFolder = registryString;
                }
                else baseKey.SetValue(keyNames[8], fullLexiconFolder, RegistryValueKind.String);
                globalVars.LexiconFolder = fullLexiconFolder;
                fullKeyboardFolder = baseDirectory + @"\Source\" + keyboardFolder;
                registryString = registrySetting(keyNames[9], fullKeyboardFolder);
                if (registryString.Length > 0)
                {
                    fullKeyboardFolder = registryString;
                }
                else baseKey.SetValue(keyNames[9], fullKeyboardFolder, RegistryValueKind.String);
                globalVars.KeyboardFolder = fullKeyboardFolder;

                //             String fullLiddellAndScott;
                fullLiddellAndScott = mainLexFile;
                registryString = registrySetting(keyNames[10], fullLiddellAndScott);
                if (registryString.Length > 0)
                {
                    fullLiddellAndScott = registryString;
                }
                else baseKey.SetValue(keyNames[10], fullLiddellAndScott, RegistryValueKind.String);
                globalVars.LiddellAndScott = fullLiddellAndScott;
            }
            globalVars.FullGkAccute = baseDirectory + @"\" + greekControlFolder + @"\" + gkAccute;
            globalVars.FullGkCircumflex = baseDirectory + @"\" + greekControlFolder + @"\" + gkCircumflex;
            globalVars.FullGkDiaereses = baseDirectory + @"\" + greekControlFolder + @"\" + gkDiaereses;
            globalVars.FullGkGrave = baseDirectory + @"\" + greekControlFolder + @"\" + gkGrave;
            globalVars.FullGkIota = baseDirectory + @"\" + greekControlFolder + @"\" + gkIota;
            globalVars.FullGkRough = baseDirectory + @"\" + greekControlFolder + @"\" + gkRough;
            globalVars.FullGkSmooth = baseDirectory + @"\" + greekControlFolder + @"\" + gkSmooth;
            globalVars.FullGkConv1 = baseDirectory + @"\" + greekControlFolder + @"\" + gkConv1;
            globalVars.FullGkConv2 = baseDirectory + @"\" + greekControlFolder + @"\" + gkConv2;
            if (!Directory.Exists(globalVars.NotesPath)) Directory.CreateDirectory(globalVars.NotesPath);

            /*---------------------------------------------------------------------------------------------*
             *                                                                                             *
             *  What follows is quite fiddly and not very elegant.  It is the result of                    *
             *  a) the facility to move the application data (see code for reasons), and                   *
             *  b) the fact that some elements are used by the application when it loads -                 *
             *       which means we had to find a mechanism to defer the deletion until the next time the  *
             *       application is loaded, when it will be using data from the new location and the old   *
             *       one can now be removed.                                                               *
             *                                                                                             *
             *---------------------------------------------------------------------------------------------*/

            if (File.Exists(globalVars.BaseDirectory + @"\" + cautionFileName))
            {
                srCaution = new StreamReader(globalVars.BaseDirectory + @"\" + cautionFileName);
                fileBuffer = srCaution.ReadToEnd();
                srCaution.Close();
                srCaution.Dispose();
                Directory.Delete(fileBuffer, true);
                File.Delete(globalVars.BaseDirectory + @"\" + cautionFileName);
            }
            isInitialising = false;
        }

        private String registrySetting(String keyName, String fromRegistry)
        {
            String outcomeString;
            Object regValue;

            regValue = baseKey.GetValue(keyName);
            if (regValue != null)
            {
                outcomeString = regValue.ToString();
                return outcomeString;
            }
            else
            {
                baseKey.SetValue(keyName, fromRegistry, RegistryValueKind.String);
                return "";
            }
        }

        public void initialiseWindowDetails()
        {
            /*===================================================================================*
             *                                                                                   *
             * Step 2: Some window parameters                                                    *
             * ------------------------------                                                    *
             *                                                                                   *
             *===================================================================================*/
            String[] windowDetails = { "Window Position X", "Window Position Y", "Window Width", "Window Height", "Window State", "Main Splitter Distance" };
            String[] historySettings = { "Maximum History Entries" };
            String[] colourSettings = { };

            Object regValue;
            SplitContainer splitMain;
            Screen mainScreen = Screen.PrimaryScreen;

            isInitialising = true;
            defaultX = (Screen.PrimaryScreen.WorkingArea.Width - globalVars.MasterForm.Width) / 2;
            defaultY = (Screen.PrimaryScreen.WorkingArea.Height - globalVars.MasterForm.Height) / 2;
            defaultHeight = globalVars.MasterForm.Height;
            defaultWidth = globalVars.MasterForm.Width;
            defaultState = (int)globalVars.MasterForm.WindowState;
            defaultSplitPosition = (globalVars.getSplit(0)).SplitterDistance;
            regValue = baseKey.GetValue(windowDetails[0]);
            if (regValue == null)
            {
                baseKey.SetValue(windowDetails[0], defaultX, RegistryValueKind.DWord);
            }
            else
            {
                defaultX = (int)regValue;
            }
            globalVars.WindowX = defaultX;
            regValue = baseKey.GetValue(windowDetails[1]);
            if (regValue == null)
            {
                baseKey.SetValue(windowDetails[1], defaultY, RegistryValueKind.DWord);
            }
            else
            {
                defaultY = (int)regValue;
            }
            globalVars.WindowY = defaultY;
            regValue = baseKey.GetValue(windowDetails[2]);
            if (regValue == null)
            {
                baseKey.SetValue(windowDetails[2], defaultWidth, RegistryValueKind.DWord);
            }
            else
            {
                defaultWidth = (int)regValue;
            }
            globalVars.WindowWidth = defaultWidth;
            regValue = baseKey.GetValue(windowDetails[3]);
            if (regValue == null)
            {
                baseKey.SetValue(windowDetails[3], defaultHeight, RegistryValueKind.DWord);
            }
            else
            {
                defaultHeight = (int)regValue;
            }
            globalVars.WindowHeight = defaultHeight;
            regValue = baseKey.GetValue(windowDetails[4]);
            if (regValue == null)
            {
                baseKey.SetValue(windowDetails[4], defaultState, RegistryValueKind.DWord);
            }
            else
            {
                defaultState = (int)regValue;
            }
            globalVars.WindowState = defaultState;
            regValue = baseKey.GetValue(windowDetails[5]);
            splitMain = globalVars.getSplit(0);
            if (regValue == null)
            {
                baseKey.SetValue(windowDetails[5], defaultSplitPosition, RegistryValueKind.DWord);
            }
            else
            {
                defaultSplitPosition = (int)regValue;
            }
            regValue = baseKey.GetValue(historySettings[0]);
            if (regValue == null)
            {
                baseKey.SetValue(historySettings[0], globalVars.HistoryMax, RegistryValueKind.DWord);
            }
            else
            {
                globalVars.HistoryMax = (int)regValue;
            }
            if( (FormWindowState)defaultState == FormWindowState.Maximized)
            {
                globalVars.MasterForm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                globalVars.MasterForm.Top = defaultY;
                globalVars.MasterForm.Left = defaultX;
                globalVars.MasterForm.Height = defaultHeight;
                globalVars.MasterForm.Width = defaultWidth;
            }
            globalVars.getSplit(0).SplitterDistance = DefaultSplitPosition;
            isInitialising = false;
        }

        private void processColour(String registryName, Color defaultColour, int globalIndex, int colourType)
        {
            /*==================================================================================================*
             *                                                                                                  *
             *                                        processColour                                             *
             *                                        =============                                             *
             *                                                                                                  *
             *  A method for the generic processing of colours.                                                 *
             *                                                                                                  *
             *  Parameters:                                                                                     *
             *  ==========                                                                                      *
             *                                                                                                  *
             *  registryName   The name of the subKey                                                           *
             *  defaultColour  Used if not found in the registry                                                *
             *                                                                                                  *
             *  globalIndex:     Value              Refers to                                                   *
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
             *  colourType       Value              Refers to                                                   *
             *                   -----              ---------                                                   *
             *                     0           Background colour                                                *
             *                     1           Main text colour                                                 *
             *                     2           Alternative text colour                                          *
             *                     3           Primary alternative text colour                                  *
             *                     4           Second alternative text colour                                   *
             *                                                                                                  *
             *==================================================================================================*/

            Object regValue;
            Color currentColour;

            regValue = baseKey.GetValue(registryName);
            if (regValue == null)
            {
                currentColour = defaultColour;
                baseKey.SetValue(registryName, currentColour.ToArgb(), RegistryValueKind.DWord);
            }
            else
            {
                currentColour = Color.FromArgb((int)regValue);
            }
            globalVars.addColourSetting(globalIndex, currentColour, colourType);

        }

        public void initialiseFontsAndColour()
        {
            /*************************************************************************************
             *                                                                                   *
             *   Initialisation of Font colours and sizes and rich text area background colours  *
             *                                                                                   *
             *************************************************************************************/

            int idx, jdx, index = 0;
            Color defaultColour;
            Object regValue;

            isInitialising = true;

            for (jdx = 0; jdx < 7; jdx++)
            {
                for (idx = 0; idx < 4; idx++)
                {
                    // Font size
                    if (keyFontSizeNames[index].Length > 0)
                    {
                        regValue = baseKey.GetValue(keyFontSizeNames[index]);
                        if (regValue == null)
                        {
                            baseKey.SetValue(keyFontSizeNames[index], 12, RegistryValueKind.DWord);
                            globalVars.addTextSize(jdx, 12F, idx + 1);
                        }
                        else globalVars.addTextSize(jdx, Convert.ToInt32( regValue ), idx + 1);
                    }
                    // Font Style
                    if (keyFontStyleNames[index].Length > 0)
                    {
                        regValue = baseKey.GetValue(keyFontStyleNames[index]);
                        if (regValue == null)
                        {
                            baseKey.SetValue(keyFontStyleNames[index], "Regular", RegistryValueKind.String);
                            globalVars.addDefinedStyle(jdx, "Regular", idx + 1);
                        }
                        else globalVars.addDefinedStyle(jdx, regValue.ToString(), idx + 1);
                    }
                    // Font Name
                    if (keyFontNameNames[index].Length > 0)
                    {
                        regValue = baseKey.GetValue(keyFontNameNames[index]);
                        if (regValue == null)
                        {
                            baseKey.SetValue(keyFontNameNames[index], "Times New Roman", RegistryValueKind.String);
                            globalVars.addFontName(jdx, "Times New Roman", idx + 1);
                        }
                        else globalVars.addFontName(jdx, regValue.ToString(), idx + 1);
                    }
                    index++;
                }
            }
            // Text Colour
            index = 0;
            for (jdx = 0; jdx < 7; jdx++)
            {
                for (idx = 0; idx < 5; idx++)
                {
                    if (keyColourNames[index].Length > 0)
                    {
                        regValue = baseKey.GetValue(keyColourNames[index]);
                        if (regValue == null)
                        {
                            switch( idx)
                            {
                                case 0: defaultColour = Color.White; break;
                                case 1: 
                                case 2: defaultColour = Color.Black; break;
                                case 3: defaultColour = Color.Red; break;
                                case 4: defaultColour = Color.Blue; break;
                                default: defaultColour = Color.Empty; break;
                            }
                            baseKey.SetValue(keyColourNames[index], defaultColour.ToArgb(), RegistryValueKind.DWord);
                            globalVars.addColourSetting(jdx, defaultColour, idx);
                        }
                        else globalVars.addColourSetting(jdx, Color.FromArgb(Convert.ToInt32(regValue.ToString())), idx);
                    }
                    index++;
                }
            }

            isInitialising = false;
        }

        public void updateFontsAndColour()
        {
            int idx;

            for (idx = 0; idx < noOfAreas; idx++)
            {
                updateSingleAreaFontsAndColour(idx);
            }
        }

        public void updateSingleAreaFontsAndColour(int areaCode)
        {
            int jdx, index, indexBase;

            index = areaCode * 5;
            openRegistry();
            for (jdx = 0; jdx < 5; jdx++)
            {
                if (keyColourNames[index].Length > 0) baseKey.SetValue(keyColourNames[index], globalVars.getColourSetting(areaCode, jdx).ToArgb(), RegistryValueKind.DWord);
                index++;
            }
            indexBase = areaCode * 4;
            index = indexBase;
            for (jdx = 1; jdx < 4; jdx++)
            {
                if (keyFontSizeNames[index].Length > 0) baseKey.SetValue(keyFontSizeNames[index], globalVars.getTextSize(areaCode, jdx), RegistryValueKind.DWord);
                index++;
            }
            index = indexBase;
            for (jdx = 1; jdx < 4; jdx++)
            {
                if (keyFontStyleNames[index].Length > 0) baseKey.SetValue(keyFontStyleNames[index], globalVars.getDefinedStyleByIndex(areaCode, jdx), RegistryValueKind.String);
                index++;
            }
            index = indexBase;
            for (jdx = 1; jdx < 4; jdx++)
            {
                if (keyFontNameNames[index].Length > 0) baseKey.SetValue(keyFontNameNames[index], globalVars.getDefinedFontNameByIndex(areaCode, jdx), RegistryValueKind.String);
                index++;
            }
        }

        public void updateSplitterDistance()
        {
            int newSplitterDistance;
            SplitContainer targetSplitter;

            if (isInitialising) return;
            targetSplitter = globalVars.getSplit(0);
            newSplitterDistance = targetSplitter.SplitterDistance;
            openRegistry();
            baseKey.SetValue("Main Splitter Distance", newSplitterDistance, RegistryValueKind.DWord);
            closeKeys();
            globalVars.SplitPstn = newSplitterDistance;
        }

        public void updateWindowPosition()
        {
            int windowX, windowY;
            FormWindowState windowState;
            Form targetForm;

            if (isInitialising) return;
            targetForm = globalVars.MasterForm;
            windowX = targetForm.Left;
            windowY = targetForm.Top;
            windowState = targetForm.WindowState;
            if ((windowState == FormWindowState.Normal) || (windowState == FormWindowState.Minimized))
            {
                openRegistry();
                baseKey.SetValue("Window Position X", windowX, RegistryValueKind.DWord);
                baseKey.SetValue("Window Position Y", windowY, RegistryValueKind.DWord);
                closeKeys();
                globalVars.WindowX = windowX;
                globalVars.WindowY = windowY;
            }
        }

        public void updateWindowSize()
        {
            int windowWidth, windowHeight;
            FormWindowState windowState;
            Form targetForm;

            if (isInitialising) return;
            targetForm = globalVars.MasterForm;
            windowWidth = targetForm.Width;
            windowHeight = targetForm.Height;
            windowState = targetForm.WindowState;
            openRegistry();
            if ((windowState == FormWindowState.Normal) || (windowState == FormWindowState.Minimized))
            {
                baseKey.SetValue("Window Width", windowWidth, RegistryValueKind.DWord);
                baseKey.SetValue("Window Height", windowHeight, RegistryValueKind.DWord);
                globalVars.WindowWidth = windowWidth;
                globalVars.WindowHeight = windowHeight;
            }
            baseKey.SetValue("Window State", windowState, RegistryValueKind.DWord);
            closeKeys();
            globalVars.WindowState = (int)windowState;
        }

        public void updateNotesSet()
        {
            /*            if (isInitialising) return;
                        openRegistry();
                        baseKey.SetValue("Notes Name", globalVars.NotesName, RegistryValueKind.String);
                        closeKeys(); */
        }

        public bool relocateFiles(String newLocation)
        {
            String originalRoot;
            DirectoryInfo diSource, diTarget;
            StreamWriter swCaution;

            originalRoot = globalVars.BaseDirectory;
            if (!Directory.Exists(newLocation)) Directory.CreateDirectory(newLocation);
            // Make sure the target directory is empty
            diTarget = new DirectoryInfo(newLocation);
            try
            {
                foreach (DirectoryInfo diName in diTarget.GetDirectories()) diName.Delete(true);
            }
            catch
            {
                MessageBox.Show("You are not able to move the files to that location", "Application Files Relocation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            foreach (FileInfo fiFile in diTarget.GetFiles()) fiFile.Delete();
            // Now copy data from the current location
            diSource = new DirectoryInfo(originalRoot);
            foreach (DirectoryInfo diName in diSource.GetDirectories()) cloneSingleDirectory(diTarget, diName);
            // Now get any files in the current directory
            foreach (FileInfo fiFile in diSource.GetFiles()) fiFile.CopyTo(diTarget.FullName + @"\" + fiFile.Name);
            globalVars.BaseDirectory = newLocation;
            openRegistry();
            if (baseKey != null) baseKey.SetValue("Base Directory", newLocation, RegistryValueKind.String);
            globalVars.NtTitlesFile = revampRegString(globalVars.NtTitlesFile, 2);
            globalVars.LxxTitlesFile = revampRegString(globalVars.LxxTitlesFile, 2);
            globalVars.NtTextFolder = revampRegString(globalVars.NtTextFolder, 2);
            globalVars.LxxTextFolder = revampRegString(globalVars.LxxTextFolder, 2);
            globalVars.NotesPath = revampRegString(globalVars.NotesPath, 2);
            globalVars.HelpFile = revampRegString(globalVars.HelpFile, 2);
            globalVars.LexiconFolder = revampRegString(globalVars.LexiconFolder, 2);
            globalVars.KeyboardFolder = revampRegString(globalVars.KeyboardFolder, 2);
            baseKey.SetValue(keyNames[1], globalVars.NtTitlesFile, RegistryValueKind.String);
            baseKey.SetValue(keyNames[2], globalVars.LxxTitlesFile, RegistryValueKind.String);
            baseKey.SetValue(keyNames[3], globalVars.NtTextFolder, RegistryValueKind.String);
            baseKey.SetValue(keyNames[4], globalVars.LxxTextFolder, RegistryValueKind.String);
            baseKey.SetValue(keyNames[5], globalVars.NotesPath, RegistryValueKind.String);
            baseKey.SetValue(keyNames[7], globalVars.HelpFile, RegistryValueKind.String);
            baseKey.SetValue(keyNames[8], globalVars.LexiconFolder, RegistryValueKind.String);
            baseKey.SetValue(keyNames[9], globalVars.KeyboardFolder, RegistryValueKind.String);
            closeKeys();
            try
            {
                Directory.Delete(originalRoot, true);
            }
            catch
            {
                swCaution = new StreamWriter(newLocation + @"\" + cautionFileName);
                swCaution.Write(originalRoot);
                swCaution.Close();
            }
            return true;
        }

        public void cloneSingleDirectory(DirectoryInfo diTargetDirectory, DirectoryInfo diSourceDirectory)
        {
            DirectoryInfo diTarget;

            // Create the new directry
            if (!Directory.Exists(diTargetDirectory.FullName + @"\" + diSourceDirectory.Name)) Directory.CreateDirectory(diTargetDirectory.FullName + @"\" + diSourceDirectory.Name);
            // Get info for the new directory
            diTarget = new DirectoryInfo(diTargetDirectory.FullName + @"\" + diSourceDirectory.Name);
            // Now go down a level and do the same
            foreach (DirectoryInfo diName in diSourceDirectory.GetDirectories()) cloneSingleDirectory(diTarget, diName);
            // Now get any files in the current directoy
            foreach (FileInfo fiFile in diSourceDirectory.GetFiles()) fiFile.CopyTo(diTargetDirectory.FullName + @"\" + diSourceDirectory.Name + @"\" + fiFile.Name);
        }

        private String revampRegString(String oldString, int noOfLevels)
        {
            int nPstn;
            String fileName;

            nPstn = oldString.LastIndexOf('\\');
            if (noOfLevels > 1) nPstn = oldString.LastIndexOf('\\', nPstn - 1);
            fileName = oldString.Substring(nPstn + 1);
            return globalVars.BaseDirectory + @"\" + fileName;
        }
    }
}
