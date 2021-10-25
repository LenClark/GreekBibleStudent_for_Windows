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
        int noOfAreas;

        classGlobal globalVars;
        classRegistry appRegistry;
        ComboBox[] cbFontSizes;
        TextBox txtPrimary, txtSecondary;
        TextBox[] txtText, txtBg;
        RichTextBox[] rtxtExample;
        TabPage[] tabCollection;
        Font largerFont;
        Font[] textFont;
        Color primarySearchColour;
        Color secondarySearchColour;
        Color[] fgColour, bgColour;
        String[] exampleMessage = { "22: Ὁ δὲ καρπὸς τοῦ πνεύματός ἐστιν ἀγάπη, χαρά, εἰρήνη, μακροθυμία, χρηστότης, ἀγαθωσύνη, πίστις,\n" +
                                    "23: πραΰτης, ἐγκράτεια· κατὰ τῶν τοιούτων οὐκ ἔστιν νόμος.\n" +
                                    "24: οἱ δὲ τοῦ Χριστοῦ τὴν σάρκα ἐσταύρωσαν σὺν τοῖς παθήμασιν καὶ ταῖς ἐπιθυμίαις.\n" +
                                    "25: εἰ ζῶμεν πνεύματι, πνεύματι καὶ στοιχῶμεν.\n" +
                                    "26: μὴ γινώμεθα κενόδοξοι, ἀλλήλους προκαλούμενοι, ἀλλήλοις φθονοῦντες.",
                                    "6: καὶ πάντες οἱ κυκλόθεν ἐνίσχυσαν ἐν χερσὶν αὐτῶν ἐν σκεύεσιν ἀργυρίου, ἐν χρυσῷ, " +
                                    "ἐν ἀποσκευῇ καὶ ἐν κτήνεσιν καὶ ἐν ξενίοις πάρεξ τῶν ἐν ἑκουσίοις.\n" +
                                    "7: καὶ ὁ βασιλεὺς Κῦρος ἐξήνεγκεν τὰ σκεύη οἴκου κυρίου, ἃ ἔλαβεν Ναβουχοδονοσορ ἀπὸ " +
                                    "Ιερουσαλημ καὶ ἔδωκεν αὐτὰ ἐν οἴκῳ θεοῦ αὐτοῦ,\n" +
                                    "8: καὶ ἐξήνεγκεν αὐτὰ Κῦρος βασιλεὺς Περσῶν ἐπὶ χεῖρα Μιθραδάτου γασβαρηνου, καὶ " +
                                    "ἠρίθμησεν αὐτὰ τῷ Σασαβασαρ ἄρχοντι τοῦ Ιουδα.\n" +
                                    "9: καὶ οὗτος ὁ ἀριθμὸς αὐτῶν· ψυκτῆρες χρυσοῖ τριάκοντα καὶ ψυκτῆρες ἀργυροῖ χίλιοι, παρηλλαγμένα ἐννέα καὶ εἴκοσι,",
                                    "μητρὸς:\n======\n\nNoun: Genitive Singular Feminine\n\nRoot of word: μήτηρ",
                                    "Word analysed: μήτηρ\n====================\n\n" +
                                    "\t\t1 a mother, Hom., etc.; of animals, a dam, id=Hom.;   ἀπό or   ἐκ μητρός from one's mother's womb, Pind., Aesch.\n" +
                                    "\t\t2 also of lands,  μήτηρ μήλων, θηρῶν mother of flocks, of game, Il.; of Earth,  γῆ πάντων μ. Hes.;   γῆ μήτηρ Aesch.;" +
                                    " ὦ γαῐα μῆτερ Eur.: also   ἡ μάτηρ alone for   δημήτηρ, Hdt.\n" +
                                    "\t\t3 of one's native land,  μᾶτερ ἐμά, θήβα Pind., etc.\n" +
                                    "\t\t4 poet. as the source of events,  μ. ἀέθλων, of Olympia, id=Pind.; night is the mother of day, Aesch.; " +
                                    "the grape of wine, id=Aesch.",
                                    "Philippians 4.3:  ναὶ ἐρωτῶ καὶ σέ γνήσιε σύζυγε συλλαμβάνου αὐταῖς αἵτινες ἐν τῷ εὐαγγελίῳ συνήθλησάν μοι " +
                                    "μετὰ καὶ Κλήμεντος καὶ τῶν λοιπῶν συνεργῶν μου ὧν τὰ ὀνόματα ἐν ^βίβλῳ *ζωῆς.|" +
                                    "Revelation 3.5:  ὁ νικῶν οὕτως περιβαλεῖται ἐν ἱματίοις λευκοῖς καὶ οὐ μὴ ἐξαλείψω τὸ ὄνομα αὐτοῦ ἐκ τῆς ^βίβλου " +
                                    "τῆς *ζωῆς καὶ ὁμολογήσω τὸ ὄνομα αὐτοῦ ἐνώπιον τοῦ πατρός μου καὶ ἐνώπιον τῶν ἀγγέλων αὐτοῦ|" +
                                    "Revelation 20.15:  καὶ εἴ τις οὐχ εὑρέθη ἐν τῇ ^βίβλῳ τῆς *ζωῆς γεγραμμένος ἐβλήθη εἰς τὴν λίμνην τοῦ πυρός",
                                    "Matthew 1:23\n\nNouns:\n\t\t	γαστήρ\n\t\t    Ἐμμανουήλ\n\t\t    θεός\n\t\t    ὄνομα\n\t\t    παρθένος\n\t\t    υἱός\n" +
                                    "Verbs:\n\t\t    εἰμί\n\t\t            ἔχω\n\t\t    καλέω\n\t\t    μεθερμηνεύω\n\t\t    τίκτω\n" +
                                    "Adverbs:\n\t\t	ἰδού",
                                    "My notes tell me that this:\n\n" +
                                    "Τοῦ δὲ Ἰησοῦ χριστοῦ ἡ γένεσις οὕτως ἦν. μνηστευθείσης τῆς μητρὸς αὐτοῦ Μαρίας τῷ Ἰωσήφ, πρὶν ἢ συνελθεῖν αὐτοὺς " +
                                    "εὑρέθη ἐν γαστρὶ ἔχουσα ἐκ πνεύματος ἁγίου.\n\nis a verse which ..." };

        public frmPreferences()
        {
            InitializeComponent();
        }

        public void initialisePreferences(classGlobal inConfig, classRegistry inRegistry)
        {
            int idx;
            String[] comboItems = { "6", "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "32", "36", "48", "60", "72" };


            Label[] fontInfo, textInfo, bgroundInfo;

            globalVars = inConfig;
            appRegistry = inRegistry;

            largerFont = new Font(FontFamily.GenericSansSerif, 12F, FontStyle.Regular);
            textFont = globalVars.FontForArea;
            fgColour = globalVars.ForeColorRec;
            bgColour = globalVars.BackColorRec;
            primarySearchColour = globalVars.PrimaryColour;
            secondarySearchColour = globalVars.SecondaryColour;
            noOfAreas = textFont.Length;
            tabCollection = new TabPage[noOfAreas];
            fontInfo = new Label[noOfAreas];
            textInfo = new Label[noOfAreas];
            bgroundInfo = new Label[noOfAreas];
            cbFontSizes = new ComboBox[noOfAreas];
            txtText = new TextBox[noOfAreas];
            txtBg = new TextBox[noOfAreas];
            rtxtExample = new RichTextBox[noOfAreas];
            tabCollection[0] = createNewTabPage("NT Text Area");
            tabCollection[1] = createNewTabPage("LXX Text Area");
            tabCollection[2] = createNewTabPage("Parse Pane");
            tabCollection[3] = createNewTabPage("Word Meaning Pane");
            tabCollection[4] = createNewTabPage("Search Results Pane");
            tabCollection[5] = createNewTabPage("Vocab List Pane");
            tabCollection[6] = createNewTabPage("Notes Area");
            for (idx = 0; idx < noOfAreas; idx++)
            {
                tabCtlColours.Controls.Add(tabCollection[idx]);
                fontInfo[idx] = new Label();
                fontInfo[idx].Text = "Set the font size to:";
                fontInfo[idx].Left = 37;
                fontInfo[idx].Top = 13;
                fontInfo[idx].Font = largerFont;
                fontInfo[idx].AutoSize = true;
                tabCollection[idx].Controls.Add(fontInfo[idx]);
                textInfo[idx] = new Label();
                textInfo[idx].Text = "Text Colour:";
                textInfo[idx].Left = 16;
                textInfo[idx].Top = 41;
                textInfo[idx].Font = largerFont;
                textInfo[idx].AutoSize = true;
                tabCollection[idx].Controls.Add(textInfo[idx]);
                bgroundInfo[idx] = new Label();
                bgroundInfo[idx].Text = "Background Colour:";
                bgroundInfo[idx].Left = 16;
                bgroundInfo[idx].Top = 69;
                bgroundInfo[idx].Font = largerFont;
                bgroundInfo[idx].AutoSize = true;
                tabCollection[idx].Controls.Add(bgroundInfo[idx]);
                cbFontSizes[idx] = new ComboBox();
                cbFontSizes[idx].Left = 190;
                cbFontSizes[idx].Top = 10;
                cbFontSizes[idx].Width = 40;
                cbFontSizes[idx].Font = largerFont;
                cbFontSizes[idx].Items.AddRange(comboItems);
                cbFontSizes[idx].Text = textFont[idx].Size.ToString();
                cbFontSizes[idx].SelectedItem = textFont[idx].Size.ToString();
                cbFontSizes[idx].SelectedIndexChanged += FrmPreferences_SelectedIndexChanged;
                tabCollection[idx].Controls.Add(cbFontSizes[idx]);
                txtText[idx] = new TextBox();
                txtText[idx].Left = 115;
                txtText[idx].Top = 38;
                txtText[idx].Width = 60;
                txtText[idx].Font = largerFont;
                txtText[idx].BackColor = fgColour[idx];
                txtText[idx].ReadOnly = true;
                txtText[idx].MouseClick += FrmPreferences_FGMouseClick;
                tabCollection[idx].Controls.Add(txtText[idx]);
                txtBg[idx] = new TextBox();
                txtBg[idx].Left = 115;
                txtBg[idx].Top = 89;
                txtBg[idx].Width = 60;
                txtBg[idx].Font = largerFont;
                txtBg[idx].BackColor = bgColour[idx];
                txtBg[idx].ReadOnly = true;
                txtBg[idx].MouseClick += FrmPreferences_BGMouseClick;
                tabCollection[idx].Controls.Add(txtBg[idx]);
                rtxtExample[idx] = new RichTextBox();
                rtxtExample[idx].Left = 253;
                rtxtExample[idx].Top = 10;
                rtxtExample[idx].Width = 400;
                rtxtExample[idx].Height = 227;
                if (idx == 4)
                {
                    createPrimaryAndSecondaryTextboxes(tabCollection[4], largerFont);
                    populateSearchExample(rtxtExample[4], exampleMessage[4], textFont[4], fgColour[4], tabCollection[4], largerFont);
                }
                else
                {
                    rtxtExample[idx].Text = exampleMessage[idx];
                    rtxtExample[idx].Font = textFont[idx];
                    rtxtExample[idx].ForeColor = fgColour[idx];
                }
                rtxtExample[idx].BackColor = bgColour[idx];
                tabCollection[idx].Controls.Add(rtxtExample[idx]);
            }
        }

        private void populateSearchExample(RichTextBox rtxtEg, String sourceString, Font mainText, Color textColour, TabPage searchPage, Font controlFont)
        {
            int idx, jdx, noOfEntries, noOfWords, nPstn;
            String workingText;
            String[] searchEntry, separateWords;
            Char[] splitParams = { '|' }, wordSplit = { ' ' };

            rtxtEg.Text = "";
            searchEntry = sourceString.Split(splitParams);
            noOfEntries = searchEntry.Length;
            for (idx = 0; idx < noOfEntries; idx++)
            {
                if (idx > 0) rtxtEg.AppendText("\n\n");
                nPstn = searchEntry[idx].IndexOf(':');
                if (nPstn > -1)
                {
                    rtxtEg.SelectionColor = textColour;
                    rtxtEg.SelectionFont = mainText;
                    rtxtEg.SelectedText = searchEntry[idx].Substring(0, nPstn) + " ";
                    nPstn++;
                    workingText = searchEntry[idx].Substring(nPstn).Trim();
                    separateWords = workingText.Split(wordSplit);
                    noOfWords = separateWords.Length;
                    for (jdx = 0; jdx < noOfWords; jdx++)
                    {
                        if (separateWords[jdx][0] == '^')
                        {
                            rtxtEg.SelectionColor = primarySearchColour;
                            rtxtEg.SelectionFont = mainText;
                            rtxtEg.SelectedText = " " + separateWords[jdx].Substring(1);
                        }
                        else
                        {
                            if (separateWords[jdx][0] == '*')
                            {
                                rtxtEg.SelectionColor = secondarySearchColour;
                                rtxtEg.SelectionFont = mainText;
                                rtxtEg.SelectedText = " " + separateWords[jdx].Substring(1);
                            }
                            else
                            {
                                rtxtEg.SelectionColor = textColour;
                                rtxtEg.SelectionFont = mainText;
                                rtxtEg.SelectedText = " " + separateWords[jdx];
                            }
                        }
                    }
                }
            }
        }

        private void createPrimaryAndSecondaryTextboxes(TabPage searchPage, Font controlFont)
        {
            Label labPrimaryLbl, labSecondaryLbl;

            labPrimaryLbl = new Label();
            labPrimaryLbl.Text = "Colour of Primary Match:";
            labPrimaryLbl.Left = 16;
            labPrimaryLbl.Top = 120;
            labPrimaryLbl.Font = controlFont;
            labPrimaryLbl.AutoSize = true;
            searchPage.Controls.Add(labPrimaryLbl);
            txtPrimary = new TextBox();
            txtPrimary.Left = 115;
            txtPrimary.Top = 140;
            txtPrimary.Width = 60;
            txtPrimary.Font = controlFont;
            txtPrimary.BackColor = primarySearchColour;
            txtPrimary.ReadOnly = true;
            txtPrimary.MouseClick += FrmPreferences_PrimaryMouseClick;
            searchPage.Controls.Add(txtPrimary);
            labSecondaryLbl = new Label();
            labSecondaryLbl.Text = "Colour of Secondary Match:";
            labSecondaryLbl.Left = 16;
            labSecondaryLbl.Top = 181;
            labSecondaryLbl.Font = controlFont;
            labSecondaryLbl.AutoSize = true;
            searchPage.Controls.Add(labSecondaryLbl);
            txtSecondary = new TextBox();
            txtSecondary.Left = 115;
            txtSecondary.Top = 201;
            txtSecondary.Width = 60;
            txtSecondary.Font = controlFont;
            txtSecondary.BackColor = secondarySearchColour;
            txtSecondary.ReadOnly = true;
            txtSecondary.MouseClick += FrmPreferences_SecondaryMouseClick;
            searchPage.Controls.Add(txtSecondary);
        }

        private void FrmPreferences_BGMouseClick(object sender, MouseEventArgs e)
        {
            int index;

            index = tabCtlColours.SelectedIndex;
            dlgBgColour.Color = txtBg[index].BackColor;
            if (dlgBgColour.ShowDialog() == DialogResult.OK)
            {
                txtBg[index].BackColor = dlgBgColour.Color;
                rtxtExample[index].BackColor = dlgBgColour.Color;
                bgColour[index] = dlgBgColour.Color;
            }
        }

        private void FrmPreferences_FGMouseClick(object sender, MouseEventArgs e)
        {
            int index;

            index = tabCtlColours.SelectedIndex;
            dlgForeColour.Color = txtText[index].BackColor;
            if (dlgForeColour.ShowDialog() == DialogResult.OK)
            {
                txtText[index].BackColor = dlgForeColour.Color;
                fgColour[index] = dlgForeColour.Color;
                if (index == 4) populateSearchExample(rtxtExample[4], exampleMessage[4], textFont[4], fgColour[4], tabCollection[4], largerFont);
                else rtxtExample[index].ForeColor = dlgForeColour.Color;
            }
        }

        private void FrmPreferences_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index;
            Font changedFont;

            index = tabCtlColours.SelectedIndex;
            changedFont = new Font(rtxtExample[index].Font.FontFamily, float.Parse(cbFontSizes[index].Text));
            textFont[index] = changedFont;
            rtxtExample[index].Font = changedFont;
        }

        private void FrmPreferences_PrimaryMouseClick(object sender, MouseEventArgs e)
        {
            int index;

            index = tabCtlColours.SelectedIndex;
            dlgBgColour.Color = txtPrimary.BackColor;
            if (dlgBgColour.ShowDialog() == DialogResult.OK)
            {
                txtPrimary.BackColor = dlgBgColour.Color;
                txtPrimary.Refresh();
                primarySearchColour = dlgBgColour.Color;
                populateSearchExample(rtxtExample[4], exampleMessage[4], textFont[4], fgColour[4], tabCollection[4], largerFont);
            }
        }

        private void FrmPreferences_SecondaryMouseClick(object sender, MouseEventArgs e)
        {
            int index;

            index = tabCtlColours.SelectedIndex;
            dlgSecondaryColour.Color = txtSecondary.BackColor;
            if (dlgSecondaryColour.ShowDialog() == DialogResult.OK)
            {
                secondarySearchColour = dlgSecondaryColour.Color;
                txtSecondary.BackColor = secondarySearchColour;
                txtSecondary.Refresh();
                populateSearchExample(rtxtExample[4], exampleMessage[4], textFont[4], fgColour[4], tabCollection[4], largerFont);
            }
        }

        private TabPage createNewTabPage(String name)
        {
            TabPage tempPage = new TabPage();

            tempPage.Text = name;
            return tempPage;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            globalVars.FontForArea = textFont;
            globalVars.ForeColorRec = fgColour;
            globalVars.BackColorRec = bgColour;
            globalVars.PrimaryColour = primarySearchColour;
            globalVars.SecondaryColour = secondarySearchColour;
            appRegistry.openRegistry();
            appRegistry.updateFontsAndColour();
            appRegistry.closeKeys();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
