using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProgrammingLearningApp
{
    public class Grid
    {
        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;
        private int cellSize = 50;
        public List<char> exerciseCharList { get; set; } = null;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public void Draw(Graphics g, int panelWidth, int panelHeight, int characterX, int characterY, List<Point> trail)
        {
            if (exerciseCharList != null)
            {
                DrawExerciseGrid(g);
            }
            DrawGrid(g, panelWidth, panelHeight);
            DrawTrail(g, trail);
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

        public void DrawExerciseGrid(Graphics g)
        {
            int listCount = 0;
            Brush pathBrush = Brushes.White;
            Brush barrierBrush = Brushes.Orange;
            for(int i = 0; i < this.Height; i++)
            {
                for(int j = 0; j < this.Width; j++)
                {
                    switch(exerciseCharList[listCount]) 
                    {
                        case '0':
                            g.FillRectangle(pathBrush, j * cellSize, i * cellSize, cellSize,cellSize);
                            break;
                        case 'x':
                            g.DrawLine(Pens.Green,j*cellSize,i*cellSize,(j+1)*cellSize,(i+1)*cellSize);
                            g.DrawLine(Pens.Green, (j + 1) * cellSize, i * cellSize, j * cellSize, (i + 1) * cellSize);
                            break;
                        default:
                            g.FillRectangle(barrierBrush, j * cellSize, i * cellSize, cellSize, cellSize);
                            break;
                    }
                    listCount++;
                }
            }
        }

        public void ClearExercise() //returns values to normal
        {
            this.exerciseCharList = null;
            this.Height = 10;
            this.Width = 10;
            this.cellSize = 50;
        }

        public void InitializeExercise(List<char> exerciseCharList) //changes the values to the size of the loaded exercise
        {
            this.exerciseCharList = exerciseCharList;
            this.Height = (int)Math.Sqrt((double)exerciseCharList.Count);
            this.Width = this.Height;
            this.cellSize = 500 / this.Height;
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

        private void DrawTrail(Graphics g, List<Point> trail)
        {
            Brush trailBrush = Brushes.LightBlue;

            foreach (var point in trail)
            {
                if (IsWithinBounds(point.X, point.Y))
                {
                    g.FillRectangle(trailBrush, point.X * cellSize, point.Y * cellSize, cellSize, cellSize);
                }
            }
        }
    }
}