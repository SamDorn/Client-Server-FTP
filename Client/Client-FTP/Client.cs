using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private IPEndPoint ipEnd;
        private IPAddress ipAddress;
        private Socket socketClient;
        private History history;
        private string his;
        private string fileName;
        private string path;
        private string file;
        private string ip;
        private int port;
        public bool creato = false;
        private bool chiuso = true;

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
            MessageBox.Show("Browse: per selezionare il file da inviare\nUpload: per caricare il file sul server", "Aiuto upload", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void but_help_visualizza_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Visualizza i file disponibili sul server per il download", "Aiuto visualizza file", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btn_help_download_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Scarica il file selezionato in una cartella a scelta", "Aiuto download", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btn_help_history_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Questo pulsante permette di aprire una nuova finestra e visualizzare lo storico delle operazioni.", "Aiuto visualizza storico", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
        #region btn_click
        private void btn_percorso_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_box_path.Text = openFileDialog1.FileName;
            }
        }
        private void btn_download_Click(object sender, EventArgs e)
        {
            if (list_box_files.SelectedIndex == -1)
            {
                MessageBox.Show("Devi selezionare un file", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = $"{folderBrowserDialog1.SelectedPath}/";

                    Thread thread = new Thread(() => Download(socketClient, path));
                    thread.Start();
                }
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            port = Int32.Parse(txt_port.Text);
            if (port > 34665)
            {
                MessageBox.Show("Porta inserita non valida", "Errore porta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ip = txt_ip.Text;
                Thread thread = new Thread(() => Connect());
                thread.Start();
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Upload(socketClient));
            thread.Start();
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

        private void btn_history_Click(object sender, EventArgs e)
        {
            history = new History();
            history.Write(his);
            history.Show();
        }
        #endregion
        #region controlli input
        private void txt_ip_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '\b')
                e.Handled = true;
        }

        private void txt_port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }

        #endregion
        #region connessione disconnessione
        private void Connect()
        {
            if (!IPAddress.TryParse(ip, out ipAddress))
                MessageBox.Show("Indirizzo ip non valido", "Errore ip", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else
            {
                ipEnd = new IPEndPoint(ipAddress, port);
                socketClient = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {

                    btn_start.Enabled = false;
                    socketClient.Connect(ipEnd);
                    chiuso = false;
                    btn_list.Enabled = true;
                    btn_upload.Enabled = true;
                    btn_percorso.Enabled = true;
                    btn_stop.Enabled = true;
                    label9.Text = $"Connesso all'HOST {ip} sulla porta {port} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                    his += label9.Text + "\r\n";
                }
                catch (Exception e)
                {
                    his += $"Generato errore: {e.Message} alle ore {DateTime.Now.ToString("HH:mm:ss")}" + "\r\n";
                    btn_start.Enabled = true;
                    MessageBox.Show(e.Message, "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        private void Disconnect(Socket s)
        {
            byte[] msg = Encoding.ASCII.GetBytes("e");
            try
            {
                chiuso = true;
                s.Send(msg);
                s.Close();
                label9.Text = $"Disconnesso dall'HOST {ip} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                his += label9.Text + "\r\n";
                btn_download.Enabled = false;
                btn_list.Enabled = false;
                btn_start.Enabled = true;
                btn_stop.Enabled = false;
                btn_upload.Enabled = false;
                btn_percorso.Enabled = false;
            }

            catch (SocketException ex)
            {
                label9.Text = $"Disconnessione forzata con errore {ex.Message} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                his += label9.Text + "\r\n";
                list_box_files.Items.Clear();
                txt_box_path.Text = "";
                btn_download.Enabled = false;
                btn_list.Enabled = false;
                btn_start.Enabled = true;
                btn_stop.Enabled = false;
                btn_upload.Enabled = false;
                btn_percorso.Enabled = false;

                ForceDisconnect(socketClient);
            }
        }
        private void ForceDisconnect(Socket s)
        {
            s.Close();
        }

        #endregion
        #region upload download visualizza
        private void Upload(Socket s)
        {
            prg_bar.Value = 0;
            if (!(txt_box_path.Text == ""))
            {
                prg_bar.Enabled = true;
                byte[] terminator = Encoding.ASCII.GetBytes("<EOF>");
                byte[] msg = Encoding.ASCII.GetBytes("u");
                try
                {
                    s.Send(msg);
                    string filePath = "";
                    fileName = txt_box_path.Text;
                    fileName = fileName.Replace("\\", "/");
                    while (fileName.IndexOf("/") > -1)
                    {
                        filePath += fileName.Substring(0, fileName.IndexOf("/") + 1);
                        fileName = fileName.Substring(fileName.IndexOf("/") + 1);
                    }
                    byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
                    s.Send(fileNameByte);
                    Thread.Sleep(20);
                    FileStream file = new FileStream(txt_box_path.Text, FileMode.Open);
                    long totalBytes = file.Length, bytesSoFar = 0;
                    byte[] buffer = new byte[1024];
                    buffer = Encoding.ASCII.GetBytes(Convert.ToString(totalBytes));
                    s.Send(buffer);
                    Thread.Sleep(20);
                    byte[] filechunk = new byte[4096];
                    int numBytes;
                    while ((numBytes = file.Read(filechunk, 0, 4096)) > 0)
                    {
                        if (socketClient.Send(filechunk, numBytes, SocketFlags.None) != numBytes)
                        {
                            MessageBox.Show("Errore nel trasmettere il file", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        bytesSoFar += numBytes;
                        Byte progress = (byte)(bytesSoFar * 100 / totalBytes);
                        prg_bar.Value = progress;
                        label9.Text = $"Caricando {fileName}...{progress}%";
                    }
                    s.Send(terminator);
                    file.Close();
                    label9.Text = $"Caricato {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                    his += label9.Text + "\r\n";
                    Thread.Sleep(2000);
                    prg_bar.Value = 0;
                }
                catch (SocketException e)
                {
                    txt_box_path.Text = "";
                    label9.Text = $"Disconnessione forzata mentre caricavo {fileName} con errore {e.Message} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                    his += label9.Text + "\r\n";
                    list_box_files.Items.Clear();
                    prg_bar.Value = 0;
                    btn_download.Enabled = false;
                    btn_list.Enabled = false;
                    btn_start.Enabled = true;
                    btn_stop.Enabled = false;
                    btn_upload.Enabled = false;
                    btn_percorso.Enabled = false;


                    MessageBox.Show(e.Message + "\r\nMi sto scollegando", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ForceDisconnect(socketClient);
                }
                catch (IOException)
                {
                    label9.Text = $"Generata IOException nel tentativo di caricare {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                    his += label9.Text + "\r\n";
                    MessageBox.Show("File già in upload o attualmente in uso", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void Download(Socket s, string str)
        {
            long lenght, bytesSoFar = 0;
            string res;
            if (list_box_files.SelectedIndex != -1)
            {

                file = list_box_files.SelectedItem.ToString();
                byte[] msg = new byte[1024];
                byte[] data = new byte[1024];
                msg = Encoding.ASCII.GetBytes("d");

                try
                {

                    s.Send(msg);
                    Thread.Sleep(50);
                    byte[] clientData = new byte[4096];
                    msg = Encoding.ASCII.GetBytes(file);
                    s.Send(msg);
                    s.Receive(data);
                    res = Encoding.ASCII.GetString(data);
                    lenght = Convert.ToInt64(res);
                    int nFile = 0;
                    FileStream fs = new FileStream(path + file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    while (true)
                    {
                        nFile = s.Receive(clientData);
                        string bre = Encoding.ASCII.GetString(clientData);
                        if (bre.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                        else
                        {
                            bytesSoFar += nFile;
                            Byte progresso = (byte)(bytesSoFar * 100 / lenght);
                            fs.Write(clientData, 0, nFile);
                            prg_bar.Value = progresso;
                            label9.Text = $"Scaricando {file}...{progresso}%";
                        }
                    }
                    prg_bar.Value = 100;
                    label9.Text = $"Scaricando {file}...100%";
                    fs.Flush();
                    fs.Close();
                    label9.Text = $"Scaricato {file} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                    Thread.Sleep(2000);
                    prg_bar.Value = 0;
                    his += label9.Text + "\r\n";
                }
                catch (SocketException e)
                {
                    prg_bar.Value = 0;
                    label9.Text = $"Disconnessione forzata mentre scaricavo {file} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                    his += label9.Text + "\r\n";
                    list_box_files.Items.Clear();
                    txt_box_path.Text = "";
                    btn_download.Enabled = false;
                    btn_list.Enabled = false;
                    btn_start.Enabled = true;
                    btn_stop.Enabled = false;
                    btn_upload.Enabled = false;
                    btn_percorso.Enabled = false;
                    MessageBox.Show(e.Message + "\r\nMi sto scollegando", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ForceDisconnect(socketClient);
                }
                catch (Exception ex)
                {
                    prg_bar.Value = 0;
                    his += $"Generato errore: {ex.Message} mentre scaricavo {file} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                    list_box_files.Items.Clear();
                    txt_box_path.Text = "";
                    btn_download.Enabled = false;
                    btn_list.Enabled = false;
                    btn_start.Enabled = true;
                    btn_stop.Enabled = false;
                    btn_upload.Enabled = false;
                    btn_percorso.Enabled = false;
                    MessageBox.Show(ex.Message + "\r\nMi sto scollegando", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Disconnect(socketClient);
                }
            }
        }
        private void Visualizza(Socket s)
        {
            string data = "";
            byte[] vis = new byte[1024];
            btn_download.Enabled = true;
            list_box_files.Items.Clear();
            byte[] msg = Encoding.ASCII.GetBytes("v");
            try
            {
                s.Send(msg);
                s.Receive(vis);
                data = Encoding.ASCII.GetString(vis);
                string[] file;
                file = data.Split('/');
                int n = file.Length;
                string[] result = file.Take(n - 1).ToArray();

                foreach (string fileStr in result)
                {
                    list_box_files.Items.Add(fileStr);
                }
            }
            catch (SocketException e)
            {
                prg_bar.Value = 0;
                his += $"Disconnessione forzata alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                list_box_files.Items.Clear();
                txt_box_path.Text = "";
                btn_download.Enabled = false;
                btn_list.Enabled = false;
                btn_start.Enabled = true;
                btn_stop.Enabled = false;
                btn_upload.Enabled = false;
                btn_percorso.Enabled = false;
                MessageBox.Show(e.Message + "\r\nMi sto scollegando", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ForceDisconnect(socketClient);
            }
        }

        #endregion
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!chiuso)
                Disconnect(socketClient);
        }


    }
}
