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
        private Form soloForm,spForm;
        private void button1_Click(object sender, EventArgs e)
        {
            soloForm = new SoloForm();
            this.Hide();
            soloForm.Location = this.Location;
            soloForm.ShowDialog();
            Application.ExitThread();
            this.Dispose();
        }

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
