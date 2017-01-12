// Project: MapleRoot
// File: GraphicPack.cs
// Updated By: Jared
// 

#region usings

using System;
using MapleRoot.Interfaces;
using ProtoBuf;

#endregion

namespace MapleRoot.Structs
{
    [ProtoContract]
    [Serializable]
    public class GraphicPack : IPack
    {
        #region IPack Members
        [ProtoMember(1)]
        public string Hash { get; set; }
        [ProtoMember(2)]
        public long Length { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public byte[] Data { get; set; }

        public void IPack() {}

        #endregion
    }
}