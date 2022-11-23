using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Server_FTP
{
    class Program
    {
        public static void StartListening(Socket clientSocket, int n)

        {
            byte[] msg = new byte[1024];
            int messaggio = clientSocket.Receive(msg);
            String data = Encoding.ASCII.GetString(msg, 0, messaggio);

            switch (data)
            {
                case "u":
                    Download(clientSocket, n);
                    break;
                case "v":
                    VisualizzaFile(clientSocket);
                    break;
            }
           



        }
        public static void controllo()
        {

        }
        public static void visualizzaFile()
        {
            string path = @"files";

            List<string> lista = Directory.GetFiles(path).ToList();

            foreach (var l in lista)
            {
                Console.WriteLine(l.Substring(6));
            }

        }
        private static void Download(Socket s, int n)
        {
            string receivedPath = @"files\";

            byte[] clientData = new byte[4096];
            int receivedBytesLen = s.Receive(clientData, clientData.Length, 0);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
            Console.WriteLine($" >> Ricevuto da client N° {Convert.ToString(n)}: {fileName}");

            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Create));
            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
            while (receivedBytesLen > 0)
            {
                receivedBytesLen = s.Receive(clientData, clientData.Length, 0);
                if (receivedBytesLen == 0)
                {
                    bWrite.Close();
                }
                else
                {
                    bWrite.Write(clientData, 0, receivedBytesLen);
                }
            }
            bWrite.Close();
        }
        private static void VisualizzaFile(Socket s)
        {
            byte[] msg = new byte[1024];  
            string path = @"files";
            List<string> lista = Directory.GetFiles(path).ToList();
            foreach(string file in lista)
            {
                string a = file.Substring(6);
                msg = Encoding.ASCII.GetBytes(a);
            }
            
            int bytestSent = s.Send(msg);
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
            Console.WriteLine(" >> Server Started");

            while (true)

            {

                counter += 1;


                Socket clientSocket = serverSocket.Accept();

                Console.WriteLine($" >> Client N°  {Convert.ToString(counter)} connected");


                /*new Thread(delegate ()
                {
                    doChat(clientSocket, counter);
                }).Start();
                */
                Thread thread = new Thread(() => StartListening(clientSocket, counter));
                thread.Start();
                visualizzaFile();

            }


        }

    }
}

