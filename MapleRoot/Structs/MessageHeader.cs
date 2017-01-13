// Project: MapleRoot
// File: MessageHeader.cs
// Updated By: Jared
// 

using Lidgren.Network;
using MapleRoot.Enums;

namespace MapleRoot.Structs
{
    public struct MessageHeader
    {
        public int Length { get; set; }
        public MessageType Type { get; private set; }
        public byte[] Data { get; private set; }

        public static MessageHeader Parse(NetIncomingMessage inMsg)
        {
            var header = new MessageHeader
            {
                Length = inMsg.ReadInt32(),
                Type = (MessageType) inMsg.ReadByte()
            };
            header.Data = inMsg.ReadBytes(header.Length);
            return header;
        }
    }
}