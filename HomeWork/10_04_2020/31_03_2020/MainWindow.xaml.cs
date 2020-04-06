using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _31_03_2020
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PathToSave_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathToSave.Text = folderBrowserDialog.SelectedPath;
            }
        }
        //private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    PRGB.Value = e.ProgressPercentage;
        //}

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Success");
        }
        private async void DownloadFileAsync()
        {
            WebClient client = new WebClient();

            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            ProgressBar progress = new ProgressBar() { Maximum=100,Width=100};
            client.DownloadProgressChanged += (s, args) => {Dispatcher.Invoke(()=> progress.Value = args.ProgressPercentage); };
            //LV.Items.Add(new Button() {Text="sdfs" });

            await client.DownloadFileTaskAsync(new Uri(PathToLoad.Text), PathToSave.Text + "\\" + System.IO.Path.GetFileName(PathToLoad.Text));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DownloadFileAsync();
        }
    }
}
