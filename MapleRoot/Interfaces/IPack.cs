// Project: MapleRoot
// File: IPack.cs
// Created By: Jared
// Last Update: 01 11, 2017 9:40 AM

using ProtoBuf;

namespace MapleRoot.Interfaces
{
    [ProtoContract]
    public interface IPack
    {
        [ProtoMember(1)]
        string Hash { get; set; }

        [ProtoMember(2)]
        long Length { get; set; }

        [ProtoMember(3)]
        string Name { get; set; }

        [ProtoMember(4)]
        byte[] Data { get; set; }

        /// <summary>
        ///     Parameterless constructor required for protobuf
        /// </summary>
        void IPack();
    }
}