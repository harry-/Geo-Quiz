using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace GeoQuiz
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();

            Debug.WriteLine(typeof(string).Assembly.ImageRuntimeVersion);

            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GeoQuiz());
        }
    }
}
