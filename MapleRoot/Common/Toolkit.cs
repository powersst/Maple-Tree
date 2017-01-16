// Project: MapleRoot
// File: Toolkit.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Globalization;
using System.IO;
using System.Management;
using System.Threading;
using Lidgren.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Timer = System.Timers.Timer;

#endregion

namespace MapleRoot.Common
{
    public static class Toolkit
    {
        static Toolkit()
        {
            if (GlobalTimer == null)
                GlobalTimer = new Timer();

            GlobalTimer.AutoReset = true;
            GlobalTimer.Interval = 30000;
            GlobalTimer.Start();
        }

        public static Timer GlobalTimer { get; }

        public static void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }

        public static string TempName()
        {
            return $"Guest#{new Random().Next(555555, 999999)}";
        }

        public static string UniqueID()
        {
            var drive = DriveInfo.GetDrives()[0].ToString().Replace(":", "").Replace("\\", "");
            var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            dsk.Get();
            var volumeSerial = dsk["VolumeSerialNumber"].ToString();
            return volumeSerial;
        }

        public static byte[] ToBson<T>(T data)
        {
            using (var ms = new MemoryStream()) {
                using (var writer = new BsonWriter(ms)) {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, data);
                    return ms.ToArray();
                }
            }
        }

        public static T FromBson<T>(string file)
        {
            using (var reader = new BsonReader(File.OpenRead(file))) {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }
        }

        public static T FromBson<T>(byte[] data)
        {
            using (var ms = new MemoryStream(data)) {
                using (var reader = new BsonReader(ms)) {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(reader);
                }
            }
        }

        public static string ToTitleID(string hexId)
        {
            var str1 = "85887bc1";
            var str2 = "1010ec00";

            var bytes1 = NetUtility.ToByteArray(str1);
            var bytes2 = NetUtility.ToByteArray(str2);

            var intValue1 = long.Parse(str1, NumberStyles.HexNumber);
            var intValue2 = long.Parse(str2, NumberStyles.HexNumber);

            var hex1 = intValue1.ToString("x8");
            var hex2 = intValue2.ToString("x8");
            return hex1;
        }
    }
}