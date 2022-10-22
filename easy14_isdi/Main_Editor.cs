using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easy14_isde
{
    public partial class Main_Editor : Form
    {
        public Main_Editor()
        {
            InitializeComponent();
            save_BTN_Module();
        }

        async private void save_BTN_Module()
        {
            while (true)
            {
                await Task.Delay(250);
                if (saveFile != null)
                {
                    string SavedfileContents = File.ReadAllText(saveFile);
                    if (SavedfileContents != code_text_area_rtb.Text)
                    {
                        save_file_btn.Enabled = true;
                    }
                    else
                    {
                        save_file_btn.Enabled = false;
                    }
                }
                else
                {
                    save_file_btn.Enabled = false;
                }
            }
        }
        public static string saveFile = null;
        public int trys_set = 1;

        private void ColourRrbText(RichTextBox rtb)
        {
            int i = rtb.SelectionStart;

            rtb.Select(0, rtb.Text.Length);
            rtb.SelectionColor = Color.White;
            rtb.SelectionStart = i;
            //Resets after coloring everything for next text
            void SetBackToNormalColor()
            {
                rtb.Select(i, 0);
                rtb.SelectionColor = Color.Black;
            }

            void ChangeColorOfWord(string Regex_str, string color_name)
            {
                i = rtb.SelectionStart;
                Regex regExp = new Regex(Regex_str);
                foreach (Match match in regExp.Matches(rtb.Text))
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = Color.FromName(color_name);
                    if (rtb.Text.Length >= i)
                    {
                        i = i++;
                    }
                    else if (rtb.Text.Length <= i)
                    {
                        i = i--;
                    }
                }
                SetBackToNormalColor();
            }

            ChangeColorOfWord("if|else|while", "Teal");
            ChangeColorOfWord("using", "Orange");
            ChangeColorOfWord("\"", "DarkGreen");
            ChangeColorOfWord("Console", "LightBlue");
        }

        private void code_text_area_rtb_TextChanged(object sender, System.EventArgs e)
        {
            if (code_text_area_rtb.Text != null)
            {
                ColourRrbText(code_text_area_rtb);
            }
        }

        private void run_code_btn_Click(object sender, System.EventArgs e)
        {
            if (saveFile == null)
            {
                DialogResult dialogResult = MessageBox.Show("File needs to be saved to run, continue?", "Unsaved File", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No) return;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != "")
                {
                    File.WriteAllText(saveFileDialog.FileName, code_text_area_rtb.Text);
                }
                Console.WriteLine(saveFileDialog.FileName);
                saveFile = saveFileDialog.FileName;
            }
            string dir = Directory.GetCurrentDirectory().Replace("easy14_isdi\\bin\\Debug", "") + "\\Easy14_Programming_language\\bin\\Debug\\net6.0-windows\\Easy14_Programming_Language.exe";
            Console.WriteLine(dir);
            Console.WriteLine(saveFile);
            Process.Start(dir, saveFile);

        }

        private void open_file_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            saveFile = openFileDialog.FileName;
            this.Text = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\") + 1, openFileDialog.FileName.Length - openFileDialog.FileName.LastIndexOf("\\") - 1) + " - Easy14 Scripter";
            code_text_area_rtb.Text = string.Join("\n", File.ReadAllLines(openFileDialog.FileName));
        }

        private void settings_btn_Click(object sender, EventArgs e)
        {
            //only for now since i dont have much time to focus on ISDE 
            if (trys_set <= 3)
            {
                MessageBox.Show("This area of the program is not flly developed (yet)", "404 Not Found");
                trys_set = trys_set + 1;
            }
            else
            {
                MessageBox.Show("Fine!, i bring you...", "?");
                settings_form settings_Form = new settings_form();
                settings_Form.Show();
            }
        }

        private void save_file_btn_Click(object sender, EventArgs e)
        {
            if (saveFile != null)
            {
                File.WriteAllText(saveFile, code_text_area_rtb.Text);
            }
        }

        private void Main_Editor_Load(object sender, EventArgs e)
        {
            string settings_file = Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location)))))) + "\\settings.ini";
            if (File.Exists(settings_file))
            {
                List<string> file_contents = new List<string>(File.ReadAllLines(settings_file));
                file_contents.RemoveAt(0);
                if (file_contents.Count > 0)
                {
                    if (file_contents[0].StartsWith("theme"))
                    {
                        if (file_contents[0].ToLower().EndsWith("dark"))
                        {
                            code_text_area_rtb.BackColor = Color.FromArgb(64, 64, 64);
                            code_text_area_rtb.ForeColor = Color.White;
                            BackColor = Color.FromKnownColor(KnownColor.WindowFrame);
                        }
                        else if (file_contents[0].ToLower().EndsWith("light"))
                        {
                            code_text_area_rtb.BackColor = Color.FromArgb(200, 200, 200);
                            code_text_area_rtb.ForeColor = Color.Black;
                            BackColor = Color.FromKnownColor(KnownColor.WindowFrame);
                        }
                        else if (file_contents[0].ToLower().EndsWith("communist"))
                        {
                            code_text_area_rtb.BackColor = Color.FromArgb(200, 20, 30);
                            code_text_area_rtb.ForeColor = Color.Yellow;
                            BackColor = Color.FromArgb(100, 10, 15);
                        }
                        else
                        {
                            MessageBox.Show("Theme at line \"" + file_contents[0] + "\" does NOT exist!", "Error");
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                File.WriteAllText(settings_file, "[Settings]\ntheme=dark");
            }
        }
    }
}
