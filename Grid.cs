using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingLearningApp
{
    public class Grid
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
