using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public static class InputDialog
    {
        public static DialogResult Show(out string strText,string ip)
        {
            string strTemp = string.Empty;

            InputIP inputDialog = new InputIP(ip);
            inputDialog.TextHandler = (str) => { strTemp = str; };

            DialogResult result = inputDialog.ShowDialog();
            strText = strTemp;

            return result;
        }
    }
}
