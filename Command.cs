using System.Collections.Generic;

namespace ProgrammingLearningApp
{
    public enum CommandType { Move, Turn, Repeat }

    public class Command
    {
        private static int idCounter = 0;
        public int Id { get; private set; }
        public CommandType Type { get; set; }
        public int Value { get; set; }
        public Character Character { get; set; }
        public List<Command> SubCommands { get; set; }

        public Command(CommandType type, int value, Character character)
        {
            Id = idCounter++;
            Type = type;
            Value = value;
            Character = character;
            SubCommands = new List<Command>();
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