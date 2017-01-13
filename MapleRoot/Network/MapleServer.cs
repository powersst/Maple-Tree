// Project: MapleRoot
// File: MapleServer.cs
// Updated By: Jared
// 

#region usings

using System;
using System.IO;
using System.Text;
using System.Threading;
using Lidgren.Network;
using MapleRoot.Network.Events;
using MapleRoot.Structs;
using ProtoBuf;

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

            NetServer.Start();
        }

        private NetServer NetServer { get; }

        private static MapleServer Instance { get; set; }

        private NetSendResult Send<T>(T data, NetConnection recipient)
        {
            var ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var len = (int)ms.Length;

            var msg = NetServer.CreateMessage(len);
            msg.Write(len);
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
                            var buf = inMsg.ReadBytes(len);
                            OnMessageReceived.Invoke(Instance, new OnMessageReceivedEventArgs { lenth = len, buffer = buf });
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