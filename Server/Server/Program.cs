using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Server_FTP
{
    class Program

    {

        public static void StartListening(Socket clientSocket, int n)

        {
            string data = "";
            byte[] msg = new byte[1024];
            while (true)
            {

                int messaggio = 0;

                messaggio = clientSocket.Receive(msg);
                data = Encoding.ASCII.GetString(msg, 0, messaggio);
                switch (data)

                {
                    case "u":
                        Download(clientSocket, n);
                        data = "";
                        break;
                    case "v":
                        VisualizzaFile(clientSocket, n);
                        data = "";
                        break;
                    case "d":
                        Upload(clientSocket, n);
                        break;
                }


            }

        }
        private static void Download(Socket s, int n)
        {
            byte[] data = new byte[4096];
            string receivedPath = @"files\";
            int nFile = s.Receive(data);
            string fileName = Encoding.ASCII.GetString(data);
            fileName = fileName.Replace("\0", string.Empty);
            Console.WriteLine(fileName);
            FileStream fs = new FileStream(receivedPath + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            while (true)
            {
                nFile = s.Receive(data);
                string bre = Encoding.ASCII.GetString(data);
                if (bre.IndexOf("CIAO") > -1)
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
            Console.WriteLine($" >> Ricevuto da client N° {Convert.ToString(n - 1)}: {fileName} alle ore {DateTime.Now.ToString("HH:mm:ss")}");

        }
        private static void Upload(Socket s, int n)
        {
            byte[] msg = new byte[1024];
            byte[] terminator = Encoding.ASCII.GetBytes("CIAO");
            int messaggio = 0;
            messaggio = s.Receive(msg);
            string data = Encoding.ASCII.GetString(msg);
            data = data.Replace("\0", string.Empty);
            s.SendFile(@"files/" + data);
            s.Send(terminator);
            Console.WriteLine($" >> Download effettuato dal client N°{n - 1} di {data} alle ore {DateTime.Now.ToString("HH:mm:ss")}");
        }




        private static void VisualizzaFile(Socket s, int n)
        {
            string a = "";
            byte[] msg = new byte[1024];
            string path = @"files";
            List<string> lista = Directory.GetFiles(path).ToList();
            foreach (string file in lista)
            {
                a += file.Substring(6);
                a += "/";
            }
            msg = Encoding.ASCII.GetBytes(a);
            s.Send(msg);
            Console.WriteLine($" >> Richista la lista di file disponibili dal client N°{n - 1}");
        }


        static void Main(string[] args)
        {
            string path = @"files";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 5000);
            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); ;
            serverSocket.Bind(ipEnd);
            int counter = 0;
            serverSocket.Listen(100);
            Console.WriteLine($" >> Server started and listening on port {ipEnd.Port}");

            while (true)

            {

                counter += 1;


                Socket clientSocket = serverSocket.Accept();

                Console.WriteLine($" >> Client N°  {Convert.ToString(counter)} connected");
                Thread thread = new Thread(() => StartListening(clientSocket, counter));
                thread.Start();

            }


        }

    }
}


