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
        public bool BorderCheck(Pattren p)
        {
            foreach(Cube cube in p.cubes)
            {
                if (cube.i < 0 || cube.i >= 10 || cube.j < 0)
                    return false;
            }
            return true;
        }
        //无法下降返回false
        public bool MoveDownCheck(Pattren p, Cube[,] canvus)
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
        public bool CollideCheck(Pattren p, Cube[,] canvus)
        {
            foreach(Cube cube in p.cubes)
            {
                if (canvus[cube.j, cube.i] != null)
                    return false;
            }
            return true;
        }
        //游戏结束返回ture
        public bool EndCheck(Pattren p, Cube[,] canvus)
        {
            if(!CollideCheck(p, canvus))
            {
                for(int x = 0; x < 4; x++)
                {
                    if(p.cubes[x].j == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //固定
        public void FixCube(Pattren p, Cube[,] canvus)
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
        //炸弹
        public void Bombard(Pattren p, Cube[,] canvus)
        {
            int xx, yy;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    xx = p.cubes[0].i + x;
                    yy = p.cubes[0].j + y;
                    if (xx >= 0 && xx <= 9 && yy >= 0 && yy <= 19)
                        canvus[yy, xx] = null;
                }
            }
        }
        public Pattren RandomPattern()
        {
            int p = new Random(Guid.NewGuid().GetHashCode()).Next(0,10000)%8;
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
        public Pattren RandomPattern2()
        {
            int p = new Random(Guid.NewGuid().GetHashCode()).Next(0, 10000) % 15;
            //int p = new Random(Guid.NewGuid().GetHashCode()).Next(0, 10000) % 2;
            if (p == 1 || p == 8)
            {
                return new I();
            }
            else if (p == 2 || p == 9)
            {
                return new T();
            }
            else if (p == 3 || p == 10)
            {
                return new O();
            }
            else if (p == 4 || p == 11)
            {
                return new L();
            }
            else if (p == 5 || p == 12)
            {
                return new J();
            }
            else if (p == 6 || p == 13)
            {
                return new S();
            }
            else if (p == 7 || p == 14)
            {
                return new Z();
            }
            else
            {
                return new Bomb();
            }
        }
    }
}
