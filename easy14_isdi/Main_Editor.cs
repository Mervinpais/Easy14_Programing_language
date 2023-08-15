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

        async private void save_BTN_Module()
        {
            while (true)
            {
                await Task.Delay(500);
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
            OutputRTB.Clear();
            actionLB.Text = "Running code";
            if (saveFile == "")
            {
                DialogResult dialogResult = MessageBox.Show("File needs to be saved to run, continue?", "Unsaved File", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No) return;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != "") File.WriteAllText(saveFileDialog.FileName, code_text_area_rtb.Text);

                saveFile = saveFileDialog.FileName;
            }
            else if (saveFile != "")
            {
                //saveFile = "2";
                var w = new Form() {};
                Task.Delay(TimeSpan.FromSeconds(1))
                    .ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                actionLB.Text = $"Saving code in \'{saveFile}\'";
                try
                {
                    var savedDialog = new Form() { Size = new Size(0, 0) };
                    File.WriteAllText(saveFile, code_text_area_rtb.Text);
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
                actionLB.Text = $"An Error occured while saving to file {saveFile}";
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string projectRoot = currentDirectory.Substring(0, currentDirectory.IndexOf("easy14_isdi\\"));

            string exePath = Path.Combine(projectRoot, "Easy14_Programming_language", "bin", "Debug", "net7.0-windows", "Easy14_Programming_Language.exe");

            Process Easy14App = new Process();
            Easy14App.StartInfo.FileName = exePath;
            Easy14App.StartInfo.Arguments = saveFile;
            Easy14App.StartInfo.UseShellExecute = false;
            Easy14App.StartInfo.RedirectStandardOutput = true;
            Easy14App.StartInfo.RedirectStandardError = true;
            Easy14App.StartInfo.CreateNoWindow = true;
            string exeDirectory = Path.GetDirectoryName(exePath); // Get the directory of the executable
            Easy14App.StartInfo.WorkingDirectory = exeDirectory;

            // Event handlers to capture the output
            Easy14App.OutputDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    UpdateOutputRTB(args.Data);
                }
            };

            Easy14App.ErrorDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    UpdateOutputRTB("Error: " + args.Data);
                }
            };
            actionLB.Text = $"Running code in {saveFile}";
            Easy14App.Start();
            Easy14App.BeginOutputReadLine();
            Easy14App.BeginErrorReadLine();
            Easy14ProcessOnExit(Easy14App);

        }

        private async void Easy14ProcessOnExit(Process easy14Process)
        {
            while (!easy14Process.HasExited)
            {
                await Task.Delay(500);
            }

            OutputRTB.AppendText(Environment.NewLine + "Easy14 exited successfully (Exit Code:" + easy14Process.ExitCode + ")");

            int startIndex = OutputRTB.Text.LastIndexOf("Easy14 exited successfully");
            int endIndex = OutputRTB.Text.Length;

            OutputRTB.Select(startIndex, endIndex - startIndex);
            OutputRTB.SelectionBackColor = Color.Green;
        }


        private void UpdateOutputRTB(string text)
        {
            if (OutputRTB.InvokeRequired)
            {
                OutputRTB.Invoke(new Action<string>(UpdateOutputRTB), text);
            }
            else
            {
                OutputRTB.AppendText(text + Environment.NewLine);
            }
        }

        private void open_file_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                this.Text = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\") + 1, openFileDialog.FileName.Length - openFileDialog.FileName.LastIndexOf("\\") - 1) + " - Easy14 Scripter";
                saveFile = openFileDialog.FileName;
                code_text_area_rtb.Text = string.Join(Environment.NewLine, File.ReadAllLines(openFileDialog.FileName));
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow AboutWindow = new AboutWindow();
            AboutWindow.ShowDialog();
        }
    }
}
