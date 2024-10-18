using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLearningApp
{
    public enum CommandType { Move, Turn, Repeat }

    public class Command
    {
        public CommandType Type { get; set; }
        public int Value { get; set; } // Used for Move steps or Repeat times
        public Character Character { get; set; }
        public List<Command> SubCommands { get; set; } = new List<Command>();

        public Command(CommandType type, int value, Character character)
        {
            Type = type;
            Value = value;
            Character = character;
        }

        public void Execute(Character character)
        {
            switch (Type)
            {
                case CommandType.Move:
                    character.Move(Value);
                    break;
                case CommandType.Turn:
                    if (Value == 1)
                        character.TurnRight();
                    else if (Value == -1)
                        character.TurnLeft();
                    break;
                case CommandType.Repeat:
                    for (int i = 0; i < Value; i++)
                    {
                        foreach (var subCommand in SubCommands)
                        {
                            subCommand.Execute(character);
                        }
                    }
                    break;
            }
        }
    }
}
