// Project: MapleSeedU
// File: Helper.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

#endregion

namespace MapleSeedU
{
    public static class Helper
    {
        public static UInt32 generateHashFromRawRPXData(byte[] rpxData, Int32 size)
        {
            UInt32 h = 0x3416DCBF;
            for (Int32 i = 0; i < size; i++)
            {
                UInt32 c = rpxData[i];
                h = (h << 3) | (h >> 29);
                h += c;
            }
            return h;
        }

        public static string RIC(string str)
        {
            return RemoveInvalidCharacters(str);
        }

        private static string RemoveInvalidCharacters(string str)
        {
            return
                Path.GetInvalidPathChars()
                    .Aggregate(str, (current, c) => current.Replace(c.ToString(), "-"))
                    .Replace(':', ' ');
        }


        public static string SizeSuffix(long bytes)
        {
            const int scale = 1024;
            string[] orders = {"GB", "MB", "KB", "Bytes"};
            var max = (long) Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders) {
                if (bytes > max)
                    return $"{decimal.Divide(bytes, max):##.##} {order}";

                max /= scale;
            }
            return "0 Bytes";
        }

        public static void RunCemu(string cemuPath, string target)
        {
            cemuPath = Path.GetFullPath(cemuPath);
            var workingDir = Path.GetDirectoryName(cemuPath);

            var process = new Process
            {
                StartInfo =
                {
                    FileName = cemuPath,
                    Arguments = $"{cemuPath} -g \"{target}\"",
                    WorkingDirectory = workingDir,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            process.Start();
        }
    }
}