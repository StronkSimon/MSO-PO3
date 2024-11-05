using System;
using System.Collections.Generic;

namespace ProgrammingLearningApp
{
    public class ProgramController
    {
        private Program program;
        private Character character;
        private ProgramEditor programEditor;

        public ProgramController()
        {
            character = new Character();
            program = new Program("Sample Program");
            programEditor = new ProgramEditor(program);
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

        public List<Command> GetCommandDisplayList()
        {
            return program.Commands;
        }

        // Executes the program and returns the final state of the character
        public string RunProgram()
        {
            character = new Character(); // Reset character position and direction
            program.Execute(character);
            return $"Final Position: ({character.X}, {character.Y}), Facing: {character.Direction}";
        }

        // Retrieves program metrics for display
        public Dictionary<string, int> GetMetrics()
        {
            return new Dictionary<string, int>
            {
                { "Total Commands", program.GetCommandCount() },
                { "Max Nesting Level", program.GetMaxNestingLevel() },
                { "Repeat Commands", program.GetRepeatCommandCount() }
            };
        }

        // Adds a command to the program based on the type
        public int AddCommand(CommandType type, int value, int? parentId = null)
        {
            Command newCommand = type switch
            {
                CommandType.Move => new Command(CommandType.Move, value, character),
                CommandType.Turn => new Command(CommandType.Turn, value, character),
                CommandType.Repeat => new Command(CommandType.Repeat, value, character),
                _ => throw new ArgumentException("Invalid command type")
            };

            if (parentId.HasValue)
            {
                // If parentId is provided, add the command as a subcommand to the specified Repeat command
                Command parentCommand = program.Commands.Find(c => c.Id == parentId.Value && c.Type == CommandType.Repeat);
                if (parentCommand != null)
                {
                    parentCommand.SubCommands.Add(newCommand);
                }
            }
            else
            {
                // Add as a top-level command
                program.Commands.Add(newCommand);
            }

            return newCommand.Id; // Return the command's unique ID
        }

        public void DeleteCommand(int commandId)
        {
            Command commandToRemove = program.Commands.Find(c => c.Id == commandId);
            if (commandToRemove != null)
            {
                program.Commands.Remove(commandToRemove);
            }
        }

        public void AddSubCommand(int repeatCommandId, int subCommandId)
        {
            Command repeatCommand = program.Commands.Find(c => c.Id == repeatCommandId);
            Command subCommand = program.Commands.Find(c => c.Id == subCommandId);

            if (repeatCommand?.Type == CommandType.Repeat && subCommand != null)
            {
                repeatCommand.SubCommands.Add(subCommand);
            }
        }

        public void DeleteSubCommand(int parentId, int subCommandId)
        {
            Command parentCommand = program.Commands.Find(c => c.Id == parentId && c.Type == CommandType.Repeat);
            if (parentCommand != null)
            {
                Command subCommandToRemove = parentCommand.SubCommands.Find(c => c.Id == subCommandId);
                if (subCommandToRemove != null)
                {
                    parentCommand.SubCommands.Remove(subCommandToRemove);
                }
            }
        }

        public void UpdateCommandValue(int commandId, int newValue)
        {
            // Find the command with the specified ID and update its value
            var command = program.Commands.Find(c => c.Id == commandId);
            if (command != null)
            {
                command.Value = newValue;
            }
        }
    }
}