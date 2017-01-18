// Project: MapleTree
// File: Program.cs
// Updated By: Jared
// 

#region usings

using System;
using MapleLib.Common;
using MapleLib.Network;

#endregion

namespace MapleTree
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            MapleServer.Init();

            var key = new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false);
            while (Console.ReadKey() != key) {
                Console.Write('\r');
                Toolkit.Sleep(1);
            }
        }
    }
}