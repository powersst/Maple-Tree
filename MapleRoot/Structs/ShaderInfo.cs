// Project: MapleRoot
// File: ShaderInfo.cs
// Updated By: Jared
// 

#region usings

using ProtoBuf;

#endregion

namespace MapleRoot.Structs
{
    [ProtoContract]
    public class ShaderInfo
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public string Hash { get; set; }

        [ProtoMember(3)]
        public long Length { get; set; }

        [ProtoMember(4)]
        public byte[] Data { get; set; }
    }
}