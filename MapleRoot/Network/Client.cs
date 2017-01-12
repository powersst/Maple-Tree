// Project: MapleRoot
// File: Client.cs
// Updated By: Jared
// 

#region usings

using System;
using System.IO;
using System.Net;
using System.Threading;
using Lidgren.Network;
using MapleRoot.Network.Messages;
using MapleRoot.Structs;
using ProtoBuf;

#endregion

namespace MapleRoot.Network
{
    public class Client
    {
        private NetClient NetClient { get; set; }

        public void Start()
        {
            while (true)
            {
                var result = Send(new GraphicPack { Hash = "fakehash" });
                //var result = Send(978477049537635276);
                Toolkit.Sleep(10000);
            }
        }

        private NetSendResult Send<T>(T data)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var msg = NetClient.CreateMessage((int) ms.Length);
            msg.Data = ms.ToArray();
            
            var result = NetClient.SendMessage(msg, NetDeliveryMethod.ReliableSequenced);
            if (result == NetSendResult.Sent) Console.WriteLine("Message Sent!");
            return result;
        }

        public static Client Create()
        {
            var client = new NetClient(new NetPeerConfiguration("MapleTree") {AutoFlushSendQueue = true});
            client.RegisterReceivedCallback(Incoming.HandleClientMessage, new SynchronizationContext());

            client.Start();

            var hail = client.CreateMessage("Hail from: " + client.UniqueIdentifier);
            client.Connect(Config.ServerIP, Config.ServerPort, hail);
            
            return new Client {NetClient = client };
        }
    }
}