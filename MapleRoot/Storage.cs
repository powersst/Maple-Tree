// Project: MapleRoot
// File: Storage.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Configuration;
using System.IO;
using System.Text;
using Lidgren.Network;
using MapleRoot.Common;
using MapleRoot.Enums;
using MapleRoot.Network;
using MapleRoot.Structs;
using ProtoBuf;

#endregion

namespace MapleRoot
{
    public static class Storage
    {
        public static string Dir => Path.GetFullPath("storage");

        public static bool AddToStorage(StorageData sd)
        {
            try {
                var dir = Path.Combine(Dir, sd.Serial);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var bson = Toolkit.ToBson(sd);
                var file = Path.Combine(dir, sd.Name);

                if (!File.Exists(file))
                    File.WriteAllBytes($"{file}", bson);
                
                var c_sd = Toolkit.FromBson<StorageData>(file);
                if (sd.Length <= c_sd.Length) {
                    Console.WriteLine($"[+Share] {sd.Name} - {NetUtility.ToHumanReadable(sd.Length)}");
                }
                else {
                    Console.WriteLine($"[^Share] {sd.Name} - {NetUtility.ToHumanReadable(sd.Length)}");
                    File.WriteAllBytes($"{file}", bson);
                }

                return true;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        public static StorageData Upload(MapleClient client, string file, string name, string serial, bool shader)
        {
            var sd = ParseFile(file, name, serial, shader);
            client.Send(sd, MessageType.StorageUpload);
            return sd;
        }

        private static StorageData ParseFile(string file, string name, string serial, bool shader)
        {
            if (!File.Exists(file))
                throw new Exception($"File [{file}] doesn't exist!");

            var sd = new StorageData();

            using (var fs = File.OpenRead(file)) {
                if (fs.Length <= 0)
                    throw new Exception("File is empty, there's nothing to upload!");

                using (var ms = new MemoryStream()) {
                    fs.CopyTo(ms);
                    sd.Length = ms.Length;
                    sd.Data = ms.ToArray();
                }
            }
            sd.Name = name;
            var bHash = NetUtility.ComputeSHAHash(sd.Data);
            sd.Hash = $"{BitConverter.ToString(bHash).Replace("-", ""),12}";
            sd.Shader = shader;
            sd.Serial = serial;
            return sd;
        }
    }
}