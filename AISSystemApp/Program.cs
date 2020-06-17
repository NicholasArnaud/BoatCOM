using System;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Security.Permissions;
using System.Windows.Forms;

namespace AISDisplay
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
            Application.Run(new Form1());

        }

    }
}