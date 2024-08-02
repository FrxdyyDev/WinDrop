using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace WinDropV2
{
    public partial class Form1 : Form
    {

        private ImageList fileCacheIconList;
        private ImageList localDeviceIconList;

        
        
        public Form1()
        {
            InitializeComponent();
            
            addFileCache();
            
            addLocalDeviceCache();
            
            addConnectionTextBoxes();
        }


        private void addConnectionTextBoxes()
        {
            nameTextBox.GotFocus += nameTextBox_Enter;
            nameTextBox.LostFocus += nameTextBox_Leave;

            ipAddressTextBox.GotFocus += addressTextBox_Enter;
            ipAddressTextBox.LostFocus += addressTextBox_Leave;
        }
        

        private void addLocalDeviceCache()
        {
            localDeviceIconList = new ImageList();
            localDeviceIconList.ImageSize = new Size(48, 48);
            localDeviceCache.LargeImageList = localDeviceIconList;
            localDeviceCache.MouseClick += new MouseEventHandler(localDeviceCache_StartDrop);
            
            Thread searchDevicesThread = new Thread(startScheduler);
            searchDevicesThread.Start();
        }

        

        private void localDeviceCache_StartDrop(object sender, MouseEventArgs e)
        {
            string selectedDeviceName = localDeviceCache.SelectedItems[0].Text;
            IPHostEntry hostEntry = Dns.GetHostEntry(selectedDeviceName);

            if (hostEntry.AddressList.Length > 0)
            {
                IPAddress ip = null;
                int i = 0;
                while (ip == null)
                {
                    if (hostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ip = hostEntry.AddressList[i];
                    }
                    i++;
                }
                ClientInstance clientInstance = new ClientInstance(ip, 9988);
                clientInstance.requestSharing(Utils.FilesInCache);
                Console.WriteLine($"{hostEntry.HostName}:{ip}");
            }
        }

        private void addFileCache()
        {
            fileCache.AllowDrop = true;
            fileCache.DragEnter += new DragEventHandler(fileCache_DragEnter);
            fileCache.DragDrop += new DragEventHandler(fileCache_DragDrop);
            fileCache.MouseClick += new MouseEventHandler(fileCache_RemoveItem);
            fileCache.MouseDown += new MouseEventHandler(fileCache_MouseClick);
            
            fileCacheIconList = new ImageList();
            fileCacheIconList.ImageSize = new Size(48, 48);
            fileCache.LargeImageList = fileCacheIconList;
        }

        private void fileCache_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        
        private void fileCache_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                addFileToFileCache(file);
            }
        }

        private void fileCache_RemoveItem(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                fileCache.Items[fileCache.SelectedIndices[0]].Remove();
                Utils.FilesInCache.Remove(Utils.FileNameRelatedToFileInfo[fileCache.SelectedItems.ToString()]);
            }
        }
        
        private void fileCache_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in fileDialog.FileNames)
                    {
                        addFileToFileCache(filePath);
                    }
                }
            } 
            
        }
        
        private void addFileToFileCache(string filePath)
        {
            fileCacheIconList.Images.Add(Icon.ExtractAssociatedIcon(filePath));
            fileCache.Items.Add(new ListViewItem(new FileInfo(filePath).Name, fileCacheIconList.Images.Count - 1));
            Utils.FilesInCache.Add(new FileInfo(filePath));
        }
        
        private void startScheduler()
        {
            new Timer(searchDevicesInLocalNetwork, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
        }

        private void searchDevicesInLocalNetwork(object state)
        {
            localDeviceCache.Items.Clear();
            string baseAddress = Utils.GetLocalIpAddress().Substring(0, 11);
            for (int i = 0; i < 254; i++)
            {
                string address = string.Concat(baseAddress, $".{i}");
                checkAddress(address);
            }
        }

        private async void checkAddress(string address)
        {
            Ping ping = new Ping();
            PingReply reply = await ping.SendPingAsync(address);
            if (reply.Status == IPStatus.Success)
            {
                using (TcpClient client = new TcpClient())
                {
                    try
                    {
                        client.Connect(address, 9988);

                        byte[] message = Encoding.Default.GetBytes("PortCheck");
                        client.GetStream().Write(message, 0, message.Length);
                        client.GetStream().Flush();
                        client.GetStream().Close();
                        
                        addDeviceToListView(address);
                        client.Close();
                    }
                    catch (Exception){}
                }
            }
        }

        private void addDeviceToListView(string address)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(address);
                localDeviceIconList.Images.Add(DrawIcon(hostEntry.HostName.Substring(0, 1).ToUpper()));
                localDeviceCache.Items.Add(new ListViewItem(hostEntry.HostName.Replace(".fritz.box", ""), localDeviceIconList.Images.Count - 1));
            }
            catch (Exception){}
        }
        
        private Image DrawIcon(string text)
        {
            Bitmap iconBitmap = new Bitmap(128, 128); 
            
            using (Graphics g = Graphics.FromImage(iconBitmap))
            {
                int circleDiameter = Math.Min(iconBitmap.Width, iconBitmap.Height) - 20;
                int circleX = (iconBitmap.Width - circleDiameter) / 2;
                int circleY = (iconBitmap.Height - circleDiameter) / 2;
                Rectangle circleRect = new Rectangle(circleX, circleY, circleDiameter, circleDiameter);
                
                using (SolidBrush brush = new SolidBrush(Color.Gray))
                {
                    g.FillEllipse(brush, circleRect);
                }
                
                using (Font font = new Font("Arial", 30)) 
                {
                    SizeF textSize = g.MeasureString(text, font);
                    PointF textLocation = new PointF(circleRect.Left + (circleRect.Width - textSize.Width) / 2,
                        circleRect.Top + (circleRect.Height - textSize.Height) / 2);

                    using (SolidBrush textBrush = new SolidBrush(Color.Black))
                    {
                        g.DrawString(text, font, textBrush, textLocation);
                    }
                }
            }

            return iconBitmap;
        }

        private void nameTextBox_Enter(object sender, EventArgs e)
        {
            if (nameTextBox.Font.Italic && nameTextBox.Text == "Connection name...")
            {
                nameTextBox.Text = "";
                nameTextBox.Font = new Font(nameTextBox.Font, FontStyle.Regular);
            }
        }
        
        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                nameTextBox.Text = "Connection name...";
                nameTextBox.Font = new Font(nameTextBox.Font, FontStyle.Italic);
            }
        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            if (ipAddressTextBox.Font.Italic && ipAddressTextBox.Text == "Address of system...")
            {
                ipAddressTextBox.Text = "";
                ipAddressTextBox.Font = new Font(nameTextBox.Font, FontStyle.Regular);
            }
        }
        
        private void addressTextBox_Leave (object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ipAddressTextBox.Text))
            {
                ipAddressTextBox.Text = "Address of system...";
                ipAddressTextBox.Font = new Font(nameTextBox.Font, FontStyle.Italic);
            }
        }

        private void addConnectionButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}