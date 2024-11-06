using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgrammingLearningApp
{
    internal class ExportManager
    {
        public void SaveProgram(Program program)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Textfiles|*.txt";
            dialog.Title = "Save text as";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
               
            }
        }
    }
}
