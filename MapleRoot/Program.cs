// Project: MapleRoot
// File: Program.cs
// Created By: Jared
// Last Update: 01 11, 2017 9:11 AM

using System.Threading.Tasks;
using MapleRoot.Network;

namespace MapleRoot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var client = Client.Create();
            Task.Run(() => Server.Init());
            Toolkit.Sleep(1000);
            Task.Run(() => client.Start()).Wait();
        }
    }
}