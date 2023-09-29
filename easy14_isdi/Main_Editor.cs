using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easy14_SE //Stands for Easy14 Integrated Scripting Developent Environment
{
    public partial class Main_Editor : Form
    {
        List<Process> Easy14Process = new List<Process>();

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
                        if (SavedfileContents != CodeEditorArea_rtb.Text)
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
        public static string saveFile = "";
        private void ColourRrbText(RichTextBox rtb)
        {
            int i = rtb.SelectionStart;
            rtb.Select(0, rtb.Text.Length);
            rtb.SelectionColor = Color.White; // Set the default color to white
            rtb.SelectionStart = i;

            void SetBackToNormalColor()
            {
                rtb.Select(i, 0);
                rtb.SelectionColor = Color.White;
            }

            // Match and colorize method calls like myClass.XYZ.myMethod();
            void ChangeColorOfMethodCalls()
            {
                string text = rtb.Text;
                int currentIndex = 0;
                while (currentIndex < text.Length)
                {
                    int startIndex = text.IndexOf('.', currentIndex);
                    if (startIndex == -1)
                        break;

                    int endIndex = text.IndexOf(')', startIndex);

                    if (endIndex == -1)
                        break;

                    rtb.Select(startIndex, endIndex - startIndex + 1);
                    rtb.SelectionColor = Color.CornflowerBlue;

                    currentIndex = endIndex + 1;
                }
            }

            // Match and colorize keywords and identifiers
            void ChangeColorOfWord(string Regex_str, Color color)
            {
                i = rtb.SelectionStart;
                Regex regExp = new Regex(Regex_str);
                foreach (Match match in regExp.Matches(rtb.Text))
                {
                    rtb.Select(match.Index, match.Length);
                    rtb.SelectionColor = color;
                }
                SetBackToNormalColor();
            }

            ChangeColorOfMethodCalls();
            ChangeColorOfWord("\"[^\"]*\"", Color.LightGreen);
            ChangeColorOfWord("\\b(if|for|while)\\b", Color.CornflowerBlue);
            ChangeColorOfWord("\\b(import|method)\\b", Color.Orange);
        }



        private void code_text_area_rtb_TextChanged(object sender, System.EventArgs e)
        {
            if (CodeEditorArea_rtb.Text != null)
            {
                ColourRrbText(CodeEditorArea_rtb);
            }
        }

        private void run_code_btn_Click(object sender, System.EventArgs e)
        {
            if (saveFile == "")
            {
                DialogResult dialogResult = MessageBox.Show("File needs to be saved to run, continue?", "Unsaved File", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No) return;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != "") File.WriteAllText(saveFileDialog.FileName, CodeEditorArea_rtb.Text);

                saveFile = saveFileDialog.FileName;
            }
            else if (saveFile != "")
            {
                //saveFile = "2";
                var w = new Form() { };
                Task.Delay(TimeSpan.FromSeconds(1))
                    .ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());
                actionLB.Text = $"Saving code in \'{saveFile}\'";
                try
                {
                    var savedDialog = new Form() { Size = new Size(0, 0) };
                    File.WriteAllText(saveFile, CodeEditorArea_rtb.Text);
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

            OutputRTB.Clear();
            actionLB.Text = "Running code";

            string currentDirectory = Directory.GetCurrentDirectory();
            string projectRoot = currentDirectory.Substring(0, currentDirectory.IndexOf("easy14_isdi\\"));

            string exePath = Path.Combine(projectRoot, "Easy14_Programming_language", "bin", "Debug", "net7.0-windows", "Easy14_Programming_Language.exe");

            Easy14Process.Clear();
            Easy14Process.Add(new Process());
            Easy14Process[0].StartInfo.FileName = exePath;
            Easy14Process[0].StartInfo.Arguments = saveFile;
            Easy14Process[0].StartInfo.UseShellExecute = false;
            Easy14Process[0].StartInfo.RedirectStandardInput = true; // Enable input redirection
            Easy14Process[0].StartInfo.RedirectStandardOutput = true;
            Easy14Process[0].StartInfo.RedirectStandardError = true;
            Easy14Process[0].StartInfo.CreateNoWindow = true;

            string exeDirectory = Path.GetDirectoryName(exePath); // Get the directory of the executable
            Easy14Process[0].StartInfo.WorkingDirectory = exeDirectory;

            // Event handlers to capture the output
            Easy14Process[0].OutputDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    UpdateOutputRTB(args.Data);
                }
            };

            Easy14Process[0].ErrorDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    UpdateOutputRTB("Error: " + args.Data);
                }
            };

            // Update the label to indicate that code is running
            actionLB.Text = $"Running code in {saveFile}";

            // Start the process and begin reading its output and errors
            Easy14Process[0].Start();
            Easy14Process[0].BeginOutputReadLine();
            Easy14Process[0].BeginErrorReadLine();
            Easy14ProcessOnExit(Easy14Process[0]);

        }

        private async void Easy14ProcessOnExit(Process Easy14Process)
        {
            while (!Easy14Process.HasExited)
            {
                await Task.Delay(500);
            }

            actionLB.Text = $"Process finished in {Easy14Process.ExitTime.Millisecond}ms (Exit Code:{Easy14Process.ExitCode})";
            OutputRTB.AppendText($"{Environment.NewLine}Easy14 exited successfully (Exit Code:{Easy14Process.ExitCode})");

            int startIndex = OutputRTB.Text.LastIndexOf("Easy14 exited successfully");
            int endIndex = OutputRTB.Text.Length;

            OutputRTB.Select(startIndex, endIndex - startIndex);
            OutputRTB.SelectionBackColor = Color.Green;

            Easy14Process.CancelOutputRead();
            Easy14Process.CancelErrorRead();
            Easy14Process.Close();
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
                CodeEditorArea_rtb.Text = string.Join(Environment.NewLine, File.ReadAllLines(openFileDialog.FileName));
            }
        }

        private void settings_btn_Click(object sender, EventArgs e)
        {
            Settings settings_Form = new Settings();
            settings_Form.Show();
        }

        private void save_file_btn_Click(object sender, EventArgs e)
        {
            if (saveFile != null)
            {
                File.WriteAllText(saveFile, CodeEditorArea_rtb.Text);
            }
        }

        private void Main_Editor_Load(object sender, EventArgs e)
        {
            //ThemeSetter();
        }

        private void Main_Editor_Paint(object sender, PaintEventArgs e)
        {
            List<string> lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\options.ini").ToList();

            string fontFamilyLine = lines.FirstOrDefault(val => val.StartsWith("fontFamily"));
            string fontSizeLine = lines.FirstOrDefault(val => val.StartsWith("fontSize"));

            if (fontFamilyLine != null && fontSizeLine != null)
            {
                string fontFamily = fontFamilyLine.Substring("fontFamily=".Length);
                float fontSize = (float)Convert.ToDouble(fontSizeLine.Substring("fontSize=".Length));

                Font newFont = new Font(fontFamily, fontSize);

                if (CodeEditorArea_rtb.Font != newFont)
                {
                    CodeEditorArea_rtb.Font = newFont;
                    CodeEditorArea_rtb.Refresh();
                }

                if (OutputRTB.Font != newFont)
                {
                    OutputRTB.Font = newFont;
                    OutputRTB.Refresh();
                }
            }
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutWindow AboutWindow = new AboutWindow();
            AboutWindow.ShowDialog();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wordWrapToolStripMenuItem.Checked = !wordWrapToolStripMenuItem.Checked;
            CodeEditorArea_rtb.WordWrap = wordWrapToolStripMenuItem.Checked;
        }

        private void Main_Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Easy14Process[0].Close();
                Easy14Process.Clear();
            }
            catch { }
        }

        private void inputTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string userInput = inputTB.Text;
                // Send the user input to the process's standard input stream.
                Easy14Process[0].StandardInput.WriteLine(userInput);
                inputTB.Clear();
            }
        }

        private void noteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
