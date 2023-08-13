using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace easy14_isde
{
    public partial class settings_form : Form
    {
        public static string optionsFile = "options.ini";
        public settings_form()
        {
            InitializeComponent();
        }

        private string current_theme_;
        public string current_theme
        {
            get { return current_theme_; }
            set { current_theme_ = value; }
        }

        private void title_label_Click(object sender, EventArgs e)
        {
            /*if (set_me_to_1 == 1)
            {1
                this.BackColor = Color.Red;
                title_label.ForeColor = Color.Yellow;
            }*/

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) //Light theme
            {
                string settings_file = Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location)))))) + "\\settings.ini";
                File.WriteAllText(settings_file, "theme light");
                Debug.WriteLine("Theme set to LIGHT");
            }
            else if (comboBox1.SelectedIndex == 1) //Dark theme
            {
                string settings_file = Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location)))))) + "\\settings.ini";
                File.WriteAllText(settings_file, "theme dark");
                Debug.WriteLine("Theme set to DARK");
            }
            else { return; }
            SuccessText();
        }

        async void SuccessText()
        {
            /*MessageLabel.Text = "Applyed Settings!";*/
            MessageLabel.Text = "";
            foreach (char c in "Applyed Settings!")
            {
                MessageLabel.Text += c;
                await Task.Delay(10);
            }
            await Task.Delay(2000);
            foreach (char c in "Applyed Settings!")
            {
                MessageLabel.Text = MessageLabel.Text.Substring(0, MessageLabel.Text.Length -1 );
                await Task.Delay(10);
            }
            foreach (char c in "...")
            {
                MessageLabel.Text += c;
                await Task.Delay(100);
            }
            MessageLabel.Text = "...";
        }

        private void settings_form_Paint(object sender, PaintEventArgs e)
        {
            button1.FlatStyle = FlatStyle.Flat; button1.FlatAppearance.BorderSize = 1;
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
                    button1.BackColor = Color.DimGray;
                    button1.ForeColor = Color.White;
                }
                else if (lines[0] == "theme light")
                {
                    if (current_theme == "light")
                    {
                        return;
                    }
                    this.BackColor = Color.White;
                    button1.BackColor = Color.Gray;
                    button1.ForeColor = Color.White;
                }
            }
            current_theme = lines[0].Substring(6);
            this.Refresh();
        }

        private void ReloadSettingsBTN_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        void LoadSettings()
        {
            List<string> optionsFileContents = File.ReadAllLines(optionsFile).ToList();
            string themesFolder = "";
            foreach (string line in optionsFileContents)
            {
                if (line != "")
                {
                    string var = line.Trim().Split('=')[0];
                    string val = line.Trim().Split('=')[1];
                    if (var == "themesFolder")
                    {
                        themesFolder = val.Substring(1, val.Length - 2);
                    }
                }
            }

            List<string> themeFiles = Directory.GetFiles(themesFolder)
                                               .Where(file => file.EndsWith(".json"))
                                               .ToList();

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(themeFiles.ToArray());
        }

    }
}
