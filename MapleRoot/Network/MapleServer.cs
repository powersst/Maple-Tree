// Project: MapleRoot
// File: MapleServer.cs
// Updated By: Jared
// 

#region usings

using Lidgren.Network;
using MapleRoot.Network.Events;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MapleRoot.Enums;
using Newtonsoft.Json;

#endregion

namespace MapleRoot.Network
{
    public class MapleServer
    {
        private MapleServer()
        {
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
                    var names = NetServer.Connections.Select(c => c.RemoteUniqueIdentifier.ToString()).ToList();
                    var json = JsonConvert.SerializeObject(names);
                    var buf = Encoding.UTF8.GetBytes(json);

                    var msg = NetServer.CreateMessage();
                    msg.Write(buf.Length);
                    msg.Write((byte) MessageType.Userlist);
                    msg.Write(buf);

                    NetServer.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
                    Toolkit.Sleep(3000);
                }
            });
        }

        private void m_OnMessageReceived(object sender, OnMessageReceivedEventArgs e)
        {
            var from = (NetConnection) sender;

            switch (e.messageType) {
                case MessageType.Userlist:
                    break;
                case MessageType.ChatMessage:
                    var str = Encoding.UTF8.GetString(e.buffer);
                    SendToAll(str, MessageType.ChatMessage);
                    break;
            }
        }

        private NetServer NetServer { get; }

        private static MapleServer Instance { get; set; }

        private void SendToAll(string message, MessageType m_type)
        {
            var buf = Encoding.UTF8.GetBytes(message);
            var length = buf.Length;

            var msg = NetServer.CreateMessage();
            msg.Write(length);
            msg.Write((byte)m_type);
            msg.Write(message);

            NetServer.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        private NetSendResult Send(string message, NetConnection recipient, MessageType m_type)
        {
            var buf = Encoding.UTF8.GetBytes(message);
            var length = buf.Length;

            var msg = NetServer.CreateMessage();
            msg.Write(length);
            msg.Write((byte)m_type);
            msg.Write(message);

            var result = NetServer.SendMessage(msg, recipient, NetDeliveryMethod.ReliableOrdered);
            if (result == NetSendResult.Sent) Console.WriteLine("Message Sent!");
            return result;
        }

        private NetSendResult Send<T>(T data, NetConnection recipient, MessageType m_type)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var len = (int)ms.Length;

            var msg = NetServer.CreateMessage();
            msg.Write(len);
            msg.Write((byte) m_type);
            msg.Write(ms.ToArray());

            return NetServer.SendMessage(msg, recipient, NetDeliveryMethod.ReliableOrdered);
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
                                Console.WriteLine($"{inMsg.SenderConnection} has connected!");
                                break;
                        }
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        var uid = inMsg.SenderConnection.RemoteUniqueIdentifier;
                        var s = inMsg.ReadString();
                        if (s == "secret") {
                            inMsg.SenderConnection.Approve();
                            Console.WriteLine($"Connection Approved! {uid}");
                        }
                        else {
                            inMsg.SenderConnection.Deny();
                            Console.WriteLine($"Connection Denied! -{uid}");
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        try {
                            var len = inMsg.ReadInt32();
                            var m_type = (MessageType) inMsg.ReadByte();
                            var buf = inMsg.ReadBytes(len);
                            OnMessageReceived.Invoke(inMsg.SenderConnection,
                                new OnMessageReceivedEventArgs {lenth = len, buffer = buf, messageType = m_type});
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