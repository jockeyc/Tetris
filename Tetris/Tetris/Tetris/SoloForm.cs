﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{

    public partial class SoloForm : Form
    {
        int Row = 20, Col = 10, grid = 32;
        int score = 0;
        bool stop;
        PaintEventArgs pe;
        Pattren current, next;
        Cube[,] canvus = new Cube[20, 10];
        Game game = new Game();
        public SoloForm()
        {
            InitializeComponent();
            pictureBox1.Focus();
            panel1.BackColor = Color.LightSlateGray;
            pe = new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.pictureBox1.Size.Width, this.pictureBox1.Size.Height));
            current = game.RandomPattern();
            next = game.RandomPattern();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            stop = false;
            timer1.Enabled = true;
            panel1.Visible = false;
            panel1.Enabled = false;
            All_Paint(pe);
            Next_Paint();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (game.MoveDownCheck(current, canvus))
            {
                current.MoveDown();
            }
            else
            {
                game.FixCube(current, canvus);
                game.ClearLine(canvus, ref score);
                pictureBox1.Refresh();
                current = next;
                next = game.RandomPattern();
                End_Paint();
                Next_Paint();                
                Score_Paint();
            }
            All_Paint(pe);
            
        }
        //所有绘制方法
        private void All_Paint(PaintEventArgs e)
        {
            Bitmap bp = new Bitmap(this.pictureBox1.Size.Width, this.pictureBox1.Size.Height);
            Graphics bg = Graphics.FromImage(bp);
            //bg.FillRectangle(new SolidBrush(Color.LightSlateGray), 0, 0, this.pictureBox1.Size.Width, this.pictureBox1.Size.Height);
            Current_Paint(bg);
            Canvus_Paint(bg);
            Grid_Paint(bg);
            this.pictureBox1.BackgroundImage = bp;
            pictureBox1.Refresh();
        }
        //绘制当前
        private void Current_Paint(Graphics g)
        {
            current.drawPattern(g, grid);
        }
        //绘制下一个
        private void Next_Paint()
        {
            Bitmap bp = new Bitmap(this.pictureBox2.Size.Width, this.pictureBox1.Size.Height);
            Graphics bg = Graphics.FromImage(bp);
            for (int i = 0; i < 4; i++)
                next.cubes[i].j += 2;
            next.drawPattern(bg, grid);
            for (int i = 0; i < 4; i++)
                next.cubes[i].j -= 2;
            for (int i = 0; i <= 4; i++)
                bg.DrawLine(new Pen(Color.LightSlateGray), 3*grid, i * grid, 7 * grid, i * grid);
            for (int i = 0; i <= 4; i++)
                bg.DrawLine(new Pen(Color.LightSlateGray), (i+3) * grid, 0, (i+3) * grid, 4 * grid);
            
            this.pictureBox2.BackgroundImage = bp;
        }
        //绘制网格
        private void Grid_Paint(Graphics g)
        {
            for (int i = 0; i <= Row; i++)
                g.DrawLine(new Pen(Color.LightSlateGray), 0, i * grid, Col * grid, i * grid);
            for (int i = 0; i <= Col; i++)
                g.DrawLine(new Pen(Color.LightSlateGray), i * grid, 0, i * grid, Row * grid);
        }
        //绘制分数
        private void Score_Paint()
        {
            label1.Text = "分数:" + score.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StartForm startForm = new StartForm();
            this.Hide();
            startForm.Location = this.Location;
            startForm.ShowDialog();
            Application.ExitThread();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            canvus = new Cube[20, 10];
            current = game.RandomPattern();
            next = game.RandomPattern();
            score = 0;
            stop = false;
            Next_Paint();
            All_Paint(pe);
            panel1.Visible = false;
            panel1.Enabled = false;
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            panel1.Visible = false;
            panel1.Enabled = false;
            stop = false;
        }

        private void End_Paint()
        {
            if (game.EndCheck(current, canvus))
            {
                current.MoveUP();
                label2.Text = "游戏结束";
                timer1.Enabled = false;
            }
        }
        //键盘事件
        private void Key_Event(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                current.MoveLeft();
                if (!game.BorderCheck(current) || !game.CollideCheck(current, canvus))
                    current.MoveRight();
            }
            else if (e.KeyCode == Keys.Right)
            {
                current.MoveRight();
                if (!game.BorderCheck(current) || !game.CollideCheck(current, canvus))
                    current.MoveLeft();
            }
            else if (e.KeyCode == Keys.Up)
            {
                current.ClockwiseRotation();
                if(!game.BorderCheck(current) || !game.CollideCheck(current,canvus))
                    current.AnticlockwiseRotation();
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (game.MoveDownCheck(current, canvus))
                    current.MoveDown();
            }
            else if(e.KeyCode == Keys.Escape && !stop)
            {
                stop = !stop;
                timer1.Enabled = false;
                panel1.Visible = true;
                panel1.Enabled = true;
            }
            pictureBox1.Refresh();
            All_Paint(pe);
        }
        //固定画布
        public void Canvus_Paint(Graphics g)
        {
            foreach (Cube cube in canvus)
            {
                if (cube != null)
                {
                    cube.drawCube(g, grid);
                }
            }

        }
    }

    
    

}
