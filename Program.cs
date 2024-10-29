using System;
using System.Collections.Generic;

namespace ProgrammingLearningApp
{
    public class Program
    {
        public string Name { get; set; }
        public List<Command> Commands { get; set; }

        // Optional parameter to initialize with commands
        public Program(string name, List<Command> commands = null)
        {
            Name = name;
            Commands = commands ?? new List<Command>();
        }

        public void Execute(Character character)
        {
            foreach (var command in Commands)
            {
                command.Execute(character);
            }
        }

        public int GetCommandCount() => Commands.Count;
        public int GetMaxNestingLevel() => GetMaxNestingLevel(Commands);
        public int GetRepeatCommandCount() => Commands.FindAll(cmd => cmd.Type == CommandType.Repeat).Count;

        private int GetMaxNestingLevel(List<Command> commands, int level = 0)
        {
            int maxLevel = level;
            foreach (var command in commands)
            {
                if (command.Type == CommandType.Repeat)
                {
                    int nestedLevel = GetMaxNestingLevel(command.SubCommands, level + 1);
                    maxLevel = Math.Max(maxLevel, nestedLevel);
                }
            }
            return maxLevel;
        }

        // New method to get execution trace
        public List<string> GetExecutionTrace()
        {
            List<string> trace = new List<string>();
            foreach (var command in Commands)
            {
                trace.Add(command.ToString());
            }
            return trace;
        }
    }
}