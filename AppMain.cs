using System;
using System.Windows.Forms;

namespace ProgrammingLearningApp
{
    static class AppMain
    {
        [STAThread]
        static void Main()
        {
            // Enable visual styles and text rendering compatibility for the form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the UIManager form as the main application window
            Application.Run(new UIManager());
        }
    }
}