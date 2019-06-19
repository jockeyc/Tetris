using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace Tetris
{
    public partial class LocalDoubleForm : Form
    {
        int Row = 20, Col = 10, grid = 20;
        int lscore = 0, rscore = 0;
        bool stop;
        PaintEventArgs lpe,rpe;
        Pattern lcurrent, rcurrent;
        Pattern lnext, rnext;
        Cube[,] lcanvus = new Cube[20, 10];
        Cube[,] rcanvus = new Cube[20, 10];
        Game lgame = new Game();
        Game rgame = new Game();
        public LocalDoubleForm()
        {
            InitializeComponent();
            panel1.BackColor = Color.LightSlateGray;
            pictureBox1.Focus();
            pictureBox2.Focus();
            lpe = new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.pictureBox1.Size.Width, this.pictureBox1.Size.Height));
            rpe = new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.pictureBox1.Size.Width, this.pictureBox1.Size.Height));
            lcurrent = lgame.RandomPattern();
            lnext = lgame.RandomPattern();
            rcurrent = rgame.RandomPattern();
            rnext = rgame.RandomPattern();
        }
        private void LocalDoubleForm_Load(object sender, EventArgs e)
        {
            stop = false;
            stop = false;
            timer1.Enabled = true;
            panel1.Visible = false;
            panel1.Enabled = false;
            All_paint(lpe);
            All_paint(rpe);
            Next_paint(pictureBox3, lnext);
            Next_paint(pictureBox4, rnext);
        }


         
        /// <summary>
        /// 每一次刷新所需要的所有方法
        /// </summary>
        private void All_paint(PaintEventArgs e)
        {
            Bitmap lbp= new Bitmap(this.pictureBox1.Size.Width, this.pictureBox1.Size.Height);
            Bitmap rbp = new Bitmap(this.pictureBox2.Size.Width, this.pictureBox2.Size.Height);
            Graphics lbg = Graphics.FromImage(lbp);
            Graphics rbg = Graphics.FromImage(rbp);
            Current_Paint(lcurrent, lbg);
            Current_Paint(rcurrent, rbg);
            Canvus_Paint(lcanvus,lbg);
            Canvus_Paint(rcanvus, rbg);
            Grid_Paint(lbg);
            Grid_Paint(rbg);
            this.pictureBox1.BackgroundImage = lbp;
            this.pictureBox2.BackgroundImage = rbp;
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            Next_paint(pictureBox3,lnext);
            Next_paint(pictureBox4,rnext);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(lgame.MoveDownCheck(lcurrent,lcanvus))
            {
                lcurrent.MoveDown();
            }
            else
            {
                lgame.FixCube(lcurrent, lcanvus);
                lgame.ClearLine(lcanvus, ref lscore);
                pictureBox1.Refresh();
                lcurrent = lnext;
                lnext = lgame.RandomPattern();
                if (timer1.Enabled == true) End_Paint();
                Next_paint(pictureBox3,lnext);
                Score_Paint("左",label1, lscore);
                
            }
            All_paint(lpe);
            if(rgame.MoveDownCheck(rcurrent,rcanvus))
            {
                rcurrent.MoveDown(); 
            }
            else
            {
                rgame.FixCube(rcurrent, rcanvus);
                rgame.ClearLine(rcanvus, ref rscore);
                pictureBox2.Refresh();
                rcurrent = rnext;
                rnext = rgame.RandomPattern();
                if(timer1.Enabled==true) End_Paint();
                Next_paint(pictureBox4,rnext);
                Score_Paint("右",label2, rscore);
            }
            All_paint(rpe);
        }

        /// <summary>
        /// 画出当前块
        /// </summary>
        private void Current_Paint(Pattern pt,Graphics g)
        {
            pt.drawPattern(g, grid);
        }

        //返回主菜单
        private void button3_Click(object sender, EventArgs e)
        {
            StartForm startForm = new StartForm();
            this.Hide();
            startForm.Location = this.Location;
            startForm.ShowDialog();
            Application.ExitThread();
            this.Dispose();
        }
        //重新开始
        private void button2_Click(object sender, EventArgs e)
        {
            lcanvus = new Cube[20, 10];
            lcurrent = lgame.RandomPattern2();
            lnext = lgame.RandomPattern2();
            lscore = 0;
            stop = false;
            Next_paint(pictureBox3, lnext);
            All_paint(lpe);

            rcanvus = new Cube[20, 10];
            rcurrent = rgame.RandomPattern2();
            rnext = rgame.RandomPattern2();
            rscore = 0;
            stop = false;
            Next_paint(pictureBox4, rnext);
            All_paint(rpe);

            panel1.Visible = false;
            panel1.Enabled = false;
            timer1.Enabled = true;
        }
        //继续
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            panel1.Visible = false;
            panel1.Enabled = false;
            stop = false;
        }

        /// <summary>
        /// 绘制下一块
        /// </summary>
        private void Next_paint(PictureBox pb,Pattern pt)
        {
            Bitmap bp = new Bitmap(pb.Size.Width,pb.Size.Height);
            Graphics bg = Graphics.FromImage(bp);
            for (int i = 0; i < 4; i++)
                pt.cubes[i].j += 2;
            pt.drawPattern(bg, grid);
            for (int i = 0; i < 4; i++)
                pt.cubes[i].j -= 2;
            for (int i = 0; i <= 4; i++)
                bg.DrawLine(new Pen(Color.LightSlateGray), 3 * grid, i * grid, 7 * grid, i * grid);
            for (int i = 0; i <= 4; i++)
                bg.DrawLine(new Pen(Color.LightSlateGray), (i + 3) * grid, 0, (i + 3) * grid, 4 * grid);

            pb.BackgroundImage = bp;
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="g">绘制的接口</param>
        private void Grid_Paint(Graphics g)
        {
            for(int i=0;i<=Row;i++)
            {
                g.DrawLine(new Pen(Color.LightSlateGray), 0, i * grid, Col * grid, i * grid);
            }
            for(int i=0;i<=Col;i++)
            {
                g.DrawLine(new Pen(Color.LightSlateGray), i * grid, 0, i * grid, Row * grid);
            }
        }

        /// <summary>
        /// 分数显示
        /// </summary>
        private void Score_Paint(string pla,Label tlabel,int tscore)
        {
            tlabel.Text = pla+"分数：" + tscore.ToString();
        }
        /// <summary>
        /// 键盘监听
        /// </summary>
        private void LocalDoubleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                lcurrent.MoveLeft();
                if (!lgame.BorderCheck(lcurrent) || !lgame.CollideCheck(lcurrent, lcanvus))
                    lcurrent.MoveRight();
            }
            else if (e.KeyCode == Keys.D)
            {
                lcurrent.MoveRight();
                if (!lgame.BorderCheck(lcurrent) || !lgame.CollideCheck(lcurrent, lcanvus))
                    lcurrent.MoveLeft();
            }
            else if (e.KeyCode == Keys.W)
            {
                lcurrent.ClockwiseRotation();
                if (!lgame.BorderCheck(lcurrent) || !lgame.CollideCheck(lcurrent, lcanvus))
                    lcurrent.AnticlockwiseRotation();
            }
            else if (e.KeyCode == Keys.S)
            {
                if(lgame.MoveDownCheck(lcurrent,lcanvus))
                {
                    lcurrent.MoveDown();
                }
            }
            pictureBox1.Refresh();
            All_paint(lpe);
            if (e.KeyCode == Keys.Left)
            {
                rcurrent.MoveLeft();
                if (!rgame.BorderCheck(rcurrent) || !rgame.CollideCheck(rcurrent, rcanvus))
                    rcurrent.MoveRight();
            }
            else if(e.KeyCode==Keys.Right)
            {
                rcurrent.MoveRight();
                if (!rgame.BorderCheck(rcurrent) || !rgame.CollideCheck(rcurrent, rcanvus))
                    rcurrent.MoveLeft();
            }
            else if(e.KeyCode==Keys.Up)
            {
                rcurrent.ClockwiseRotation();
                if (!rgame.BorderCheck(rcurrent) || !rgame.CollideCheck(rcurrent, rcanvus))
                    rcurrent.AnticlockwiseRotation();
            }
            else if(e.KeyCode==Keys.Down)
            {
                if (rgame.MoveDownCheck(rcurrent, rcanvus))
                    rcurrent.MoveDown();
            }
            else if (e.KeyCode == Keys.Escape && !stop)
            {
                stop = !stop;
                timer1.Enabled = false;
                panel1.Visible = true;
                panel1.Enabled = true;
            }
            pictureBox2.Refresh();
            All_paint(rpe);
        }
        public void Canvus_Paint(Cube[,] canvus,Graphics g)
        {
            foreach(Cube cube in canvus)
            {
                if(cube != null)
                {
                    cube.drawCube(g, grid);
                }
            }
        }
        /// <summary>
        /// 结束游戏
        /// </summary>
        private void End_Paint()
        {
            if (lgame.EndCheck(lcurrent, lcanvus) || rgame.EndCheck(rcurrent, rcanvus))
            {
                lcurrent.MoveUP();
                rcurrent.MoveUP();
                timer1.Enabled = false;
                label3.Text = "游戏结束！~";
                MessageBox.Show("游戏结束！请输入玩家姓名！");
                string lgamer = Interaction.InputBox("该玩家分数为"+lscore.ToString(),"请输入左边玩家昵称","玩家昵称",-1,-1);
                string rgamer = Interaction.InputBox("该玩家分数为" + rscore.ToString(), "请输入右边玩家昵称", "玩家昵称", -1, -1);
                DatabaseConnect newcon = new DatabaseConnect();
                MySqlConnection con = newcon.Getcon();
                newcon.Insertdata(con, newcon.insertdata, lgamer, lscore.ToString(), DateTime.Now.ToString());
                newcon.Insertdata(con, newcon.insertdata, rgamer, rscore.ToString(), DateTime.Now.ToString());
                newcon.Closeconnect(con);
            }
            
        }
    }
}
