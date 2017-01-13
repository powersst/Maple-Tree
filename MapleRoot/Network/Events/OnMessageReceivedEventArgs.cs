// Project: MapleRoot
// File: OnMessageReceivedEventArgs.cs
// Updated By: Jared
// 

using MapleRoot.Enums;

namespace MapleRoot.Network.Events
{
    public class OnMessageReceivedEventArgs
    {
        public int lenth { get; set; }
        public byte[] buffer { get; set; }
        public MessageType messageType { get; set; }
    }
}