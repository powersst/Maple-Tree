// Project: MapleRoot
// File: MapleBase.cs
// Updated By: Jared
// 

using System.Text;
using Lidgren.Network;
using MapleRoot.Enums;

namespace MapleRoot.Network
{
    public class MapleBase
    {
        internal NetSendResult Send(NetPeer peer, NetConnection recipient, byte[] data, MessageType m_type)
        {
            var msg = peer.CreateMessage();
            msg.Write(data.Length);
            msg.Write((byte)m_type);
            msg.Write(data);

            return peer.SendMessage(msg, recipient, NetDeliveryMethod.ReliableOrdered);
        }
    }
}