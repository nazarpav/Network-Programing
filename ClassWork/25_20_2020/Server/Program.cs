using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
                localPort = 1024;

                while (true)
                {
                    UdpClient receivingUdpClient = new UdpClient(localPort);
                    IPEndPoint RemoteIpEndPoint = null;
                    try
                    {
                        Console.WriteLine("\n-----------*******Common room*******-----------");
                        while (true)
                        {
                            byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                            string msg = Encoding.ASCII.GetString(receiveBytes);
                            switch (msg)
                            {
                                case "1":
                                    Send(takeScreenShot(), RemoteIpEndPoint);
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
        private static void Send(Bitmap bitmap, IPEndPoint endPoint)
        {
            UdpClient sender = new UdpClient();
            try
            {
                byte[] bytes = ImageToByte(bitmap);
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex} \n {ex.Message}");
            }
            finally
            {
                sender.Close();
            }
        }
        private static Bitmap takeScreenShot()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                return bmp;
                //bmp.Save("screenshot.png");  // saves the image
            }
        }
    }
}
