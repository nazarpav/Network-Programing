using EAGetMail;
using EASendMail;
using System;
using System.Collections.Generic;
using System.Linq;
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
        MailServer mailServer = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //sign in
            Login login = new Login(ref Logins);
            login.Show();
        }
        private void LoadLetters()
        {
            MailClient client = new MailClient("TryIt");

            try
            {
                client.Connect(mailServer);
                UpdateMessage(client);
                UpdateFolders(client);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateFolders(MailClient client)
        {
            Dispatcher.Invoke(() => Folders.Items.Clear());
            foreach (var item in client.Imap4Folders)
            {
                Dispatcher.Invoke(() => Folders.Items.Add(item.Name));
            }
        }
        //get folder info nuget
        private void UpdateMessage(MailClient client)
        {
            Dispatcher.Invoke(() => History.Items.Clear());
            foreach (var item in client.GetMailInfos().Reverse().Take(30))
            {
                Letter l = new Letter(client.GetMail(item));
                Dispatcher.Invoke(() => History.Items.Add(l));
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
            SendLetter sendLetter = new SendLetter(server);
            sendLetter.Show();
        }

        private void History_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //history
            if (server == null)
            {
                MessageBox.Show("Empty");
                return;
            }
            AnswerLetter AL = new AnswerLetter(server, (Letter)History.SelectedItem,mailServer);
            AL.Show();
        }

        private void Logins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                server = new SmtpServer(((LoginPassword)Logins.SelectedItem).SmtpServer)
                {
                    Port = 465,
                    ConnectType = SmtpConnectType.ConnectSSLAuto,
                    User = ((LoginPassword)Logins.SelectedItem).Login,
                    Password = ((LoginPassword)Logins.SelectedItem).Password
                };
                new SmtpClient().Connect(server);
                //Rebex.Licensing.Key = "==ABPXWhuN2LbrRE091StstWMylboZ2Trwx8yrWUOkviHI==";
                //var imap = new Rebex.Net.Imap();
                //imap.Connect(hostname, SslMode.Implicit);
                //imap.Login(username, password);

                mailServer = new MailServer(
                Dispatcher.Invoke(() => ((LoginPassword)Logins.SelectedItem)).ImapServer,
                Dispatcher.Invoke(() => ((LoginPassword)Logins.SelectedItem)).Login,
                Dispatcher.Invoke(() => ((LoginPassword)Logins.SelectedItem)).Password,
                EAGetMail.ServerProtocol.Imap4)
                {
                    SSLConnection = true,
                    Port = 993
                };
                Task.Run(()=>LoadLetters());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //update
            Task.Run(() => LoadLetters());
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MessageBox.Show("Ну всьо до нових встреч!");
        }
    }
}
