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
        public static void StartListening(Socket clientSocket, int n)

        {
            byte[] msg = new byte[1024];
            while (true)
            {
                try
                {
                    if (clientSocket.Connected)
                    {
                        int messaggio = clientSocket.Receive(msg);
                        string data = Encoding.ASCII.GetString(msg, 0, messaggio);
                        switch (data)
                        {
                            case "u":
                                Download(clientSocket, n);
                                break;
                            case "v":
                                VisualizzaFile(clientSocket, n);
                                break;
                            case "d":
                                Upload(clientSocket, n);
                                break;
                            case "e":
                                Disconnect(clientSocket, n);
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
                    Disconnect(clientSocket, n);
                    Console.WriteLine("");
                }
            }
        }
        private static void Disconnect(Socket s, int n)
        {
            s.Close();
            Console.WriteLine($" >> Client N° {n - 1} disconnesso alle ore {DateTime.Now.ToString("HH:mm:ss")}");
            Console.WriteLine("");
        }
        #endregion
        #region upload download visualizza
        private static void Download(Socket s, int n)
        {
            long lenght, bytesSoFar = 0;
            string res;
            byte[] data = new byte[4096];
            byte[] msg = new byte[1024];
            string receivedPath = @"files\";
            try
            {
                int nFile = s.Receive(data);
                fileName = Encoding.ASCII.GetString(data);
                fileName = fileName.Replace("\0", string.Empty);
                s.Receive(msg);
                res = Encoding.ASCII.GetString(msg);
                lenght = Convert.ToInt64(res);
                FileStream fs = new FileStream(receivedPath + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                Console.Write($" >> Scaricando {fileName} da client N°{n - 1}... ");
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
                            Byte progresso = (byte)(bytesSoFar * 100 / lenght);
                            fs.Write(data, 0, nFile);
                            progress.Report((double)progresso / 100);
                        }
                    }
                    progress.Report(1);
                    fs.Flush();
                    fs.Close();
                }
                Console.WriteLine("");
                Console.WriteLine($" >> Ricevuto dal client N°{Convert.ToString(n - 1)}: {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}");
                Console.WriteLine("");
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
        }
        private static void Upload(Socket s, int n)
        {
            byte[] buffer = new byte[1024];
            byte[] msg = new byte[1024];
            byte[] terminator = Encoding.ASCII.GetBytes("<EOF>");
            try
            {
                s.Receive(msg);
                data = Encoding.ASCII.GetString(msg);
                data = data.Replace("\0", string.Empty);
                //Console.WriteLine($" > Caricamente del file {data} al client N°{n - 1}");
                FileStream file = new FileStream(path + data, FileMode.Open);
                long totalBytes = file.Length, bytesSoFar = 0;
                buffer = Encoding.ASCII.GetBytes(Convert.ToString(totalBytes));
                s.Send(buffer);
                byte[] filechunk = new byte[4096];
                int numBytes;
                Console.WriteLine($" >> Inizio download del file {data} da client N°{n - 1}... ");
                using (var progress = new ProgressBar())
                {
                    while ((numBytes = file.Read(filechunk, 0, 4096)) > 0)
                    {
                        if (s.Send(filechunk, numBytes, SocketFlags.None) != numBytes)
                        {
                            throw new Exception(" >> Errore nel trasmettere il file");
                        }
                        bytesSoFar += numBytes;
                        Byte progresso = (byte)(bytesSoFar * 100 / totalBytes);
                        progress.Report((double)progresso / 100);
                    }
                }
                s.Send(terminator);
                file.Close();
                Console.WriteLine($" >> Download di {data} finito alle ore {DateTime.Now.ToString("HH:mm:ss")}");
                Console.WriteLine("");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("");
            }
        }
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
        private static void Main(string[] args)
        {
        prova:
            Console.Clear();
            while (o < 5)
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
                    o++;
                    Console.WriteLine($" >> {e.Message}");
                    Thread.Sleep(5000);
                    goto prova;
                }
            }


        }
    }
}


