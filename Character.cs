using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLearningApp
{
    public class Character
    {
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public Direction Direction { get; private set; } = Direction.East;

        public void Move(int steps)
        {
            switch (Direction)
            {
                case Direction.North:
                    Y += steps;
                    break;
                case Direction.East:
                    X += steps;
                    break;
                case Direction.South:
                    Y -= steps;
                    break;
                case Direction.West:
                    X -= steps;
                    break;
            }
        }

        public void TurnLeft()
        {
            Direction = (Direction)(((int)Direction + 3) % 4);
        }

        public void TurnRight()
        {
            Direction = (Direction)(((int)Direction + 1) % 4);
        }
    }

    public enum Direction { North, East, South, West }
}
