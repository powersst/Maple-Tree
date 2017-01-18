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
using System.Timers;
using System.Windows.Forms;
using Lidgren.Network;
using MapleLib;
using MapleLib.Common;
using MapleLib.Enums;
using MapleLib.Network;
using MapleLib.Network.Events;
using MapleLib.Properties;
using MapleLib.Structs;
using ProtoBuf;

#endregion

namespace MapleSeed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Toolbelt.Form1 = this;
        }

        private static bool IsLive { get; set; } = true;

        private static List<string> Library { get; set; }

        private static MapleClient Client { get; set; }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Database.Initialize();

            if (Client == null) {
                Client = MapleClient.Create();
                Client.OnMessageReceived += ClientOnMessageReceived;
                Client.OnConnected += ClientOnConnected;
                Client.NetClient.Start();
            }

            MinimumSize = MaximumSize = Size;
            Text += Toolbelt.Version;
            Text += $@" - Serial {Toolbelt.Settings.Serial}";

            ReadLibrary();

            fullScreen.Checked = Settings.Instance.FullScreenMode;

            username.Text = Settings.Instance.Username;
            if (Settings.Instance.Username.IsNullOrEmpty())
                username.Text = Settings.Instance.Username = Toolkit.TempName();

            Toolkit.GlobalTimer.Elapsed += GlobalTimer_Elapsed;
            GlobalTimer_Elapsed(null, null);


            AppendLog($"Game Directory [{Toolbelt.Settings.TitleDirectory}]");
        }

        private void ReadLibrary()
        {
            var dir = Toolbelt.Settings.TitleDirectory;
            if (dir.IsNullOrEmpty()) return;

            Library = new List<string>(Directory.GetDirectories(dir));
            foreach (var item in Library) {
                var name = new FileInfo(item).Name;
                if (!titleList.Items.Contains(name)) ListBoxAddItem(name);
            }

            var cache = new object[titleList.Items.Count];
            titleList.Items.CopyTo(cache, 0);

            foreach (var item in cache) {
                var path = Path.Combine(dir, item.ToString());
                if (!Directory.Exists(path))
                    titleList.Invoke(new Action(() => titleList.Items.Remove(item)));
            }
        }

        private void UpdateUIModes()
        {
            if (Client.NetClient.ConnectionsCount <= 0 && IsLive) {
                Client.Start(Toolbelt.Settings.Hub);
                shareBtn.Enabled = false;
                myUploads.Enabled = false;
                sendChat.Enabled = false;
                username.Enabled = false;
            }
            else {
                shareBtn.Enabled = true;
                myUploads.Enabled = true;
                sendChat.Enabled = true;
                username.Enabled = true;
            }

            connectBtn.BackgroundImage = Client.NetClient.ConnectionStatus == NetConnectionStatus.Connected
                ? Resources.Green_Light.ToBitmap()
                : Resources.Red_Light.ToBitmap();
        }

        private void GlobalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try {
                if (InvokeRequired)
                    Invoke(new Action(UpdateUIModes));
                else UpdateUIModes();

                ReadLibrary();
                Client.Send("", MessageType.Userlist);
            }
            catch (Exception ex) {
                AppendLog(ex.StackTrace);
            }
        }

        private void ClientOnConnected(object sender, EventArgs e)
        {
            Client.UserData = new UserData
            {
                Username = Settings.Instance.Username,
                Serial = Settings.Instance.Serial
            };
            Client.Send(Client.UserData, MessageType.ModUserData);

            GlobalTimer_Elapsed(null, null);

            Toolbelt.AppendLog($"Connected to Hub [{Toolbelt.Settings.Hub}]", Color.DarkGreen);
        }

        private void ClientOnMessageReceived(object sender, OnMessageReceivedEventArgs e)
        {
            var header = e.Header;

            switch (header.Type) {
                case MessageType.Userlist:
                    HandleUserList(header.Data);
                    break;
                case MessageType.ChatMessage:
                    HandleChatMessage(header.Data);
                    break;
                case MessageType.ModUserData:
                    UpdateUsername(e.Header.Data);
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

        private void HandleChatMessage(byte[] data)
        {
            var msg = Encoding.UTF8.GetString(data);
            AppendChat(msg);
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
                var dataGridViewColumn = dataGrid1.Columns["Serial"];
                if (dataGridViewColumn != null) dataGridViewColumn.HeaderText = @"Owner";
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
                        var dataGridViewColumn = dataGrid1.Columns["Serial"];
                        if (dataGridViewColumn != null) dataGridViewColumn.HeaderText = @"Owner";
                    }));
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        private void HandleUserList(byte[] data)
        {
            List<string> userlist;
            using (var ms = new MemoryStream(data)) {
                userlist = Serializer.Deserialize<List<string>>(ms);
            }

            userList.Invoke(new Action(() => {
                userList.Items.Clear();

                foreach (var name in userlist)
                    if (!string.IsNullOrEmpty(name))
                        userList.Items.Add(name);
            }));
        }

        private void ListBoxAddItem(object obj)
        {
            if (titleList.InvokeRequired)
                titleList.BeginInvoke(new Action(() => { titleList.Items.Add(obj); }));
            else
                titleList.Items.Add(obj);
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            if (!IsLive) Client.Start(Toolbelt.Settings.Hub);
            else Client.Stop();
            IsLive = !IsLive;
        }

        public void UpdateProgress(int percentage, long recvd, long toRecv)
        {
            Invoke(new Action(() => { progressBar.Value = percentage; }));

            var received = Toolbelt.SizeSuffix(recvd);
            var toReceive = Toolbelt.SizeSuffix(toRecv);

            progressOverlay.Invoke(new Action(() => { progressOverlay.Text = $@"{received} / {toReceive}"; }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Client.IsRunning) Client.Stop();
        }

        private void AppendChat(string msg, Color color = default(Color))
        {
            msg += '\n';
            if (chatbox.InvokeRequired) {
                chatbox.BeginInvoke(new Action(() => {
                    chatbox.AppendText(msg, color);
                    chatbox.ScrollToCaret();
                }));
            }
            else {
                chatbox.AppendText(msg, color);
                chatbox.ScrollToCaret();
            }
        }

        public void AppendLog(string msg, Color color = default(Color))
        {
            msg += '\n';
            if (richTextBox1.InvokeRequired) {
                richTextBox1.BeginInvoke(new Action(() => {
                    richTextBox1.AppendText(msg, color);
                    richTextBox1.ScrollToCaret();
                }));
            }
            else {
                richTextBox1.AppendText(msg, color);
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

        private async void updateBtn_Click(object sender, EventArgs e)
        {
            try {
                if (
                    MessageBox.Show(@"This action will overwrite pre-existing files!", @"Confirm Update",
                        MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    updateBtn.Enabled = false;
                    foreach (var item in titleList.SelectedItems) {
                        var fullPath = item as string;
                        if (fullPath.IsNullOrEmpty()) continue;

                        // ReSharper disable once AssignNullToNotNullAttribute
                        fullPath = Path.Combine(Toolbelt.Settings.TitleDirectory, fullPath);

                        if (Toolbelt.Database == null) continue;
                        var title = Database.Find(new FileInfo(fullPath).Name);
                        await Toolbelt.Database.UpdateGame(title.TitleID, fullPath);
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }

            updateBtn.Enabled = true;
            titleList.Enabled = true;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            updateBtn_Click(null, null);
        }

        private void fullTitle_CheckedChanged(object sender, EventArgs e)
        {
            updateBtn.Text = fullTitle.Checked ? "Download" : "Update";
        }

        private void fullScreen_CheckedChanged(object sender, EventArgs e)
        {
            Toolbelt.Settings.FullScreenMode = fullScreen.Checked;
        }

        private void username_TextChanged(object sender, EventArgs e)
        {
            Settings.Instance.Username = username.Text;

            if (Client.UserData == null) return;
            Client.UserData.Username = username.Text;
            Client.Send(Client.UserData, MessageType.ModUserData);
            Client.Send("", MessageType.Userlist);
        }

        private void UpdateUsername(byte[] data)
        {
            UserData ud;
            using (var ms = new MemoryStream(data)) {
                ud = Serializer.Deserialize<UserData>(ms);
            }
            Client.UserData.Username = ud.Username;
            Settings.Instance.Username = ud.Username;
            username.Invoke(new Action(() => { username.Text = ud.Username; }));
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
            if (search.Text.Length > 2)
                Client.Send(search.Text, MessageType.RequestSearch);
        }

        private void sendChat_Click(object sender, EventArgs e)
        {
            if (chatInput.Text.IsNullOrEmpty()) return;
            CheckForCommandInput(chatInput.Text);
            if (Client.NetClient.ServerConnection == null) return;
            Client.Send($"[{username.Text}]: {chatInput.Text}", MessageType.ChatMessage);
            chatInput.Text = string.Empty;
        }

        private async void CheckForCommandInput(string s)
        {
            if (s.StartsWith("/dl")) {
                var titleId = s.Substring(3).Trim();
                var title = Database.FindByTitleId(titleId);
                var fullPath = Path.Combine(Settings.Instance.TitleDirectory, title.ToString());
                if (title.TitleID.IsNullOrEmpty()) return;
                await Toolbelt.Database.UpdateGame(titleId, fullPath, false);
            }
            else if (s.StartsWith("/help")) {
                AppendChat("This function is still a work in progress.");
            }
        }
    }
}