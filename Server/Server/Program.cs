using System.Net;
using System.Net.Sockets;
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
            string receivedPath = @"files\";
            byte[] clientData = new byte[4096];
            int receivedBytesLen = s.Receive(clientData, clientData.Length, 0);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);

            using (var bWrite = File.Open(receivedPath + fileName, FileMode.OpenOrCreate))
            {
                bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
                while (receivedBytesLen > 0)
                {
                    receivedBytesLen = s.Receive(clientData, clientData.Length, 0);
                    string data = Encoding.ASCII.GetString(clientData);
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

                Console.WriteLine($" >> Ricevuto da client N° {Convert.ToString(n-1)}: {fileName}");


            }

        }
        private static void Upload(Socket s, int n)
        {
            byte[] msg = new byte[1024];
            int messaggio = 0;
            string data = "";
            byte[] terminator = Encoding.ASCII.GetBytes("CIAO");
            messaggio = s.Receive(msg);
            data = Encoding.ASCII.GetString(msg, 0, messaggio);
            byte[] fileNameByte = Encoding.ASCII.GetBytes(data);
            byte[] fileData = File.ReadAllBytes($"files/{data}");
            byte[] clientData = new byte[fileNameByte.Length + 4 + fileData.Length];
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);
            s.Send(clientData, 0, clientData.Length, 0);
            s.Send(terminator);
            Console.WriteLine($" >> Download effettuato dal client N°{n-1} di {data}");
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


