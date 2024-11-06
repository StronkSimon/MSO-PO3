using System.Collections.Generic;
using System.Windows.Forms;

namespace ProgrammingLearningApp
{
    public class ProgramController
    {
        private Program program;
        private Character character;
        private ProgramEditor programEditor;
        private ExportManager exportManager;

        public ProgramController()
        {
            character = new Character();
            program = new Program("Sample Program");
            programEditor = new ProgramEditor(program);
            exportManager = new ExportManager();
        }

        // Loads a hardcoded sample program based on difficulty level
        public void LoadSampleProgram(string level)
        {
            program.Commands.Clear();

            if (level == "Basic")
            {
                programEditor.AddCommand(new Command(CommandType.Move, 5, character));
                programEditor.AddCommand(new Command(CommandType.Turn, 1, character));
            }
            else if (level == "Advanced")
            {
                var repeatCommand = new Command(CommandType.Repeat, 3, character);
                repeatCommand.SubCommands.Add(new Command(CommandType.Move, 2, character));
                repeatCommand.SubCommands.Add(new Command(CommandType.Turn, 1, character));
                programEditor.AddCommand(repeatCommand);
            }
            else if (level == "Expert")
            {
                var expertRepeat = new Command(CommandType.Repeat, 5, character);
                expertRepeat.SubCommands.Add(new Command(CommandType.Move, 2, character));
                programEditor.AddCommand(expertRepeat);
                programEditor.AddCommand(new Command(CommandType.Turn, -1, character));
            }
        }

        // Returns a list of command descriptions for display
        public List<string> GetCommandDisplayList()
        {
            return programEditor.GetCommandDisplayList();
        }

        // Executes the program and returns the final state of the character
        public string RunProgram()
        {
            character = new Character(); // Reset character position and direction
            program.Execute(character);
            return $"Final Position: ({character.X}, {character.Y}), Facing: {character.Direction}";
        }

        public void SaveProgram()
        {
            if (program.Commands.Count == 0)
            {
                MessageBox.Show("No Program Loaded", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                exportManager.SaveProgram(program);
            }
        }

        // Retrieves program metrics for display
        public Dictionary<string, int> GetMetrics()
        {
            return new Dictionary<string, int>
            {
                { "Total Commands", program.GetCommandCount() },
                { "   Max Nesting Level", program.GetMaxNestingLevel() },
                { "   Repeat Commands", program.GetRepeatCommandCount() }
            };
        }

        // Adds a command to the program based on the type
        public void AddCommand(CommandType type, int value)
        {
            if (type == CommandType.Repeat)
            {
                var repeatCommand = new Command(type, value, character);
                repeatCommand.SubCommands.Add(new Command(CommandType.Move, 1, character)); // Sample subcommand
                programEditor.AddCommand(repeatCommand);
            }
            else
            {
                programEditor.AddCommand(new Command(type, value, character));
            }
        }
    }
}