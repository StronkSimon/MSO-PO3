using System;

namespace ProgrammingLearningApp
{
    public class Character
    {
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public Direction Direction { get; private set; } = Direction.East;
        private readonly Grid grid;

        public Character(Grid grid)
        {
            this.grid = grid;
        }

        public void Move(int steps)
        {
            int newX = X;
            int newY = Y;

            switch (Direction)
            {
                case Direction.North:
                    newY += steps;
                    break;
                case Direction.East:
                    newX += steps;
                    break;
                case Direction.South:
                    newY -= steps;
                    break;
                case Direction.West:
                    newX -= steps;
                    break;
            }

            // Check if the new position is within the grid boundaries
            if (grid.IsWithinBounds(newX, newY))
            {
                X = newX;
                Y = newY;
            }
            else
            {
                Console.WriteLine("Move blocked: Character is at the grid boundary.");
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

        public void Reset()
        {
            X = 0;
            Y = 0;
            Direction = Direction.East;
        }
    }

    public enum Direction { North, East, South, West }
}