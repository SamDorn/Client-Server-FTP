using System;
using System.Windows.Forms;

namespace Client
{
    public partial class History : Form
    {
        public History()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void Write(string s)
        {
            txt_box_history.AppendText(s);
        }


    }
}
