using System;
using System.Collections.Generic;
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

namespace Client
{
    class MyData
    {
        public MyData(Socket so)
        {
            buf = new byte[1024];
            socket = so;
        }
        public Socket socket { get; set; }
        public byte[] buf { get; set; }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AsyncReceive(IAsyncResult result)
        {
            MyData s = (MyData)result.AsyncState;
            Socket s2 = s.socket;
            int l = s2.EndReceive(result);
            if (l > 0)
            {
                Dispatcher.Invoke(() => LV.Items.Add(Encoding.ASCII.GetString(s.buf, 0, l)));
            }
            else
            {
                if (s.buf.Length > 1)
                {
                    Dispatcher.Invoke(() => LV.Items.Add("?"));
                }
            }
            s2.Shutdown(SocketShutdown.Both);
            s2.Close();
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(ip, 1024);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Connect(ep);
                if (s.Connected)
                {
                    String strSend = TB.Text;
                    byte[] data = Encoding.ASCII.GetBytes(strSend);
                    s.BeginSend(data, 0, data.Length, 0,new AsyncCallback(SendCallback),s);
                    MyData MD2 = new MyData(s);
                    s.BeginReceive(MD2.buf, 0, 1024, 0, new AsyncCallback(AsyncReceive), MD2);
                }
                else
                    MessageBox.Show("Connection Error");
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
