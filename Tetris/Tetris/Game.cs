using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    class Game
    {
        //超出边界返回false
        public bool BorderCheck(Pattern p)
        {
            foreach(Cube cube in p.cubes)
            {
                if (cube.i < 0 || cube.i >= 10)
                    return false;
            }
            return true;
        }
        //无法下降返回false
        public bool MoveDownCheck(Pattern p, Cube[,] canvus)
        {
            foreach (Cube cube in p.cubes)
            {
                if (cube.j == -1)
                    continue;
                if (cube.j == 19 || canvus[cube.j + 1, cube.i] != null)
                    return false;
            }
            return true;
        }
        //发生碰撞返回false
        public bool CollideCheck(Pattern p, Cube[,] canvus)
        {
            foreach(Cube cube in p.cubes)
            {
                if (canvus[cube.j < 0 ? 0:cube.j, cube.i] != null)
                    return false;
            }
            return true;
        }
        //固定
        public void FixCube(Pattern p, Cube[,] canvus)
        {
            if(!MoveDownCheck(p,canvus))
            {
                foreach(Cube cube in p.cubes)
                {
                    canvus[cube.j, cube.i] = cube;
                }
            }
        }
        //清除
        public void ClearLine(Cube[,] canvus, ref int score)
        {
            int flag;
            for (int i = 19; i >= 1; i--)
            {
                flag = 0;
                for (int j = 0; j < 10; j++)
                {
                    if (canvus[i, j] != null)
                    {
                        flag++;
                    }
                }
                if(flag == 10)
                {
                    for(int p = i; p >= 1; p--)
                    {
                        for(int j = 0; j < 10; j++)
                        {
                            if (canvus[p - 1, j] != null)
                                canvus[p, j] = new Cube(j, p, canvus[p - 1, j].color);
                            else
                                canvus[p, j] = null;
                        }
                    }
                    i++;
                    for(int j = 0; j < 10; j++)
                    {
                        canvus[0, j] = null;
                    }
                    score += 100;
                }
            }
        }

        public Pattern RandomPattern()
        {
            int p = new Random().Next(0,10000)%8;
            if(p == 1)
            {
                return new I();
            }
            else if (p == 2)
            {
                return new T();
            }
            else if (p == 3)
            {
                return new O();
            }
            else if (p == 4)
            {
                return new L();
            }
            else if (p == 5)
            {
                return new J();
            }
            else if (p == 6)
            {
                return new S();
            }
            else
            {
                return new Z();
            }
        }

        public Pattern GetPatternById(int p)
        {
            if (p == 1)
            {
                return new I();
            }
            else if (p == 2)
            {
                return new T();
            }
            else if (p == 3)
            {
                return new O();
            }
            else if (p == 4)
            {
                return new L();
            }
            else if (p == 5)
            {
                return new J();
            }
            else if (p == 6)
            {
                return new S();
            }
            else
            {
                return new Z();
            }
        }
    }
}
