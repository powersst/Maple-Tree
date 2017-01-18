// Project: MapleLib
// File: MessageHeader.cs
// Updated By: Jared
// 

#region usings

using Lidgren.Network;
using MapleLib.Enums;

#endregion

namespace MapleLib.Structs
{
    public struct MessageHeader
    {
        private int Length { get; set; }
        public MessageType Type { get; private set; }
        public byte[] Data { get; private set; }
        private NetIncomingMessage Message { get; set; }

        public static MessageHeader Parse(NetIncomingMessage inMsg)
        {
            var header = new MessageHeader
            {
                Length = inMsg.ReadInt32(),
                Type = (MessageType) inMsg.ReadByte(),
                Message = inMsg
            };
            header.Data = inMsg.ReadBytes(header.Length);
            inMsg.Position = 0;
            return header;
        }
    }
}