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
using System.Windows.Shapes;

namespace _01_04_2020
{
    public partial class AnswerLetter : Window
    {
        Letter letter = null;
        SmtpServer server = null;
        MailServer mailServer = null;
        public AnswerLetter(SmtpServer server,Letter letter,MailServer mailServer)
        {
            InitializeComponent();
            this.letter = letter;
            this.server = server;
            this.mailServer = mailServer;
            LoadLetter();
        }
        private void LoadLetter()
        {
            Letter.Text =
                "Date: " + letter.letter.SentDate
                + "\nSender: " + letter.letter.From
                + "\nSubject: " + letter.letter.Subject
                + "\nLetter: " + letter.letter.TextBody;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendLetter w = new SendLetter(server,letter.letter.From.Address);
            w.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MailClient client = new MailClient("TryIt");
            client.Connect(mailServer);
            client.Delete(client.GetMailInfos().Reverse().Take(60).Where(q=> client.GetMail(q).TextBody == letter.letter.TextBody).FirstOrDefault());
        }
    }
}
