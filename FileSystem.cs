using System;
using System.IO;

namespace MaryJane
{
    public static class FileSystem
    {
        public static async void MoveFile(string from, string to)
        {
            if (!File.Exists(from))
                throw new Exception("File does not exist!");

            if (File.Exists(to))
                File.Delete(to);

            var f1 = File.OpenRead(from);
            var t1 = File.OpenWrite(to);
            await f1.CopyToAsync(t1);
            f1.Close();
            t1.Close();

            if (new FileInfo(from).Length == new FileInfo(to).Length)
                File.Delete(from);

            //await Task.Run(() => Toolbelt.Form1.UpdateProgress(0, f1.Position, f1.Length));
        }
    }
}