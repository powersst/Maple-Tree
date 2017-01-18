// Project: MapleLib
// File: OnMessageReceivedEventArgs.cs
// Updated By: Jared
// 

#region usings

using MapleLib.Structs;

#endregion

namespace MapleLib.Network.Events
{
    public class OnMessageReceivedEventArgs
    {
        public MessageHeader Header { get; set; }
    }
}