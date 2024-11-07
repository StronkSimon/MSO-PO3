using System.Collections.Generic;
using System.Windows.Forms;
﻿using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace ProgrammingLearningApp
{
    public class ProgramController
    {
        private ExportManager exportManager;
        private readonly Program program;
        private readonly Character character;
        private readonly Grid grid;

        public ProgramController()
        {
            grid = new Grid(10, 10); // Initialize a 10x10 grid
            character = new Character(grid); // Pass the grid to the character
            program = new Program("Sample Program");
            exportManager = new ExportManager();
        }

        public Character Character => character; // Expose character to UIManager

        // Loads a hardcoded sample program based on difficulty level
        public void LoadSampleProgram(string level)
        {
            program.Commands.Clear();

            if (level == "Basic")
            {
                AddCommand(CommandType.Move, 5);
                AddCommand(CommandType.Turn, 1);
            }
            else if (level == "Advanced")
            {
                int repeatCommandId = AddCommand(CommandType.Repeat, 3);
                Command repeatCommand = program.Commands.Find(c => c.Id == repeatCommandId);
                if (repeatCommand != null && repeatCommand.Type == CommandType.Repeat)
                {
                    repeatCommand.SubCommands.Add(new Command(CommandType.Move, 2, character));
                    repeatCommand.SubCommands.Add(new Command(CommandType.Turn, 1, character));
                }
            }
            else if (level == "Expert")
            {
                int expertRepeatCommandId = AddCommand(CommandType.Repeat, 5);

                Command expertRepeatCommand = program.Commands.Find(c => c.Id == expertRepeatCommandId);
                if (expertRepeatCommand != null && expertRepeatCommand.Type == CommandType.Repeat)
                {
                    expertRepeatCommand.SubCommands.Add(new Command(CommandType.Move, 2, character));
                }

                AddCommand(CommandType.Turn, -1);
            }
        }

        public List<Command> GetCommandDisplayList()
        {
            return program.Commands;
        }

        // Executes the program and returns the final state of the character
        public string RunProgram()
        {
            character.Reset(); // Reset character position and direction without reassigning
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
                Command parentCommand = program.Commands.Find(c => c.Id == parentId.Value && c.Type == CommandType.Repeat);
                parentCommand?.SubCommands.Add(newCommand);
            }
            else
            {
                program.Commands.Add(newCommand);
            }

            return newCommand.Id;
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
            Command subCommand = program.Commands.Find(c => c.Id == subCommandId)
                                 ?? program.Commands.SelectMany(cmd => cmd.SubCommands).FirstOrDefault(c => c.Id == subCommandId);

            if (repeatCommand?.Type == CommandType.Repeat && subCommand != null)
            {
                // Remove subcommand from top-level if it exists
                program.Commands.Remove(subCommand);

                // Add the subcommand to the repeat command's subcommand list
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

        public async Task RunProgramWithDelay(int delayMs, Action onUpdate)
        {
            character.Reset(); // Reset character position and direction
            foreach (var command in program.Commands)
            {
                command.Execute(character);
                onUpdate(); // Trigger UI update after each command
                await Task.Delay(delayMs); // Delay between commands
            }
        }

        public void LoadProgramFromFile(string filePath)
        {
            program.Commands.Clear(); // Clear existing commands before loading new ones

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(' ');
                    if (parts.Length > 0)
                    {
                        CommandType commandType;
                        int value = 1; // Default value for Turn commands

                        switch (parts[0].ToLower())
                        {
                            case "move":
                                commandType = CommandType.Move;
                                if (parts.Length > 1 && int.TryParse(parts[1], out int moveValue))
                                {
                                    value = moveValue;
                                }
                                break;

                            case "turn":
                                commandType = CommandType.Turn;
                                if (parts.Length > 1)
                                {
                                    value = parts[1].ToLower() == "right" ? 1 : -1;
                                }
                                break;

                            case "repeat":
                                commandType = CommandType.Repeat;
                                if (parts.Length > 1 && int.TryParse(parts[1], out int repeatValue))
                                {
                                    value = repeatValue;
                                }
                                break;

                            default:
                                continue; // Ignore unrecognized commands
                        }

                        // Add the parsed command to the program
                        AddCommand(commandType, value);
                    }
                }
            }
        }
    }
}