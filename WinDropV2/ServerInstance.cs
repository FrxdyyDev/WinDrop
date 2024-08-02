using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Toolkit.Uwp.Notifications;

namespace WinDropV2
{
    public class ServerInstance
    {
        
        public ServerInstance(int port)
        {
            TcpListener listener = new TcpListener( IPAddress.Parse(Utils.GetLocalIpAddress()), port); 
            Thread acceptIncomingClientThread = new Thread(() => acceptIncomingClient(listener));
            acceptIncomingClientThread.Start();
        }

        
        private void acceptIncomingClient(TcpListener listener)
        {
            listener.Start();
            Console.WriteLine("Server ready for connection! -> " + listener.LocalEndpoint.ToString());
            TcpClient incomingClient = null;
            while (incomingClient == null)
            {
                incomingClient = listener.AcceptTcpClient();
                if (incomingClient != null)
                {
                    TcpClient client = incomingClient;
                    Thread handleConnectionThread = new Thread(() => handleConnection(client));
                    handleConnectionThread.Start();
                    incomingClient = null;
                }
            }
        }

        private void handleConnection(TcpClient client)
        {
                NetworkStream stream = client.GetStream();
                while (client.Connected && (stream.CanRead && stream.CanWrite))
                {
                    if (stream.DataAvailable)
                    {
                        byte[] buffer = new byte[client.ReceiveBufferSize];
                        int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                        string data = Encoding.Default.GetString(buffer, 0, bytesRead);
                        if (data.Contains("request accepted connect to:"))
                        {
                            Console.WriteLine("Client: " + data);
                            ClientInstance clientInstance = new ClientInstance(((IPEndPoint) client.Client.RemoteEndPoint).Address, int.Parse(data.Split(':')[1]));
                            clientInstance.shareFiles();
                        }
                        else if (data.Contains(":NEXTFILE:"))
                        {
                            Console.WriteLine("Client is sending Files");
                            string[] fileDataOnEachFile = data.Split(new string[] {"\n:NEXTFILE:"}, StringSplitOptions.RemoveEmptyEntries);
                            fileDataOnEachFile[0] = "";
                            foreach (String fileInfoString in fileDataOnEachFile)
                            {
                                if (fileInfoString != "")
                                {
                                    string fileName = fileInfoString.Split(new string[] {":FILEDATA:"}, StringSplitOptions.RemoveEmptyEntries)[0];
                                    Console.WriteLine(fileName);
                                    string fileData = fileInfoString.Split(new string[] {":FILEDATA:"}, StringSplitOptions.RemoveEmptyEntries)[1];
                                    Directory.CreateDirectory(
                                        (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Drops/"));
                                    File.WriteAllBytes(
                                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Drops/" +
                                        fileName, Encoding.Default.GetBytes(fileData));
                                }
                            }

                            Utils.ClientInstances.Remove((IPEndPoint) client.Client.RemoteEndPoint);
                        }else if (!data.Equals("PortCheck"))
                        {
                            if (((IPEndPoint)client.Client.LocalEndPoint).Port.Equals(9988))
                            {
                                Console.WriteLine($"Server: {Encoding.Default.GetString(buffer, 0, bytesRead)}");
                                extractSharingDetails(data, client);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Server: {Encoding.Default.GetString(buffer, 0, bytesRead)}");
                        }
                    }
                }
        }

        private void extractSharingDetails(String data, TcpClient client)
        {
            String hostName = data.Split(':')[0];
            String[] fileDetails = new string[data.Split(':').Length - 1];
            for (int i = 1; i < data.Split(':').Length - 1; i++)
            {
                fileDetails[i] = data.Split(':')[i];
            }

            String[] fileNames = new string[fileDetails.Length];
            int i2 = 0;
            int bufferSize = 0;
            foreach (string info in fileDetails)
            {
                if (info != null)
                {
                    if (!info.Equals(""))
                    {
                        fileNames[i2] = info.Split('|')[0];
                        try
                        {
                            bufferSize += int.Parse(info.Split('|')[1]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        Console.WriteLine(info);
                        i2++;
                    }
                }
            }

            if (Utils.HostNameBufferSizeRelatives.ContainsKey(hostName))
            {
                Utils.HostNameBufferSizeRelatives.Remove(hostName);
            }
            Utils.HostNameBufferSizeRelatives.Add(hostName, bufferSize);
            askForSharingPermission(hostName, fileNames, client);
        }
        

        private void askForSharingPermission(String hostName, String[] fileNames, TcpClient client)
        {
            try
            {
                string fileNameString = "";
                for (int i = 0; i < fileNames.Length - 1; i++)
                {
                    Console.WriteLine(fileNames[i]);
                    fileNameString += fileNames[i] + ", ";
                }

                fileNameString = fileNameString.Remove(fileNameString.Length - 2);
                Console.WriteLine(fileNameString);
            
                ToastContentBuilder toastContentBuilder = new ToastContentBuilder();
                toastContentBuilder.AddText($"Share request from {hostName}")
                    .AddText($"{hostName} wants to share {fileNameString}")
                    .AddButton(new ToastButton()
                        .SetContent("Accept").AddArgument("action", "accept")
                        .SetBackgroundActivation())
                    .AddButton(new ToastButton()
                        .SetContent("Reject").AddArgument("action", "reject")
                        .SetBackgroundActivation());
                ToastNotificationManagerCompat.OnActivated += toastArgs =>
                {
                    if (toastArgs.Argument.Contains("action=accept"))
                    {
                        establishSharingConnection(client);
                    }
                    else
                    {
                        client.GetStream().Close();
                        client.Close();
                    }
                };
                toastContentBuilder.Show();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        private void establishSharingConnection(TcpClient client)
        {
            int port = Utils.findFreePort();
            if (port != 0)
            {
                new ServerInstance(port);
                Utils.ClientInstances[(IPEndPoint) client.Client.LocalEndPoint].sendAcceptBytes(port);
            }
            else
            {
                establishSharingConnection(client);
            }
        }

        
        
    }
}