using System;
using System.Drawing;

namespace ProgrammingLearningApp
{
    public class Grid
    {
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;
        private const int cellSize = 50;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public void Draw(Graphics g, int panelWidth, int panelHeight, int characterX, int characterY)
        {
            DrawGrid(g, panelWidth, panelHeight);
            DrawCharacter(g, characterX, characterY);
        }

        private void DrawGrid(Graphics g, int panelWidth, int panelHeight)
        {
            Pen gridPen = Pens.Black;

            for (int x = 0; x <= Width; x++)
            {
                g.DrawLine(gridPen, x * cellSize, 0, x * cellSize, Height * cellSize);
            }

            for (int y = 0; y <= Height; y++)
            {
                g.DrawLine(gridPen, 0, y * cellSize, Width * cellSize, y * cellSize);
            }
        }

        private void DrawCharacter(Graphics g, int characterX, int characterY)
        {
            if (IsWithinBounds(characterX, characterY))
            {
                int centerX = characterX * cellSize + cellSize / 2;
                int centerY = characterY * cellSize + cellSize / 2;

                // Draw a green cross at the character's position
                g.DrawLine(Pens.Green, centerX - 5, centerY, centerX + 5, centerY);
                g.DrawLine(Pens.Green, centerX, centerY - 5, centerX, centerY + 5);
            }
        }
    }
}