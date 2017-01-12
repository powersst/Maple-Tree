// Project: MapleRoot
// File: Server.cs
// Created By: Jared
// Last Update: 01 11, 2017 11:19 AM

using System;
using System.IO;
using Lidgren.Network;
using ProtoBuf;

namespace MapleRoot.Network
{
    public class Server
    {
        private Server()
        {
            var config = new NetPeerConfiguration("MapleRootServer") {Port = ServerPort};

            NetServer = new NetServer(config);
            NetServer.Start();
            StartReceiving();
        }

        private NetServer NetServer { get; }

        private static short ServerPort => 24862;

        internal static Server Instance { get; private set; }

        private NetSendResult Send<T>(T data, NetConnection recipient)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var msg = new NetOutgoingMessage {Data = ms.GetBuffer()};

            return NetServer.SendMessage(msg, recipient, NetDeliveryMethod.ReliableOrdered);
        }

        private void StartReceiving()
        {
            while (NetServer.Status == NetPeerStatus.Running)
            {
                var msg = NetServer.ReadMessage();
                if (msg == null)
                {
                    Toolkit.Sleep(1);
                    continue;
                }

                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
                NetServer.Recycle(msg);
            }
        }

        public static Server Init()
        {
            return Instance ?? (Instance = new Server());
        }
    }
}