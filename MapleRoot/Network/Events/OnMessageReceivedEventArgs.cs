// Project: MapleRoot
// File: OnMessageReceivedEventArgs.cs
// Updated By: Jared
// 

using System.Dynamic;
using MapleRoot.Enums;
using MapleRoot.Structs;

namespace MapleRoot.Network.Events
{
    public class OnMessageReceivedEventArgs
    {
        public MessageHeader Header { get; set; }
    }
}