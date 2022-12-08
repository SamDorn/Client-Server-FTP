using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        private static string fileName = "";
        private static string data = "";
        private static readonly string path = @"files/";
        private static bool attivo = false;
        private static int o = 0;
        #region connessione disconnessione
        /// <summary>
        /// Viene implementato uno switch in quanto il client
        /// prima di effettuare ogni operazione invia una lettera
        /// in modo che il server possa gestirsi.
        /// </summary>
        /// <param name="s">Socket client</param>
        /// <param name="n"></param>
        public static void StartListening(Socket s, int n)
        {
            byte[] msg = new byte[1024];
            while (true)
            {
                try
                {
                    if (s.Connected)
                    {
                        int messaggio = s.Receive(msg);
                        string data = Encoding.ASCII.GetString(msg, 0, messaggio);
                        switch (data)
                        {
                            case "u":
                                Download(s, n);
                                break;
                            case "v":
                                VisualizzaFile(s, n);
                                break;
                            case "d":
                                Upload(s, n);
                                break;
                            case "e":
                                Disconnect(s, n);
                                attivo = true;
                                break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Disconnect(s, n);
                    Console.WriteLine("");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">Socket client</param>
        /// <param name="n">Counter numero client</param>
        private static void Disconnect(Socket s, int n)
        {
            s.Close();
            Console.WriteLine($" >> Client N° {n - 1} disconnesso alle ore {DateTime.Now.ToString("HH:mm:ss")}");
            Console.WriteLine("");
        }
        #endregion
        #region metodi globali
        /// <summary>
        /// Riceve la lunghezza del file
        /// </summary>
        /// <param name="s">Socket creata</param>
        /// <returns></returns>
        private static long GetLenght(Socket s)
        {
            byte[] data = new byte[1024];
            s.Receive(data);
            string res = Encoding.ASCII.GetString(data);
            long lenght = Convert.ToInt64(res);
            return lenght;
        }/// <summary>
         /// Riceve il nome del file
         /// </summary>
         /// <param name="s">Socket client</param>
         /// <returns></returns>
        private static string GetFileName(Socket s)
        {
            byte[] data = new byte[1024];
            s.Receive(data);
            string str = Encoding.ASCII.GetString(data);
            str = str.Replace("\0", string.Empty);
            return str;

        }/// <summary>
         /// Viene adoperata la classe FileStream per scrivere i byte che arrivano
         /// Viene fatto un controllo sulla receive, se il client ha inviato il terminator non ricevere più nulla 
         /// ed esci dal while, viene chiuso il file e viene aggiornata la barra progressi.
         /// </summary>
         /// <param name="s">Socket client</param>
         /// <param name="str">Nome file</param>
         /// <param name="n">Counter client</param>
         /// <param name="lenght">Lunghezza file</param>
        private static void GetFile(Socket s, string str, int n, long lenght)
        {
            long bytesSoFar = 0;
            byte[] data = new byte[4096];
            int nFile = 0;
            FileStream fs = new FileStream(path + str, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Console.Write($" >> Scaricando {str} da client N°{n - 1}... ");
            using (var progress = new ProgressBar())
            {
                while (true)
                {
                    nFile = s.Receive(data);
                    string bre = Encoding.ASCII.GetString(data);
                    if (bre.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                    else
                    {
                        bytesSoFar += nFile;
                        double progresso = (double)(bytesSoFar * 100 / lenght);
                        fs.Write(data, 0, nFile);
                        progress.Report((double)progresso / 100);
                    }
                }
                progress.Report(1);
                fs.Flush();
                fs.Close();
            }
            Console.WriteLine("");
            Console.WriteLine($" >> Ricevuto dal client N°{Convert.ToString(n - 1)}: {str} alle ore {DateTime.Now.ToString("HH:mm:ss")}");
            Console.WriteLine("");
        }
        /// <summary>
        /// Invio del terminatore
        /// </summary>
        /// <param name="s">Socket client</param>
        private static void InvioTerminator(Socket s)
        {
            byte[] data = Encoding.ASCII.GetBytes("<EOF>");
            s.Send(data);
        }
        /// <summary>
        /// Viene ricevuto il nome del file e gli /0 vengono rimpiazzati con nulla
        /// </summary>
        /// <param name="s">Socket client</param>
        /// <returns></returns>
        private static string ReceiveFileName(Socket s)
        {
            byte[] msg = new byte[1024];
            s.Receive(msg);
            string nome = Encoding.ASCII.GetString(msg);
            return nome.Replace("\0", string.Empty);
        }
        private static void InvioLunghezzaFile(Socket s, long lenght)
        {
            byte[] data = Encoding.ASCII.GetBytes(Convert.ToString(lenght));
            s.Send(data);
        }/// <summary>
         /// Il file viene spezzettato, con un while, in blocchi da 4096 byte e vengono inviati
         /// Viene fatto per poter lavorare con file di grandi dimensioni
         /// Viene applicato un controllo alla send
         /// Viene creato un int progress che servirà a tenere traccia dello stato della send grazie alla classe ProgressBar
         /// Viene aggiornata la progress bar
         /// Viene chiuso il file
         /// </summary>
         /// <param name="s">Socket client</param>
         /// <param name="file">FileStream del file</param>
         /// <param name="str">Nome file</param>
         /// <param name="n">Counter client</param>
        private static void InvioFile(Socket s, FileStream file, string str, int n)
        { 
            
            long lenght = file.Length, bytesSoFar = 0;
            byte[] filechunk = new byte[4096];
            int numBytes;
            Console.Write($" >> Inizio download del file {str} da client N°{n - 1}... ");
            using (var progress = new ProgressBar())
            {
                while ((numBytes = file.Read(filechunk, 0, 4096)) > 0)
                {
                    if (s.Send(filechunk, numBytes, SocketFlags.None) != numBytes)
                    {
                        throw new Exception(" >> Errore nel trasmettere il file");
                    }
                    bytesSoFar += numBytes;
                    int progresso = (int)(bytesSoFar * 100 / lenght);
                    progress.Report((double)progresso / 100);
                }
            }

            file.Close();
            Console.WriteLine("");
            Console.WriteLine($" >> Download di {str} finito alle ore {DateTime.Now.ToString("HH:mm:ss")}");
            Console.WriteLine("");
        }
        #endregion
        #region upload download visualizza
        /// <summary>
        /// Viene invocato il metodo per la ricezione del nome del file
        /// Viene invocato il metodo per la ricezione della lunghezza del file
        /// Viene invocato il metodo per la ricezione del file
        /// </summary>
        /// <exception cref="SocketException">Errore socket, viene invocata la disconnessione</exception>
        /// <exception cref="Exception">Altri errori</exception>
        /// <param name="s">Socket client</param>
        /// <param name="n">Counter client</param>
        private static void Download(Socket s, int n)
        {
            try
            {

                string nome = GetFileName(s);
                Thread.Sleep(30);
                long lenght = GetLenght(s);
                Thread.Sleep(30);
                GetFile(s, nome, n, lenght);
            }
            catch (SocketException)
            {
                Disconnect(s, n);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("");
            }


        }/// <summary>
        /// Viene einvocato il metodo che riceve il nome del file
        /// Viene creato l'oggetto file dalla classe FileStream
        /// Viene inviata la lunghezza del file
        /// Viene inviato il file
        /// Viene inviato il terminator
        /// </summary>
        /// <param name="s">Socket client</param>
        /// <param name="n">Counter client</param>
        private static void Upload(Socket s, int n)
        {
            try
            {
                string nome = ReceiveFileName(s);
                Thread.Sleep(30);
                FileStream file = new FileStream(path + nome, FileMode.Open);
                long lenght = file.Length;
                InvioLunghezzaFile(s, lenght);
                Thread.Sleep(30);
                InvioFile(s, file, nome, n);
                Thread.Sleep(30);
                InvioTerminator(s);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("");
            }
        }
        /// <summary>
        /// Metodo che serve per immagazzinare e mandare la lista dei file all'interno della cartella /files
        /// I diversi file vengono divisi da un /
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        private static void VisualizzaFile(Socket s, int n)
        {
        controllo:
            try
            {
                string a = "";
                byte[] msg = new byte[1024];
                byte[] terminator = Encoding.ASCII.GetBytes("<EOF>");
                string path = @"files";
                List<string> lista = Directory.GetFiles(path).ToList();
                foreach (string file in lista)
                {
                    a += file.Substring(6);
                    a += "/";
                }
                msg = Encoding.ASCII.GetBytes(a);
                s.Send(msg);
                Console.WriteLine($" >> Richista la lista di file disponibili dal client N°{n - 1} alle ore {DateTime.Now.ToString("HH:mm:ss")}");
                Console.WriteLine("");
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(@"files/");
                goto controllo;
            }
            catch (Exception)
            {
                Disconnect(s, n);
            }
        }
        #endregion
        /// <summary>
        /// Metodo Main che si mette in ascolto per connessioni in entrata e per ogni accept crea un nuovo thread
        /// cos' da gestire più client
        /// <exception cref="Exception"></exception>
        /// </summary>
        private static void Main(string[] args)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 5000);

            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverSocket.Bind(ipEnd);
                int counter = 0;
                serverSocket.Listen(100);
                Console.WriteLine($" >> Server partito e in ascolto sulla porta {ipEnd.Port} alle ore {DateTime.Now.ToString("HH:mm:ss")}");
                Console.WriteLine("");
                while (true)

                {
                    try
                    {
                        counter += 1;
                        Socket clientSocket = serverSocket.Accept();

                        Console.WriteLine($" >> Client N°{Convert.ToString(counter)} connesso alle ore {DateTime.Now.ToString("HH:mm:ss")}");
                        Console.WriteLine("");
                        Thread thread = new Thread(() => StartListening(clientSocket, counter));
                        thread.Start();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("");
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($" >> {e.Message}");
            }
        }
    }
}
