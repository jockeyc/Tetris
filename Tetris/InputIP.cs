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
    public partial class InputIP : Form
    {
        public InputIP(string ip)
        {
            InitializeComponent();
            label2.Text = "本机IP:  " + ip;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TextHandler.Invoke(textBox1.Text);
            DialogResult = DialogResult.OK;
        }

        public delegate void TextEventHandler(string strText);

        public TextEventHandler TextHandler;
    }
}
