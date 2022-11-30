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
        private string his = "";
        private string path;
        private History history;

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
        private void btn_help_history_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Questo pulsante ermette di aprire una nuova finestra e visualizzare lo storico delle operazioni.", "Aiuto visualizza storico", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btn_start_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Connect(txt_ip.Text, txt_port.Text));
            thread.Start();
           
        }


        private void btn_upload_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Upload(socketClient));
            thread.Start();
        }

        private void Disconnect(Socket s, string ip)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
            label9.Text = $"Disconnesso dall'HOST {ip} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
            his += label9.Text + "\r\n";
        }
        private void Connect(string ip, string port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, int.Parse(port));
            socketClient = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socketClient.Connect(ipEnd);
            btn_list.Enabled = true;
            btn_upload.Enabled = true;
            btn_percorso.Enabled = true;
            btn_start.Enabled = false;
            btn_stop.Enabled = true;
            label9.Text = $"Connesso all'HOST {ip} sulla porta {port} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
            his += label9.Text + "\r\n";
        }
        private void Upload(Socket s)
        {
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
            s.Send(fileNameByte);
            Thread.Sleep(50);
            s.SendFile(txt_box_path.Text);
            s.Send(terminator);
            label9.Text = $"Caricato {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
            his += label9.Text + "\r\n";
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
            string file = list_box_files.SelectedItem.ToString();
            byte[] msg = new byte[1024];
            msg = Encoding.ASCII.GetBytes("d");
            s.Send(msg);
            Thread.Sleep(50);
            byte[] clientData = new byte[4096];
            msg = Encoding.ASCII.GetBytes(file);
            s.Send(msg);
            int nFile = 0;
            FileStream fs = new FileStream(str + file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            while (true)
            {
                nFile = s.Receive(clientData);
                string bre = Encoding.ASCII.GetString(clientData);
                if (bre.IndexOf("CIAO") > -1)
                {
                    break;
                }
                else
                {
                    fs.Write(clientData, 0, nFile);
                }
            }
            label9.Text = $"Scaricato {file} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
            his += label9.Text + "\r\n";
            fs.Flush();
            fs.Close();
        
        }

        private void btn_list_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Visualizza(socketClient));
            thread.Start();
        }



        private void btn_stop_Click(object sender, EventArgs e)
        {
            Disconnect(socketClient, txt_ip.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_history_Click(object sender, EventArgs e)
        {
            history = new History();
            history.Write(his);
            history.Show();
        }

        
    }
}
