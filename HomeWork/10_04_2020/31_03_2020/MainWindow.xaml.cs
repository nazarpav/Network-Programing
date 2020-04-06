using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<DGElement> dgrids;
        public MainWindow()
        {
            InitializeComponent();
            dgrids = new ObservableCollection<DGElement>();
            DG.ItemsSource = dgrids;
        }

        private void PathToSave_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathToSave.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs eventArgs)
        {
            var s = dgrids.Where(q => q.Uri == ((WebClient)sender).BaseAddress).FirstOrDefault();
            s.ProgBar = "%" + eventArgs.ProgressPercentage;
            Dispatcher.Invoke(() => DG.Items.Refresh());
			//з прогрес баром не працювало його не відображало
        }
        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Success");
        }
        private async void DownloadFileAsync()
        {
            try
            {
                WebClient client = new WebClient();

                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                client.BaseAddress = PathToLoad.Text;
                dgrids.Add(new DGElement(System.IO.Path.GetFileName(PathToLoad.Text), "%0", PathToLoad.Text));
                client.DownloadProgressChanged += DownloadProgressChanged;

                await client.DownloadFileTaskAsync(new Uri(PathToLoad.Text), PathToSave.Text + "\\" + System.IO.Path.GetFileName(PathToLoad.Text));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DownloadFileAsync();
        }
    }
    public class DGElement
    {
        public DGElement(string name, string progress, string uri)
        {
            Name = name;
            ProgBar = progress;
            Uri = uri;
        }
        public string Name { get; set; }
        public string ProgBar { get; set; }
        public string Uri { get; set; }
    }
}
