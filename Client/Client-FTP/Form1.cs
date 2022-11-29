using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace Client_FTP
{
    public partial class Form1 : Form
    {
        private Socket socketClient;
        private string path;

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        #region help buttuns

        private void btn_help_server_Click(object sender, EventArgs e)
        {
            MessageBox.Show("L'indirizzo ip del server locale è 127.0.0.1, non modificare se non si sa cosa si sta facendo, utilizzare la porta con cui il server " +
                "ha aperto la connessione\nStart: per avviare la connessione \nStop: per fermare la connessione", "Aiuto connessione server", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_help_progress_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Questa barra visulizza il caricamento di upload/download di un file.", "Aiuto barra progressi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_help_upload_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Browse: per selezionare il file da inviare\nCarica: per caricare il file sul server", "Aiuto upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void but_help_visualizza_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Visualizza i file disponibili sul server per il download", "Aiuto visualizza file", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btn_help_download_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Scarica il file selezionato nella cartella " + Path.GetFullPath(path), "Aiuto download", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                path = $"{folderBrowserDialog1.SelectedPath}/";
            }
            Thread thread = new Thread(() => Download(socketClient, path));
            thread.Start();
        }

        private void btm_start_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Connect(txt_ip.Text, txt_port.Text));
            //btn_stop.Enabled = true;
            thread.Start();
           
        }


        private void btn_upload_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Upload(socketClient));
            thread.Start();
        }

        private void Disconnect(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
        private void Connect(string ip, string port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, int.Parse(port));
            socketClient = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketClient.Connect(ipEnd);
            btn_list.Enabled = true;
            btn_upload.Enabled = true;
            txt_box_path.Enabled = true;
            btn_percorso.Enabled = true;
            btm_start.Enabled = false;
        }
        private void Upload(Socket s)
        {
            int lastStatus = 0;
            int numBytes = 0;
            prg_bar.Enabled = true;
            byte[] terminator = Encoding.ASCII.GetBytes("CIAO");
            byte[] msg = Encoding.ASCII.GetBytes("u");
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
            long bytesSoFar = 0, totalBytes = clientData.Length;
            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);

            s.Send(clientData, 0, clientData.Length, 0);
            s.Send(terminator);
            
            

        }

        private void Visualizza(Socket s)
        {

            byte[] vis = new byte[1024];
            btn_download.Enabled = true;
            list_box_files.Items.Clear();
            byte[] msg = Encoding.ASCII.GetBytes("v");
            s.Send(msg);

            int bytesRec = s.Receive(vis);
            string data = "";
            data = Encoding.ASCII.GetString(vis, 0, bytesRec);
            string[] file;
            file = data.Split('/');
            int n = file.Length;
            string[] result = file.Take(n-1).ToArray();

            foreach (string fileStr in result)
            {
                list_box_files.Items.Add(fileStr);
            }
        }
        private void Download(Socket s, string str)
        {

            byte[] msg = new byte[1024];
            byte[] vis = new byte[1024];
            string data = "";
            msg = Encoding.ASCII.GetBytes("d");
            s.Send(msg);
            msg = Encoding.ASCII.GetBytes(list_box_files.SelectedItem.ToString());
            s.Send(msg);
            byte[] clientData = new byte[4096];
            int receivedBytesLen = s.Receive(clientData, clientData.Length, 0);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
            using (var bWrite = File.Open(str + fileName, FileMode.OpenOrCreate))
            {
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                while (receivedBytesLen > 0)
                {
                    receivedBytesLen = s.Receive(clientData, clientData.Length, 0);
                    data = Encoding.ASCII.GetString(clientData);
                    if (data.IndexOf("CIAO") > -1)
                    {
                        break;
                    }
                    else
                    {
                        bWrite.Write(clientData, 0, receivedBytesLen);
                    }
                }
                bWrite.Flush();
                bWrite.Close();

            }
        }

        private void btn_list_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Visualizza(socketClient));
            thread.Start();
        }



        private void btn_stop_Click(object sender, EventArgs e)
        {
            Disconnect(socketClient);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /*private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }*/


    }
}
