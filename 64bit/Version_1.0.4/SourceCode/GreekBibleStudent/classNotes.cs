using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    class classNotes
    {
        /************************************************
          *                                              *
          *            Key Global references             *
          *            =====================             *
          *                                              *
          ************************************************/

        classGlobal globalVars;
        classText textHandler;
        classRegistry regClass;

        public void initialiseNotes(classGlobal inConfig, classText inHandler, classRegistry inReg)
        {
            globalVars = inConfig;
            textHandler = inHandler;
            regClass = inReg;
        }

        public void processOnStartup()
        {
            /*****************************************************************************
             *                                                                           *
             *                         processOnStartup                                  *
             *                         ================                                  *
             *                                                                           *
             *  This will be called when the application starts.  It will function as    *
             *    follows:                                                               *
             *                                                                           *
             *    1. It will check for the presence of the notes directory with the name *
             *       of the notes group currently being used (i.e. in the registry);     *
             *    3. If any stored notes are found:                                      *
             *       a) each note in turn will be applied to the relevant class store    *
             *          for the verse;                                                   *
             *       b) processing will then pass to the next note, so that each one is  *
             *          applied in turn;                                                 *
             *                                                                           *
             *****************************************************************************/
            int tabCode, bookId;
            String newNote, baseDirectory, notesPath, noteGroupName, fullPath;
            DirectoryInfo storeDir;
            String[] fileComponents;
            Char[] fileSplit = { '-' };
            DirectoryInfo[] listOfDirectories;
            FileInfo[] notesFiles;
            StreamReader srNote;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            SortedDictionary<int, classBookContent> listOfNTBooks;
            SortedDictionary<int, classBookContent> listOfLxxBooks;

            // Before we do anything else, we need to find out whether we have stored any notes
            listOfNTBooks = textHandler.ListOfNTBooks;
            listOfLxxBooks = textHandler.ListOfLxxBooks;
            baseDirectory = globalVars.BaseDirectory;
            notesPath = globalVars.NotesPath;
            noteGroupName = globalVars.NotesName;
            fullPath = baseDirectory + @"\" + notesPath + @"\" + noteGroupName;
            // BTW, document the notes tab with the Set name
            globalVars.NotesTabPage.Text = "Notes: " + noteGroupName;
            storeDir = new DirectoryInfo(fullPath);
            if (!storeDir.Exists) return; // No stored data; nothing to do
            listOfDirectories = storeDir.GetDirectories();
            foreach (DirectoryInfo subDir in listOfDirectories)
            {
                notesFiles = subDir.GetFiles();
                fileComponents = subDir.Name.Split(fileSplit);
                tabCode = Convert.ToInt32(fileComponents[0]);
                bookId = Convert.ToInt32(fileComponents[1]);
                foreach (FileInfo noteFile in notesFiles)
                {
                    if (tabCode == 0) listOfNTBooks.TryGetValue(bookId, out currentBook);
                    else listOfLxxBooks.TryGetValue(bookId, out currentBook);
                    newNote = noteFile.Name.Substring(0, noteFile.Name.Length - 5);
                    fileComponents = newNote.Split(fileSplit);
                    currentChapter = currentBook.getChapterBySeqNo(Convert.ToInt32(fileComponents[0]));
                    currentVerse = currentChapter.getVerseBySeqNo(Convert.ToInt32(fileComponents[1]));
                    srNote = new StreamReader(noteFile.FullName);
                    currentVerse.NoteText = srNote.ReadToEnd();
                    srNote.Close();
                    srNote.Dispose();
                }
            }
            // Update the verse currently active
            tabCode = globalVars.TabCtlText.SelectedIndex;
            bookId = globalVars.getComboBoxControlByIndex(3 * tabCode).SelectedIndex;
            if (tabCode == 0) listOfNTBooks.TryGetValue(bookId, out currentBook);
            else listOfLxxBooks.TryGetValue(bookId, out currentBook);
            currentChapter = currentBook.getChapterBySeqNo(globalVars.getComboBoxControlByIndex(3 * tabCode + 1).SelectedIndex);
            currentVerse = currentChapter.getVerseBySeqNo(globalVars.getComboBoxControlByIndex(3 * tabCode + 2).SelectedIndex);
            globalVars.getRichtextControlByIndex(6).Text = currentVerse.NoteText;
        }

        public void processOldNote(int tabCode)
        {
            /*****************************************************************************
             *                                                                           *
             *                         processOldNote                                    *
             *                         ==============                                    *
             *                                                                           *
             *  This will be called when the notes area has lost focus - i.e. when the   *
             *    user has clicked anywhere outside the notes area.  This will include   *
             *    the case where he or she selects a new verse.                          *
             *                                                                           *
             *  It will also be called when ending the application.                      *
             *                                                                           *
             *  It will perform two tasks:                                               *
             *    1  Save the discrete note the current folder                           *
             *    2  Update (or create) a copy of the note in the class instance for the *
             *       verse.                                                              *
             *                                                                           *
             *  The purpose of step 1 will be to provide a durable copy that will be     *
             *    available in the event of unplanned application exit, such as a loss   *
             *    of power.  These will constitute the saved version, which will be      *
             *    loaded on system startup and retained until changed by the user and    *
             *    application.                                                           *
             *                                                                           *
             *****************************************************************************/

            int bkVal, chVal, vsVal;
            String newNote, previousNote, baseDirectory, notesPath, noteGroupName, notesName, fullPath;
            DirectoryInfo storeDir;
            StreamWriter swNote;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            RichTextBox rtxtNotes;
            ComboBox cbBook, cbChapter, cbVerse;
            SortedDictionary<int, classBookContent> listOfBooks;

            // Make sure we can see the note
            rtxtNotes = globalVars.getRichtextControlByIndex(6);
            // And the current book, chapter and verse references
            if (tabCode == 0)
            {
                cbBook = globalVars.getComboBoxControlByIndex(0);
                cbChapter = globalVars.getComboBoxControlByIndex(1);
                cbVerse = globalVars.getComboBoxControlByIndex(2);
                listOfBooks = textHandler.ListOfNTBooks;
            }
            else
            {
                cbBook = globalVars.getComboBoxControlByIndex(3);
                cbChapter = globalVars.getComboBoxControlByIndex(4);
                cbVerse = globalVars.getComboBoxControlByIndex(5);
                listOfBooks = textHandler.ListOfLxxBooks;
            }
            // Get the class instance storing the verse data
            bkVal = cbBook.SelectedIndex;
            chVal = cbChapter.SelectedIndex;
            vsVal = cbVerse.SelectedIndex;
            listOfBooks.TryGetValue(bkVal, out currentBook);
            currentChapter = currentBook.getChapterBySeqNo(chVal);
            currentVerse = currentChapter.getVerseBySeqNo(vsVal);
            // Before we do anything else, we need to find out whether we have stored any temporary notes
            baseDirectory = globalVars.BaseDirectory;
            notesPath = globalVars.NotesPath;
            noteGroupName = globalVars.NotesName;
            fullPath = baseDirectory + @"\" + notesPath + @"\" + noteGroupName;
            storeDir = new DirectoryInfo(fullPath);
            if (!storeDir.Exists) storeDir.Create();
            fullPath = fullPath + @"\" + tabCode.ToString() + "-" + bkVal.ToString();
            storeDir = new DirectoryInfo(fullPath);
            if (!storeDir.Exists) storeDir.Create();
            notesName = chVal.ToString() + "-" + vsVal.ToString() + ".note";
            fullPath = fullPath + @"\" + notesName;
            // Whether we do anything depends on whether the new note is a change from the old
            previousNote = currentVerse.NoteText;
            newNote = rtxtNotes.Text;
            if ((previousNote == null) || (previousNote.Length == 0))
            {
                if ((newNote == null) || (newNote.Length == 0)) return;
            }
            else
            {
                if (String.Compare(previousNote, newNote) == 0) return;
            }
            swNote = new StreamWriter(fullPath);
            swNote.Write(newNote);
            swNote.Close();
            swNote.Dispose();
            currentVerse.NoteText = newNote;
        }

        public void processNewNote(int tabCode)
        {
            /*****************************************************************************
             *                                                                           *
             *                         processNewNote                                    *
             *                         ==============                                    *
             *                                                                           *
             *  This will be called when a new value for cbVeses is selected (or after   *
             *    cbBooks and/or cbChapters has changed.                                 *
             *                                                                           *
             *****************************************************************************/

            int bkVal, chVal, vsVal;
            String newNote, previousNote, bookName, chapter, verse;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;
            RichTextBox rtxtNotes;
            ComboBox cbBook, cbChapter, cbVerse;
            SortedDictionary<int, classBookContent> listOfBooks;

            // Make sure we can see the note
            rtxtNotes = globalVars.getRichtextControlByIndex(6);
            // And the current book, chapter and verse references
            if (tabCode == 0)
            {
                cbBook = globalVars.getComboBoxControlByIndex(0);
                cbChapter = globalVars.getComboBoxControlByIndex(1);
                cbVerse = globalVars.getComboBoxControlByIndex(2);
                listOfBooks = textHandler.ListOfNTBooks;
            }
            else
            {
                cbBook = globalVars.getComboBoxControlByIndex(3);
                cbChapter = globalVars.getComboBoxControlByIndex(4);
                cbVerse = globalVars.getComboBoxControlByIndex(5);
                listOfBooks = textHandler.ListOfLxxBooks;
            }
            // Get the class instance storing the verse data
            bkVal = cbBook.SelectedIndex;
            chVal = cbChapter.SelectedIndex;
            vsVal = cbVerse.SelectedIndex;
            if (chVal < 0) return;
            listOfBooks.TryGetValue(bkVal, out currentBook);
            currentChapter = currentBook.getChapterBySeqNo(chVal);
            currentVerse = currentChapter.getVerseBySeqNo(vsVal);
            bookName = currentBook.BookName;
            chapter = currentBook.getChapterIdBySeqNo(chVal);
            verse = currentChapter.getVerseIdBySeqNo(vsVal);
            globalVars.NotesTabPage.Text = "Notes: " + globalVars.NotesName + " - " + bookName + " " + chapter + ":" + verse;
            // Whether we do anything depends on whether the new note is a change from the old
            previousNote = currentVerse.NoteText;
            newNote = rtxtNotes.Text;
            if ((previousNote == null) || (previousNote.Length == 0))
            {
                if ((newNote == null) || (newNote.Length == 0)) return;
            }
            else
            {
                if (String.Compare(previousNote, newNote) == 0) return;
            }
            rtxtNotes.Text = currentVerse.NoteText;
        }

        public void moveNotesAndOthers(FolderBrowserDialog dlgNotes)
        {
            /**********************************************************************************************
             *                                                                                            *
             *                                  moveNotesAndOthers                                        *
             *                                  ==================                                        *
             *                                                                                            *
             *  The aim of this method is to move the current location of all transient files (i.e. Help, *
             *    Notes and Source files) to a new location.                                              *
             *                                                                                            *
             *  The location of the entire fileset to be moved is found in globalVars.baseDirectory.      *
             *  The target folder name will be GBSFiles.  Since this is different from the initial        *
             *    directory name, the base folder (initially "Files") will need to be extracted.          *
             *                                                                                            *
             **********************************************************************************************/
            int idx;
            String startingPath, targetFolder = "GBSFiles", endingPath, subDirectory;
            DirectoryInfo diTarget;
            String[] subDirectoryName = { "Source", "Notes", "Help" };

            startingPath = globalVars.BaseDirectory;
            if (startingPath[startingPath.Length - 1] == '\\') startingPath = startingPath.Substring(0, startingPath.Length - 1);
            dlgNotes.SelectedPath = startingPath;
            dlgNotes.Description = "Location of Folder containing Source files, Notes and Help\nThe folders, Source, Notes and Help and the " +
                "parent folder, GBSFiles, will be created *below* the folder that you select or create.";
            if (dlgNotes.ShowDialog() == DialogResult.OK)
            {
                regClass.openRegistry();
                endingPath = dlgNotes.SelectedPath;
                if (String.Compare(endingPath, startingPath) == 0) return;  // i.e. no change
                subDirectory = endingPath + @"\" + targetFolder;
                diTarget = new DirectoryInfo(subDirectory);
                if (!diTarget.Exists) diTarget.Create();
                endingPath = subDirectory;
                for (idx = 0; idx <= 2; idx++)
                {
                    createAndPopulateFolder(startingPath + @"\" + subDirectoryName[idx], endingPath + @"\" + subDirectoryName[idx], subDirectoryName[idx],
                                                idx);
                }
                regClass.closeKeys();
            }
        }

        private void createAndPopulateFolder(String fromDirectory, String toDirectory, String folderName, int actionCode)
        {
            String fileName, fullFileName, finalFileName;
            DirectoryInfo diTarget, diSourceContents, diTemp;
            FileInfo[] directoryContents, sourceContents, notesBottomLevelFiles;
            DirectoryInfo[] notesSubDirectories, notesItems;

            diTarget = new DirectoryInfo(toDirectory);
            if (diTarget.Exists)
            {
                if (MessageBox.Show("The folder " + toDirectory + "already exist\nIf you continue, any data currently in this folder will be erased\n\n" +
                    "Select \"OK\" to continue.", folderName + " Folder", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    directoryContents = diTarget.GetFiles();
                    foreach (FileInfo fiSingleFile in directoryContents)
                    {
                        fiSingleFile.Delete();
                    }
                }
                else return;
            }
            else
            {
                diTarget.Create();
            }
            diSourceContents = new DirectoryInfo(fromDirectory);
            sourceContents = diSourceContents.GetFiles();
            fileName = "";
            foreach (FileInfo fiSourceFile in sourceContents)
            {
                fileName = toDirectory + @"\" + fiSourceFile.Name;
                fiSourceFile.CopyTo(fileName);
            }
            if (actionCode == 1) // i.e. Notes
            {
                notesSubDirectories = diSourceContents.GetDirectories();
                foreach (DirectoryInfo diSubSource in notesSubDirectories)
                {
                    fileName = toDirectory + @"\" + diSubSource.Name;
                    diTemp = new DirectoryInfo(fileName);
                    if (!diTemp.Exists) diTemp.Create();
                    notesItems = diSubSource.GetDirectories();
                    foreach (DirectoryInfo fiFinalDir in notesItems)
                    {
                        fullFileName = fileName + @"\" + fiFinalDir.Name;
                        diTemp = new DirectoryInfo(fullFileName);
                        if (!diTemp.Exists) diTemp.Create();
                        notesBottomLevelFiles = fiFinalDir.GetFiles();
                        foreach (FileInfo fiFinalFile in notesBottomLevelFiles)
                        {
                            finalFileName = fiFinalFile.Name;
                            finalFileName = fullFileName + @"\" + fiFinalFile.Name;
                            fiFinalFile.CopyTo(finalFileName);
                        }
                    }
                }
            }
            if (regClass.updateFilePath(toDirectory, actionCode) == -1)
            {
                MessageBox.Show("Please contact the system provider", "Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                diSourceContents.Delete(true);
            }
        }
    }
}
