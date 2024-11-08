using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProgrammingLearningApp
{
    public class Character
    {
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public Direction Direction { get; private set; } = Direction.East;
        public Grid grid;

        public List<Point> Trail { get; private set; } = new List<Point>();

        public Character(Grid grid)
        {
            this.grid = grid;
            Trail.Add(new Point(X, Y));
        }

        public void Move(int steps)
        {
            int newX = X;
            int newY = Y;

            switch (Direction)
            {
                case Direction.North:
                    newY -= steps;
                    break;
                case Direction.East:
                    newX += steps;
                    break;
                case Direction.South:
                    newY += steps;
                    break;
                case Direction.West:
                    newX -= steps;
                    break;
            }

            // Check if the new position is within the grid boundaries
            if (grid.IsWithinBounds(newX, newY))
            {
                // Record all intermediate steps
                while (X != newX || Y != newY)
                {
                    if (X < newX) X++;
                    else if (X > newX) X--;

                    if (Y < newY) Y++;
                    else if (Y > newY) Y--;

                    // Add the intermediate position to the trail
                    Trail.Add(new Point(X, Y));
                }
            }
            else
            {
                MessageBox.Show("Move blocked: Character is at the grid boundary.");
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
            Trail.Clear();
            Trail.Add(new Point(X, Y));
        }
    }

    public enum Direction { North, East, South, West }
}