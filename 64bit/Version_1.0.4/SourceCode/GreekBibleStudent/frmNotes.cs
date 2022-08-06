using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GreekBibleStudent
{
    public partial class frmNotes : Form
    {
        int actionCode;
        classGlobal globalVars;
        classText textHandler;
        TextBox txtNote;
        ComboBox cbNote;

        public frmNotes()
        {
            InitializeComponent();
        }

        public void initialiseNotesDialog(classGlobal inConfig, classText handlerClass, int inAction)
        {
            /**********************************************************************
             *                                                                    *
             *                            frmNotes                                *
             *                            ========                                *
             *                                                                    *
             *  Essentially, handles the four menu options from the main File     *
             *    menu.  The only parameter that needs explanation is inAction:   *
             *                                                                    *
             *   Code                      Meaning                                *
             *     1      Create a new note group                                 *
             *     2      Change to a different (existing) note group             *
             *     3      Delete an existing note group                           *
             *     4      Simply display the note group currently in use          *
             *                                                                    *
             **********************************************************************/
            const int okXPstn = 306;

            int idx;
            String fileLocation, currentNoteName, field4, field5;
            DirectoryInfo notesDirectory;
            DirectoryInfo[] dirList;
            int[] lblLeft, lblTop;
            String[] dialogText, field1, field2, field3;
            Label[] lblInfo;

            globalVars = inConfig;
            textHandler = handlerClass;
            dialogText = new String[4] { "Create a New Note Set", "Select a different, existing Note Set", "Delete an existing Note Set (remove it completely)", "Displaying the current Note Set Name" };
            field1 = new String[4] { "Provide a name for the new Notes Set:", "Select an Existing Notes Set you want to use:", "Select an Existing Notes Set you want to delete:", "" };
            field2 = new String[4] { "(Note: avoid spaces and symbols.  This name will be used as part of a file name.)", "", "(Note that this will remove all notes contained in the Note Set and the action", "" };
            field3 = new String[4] { "", "", "cannot be reversed.)", "" };
            lblLeft = new int[5] { 15, 15, 58, 15, 175 };
            lblTop = new int[5] { 18, 38, 58, 78, 78 };
            field4 = "The current Notes Set name is:";
            actionCode = inAction;
            currentNoteName = globalVars.NotesName;
            field5 = currentNoteName;
            lblInfo = new Label[5];
            for (idx = 0; idx < 5; idx++) lblInfo[idx] = new Label();
            fileLocation = globalVars.BaseDirectory + @"\" + globalVars.NotesPath;
            notesDirectory = new DirectoryInfo(fileLocation);
            dirList = notesDirectory.GetDirectories();
            for (idx = 0; idx < 5; idx++)
            {
                lblInfo[idx].Left = lblLeft[idx];
                lblInfo[idx].Top = lblTop[idx];
                lblInfo[idx].AutoSize = true;
            }
            lblInfo[3].Text = field4;
            lblInfo[4].Text = field5;
            lblInfo[4].ForeColor = Color.Red;
            btnOK.Left = okXPstn;
            btnCancel.Visible = true;
            switch (inAction)
            {
                case 1:
                    lblInfo[0].Text = field1[0];
                    lblInfo[1].Text = field2[0];
                    lblInfo[2].Text = field3[0];
                    this.Controls.Add(lblInfo[0]);
                    this.Controls.Add(lblInfo[1]);
                    this.Controls.Add(lblInfo[3]);
                    this.Controls.Add(lblInfo[4]);
                    txtNote = new TextBox();
                    txtNote.Left = 211;
                    txtNote.Top = 15;
                    txtNote.Width = 176;
                    txtNote.Height = 20;
                    this.Controls.Add(txtNote);
                    this.Text = "New Note Set";
                    break;
                case 2:
                    lblInfo[0].Text = field1[1];
                    lblInfo[1].Text = field2[1];
                    lblInfo[2].Text = field3[1];
                    this.Controls.Add(lblInfo[0]);
                    this.Controls.Add(lblInfo[3]);
                    this.Controls.Add(lblInfo[4]);
                    cbNote = new ComboBox();
                    cbNote.Left = 239;
                    cbNote.Top = 15;
                    cbNote.Width = 162;
                    cbNote.Height = 21;
                    this.Controls.Add(cbNote);
                    foreach (DirectoryInfo notesSet in dirList)
                    {
                        if (String.Compare(notesSet.Name, currentNoteName) == 0) continue;
                        cbNote.Items.Add(notesSet.Name);
                    }
                    if (cbNote.Items.Count > 0) cbNote.SelectedIndex = 0;
                    this.Text = "Change Current Note Set";
                    break;
                case 3:
                    lblInfo[0].Text = field1[2];
                    lblInfo[1].Text = field2[2];
                    lblInfo[2].Text = field3[2];
                    this.Controls.Add(lblInfo[0]);
                    this.Controls.Add(lblInfo[1]);
                    this.Controls.Add(lblInfo[2]);
                    this.Controls.Add(lblInfo[3]);
                    this.Controls.Add(lblInfo[4]);
                    cbNote = new ComboBox();
                    cbNote.Left = 250;
                    cbNote.Top = 15;
                    cbNote.Width = 162;
                    cbNote.Height = 21;
                    this.Controls.Add(cbNote);
                    foreach (DirectoryInfo notesSet in dirList)
                    {
                        if (String.Compare(notesSet.Name, currentNoteName) == 0) continue;
                        cbNote.Items.Add(notesSet.Name);
                    }
                    if (cbNote.Items.Count > 0) cbNote.SelectedIndex = 0;
                    this.Text = "Delete an Existing Note Set";
                    break;
                case 4:
                    lblInfo[3].Top = 42;
                    lblInfo[4].Left = 246;
                    lblInfo[4].Top = 42;
                    lblInfo[3].Font = new Font(lblInfo[0].Font.FontFamily, 12);
                    lblInfo[4].Font = new Font(lblInfo[0].Font.FontFamily, 12);
                    this.Controls.Add(lblInfo[3]);
                    this.Controls.Add(lblInfo[4]);
                    btnOK.Left = (this.Width - btnOK.Width) / 2;
                    btnCancel.Visible = false;
                    this.Text = "Current Note Set";
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int bookId, chapId, VerseId;
            String currentNoteName, newNoteName, notePath, fullPath, chapterAndVerse;
            String[] chapterVerseBreakdown;
            Char[] paramSplit = { '-' };
            StreamReader srNote;
            FileInfo[] listOfFiles;
            DirectoryInfo newNoteDir;
            DirectoryInfo[] listOfBookDirs;
            SortedDictionary<int, classBookContent> listOfBooks;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;

            currentNoteName = globalVars.NotesName;
            switch (actionCode)
            {
                case 1:  // Create a new note group
                    newNoteName = txtNote.Text;
                    notePath = globalVars.NotesPath;
                    fullPath = globalVars.BaseDirectory + @"\" + notePath + @"\" + newNoteName;
                    removeAllNotes();
                    globalVars.NotesName = newNoteName;
                    globalVars.NotesTabPage.Text = "Notes: " + newNoteName;
                    newNoteDir = new DirectoryInfo(fullPath);
                    if (!newNoteDir.Exists) newNoteDir.Create();
                    break;
                case 2:   // Change to a different (existing) note group
                    newNoteName = cbNote.SelectedItem.ToString();
                    notePath = globalVars.NotesPath;
                    fullPath = globalVars.BaseDirectory + @"\" + notePath + @"\" + newNoteName;
                    removeAllNotes();
                    newNoteDir = new DirectoryInfo(fullPath);
                    if (!newNoteDir.Exists) newNoteDir.Create();
                    listOfBookDirs = newNoteDir.GetDirectories();
                    listOfBooks = textHandler.ListOfNTBooks;
                    foreach (DirectoryInfo specificBookDir in listOfBookDirs)
                    {
                        chapterAndVerse = specificBookDir.Name;
                        chapterVerseBreakdown = chapterAndVerse.Split(paramSplit);
                        bookId = Convert.ToInt32(chapterVerseBreakdown[1]);
                        listOfFiles = specificBookDir.GetFiles("*.note");
                        foreach (FileInfo noteFile in listOfFiles)
                        {
                            chapterAndVerse = noteFile.Name.Substring(0, noteFile.Name.Length - 5);
                            chapterVerseBreakdown = chapterAndVerse.Split(paramSplit);
                            chapId = Convert.ToInt32(chapterVerseBreakdown[0]);
                            VerseId = Convert.ToInt32(chapterVerseBreakdown[1]);
                            listOfBooks.TryGetValue(bookId, out currentBook);
                            currentChapter = currentBook.getChapterBySeqNo(chapId);
                            currentVerse = currentChapter.getVerseBySeqNo(VerseId);
                            srNote = new StreamReader(noteFile.FullName);
                            currentVerse.NoteText = srNote.ReadToEnd();
                            srNote.Close();
                            srNote.Dispose();
                        }
                    }
                    listOfBooks = textHandler.ListOfLxxBooks;
                    foreach (DirectoryInfo specificBookDir in listOfBookDirs)
                    {
                        chapterAndVerse = specificBookDir.Name;
                        chapterVerseBreakdown = chapterAndVerse.Split(paramSplit);
                        bookId = Convert.ToInt32(chapterVerseBreakdown[1]);
                        listOfFiles = specificBookDir.GetFiles("*.note");
                        foreach (FileInfo noteFile in listOfFiles)
                        {
                            chapterAndVerse = noteFile.Name.Substring(0, noteFile.Name.Length - 5);
                            chapterVerseBreakdown = chapterAndVerse.Split(paramSplit);
                            chapId = Convert.ToInt32(chapterVerseBreakdown[0]);
                            VerseId = Convert.ToInt32(chapterVerseBreakdown[1]);
                            listOfBooks.TryGetValue(bookId, out currentBook);
                            currentChapter = currentBook.getChapterBySeqNo(chapId);
                            currentVerse = currentChapter.getVerseBySeqNo(VerseId);
                            srNote = new StreamReader(noteFile.FullName);
                            currentVerse.NoteText = srNote.ReadToEnd();
                            srNote.Close();
                            srNote.Dispose();
                        }
                    }
                    globalVars.NotesName = newNoteName;
                    globalVars.NotesTabPage.Text = "Notes: " + newNoteName;
                    break;
                case 3:  // Delete a note set
                    newNoteName = cbNote.SelectedItem.ToString();
                    notePath = globalVars.NotesPath;
                    fullPath = globalVars.BaseDirectory + @"\" + notePath + @"\" + newNoteName;
                    newNoteDir = new DirectoryInfo(fullPath);
                    if (newNoteDir.Exists) newNoteDir.Delete(true);
                    break;
                case 4:
                    break;
            }
            Close();
        }

        private void removeAllNotes()
        {
            int bIdx, cIdx, vIdx, noOfBooks, noOfChapters, noOfVerses;
            SortedDictionary<int, classBookContent> listOfBooks;
            classBookContent currentBook;
            classChapterContent currentChapter;
            classVerseContent currentVerse;

            listOfBooks = textHandler.ListOfNTBooks;
            noOfBooks = listOfBooks.Count;
            for (bIdx = 0; bIdx < noOfBooks; bIdx++)
            {
                listOfBooks.TryGetValue(bIdx, out currentBook);
                noOfChapters = currentBook.NoOfChaptersInBook;
                for (cIdx = 0; cIdx < noOfChapters; cIdx++)
                {
                    currentChapter = currentBook.getChapterBySeqNo(cIdx);
                    noOfVerses = currentChapter.NoOfVersesInChapter;
                    for (vIdx = 0; vIdx < noOfVerses; vIdx++)
                    {
                        currentVerse = currentChapter.getVerseBySeqNo(vIdx);
                        if ((currentVerse.NoteText != null) && (currentVerse.NoteText.Length > 0)) currentVerse.NoteText = "";
                    }
                }
            }
            listOfBooks = textHandler.ListOfLxxBooks;
            noOfBooks = listOfBooks.Count;
            for (bIdx = 0; bIdx < noOfBooks; bIdx++)
            {
                listOfBooks.TryGetValue(bIdx, out currentBook);
                noOfChapters = currentBook.NoOfChaptersInBook;
                for (cIdx = 0; cIdx < noOfChapters; cIdx++)
                {
                    currentChapter = currentBook.getChapterBySeqNo(cIdx);
                    noOfVerses = currentChapter.NoOfVersesInChapter;
                    for (vIdx = 0; vIdx < noOfVerses; vIdx++)
                    {
                        currentVerse = currentChapter.getVerseBySeqNo(vIdx);
                        if ((currentVerse.NoteText != null) && (currentVerse.NoteText.Length > 0)) currentVerse.NoteText = "";
                    }
                }
            }
            globalVars.getRichtextControlByIndex(6).Text = "";
        }
    }
}
