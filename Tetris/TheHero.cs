using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Tetris
{
    public partial class TheHero : Form
    {
        DatabaseConnect Connect = new DatabaseConnect();
        public TheHero()
        {
            InitializeComponent();
        }

        private void TheHero_Load(object sender, EventArgs e)
        {
            
            MySqlConnection tcon=Connect.Getcon();
            MySqlDataReader treader = Connect.GetMyReader(tcon, Connect.readerdata);
            int readnum = 1;
            while (treader.Read() & (readnum<=8))
            {
                string nowplayername = treader.GetString("playername");
                string nowplascore = treader.GetString("playerscore");
                string nowstime = treader.GetString("submissiondate");
                //playername[readnum].Text = nowplayername;
                //score[readnum].Text = nowplascore;
                //stime[readnum].Text = nowstime;
                if (readnum == 1)
                {
                    playername1.Text = nowplayername;
                    score1.Text = nowplascore;
                    stime1.Text = nowstime;
                }
                else if (readnum == 2)
                {
                    playername2.Text = nowplayername;
                    score2.Text = nowplascore;
                    stime2.Text = nowstime;
                }
                else if (readnum == 3)
                {
                    playername3.Text = nowplayername;
                    score3.Text = nowplascore;
                    stime3.Text = nowstime;
                }
                else if (readnum == 4)
                {
                    playername4.Text = nowplayername;
                    score4.Text = nowplascore;
                    stime4.Text = nowstime;
                }
                else if (readnum == 5)
                {
                    playername5.Text = nowplayername;
                    score5.Text = nowplascore;
                    stime5.Text = nowstime;
                }
                else if (readnum == 6)
                {
                    playername6.Text = nowplayername;
                    score6.Text = nowplascore;
                    stime6.Text = nowstime;
                }
                else if (readnum == 7)
                {
                    playername7.Text = nowplayername;
                    score7.Text = nowplascore;
                    stime7.Text = nowstime;
                }
                else if (readnum == 8)
                {
                    playername8.Text = nowplayername;
                    score8.Text = nowplascore;
                    stime8.Text = nowstime;
                }
                readnum++;
            }
            Connect.Closeconnect(tcon);
        }
    }
}
