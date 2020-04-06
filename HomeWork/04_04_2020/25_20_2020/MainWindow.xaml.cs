using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace _25_20_2020
{
    public partial class MainWindow : Window
    {
        UdpClient client = new UdpClient(1024);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Send("1", client);
            Receiver(client);


        }

        private void Send(string datagram, UdpClient sender)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1020);
            byte[] bytes = Encoding.ASCII.GetBytes(datagram);
            sender.BeginSend(bytes, bytes.Length, endPoint, (IAsyncResult res) => { ((UdpClient)res.AsyncState).EndSend(res); }, sender);
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex} \n {ex.Message}");
            }
            finally
            {
                //sender.Close();
            }
        }
        private void EndReceive(IAsyncResult res)
        {
            UdpState st = (UdpState)res.AsyncState;
            BitmapImage biImg = new BitmapImage();
            biImg.BeginInit();
            var f = st.UdpClient.EndReceive(res, ref st.endPoint);
            biImg.StreamSource = new MemoryStream(f);
            biImg.EndInit();
            var imSor = biImg as ImageSource;
            st.Image = imSor;
        }
        public void Receiver(UdpClient receivingUdpClient)
        {
            IPEndPoint RemoteIpEndPoint = null;
            UdpState s = new UdpState();
            s.endPoint = RemoteIpEndPoint;
            s.UdpClient = receivingUdpClient;
            receivingUdpClient.BeginReceive(EndReceive, s).AsyncWaitHandle.WaitOne();
            Image_.Source = s.Image;

            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex} \n {ex.Message}");
            }
            finally
            {
                // receivingUdpClient.Close();
            }
        }
    }
    public class UdpState
    {
        public UdpClient UdpClient { get; set; }
        public IPEndPoint endPoint;
        public ImageSource Image { get; set; }
    }
}
