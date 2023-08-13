using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace easy14_isde //Stands for Easy14 Integrated Scripting Developent Environment
{
    public partial class Main_Editor : Form
    {
        public Main_Editor()
        {
            InitializeComponent();
            save_BTN_Module();
        }

        private string current_theme_;
        public string current_theme
        {
            get { return current_theme_; }
            set { current_theme_ = value; }
        }
        async private void save_BTN_Module()
        {
            while (true)
            {
                await Task.Delay(250);
                if (saveFile != null)
                {
                    if (saveFile != "")
                    {
                        string SavedfileContents = File.ReadAllText(saveFile);
                        if (SavedfileContents != code_text_area_rtb.Text)
                        {
                            save_file_btn.Enabled = true;
                            save_file_btn.Visible = true;
                        }
                        else
                        {
                            save_file_btn.Enabled = false;
                            save_file_btn.Visible = false;
                        }
                    }
                    else
                    {
                        save_file_btn.Enabled = false;
                        save_file_btn.Visible = false;
                    }
                }
                else
                {
                    save_file_btn.Enabled = false;
                    save_file_btn.Visible = false;
                }
            }
        }
        public static string saveFile = null;

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
            ChangeColorOfWord("\"", "LightGreen");
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
            else if (saveFile != null)
            {
                saveFile = "2";
                var w = new Form() { Size = new Size(0, 0) };
                Task.Delay(TimeSpan.FromSeconds(1))
                    .ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                MessageBox.Show(w, $"Saving code as {saveFile}", $"Saving file {saveFile}");
                try
                {
                    var savedDialog = new Form() { Size = new Size(0, 0) };
                    File.WriteAllText(saveFile, code_text_area_rtb.Text);
                    Task.Delay(TimeSpan.FromSeconds(10))
                    .ContinueWith((t) => savedDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    MessageBox.Show(savedDialog, "File Saved");
                }
                catch
                {
                    var errorDialog = new Form() { Size = new Size(0, 0) };

                    for (int x = 10; x < 0; x--)
                    {
                        errorDialog = new Form() { Size = new Size(0, 0) };
                        Task.Delay(TimeSpan.FromSeconds(1))
                        .ContinueWith((t) => errorDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                        MessageBox.Show(errorDialog, $"File Failed to save!", $"Auto Closing in {x}");
                    }
                    Task.Delay(TimeSpan.FromSeconds(1))
                        .ContinueWith((t) => errorDialog.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                    return;
                }
            }
            else
            {
                MessageBox.Show("An Error occured while saving to file " + saveFile);
            }
            string dir = Directory.GetCurrentDirectory().Replace("easy14_isdi\\bin\\Debug", "") + "\\Easy14_Programming_language\\bin\\Debug\\net6.0-windows\\Easy14_Programming_Language.exe";
            Console.WriteLine(dir);
            Console.WriteLine(saveFile);
            Process.Start(dir, saveFile);

        }

        private void open_file_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Easy14 files(*.e14) | *.e14 |Older Easy14 files(*.easy14) | *.easy14";
            openFileDialog.ShowDialog();
            saveFile = openFileDialog.FileName;
            this.Text = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\") + 1, openFileDialog.FileName.Length - openFileDialog.FileName.LastIndexOf("\\") - 1) + " - Easy14 Scripter";
            if (openFileDialog.FileName != "")
            {
                code_text_area_rtb.Text = string.Join("\n", File.ReadAllLines(openFileDialog.FileName));
            }
        }

        private void settings_btn_Click(object sender, EventArgs e)
        {
            settings_form settings_Form = new settings_form();
            settings_Form.Show();
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
            //ThemeSetter();
        }

        private void Main_Editor_Paint(object sender, PaintEventArgs e)
        {
            //ThemeSetter();
        }

        private void ThemeSetter()
        {
            run_code_btn.FlatStyle = FlatStyle.Flat; run_code_btn.FlatAppearance.BorderSize = 1;
            save_file_btn.FlatStyle = FlatStyle.Flat; save_file_btn.FlatAppearance.BorderSize = 1;
            open_file_btn.FlatStyle = FlatStyle.Flat; open_file_btn.FlatAppearance.BorderSize = 1;

            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            dir = dir.Substring(6, dir.Length - 16);
            /*MessageBox.Show(File.Exists(dir + "\\options.txt").ToString());
            MessageBox.Show(dir + "\\options.txt".ToString());*/
            string file = dir + "\\settings.ini";
            string[] lines = File.ReadAllLines(file);

            if (lines.Length > 0)
            {
                if (lines[0] == "theme dark")
                {
                    if (current_theme == "dark")
                    {
                        return;
                    }
                    this.BackColor = SystemColors.WindowFrame;
                    run_code_btn.BackColor = Color.DimGray;
                    save_file_btn.BackColor = Color.DimGray;
                    open_file_btn.BackColor = Color.DimGray;
                    run_code_btn.ForeColor = Color.Lime;
                    save_file_btn.ForeColor = Color.DarkTurquoise;
                    open_file_btn.ForeColor = Color.Gold;
                }
                else if (lines[0] == "theme light")
                {
                    if (current_theme == "light")
                    {
                        return;
                    }
                    this.BackColor = Color.White;
                    run_code_btn.BackColor = Color.Gray;
                    save_file_btn.BackColor = Color.Gray;
                    open_file_btn.BackColor = Color.Gray;
                    run_code_btn.ForeColor = Color.Lime;
                    save_file_btn.ForeColor = Color.DarkTurquoise;
                    open_file_btn.ForeColor = Color.Gold;
                }
            }
            current_theme = lines[0].Substring(6);
            this.Refresh();
        }
    }
}
