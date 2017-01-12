// Project: MapleRoot
// File: Client.cs
// Created By: Jared
// Last Update: 01 11, 2017 11:40 AM

using System;
using System.IO;
using Lidgren.Network;
using MapleRoot.Structs;
using ProtoBuf;

namespace MapleRoot.Network
{
    public class Client
    {
        private static string ServerIP => "192.168.1.2";
        private static short ServerPort => 24862;
        private NetClient NetClient { get; set; }

        public void Start()
        {
            NetClient.Start();
            NetClient.Connect(ServerIP, ServerPort);

            var result = Send(new GraphicPack{Hash = "fakehash"});
        }

        private NetSendResult Send<T>(T data)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var msg = new NetOutgoingMessage {Data = ms.GetBuffer()};

            Console.WriteLine("Sending message");
            return NetClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        public static Client Create()
        {
            var client = new NetClient(new NetPeerConfiguration("MapleRootClient")
            {
                EnableUPnP = true,
                Port = ServerPort + 1
            });
            return new Client {NetClient = client};
        }
    }
}