using System;
using System.IO;
using System.Net;

namespace MaryJane
{
    public static class Network
    {
        private const string WII_USER_AGENT = "wii libnup/1.0";

        public static void DownloadFile(string url, string saveTo)
        {
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.UserAgent] = WII_USER_AGENT;
                wc.DownloadProgressChanged += DownloadProgressChanged;
                wc.DownloadDataCompleted += DownloadDataCompleted;
                var data = wc.DownloadData(url);
                File.WriteAllBytes(saveTo, data);
            }
        }

        public static byte[] DownloadData(string url)
        {
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.UserAgent] = WII_USER_AGENT;
                wc.DownloadProgressChanged += DownloadProgressChanged;
                wc.DownloadDataCompleted += DownloadDataCompleted;
                return wc.DownloadData(url);
            }
        }

        private static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var pg = Toolbelt.Form1.progressBar;

            if (pg.InvokeRequired)
                pg.Invoke(new Action(() => { pg.Value = e.ProgressPercentage; }));
            else
                pg.Value = e.ProgressPercentage;

            var received = Toolbelt.SizeSuffix(e.BytesReceived);
            var toReceive = Toolbelt.SizeSuffix(e.TotalBytesToReceive);

            Toolbelt.SetStatus($"{e.ProgressPercentage}% | {received} / {toReceive}");
        }

        private static void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            var pg = Toolbelt.Form1.progressBar;

            if (pg.InvokeRequired)
                pg.Invoke(new Action(() => { pg.Value = 0; }));
            else
                pg.Value = 0;
        }
    }
}