using System.Collections.Generic;
using System.Windows.Forms;
﻿using System;
using System.Collections.Generic;

namespace ProgrammingLearningApp
{
    public class ProgramController
    {
        private Character character;
        private ProgramEditor programEditor;
        private ExportManager exportManager;
        private readonly Program program;
        private readonly Character character;
        private readonly Grid grid;

        public ProgramController()
        {
            grid = new Grid(10, 10); // Initialize a 10x10 grid
            character = new Character(grid); // Pass the grid to the character
            program = new Program("Sample Program");
            programEditor = new ProgramEditor(program);
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