// Project: MapleRoot
// File: MessageHandler.cs
// Updated By: Jared
// 

using System;
using System.IO;
using System.Text;
using Lidgren.Network;
using MapleRoot.Structs;
using ProtoBuf;
using ProtoBuf.Serializers;

namespace MapleRoot.Network.Messages
{
    public static class Incoming
    {
        public static void HandleServerMessage(object state)
        {
            var server = state as NetServer;
            if (server == null) return;

            NetIncomingMessage inMsg;
            while ((inMsg = server.ReadMessage()) != null) {
                switch (inMsg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(inMsg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        var status = (NetConnectionStatus)inMsg.ReadByte();
                        switch (status)
                        {
                            case NetConnectionStatus.Connected:
                                Console.WriteLine($"{inMsg.SenderConnection} has connected!");
                                break;
                        }
                        break;
                    default:
                        try
                        {
                            var ms = new MemoryStream(inMsg.Data);
                            var str = Encoding.UTF8.GetString(inMsg.Data);
                            var pack = Serializer.Deserialize<GraphicPack>(ms);

                            Console.WriteLine("Unhandled type: " + inMsg.MessageType);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                }
                server.Recycle(inMsg);
            }
        }

        public static void HandleClientMessage(object state)
        {
            var client = state as NetClient;

            if (client == null) return;

            NetIncomingMessage inMsg;
            while ((inMsg = client.ReadMessage()) != null)
            {
                switch (inMsg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(inMsg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + inMsg.MessageType);
                        break;
                }
                client.Recycle(inMsg);
            }
        }
    }
}