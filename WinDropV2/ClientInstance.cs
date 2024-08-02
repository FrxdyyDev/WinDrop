using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WinDropV2
{
    public class ClientInstance
    {

        private TcpClient client;
        private IPAddress _address;
        private int port;

        private Boolean connected = false;

        private NetworkStream stream;
        
        public ClientInstance(IPAddress address)
        {
            new ClientInstance(address, 9988);
        }
        
        public ClientInstance(IPAddress address, int pPort)
        {
            Console.WriteLine("Created new Client-instance(" + address + ":" + pPort + ")");
            _address = address;
            port = pPort;
            
            client = new TcpClient();
            try
            {
                client.Connect(address, pPort);
                stream = client.GetStream();
                Utils.ClientInstances.Add(new IPEndPoint(address, pPort), this);
                connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public TcpClient Client
        {
            get => client;
            set => client = value;
        }

        public IPAddress Address
        {
            get => _address;
            set => _address = value;
        }

        public int Port
        {
            get => port;
            set => port = value;
        }

        public void requestSharing(List<FileInfo> files)
        {
            if (files != null && files.Count > 0)
            {
                string requestString = $"{Dns.GetHostName()}:";
                foreach (FileInfo fileInfo in files)
                {
                    requestString += ($"{fileInfo.Name}|{fileInfo.Length}:");
                }
                byte[] request = Encoding.Default.GetBytes(requestString);
                stream.Write(request, 0, request.Length);
                Console.WriteLine($"Client: {requestString}");
                stream.Flush();
            }
        }

        public void shareFiles()
        {
            if (Utils.FilesInCache != null && Utils.FilesInCache.Count > 0)
            {
                String data = Dns.GetHostName();
                foreach (FileInfo fileInfo in Utils.FilesInCache)
                {
                    data += "\n:NEXTFILE:" + fileInfo.Name + ":FILEDATA:";
                    data += getFileData(fileInfo);
                }

                byte[] dataByteArray = Encoding.Default.GetBytes(data);
                stream.Write(dataByteArray, 0, dataByteArray.Length);
            }
        }

        public void sendAcceptBytes(int pPort)
        {
            String acceptAnswer = "request accepted connect to:" + pPort;
            byte[] acceptAnswerBytes = Encoding.Default.GetBytes(acceptAnswer);
            stream.Write(acceptAnswerBytes, 0, acceptAnswerBytes.Length);
        }

        public String getFileData(FileInfo fileInfo)
        {
            return new StreamReader(fileInfo.FullName, Encoding.Default).ReadToEnd();
        }
        
        
        
    }
}