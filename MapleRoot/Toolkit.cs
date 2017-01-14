// Project: MapleRoot
// File: Toolkit.cs
// Updated By: Jared
// 

#region usings

using Lidgren.Network;
using System;
using System.Threading;

#endregion

namespace MapleRoot
{
    public static class Toolkit
    {
        public static void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }

        public static string TempName()
        {
            return $"Guest#{new Random().Next(555555, 999999)}";
        }

        public static string ToTitleID(string hexId)
        {
            string str1 = "85887bc1";
            string str2 = "1010ec00";

            var bytes1 = NetUtility.ToByteArray(str1);
            var bytes2 = NetUtility.ToByteArray(str2);

            var intValue1 = long.Parse(str1, System.Globalization.NumberStyles.HexNumber);
            var intValue2 = long.Parse(str2, System.Globalization.NumberStyles.HexNumber);

            var hex1 = intValue1.ToString("x8");
            var hex2 = intValue2.ToString("x8");
            return hex1;
        }
    }
}