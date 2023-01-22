using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace easy14_isde
{
    public partial class settings_form : Form
    {
        public settings_form()
        {
            InitializeComponent();
        }

        private void settings_form_Load(object sender, EventArgs e)
        {

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
            if (comboBox1.SelectedIndex == 1) //Dark theme
            {
                string settings_file = Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Convert.ToString(Directory.GetParent(Assembly.GetExecutingAssembly().Location)))))) + "\\settings.ini";
                File.WriteAllText(settings_file, "theme dark");
                Debug.WriteLine("Theme set to DARK");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
