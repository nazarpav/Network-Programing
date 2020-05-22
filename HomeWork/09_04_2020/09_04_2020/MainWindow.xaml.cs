using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace _09_04_2020
{
    public partial class MainWindow : Window
    {
        private UdpClient clientReciever = new UdpClient();
        private UdpClient clientSender = new UdpClient();
        private int SERVER_PORT = 745;
        private int MY_PORT;
        private byte[] PlayingField = new byte[10];
        private byte mySymbol;
        private bool IsMyMove;
        public MainWindow()
        {
            InitializeComponent();
            clientReciever.Client.ReceiveBufferSize = 1048576;//2^20
        }
        private void ReceivePlayerField()
        {
            while (true)
            {
                IPEndPoint endPoint = null;
                byte[] res = clientReciever.Receive(ref endPoint);
                lock (PlayingField)
                {
                    for (int i = 0; i < res.Length; i++)
                    {
                        PlayingField[i] = res[i];
                    }
                }
                if (PlayingField[0] == 11)
                {
                    mySymbol = 1;
                    IsMyMove = false;
                    PlayingField[9] = mySymbol;
                    PlayingField[0] = 0;
                }
                else if (PlayingField[0] == 12)
                {
                    mySymbol = 2;
                    IsMyMove = true;
                    PlayingField[9] = mySymbol;
                    PlayingField[0] = 0;
                }
                else if (PlayingField[0] == 111)
                {
                    //win
                    MessageBox.Show("You win!");
                    for (int i = 0; i < res.Length; i++)
                    {
                        PlayingField[i] = 0;
                    }
                    RenderMyPlayingField();
                    Dispatcher.Invoke(() => { Wins.Text = (int.Parse(Wins.Text) + 1).ToString(); });
                }
                else if (PlayingField[0] == 112)
                {
                    //lose
                    MessageBox.Show("You lose!");
                    for (int i = 0; i < res.Length; i++)
                    {
                        PlayingField[i] = 0;
                    }
                    RenderMyPlayingField();
                    Dispatcher.Invoke(() => { Loses.Text = (int.Parse(Loses.Text) + 1).ToString(); });
                }
                else
                {
                    IsMyMove = true;
                    RenderMyPlayingField();
                }
            }
        }
        private void ResetPlayingField()
        {
            for (int i = 0; i < PlayingField.Length; i++)
            {
                PlayingField[i] = 0;
            }
        }
        private void RenderMyPlayingField()
        {
            Dispatcher.Invoke(() =>
            {
                if (PlayingField[0] == 1)
                    _00.Content = "O";
                else if (PlayingField[0] == 2)
                    _00.Content = "X";
                else if (PlayingField[0] == 0)
                    _00.Content = "";

                if (PlayingField[1] == 1)
                    _01.Content = "O";
                else if (PlayingField[1] == 2)
                    _01.Content = "X";
                else if (PlayingField[1] == 0)
                    _01.Content = "";

                if (PlayingField[2] == 1)
                    _02.Content = "O";
                else if (PlayingField[2] == 2)
                    _02.Content = "X";
                else if (PlayingField[2] == 0)
                    _02.Content = "";

                if (PlayingField[3] == 1)
                    _10.Content = "O";
                else if (PlayingField[3] == 2)
                    _10.Content = "X";
                else if (PlayingField[3] == 0)
                    _10.Content = "";

                if (PlayingField[4] == 1)
                    _11.Content = "O";
                else if (PlayingField[4] == 2)
                    _11.Content = "X";
                else if (PlayingField[4] == 0)
                    _11.Content = "";

                if (PlayingField[5] == 1)
                    _12.Content = "O";
                else if (PlayingField[5] == 2)
                    _12.Content = "X";
                else if (PlayingField[5] == 0)
                    _12.Content = "";

                if (PlayingField[6] == 1)
                    _20.Content = "O";
                else if (PlayingField[6] == 2)
                    _20.Content = "X";
                else if (PlayingField[6] == 0)
                    _20.Content = "";

                if (PlayingField[7] == 1)
                    _21.Content = "O";
                else if (PlayingField[7] == 2)
                    _21.Content = "X";
                else if (PlayingField[7] == 0)
                    _21.Content = "";

                if (PlayingField[8] == 1)
                    _22.Content = "O";
                else if (PlayingField[8] == 2)
                    _22.Content = "X";
                else if (PlayingField[8] == 0)
                    _22.Content = "";
            });
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!IsMyMove)
            {
                MessageBox.Show("Opponent move");
                return;
            }
            switch (((Button)sender).Name)
            {
                case "_00":
                    if (_00.Content == null || _00.Content as string == "") PlayingField[0] = mySymbol;
                    break;
                case "_01":
                    if (_01.Content == null || _01.Content as string == "") PlayingField[1] = mySymbol;
                    break;
                case "_02":
                    if (_02.Content == null || _02.Content as string == "") PlayingField[2] = mySymbol;
                    break;
                ///////////////
                case "_10":
                    if (_10.Content == null || _10.Content as string == "") PlayingField[3] = mySymbol;
                    break;
                case "_11":
                    if (_11.Content == null || _11.Content as string == "") PlayingField[4] = mySymbol;
                    break;
                case "_12":
                    if (_12.Content == null || _12.Content as string == "") PlayingField[5] = mySymbol;
                    break;
                ///////////////
                case "_20":
                    if (_20.Content == null || _20.Content as string == "") PlayingField[6] = mySymbol;
                    break;
                case "_21":
                    if (_21.Content == null || _21.Content as string == "") PlayingField[7] = mySymbol;
                    break;
                case "_22":
                    if (_22.Content == null || _22.Content as string == "") PlayingField[8] = mySymbol;
                    break;
                default:
                    MessageBox.Show("Eror Invalit press button☺");
                    break;
            }
            IsMyMove = false;
            clientSender.Send(PlayingField, PlayingField.Length, new IPEndPoint(IPAddress.Parse(ServerIPAddress.Text), SERVER_PORT));
            RenderMyPlayingField();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress.Parse(ServerIPAddress.Text);
                MY_PORT = int.Parse(MyPort.Text);
                SERVER_PORT = int.Parse(ServerPort.Text);
                clientReciever.Client.Bind(new IPEndPoint(IPAddress.Parse(MyIPAddress.Text), MY_PORT));
                clientReciever.Send(PlayingField, PlayingField.Length, new IPEndPoint(IPAddress.Parse(ServerIPAddress.Text), SERVER_PORT));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            ServerIPAddress.IsEnabled = false;
            MyIPAddress.IsEnabled = false;
            MyPort.IsEnabled = false;
            ServerPort.IsEnabled = false;
            ResetPlayingField();
            ThreadPool.QueueUserWorkItem((obj) => ReceivePlayerField());
        }
    }
}
