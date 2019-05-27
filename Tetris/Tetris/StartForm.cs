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
        }

        private void StartForm_Load(object sender, EventArgs e)
        {

        }
        private Form onlineForm;
        private void Button2_Click(object sender, EventArgs e)
        {
            onlineForm = new OnlineForm();
            this.Hide();
            onlineForm.Location = this.Location;
            onlineForm.ShowDialog();
            Application.ExitThread();
        }
    }
}
