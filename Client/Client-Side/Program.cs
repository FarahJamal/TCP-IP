using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Client_Socket
{
    //FILE TRANSFER USING C#.NET SOCKET PROGRAMMING - CLIENT
    class Program
    {
        public static string StringA { get; set; }

         static void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {

            StringA = e.Name;
            Console.WriteLine("new file added " + StringA);

            IPAddress[] ipAddress = Dns.GetHostAddresses("192.168.1.6");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress[0], 4444);
            Socket clientSock = new Socket(ipEnd.AddressFamily, SocketType.Stream, ProtocolType.IP);

            Console.WriteLine(ipEnd);
            string fileName = e.Name;
            string filePath = "C://Users//DeLL//Downloads//";
            byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);

            byte[] fileData = File.ReadAllBytes(filePath + fileName);
            byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);

            clientSock.Connect(ipEnd);
            clientSock.Send(clientData);
            Console.WriteLine("File:{0} has been sent.", fileName);
            //Console.WriteLine(StringA + " this is just the begining");

           // clientSock.Close();


        }


        static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("That program can transfer small file. I've test up to 850kb file");
                //Create a new FileSystemWatcher.
                FileSystemWatcher watcher = new FileSystemWatcher();

                //Set the filter to all files.
                watcher.Filter = "*.*";

                //Subscribe to the Created event.
                watcher.Created += new FileSystemEventHandler(watcher_FileCreated);

                //Set the path 
                watcher.Path = "C://Users//DeLL//Downloads//";

                //Enable the FileSystemWatcher events.
                watcher.EnableRaisingEvents = true;

                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("File Sending fail." + ex.Message);
            }

        }
    }
}