using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_FTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btn_help_server_Click(object sender, EventArgs e)
        {
            MessageBox.Show("L'indirizzo ip del server locale è 127.0.0.1, non modificare se non si sa cosa si sta facendo, utilizzare la porta con cui il server " +
                "ha aperto la connessione\nStart: per avviare la connessione \nStop: per fermare la connessione", "Aiuto connessione server");
        }



        private void but_percorso_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog1.FileName;
            }
            
        }

        


        private void btn_help_upload_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Browse: per selezionare il file da inviare\nCarica: per caricare il file sul server", "Aiuto upload");
        }

        private void but_help_visualizza_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Visualizza i file disponibili sul server per il download", "Aiuto visualizza file");
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

            }
        }
    }
}
