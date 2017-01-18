// Project: MapleLib
// File: UserData.cs
// Updated By: Jared
// 

#region usings

using ProtoBuf;

#endregion

namespace MapleLib.Structs
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