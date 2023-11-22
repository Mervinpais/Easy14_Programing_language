using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easy14_SE
{
    public partial class SE_Window : Form
    {
        public SE_Window()
        {
            InitializeComponent();
        }

        public void OpenFile(string fileName)
        {
            this.Text = fileName;
            CodeEditorRTB.Text = string.Join(Environment.NewLine, File.ReadAllLines(fileName));
        }

        private void OpenBTN_Click(object sender, EventArgs e)
        {

        }
    }
}
