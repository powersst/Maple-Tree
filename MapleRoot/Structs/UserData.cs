// Project: MapleRoot
// File: UserData.cs
// Updated By: Jared
// 

using ProtoBuf;

namespace MapleRoot.Structs
{
    [ProtoContract]
    public class UserData
    {
        [ProtoMember(1)]
        public string Username { get; set; }

        [ProtoMember(2)]
        public string Serial { get; set; }
    }
}