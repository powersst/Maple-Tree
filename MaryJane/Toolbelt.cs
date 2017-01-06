using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MaryJane
{
    public static class Toolbelt
    {
        private static Database Db { get; set; }
        public static Database Database => Db ?? (Db = new Database());

        public static Form1 Form1 { get; set; }

        public static void SetStatus(string msg, Color color = default(Color))
        {
            Form1?.SetStatus(msg, color);
        }

        public static void CDecrypt(string tdir)
        {
            try
            {
                //if (!GZip.Decompress(Resources.CDecrypt, tdir + "/CDecrypt.exe"))
                //nus.FireDebug("Error decrypting contents!\r\n       Could not extract CDecrypt.");

                //if (!GZip.Decompress(Resources.libeay32, tdir + "/libeay32.dll"))
                //nus.FireDebug("Error decrypting contents!\r\n       Could not extract libeay32.");

                var cdecryptP = new Process
                {
                    StartInfo =
                    {
                        FileName = tdir + "/CDecrypt.exe",
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

                while (!cdecryptP.StandardOutput.EndOfStream)
                    Application.DoEvents();
                cdecryptP.WaitForExit();
                cdecryptP.Dispose();

                File.Delete(tdir + "/CDecrypt.exe");
                File.Delete(tdir + "/libeay32.dll");

                //nus.FireDebug("Finished decrypting contents.");
            }
            catch (Exception ex)
            {
                //nus.FireDebug("Error decrypting contents!\r\n" + ex.Message);
            }
        }
        
        public static string SizeSuffix(long bytes)
        {
            const int scale = 1024;
            string[] orders = { "GB", "MB", "KB", "Bytes" };
            var max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders)
            {
                if (bytes > max)
                    return $"{decimal.Divide(bytes, max):##.##} {order}";

                max /= scale;
            }
            return "0 Bytes";
        }
    }
}