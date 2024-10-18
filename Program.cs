using System;
using System.Collections.Generic;
using System.Data;

namespace ProgrammingLearningApp
{
    public class Program
    {
        public string Name { get; set; }
        public List<Command> Commands { get; set; }

        public Program(string name)
        {
            Name = name;
            Commands = new List<Command>();
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
    }
}