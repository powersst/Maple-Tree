// Project: MapleRoot
// File: Server.cs
// Updated By: Jared
// 

#region usings

using System;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Windows.Forms;
using Lidgren.Network;
using MapleRoot.Network.Messages;
using ProtoBuf;

#endregion

namespace MapleRoot.Network
{
    public class Server
    {
        private Server()
        {
            var config = new NetPeerConfiguration("MapleTree")
            {
                Port = Config.ServerPort,
                MaximumConnections = 100
            };
            config.SetMessageTypeEnabled(NetIncomingMessageType.UnconnectedData, true);

            NetServer = new NetServer(config);
            NetServer.RegisterReceivedCallback(Incoming.HandleServerMessage, new SynchronizationContext());
            
            NetServer.Start();
        }

        private NetServer NetServer { get; }

        private static Server Instance { get; set; }

        private NetSendResult Send<T>(T data, NetConnection recipient)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var msg = NetServer.CreateMessage(ms.GetBuffer().Length);
            msg.Data = ms.GetBuffer();

            return NetServer.SendMessage(msg, recipient, NetDeliveryMethod.ReliableOrdered);
        }

        public static Server Init()
        {
            return Instance ?? (Instance = new Server());
        }
    }
}