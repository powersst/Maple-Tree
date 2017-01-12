// Project: MapleRoot
// File: GraphicPack.cs
// Created By: Jared
// Last Update: 01 11, 2017 10:33 AM

using MapleRoot.Interfaces;
using ProtoBuf;

namespace MapleRoot.Structs
{
    [ProtoContract]
    public class GraphicPack : IPack
    {
        public string Hash { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }

        public void IPack()
        {
        }
    }
}