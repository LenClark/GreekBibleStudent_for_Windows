using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GreekBibleStudent
{
    class classKeyboard
    {
        bool isCaps = false, isOneTimeCaps = false;
        classGlobal globalVars;
        Panel keyboardPanel;
        Button[,] GkKeys;
        RadioButton[] rbtnTarget = new RadioButton[3];
        Label[] verseRefLabels;
        SortedDictionary<String, String> addRoughBreathing, addSmoothBreathing, addAccute, addGrave, addCirc, addDiaeresis, addIotaSub;

        static String[,] vowelConversion = new String[28, 14];

        public void initialiseKeyboard(classGlobal inConfig, Panel inputKBrdPanel, Label[] inputList)
        {
            globalVars = inConfig;
            keyboardPanel = inputKBrdPanel;
            verseRefLabels = inputList;
        }

        public void setupKeyboard()
        {
            const int xmax = 12, ymax = 4, ytop = 0, keyWidth = 25, keyHeight = 28, baseHeight = 5;   //baseHeight = 47;

            int x, y, keyIndent = 30;
            GroupBox gbTarget;
            RadioButton tempButton;

            GkKeys = new Button[ymax, xmax];
            //            keyIndent = 0;
            for (y = 0; y < ymax - 1; y++)
            {
                //                keyIndent += 5;
                for (x = 0; x < xmax; x++)
                {
                    if ((y == 2) && (x == 10))
                    {
                        break;
                    }
                    GkKeys[y, x] = new Button();
                    if (((y == 2) && (x == 7)) || ((y == 2) && (x == 8)))
                    {
                        GkKeys[y, x].Width = 2 * keyWidth;
                    }
                    else
                    {
                        GkKeys[y, x].Width = keyWidth;
                    }
                    GkKeys[y, x].Height = keyHeight;
                    if (((y == 2) && (x == 8)) || ((y == 2) && (x == 9)))
                    {
                        GkKeys[y, x].Left = keyIndent + keyWidth * (2 * x - 7);
                    }
                    else
                    {
                        GkKeys[y, x].Left = keyIndent + keyWidth * x;
                    }
                    GkKeys[y, x].Top = ytop + keyHeight * y + baseHeight;
                    GkKeys[y, x].Font = new System.Drawing.Font("Arial Unicode MS", 12);
                    GkKeys[y, x].Click += respondToButton;
                    keyboardPanel.Controls.Add(GkKeys[y, x]);
                }
            }
            // Tab, Space and (eventually) backspace
            //            keyIndent -= 5;
            GkKeys[3, 0] = new Button();
            GkKeys[3, 0].Width = 2 * keyWidth;
            GkKeys[3, 0].Height = keyHeight;
            GkKeys[3, 0].Left = keyIndent;
            GkKeys[3, 0].Top = ytop + 3 * keyHeight + baseHeight;
            GkKeys[3, 0].Font = new System.Drawing.Font("Arial Unicode MS", 12);
            GkKeys[3, 0].Click += respondToButton;
            keyboardPanel.Controls.Add(GkKeys[3, 0]);
            GkKeys[3, 1] = new Button();
            GkKeys[3, 1].Width = 6 * keyWidth;
            GkKeys[3, 1].Height = keyHeight;
            GkKeys[3, 1].Left = keyIndent + 2 * keyWidth;
            GkKeys[3, 1].Top = ytop + 3 * keyHeight + baseHeight;
            GkKeys[3, 1].Font = new System.Drawing.Font("Arial Unicode MS", 12);
            GkKeys[3, 1].Click += respondToButton;
            keyboardPanel.Controls.Add(GkKeys[3, 1]);
            GkKeys[3, 2] = new Button();
            GkKeys[3, 2].Width = 2 * keyWidth;
            GkKeys[3, 2].Height = keyHeight;
            GkKeys[3, 2].Left = keyIndent + 8 * keyWidth;
            GkKeys[3, 2].Top = ytop + 3 * keyHeight + baseHeight;
            GkKeys[3, 2].Font = new System.Drawing.Font("Arial Unicode MS", 12);
            GkKeys[3, 2].Click += respondToButton;
            keyboardPanel.Controls.Add(GkKeys[3, 2]);
            GkKeys[3, 3] = new Button();
            GkKeys[3, 3].Width = 2 * keyWidth;
            GkKeys[3, 3].Height = keyHeight;
            GkKeys[3, 3].Left = keyIndent + 10 * keyWidth;
            GkKeys[3, 3].Top = ytop + 3 * keyHeight + baseHeight;
            GkKeys[3, 3].Font = new System.Drawing.Font("Arial Unicode MS", 12);
            GkKeys[3, 3].Click += respondToButton;
            keyboardPanel.Controls.Add(GkKeys[3, 3]);

            createKeyCaptions(GkKeys, 0);

            // Now for the control of where typed text goes
            gbTarget = new GroupBox();
            gbTarget.Name = "gbKeyboardOptions";
            gbTarget.Left = keyIndent + 13 * keyWidth;
            gbTarget.Top = 5;
            gbTarget.Width = 195;
            gbTarget.Height = 80;
            gbTarget.Text = "Add text to ... ";
            keyboardPanel.Controls.Add(gbTarget);

            // And the radio buttons
            tempButton = new RadioButton();
            tempButton.Left = 25;
            tempButton.Top = 15;
            tempButton.Text = "Notes";
            tempButton.AutoSize = true;
            tempButton.Checked = true;
            tempButton.Tag = 1;
            rbtnTarget[0] = tempButton;
            gbTarget.Controls.Add(tempButton);

            tempButton = new RadioButton();
            tempButton.Left = 25;
            tempButton.Top = 10 + tempButton.Height;
            tempButton.Text = "Primary Search String";
            tempButton.AutoSize = true;
            tempButton.Checked = false;
            tempButton.Tag = 2;
            rbtnTarget[1] = tempButton;
            gbTarget.Controls.Add(tempButton);

            tempButton = new RadioButton();
            tempButton.Left = 25;
            tempButton.Top = 5 + 2 * tempButton.Height;
            tempButton.Text = "Secondary Search String";
            tempButton.AutoSize = true;
            tempButton.Checked = false;
            tempButton.Tag = 3;
            rbtnTarget[2] = tempButton;
            gbTarget.Controls.Add(tempButton);

            // Set up the vowel conversion table - not strictly keyboard but might as well combine the two

            addRoughBreathing = new SortedDictionary<string, string>();
            addRoughBreathing.Add("α", @"\u1f01");
            addRoughBreathing.Add("\u1f70", "\u1f03");
            addRoughBreathing.Add("ά", "\u1f05");
            addRoughBreathing.Add("\u1fb6", "\u1f07");
            addRoughBreathing.Add("\u1fb3", "\u1f81");
            addRoughBreathing.Add("\u1fb2", "\u1f83");
            addRoughBreathing.Add("\u1fb4", "\u1f85");
            addRoughBreathing.Add("\u1fb7", "\u1f87");
            addRoughBreathing.Add("ε", "\u1f11");
            addRoughBreathing.Add("\u1f72", "\u1f13");
            addRoughBreathing.Add("έ", "\u1f15");
            addRoughBreathing.Add("η", "\u1f21");
            addRoughBreathing.Add("\u1f74", "\u1f23");
            addRoughBreathing.Add("ή", "\u1f25");
            addRoughBreathing.Add("\u1fc6", "\u1f27");
            addRoughBreathing.Add("\u1fc3", "\u1f91");
            addRoughBreathing.Add("\u1fc2", "\u1f93");
            addRoughBreathing.Add("\u1fc4", "\u1f95");
            addRoughBreathing.Add("\u1fc7", "\u1f97");
            addRoughBreathing.Add("ι", "\u1f31");
            addRoughBreathing.Add("\u1f76", "\u1f33");
            addRoughBreathing.Add("ί", "\u1f35");
            addRoughBreathing.Add("\u1fd6", "\u1f37");
            addRoughBreathing.Add("ο", "\u1f41");
            addRoughBreathing.Add("\u1f78", "\u1f43");
            addRoughBreathing.Add("ό", "\u1f45");
            addRoughBreathing.Add("υ", "\u1f51");
            addRoughBreathing.Add("\u1f7a", "\u1f53");
            addRoughBreathing.Add("ύ", "\u1f55");
            addRoughBreathing.Add("\u1fe6", "\u1f57");
            addRoughBreathing.Add("ω", "\u1f61");
            addRoughBreathing.Add("\u1f7c", "\u1f63");
            addRoughBreathing.Add("ώ", "\u1f65");
            addRoughBreathing.Add("\u1ff6", "\u1f67");
            addRoughBreathing.Add("\u1ff3", "\u1fa1");
            addRoughBreathing.Add("\u1ff2", "\u1fa3");
            addRoughBreathing.Add("\u1ff4", "\u1fa5");
            addRoughBreathing.Add("\u1ff7", "\u1fa7");
            addRoughBreathing.Add("ρ", "ῥ");
            addRoughBreathing.Add("Α", "\u1f09");
            addRoughBreathing.Add("\u1fba", "\u1f0b");
            addRoughBreathing.Add("Ά", "\u1f0d");
            addRoughBreathing.Add("\u1fbc", "\u1f89");
            addRoughBreathing.Add("Ε", "\u1f19");
            addRoughBreathing.Add("\u1fc8", "\u1f1b");
            addRoughBreathing.Add("Έ", "\u1f1d");
            addRoughBreathing.Add("Η", "\u1f29");
            addRoughBreathing.Add("\u1fca", "\u1f2b");
            addRoughBreathing.Add("Ή", "\u1f2d");
            addRoughBreathing.Add("\u1fcc", "\u1f99");
            addRoughBreathing.Add("Ι", "\u1f39");
            addRoughBreathing.Add("\u1fda", "\u1f3b");
            addRoughBreathing.Add("Ί", "\u1f3d");
            addRoughBreathing.Add("Ο", "\u1f49");
            addRoughBreathing.Add("\u1ff8", "\u1f4b");
            addRoughBreathing.Add("Ό", "\u1f4d");
            addRoughBreathing.Add("Υ", "\u1f59");
            addRoughBreathing.Add("\u1fea", "\u1f5b");
            addRoughBreathing.Add("Ύ", "\u1f5d");
            addRoughBreathing.Add("Ω", "\u1f69");
            addRoughBreathing.Add("\u1ffa", "\u1f6b");
            addRoughBreathing.Add("Ώ", "\u1f6d");
            addRoughBreathing.Add("\u1ffc", "\u1fa9");
            addRoughBreathing.Add("Ρ", "Ρ");

            addSmoothBreathing = new SortedDictionary<string, string>();
            addSmoothBreathing.Add("α", "\u1f00");
            addSmoothBreathing.Add("\u1f70", "\u1f02");
            addSmoothBreathing.Add("ά", "\u1f04");
            addSmoothBreathing.Add("\u1fb6", "\u1f06");
            addSmoothBreathing.Add("\u1fb3", "\u1f80");
            addSmoothBreathing.Add("\u1fb2", "\u1f82");
            addSmoothBreathing.Add("\u1fb4", "\u1f84");
            addSmoothBreathing.Add("\u1fb7", "\u1f86");
            addSmoothBreathing.Add("ε", "\u1f10");
            addSmoothBreathing.Add("\u1f72", "\u1f12");
            addSmoothBreathing.Add("έ", "\u1f14");
            addSmoothBreathing.Add("η", "\u1f20");
            addSmoothBreathing.Add("\u1f74", "\u1f22");
            addSmoothBreathing.Add("ή", "\u1f24");
            addSmoothBreathing.Add("\u1fc6", "\u1f26");
            addSmoothBreathing.Add("\u1fc3", "\u1f90");
            addSmoothBreathing.Add("\u1fc2", "\u1f92");
            addSmoothBreathing.Add("\u1fc4", "\u1f94");
            addSmoothBreathing.Add("\u1fc7", "\u1f96");
            addSmoothBreathing.Add("ι", "\u1f30");
            addSmoothBreathing.Add("\u1f76", "\u1f32");
            addSmoothBreathing.Add("ί", "\u1f34");
            addSmoothBreathing.Add("\u1fd6", "\u1f36");
            addSmoothBreathing.Add("ο", "\u1f40");
            addSmoothBreathing.Add("\u1f78", "\u1f42");
            addSmoothBreathing.Add("ό", "\u1f44");
            addSmoothBreathing.Add("υ", "\u1f50");
            addSmoothBreathing.Add("\u1f7a", "\u1f52");
            addSmoothBreathing.Add("ύ", "\u1f54");
            addSmoothBreathing.Add("\u1fe6", "\u1f56");
            addSmoothBreathing.Add("ω", "\u1f60");
            addSmoothBreathing.Add("\u1f7c", "\u1f62");
            addSmoothBreathing.Add("ώ", "\u1f64");
            addSmoothBreathing.Add("\u1ff6", "\u1f66");
            addSmoothBreathing.Add("\u1ff3", "\u1fa0");
            addSmoothBreathing.Add("\u1ff2", "\u1fa2");
            addSmoothBreathing.Add("\u1ff4", "\u1fa4");
            addSmoothBreathing.Add("\u1ff7", "\u1fa6");
            addSmoothBreathing.Add("ρ", "ῤ");
            addSmoothBreathing.Add("Α", "\u1f08");
            addSmoothBreathing.Add("\u1fba", "\u1f0a");
            addSmoothBreathing.Add("Ά", "\u1f0c");
            addSmoothBreathing.Add("\u1fbc", "\u1f88");
            addSmoothBreathing.Add("Ε", "\u1f18");
            addSmoothBreathing.Add("\u1fc8", "\u1f1a");
            addSmoothBreathing.Add("Έ", "\u1f1c");
            addSmoothBreathing.Add("Η", "\u1f28");
            addSmoothBreathing.Add("\u1fca", "\u1f2a");
            addSmoothBreathing.Add("Ή", "\u1f2c");
            addSmoothBreathing.Add("\u1fcc", "\u1f98");
            addSmoothBreathing.Add("Ι", "\u1f38");
            addSmoothBreathing.Add("\u1fda", "\u1f3a");
            addSmoothBreathing.Add("Ί", "\u1f3c");
            addSmoothBreathing.Add("Ο", "\u1f48");
            addSmoothBreathing.Add("\u1ff8", "\u1f4a");
            addSmoothBreathing.Add("Ό", "\u1f4c");
            addSmoothBreathing.Add("Υ", "\u1f58");
            addSmoothBreathing.Add("\u1fea", "\u1f5a");
            addSmoothBreathing.Add("Ύ", "\u1f5c");
            addSmoothBreathing.Add("Ω", "\u1f68");
            addSmoothBreathing.Add("\u1ffa", "\u1f6a");
            addSmoothBreathing.Add("Ώ", "\u1f6c");
            addSmoothBreathing.Add("\u1ffc", "\u1fa8");

            addAccute = new SortedDictionary<string, string>();
            addAccute.Add("α", "ά");
            addAccute.Add("\u1f00", "\u1f04");
            addAccute.Add("\u1f01", "\u1f05");
            addAccute.Add("\u1fb3", "\u1fb4");
            addAccute.Add("\u1f80", "\u1f84");
            addAccute.Add("\u1f81", "\u1f85");
            addAccute.Add("ε", "έ");
            addAccute.Add("\u1f10", "\u1f14");
            addAccute.Add("\u1f11", "\u1f15");
            addAccute.Add("η", "ή");
            addAccute.Add("\u1f20", "\u1f24");
            addAccute.Add("\u1f21", "\u1f25");
            addAccute.Add("\u1fc3", "\u1fc4");
            addAccute.Add("\u1f90", "\u1f94");
            addAccute.Add("\u1f91", "\u1f95");
            addAccute.Add("ι", "ί");
            addAccute.Add("\u1f30", "\u1f34");
            addAccute.Add("\u1f31", "\u1f35");
            addAccute.Add("ϊ", "\u1fd3");
            addAccute.Add("ο", "ό");
            addAccute.Add("\u1f40", "\u1f44");
            addAccute.Add("\u1f41", "\u1f45");
            addAccute.Add("υ", "ύ");
            addAccute.Add("\u1f50", "\u1f54");
            addAccute.Add("\u1f51", "\u1f55");
            addAccute.Add("ϋ", "ΰ");
            addAccute.Add("ω", "ώ");
            addAccute.Add("\u1f60", "\u1f64");
            addAccute.Add("\u1f61", "\u1f65");
            addAccute.Add("\u1ff3", "\u1ff4");
            addAccute.Add("\u1fa0", "\u1fa4");
            addAccute.Add("\u1fa1", "\u1fa5");
            addAccute.Add("Α", "Ά");
            addAccute.Add("\u1f08", "\u1f0c");
            addAccute.Add("\u1f09", "\u1f0d");
            addAccute.Add("\u1f88", "\u1f8c");
            addAccute.Add("\u1f89", "\u1f8d");
            addAccute.Add("Ε", "Έ");
            addAccute.Add("\u1f18", "\u1f1c");
            addAccute.Add("\u1f19", "\u1f1d");
            addAccute.Add("Η", "Ή");
            addAccute.Add("\u1f28", "\u1f2c");
            addAccute.Add("\u1f29", "\u1f2d");
            addAccute.Add("\u1f98", "\u1f9c");
            addAccute.Add("\u1f99", "\u1f9d");
            addAccute.Add("Ι", "Ί");
            addAccute.Add("\u1f38", "\u1f3c");
            addAccute.Add("\u1f39", "\u1f3d");
            addAccute.Add("Ϊ", "\u1fd9");
            addAccute.Add("Ο", "Ό");
            addAccute.Add("\u1f48", "\u1f4c");
            addAccute.Add("\u1f49", "\u1f4d");
            addAccute.Add("Υ", "Ύ");
            addAccute.Add("\u1f58", "\u1f5c");
            addAccute.Add("\u1f59", "\u1f5d");
            addAccute.Add("Ϋ", "Ϋ");
            addAccute.Add("Ω", "Ώ");
            addAccute.Add("\u1f68", "\u1f6c");
            addAccute.Add("\u1f69", "\u1f6d");
            addAccute.Add("\u1fa8", "\u1fac");
            addAccute.Add("\u1fa9", "\u1fad");

            addGrave = new SortedDictionary<string, string>();
            addGrave.Add("α", "\u1f70");
            addGrave.Add("\u1f00", "\u1f02");
            addGrave.Add("\u1f01", "\u1f03");
            addGrave.Add("\u1fb3", "\u1fb2");
            addGrave.Add("\u1f80", "\u1f82");
            addGrave.Add("\u1f81", "\u1f83");
            addGrave.Add("ε", "\u1f72");
            addGrave.Add("\u1f10", "\u1f12");
            addGrave.Add("\u1f11", "\u1f13");
            addGrave.Add("η", "\u1f74");
            addGrave.Add("\u1f20", "\u1f22");
            addGrave.Add("\u1f21", "\u1f23");
            addGrave.Add("\u1fc3", "\u1fc2");
            addGrave.Add("\u1f90", "\u1f92");
            addGrave.Add("\u1f91", "\u1f93");
            addGrave.Add("ι", "\u1f76");
            addGrave.Add("\u1f30", "\u1f32");
            addGrave.Add("\u1f31", "\u1f33");
            addGrave.Add("ϊ", "\u1fd2");
            addGrave.Add("ο", "\u1f78");
            addGrave.Add("\u1f40", "\u1f42");
            addGrave.Add("\u1f41", "\u1f43");
            addGrave.Add("υ", "\u1f7a");
            addGrave.Add("\u1f50", "\u1f52");
            addGrave.Add("\u1f51", "\u1f53");
            addGrave.Add("ϋ", "\u1fe2");
            addGrave.Add("ω", "\u1f7c");
            addGrave.Add("\u1f60", "\u1f62");
            addGrave.Add("\u1f61", "\u1f63");
            addGrave.Add("\u1ff3", "\u1ff2");
            addGrave.Add("\u1fa0", "\u1fa2");
            addGrave.Add("\u1fa1", "\u1fa3");
            addGrave.Add("Α", "\u1fba");
            addGrave.Add("\u1f08", "\u1f0a");
            addGrave.Add("\u1f09", "\u1f0b");
            addGrave.Add("\u1f88", "\u1f8a");
            addGrave.Add("\u1f89", "\u1f8b");
            addGrave.Add("Ε", "\u1fc8");
            addGrave.Add("\u1f18", "\u1f1a");
            addGrave.Add("\u1f19", "\u1f1b");
            addGrave.Add("Η", "\u1fca");
            addGrave.Add("\u1f28", "\u1f2a");
            addGrave.Add("\u1f29", "\u1f2b");
            addGrave.Add("\u1f98", "\u1f9a");
            addGrave.Add("\u1f99", "\u1f9b");
            addGrave.Add("Ι", "\u1fda");
            addGrave.Add("\u1f38", "\u1f3a");
            addGrave.Add("\u1f39", "\u1f3b");
            addGrave.Add("Ο", "\u1ff8");
            addGrave.Add("\u1f48", "\u1f4a");
            addGrave.Add("\u1f49", "\u1f4b");
            addGrave.Add("Υ", "\u1fea");
            addGrave.Add("\u1f58", "\u1f5a");
            addGrave.Add("\u1f59", "\u1f5b");
            addGrave.Add("Ω", "\u1ffa");
            addGrave.Add("\u1f68", "\u1f6a");
            addGrave.Add("\u1f69", "\u1f6b");
            addGrave.Add("\u1fa8", "\u1faa");
            addGrave.Add("\u1fa9", "\u1fab");

            addCirc = new SortedDictionary<string, string>();
            addCirc.Add("α", "\u1fb6");
            addCirc.Add("\u1f00", "\u1f06");
            addCirc.Add("\u1f01", "\u1f07");
            addCirc.Add("\u1fb7", "\u1fb2");
            addCirc.Add("\u1f80", "\u1f86");
            addCirc.Add("\u1f81", "\u1f87");
            addCirc.Add("η", "\u1fc6");
            addCirc.Add("\u1f20", "\u1f26");
            addCirc.Add("\u1f21", "\u1f27");
            addCirc.Add("\u1fc7", "\u1fc2");
            addCirc.Add("\u1f90", "\u1f96");
            addCirc.Add("\u1f91", "\u1f97");
            addCirc.Add("ι", "\u1fd6");
            addCirc.Add("\u1f30", "\u1f36");
            addCirc.Add("\u1f31", "\u1f37");
            addCirc.Add("ϊ", "\u1fd7");
            addCirc.Add("υ", "\u1fe6");
            addCirc.Add("\u1f50", "\u1f56");
            addCirc.Add("\u1f51", "\u1f57");
            addCirc.Add("ϋ", "\u1fe7");
            addCirc.Add("ω", "\u1ff6");
            addCirc.Add("\u1f60", "\u1f66");
            addCirc.Add("\u1f61", "\u1f67");
            addCirc.Add("\u1ff7", "\u1ff2");
            addCirc.Add("\u1fa0", "\u1fa6");
            addCirc.Add("\u1fa1", "\u1fa7");
            addCirc.Add("\u1f08", "\u1f0e");
            addCirc.Add("\u1f09", "\u1f0f");
            addCirc.Add("\u1f88", "\u1f8e");
            addCirc.Add("\u1f89", "\u1f8f"); //Α
            addCirc.Add("\u1f28", "\u1f2e");
            addCirc.Add("\u1f29", "\u1f2f");
            addCirc.Add("\u1f98", "\u1f9e");
            addCirc.Add("\u1f99", "\u1f9f"); //Η
            addCirc.Add("\u1f38", "\u1f3e");
            addCirc.Add("\u1f39", "\u1f3f"); //Ι
            addCirc.Add("\u1f58", "\u1f5e");
            addCirc.Add("\u1f59", "\u1f5f"); //Υ
            addCirc.Add("\u1f68", "\u1f6e");
            addCirc.Add("\u1f69", "\u1f6f");
            addCirc.Add("\u1fa8", "\u1fae");
            addCirc.Add("\u1fa9", "\u1faf"); //Ω

            addDiaeresis = new SortedDictionary<string, string>();
            addDiaeresis.Add("ι", "ϊ");
            addDiaeresis.Add("\u1f76", "\u1fd2");
            addDiaeresis.Add("ί", "\u1fd3");
            addDiaeresis.Add("\u1fd6", "\u1fd7");
            addDiaeresis.Add("υ", "ϋ");
            addDiaeresis.Add("\u1f7a", "\u1fe2");
            addDiaeresis.Add("ύ", "ΰ");
            addDiaeresis.Add("\u1fe6", "\u1fe7");
            addDiaeresis.Add("Ι", "Ϊ");
            addDiaeresis.Add("Υ", "Ϋ");

            addIotaSub = new SortedDictionary<string, string>();
            addIotaSub.Add("α", "\u1fb3");
            addIotaSub.Add("\u1f00", "\u1f80");
            addIotaSub.Add("\u1f01", "\u1f81");
            addIotaSub.Add("\u1f70", "\u1fb2");
            addIotaSub.Add("ά", "\u1fb4");
            addIotaSub.Add("\u1fb6", "\u1fb7");
            addIotaSub.Add("\u1f02", "\u1f82");
            addIotaSub.Add("\u1f03", "\u1f83");
            addIotaSub.Add("\u1f04", "\u1f84");
            //            addIotaSub.Add("\u1f05", "\u1f85");
            addIotaSub.Add("\u1f06", "\u1f86");
            addIotaSub.Add("\u1f07", "\u1f87");
            addIotaSub.Add("η", "\u1fc3");
            addIotaSub.Add("\u1f20", "\u1f90");
            addIotaSub.Add("\u1f21", "\u1f91");
            addIotaSub.Add("\u1f74", "\u1fc2");
            addIotaSub.Add("ή", "\u1fc4");
            addIotaSub.Add("\u1fc6", "\u1fc7");
            addIotaSub.Add("\u1f22", "\u1f92");
            addIotaSub.Add("\u1f23", "\u1f93");
            addIotaSub.Add("\u1f24", "\u1f94");
            //            addIotaSub.Add("\u1f25", "\u1f95");
            addIotaSub.Add("\u1f26", "\u1f96");
            addIotaSub.Add("\u1f27", "\u1f97");
            addIotaSub.Add("ω", "\u1ff3");
            addIotaSub.Add("\u1f60", "\u1fa0");
            addIotaSub.Add("\u1f61", "\u1fa1");
            addIotaSub.Add("\u1f7c", "\u1ff2");
            addIotaSub.Add("ώ", "\u1ff4");
            addIotaSub.Add("\u1ff6", "\u1ff7");
            addIotaSub.Add("\u1f62", "\u1fa2");
            addIotaSub.Add("\u1f63", "\u1fa3");
            addIotaSub.Add("\u1f64", "\u1fa4");
            //            addIotaSub.Add("\u1f65", "\u1fa5");
            addIotaSub.Add("\u1f66", "\u1fa6");
            addIotaSub.Add("\u1f67", "\u1fa7");
            addIotaSub.Add("Α", "\u1fbc");
            addIotaSub.Add("\u1f08", "\u1f88");
            addIotaSub.Add("\u1f09", "\u1f89");
            addIotaSub.Add("\u1f0a", "\u1f8a");
            addIotaSub.Add("\u1f0b", "\u1f8b");
            addIotaSub.Add("\u1f0c", "\u1f8c");
            //            addIotaSub.Add("\u1f0d", "\u1f8d");
            addIotaSub.Add("\u1f0e", "\u1f8e");
            addIotaSub.Add("\u1f0f", "\u1f8f");
            addIotaSub.Add("Η", "\u1fcc");
            addIotaSub.Add("\u1f28", "\u1f98");
            addIotaSub.Add("\u1f29", "\u1f99");
            addIotaSub.Add("\u1f2a", "\u1f9a");
            addIotaSub.Add("\u1f2b", "\u1f9b");
            addIotaSub.Add("\u1f2c", "\u1f9c");
            //            addIotaSub.Add("\u1f2d", "\u1f9d");
            addIotaSub.Add("\u1f2e", "\u1f9e");
            addIotaSub.Add("\u1f2f", "\u1f9f");
            addIotaSub.Add("Ω", "\u1ffc");
            addIotaSub.Add("\u1f68", "\u1fa8");
            addIotaSub.Add("\u1f69", "\u1fa9");
            addIotaSub.Add("\u1f6a", "\u1faa");
            addIotaSub.Add("\u1f6b", "\u1fab");
            addIotaSub.Add("\u1f6c", "\u1fac");
            //            addIotaSub.Add("\u1f6d", "\u1fad");
            addIotaSub.Add("\u1f6e", "\u1fae");
            addIotaSub.Add("\u1f6f", "\u1faf");
        }

        private void respondToButton(object sender, EventArgs e)
        {
            /*------------------------------------------------------------------------*
             *                                                                        *
             *                         respondToButton                                *
             *                         ===============                                *
             *                                                                        *
             *  Handle a single key press (which involves sending text to one of:     *
             *  a) the notes area;                                                    *
             *  b) the main Search Text box;                                          *
             *  c) the secondary Search text box.                                     *
             *                                                                        *
             *------------------------------------------------------------------------*/

            bool isSearchUpdate = false, isComplexUpdate = false;
            int tagValue;
            String unicodeRepresentation;
            Button thisKey;

            thisKey = (Button)sender;
            unicodeRepresentation = thisKey.Text;
            tagValue = Convert.ToInt32(thisKey.Tag);
            // Processing Notes is a little different from handling the text boxes, so 
            //    do them seperately
            if (rbtnTarget[0].Checked)
            {
                updateNotes(thisKey.Text, Convert.ToInt32(thisKey.Tag), globalVars.getRichtextControlByIndex(4));
            }
            if (rbtnTarget[1].Checked)
            {
                isSearchUpdate = true;
                updateSearchBox(thisKey.Text, Convert.ToInt32(thisKey.Tag), globalVars.getSearchTextControlByIndex(0), false);
            }
            if (rbtnTarget[2].Checked)
            {
                isComplexUpdate = true;
                updateSearchBox(thisKey.Text, Convert.ToInt32(thisKey.Tag), globalVars.getSearchTextControlByIndex(1), true);
            }
            if (isSearchUpdate)
            {
                verseRefLabels[0].Text = "-1";
                verseRefLabels[1].Text = "-1";
                verseRefLabels[2].Text = "-1";
                verseRefLabels[3].Text = "-1";
            }
            if (isComplexUpdate)
            {
                verseRefLabels[4].Text = "-1";
                verseRefLabels[5].Text = "-1";
                verseRefLabels[6].Text = "-1";
                verseRefLabels[7].Text = "-1";
            }
        }

        private void updateSearchBox(String unicodeRepresentation, int tagValue, TextBox txtTarget, bool isSecondary)
        {
            int currPstn;
            String fulText;
            Label lblFirst, lblSecond;
            Button btnSecondarySearch;
            NumericUpDown udSearch;

            globalVars.TabCtrlBottomLeft.SelectedIndex = 2;
            if (isSecondary)
            {
                lblFirst = (Label)globalVars.getSearchItemsForTextEntryByIndex(0);
                lblSecond = (Label)globalVars.getSearchItemsForTextEntryByIndex(1);
                udSearch = (NumericUpDown)globalVars.getSearchItemsForTextEntryByIndex(2);
                btnSecondarySearch = (Button)globalVars.getSearchItemsForTextEntryByIndex(3);
                lblFirst.Visible = true;
                lblSecond.Visible = true;
                udSearch.Visible = true;
                txtTarget.Visible = true;
                btnSecondarySearch.Text = "Basic Search";
            }
            fulText = txtTarget.Text;
            currPstn = txtTarget.SelectionStart;
            if (((tagValue >= 1) && (tagValue <= 9)) || ((tagValue >= 13) && (tagValue <= 21)) || ((tagValue >= 25) && (tagValue <= 31)))
            {
                // Process normal Greek character
                simpleSearchBoxUpdate(txtTarget, fulText, currPstn, unicodeRepresentation);
                return;
            }
            switch (tagValue)
            {
                case 10: addSpecialSearchBox(txtTarget, fulText, currPstn, 1); break; // Smooth Breathing
                case 11: addSpecialSearchBox(txtTarget, fulText, currPstn, 2); break; // Rough Breathing
                case 12: addSpecialSearchBox(txtTarget, fulText, currPstn, 3); break; // Iota Subscript
                case 22: addSpecialSearchBox(txtTarget, fulText, currPstn, 4); break; // Varia (Grave accent)
                case 23: addSpecialSearchBox(txtTarget, fulText, currPstn, 5); break; // Oxia (Accute accent)
                case 24: addSpecialSearchBox(txtTarget, fulText, currPstn, 6); break; // Perispomeni (Circumflex)
                case 34: addSpecialSearchBox(txtTarget, fulText, currPstn, 7); break; // Diaeresis

                // First capture non-printable key info (e.g. breathings, accents)
                case 32:  // Shift
                    if (isOneTimeCaps)
                    {
                        isOneTimeCaps = false;
                        isCaps = false;
                        changeCase(true);
                    }
                    else
                    {
                        if (!isCaps)
                        {
                            isCaps = true;
                            isOneTimeCaps = true;
                            changeCase(false);
                        }
                    }
                    break;
                case 33:  // Caps Lock
                    if (isCaps)
                    {
                        isCaps = false;
                        isOneTimeCaps = false;
                        changeCase(true);
                    }
                    else
                    {
                        isCaps = true;
                        changeCase(false);
                    }
                    break;
                case 35:; break; // Tab
                case 36:; break; // Space
                case 37: deleteSearchBox(txtTarget, fulText, currPstn); break; // Backspace
                case 38:; break; // Return
                default:
                    {
                        break;
                    }
            }
        }

        private void simpleSearchBoxUpdate(TextBox txtEntry, String fullText, int currPstn, String unicodeRepresentation)
        {
            String beforeCursor, afterCursor;

            if ((fullText == null) || (fullText.Length == 0))
            {
                txtEntry.Text = unicodeRepresentation;
                txtEntry.SelectionStart = 1;
                return;
            }
            if (currPstn == 0)
            {
                txtEntry.Text = unicodeRepresentation + fullText;
                txtEntry.SelectionStart = 1;
                return;
            }
            if (currPstn == fullText.Length - 1)
            {
                txtEntry.Text = fullText + unicodeRepresentation;
                txtEntry.SelectionStart = ++currPstn;
                return;
            }
            beforeCursor = fullText.Substring(0, currPstn);
            afterCursor = fullText.Substring(currPstn);
            txtEntry.Text = beforeCursor + unicodeRepresentation + afterCursor;
            txtEntry.SelectionStart = ++currPstn;
            if (isOneTimeCaps)
            {
                isCaps = false;
                isOneTimeCaps = false;
                changeCase(true);
            }
            return;
        }

        private void addSpecialSearchBox(TextBox txtEntry, String fullText, int currPstn, int charCode)
        {
            bool hasChanged = false;
            String priorCharacter, newCharacter = "", beforeCursor, afterCursor;

            if ((fullText == null) || (fullText.Length == 0))
            {
                // Do nothing
                return;
            }
            if (currPstn == 0)
            {
                // Do nothing
                return;
            }
            beforeCursor = fullText.Substring(0, currPstn);
            afterCursor = fullText.Substring(currPstn);
            priorCharacter = beforeCursor.Substring(beforeCursor.Length - 1, 1);
            switch (charCode)
            {
                case 1: // Smooth breathing
                    if (addSmoothBreathing.ContainsKey(priorCharacter))
                    {
                        addSmoothBreathing.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 2: // Rough breathing
                    if (addRoughBreathing.ContainsKey(priorCharacter))
                    {
                        addRoughBreathing.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 3: // Iota subscript
                    if (addIotaSub.ContainsKey(priorCharacter))
                    {
                        addIotaSub.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 4: // Varia
                    if (addGrave.ContainsKey(priorCharacter))
                    {
                        addGrave.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 5: // Oxia
                    if (addAccute.ContainsKey(priorCharacter))
                    {
                        addAccute.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 6: // Perispomeni
                    if (addCirc.ContainsKey(priorCharacter))
                    {
                        addCirc.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 7: // Diaeresis
                    if (addDiaeresis.ContainsKey(priorCharacter))
                    {
                        addDiaeresis.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
            }
            if (hasChanged)
            {
                beforeCursor = beforeCursor.Substring(0, beforeCursor.Length - 1) + newCharacter;
                txtEntry.Text = beforeCursor + afterCursor;
                txtEntry.SelectionStart = currPstn;
            }
        }

        private void deleteSearchBox(TextBox txtEntry, String fullText, int currPstn)
        {
            String beforeCursor, afterCursor;

            if ((fullText == null) || (fullText.Length == 0))
            {
                // Do nothing
                return;
            }
            if (currPstn == 0)
            {
                // Do nothing
                return;
            }
            beforeCursor = fullText.Substring(0, currPstn);
            afterCursor = fullText.Substring(currPstn);
            txtEntry.Text = beforeCursor.Substring(0, beforeCursor.Length - 1) + afterCursor;
            txtEntry.SelectionStart = --currPstn;
        }

        private void updateNotes(String unicodeRepresentation, int tagValue, RichTextBox rtxtNotes)
        {
            int currPstn;
            String fullNote;

            globalVars.TabCtrlBottomLeft.SelectedIndex = 0;
            fullNote = rtxtNotes.Text;
            currPstn = rtxtNotes.SelectionStart;
            if (((tagValue >= 1) && (tagValue <= 9)) || ((tagValue >= 13) && (tagValue <= 21)) || ((tagValue >= 25) && (tagValue <= 31)))
            {
                // Process normal Greek character
                simpleRtxtUpdate(rtxtNotes, fullNote, currPstn, unicodeRepresentation);
                return;
            }
            switch (tagValue)
            {
                case 10: addSpecialRtxt(rtxtNotes, fullNote, currPstn, 1); break; // Smooth Breathing
                case 11: addSpecialRtxt(rtxtNotes, fullNote, currPstn, 2); break; // Rough Breathing
                case 12: addSpecialRtxt(rtxtNotes, fullNote, currPstn, 3); break; // Iota Subscript
                case 22: addSpecialRtxt(rtxtNotes, fullNote, currPstn, 4); break; // Varia (Grave accent)
                case 23: addSpecialRtxt(rtxtNotes, fullNote, currPstn, 5); break; // Oxia (Accute accent)
                case 24: addSpecialRtxt(rtxtNotes, fullNote, currPstn, 6); break; // Perispomeni (Circumflex)
                case 34: addSpecialRtxt(rtxtNotes, fullNote, currPstn, 7); break; // Diaeresis

                // First capture non-printable key info (e.g. breathings, accents)
                case 32:  // Shift
                    if (isOneTimeCaps)
                    {
                        isOneTimeCaps = false;
                        isCaps = false;
                        changeCase(true);
                    }
                    else
                    {
                        if (!isCaps)
                        {
                            isCaps = true;
                            isOneTimeCaps = true;
                            changeCase(false);
                        }
                    }
                    break;
                case 33:  // Caps Lock
                    if (isCaps)
                    {
                        isCaps = false;
                        isOneTimeCaps = false;
                        changeCase(true);
                    }
                    else
                    {
                        isCaps = true;
                        changeCase(false);
                    }
                    break;
                case 35: simpleRtxtUpdate(rtxtNotes, fullNote, currPstn, "	"); break; // Tab
                case 36: simpleRtxtUpdate(rtxtNotes, fullNote, currPstn, " "); break; // Space
                case 37: deleteRtxt(rtxtNotes, fullNote, currPstn); break; // Backspace
                case 38: simpleRtxtUpdate(rtxtNotes, fullNote, currPstn, "\n"); break; // Return
                default:
                    {
                        break;
                    }
            }
        }

        private void simpleRtxtUpdate(RichTextBox rtxtNotes, String fullNote, int currPstn, String unicodeRepresentation)
        {
            String beforeCursor, afterCursor;

            if ((fullNote == null) || (fullNote.Length == 0))
            {
                rtxtNotes.Text = unicodeRepresentation;
                rtxtNotes.SelectionStart = 1;
                return;
            }
            if (currPstn == 0)
            {
                rtxtNotes.Text = unicodeRepresentation + fullNote;
                rtxtNotes.SelectionStart = 1;
                return;
            }
            if (currPstn == fullNote.Length - 1)
            {
                rtxtNotes.Text = fullNote + unicodeRepresentation;
                rtxtNotes.SelectionStart = ++currPstn;
                return;
            }
            beforeCursor = fullNote.Substring(0, currPstn);
            afterCursor = fullNote.Substring(currPstn);
            rtxtNotes.Text = beforeCursor + unicodeRepresentation + afterCursor;
            rtxtNotes.SelectionStart = ++currPstn;
            if (isOneTimeCaps)
            {
                isCaps = false;
                isOneTimeCaps = false;
                changeCase(true);
            }
            return;
        }

        private void addSpecialRtxt(RichTextBox rtxtNotes, String fullNote, int currPstn, int charCode)
        {
            bool hasChanged = false;
            String priorCharacter, newCharacter = "", beforeCursor, afterCursor;

            if ((fullNote == null) || (fullNote.Length == 0))
            {
                // Do nothing
                return;
            }
            if (currPstn == 0)
            {
                // Do nothing
                return;
            }
            beforeCursor = fullNote.Substring(0, currPstn);
            afterCursor = fullNote.Substring(currPstn);
            priorCharacter = beforeCursor.Substring(beforeCursor.Length - 1, 1);
            switch (charCode)
            {
                case 1: // Smooth breathing
                    if (addSmoothBreathing.ContainsKey(priorCharacter))
                    {
                        addSmoothBreathing.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 2: // Rough breathing
                    if (addRoughBreathing.ContainsKey(priorCharacter))
                    {
                        addRoughBreathing.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 3: // Iota subscript
                    if (addIotaSub.ContainsKey(priorCharacter))
                    {
                        addIotaSub.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 4: // Varia
                    if (addGrave.ContainsKey(priorCharacter))
                    {
                        addGrave.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 5: // Oxia
                    if (addAccute.ContainsKey(priorCharacter))
                    {
                        addAccute.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 6: // Perispomeni
                    if (addCirc.ContainsKey(priorCharacter))
                    {
                        addCirc.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
                case 7: // Diaeresis
                    if (addDiaeresis.ContainsKey(priorCharacter))
                    {
                        addDiaeresis.TryGetValue(priorCharacter, out newCharacter);
                        if (newCharacter != null) hasChanged = true;
                    }
                    break;
            }
            if (hasChanged)
            {
                beforeCursor = beforeCursor.Substring(0, beforeCursor.Length - 1) + newCharacter;
                rtxtNotes.Text = beforeCursor + afterCursor;
                rtxtNotes.SelectionStart = currPstn;
            }
        }

        private void deleteRtxt(RichTextBox rtxtNotes, String fullNote, int currPstn)
        {
            String beforeCursor, afterCursor;

            if ((fullNote == null) || (fullNote.Length == 0))
            {
                // Do nothing
                return;
            }
            if (currPstn == 0)
            {
                // Do nothing
                return;
            }
            beforeCursor = fullNote.Substring(0, currPstn);
            afterCursor = fullNote.Substring(currPstn);
            rtxtNotes.Text = beforeCursor.Substring(0, beforeCursor.Length - 1) + afterCursor;
            rtxtNotes.SelectionStart = --currPstn;
        }

        private void changeCase(bool isLower)
        {
            if (isLower)
            {
                GkKeys[0, 0].Text = "ς";
                GkKeys[0, 1].Text = "ε";
                GkKeys[0, 2].Text = "ρ";
                GkKeys[0, 3].Text = "τ";
                GkKeys[0, 4].Text = "υ";
                GkKeys[0, 5].Text = "θ";
                GkKeys[0, 6].Text = "ι";
                GkKeys[0, 7].Text = "ο";
                GkKeys[0, 8].Text = "π";
                GkKeys[1, 0].Text = "α";
                GkKeys[1, 1].Text = "σ";
                GkKeys[1, 2].Text = "δ";
                GkKeys[1, 3].Text = "φ";
                GkKeys[1, 4].Text = "γ";
                GkKeys[1, 5].Text = "η";
                GkKeys[1, 6].Text = "ξ";
                GkKeys[1, 7].Text = "κ";
                GkKeys[1, 8].Text = "λ";
                GkKeys[2, 0].Text = "ζ";
                GkKeys[2, 1].Text = "χ";
                GkKeys[2, 2].Text = "ψ";
                GkKeys[2, 3].Text = "ω";
                GkKeys[2, 4].Text = "β";
                GkKeys[2, 5].Text = "ν";
                GkKeys[2, 6].Text = "μ";
            }
            else
            {
                GkKeys[0, 0].Text = "";
                GkKeys[0, 1].Text = "Ε";
                GkKeys[0, 2].Text = "Ρ";
                GkKeys[0, 3].Text = "Τ";
                GkKeys[0, 4].Text = "Υ";
                GkKeys[0, 5].Text = "Θ";
                GkKeys[0, 6].Text = "Ι";
                GkKeys[0, 7].Text = "Ο";
                GkKeys[0, 8].Text = "Π";
                GkKeys[1, 0].Text = "Α";
                GkKeys[1, 1].Text = "Σ";
                GkKeys[1, 2].Text = "Δ";
                GkKeys[1, 3].Text = "Φ";
                GkKeys[1, 4].Text = "Γ";
                GkKeys[1, 5].Text = "Η";
                GkKeys[1, 6].Text = "Ξ";
                GkKeys[1, 7].Text = "Κ";
                GkKeys[1, 8].Text = "Λ";
                GkKeys[2, 0].Text = "Ζ";
                GkKeys[2, 1].Text = "Χ";
                GkKeys[2, 2].Text = "Ψ";
                GkKeys[2, 3].Text = "Ω";
                GkKeys[2, 4].Text = "Β";
                GkKeys[2, 5].Text = "Ν";
                GkKeys[2, 6].Text = "Μ";
            }
        }

        private void createKeyCaptions(Button[,] keys, int caseCode)
        {
            int keyCount = 0, x, y;
            ToolTip[,] keyTooltip;
            String[] keyToolTip;

            keys[0, 0].Text = "ς"; keys[0, 0].Tag = "1";
            keys[0, 1].Text = "ε"; keys[0, 1].Tag = "2";
            keys[0, 2].Text = "ρ"; keys[0, 2].Tag = "3";
            keys[0, 3].Text = "τ"; keys[0, 3].Tag = "4";
            keys[0, 4].Text = "υ"; keys[0, 4].Tag = "5";
            keys[0, 5].Text = "θ"; keys[0, 5].Tag = "6";
            keys[0, 6].Text = "ι"; keys[0, 6].Tag = "7";
            keys[0, 7].Text = "ο"; keys[0, 7].Tag = "8";
            keys[0, 8].Text = "π"; keys[0, 8].Tag = "9";
            keys[1, 0].Text = "α"; keys[1, 0].Tag = "13";
            keys[1, 1].Text = "σ"; keys[1, 1].Tag = "14";
            keys[1, 2].Text = "δ"; keys[1, 2].Tag = "15";
            keys[1, 3].Text = "φ"; keys[1, 3].Tag = "16";
            keys[1, 4].Text = "γ"; keys[1, 4].Tag = "17";
            keys[1, 5].Text = "η"; keys[1, 5].Tag = "18";
            keys[1, 6].Text = "ξ"; keys[1, 6].Tag = "19";
            keys[1, 7].Text = "κ"; keys[1, 7].Tag = "20";
            keys[1, 8].Text = "λ"; keys[1, 8].Tag = "21";
            keys[2, 0].Text = "ζ"; keys[2, 0].Tag = "25";
            keys[2, 1].Text = "χ"; keys[2, 1].Tag = "26";
            keys[2, 2].Text = "ψ"; keys[2, 2].Tag = "27";
            keys[2, 3].Text = "ω"; keys[2, 3].Tag = "28";
            keys[2, 4].Text = "β"; keys[2, 4].Tag = "29";
            keys[2, 5].Text = "ν"; keys[2, 5].Tag = "30";
            keys[2, 6].Text = "μ"; keys[2, 6].Tag = "31";

            keyToolTip = new String[] { "Final sigma", "Epsilon", "Rho", "Tau", "Upsilon", "Theta", "Iota", "Omicron", "Pi", "Smooth Breathing",
                "Rough Breathing", "Iota subscript", "Alpha", "Sigma", "Delta", "Phi", "Gamma", "Eta", "Xi", "Kappa", "Lambda", "Varia (Grave accent)",
                "Oxia (Accute accent)", "Perispomeni (Circumflex)" , "Zeta", "Chi", "Psi", "Omega", "Beta", "Nu", "Mu", "Shift", "Caps Lock", "Diaeresis",
                   "Tab", "Space", "Backspace", "Return"};

            keyTooltip = new ToolTip[4, 12];
            for (y = 0; y < 3; y++)
            {
                for (x = 0; x < 12; x++)
                {
                    if ((y == 2) && (x == 10)) break;
                    keyTooltip[y, x] = new ToolTip();
                    keyTooltip[y, x].SetToolTip(GkKeys[y, x], keyToolTip[keyCount]);
                    keyCount++;
                }
            }

            keyTooltip[3, 2] = new ToolTip();
            keyTooltip[3, 3] = new ToolTip();

            keyTooltip[3, 2].SetToolTip(keys[3, 2], "Backspace");
            keyTooltip[3, 3].SetToolTip(keys[3, 3], "Return/Enter");

            keys[0, 9].Text = "᾿"; keys[0, 9].Tag = "10"; //Smooth breathing
            keys[0, 10].Text = "῾"; keys[0, 10].Tag = "11"; // Rough breathing
            keys[0, 11].Text = "ι"; keys[0, 11].Tag = "12"; //iota subscript
            keys[1, 9].Text = "\u1FEF"; keys[1, 9].Tag = "22"; // Varia (Grave accent) 
            keys[1, 10].Text = "\u1FFD"; keys[1, 10].Tag = "23"; // Oxia (Accute accent)
            keys[1, 11].Text = "\u1FC0"; keys[1, 11].Tag = "24"; // Perispomeni (Circumflex)
            keys[2, 7].Text = "Shft"; keys[2, 7].Tag = "32";
            keys[2, 8].Text = "Caps"; keys[2, 8].Tag = "33";
            keys[2, 9].Text = "¨"; keys[2, 9].Tag = "34"; //Diaeresis
            keys[3, 0].Image = Image.FromFile(globalVars.BaseDirectory + @"\" + globalVars.SourceFolder + @"\" +
                globalVars.KeyboardFolder + @"\Tab.png"); keys[3, 0].Tag = "35";
            keys[3, 1].Text = "Space"; keys[3, 1].Tag = "36";
            keys[3, 2].Image = Image.FromFile(globalVars.BaseDirectory + @"\" + globalVars.SourceFolder + @"\" +
                globalVars.KeyboardFolder + @"\BSpace.png"); keys[3, 2].Tag = "37";
            keys[3, 3].Image = Image.FromFile(globalVars.BaseDirectory + @"\" + globalVars.SourceFolder + @"\" +
                globalVars.KeyboardFolder + @"\Enter.png"); keys[3, 3].Tag = "38";
        }
    }
}
