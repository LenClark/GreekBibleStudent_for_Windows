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
        bool isInitialising = false;
        String keyString, cautionFileName = "deferredAction.txt";
        RegistryKey baseKey;
        Form mainForm;

        classGlobal globalVars;

        public Form MainForm { get => mainForm; set => mainForm = value; }
        public string KeyString { get => keyString; set => keyString = value; }

        public void initialiseRegistry(classGlobal inConfig)
        {
            globalVars = inConfig;
        }

        public void initialiseRegistry()
        {
            openRegistry();
            initialisePaths();
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
            /*************************************************************************************
             *                                                                                   *
             *   Manage File location details                                                    *
             *   ----------------------------                                                    *
             *                                                                                   *
             * Note: The actual directory names (Notes, Source) are "hard coded"                 *
             *                                                                                   *
             *************************************************************************************/

            String registryString, fileBuffer;
            String baseDirectory, sourceFolder, ntTitlesFile, lxxTitlesFile, ntTextFolder, lxxTextFolder, helpPath, notesPath, notesName, gkGroupsName;
            String[] keyNames = { "Base Directory", "Source Folder", "NT Titles", "LXX Titles", "NT Folder", "LXX Folder", "Help Folder", "Notes Folder",
                                   "Notes Name", "Greek Groups" };
            DirectoryInfo diTarget, diSource;
            StreamReader srCaution;

            isInitialising = true;
            // Now get file location details from the registry - and create if they don't exist
            if (baseKey == null)
            {
                if (Directory.Exists(globalVars.AltBaseDirectory))
                {
                    if (!Directory.Exists(globalVars.BaseDirectory)) Directory.CreateDirectory(globalVars.BaseDirectory);
                    // Make sure the target directory is empty
                    diTarget = new DirectoryInfo(globalVars.BaseDirectory);
                    foreach (DirectoryInfo diName in diTarget.GetDirectories()) diName.Delete(true);
                    foreach (FileInfo fiFile in diTarget.GetFiles()) fiFile.Delete();
                    // Now copy data from the alternative location
                    diSource = new DirectoryInfo(globalVars.AltBaseDirectory);
                    foreach (DirectoryInfo diName in diSource.GetDirectories()) cloneSingleDirectory(diTarget, diName);
                    // Now get any files in the current directoy
                    foreach (FileInfo fiFile in diSource.GetFiles()) fiFile.CopyTo(diTarget.FullName + @"\" + fiFile.Name);
                }
                baseDirectory = globalVars.BaseDirectory;
                sourceFolder = globalVars.SourceFolder;
                ntTitlesFile = globalVars.NtTitlesFile;
                lxxTitlesFile = globalVars.LxxTitlesFile;
                ntTextFolder = globalVars.NtTextFolder;
                lxxTextFolder = globalVars.LxxTextFolder;
                helpPath = globalVars.HelpPath;
                notesPath = globalVars.NotesPath;
                notesName = globalVars.NotesName;
                gkGroupsName = globalVars.GkGroupsName;
                // Go here if the base registry key doesn't exist
                baseKey = Registry.CurrentUser.CreateSubKey(keyString);
                baseKey.SetValue(keyNames[0], baseDirectory, RegistryValueKind.String);
                baseKey.SetValue(keyNames[1], sourceFolder, RegistryValueKind.String);
                baseKey.SetValue(keyNames[2], ntTitlesFile, RegistryValueKind.String);
                baseKey.SetValue(keyNames[3], lxxTitlesFile, RegistryValueKind.String);
                baseKey.SetValue(keyNames[4], ntTextFolder, RegistryValueKind.String);
                baseKey.SetValue(keyNames[5], lxxTextFolder, RegistryValueKind.String);
                baseKey.SetValue(keyNames[6], helpPath, RegistryValueKind.String);
                baseKey.SetValue(keyNames[7], notesPath, RegistryValueKind.String);
                baseKey.SetValue(keyNames[8], notesName, RegistryValueKind.String);
                baseKey.SetValue(keyNames[9], gkGroupsName, RegistryValueKind.String);
            }
            else
            {
                registryString = registrySetting(keyNames[0], globalVars.BaseDirectory);
                if (registryString.Length > 0) globalVars.BaseDirectory = registryString;
                registryString = registrySetting(keyNames[1], globalVars.SourceFolder);
                if (registryString.Length > 0) globalVars.SourceFolder = registryString;
                registryString = registrySetting(keyNames[2], globalVars.NtTitlesFile);
                if (registryString.Length > 0) globalVars.NtTitlesFile = registryString;
                registryString = registrySetting(keyNames[3], globalVars.LxxTitlesFile);
                if (registryString.Length > 0) globalVars.LxxTitlesFile = registryString;
                registryString = registrySetting(keyNames[4], globalVars.NtTextFolder);
                if (registryString.Length > 0) globalVars.NtTextFolder = registryString;
                registryString = registrySetting(keyNames[5], globalVars.LxxTextFolder);
                if (registryString.Length > 0) globalVars.LxxTextFolder = registryString;
                registryString = registrySetting(keyNames[6], globalVars.HelpPath);
                if (registryString.Length > 0) globalVars.HelpPath = registryString;
                registryString = registrySetting(keyNames[7], globalVars.NotesPath);
                if (registryString.Length > 0) globalVars.NotesPath = registryString;
                registryString = registrySetting(keyNames[8], globalVars.NotesName);
                if (registryString.Length > 0) globalVars.NotesName = registryString;
                registryString = registrySetting(keyNames[9], globalVars.GkGroupsName);
                if (registryString.Length > 0) globalVars.GkGroupsName = registryString;
            }
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
            if( Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LFCConsulting\GreekBibleStudent"))
            {
                /*-----------------------------------------------------------------------------------------*
                 *                                                                                         *
                 *  ... and this is even less elegant.                                                     *
                 *                                                                                         *
                 *  In the *one* case of the initial location, the base folder is actually                 *
                 *            GreekBibleStudent                                                            *
                 *  so we hunt for that explicitly - but this ensures we *don't* delete it when it is      *
                 *  actually functioning.                                                                  *
                 *                                                                                         *
                 *-----------------------------------------------------------------------------------------*/

                try
                {
                    // Note that this has been deliberately left without the 2nd parameter set to true.
                    // If the folder, GreekBibleStudent, contains anything, this command will fail
                    Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\LFCConsulting\GreekBibleStudent");
                }
                catch
                {
                    // do nothing - just don't report an error
                }
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
            /*************************************************************************************
             *                                                                                   *
             * Step 2: Some window parameters                                                    *
             * ------------------------------                                                    *
             *                                                                                   *
             *************************************************************************************/

                int positionValue;
            Object regValue;
            SplitContainer splitMain;
            Screen mainScreen = Screen.PrimaryScreen;

            isInitialising = true;
            regValue = baseKey.GetValue("Window Position X");
            if (regValue == null)
            {
                positionValue = (mainScreen.WorkingArea.Width - mainForm.Width) / 2;
                baseKey.SetValue("Window Position X", positionValue, RegistryValueKind.DWord);
                mainForm.Left = positionValue;
            }
            else
            {
                mainForm.Left = (int)regValue;
            }
            globalVars.WindowX = mainForm.Left;
            regValue = baseKey.GetValue("Window Position Y");
            if (regValue == null)
            {
                positionValue = (mainScreen.WorkingArea.Height - mainForm.Height) / 2;
                baseKey.SetValue("Window Position Y", positionValue, RegistryValueKind.DWord);
                mainForm.Top = positionValue;
            }
            else
            {
                mainForm.Top = (int)regValue;
            }
            globalVars.WindowY = mainForm.Top;
            regValue = baseKey.GetValue("Window Width");
            if (regValue == null)
            {
                baseKey.SetValue("Window Width", mainForm.Width, RegistryValueKind.DWord);
            }
            else
            {
                mainForm.Width = (int)regValue;
            }
            globalVars.WindowWidth = mainForm.Width;
            regValue = baseKey.GetValue("Window Height");
            if (regValue == null)
            {
                baseKey.SetValue("Window Height", mainForm.Height, RegistryValueKind.DWord);
            }
            else
            {
                mainForm.Height = (int)regValue;
            }
            globalVars.WindowHeight = mainForm.Height;
            regValue = baseKey.GetValue("Main Splitter Distance");
            splitMain = globalVars.SplitMain;
            if (regValue == null)
            {
                splitMain.SplitterDistance = splitMain.Width / 2;
                baseKey.SetValue("Main Splitter Distance", splitMain.SplitterDistance, RegistryValueKind.DWord);
            }
            else
            {
                splitMain.SplitterDistance = (int)regValue;
            }
            regValue = baseKey.GetValue("Maximum History Entries");
            if (regValue == null)
            {
                baseKey.SetValue("Maximum History Entries", globalVars.HistoryMax, RegistryValueKind.DWord);
            }
            else
            {
                globalVars.HistoryMax = (int)regValue;
            }
            isInitialising = false;
        }

        public void initialiseFontsAndColour()
        {
            /*************************************************************************************
             *                                                                                   *
             *   Initialisation of Font colours and sizes and rich text area background colours  *
             *                                                                                   *
             *************************************************************************************/

            int idx, noOfAreas, regIdx;
            Object regValue;
            String[] registryKeyValue;
            Color[] foreColorRec;
            Color[] backColorRec;
            Font[] fontForArea;
            FontFamily currentFontFamily;

            isInitialising = true;
            registryKeyValue = globalVars.RegistryKeyValue;
            regIdx = 0;
            noOfAreas = globalVars.NoOfRichtextBoxes;
            foreColorRec = new Color[noOfAreas];
            backColorRec = new Color[noOfAreas];
            fontForArea = new Font[noOfAreas];
            for (idx = 0; idx < noOfAreas; idx++)
            {
                regValue = baseKey.GetValue(registryKeyValue[regIdx]);
                if (regValue == null)
                {
                    foreColorRec[idx] = globalVars.getRichtextControlByIndex(idx).ForeColor;
                    baseKey.SetValue(registryKeyValue[regIdx], foreColorRec[idx].ToArgb().ToString(), RegistryValueKind.String);
                }
                else
                {
                    foreColorRec[idx] = Color.FromArgb(Convert.ToInt32(regValue.ToString()));
                    globalVars.getRichtextControlByIndex(idx).ForeColor = foreColorRec[idx];
                }
                regIdx++;
                regValue = baseKey.GetValue(registryKeyValue[regIdx]);
                if (regValue == null)
                {
                    backColorRec[idx] = globalVars.getRichtextControlByIndex(idx).BackColor;
                    baseKey.SetValue(registryKeyValue[regIdx], backColorRec[idx].ToArgb().ToString(), RegistryValueKind.String);
                }
                else
                {
                    backColorRec[idx] = Color.FromArgb(Convert.ToInt32(regValue.ToString()));
                    globalVars.getRichtextControlByIndex(idx).BackColor = backColorRec[idx];
                }
                regIdx++;
                regValue = baseKey.GetValue(registryKeyValue[regIdx]);
                if (regValue == null)
                {
                    fontForArea[idx] = globalVars.getRichtextControlByIndex(idx).Font;
                    baseKey.SetValue(registryKeyValue[regIdx], fontForArea[idx].Size.ToString(), RegistryValueKind.String);
                }
                else
                {
                    currentFontFamily = globalVars.getRichtextControlByIndex(idx).Font.FontFamily;
                    fontForArea[idx] = new Font(currentFontFamily, float.Parse(regValue.ToString()));
                    globalVars.getRichtextControlByIndex(idx).Font = fontForArea[idx];
                }
                regIdx++;
            }
            globalVars.FontForArea = fontForArea;
            globalVars.ForeColorRec = foreColorRec;
            globalVars.BackColorRec = backColorRec;
            // Now deal with the "one off" primary and secondary colours
            regValue = baseKey.GetValue(globalVars.PrimaryKeyValue);
            if (regValue == null)
            {
                globalVars.PrimaryColour = Color.Red;
                baseKey.SetValue(globalVars.PrimaryKeyValue, globalVars.PrimaryColour.ToArgb().ToString(), RegistryValueKind.String);
            }
            else globalVars.PrimaryColour = Color.FromArgb(Convert.ToInt32(regValue.ToString()));
            regValue = baseKey.GetValue(globalVars.SecondaryKeyValue);
            if (regValue == null)
            {
                globalVars.SecondaryColour = Color.Blue;
                baseKey.SetValue(globalVars.SecondaryKeyValue, globalVars.SecondaryColour.ToArgb().ToString(), RegistryValueKind.String);
            }
            else globalVars.SecondaryColour = Color.FromArgb(Convert.ToInt32(regValue.ToString()));
            isInitialising = false;
        }

        private void modifyMenuSelection()
        {
            int tagValue;
            ToolStripMenuItem parentItem;

            parentItem = globalVars.ParentMenuItem;
            foreach (ToolStripMenuItem childItem in parentItem.DropDownItems)
            {
                tagValue = Convert.ToInt32(childItem.Tag);
                if (tagValue == globalVars.DisplayTextCode) childItem.Checked = true;
                else childItem.Checked = false;
            }
        }

        public void updateFontsAndColour()
        {
            int idx, noOfAreas, regIdx;
            String[] registryKeyValue;
            Color[] foreColorRec = globalVars.ForeColorRec;
            Color[] backColorRec = globalVars.BackColorRec;
            Font[] fontForArea = globalVars.FontForArea;

            registryKeyValue = globalVars.RegistryKeyValue;
            regIdx = 0;
            noOfAreas = globalVars.NoOfRichtextBoxes;
            for (idx = 0; idx < noOfAreas; idx++)
            {
                baseKey.SetValue(registryKeyValue[regIdx], foreColorRec[idx].ToArgb().ToString(), RegistryValueKind.String);
                regIdx++;
                baseKey.SetValue(registryKeyValue[regIdx], backColorRec[idx].ToArgb().ToString(), RegistryValueKind.String);
                regIdx++;
                baseKey.SetValue(registryKeyValue[regIdx], fontForArea[idx].Size.ToString(), RegistryValueKind.String);
                regIdx++;
            }
            baseKey.SetValue(globalVars.PrimaryKeyValue, globalVars.PrimaryColour.ToArgb().ToString(), RegistryValueKind.String);
            baseKey.SetValue(globalVars.SecondaryKeyValue, globalVars.SecondaryColour.ToArgb().ToString(), RegistryValueKind.String);
        }

        public int updateFilePath(String pathName, int pathCode)
        {
            /********************************************************************
             *                                                                  *
             *                     updateFilePath                               *
             *                     ==============                               *
             *                                                                  *
             *  Method for updating the registry when paths are moved.          *
             *  The pathCode indicates which path is being loaded, as follows:  *
             *    0  =  Source path                                             *
             *    1  =  Notes path                                              *
             *    2  =  Help path                                               *
             *                                                                  *
             ********************************************************************/

            /*            String sourcePath;
                        String[] regName = { "Source Path", "Notes Path", "Help Path" };
                        Object regValue;

                        // Now get file location details from the registry - and create if they don't exist
                        if (baseKey == null) return -1;
                        regValue = baseKey.GetValue(regName[pathCode]);
                        baseKey.SetValue(regName[pathCode], pathName + @"\", RegistryValueKind.String);
                        globalVars.SourcePath = pathName + @"\"; */
            return 0;
        }

        public void updateSplitterDistance()
        {
            int newSplitterDistance;
            SplitContainer targetSplitter;

            if (isInitialising) return;
            targetSplitter = globalVars.SplitMain;
            newSplitterDistance = targetSplitter.SplitterDistance;
            openRegistry();
            baseKey.SetValue("Main Splitter Distance", newSplitterDistance, RegistryValueKind.DWord);
            closeKeys();
            globalVars.SplitPstn = newSplitterDistance;
        }

        public void updateWindowPosition()
        {
            int windowX, windowY;
            Form targetForm;

            if (isInitialising) return;
            targetForm = globalVars.MasterForm;
            windowX = targetForm.Left;
            windowY = targetForm.Top;
            openRegistry();
            baseKey.SetValue("Window Position X", windowX, RegistryValueKind.DWord);
            baseKey.SetValue("Window Position Y", windowY, RegistryValueKind.DWord);
            closeKeys();
            globalVars.WindowX = windowX;
            globalVars.WindowY = windowY;
        }

        public void updateWindowSize()
        {
            int windowWidth, windowHeight;
            Form targetForm;

            if (isInitialising) return;
            targetForm = globalVars.MasterForm;
            windowWidth = targetForm.Width;
            windowHeight = targetForm.Height;
            openRegistry();
            baseKey.SetValue("Window Width", windowWidth, RegistryValueKind.DWord);
            baseKey.SetValue("Window Height", windowHeight, RegistryValueKind.DWord);
            closeKeys();
            globalVars.WindowWidth = windowWidth;
            globalVars.WindowHeight = windowHeight;
        }

        public void updateNotesSet()
        {
            if (isInitialising) return;
            openRegistry();
            if (baseKey == null) return;
            baseKey.SetValue("Notes Name", globalVars.NotesName, RegistryValueKind.String);
            closeKeys();
        }

        public void cloneSingleDirectory( DirectoryInfo diTargetDirectory, DirectoryInfo diSourceDirectory)
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
            if( baseKey != null ) baseKey.SetValue("Base Directory", newLocation, RegistryValueKind.String);
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
    }
}
