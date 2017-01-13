using MapleSeed;

namespace MaryJane.Structs
{
    public struct WiiUTitle
    {
        public string TitleID { get; set; }
        public string TitleKey { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Ticket { get; set; }

        public override string ToString()
        {
            return Toolbelt.RIC($"{Name} ({Region})");
        }
    }
}