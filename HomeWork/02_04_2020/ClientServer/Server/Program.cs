using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Database;

namespace Server
{    
    class Server
    {
        private DALClass _dal;
        public Server()
        {
            _dal = new DALClass();
        }

        public void Start()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1024);


            Socket listener = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream,
                                         ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(0);

                byte[] bytes = null;
                string req = "";

                do
                {
                    Console.WriteLine("Waiting for a request...");

                    Socket handler = listener.Accept();                    

                    req = "";
                    do
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        req += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    } while (handler.Available > 0);

                    Console.WriteLine($"Request : {req} from machine: {handler.RemoteEndPoint}");

                    Thread.Sleep(1500);
                    Console.WriteLine(req);
                    string res = _dal.GetResponse(req);
                    byte[] msg = Encoding.ASCII.GetBytes(res);

                    handler.Send(msg);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                } while (req != "###");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            new Server().Start();
        }
    }
}