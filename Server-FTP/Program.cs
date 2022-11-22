using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server_FTP
{
    class Program
    {
        public static void doChat(Socket clientSocket, int n)

        {
            string receivedPath = @"files\";
            byte[] clientData = new byte[4096];
            clientSocket.ReceiveBufferSize = 3072;
            int receivedBytesLen = clientSocket.Receive(clientData, clientData.Length, 0);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = Encoding.UTF8.GetString(clientData, 4, fileNameLen);
            Console.WriteLine($" >> Ricevuto da client N° {Convert.ToString(n-1)} : {fileName}");

            BinaryWriter bWrite = new BinaryWriter(File.Open(receivedPath + fileName, FileMode.Create));
            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
            while (receivedBytesLen > 0)
            {
                receivedBytesLen = clientSocket.Receive(clientData, clientData.Length, 0);
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
            clientSocket.Close();





            /*Console.WriteLine("getting file....");
            byte[] clientData = new byte[1024 * 5000];
            int receivedBytesLen = clientSocket.Receive(clientData);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
            BinaryWriter bWrite = new BinaryWriter(File.Open(fileName + n, FileMode.Create));
            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
            bWrite.Close();
            clientSocket.Close();
            */
            //[0]filenamelen[4]filenamebyte[*]filedata   

        }
        
        public static void visualizzaFile()
        {
            string path = @"files";

            var lista = Directory.GetFiles(path);

            foreach(var l in lista)
            {
                Console.WriteLine(l.Substring(6));
            }

        }
        





        static void Main(string[] args)

        {
            visualizzaFile();
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


                new Thread(delegate ()
                {
                    doChat(clientSocket, counter);
                }).Start();



            }


        }

    }
}

