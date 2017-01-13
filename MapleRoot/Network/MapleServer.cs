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

            Task.Run(() => {
                while (NetServer.Status == NetPeerStatus.Running) {
                    var names =
                    (from c in NetServer.Connections
                        select (UserData) c.Tag into mc select mc.Username).ToList();

                    if (names.Count > 0) {
                        var json = JsonConvert.SerializeObject(names);
                        var buf = Encoding.UTF8.GetBytes(json);

                        var msg = NetServer.CreateMessage();
                        msg.Write(buf.Length);
                        msg.Write((byte)MessageType.Userlist);
                        msg.Write(buf);

                        NetServer.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
                    }
                    
                    Toolkit.Sleep(1000);
                }
            });
        }

        private NetServer NetServer { get; }

        private static MapleServer Instance { get; set; }

        private static List<NetConnection> Connections { get; set; }

        private void m_OnMessageReceived(object sender, OnMessageReceivedEventArgs e)
        {
            var from = (NetConnection) sender;

            switch (e.Header.Type) {
                case MessageType.Userlist:
                    break;
                case MessageType.ChatMessage:
                    var str = Encoding.UTF8.GetString(e.Header.Data);
                    SendToAll(str, MessageType.ChatMessage);
                    break;
                case MessageType.ModUsername:
                    var name = Encoding.UTF8.GetString(e.Header.Data);
                    var mc = (UserData) from.Tag;
                    mc.Username = name;
                    from.Tag = mc;
                    break;
            }
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