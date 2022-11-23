using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client_FTP
{
    public partial class Form1 : Form
    {
        private Socket socketClient;
        private byte[] msg;

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        #region help buttun

        private void btn_help_server_Click(object sender, EventArgs e)
        {
            MessageBox.Show("L'indirizzo ip del server locale è 127.0.0.1, non modificare se non si sa cosa si sta facendo, utilizzare la porta con cui il server " +
                "ha aperto la connessione\nStart: per avviare la connessione \nStop: per fermare la connessione", "Aiuto connessione server");
        }

        private void btn_help_progress_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Questa barra visulizza il caricamento di upload/download di un file.", "Aiuto barra progressi");
        }

        private void btn_help_upload_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Browse: per selezionare il file da inviare\nCarica: per caricare il file sul server", "Aiuto upload");
        }

        private void but_help_visualizza_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Visualizza i file disponibili sul server per il download", "Aiuto visualizza file");
        }
        #endregion



        private void but_percorso_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_box_path.Text = openFileDialog1.FileName;
            }

        }








        private void btn_download_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

            }
        }



        private void btm_start_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Connect());
            btn_stop.Enabled = true;
            thread.Start();
        }


        private void btn_upload_Click(object sender, EventArgs e)
        {
            Upload(socketClient);

        }
        
        private void Disconnect(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
        private void Connect()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 5000);
            socketClient = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socketClient.Connect(ipEnd);

            btn_list.Enabled = true;
            btn_upload.Enabled = true;
            txt_box_path.Enabled = true;
            btn_percorso.Enabled = true;
        }
        private void Upload(Socket s)
        {
            msg = Encoding.ASCII.GetBytes("u");
            s.Send(msg);
            string filePath = "";
            string fileName = txt_box_path.Text;
            fileName = fileName.Replace("\\", "/");
            while (fileName.IndexOf("/") > -1)
            {
                filePath += fileName.Substring(0, fileName.IndexOf("/") + 1);
                fileName = fileName.Substring(fileName.IndexOf("/") + 1);
            }

            byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
            byte[] fileData = File.ReadAllBytes(txt_box_path.Text);
            byte[] clientData = new byte[fileNameByte.Length + 4 + fileData.Length];
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);
            s.Send(clientData, 0, clientData.Length, 0);
        }

        private void Visualizza(Socket s)
        {
            byte[] msg = Encoding.ASCII.GetBytes("v");
            s.Send(msg);

            int bytesRec = s.Receive(msg);
            string data = null;
            data += Encoding.ASCII.GetString(msg, 0, bytesRec);

            list_box_files.Items.Add(data);
        }

        private void btn_list_Click(object sender, EventArgs e)
        {
            Visualizza(socketClient);

        }

        private void list_box_files_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Disconnect(socketClient);
        }
    }
}
