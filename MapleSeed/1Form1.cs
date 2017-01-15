// Project: MapleSeed
// File: 1Form1.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MapleRoot;
using MapleRoot.Enums;
using MapleRoot.Network;
using MapleRoot.Network.Events;
using MapleRoot.Network.Messages;
using MapleRoot.Structs;
using ProtoBuf;
using MaterialSkin.Controls;
using MaterialSkin;

#endregion

namespace MapleSeed
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Amber700, Primary.Amber900, Primary.Amber500, Accent.Amber100, TextShade.WHITE);
            var btnBackgroundColor = Color.FromArgb(255, 255, 160, 0);
            var btnForeColor = Color.White;
            playBtn.BackColor = btnBackgroundColor;
            shareBtn.BackColor = btnBackgroundColor;
            updateBtn.BackColor = btnBackgroundColor;
            myUploads.BackColor = btnBackgroundColor;
            playBtn.ForeColor = btnForeColor;
            shareBtn.ForeColor = btnForeColor;
            updateBtn.ForeColor = btnForeColor;
            myUploads.ForeColor = btnForeColor;
            Toolbelt.Form1 = this;
        }

        private static List<string> Library { get; set; }

        private static MapleClient Client { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            Database.Initialize();
            if (Client == null) {
                Client = MapleClient.Create();
                Client.OnMessageReceived += ClientOnOnMessageReceived;
                Client.OnConnected += ClientOnConnected;
                Client.NetClient.Start();
            }

            Text += $@" - Serial {Toolbelt.Settings.Serial}";
            MinimumSize = MaximumSize = Size;
            Text += Toolbelt.Version;

            Library = new List<string>(Directory.GetDirectories(Toolbelt.Settings.TitleDirectory));
            foreach (var item in Library) if (!string.IsNullOrEmpty(item)) ListBoxAddItem(new FileInfo(item).Name);

            status.Text = Settings.Instance.TitleDirectory;
            fullScreen.Checked = Settings.Instance.FullScreenMode;

            if (string.IsNullOrEmpty(Settings.Instance.Username))
                username.Text = Settings.Instance.Username = Toolkit.TempName();
            else
                username.Text = Settings.Instance.Username;

            Toolkit.GlobalTimer.Elapsed += GlobalTimer_Elapsed;
        }

        private void GlobalTimer_Elapsed(object sender, ElapsedEventArgs e) {}

        private static void ClientOnConnected(object sender, EventArgs e)
        {
            Client.UserData = new UserData
            {
                Username = Settings.Instance.Username,
                Serial = Settings.Instance.Serial
            };
            Client.Send(Client.UserData, MessageType.ModUsername);
            Toolbelt.SetStatus($"You are now connected to Hub[{Toolbelt.Settings.Hub}]", Color.DarkGreen);

            Task.Run(() => {
                while (Client.NetClient.ConnectionsCount > 0) {
                    Toolbelt.SetStatus(
                        $@"Total In {Client.Stats.ReceivedBytes} bytes, Total Out {Client.Stats.SentBytes} bytes",
                        Color.Gray);
                    Toolkit.Sleep(100);
                }
                Toolbelt.SetStatus("Disconnected from server!!");
            });
        }

        private void ClientOnOnMessageReceived(object sender, OnMessageReceivedEventArgs e)
        {
            var header = e.Header;

            switch (header.Type) {
                case MessageType.Userlist:
                    HandleUserList.Init(header.Data, userList);
                    break;
                case MessageType.ChatMessage:
                    var msg = Encoding.UTF8.GetString(header.Data);
                    Toolbelt.AppendLog(msg);
                    break;
                case MessageType.ModUsername:
                    UpdateUsername(Encoding.UTF8.GetString(e.Header.Data));
                    break;
                case MessageType.StorageUpload:
                    ConfirmStorageUpload(e.Header);
                    break;
                case MessageType.ShaderData:
                    HandleShaderData(e.Header);
                    break;
                case MessageType.ReceiveFile:
                    break;
                case MessageType.RequestDownload:
                    HandleRequestDownload(e.Header);
                    break;
                case MessageType.RequestSearch:
                    HandleRequestSearch(e.Header.Data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleRequestDownload(MessageHeader header)
        {
            try {
                using (var ms = new MemoryStream(header.Data)) {
                    var sd = Serializer.Deserialize<StorageData>(ms);

                    var saveTo = sd.Shader
                        ? Path.Combine(Toolbelt.Settings.CemuDirectory, "shaderCache", "transferable", sd.Name)
                        : Path.Combine(Toolbelt.Settings.CemuDirectory, "graphicPacks", sd.Name, "rules.txt");

                    File.WriteAllBytes(saveTo, sd.Data);
                    AppendLog($"[{sd.Name}] Download Complete.");
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message + "\n" + e.StackTrace);
            }
        }

        private void HandleRequestSearch(byte[] data)
        {
            List<StorageData> list;
            using (var ms = new MemoryStream(data)) {
                list = Serializer.Deserialize<List<StorageData>>(ms);
            }
            dataGrid1.Invoke(new Action(() => {
                dataGrid1.DataSource = list;
                dataGrid1.Columns.Remove("Data");
                dataGrid1.Columns["Serial"].HeaderText = @"Owner";
            }));
        }

        private void HandleShaderData(MessageHeader eHeader)
        {
            try {
                using (var ms = new MemoryStream(eHeader.Data)) {
                    var list = Serializer.Deserialize<List<StorageData>>(ms);
                    dataGrid1.Invoke(new Action(() => {
                        dataGrid1.DataSource = list;
                        dataGrid1.Columns.Remove("Data");
                        dataGrid1.Columns["Serial"].HeaderText = @"Owner";
                    }));
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Client.IsRunning) Client.Stop();
        }

        private void ListBoxAddItem(object obj)
        {
            if (titleList.InvokeRequired)
                titleList.BeginInvoke(new Action(() => { titleList.Items.Add(obj); }));
            else
                titleList.Items.Add(obj);
        }

        public void UpdateProgress(int percentage, long recvd, long toRecv)
        {
            if (Toolbelt.Form1 != null) {
                var pg = Toolbelt.Form1.progressBar;
                pg.Invoke(new Action(() => pg.Value = percentage));
            }

            var received = Toolbelt.SizeSuffix(recvd);
            var toReceive = Toolbelt.SizeSuffix(toRecv);
            SetStatus($"{percentage}% | {received} / {toReceive}");
        }

        public void AppendLog(string msg, Color color = default(Color))
        {
            msg += '\n';
            if (richTextBox1.InvokeRequired) {
                richTextBox1.BeginInvoke(new Action(() => {
                    richTextBox1.AppendText(msg);
                    richTextBox1.ScrollToCaret();
                }));
            }
            else {
                richTextBox1.AppendText(msg);
                richTextBox1.ScrollToCaret();
            }
        }

        public void SetStatus(string msg, Color color = default(Color))
        {
            msg += '\n';
            if (status.InvokeRequired) {
                status.BeginInvoke(new Action(() => {
                    status.Text = msg;
                    status.ForeColor = color;
                }));
            }
            else {
                status.Text = msg;
                status.ForeColor = color;
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            try {
                string fullPath = null, item = titleList.SelectedItem as string;

                if (item != null)
                    fullPath = Path.Combine(Toolbelt.Settings.TitleDirectory, item);

                if (Toolbelt.Database != null) {
                    var title = Database.Find(item);
                    Toolbelt.Database.UpdateGame(title.TitleID, fullPath);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }

            titleList.Enabled = false;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            var item = titleList.SelectedItem as string;
            if (string.IsNullOrEmpty(item)) return;
        }

        private void fullTitle_CheckedChanged(object sender, EventArgs e)
        {
            updateBtn.Text = fullTitle.Checked ? "Download" : "Update";
            if (fullTitle.Checked)
            {
                myUploads.Location = new Point(myUploads.Location.X + 24, myUploads.Location.Y);
            } else
            {
                myUploads.Location = new Point(myUploads.Location.X - 24, myUploads.Location.Y);
            }
        }

        private void fullScreen_CheckedChanged(object sender, EventArgs e)
        {
            Toolbelt.Settings.FullScreenMode = fullScreen.Checked;
        }
        
        private void username_TextChanged(object sender, EventArgs e)
        {
            Settings.Instance.Username = username.Text;
        }

        private void UpdateUsername(string name)
        {
            Settings.Instance.Username = name;
            username.Invoke(new Action(() => { username.Text = name; }));
        }

        private void shareBtn_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = @"Tansferable Data |*.bin;rules.txt",
                InitialDirectory = Toolbelt.Settings.CemuDirectory,
                Multiselect = true
            };
            var result = ofd.ShowDialog();
            if (!File.Exists(ofd.FileName))
                return;

            if (result != DialogResult.OK)
                return;

            var file = Path.GetFullPath(ofd.FileName);

            if (string.IsNullOrWhiteSpace(file))
                return;

            if (new FileInfo(file).Extension == ".txt") {
                var folder = Path.GetDirectoryName(file);
                folder = Path.GetFileName(folder);
                Storage.Upload(Client, file, folder, Toolbelt.Serial, false);
            }
            else {
                if (ofd.FileNames.Length > 0)
                    foreach (var ofdFile in ofd.FileNames)
                        Storage.Upload(Client, ofdFile, Path.GetFileName(ofdFile), Toolbelt.Settings.Serial, true);
                else Storage.Upload(Client, file, Path.GetFileName(file), Toolbelt.Settings.Serial, true);
            }
        }

        private void ConfirmStorageUpload(MessageHeader header)
        {
            using (var ms = new MemoryStream(header.Data)) {
                var sd = Serializer.Deserialize<StorageData>(ms);
                if (sd.Length <= 0) return;

                AppendLog(!sd.Shader
                    ? $"Graphic Pack, {sd.Name} has been uploaded!"
                    : $"Transferable Shader, {sd.Name} has been uploaded!");
            }
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            var title = titleList.SelectedItem as string;
            if (title == null) return;

            Toolbelt.LaunchCemu(title);
            var msg = $"[{Client.UserData.Username}] Has started playing {title}!";
            Client.Send(msg, MessageType.ChatMessage);
            AppendLog(msg);
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            Client.Send(Toolbelt.Serial, MessageType.ShaderData);
        }

        private void dataGrid1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGrid1.SelectedRows.Count <= 0) return;
            var row = (StorageData) dataGrid1.SelectedRows[0].DataBoundItem;
            AppendLog($"[{row.Name}] Requesting download from storage server.");
            Client.Send(row, MessageType.RequestDownload);
        }

        private void search_TextChanged(object sender, EventArgs e)
        {
            if (search.Text.Length > 5)
                Client.Send(search.Text, MessageType.RequestSearch);
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            if (Client.NetClient.ConnectionsCount <= 0) {
                Client.Start(Toolbelt.Settings.Hub);
                connectBtn.Text = @"Disconnect";
            }
            else {
                Client.Stop();
                connectBtn.Text = @"Connect";
            }
        }

        private void sendChat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(chatInput.Text)) return;
            if (Client.NetClient.ServerConnection == null) return;
            Client.Send($"[{username.Text}]: {chatInput.Text}", MessageType.ChatMessage);
            chatInput.Text = string.Empty;
        }
    }
}