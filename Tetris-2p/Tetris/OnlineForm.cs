using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class OnlineForm : Form
    {
        int Row = 20, Col = 10, grid = 32;
        int score = 0;
        int friendScore = 0;
        PaintEventArgs pe;
        Pattern current, next;
        Pattern friendCurrent, friendNext;
        Cube[,] canvus = new Cube[20, 10];
        Cube[,] friendCanvus = new Cube[20, 10];
        Game game = new Game();
        Game friendGame = new Game();
        TcpClient tcpClient = new TcpClient();
        TcpListener tcpListener;
        TcpClient friendTcpClient;
        NetworkStream ns;
        NetworkStream friendNs;
        Thread thread;
        public OnlineForm()
        {
            InitializeComponent();
            pictureBox1.Focus();
            pe = new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.pictureBox1.Size.Width, this.pictureBox1.Size.Height));
            current = game.RandomPattern();
            next = game.RandomPattern();

            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), Int32.Parse("8090"));

            thread = new Thread(Listen);
            thread.Start();
            
            while (!tcpClient.Connected)
            {
                
                try
                {
                    tcpClient.Connect(IPAddress.Parse("127.0.0.1"), Int32.Parse("8080"));
                    ns = tcpClient.GetStream();
                }
                catch
                {

                }
                if (!tcpClient.Connected) MessageBox.Show("等待连接中");
            }
        }
        private void Form_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            All_Paint(pe);
            Next_Paint();
        }

        private void timer_Tick(object sender, EventArgs e)
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
                Next_Paint();
                Score_Paint();
            }
            SendState();
            SendCurrent();
            All_Paint(pe);
            FriendAllPaint(pe);
        }
        //所有绘制方法
        private void All_Paint(PaintEventArgs e)
        {
            Bitmap bp = new Bitmap(this.pictureBox1.Size.Width, this.pictureBox1.Size.Height);
            Graphics bg = Graphics.FromImage(bp);
            Current_Paint(bg);
            Canvus_Paint(bg);
            Grid_Paint(bg);

            this.pictureBox1.BackgroundImage = bp;

            pictureBox1.Refresh();

        }

        private void FriendAllPaint(PaintEventArgs e)
        {
            Bitmap bp2 = new Bitmap(this.pictureBox3.Size.Width, this.pictureBox3.Size.Height);
            Graphics bg2 = Graphics.FromImage(bp2);
            FriendCurrent_Paint(bg2);
            FriendCanvus_Paint(bg2);
            Grid_Paint(bg2);
            this.pictureBox3.BackgroundImage = bp2;
            pictureBox3.Refresh();
        }
        //绘制当前
        private void Current_Paint(Graphics g)
        {
            current.drawPattern(g, grid);
        }
        private void FriendCurrent_Paint(Graphics g)
        {
            if (friendCurrent != null)
            {
                friendCurrent.drawPattern(g, grid);
            }

        }
        //绘制下一个
        private void Next_Paint()
        {
            Bitmap bp = new Bitmap(this.pictureBox2.Size.Width, this.pictureBox1.Size.Height);
            Graphics bg = Graphics.FromImage(bp);
            next.drawPattern(bg, grid);
            for (int i = 0; i <= 4; i++)
                bg.DrawLine(new Pen(Color.LightSlateGray), 3 * grid, i * grid, 7 * grid, i * grid);
            for (int i = 0; i <= 4; i++)
                bg.DrawLine(new Pen(Color.LightSlateGray), (i + 3) * grid, 0, (i + 3) * grid, 4 * grid);

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
                if (!game.BorderCheck(current) || !game.CollideCheck(current, canvus))
                    current.AnticlockwiseRotation();
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (game.MoveDownCheck(current, canvus))
                    current.MoveDown();
            }
            SendCurrent();
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

        public void FriendCanvus_Paint(Graphics g)
        {
            foreach (Cube cube in friendCanvus)
            {
                if (cube != null)
                {
                    cube.drawCube(g, grid);
                }
            }

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">消息类型 0：操作信息 1：current</param>
        /// <param name="patterntype">方块类型</param>
        /// <param name="keyValue">按键值</param>
        public void SendMsg(int type, int patternType, int keyValue)
        {
            string message = string.Format("{0}:{1}:{2}", type, patternType, keyValue);
            Byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            //Console.WriteLine(ns.CanWrite+message);
            ns.Write(sendBytes, 0, sendBytes.Length);
        }
        public void SendState()
        {
            string message = "0@";
            foreach (Cube cube in canvus)
            {
                if (cube != null)
                {
                    message += cube.i + "," + cube.j + "," + (int)cube.color.ToArgb() + ":";
                }
            }
            Byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            //Console.WriteLine("1p:"+message);
            ns.Write(sendBytes, 0, sendBytes.Length);
        }
        public void SendCurrent()
        {
            string message = "1@";
            foreach (Cube cube in current.cubes)
            {
                if (cube != null)
                {
                    message += cube.i + "," + cube.j + "," + (int)cube.color.ToArgb() + ":";
                }
            }
            Byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            //Console.WriteLine("1p:"+message);
            ns.Write(sendBytes, 0, sendBytes.Length);
        }

        void Listen()
        {
            Console.WriteLine("8090监听中");
            tcpListener.Start(100);
            friendTcpClient = tcpListener.AcceptTcpClient();
            byte[] data = new byte[4096];
            while (true)
            {
                try
                {
                    friendNs = friendTcpClient.GetStream();
                    int length = friendNs.Read(data, 0, 4096);
                    string msg = Encoding.UTF8.GetString(data, 0, length);
                    //Console.WriteLine("2p:" + msg);
                    string[] temp = msg.Split('@');
                    string[] msgs = temp[1].Split(':');
                    if (temp[0] == "0")
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                friendCanvus[i, j] = null;
                            }
                        }
                        foreach (String s in msgs)
                        {
                            string[] cubeInfo = s.Split(',');
                            int i = int.Parse(cubeInfo[0]);
                            int j = int.Parse(cubeInfo[1]);
                            Color color = Color.FromArgb(int.Parse(cubeInfo[2]));
                            friendCanvus[j, i] = new Cube(i, j, color);
                        }
                    }
                    else
                    {
                        friendCurrent = new Pattern();
                        friendCurrent.cubes.Clear();
                        foreach (String s in msgs)
                        {
                            string[] cubeInfo = s.Split(',');
                            int i = int.Parse(cubeInfo[0]);
                            int j = int.Parse(cubeInfo[1]);
                            Color color = Color.FromArgb(int.Parse(cubeInfo[2]));
                            friendCurrent.cubes.Add(new Cube(i, j, color));
                        }
                    }
                    pictureBox3.Refresh();
                    FriendAllPaint(pe);
                }
                catch
                {

                }
            }
        }
    }
}
