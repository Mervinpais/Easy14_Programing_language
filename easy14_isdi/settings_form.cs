using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easy14_SE
{
    public partial class settings_form : Form
    {
        public static string optionsFile = "options.ini";
        Dictionary<string, string> configFile = new Dictionary<string, string>();
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

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedThemeFile = comboBox1.SelectedItem.ToString();
            ApplyTheme(selectedThemeFile);
            SuccessText();
        }

        private void ApplyTheme(string themeFilePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(themeFilePath);
                dynamic themeSettings = JsonConvert.DeserializeObject(jsonContent);

                if (themeSettings != null)
                {
                    // Apply background color
                    Color backgroundColor = Color.FromName(themeSettings.BackgoundColor);
                    this.BackColor = backgroundColor;

                    // Apply text color
                    Color textColor = Color.FromName(themeSettings.TextColor);
                    // Apply the text color to relevant controls, e.g., labels, buttons, etc.

                    // Apply button colors
                    dynamic buttonColors = themeSettings.ButtonColors;
                    Color defaultButtonColor = Color.FromName(buttonColors.Default);
                    Color runButtonColor = Color.FromName(buttonColors.Run);
                    Color openButtonColor = Color.FromName(buttonColors.Open);
                    Color saveButtonColor = Color.FromName(buttonColors.Save);
                    // Apply these button colors to the respective buttons in your form

                    // Save the selected theme to settings.ini1
                    string settings_file = "settings.ini";
                    File.WriteAllText(settings_file, "theme " + themeFilePath);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions if necessary
                MessageBox.Show("Error applying theme: " + ex.Message);
            }
        }



        async void SuccessText()
        {
            try
            {
                MessageLabel.Text = "";
                foreach (char c in "Applyed Settings!")
                {
                    MessageLabel.Text += c;
                    await Task.Delay(10);
                }
                await Task.Delay(2000);
                foreach (char c in "Applyed Settings!")
                {
                    MessageLabel.Text = MessageLabel.Text.Substring(0, MessageLabel.Text.Length - 1);
                    await Task.Delay(10);
                }
                foreach (char c in "...")
                {
                    MessageLabel.Text += c;
                    await Task.Delay(100);
                }
                MessageLabel.Text = "...";
            }
            catch
            {

            }
        }

        private void settings_form_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void settings_form_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void ReloadSettingsBTN_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        void LoadSettings()
        {
            configFile.Clear();
            List<string> optionsFileContents = File.ReadAllLines(optionsFile).ToList();
            string themesFolder = "";
            string fontFamily = "";
            double fontSize = 0;
            string fontStyle = "";
            foreach (string line in optionsFileContents)
            {
                if (line != "")
                {
                    string var = line.Trim().Split('=')[0];
                    string val = line.Trim().Split('=')[1];
                    configFile.Add(var, val);
                }
            }

            for (int i = 0;i < configFile.Keys.Count; i++)
            {
                string var = configFile.Keys.ElementAt(i);
                string val = configFile.Values.ElementAt(i);
                if (var == "themesFolder")
                {
                    themesFolder = val;
                }
                else if (var == "fontFamily")
                {
                    fontFamily = val;
                }
                else if (var == "fontSize")
                {
                    try { fontSize = Convert.ToDouble(val); }
                    catch { }
                }
                else if (var == "fontStyle")
                {
                    fontStyle = val;
                }   
            }

            try
            {
                List<string> themeFiles = Directory.GetFiles(themesFolder)
                                                   .Where(file => file.EndsWith(".json"))
                                                   .ToList();
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(themeFiles.ToArray());
            } catch { }

            fontTest_label.Font = new Font(fontFamily, (float)Math.Round(fontSize));
        }

        private void changeFontBTN_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = new Font(configFile["fontFamily"], (float)(Math.Round(Convert.ToDouble(configFile["fontSize"]))));
            fontDialog.ShowDialog();

            configFile["fontFamily"] = fontDialog.Font.FontFamily.Name.ToString();
            configFile["fontSize"] = fontDialog.Font.Size.ToString();

            List<string> mainFileContent = new List<string>();

            for (int i = 0;i < configFile.Keys.Count; i++)
            {
                mainFileContent.Add($"{configFile.Keys.ElementAt(i)}={configFile.Values.ElementAt(i)}");
            }

            File.WriteAllLines(optionsFile, mainFileContent.ToArray());
            LoadSettings();
            SuccessText();
        }
    }
}
