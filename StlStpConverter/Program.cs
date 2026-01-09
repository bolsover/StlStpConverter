using System;
using System.Windows.Forms;

namespace Bolsover
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry cartesianPoint for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StlStpForm());
        }
    }
}