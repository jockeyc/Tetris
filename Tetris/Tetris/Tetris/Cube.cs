using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Cube
    {
        public int grid = 32;
        public Color color;
        public int i, j;
        public Cube(int x, int y, Color c)
        {
            i = x; j = y;
            color = c;
        }
        public void moveLeft()
        {
            i--;
        }
        public void moveRight()
        {
            i++;
        }
        public void moveDown()
        {
            j++;
        }
        public void moveUp()
        {
            j--;
        }
        public void drawCube(Graphics g, int grid)
        {
            g.FillRectangle(new SolidBrush(color), new Rectangle(i * grid, j * grid, grid, grid));
        }
    }
}
