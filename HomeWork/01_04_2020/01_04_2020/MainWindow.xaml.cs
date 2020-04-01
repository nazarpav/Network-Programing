using EAGetMail;
using EASendMail;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace _01_04_2020
{
    public class Letter
    {
        public Mail letter { get; set; }
        public Letter(Mail letter)
        {
            this.letter = letter;
        }
        public override string ToString()
        {
            return letter.From + " | " + letter.Subject;
        }
    }
    public partial class MainWindow : Window
    {
        SmtpServer server = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //sign in
            try
            {
                server = new SmtpServer("smtp.gmail.com")
                {
                    Port = 465,
                    ConnectType = SmtpConnectType.ConnectSSLAuto,
                    User = Login.Text,
                    Password = Password.Password
                };
                new SmtpClient().Connect(server);
                LoadLetters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void LoadLetters()
        {
            MailClient client = new MailClient("TryIt");

            try
            {
                MailServer server = new MailServer(
                "imap.gmail.com",
                Login.Text,
                Password.Password,
                EAGetMail.ServerProtocol.Imap4)
                {
                    SSLConnection = true,
                    Port = 993
                };
                client.Connect(server);

                foreach (var item in client.GetMailInfos().Skip(20).Take(10))
                {
                    Letter l= new Letter(client.GetMail(item));
                    History.Items.Add(l);
                }

                //foreach (var m in messages)
                //{
                //    Console.WriteLine($"Index: {m.Index}{Environment.NewLine}Size: {m.Size}");
                //    Console.WriteLine(Environment.NewLine);

                //    Mail message = client.GetMail(m);

                //    Console.WriteLine($"From: {message.From}\n\n\t{message.Subject}");
                //    Console.WriteLine("-----------------------------------------------------");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //send
            if (server == null)
            {
                MessageBox.Show("Please log in");
                return;
            }
            Window1 sendLetter = new Window1(server);
            sendLetter.Show();
        }

        private void History_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(server==null)
            {
                MessageBox.Show("Empty");
                return;
            }
            AnswerLetter AL = new AnswerLetter(server,(Letter)History.SelectedItem);
            AL.Show();
        }
    }
}
