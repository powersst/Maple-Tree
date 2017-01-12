// Project: MapleRoot
// File: Program.cs
// Updated By: Jared
// 

#region usings

using System.Threading.Tasks;
using MapleRoot.Network;

#endregion

namespace MapleRoot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var client = Client.Create();
            Task.Run(() => Server.Init());
            Toolkit.Sleep(3000);
            client.Start();
        }
    }
}