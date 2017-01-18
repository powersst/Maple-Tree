// Project: MapleLib
// File: StorageData.cs
// Updated By: Jared
// 

#region usings

using ProtoBuf;

#endregion

namespace MapleLib.Structs
{
    [ProtoContract]
    public struct StorageData
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public long Length { get; set; }

        [ProtoMember(3)]
        public string Hash { get; set; }

        [ProtoMember(4)]
        public byte[] Data { get; set; }

        [ProtoMember(5)]
        public bool Shader { get; set; }

        [ProtoMember(6)]
        public string Serial { get; set; }
    }
}