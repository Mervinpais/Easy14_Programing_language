using System;
using System.Threading;
using System.Windows.Forms;

namespace Easy14_Programming_Language
{
    public static class KeyboardPressKey
    {
        [STAThread]
        public static void PressKeyEnter()
        {
            SendKeys.SendWait(Keys.Enter.ToString());
            Thread.Sleep(5000);
        }
    }
}