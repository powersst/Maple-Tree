// Project: MaryJane
// File: Network.cs
// Created By: Jared
// Last Update: 01 10, 2017 6:01 AM

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MaryJane
{
    public static class Network
    {
        private const string WII_USER_AGENT = "wii libnup/1.0";

        public static async Task DownloadFile(string url, string saveTo)
        {
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.UserAgent] = WII_USER_AGENT;
                wc.DownloadProgressChanged += DownloadProgressChanged;
                wc.DownloadDataCompleted += DownloadDataCompleted;
                var stream = await wc.OpenReadTaskAsync(url);
                var fs = File.Create(saveTo);
                await stream.CopyToAsync(fs);
                fs.Close();
            }
        }

        public static async Task<byte[]> DownloadData(string url)
        {
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.UserAgent] = WII_USER_AGENT;
                wc.DownloadProgressChanged += DownloadProgressChanged;
                wc.DownloadDataCompleted += DownloadDataCompleted;
                return await wc.DownloadDataTaskAsync(url);
            }
        }

        private static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Toolbelt.Form1?.UpdateProgress(e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive);
        }

        private static void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (Toolbelt.Form1 != null)
            {
                var pg = Toolbelt.Form1.progressBar;
                pg.Invoke(new Action(() => pg.Value = 0));
            }
            Toolbelt.SetStatus(string.Empty);
        }
    }
}