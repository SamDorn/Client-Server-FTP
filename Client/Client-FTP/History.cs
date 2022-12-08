using System;
using System.Windows.Forms;

namespace Client
{
    public partial class History : Form
    {
        private Form1 form1;
        public History()
        {
            InitializeComponent();

        }
        public void History_Load(object sender, EventArgs e)
        {
            form1 = new Form1();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            form1.btn_history.Enabled = true;
            Close();
        }
        public void Write(string s)
        {
            txt_box_history.AppendText(s);
        }


    }
}
