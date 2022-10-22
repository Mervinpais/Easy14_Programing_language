using System;
using System.Windows.Forms;

namespace easy14_isde
{
    public partial class settings_form : Form
    {
        public settings_form()
        {
            InitializeComponent();
        }

        public int set_me_to_1 = 1;
        private void settings_form_Load(object sender, EventArgs e)
        {
            if (set_me_to_1 == 0)
            {
                MessageBox.Show("◻︎●︎♏︎♋︎⬧︎♏︎ ⬧︎♏︎⧫︎ ⧫︎♒︎♏︎ ⬧︎♏︎⧫︎♉︎❍︎♏︎♉︎⧫︎□︎♉︎📂︎ ❖︎♋︎❒︎♓︎♋︎♌︎●︎♏︎ ♓︎■︎ ⧫︎♒︎♏︎ ♍︎□︎♎︎♏︎ ⧫︎□︎ 📂︎ ⧫︎□︎ ♍︎□︎■︎⧫︎♓︎■︎◆︎♏︎");
                Environment.Exit(-1);
            }
        }

        private void title_label_Click(object sender, EventArgs e)
        {
            /*if (set_me_to_1 == 1)
            {
                this.BackColor = Color.Red;
                title_label.ForeColor = Color.Yellow;
            }*/

        }
    }
}
