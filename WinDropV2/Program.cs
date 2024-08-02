using System;
using System.Windows.Forms;

namespace WinDropV2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new ServerInstance(9988);
            Application.Run(new Form1());
        }
    }
}