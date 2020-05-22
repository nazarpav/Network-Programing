using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static UdpClient client = new UdpClient();
        private static IPEndPoint player1 = null;
        private const byte player1Symbol = 1;
        private static IPEndPoint player2 = null;
        private const byte player2Symbol = 2;
        private static int ReceivePort = 745;
        private static bool isBusy = false;
        static void Main(string[] args)
        {
            Console.WriteLine("Enter server receive port: ");
            while (true)
            {
                try
                {
                    int.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid Port!");
                }
            }

            client.Client.Bind(new IPEndPoint(IPAddress.Any, ReceivePort));
            Console.WriteLine("Receive new players...");
            StartServer();
            while (RecivePlayer())
            {
                StartServer();
            }
            client.Close();
        }
        private static void StartServer()
        {
            IPEndPoint player = null;
            client.Receive(ref player);
            player1 = player;
            Console.WriteLine("Player 1 is connected;");
            player = null;
            SendPlayingFieldToPlayer(player1, new byte[9] { 11, 0, 0, 0, 0, 0, 0, 0, 0 });
            client.Receive(ref player);
            player2 = player;
            SendPlayingFieldToPlayer(player2, new byte[9] { 12, 0, 0, 0, 0, 0, 0, 0, 0 });
            Console.WriteLine("Player 2 is connected;");
            isBusy = true;
        }
        private static bool CheckWin(byte[] F)
        {
            return (F[0] == 1 || F[0] == 2) && F[0] == F[1] && F[2] == F[0] ||
                (F[3] == 1 || F[3] == 2) && F[3] == F[4] && F[5] == F[3] ||
                (F[6] == 1 || F[6] == 2) && F[6] == F[7] && F[8] == F[6]
                ||
                (F[0] == 1 || F[0] == 2) && F[0] == F[3] && F[6] == F[0] ||
                (F[1] == 1 || F[1] == 2) && F[1] == F[4] && F[7] == F[1] ||
                (F[2] == 1 || F[2] == 2) && F[2] == F[5] && F[8] == F[2]
                ||
                (F[0] == 1 || F[0] == 2) && F[0] == F[4] && F[8] == F[0] ||
                (F[2] == 1 || F[2] == 2) && F[2] == F[4] && F[6] == F[2];
        }
        private static bool RecivePlayer()
        {
            try
            {
                while (true)
                {
                    IPEndPoint player = new IPEndPoint(IPAddress.Any, ReceivePort);
                    byte[] res = client.Receive(ref player);
                    if (res[0] == 200)
                    {
                        return true;
                    }
                    if (res[0] != 201)
                    {
                        if (res[9] == player1Symbol)
                        {
                            SendPlayingFieldToPlayer(player2, res.Take(9).ToArray());
                        }
                        else
                        {
                            SendPlayingFieldToPlayer(player1, res.Take(9).ToArray());
                        }
                        Console.WriteLine("Resend game field☻");
                        if (CheckWin(res))
                        {
                            if (res[9] == player1Symbol)
                            {
                                SendPlayingFieldToPlayer(player1, new byte[9] { 111, 0, 0, 0, 0, 0, 0, 0, 0 });
                                SendPlayingFieldToPlayer(player2, new byte[9] { 112, 0, 0, 0, 0, 0, 0, 0, 0 });
                            }
                            else
                            {
                                SendPlayingFieldToPlayer(player1, new byte[9] { 112, 0, 0, 0, 0, 0, 0, 0, 0 });
                                SendPlayingFieldToPlayer(player2, new byte[9] { 111, 0, 0, 0, 0, 0, 0, 0, 0 });
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Game ended with Error ☺");
                return false;
            }
        }
        private static void SendPlayingFieldToPlayer(IPEndPoint player, byte[] PField)
        {
            client.Send(PField, PField.Length, player);
        }
    }
}
