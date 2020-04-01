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
    public partial class Window1 : Window
    {
        SmtpServer server = null;
        public Window1(SmtpServer server, string destinationLogin=null)
        {
            InitializeComponent();
            this.server = server;
            if(destinationLogin!=null)
            {
                SendTo.Text = destinationLogin;
                SendTo.IsEnabled = false;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //send
            try
            {
                SmtpMail message = new SmtpMail("TryIt") // trial licence
                {
                    From = server.User,
                    To = SendTo.Text,
                    Subject = Subject.Text,
                    TextBody = Letter.Text,
                    Priority = MailPriority.High
                };
                foreach (var file in Files.Items)
                {
                    message.AddAttachment(file.ToString());
                }
                SmtpClient client = new SmtpClient();
                client.SendMail(server, message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                foreach (var file in dlg.FileNames)
                {
                    Files.Items.Add(file);
                }
            }
        }
    }
}
