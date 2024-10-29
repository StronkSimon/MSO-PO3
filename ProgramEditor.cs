using System;
using System.Collections.Generic;

namespace ProgrammingLearningApp
{
    public class ProgramEditor
    {
        private Program program;

        public ProgramEditor(Program program)
        {
            this.program = program ?? throw new ArgumentNullException(nameof(program));
        }

        /// <summary>
        /// Adds a new command to the program.
        /// </summary>
        public void AddCommand(Command command, int? repeatIndex = null)
        {
            if (repeatIndex.HasValue && repeatIndex.Value >= 0 && repeatIndex.Value < program.Commands.Count &&
                program.Commands[repeatIndex.Value].Type == CommandType.Repeat)
            {
                // If within a Repeat command, add as a subcommand
                program.Commands[repeatIndex.Value].SubCommands.Add(command);
            }
            else
            {
                // Add at the top level
                program.Commands.Add(command);
            }
        }

        /// <summary>
        /// Deletes a command at the specified index.
        /// </summary>
        public void DeleteCommand(int index, int? repeatIndex = null)
        {
            if (repeatIndex.HasValue && repeatIndex.Value >= 0 && repeatIndex.Value < program.Commands.Count &&
                program.Commands[repeatIndex.Value].Type == CommandType.Repeat)
            {
                // Delete from a Repeat command's subcommands
                var repeatCommand = program.Commands[repeatIndex.Value];
                if (index >= 0 && index < repeatCommand.SubCommands.Count)
                {
                    repeatCommand.SubCommands.RemoveAt(index);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Subcommand index is out of range.");
                }
            }
            else
            {
                // Delete from top-level commands
                if (index >= 0 && index < program.Commands.Count)
                {
                    program.Commands.RemoveAt(index);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Command index is out of range.");
                }
            }
        }

        /// <summary>
        /// Moves a command from one index to another within the top-level or Repeat command.
        /// </summary>
        public void MoveCommand(int fromIndex, int toIndex, int? repeatIndex = null)
        {
            if (repeatIndex.HasValue && repeatIndex.Value >= 0 && repeatIndex.Value < program.Commands.Count &&
                program.Commands[repeatIndex.Value].Type == CommandType.Repeat)
            {
                // Move within a Repeat command's subcommands
                var repeatCommand = program.Commands[repeatIndex.Value];
                if (fromIndex >= 0 && fromIndex < repeatCommand.SubCommands.Count &&
                    toIndex >= 0 && toIndex < repeatCommand.SubCommands.Count)
                {
                    var command = repeatCommand.SubCommands[fromIndex];
                    repeatCommand.SubCommands.RemoveAt(fromIndex);
                    repeatCommand.SubCommands.Insert(toIndex, command);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Subcommand index is out of range.");
                }
            }
            else
            {
                // Move within top-level commands
                if (fromIndex >= 0 && fromIndex < program.Commands.Count &&
                    toIndex >= 0 && toIndex < program.Commands.Count)
                {
                    var command = program.Commands[fromIndex];
                    program.Commands.RemoveAt(fromIndex);
                    program.Commands.Insert(toIndex, command);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Command index is out of range.");
                }
            }
        }

        /// <summary>
        /// Retrieves the list of commands for display purposes.
        /// </summary>
        public List<string> GetCommandDisplayList()
        {
            List<string> displayList = new List<string>();
            foreach (var command in program.Commands)
            {
                displayList.Add(command.ToString());
                if (command.Type == CommandType.Repeat)
                {
                    foreach (var subCommand in command.SubCommands)
                    {
                        displayList.Add("    " + subCommand.ToString());
                    }
                }
            }
            return displayList;
        }
    }
}