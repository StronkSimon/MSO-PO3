using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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
                StreamWriter writer = new StreamWriter(dialog.FileName);
                writer.Write(Translate(program.Commands));
                writer.Close();
            }
        }

        public static string Translate(List<Command> commands)
        {
            var builder = new StringBuilder();
            TranslateCommands(commands, builder, 0);
            return builder.ToString();
        }

        private static void TranslateCommands(List<Command> commands, StringBuilder builder, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4); // 4 spaces per indent level

            foreach (var command in commands)
            {
                switch (command.Type)
                {
                    case CommandType.Move:
                        builder.AppendLine($"{indent}Move {command.Value}");
                        break;

                    case CommandType.Turn:
                        string turnDirection = command.Value == 1 ? "Turn right" : "Turn left";
                        builder.AppendLine($"{indent}{turnDirection}");
                        break;

                    case CommandType.Repeat:
                        builder.AppendLine($"{indent}Repeat {command.Value}");
                        TranslateCommands(command.SubCommands, builder, indentLevel + 1);
                        break;

                    default:
                        throw new InvalidOperationException("Unknown command type");
                }
            }
        }
    }
}
