using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;
using System.Diagnostics;

namespace _26_03_2020
{
    public partial class MainWindow : Window
    {
        string[] files;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Path_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                files = dlg.FileNames;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var file in files)
            {
                ServerApp SA = new ServerApp();
                SA.Start(IPAddress.Parse(IP_.Text), int.Parse(PORT_.Text), Path_.Text);
            }
        }

        // Інформація про файл (потрібно для клієнта)
        [Serializable]
        public class FileDetails
        {
            public string FILETYPE = "";
            public long FILESIZE = 0;
        }
        public class ServerApp
        {
            // Поля, які відносяться до UdpClient
            private int remotePort;
            private FileDetails fileInfo = new FileDetails();
            private IPAddress remoteIPAddress = null;
            private UdpClient server = new UdpClient();
            private IPEndPoint endPoint = null;

            // Об'єкт файлового потоку
            private FileStream fs;

            public void Start(IPAddress remoteIPAddress, int remotePort, string Path_)
            {
                try
                {
                    this.remoteIPAddress = remoteIPAddress;
                    this.remotePort = remotePort;
                    endPoint = new IPEndPoint(remoteIPAddress, remotePort);
                    // Отримуємо шлях файлу і його розмір (повинен бути менше 8kb)
                    fs = new FileStream(Path_, FileMode.Open, FileAccess.Read);

                    if (fs.Length > 8192)
                    {
                        server.Close();
                        fs.Close();
                        return;
                    }

                    // Відправляємо інформацію про фото
                    SendFileInfo();
                    // Чекаємо 2 секунди
                    Thread.Sleep(2000);
                    // Відправляємо сам файл
                    SendFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            public void SendFileInfo()
            {
                // Отримуємо тип і розширення файлу
                fileInfo.FILETYPE = System.IO.Path.GetExtension(fs.Name); // fs.Name.Substring((int)fs.Name.Length - 3, 3);

                // Отримуємо довжину файла
                fileInfo.FILESIZE = fs.Length;

                XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));
                using (MemoryStream stream = new MemoryStream())
                {
                    // Серіалізуємо об'єкт
                    fileSerializer.Serialize(stream, fileInfo);

                    // Зчитуємо потік в масив байтів
                    stream.Position = 0;
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
                    // Відправляємо інформацію про файл
                    server.Send(bytes, bytes.Length, endPoint);
                }

            }
            private void SendFile()
            {
                // Створюємо файловий потік і перетворюємо його в байти
                Byte[] bytes = new Byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                try
                {
                    // Відправляємо файл
                    server.Send(bytes, bytes.Length, endPoint);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                }
                finally
                {
                    // Закриваємо з'єднання та очищаємо потік
                    fs.Close();
                    server.Close();
                }
                MessageBox.Show("Success");
                Console.Read();
            }
        }
    }
}
