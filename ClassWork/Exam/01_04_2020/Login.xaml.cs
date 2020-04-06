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
using System.Windows.Shapes;

namespace _01_04_2020
{
    public class LoginPassword
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ImapServer { get; set; }
        public string SmtpServer { get; set; }
        public LoginPassword(string Login, string Password, string ImapServer, string SmtpServer)
        {
            this.Login = Login;
            this.Password = Password;
            this.ImapServer = ImapServer;
            this.SmtpServer = SmtpServer;
        }
        public override string ToString()
        {
            return Login;
        }
    }
    public partial class Login : Window
    {
        int counter = 1;
        SmtpServer server = null;
        ItemCollection Logins;
        public Login(ref ComboBox Logins)
        {
            InitializeComponent();
            this.Logins = Logins.Items;
            LoadServers();
        }
        private void LoadServers()
        {
            Servers.Items.Add("Gmail");
            Servers.Items.Add("UkrNet");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Servers.SelectedItem.ToString())
                {
                    case "Gmail":
                        server = new SmtpServer("smtp.gmail.com")
                        {
                            Port = 465,
                            ConnectType = SmtpConnectType.ConnectSSLAuto,
                            User = Login_.Text,
                            Password = Password.Password
                        };
                        new SmtpClient().Connect(server);
                        Logins.Add(new LoginPassword(Login_.Text, Password.Password, "imap.gmail.com", "smtp.gmail.com"));
                        break;
                    case "UkrNet":
                        server = new SmtpServer("smtp.ukr.net")
                        {
                            Port = 465,
                            ConnectType = SmtpConnectType.ConnectSSLAuto,
                            User = Login_.Text,
                            Password = Password.Password
                        };
                        new SmtpClient().Connect(server);
                        Logins.Add(new LoginPassword(Login_.Text, Password.Password, "imap.ukr.net", "smtp.ukr.net"));
                        break;
                    default:
                        break;
                }
                MessageBox.Show("Привіт "+Login_.Text);
            }
            catch (Exception ex)
            {
                if (counter % 3 == 0)
                {
                    MessageBox.Show("Вийди от сюда РОЗБІЙНИК!");
                }
                else
                {
                    MessageBox.Show("Невірний логін і пароль!!"+ counter % 3);
                }
                ++counter;
            }
        }
    }
}
