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
        public AnswerLetter(SmtpServer server,Letter letter)
        {
            InitializeComponent();
            this.letter = letter;
            this.server = server;
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
            Window1 w = new Window1(server,letter.letter.From.Address);
            w.Show();
        }
    }
}
