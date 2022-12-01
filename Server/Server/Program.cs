using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program

    {
        static string fileName = "";
        static string data = "";
        static readonly string path = @"files";
        static bool attivo = false;

        public static void StartListening(Socket clientSocket, int n)

        {
            string data = "";
            byte[] msg = new byte[1024];
            int messaggio = 0;
            while (true)
            {
                try
                {
                    if (clientSocket.Connected)
                    {
                        messaggio = clientSocket.Receive(msg);
                        data = Encoding.ASCII.GetString(msg, 0, messaggio);
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
                }

            }


        }
        private static void Download(Socket s, int n)
        {
            byte[] data = new byte[1024 * 7];
            string receivedPath = @"files\";
            try
            {
                int nFile = s.Receive(data);
                fileName = Encoding.ASCII.GetString(data);
                fileName = fileName.Replace("\0", string.Empty);
                FileStream fs = new FileStream(receivedPath + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                Console.WriteLine($" > Scaricando {fileName} da client N°{n - 1}... ");
                int i = 0;
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
                        fs.Write(data, 0, nFile);
                    }
                }
                fs.Flush();
                fs.Close();


                Console.WriteLine($" >> Ricevuto dal client N°{Convert.ToString(n - 1)}: {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}");


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void Upload(Socket s, int n)
        {
            byte[] msg = new byte[1024];
            byte[] terminator = Encoding.ASCII.GetBytes("<EOF>");
            try
            {
                s.Receive(msg);
                data = Encoding.ASCII.GetString(msg);
                data = data.Replace("\0", string.Empty);
                Console.WriteLine($" > Caricamente del file {data} al client N°{n - 1}");
                s.SendFile(@"files/" + data);
                s.Send(terminator);
                Console.WriteLine($" >> Download effettuato dal client N°{n - 1} di {data} alle ore {DateTime.Now.ToString("HH:mm:ss")}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                //s.Send(terminator);
                Console.WriteLine($" >> Richista la lista di file disponibili dal client N°{n - 1} alle ore {DateTime.Now.ToString("HH:mm:ss")}");

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
        private static void Disconnect(Socket s, int n)
        {
            s.Close();
            Console.WriteLine($" >> Client N° {n - 1} disconnesso alle ore {DateTime.Now.ToString("HH:mm:ss")}");
        }

        static void Main(string[] args)
        {
            Console.Write("Non servo a nulla. spero qualcuno possa aiutarmi... ");
            using (var progress = new ProgressBar())
            {
                for (int i = 0; i <= 100; i++)
                {
                    progress.Report((double)i / 100);
                    Thread.Sleep(20);
                }
            }
            Console.WriteLine("Done.");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 5000);
            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); ;
            serverSocket.Bind(ipEnd);
            int counter = 0;
            serverSocket.Listen(100);
            Console.WriteLine($" >> Server started and listening on port {ipEnd.Port} alle ore {DateTime.Now.ToString("HH:mm:ss")}");

            while (true)

            {

                counter += 1;


                Socket clientSocket = serverSocket.Accept();

                Console.WriteLine($" >> Client N°{Convert.ToString(counter)} connected alle ore {DateTime.Now.ToString("HH:mm:ss")}");
                Thread thread = new Thread(() => StartListening(clientSocket, counter));
                thread.Start();

            }


        }

    }
}


