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
        private string his, ip;
        private int port;
        private bool inUso = false, chiuso = true;

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
        /// <summary>
        /// Apre la finestra di dialogo dove si può selezionare un file. il tread viene avviato solo se si preme OK
        /// </summary>
        private void btn_percorso_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_box_path.Text = openFileDialog1.FileName;
            }
        }
        /// <summary>
        /// Se non è selezionato alcun file dalla listbox viene detto. Se un file è selezionato si procede a chiedere all'utente dove vuole salvare il file.
        /// Alla variabile viene aggiunta un / poichè necessaria.
        /// </summary>
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
                    string path = $"{folderBrowserDialog1.SelectedPath}/";

                    Thread thread = new Thread(() => Download(socketClient, path));
                    thread.Start();
                }
            }
        }
        /// <summary>
        /// Viene fatto un semplice controllo sulle porte: se maggiore dele porte totali disponibili appare un MessageBox
        /// </summary>
        private void btn_start_Click(object sender, EventArgs e)
        {
            port = Int32.Parse(txt_port.Text);
            if (port > 65535)
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
        /// <summary>
        /// Quando viene premuto il pulsante upload viene chiamata la funzione Upload e si crea un nuovo thread
        /// </summary>
        private void btn_upload_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Upload(socketClient));
            thread.Start();
        }

        /// <summary>
        /// Quando viene premuto il pulsante upload viene chiamata la funzione Visualizza e si crea un nuovo thread
        /// </summary>
        private void btn_list_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Visualizza(socketClient));
            thread.Start();
        }
        /// <summary>
        /// Quando viene premuto il pulsante upload viene chiamata la funzione Disconnect
        /// </summary>
        private void btn_stop_Click(object sender, EventArgs e)
        {
            Disconnect(socketClient);
        }
        /// <summary>
        /// Quando viene premuto il pulsante upload viene creato un nuovo oggetto della classe Hystory che servirà per visualizzare 
        /// tutte le operazioni effettuate ed eventali errori
        /// </summary>
        private void btn_history_Click(object sender, EventArgs e)
        {
            history = new History();
            history.Write(his);
            history.Show();
        }
        #endregion
        #region controlli input
        /// <summary>
        /// Controlli input, nella casella ip non è possibile inserire lettere
        /// </summary>
        private void txt_ip_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '\b')
                e.Handled = true;
        }
        /// <summary>
        /// Controlli input, nella casella porta non è possibile inserire lettere
        /// </summary>
        private void txt_port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
                e.Handled = true;
        }

        #endregion
        #region metodi globali
        /// <summary>
        /// Viene inviata la lettera al server in modo che sappia cosa il client si aspetta
        /// </summary>
        /// <param name="s">Socket creata</param>
        /// <param name="lettera">String lettera</param>
        private void InvioLettera(Socket s, string lettera)
        {
            byte[] data = Encoding.ASCII.GetBytes(lettera);
            s.Send(data);
        }/// <summary>
         /// Viene inviato il terminator così che il server sappia che l'invio del file è terminato
         /// </summary>
         /// <param name="s">Socket creata</param>
        private void InvioTerminator(Socket s)
        {
            byte[] data = Encoding.ASCII.GetBytes("<EOF>");
            s.Send(data);
        }
        /// <summary>
        /// Viene inviato il nome del file in modo che il server sappia come salvare ol file
        /// </summary>
        /// <param name="s">Socket creata</param>
        /// <param name="str">Stringa nome file</param>
        private void InvioNomeFile(Socket s, string str)
        {
            byte[] data = Encoding.ASCII.GetBytes(str);
            s.Send(data);
        }/// <summary>
         /// Viene inviata la lunghezza del file
         /// </summary>
         /// <param name="s">Socket creata</param>
         /// <param name="lenght">Lunghezza file</param>
        private void InvioLunghezzaFile(Socket s, long lenght)
        {
            byte[] data = Encoding.ASCII.GetBytes(Convert.ToString(lenght));
            s.Send(data);
        }
        /// <summary>
        /// Riceve la lunghezza del file
        /// </summary>
        /// <param name="s">Socket creata</param>
        /// <returns></returns>
        private long GetLenght(Socket s)
        {
            byte[] data = new byte[1024];
            s.Receive(data);
            string res = Encoding.ASCII.GetString(data);
            long lenght = Convert.ToInt64(res);
            return lenght;
        }/// <summary>
         /// Viene adoperata la classe FileStream per scrivere i byte che arrivano
         /// Viene fatto un controllo sulla receive, se il server ha inviato il terminator non ricevere più nulla 
         /// ed esci dal while, viene chiuso il file e viene aggiornata la label.
         /// </summary>
         /// <param name="s">Socket creata</param>
         /// <param name="str">Percorso deove salvare</param>
         /// <param name="fileName">Nome file</param>
         /// <param name="lenght">Lunghezza file</param>
        private void GetFile(Socket s, string str, string fileName, long lenght)
        {
            
            btn_stop.Enabled = false;
            btn_download.Enabled = false;
            btn_upload.Enabled = false;
            byte[] clientData = new byte[4096];
            long bytesSoFar = 0;
            FileStream fs = new FileStream(str + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            while (true)
            {
                int nFile = s.Receive(clientData);
                string bre = Encoding.ASCII.GetString(clientData);
                if (bre.IndexOf("<EOF>") > -1)
                {
                    break;
                }
                else
                {
                    bytesSoFar += nFile;
                    int progresso = (int)(bytesSoFar * 100 / lenght);
                    fs.Write(clientData, 0, nFile);
                    prg_bar.Value = progresso;
                    label9.Text = $"Scaricando {fileName}...{progresso}%";
                }
            }
            prg_bar.Value = 100;
            fs.Flush();
            fs.Close();
            label9.Text = $"Scaricato {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
            Thread.Sleep(2000);
            prg_bar.Value = 0;
            his += label9.Text + "\r\n";
            btn_download.Enabled = true;
            btn_upload.Enabled = true;
            btn_stop.Enabled = true;
        }
        /// <summary>
        /// Questo metodo cambia gli \\ in / edi una stringa e ottiene solo la parte finale del percorso 
        /// </summary>
        /// <param name="str">Stringa percorso</param>
        /// <returns></returns>
        private string GetFileName(string str)
        {
            str = str.Replace("\\", "/");
            while (str.IndexOf("/") > -1)
            {
                str = str.Substring(str.IndexOf("/") + 1);
            }
            return str;
        }/// <summary>
         /// Il file viene spezzettato, con un while, in blocchi da 4096 byte e vengono inviati
         /// Viene fatto per poter lavorare con file di grandi dimensioni
         /// Viene applicato un controllo alla send
         /// Viene creato un int progress che servirà a tenere traccia dello stato della send
         /// Viene aggiornata la label e la progress bar
         /// Viene chiuso il file e viene aggiornata la label
         /// </summary>
         /// <param name="s">Socket creata</param>
         /// <param name="file">FileStream creata</param>
         /// <param name="nome">Stringa nome file</param>
        private void InvioFile(Socket s, FileStream file, string nome)
        {
            btn_stop.Enabled = false;
            btn_download.Enabled = false;
            btn_upload.Enabled = false;
            long lenght = file.Length, bytesSoFar = 0;
            byte[] filechunk = new byte[4096];
            int numBytes;
            while ((numBytes = file.Read(filechunk, 0, 4096)) > 0)
            {
                if (s.Send(filechunk, numBytes, SocketFlags.None) != numBytes)
                {
                    MessageBox.Show("Errore nel trasmettere il file", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                }
                bytesSoFar += numBytes;
                int progress = (int)(bytesSoFar * 100 / lenght);
                prg_bar.Value = progress;
                label9.Text = $"Caricando {nome}...{progress}%";
            }
            file.Close();
            label9.Text = $"Caricato {nome} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
            his += label9.Text + "\r\n";
            Thread.Sleep(2000);
            prg_bar.Value = 0;
            btn_download.Enabled = true;
            btn_upload.Enabled = true;
            btn_stop.Enabled = true;
        }
        /// <summary>
        /// Resetta l'interfaccia grafica
        /// </summary>
        private void Reset()
        {
            prg_bar.Value = 0;
            list_box_files.Items.Clear();
            txt_box_path.Text = "";
            btn_download.Enabled = false;
            btn_list.Enabled = false;
            btn_start.Enabled = true;
            btn_stop.Enabled = false;
            btn_upload.Enabled = false;
            btn_percorso.Enabled = false;


        }
        #endregion
        #region connessione disconnessione
        /// <summary>
        /// Si connette al server
        /// </summary>
        private void Connect()
        {
            if (!IPAddress.TryParse(ip, out ipAddress))
                MessageBox.Show("Indirizzo ip non valido", "Errore ip", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else
            {

                btn_history.Enabled = true;
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
        }/// <summary>
        /// Invia una lettera al server e poi chiude la connessione con il server.
        /// </summary>
        /// <param name="s">Socket creata</param>
        private void Disconnect(Socket s)
        {
            try
            {
                InvioLettera(s, "e");
                chiuso = true;
                s.Close();
                label9.Text = $"Disconnesso dall'HOST {ip} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                his += label9.Text + "\r\n";
                Reset();
            }

            catch (SocketException ex)
            {
                label9.Text = $"Disconnessione forzata con errore {ex.Message} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                his += label9.Text + "\r\n";
                Reset();

                ForceDisconnect(socketClient);
            }
        }/// <summary>
        /// Forza la chiusura della socket senza invio messaggio
        /// </summary>
        /// <param name="s">Socket creata</param>
        private void ForceDisconnect(Socket s)
        {
            s.Close();
        }

        #endregion
        #region upload download visualizza
        /// <summary>
        /// Controlla se la txt box è vuota
        /// Viene utilizzata la classe FileStram per aprire il file che si vuole inviare
        /// Viene dichiarata una variabile long uguale alla lunghezza del file in byte
        /// Viene inviata una lettera
        /// Viene invocato il metodo che genera il nome del file dal percorso
        /// Viene inviato il nome del file
        /// Viene inviata la lunghezza del file
        /// Viene inviato il file
        /// Viene inviato il terminatore
        /// </summary>
        /// <exception cref="SocketException">Errore socket, viene invocata la disconnessione forzata</exception>
        /// <param name="s">Socket creata</param>
        private void Upload(Socket s)
        {
            prg_bar.Value = 0;
            if (!(txt_box_path.Text == ""))
            {

                try
                {
                    FileStream file = new FileStream(txt_box_path.Text, FileMode.Open);
                    long lenght = file.Length;
                    InvioLettera(s, "u");
                    string fileName = GetFileName(txt_box_path.Text);
                    Thread.Sleep(30);
                    InvioNomeFile(s, fileName);
                    Thread.Sleep(30);
                    InvioLunghezzaFile(s, lenght);
                    Thread.Sleep(30);
                    InvioFile(s, file, fileName);
                    InvioTerminator(s);

                }
                catch (SocketException e)
                {
                    label9.Text = $"Disconnessione forzata  con errore: {e.Message} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                    his += label9.Text + "\r\n";
                    Reset();
                    MessageBox.Show(e.Message + "\r\nMi sto scollegando", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ForceDisconnect(socketClient);
                }
            }
        }


        /// <summary>
        /// viene inviata una lettera
        /// è necessario far riposare il thread in quanto la Socket.Send potrebbe generare dei problemi 
        /// in quanbto ne vengono invocate 2 in un breve periodo di tempo
        /// Viene inviato il nome del file
        /// Si riceve la lunghezza del file che si riceverà così da poter implementare la progress bar
        /// Si riceve il file
        /// <exception cref="SocketException">Errore socket, viene invocata la disconnessione forzata</exception>
        /// <exception cref="Exception">Altri errori, viene invocata la disconnessione programmata</exception>
        /// </summary>
        /// <param name="s">Socket creata</param>
        /// <param name="str">Stringa dove salvare il file</param>
        private void Download(Socket s, string str)
        {
            long lenght;

            string fileName = list_box_files.SelectedItem.ToString();

            try
            {
                InvioLettera(s, "d");
                Thread.Sleep(30);
                InvioNomeFile(s, fileName);
                Thread.Sleep(30);
                lenght = GetLenght(s);
                Thread.Sleep(30);
                GetFile(s, str, fileName, lenght);
            }
            catch (SocketException e)
            {
                label9.Text = $"Disconnessione forzata mentre scaricavo {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                his += label9.Text + "\r\n";
                Reset();
                MessageBox.Show(e.Message + "\r\nMi sto scollegando", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ForceDisconnect(socketClient);
            }
            catch (Exception ex)
            {
                his += $"Generato errore: {ex.Message} mentre scaricavo {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                Reset();
                MessageBox.Show(ex.Message + "\r\nMi sto scollegando", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Disconnect(socketClient);
            }

        }/// <summary>
         /// Viene invocato il metodo che invia una lettera. Riceve quindi i dati contenenti 
         /// i file disponibili per il download. Per visualizzarli sulla listbox è necessario fare i seguenti passaggi
         /// 1- Siccome il server per distinguere i file aggiunge uno / alla fine del file è necessario usare
         /// il metodo split 
         /// 2- Una volta fatto ciò è necessario rimuovere l'ultima posizione nell'array in quanto vuota
         /// 3 Si implementa un foreach che aggiunge nella listbox i file
         /// </summary>
         /// /// <exception cref="SocketException">Errore socket, viene invocata la disconnessione forzata</exception>
         /// <param name="s">Socket creata</param>
        private void Visualizza(Socket s)
        {
            byte[] vis = new byte[1024];

            btn_download.Enabled = true;
            list_box_files.Items.Clear();
            try
            {
                InvioLettera(s, "v");
                s.Receive(vis);
                string data = Encoding.ASCII.GetString(vis);
                string[] file = data.Split('/');
                int n = file.Length;
                string[] result = file.Take(n - 1).ToArray();

                foreach (string fileStr in result)
                {
                    list_box_files.Items.Add(fileStr);
                }
            }
            catch (SocketException e)
            {
                his += $"Disconnessione forzata alle ore {DateTime.Now.ToString("HH:mm:ss")}";
                list_box_files.Items.Clear();
                Reset();
                MessageBox.Show(e.Message + "\r\nMi sto scollegando", "Problema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ForceDisconnect(socketClient);
            }
        }
        /// <summary>
        /// Se si preme il tasto x per chiudere la form avviene un controllo che se falso disconnette dal server
        /// </summary>
        #endregion
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!chiuso)
                Disconnect(socketClient);
        }


    }
}
