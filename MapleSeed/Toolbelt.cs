// Project: MapleSeed
// File: Toolbelt.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cemu_UI;
using libWiiSharp;
using MaryJane.Properties;
using NUS_Downloader;

#endregion

namespace MaryJane
{
    public static class Toolbelt
    {
        public static readonly string Version = $" - Git {Resources.version.Trim('\n')}";

        public static Database Database { get; internal set; }
        public static Settings Settings { get; internal set; }
        public static Form1 Form1 { get; set; }

        public static string RIC(string str)
        {
            return RemoveInvalidCharacters(str);
        }

        private static string RemoveInvalidCharacters(string str)
        {
            return
                Path.GetInvalidPathChars()
                    .Aggregate(str, (current, c) => current.Replace(c.ToString(), ""))
                    .Replace(':', ' ');
        }

        public static async void AppendLog(string msg, Color color = default(Color))
        {
#if (DEBUG)
            try {
                Logger.log(msg);
            }
            catch {}
#endif
            if (Form1 != null)
                await Task.Run(() => Form1?.AppendLog(msg, color));
        }

        public static void SetStatus(string msg, Color color = default(Color))
        {
            Form1?.SetStatus(msg, color);
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

        public static bool IsValid(TMD_Content content, string contentFile)
        {
            if (!File.Exists(contentFile)) return false;

            return (ulong) new FileInfo(contentFile).Length == content.Size;
        }

        public static void RunCemu(string cemuPath, string rpx)
        {
            try {
                var workingDir = Path.GetDirectoryName(cemuPath);
                if (workingDir == null) return;

                var o1 = Settings.FullScreenMode ? "-f" : "";
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = cemuPath,
                        Arguments = $"{o1} -g \"{rpx}\"",
                        WorkingDirectory = workingDir,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };

                process.Start();
                AppendLog("Started playing a game!");
            }
            catch (Exception ex) {
                AppendLog("Error!\r\n" + ex.Message);
            }
        }

        public static void CDecrypt(string tdir)
        {
            try {
                var cdecrypt = Path.Combine(tdir, "CDecrypt.exe");
                var libeay32 = Path.Combine(tdir, "libeay32.dll");

                if (!GZip.Decompress(Resources.CDecrypt, cdecrypt))
                    AppendLog("Error decrypting contents!\r\n       Could not extract CDecrypt.");

                if (!GZip.Decompress(Resources.libeay32, libeay32))
                    AppendLog("Error decrypting contents!\r\n       Could not extract libeay32.");

                var cdecryptP = new Process
                {
                    StartInfo =
                    {
                        FileName = cdecrypt,
                        Arguments = "tmd cetk",
                        WorkingDirectory = tdir,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };

                cdecryptP.Start();
                while (!cdecryptP.StandardOutput.EndOfStream) {
                    cdecryptP.StandardOutput.ReadLine();
                    AppendLog(cdecryptP.StandardOutput.ReadLine());
                    Application.DoEvents();
                }
                cdecryptP.WaitForExit();
                cdecryptP.Dispose();

                AppendLog("Finished decrypting contents.");
            }
            catch (Exception ex) {
                AppendLog("Error decrypting contents!\r\n" + ex.Message);
            }
        }
    }
}