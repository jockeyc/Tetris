using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }
        private Form soloForm;
        private void button1_Click(object sender, EventArgs e)
        {
            soloForm = new SoloForm();
            this.Hide();
            soloForm.Location = this.Location;
            soloForm.ShowDialog();
            Application.ExitThread();
            this.Dispose();
        }
        private Form localDoubleForm;
        private void button3_Click(object sender, EventArgs e)
        {
            localDoubleForm = new LocalDoubleForm();
            this.Hide();
            localDoubleForm.Location = this.Location;
            localDoubleForm.ShowDialog();
            Application.ExitThread();
            this.Dispose();
        }
        private Form theHero; 
        private void button4_Click(object sender, EventArgs e)
        {
            theHero = new TheHero();
            this.Hide();
            theHero.Location = this.Location;
            theHero.ShowDialog();
            Application.ExitThread();
            this.Dispose();

        }
        private Form onlineForm;
        private void button5_Click(object sender, EventArgs e)
        {
            onlineForm = new OnlineForm();
            this.Hide();
            onlineForm.Location = this.Location;
            onlineForm.ShowDialog();
            Application.ExitThread();
            this.Dispose();
        }
        private Form spForm;
        private void button2_Click(object sender, EventArgs e)
        {
            spForm = new SpForm();
            this.Hide();
            spForm.Location = this.Location;
            spForm.ShowDialog();
            Application.ExitThread();
            this.Dispose();
        }
    }
}
