// Project: MapleSeed
// File: 1Form1.cs
// Updated By: Jared
// 

#region usings

using MapleRoot;
using MapleRoot.Enums;
using MapleRoot.Network;
using MapleRoot.Network.Events;
using MapleRoot.Network.Messages;
using MaryJane;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lidgren.Network;
using MapleRoot.Structs;
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

        private static List<string> Library { get; set; }

        private static MapleClient Client { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            Client = MapleClient.Create();
            Client.OnMessageReceived += ClientOnOnMessageReceived;
            Client.Start();

            MinimumSize = MaximumSize = Size;
            Text += Toolbelt.Version;

            Database.Initialize();

            Library = new List<string>(Directory.GetDirectories(Toolbelt.Settings.TitleDirectory));
            foreach (var item in Library) {
                if (!string.IsNullOrEmpty(item)) {
                    ListBoxAddItem(new FileInfo(item).Name);
                }
            }

            status.Text = Settings.Instance.TitleDirectory;
            fullScreen.Checked = Settings.Instance.FullScreenMode;

            if (string.IsNullOrEmpty(Settings.Instance.Username))
                username.Text = Settings.Instance.Username = Toolkit.TempName();
            else
                username.Text = Settings.Instance.Username;

            Task.Run(() => {
                while (Client.IsRunning) {
                    SetStatus(
                        $@"Total In {Client.Stats.ReceivedBytes} bytes, Total Out {Client.Stats.SentBytes} bytes",
                        Color.Gray);
                    Toolkit.Sleep(250);
                }
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
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Client.IsRunning) Client.Stop();
        }
        
        private void ListBoxAddItem(object obj)
        {
            if (listBox1.InvokeRequired)
                listBox1.BeginInvoke(new Action(() => { listBox1.Items.Add(obj); }));
            else
                listBox1.Items.Add(obj);
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
            string fullPath = null, item = listBox1.SelectedItem as string;

            if (item != null)
                fullPath = Path.Combine(Toolbelt.Settings.TitleDirectory, item);

            if (Toolbelt.Database != null) {
                var title = Database.Find(item);
                Toolbelt.Database.UpdateGame(title.TitleID, fullPath);
            }

            listBox1.Enabled = false;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void fullTitle_CheckedChanged(object sender, EventArgs e)
        {
            updateBtn.Text = fullTitle.Checked ? "Download" : "Update";
        }

        private void fullScreen_CheckedChanged(object sender, EventArgs e)
        {
            Toolbelt.Settings.FullScreenMode = fullScreen.Checked;
        }
        
        private void chatInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Return && !string.IsNullOrEmpty(chatInput.Text))
                if (Client.NetClient.ServerConnection != null) {
                    Client.Send($"[{username.Text}]: {chatInput.Text}", MessageType.ChatMessage);
                    chatInput.Text = string.Empty;
                }
        }

        private void username_TextChanged(object sender, EventArgs e)
        {
            Settings.Instance.Username = username.Text;
            Client.SetUsername(username.Text);
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
                InitialDirectory = Toolbelt.Settings.CemuDirectory
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
                //Storage.Upload(Client, file, folder, false);
            }
            else {
                var filename = Path.GetFileName(file);
                Storage.Upload(Client, file, filename, true);
            }
        }

        private void ConfirmStorageUpload(MessageHeader header)
        {
            using (var ms = new MemoryStream(header.Data))
            {
                var sd = Serializer.Deserialize<StorageData>(ms);
                if (sd.Length <= 0) return;

                AppendLog(!sd.Shader
                    ? $"Graphic Pack, {sd.Name} has been uploaded!"
                    : $"Transferable Shader, {sd.Name} has been uploaded!");
            }
        }

        private void playBtn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                Toolbelt.LaunchCemu(listBox1.SelectedItem as string);
        }
    }
}