using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client_FTP
{
    public partial class Form1 : Form
    {
        private Socket socketClient;

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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_box_path.Text = openFileDialog1.FileName;
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
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void btn_help_progress_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Questa barra visulizza il caricamento di upload/download di un file.", "Aiuto barra progressi");
        }

        private void btm_start_Click(object sender, EventArgs e)
        {


            btn_upload.Enabled = true;
            txt_box_path.Enabled = true;
            btn_percorso.Enabled = true;
        }


        private void btn_upload_Click(object sender, EventArgs e)
        {
            string filePath = "";
            string fileName = txt_box_path.Text;
            fileName = fileName.Replace("\\", "/");
            while (fileName.IndexOf("/") > -1)
            {
                filePath += fileName.Substring(0, fileName.IndexOf("/") + 1);
                fileName = fileName.Substring(fileName.IndexOf("/") + 1);
            }
            //sendfile(txt_box_path.Text);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 5000);
            Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            byte[] fileNameByte = Encoding.UTF8.GetBytes(fileName);


            string fullPath = filePath + fileName;

            byte[] fileData = File.ReadAllBytes(fullPath);
            byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);
            clientSocket.Connect(ipEnd);
            clientSocket.Send(clientData, 0, clientData.Length, 0);

        }
        
        

    private void sendfile(string fn)

    {


















        /*FileStream file = new FileStream(txt_box_path.Text, FileMode.Open); 
        long totalBytes = file.Length;
        try
        {
            clientSocket.Connect(ipEnd);
            byte[] filechunk = new byte[4096];
            int numBytes;
            while ((numBytes = file.Read(filechunk, 0, 4096)) > 0)
            {
                if (clientSocket.Send(filechunk, numBytes, SocketFlags.None) != numBytes)
                {
                    throw new Exception("Error in sending the file");
                }
                /*bytesSoFar += numBytes;
                Byte progress = (byte)(bytesSoFar * 100 / totalBytes);
                if (progress > lastStatus && progress != 100)
                {
                    MessageBox.Show(Convert.ToString(lastStatus));
                    lastStatus = progress;
                }
            }
            file.Close();
        }
        catch(Exception e){
            MessageBox.Show(e.ToString());
        }

            //[0]filenamelen[4]filenamebyte[*]filedata     
        */

    }
}
}
