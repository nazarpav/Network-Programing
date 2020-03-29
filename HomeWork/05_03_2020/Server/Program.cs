using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Server
{
    public class Client
    {
        // Деталі файла
        [Serializable]
        public class FileDetails
        {
            public string FILETYPE = "";
            public long FILESIZE = 0;

            public override string ToString() => $"FileType {FILETYPE} size {FILESIZE}";
        }

        // Поля, які потрібні для UdpClient
        private static FileDetails fileInfo;
        private static int localPort = 1024;
        private static UdpClient client = new UdpClient(localPort);
        private static IPEndPoint RemoteIpEndPoint = null;

        // Файловий потік
        private static FileStream fs;
        private static byte[] receiveBytes = null;
        private static Random rnd = new Random();

        static void Main(string[] args)
        {
            while (true)
            {

                // Отримуємо інформацію про файл
                GetFileDetails();

                // Отримуємо файл
                ReceiveFile();
            }
            client.Close();
        }
        private static void GetFileDetails()
        {
            try
            {
                Console.WriteLine("-----------********* Waiting for file info **********----------");

                // Отримуємо інформацію про файл
                receiveBytes = client.Receive(ref RemoteIpEndPoint);
                Console.WriteLine("info has come!");

                XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDetails));
                using (MemoryStream stream1 = new MemoryStream())
                {
                    // Зчитуємо інформацію про файл
                    stream1.Write(receiveBytes, 0, receiveBytes.Length);
                    stream1.Position = 0;

                    // Десеріалізуємо об'єкт
                    fileInfo = (FileDetails)fileSerializer.Deserialize(stream1);
                    Console.WriteLine($"Recieved file. {fileInfo}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void ReceiveFile()
        {
            try
            {
                Console.WriteLine("---------********* Waition for a file *********---------");

                // Отримуємо файл
                receiveBytes = client.Receive(ref RemoteIpEndPoint);
                Console.WriteLine("I have a file. Saving...");

                // Створюємо тимчасовий файл з отриманим розширенням
                fs = new FileStream(@"file_from_server" + rnd.Next(int.MinValue, int.MaxValue) + fileInfo.FILETYPE, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(receiveBytes, 0, receiveBytes.Length);

                Console.WriteLine("File is saved.");

                Console.WriteLine("Opening...");
                // Відкриваємо файл
                Process.Start(fs.Name);
            }
            catch (Exception eR)
            {
                Console.WriteLine(eR.ToString());
            }
            finally
            {
                fs.Close();
                //client.Close();
                //Console.Read();
            }
        }
    }
}
