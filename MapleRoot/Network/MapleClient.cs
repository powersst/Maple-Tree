// Project: MapleRoot
// File: MapleClient.cs
// Updated By: Jared
// 

#region usings

using System;
using System.IO;
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
    public class MapleClient : MapleBase
    {
        public NetClient NetClient { get; private set; }
        public NetPeerStatistics Stats => NetClient.Statistics;
        public bool IsRunning => NetClient.Status == NetPeerStatus.Running;
        public UserData UserData { get; set; }
        
        public void Start(string serverIP)
        {
            NetClient.RegisterReceivedCallback(ReadMessage, new SynchronizationContext());
            NetClient.Connect(serverIP, Config.ServerPort);
        }

        public void Stop()
        {
            NetClient.Disconnect($"Bye! -{NetClient.UniqueIdentifier}");
        }

        public void UpdateUsername(string username)
        {
            if (UserData == null) return;
            UserData = new UserData
            {
                Username = username,
                Serial = UserData.Serial
            };
            Send(UserData, MessageType.ModUsername);
        }

        public NetSendResult Send(string message, MessageType m_type)
        {
            if (NetClient.ConnectionsCount <= 0) return new NetSendResult();

            var buf = Encoding.UTF8.GetBytes(message);
            var result = Send(NetClient, NetClient.ServerConnection, buf, m_type);
            return result;
        }

        public NetSendResult Send<T>(T data, MessageType m_type)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var result = Send(NetClient, NetClient.ServerConnection, ms.ToArray(), m_type);
            return result;
        }

        public NetSendResult SendWithProgress<T>(T data, MessageType m_type)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var result = Send(NetClient, NetClient.ServerConnection, ms.ToArray(), m_type);
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
                            var header = MessageHeader.Parse(inMsg);
                            OnMessageReceived.Invoke(m_client,
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
                client.Recycle(inMsg);
            }
        }
        
        public event EventHandler<OnMessageReceivedEventArgs> OnMessageReceived = (sender, args) => { };
        public event EventHandler OnConnected = (sender, args) => { };
        public event EventHandler OnDisconnect = (sender, args) => { };
    }
}