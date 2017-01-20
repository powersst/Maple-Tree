// Project: MapleSeedU
// File: TitleInfo.cs
// Updated By: Jared
// 

namespace MapleSeedU.Models
{
    public class TitleInfo
    {
        public string TitleID { get; set; }
        public string TitleKey { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Ticket { get; set; }

        public override string ToString()
        {
            return Helper.RIC($"{Name} ({Region})");
        }
    }
}