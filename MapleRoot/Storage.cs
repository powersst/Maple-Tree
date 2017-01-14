// Project: MapleRoot
// File: Storage.cs
// Updated By: Jared
// 

#region usings

using System;
using System.IO;
using Lidgren.Network;
using MapleRoot.Enums;
using MapleRoot.Network;
using MapleRoot.Structs;
using ProtoBuf;

#endregion

namespace MapleRoot
{
    public static class Storage
    {
        private static string StorageDir => Path.GetFullPath("storage");

        public static bool AddToStorage(StorageData sd)
        {
            try {
                if (!Directory.Exists(StorageDir))
                    Directory.CreateDirectory(StorageDir);

                using (var ms = new MemoryStream()) {
                    Serializer.Serialize(ms, sd);
                    var file = Path.Combine(StorageDir, sd.Name);

                    if (!File.Exists(file))
                        File.WriteAllBytes($"{file}", ms.ToArray());

                    if (!sd.Shader) {
                        Console.WriteLine($"[+Share] {sd.Name} - {NetUtility.ToHumanReadable(sd.Length)}");
                        return true;
                    }

                    var c_sd = Serializer.Deserialize<StorageData>(File.OpenRead(file));
                    if (sd.Length > c_sd.Length) {
                        Console.WriteLine($"[^Share] {sd.Name} - {NetUtility.ToHumanReadable(sd.Length)}");
                        File.WriteAllBytes($"{file}", ms.ToArray());
                    }
                }

                return true;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        public static StorageData Upload(MapleClient client, string file, string name, bool shader)
        {
            var sd = ParseFile(file, name, shader);
            client.Send(sd, MessageType.StorageUpload);
            return sd;
        }

        private static StorageData ParseFile(string file, string name, bool shader)
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
            return sd;
        }
    }
}