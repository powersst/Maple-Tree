﻿// Project: MapleRoot
// File: MapleBase.cs
// Updated By: Jared
// 

using System.Text;
using Lidgren.Network;
using MapleLib.Enums;

namespace MapleLib.Network
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

        internal NetSendResult SendFile(NetPeer peer, NetConnection recipient, byte[] data)
        {
            var msg = peer.CreateMessage();
            msg.Write(data.Length);
            msg.Write((byte) MessageType.ReceiveFile);
            msg.Write(data);

            return peer.SendMessage(msg, recipient, NetDeliveryMethod.ReliableOrdered);
        }
    }
}