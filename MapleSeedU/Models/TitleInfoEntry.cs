// Project: MapleSeedU
// File: TitleInfo.cs
// Updated By: Jared
// 

using System.IO;

namespace MapleSeedU.Models
{
    public class TitleInfoEntry
    {
        public string TitleID { get; set; }
        public string TitleKey { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Ticket { get; set; }

        public TitleInfoEntry(string fullPath)
        {
            Name = Path.GetFileName(fullPath);
        }

        public override string ToString()
        {
            return Helper.RIC($"{Name} - {Region}");
        }
    }
}