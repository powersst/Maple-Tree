// Project: MapleRoot
// File: MapleServer.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lidgren.Network;
using MapleRoot.Common;
using MapleRoot.Enums;
using MapleRoot.Network.Events;
using MapleRoot.Structs;
using Newtonsoft.Json;
using ProtoBuf;

#endregion

namespace MapleRoot.Network
{
    public class MapleServer : MapleBase
    {
        private MapleServer()
        {
            Connections = new List<NetConnection>();
            var config = new NetPeerConfiguration("MapleTree")
            {
                Port = Config.ServerPort,
                MaximumConnections = 100
            };
            NetServer = new NetServer(config);
            NetServer.RegisterReceivedCallback(ReadMessage, new SynchronizationContext());
            OnMessageReceived += m_OnMessageReceived;

            NetServer.Start();
        }

        private NetServer NetServer { get; }

        private static MapleServer Instance { get; set; }

        private static List<NetConnection> Connections { get; set; }

        private void m_OnMessageReceived(object sender, OnMessageReceivedEventArgs e)
        {
            var from = (NetConnection) sender;

            switch (e.Header.Type) {
                case MessageType.Userlist:
                    HandleUserList(from);
                    break;
                case MessageType.ChatMessage:
                    string msg = Encoding.UTF8.GetString(e.Header.Data);
                    SendToAll(msg, MessageType.ChatMessage);
                    Console.WriteLine(msg);
                    break;
                case MessageType.ModUserData:
                    using (var ms = new MemoryStream(e.Header.Data)) {
                        from.Tag = Serializer.Deserialize<UserData>(ms);
                        HandleUserList(from);
                        Console.WriteLine($"[{((UserData)from.Tag).Username}] User data updated.");
                    }
                    break;
                case MessageType.StorageUpload:
                    HandleStorageUpload(e.Header.Data, from);
                    break;
                case MessageType.ShaderData:
                    HandleShaderData(e.Header, from);
                    break;
                case MessageType.ReceiveFile:
                    break;
                case MessageType.RequestDownload:
                    HandleRequestDownload(e.Header.Data, from);
                        break;
                case MessageType.RequestSearch:
                    HandleRequestSearch(e.Header.Data, from);
                    break;
            }
        }

        private void HandleUserList(NetConnection from)
        {
            var names = (from c in NetServer.Connections select (UserData)c.Tag into mc select mc.Username).ToList();
            names.RemoveAll(string.IsNullOrEmpty);

            if (names.Count <= 0) return;

            Send(names, from, MessageType.Userlist);
        }

        private void HandleRequestDownload(byte[] data, NetConnection @from)
        {
            using (var ms = new MemoryStream(data))
            {
                var rsd = Serializer.Deserialize<StorageData>(ms);
                if (string.IsNullOrEmpty(rsd.Name)) return;
                var path = Path.Combine(Storage.Dir, rsd.Serial, rsd.Name);
                if (!File.Exists(path)) return;
                var sd = Toolkit.FromBson<StorageData>(path);
                Send(sd, @from, MessageType.RequestDownload);
            }
        }

        private void HandleRequestSearch(byte[] data, NetConnection @from)
        {
            var str = Encoding.UTF8.GetString(data);
            var dCache = new List<StorageData>();
            var dir = Path.GetFullPath(Storage.Dir);
            var files = Directory.GetFiles(dir, $"*{str}*", SearchOption.AllDirectories);
            foreach (var file in files)
                try {
                    var sd = Toolkit.FromBson<StorageData>(file);
                    sd.Data = null; //dont send data
                    dCache.Add(sd);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                }

            Send(dCache, @from, MessageType.RequestSearch);
        }

        private void HandleStorageUpload(byte[] data, NetConnection @from)
        {
            using (var ms = new MemoryStream(data)) {
                var sd = Serializer.Deserialize<StorageData>(ms);
                Console.WriteLine($"[{((UserData)from.Tag).Username}] Uploading {sd.Name}");
                if (!Storage.AddToStorage(sd)) return;
                sd.Data = null;
                Send(sd, @from, MessageType.StorageUpload);
                SendToAll($"[+New] {sd.Name} has been uploaded by {((UserData)@from.Tag).Username}", MessageType.ChatMessage);
            }
        }

        private void HandleShaderData(MessageHeader header, NetConnection from)
        {
            var ud = (UserData) from.Tag;
            if (ud == null) return;

            var dCache = new List<StorageData>();
            var dir = Path.GetFullPath(Path.Combine(Storage.Dir, ud.Serial));

            var files = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
                try {
                    var sd = Toolkit.FromBson<StorageData>(file);
                    sd.Data = null; //dont send byte array
                    dCache.Add(sd);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }

            Console.WriteLine($"[{((UserData)from.Tag).Username}] Requested ShaderData");
            Send(dCache, from, MessageType.ShaderData);
        }

        private void SendToAll(string message, MessageType m_type)
        {
            var buf = Encoding.UTF8.GetBytes(message);
            var msg = NetServer.CreateMessage();
            msg.Write(buf.Length);
            msg.Write((byte) m_type);
            msg.Write(buf);
            NetServer.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        private NetSendResult Send<T>(T data, NetConnection recipient, MessageType m_type)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            return Send(NetServer, recipient, ms.ToArray(), m_type);
        }

        private void ReadMessage(object state)
        {
            var server = state as NetServer;
            if (server == null) return;

            NetIncomingMessage inMsg;
            while ((inMsg = server.ReadMessage()) != null) {
                switch (inMsg.MessageType) {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(inMsg.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus) inMsg.ReadByte();
                        switch (status) {
                            case NetConnectionStatus.Connected:
                                inMsg.SenderConnection.Tag = new UserData();
                                Connections.Add(inMsg.SenderConnection);
                                Console.WriteLine($"{inMsg.SenderConnection} has connected!");
                                break;
                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine($"{inMsg.SenderConnection} has disconnected!");
                                break;
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        try {
                            var header = MessageHeader.Parse(inMsg);
                            OnMessageReceived.Invoke(inMsg.SenderConnection,
                                new OnMessageReceivedEventArgs {Header = header});
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + inMsg.MessageType);
                        break;
                }
                server.Recycle(inMsg);
            }
        }

        public static MapleServer Init()
        {
            return Instance ?? (Instance = new MapleServer());
        }

        public event EventHandler<OnMessageReceivedEventArgs> OnMessageReceived = (sender, args) => { };
    }
}