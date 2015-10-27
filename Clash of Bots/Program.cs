using Clash_of_Bots.Forms;
using System;
using System.Windows.Forms;

namespace Clash_of_Bots
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
