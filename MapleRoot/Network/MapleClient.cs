// Project: MapleRoot
// File: MapleClient.cs
// Updated By: Jared
// 

#region usings

using System;
using System.IO;
using System.Text;
using System.Threading;
using Lidgren.Network;
using MapleRoot.Enums;
using MapleRoot.Network.Events;
using ProtoBuf;

#endregion

namespace MapleRoot.Network
{
    public class MapleClient
    {
        public NetClient NetClient { get; private set; }
        public NetPeerStatistics Stats => NetClient.Statistics;
        public bool IsRunning => NetClient.Status == NetPeerStatus.Running;

        public void Start()
        {
            var approval = NetClient.CreateMessage();
            approval.Write("secret");
            NetClient.RegisterReceivedCallback(ReadMessage, new SynchronizationContext());
            NetClient.Connect(Config.ServerIP, Config.ServerPort, approval);
        }

        public void Stop()
        {
            NetClient.Disconnect($"Bye! -{NetClient.UniqueIdentifier}");
            NetClient.Shutdown($"Bye! -{NetClient.UniqueIdentifier}");
        }

        public NetSendResult Send(string message, MessageType m_type)
        {
            var buf = Encoding.UTF8.GetBytes(message);
            var length = buf.Length;

            var msg = NetClient.CreateMessage();
            msg.Write(length);
            msg.Write((byte) m_type);
            msg.Write(message);

            var result = NetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            if (result == NetSendResult.Sent) Console.WriteLine("Message Sent!");
            return result;
        }

        public NetSendResult Send<T>(T data, MessageType m_type)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var len = (int) ms.Length;

            var msg = NetClient.CreateMessage();
            msg.Write(len);
            msg.Write((byte) m_type);
            msg.Write(ms.ToArray());

            var result = NetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            if (result == NetSendResult.Sent) Console.WriteLine("Message Sent!");
            return result;
        }

        public static MapleClient Create()
        {
            var client = new NetClient(new NetPeerConfiguration("MapleTree") {AutoFlushSendQueue = true});
            client.Tag = new MapleClient {NetClient = client};
            client.Start();

            return client.Tag as MapleClient;
        }

        private void ReadMessage(object state)
        {
            var client = state as NetClient;
            if (client == null) return;
            var m_client = client.Tag as MapleClient;

            NetIncomingMessage inMsg;
            while ((inMsg = client.ReadMessage()) != null) {
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
                                m_client?.OnConnected.Invoke(m_client, EventArgs.Empty);
                                break;

                            case NetConnectionStatus.Disconnected:
                                m_client?.OnDisconnect.Invoke(m_client, EventArgs.Empty);
                                break;
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        try {
                            var len = inMsg.ReadInt32();
                            var m_type = (MessageType)inMsg.ReadByte();
                            var buf = inMsg.ReadBytes(len);
                            OnMessageReceived.Invoke(m_client,
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
                client.Recycle(inMsg);
            }
        }
        
        public event EventHandler<OnMessageReceivedEventArgs> OnMessageReceived = (sender, args) => { };
        public event EventHandler OnConnected = (sender, args) => { };
        public event EventHandler OnDisconnect = (sender, args) => { };
    }
}