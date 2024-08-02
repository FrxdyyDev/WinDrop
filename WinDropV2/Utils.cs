using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WinDropV2
{
    public class Utils
    {
        private static Dictionary<IPEndPoint, ClientInstance> clientInstances = new Dictionary<IPEndPoint, ClientInstance>();
         
        private static Dictionary<String, int> hostNameBufferSizeRelatives = new Dictionary<string, int>();
        
        private static List<FileInfo> filesInCache = new List<FileInfo>();

        private static Dictionary<String, FileInfo> fileNameRelatedToFileInfo = new Dictionary<string, FileInfo>();

        public static Dictionary<string, int> HostNameBufferSizeRelatives
        {
            get => hostNameBufferSizeRelatives;
            set => hostNameBufferSizeRelatives = value;
        }

        public static List<FileInfo> FilesInCache
        {
            get => filesInCache;
            set => filesInCache = value;
        }

        public static Dictionary<string, FileInfo> FileNameRelatedToFileInfo
        {
            get => fileNameRelatedToFileInfo;
            set => fileNameRelatedToFileInfo = value;
        }

        public static Dictionary<IPEndPoint, ClientInstance> ClientInstances
        {
            get => clientInstances;
            set => clientInstances = value;
        }

        public static string GetLocalIpAddress()
        {
            string localIpAddress = "";
            
            string hostName = Dns.GetHostName();
            
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
            
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIpAddress = address.ToString();
                    break;
                }
            }

            return localIpAddress;
        }
        
        public static int findFreePort()
        {
            var portListener = new TcpListener(IPAddress.Any, 0);
            portListener.Start();
            int port = ((IPEndPoint)portListener.LocalEndpoint).Port;
            portListener.Stop();
            
            return port;
        } 
    }
}