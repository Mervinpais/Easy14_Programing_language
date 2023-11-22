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
    public partial class MainUI_Output : Form
    {
        List<Process> Easy14Process = new List<Process>();

        public MainUI_Output()
        {
            InitializeComponent();
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

        private void noteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("", string.Join(Environment.NewLine, new string[]
            {
                "From; Mervin",
                "To; You",
                "",
                "   Dear User,",
                "       I am Unable to keep up with school and working on this, and so i cant work on 3 things at once, and so i will probably never fix any bugs that occur in Easy14 SE (Scripting Environment) and will only patch any security issues, i hope you understand, and if you want, You, yes, YOU can support me by working on SE for me, but please, do it out of your own love for this Project, i dont want to force any of my brothers and sisters to be forced to do something i cant do due to my problems",
                "",
                "       And now, i guess i have to end this message",
                "",
                "       but yes, i hope you are well, your family is well etc. etc.",
                "       Thank you, for reading this.. <3",
                "",
                "   Yours truly,",
                "",
                "Mervin14"
            }));
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SE_Window se_Window = new SE_Window();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            se_Window.Show();
            se_Window.OpenFile(openFileDialog.FileName);
        }
    }
}
