using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _25_20_2020
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Send(string datagram)
        {
            UdpClient sender = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(IPAddress,"1024");
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(datagram);
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

        public void Receiver()
        {
            UdpClient receivingUdpClient = new UdpClient(1024);
            IPEndPoint RemoteIpEndPoint = null;
            try
            {
                Console.WriteLine("\n-----------*******Common room*******-----------");
                while (true)
                {
                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);
                    Image_.=((Bitmap)(new ImageConverter().ConvertFrom(receiveBytes)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex} \n {ex.Message}");
            }
        }
    }
}
