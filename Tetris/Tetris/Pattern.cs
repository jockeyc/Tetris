using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class Pattern
    {
        public int state = 0;
        public int stateNum = 0;
        public Color color;
        public int id=0;
        public class State
        {
            public int[] i = new int[4];
            public int[] j = new int[4];
            public State(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
            {
                i[0] = x1; i[1] = x2; i[2] = x3; i[3] = x4;
                j[0] = y1; j[1] = y2; j[2] = y3; j[3] = y4;
            }
        }
        public List<Cube> cubes = new List<Cube>();
        public List<State> states = new List<State>();
        public void MoveLeft()
        {
            foreach (Cube i in cubes)
            {
                i.moveLeft();
            }
        }
        public void MoveRight()
        {
            foreach (Cube i in cubes)
            {
                i.moveRight();
            }
        }
        public void MoveDown()
        {
            foreach (Cube i in cubes)
            {
                i.moveDown();
            }
        }
        public virtual void ClockwiseRotation()
        {
            state = (state + 1) % stateNum;
            for (int x = 1; x < 4; x++)
            {
                cubes[x] = new Cube(cubes[0].i + states[state].i[x], cubes[0].j + states[state].j[x], color);
            }
        }

        public virtual void AnticlockwiseRotation()
        {
            state = (state - 1 + stateNum) % stateNum;
            for (int x = 1; x < 4; x++)
            {
                cubes[x] = new Cube(cubes[0].i + states[state].i[x], cubes[0].j + states[state].j[x], color);
            }
        }

        public virtual void drawPattern(Graphics g, int grid)
        {
            foreach (Cube x in cubes)
            {
                x.drawCube(g, grid);
            }
        }

        public virtual void init() { }
    }


    public class I : Pattern
    {
        int Centj(int state)
        {
            if (state == 1)
            {
                return 1;
            }
            else if (state == 2)
            {
                return 0;
            }
            else if (state == 3)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        int Centi(int state)
        {
            if(state == 1)
            {
                return 0;
            }
            else if(state == 2)
            {
                return -1;
            }
            else if(state == 3)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public I()
        {
            init();
            stateNum = 4;
            id = 1;
        }
        void postionStart()
        {
            cubes.Add(new Cube(5, 0, color));
            cubes.Add(new Cube(3, 0, color));
            cubes.Add(new Cube(4, 0, color));
            cubes.Add(new Cube(6, 0, color));
        }
        void setState()
        {
            states.Add(new State(0, 0, -2, 0, -1, 0, 1, 0));
            states.Add(new State(0, 0, 0, -2, 0, -1, 0, 1));
            states.Add(new State(0, 0, -1, 0, 1, 0, 2, 0));
            states.Add(new State(0, 0, 0, -1, 0, 1, 0, 2));
        }
        public override void init()
        {
            color = Color.SkyBlue;
            postionStart();
            setState();
        }
        public override void ClockwiseRotation()
        {
            if (state == 1 && cubes[0].i == 1)
            {
                MoveRight();
            }
            else if (state == 3 && cubes[0].i == 8)
            {
                MoveLeft();
            }
            state = (state + 1) % stateNum;
            cubes[0].i += Centi(state);
            cubes[0].j += Centj(state);
            for (int x = 1; x < 4; x++)
            {
                cubes[x] = new Cube(cubes[0].i + states[state].i[x], cubes[0].j + states[state].j[x], color);
            }
            
        }
        public override void AnticlockwiseRotation()
        {
            cubes[0].i -= Centi(state);
            cubes[0].j -= Centj(state);
            state = (state - 1 + stateNum) % stateNum;            
            for (int x = 1; x < 4; x++)
            {
                cubes[x] = new Cube(cubes[0].i + states[state].i[x], cubes[0].j + states[state].j[x], color);
            }
        }
    }

    public class L : Pattern
    {
        public L()
        {
            init();
            stateNum = 4;
            id = 4;
        }
        void postionStart()
        {
            cubes.Add(new Cube(4, 1, color));
            cubes.Add(new Cube(3, 1, color));
            cubes.Add(new Cube(5, 1, color));
            cubes.Add(new Cube(5, 0, color));
        }
        void setState()
        {
            states.Add(new State(0, 0, -1, 0, 1, 0, 1, -1));
            states.Add(new State(0, 0, 0, -1, 0, 1, 1, 1));
            states.Add(new State(0, 0, -1, 0, 1, 0, -1, 1));
            states.Add(new State(0, 0, 0, -1, -1, -1, 0, 1));
        }
        public override void init()
        {
            color = Color.Coral;
            postionStart();
            setState();
        }
    }

    public class O : Pattern
    {
        public O()
        {
            init();
            stateNum = 1;
            id = 3;
        }
        void postionStart()
        {
            cubes.Add(new Cube(4, 1, color));
            cubes.Add(new Cube(5, 1, color));
            cubes.Add(new Cube(4, 0, color));
            cubes.Add(new Cube(5, 0, color));
        }
        void setState()
        {
            states.Add(new State(0, 0, 1, 0, 0, -1, 1, -1));
        }
        public override void init()
        {
            color = Color.Yellow;
            postionStart();
            setState();
        }
    }

    public class J : Pattern
    {
        public J()
        {
            init();
            stateNum = 4;
            id = 5;
        }
        void postionStart()
        {
            cubes.Add(new Cube(4, 1, color));
            cubes.Add(new Cube(3, 1, color));
            cubes.Add(new Cube(3, 0, color));
            cubes.Add(new Cube(5, 1, color));
        }
        void setState()
        {
            states.Add(new State(0, 0, -1, 0, -1, -1, 1, 0));
            states.Add(new State(0, 0, 0, -1, 1, -1, 0, 1));
            states.Add(new State(0, 0, -1, 0, 1, 0, 1, 1));
            states.Add(new State(0, 0, 0, -1, 0, 1, -1, 1));
        }
        public override void init()
        {
            color = Color.RoyalBlue;
            postionStart();
            setState();
        }
    }

    public class T : Pattern
    {
        public T()
        {
            init();
            stateNum = 4;
            id = 2;
        }
        void postionStart()
        {
            cubes.Add(new Cube(4, 1, color));
            cubes.Add(new Cube(3, 1, color));
            cubes.Add(new Cube(5, 1, color));
            cubes.Add(new Cube(4, 0, color));
        }
        void setState()
        {
            states.Add(new State(0, 0, -1, 0, 1, 0, 0, -1));
            states.Add(new State(0, 0, 0, -1, 0, 1, 1, 0));
            states.Add(new State(0, 0, -1, 0, 1, 0, 0, 1));
            states.Add(new State(0, 0, 0, -1, 0, 1, -1, 0));
        }
        public override void init()
        {
            color = Color.DarkViolet;
            postionStart();
            setState();
        }
    }

    public class S : Pattern
    {
        public S()
        {
            init();
            stateNum = 4;
            id = 6;
        }
        void postionStart()
        {
            cubes.Add(new Cube(4, 1, color));
            cubes.Add(new Cube(3, 1, color));
            cubes.Add(new Cube(4, 0, color));
            cubes.Add(new Cube(5, 0, color));
        }
        void setState()
        {
            states.Add(new State(0, 0, -1, 0, 0, -1, 1, -1));
            states.Add(new State(0, 0, 0, -1, 1, 0, 1, 1));
            states.Add(new State(0, 0, 1, 0, 0, 1, -1, 1));
            states.Add(new State(0, 0, -1, 0, -1, -1, 0, 1));
        }
        public override void init()
        {
            color = Color.Chartreuse;
            postionStart();
            setState();
        }
    }

    public class Z : Pattern
    {
        public Z()
        {
            init();
            stateNum = 4;
            id = 7;
        }
        void postionStart()
        {
            cubes.Add(new Cube(4, 1, color));
            cubes.Add(new Cube(5, 1, color));
            cubes.Add(new Cube(3, 0, color));
            cubes.Add(new Cube(4, 0, color));
        }
        void setState()
        {
            states.Add(new State(0, 0, 1, 0, -1, -1, 0, -1));
            states.Add(new State(0, 0, 0, 1, 1, 0, 1, -1));
            states.Add(new State(0, 0, -1, 0, 0, 1, 1, 1));
            states.Add(new State(0, 0, 0, -1, -1, 0, -1, 1));
        }
        public override void init()
        {
            color = Color.OrangeRed;
            postionStart();
            setState();
        }
    }

}
