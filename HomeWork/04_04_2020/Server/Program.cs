using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    class Program
    {
        private static int localPort;

        static void Main(string[] args)
        {
            try
            {
                localPort = 1020;

                UdpClient client = new UdpClient(localPort);
                while (true)
                {
                    IPEndPoint RemoteIpEndPoint = null;
                    try
                    {
                        Console.WriteLine("\n-----------*******Common room*******-----------");
                        while (true)
                        {
                            byte[] receiveBytes = client.Receive(ref RemoteIpEndPoint);
                            string msg = Encoding.ASCII.GetString(receiveBytes);
                            switch (msg)
                            {
                                case "1":
                                    //Console.WriteLine("Received");
                                    int size = Send(takeScreenShot(), RemoteIpEndPoint, client);
                                    //Console.WriteLine("Sended");
                                    //Console.WriteLine("\n++++++++++++++++++++\n size: " + size + "\n++++++++++++++++++++\n");
                                    break;
                                case "0":
                                    return;
                                default:
                                    Console.WriteLine("Invalid msg");
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception: {ex} \n {ex.Message}");
                    }
                }
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex} \n {ex.Message}");
            }
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        private static int Send(Bitmap bitmap, IPEndPoint endPoint, UdpClient sender)
        {
            byte[] bytes = null;
            try
            {
                bytes = ImageToByte(bitmap);
                sender.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1024));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex} \n {ex.Message}" + "\n++++++++++++++++++++\n size: " + bytes.Length + "\n++++++++++++++++++++\n");
            }
            return bytes.Length;
        }
        private static Bitmap takeScreenShot()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(3700, 800, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                return bmp;
                //bmp.Save("screenshot.png");  // saves the image
            }
        }
    }
}
