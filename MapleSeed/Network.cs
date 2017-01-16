// Project: MapleSeed
// File: Network.cs
// Updated By: Jared
// 

#region usings

using System;
using System.Net;
using System.Threading.Tasks;

#endregion

namespace MapleSeed
{
    public static class Network
    {
        private const string WII_USER_AGENT = "wii libnup/1.0";

        public static async Task DownloadFileAsync(string url, string saveTo)
        {
            var wc = new WebClient {Headers = {[HttpRequestHeader.UserAgent] = WII_USER_AGENT}};
            wc.DownloadProgressChanged += DownloadProgressChanged;
            wc.DownloadDataCompleted += DownloadDataCompleted;

            await wc.DownloadFileTaskAsync(new Uri(url), saveTo);

            while (wc.IsBusy) await Task.Delay(100);
            wc.Dispose();
        }

        public static async void DownloadData(string url, DownloadDataCompletedEventHandler downloadDataCompleted)
        {
            using (var wc = new WebClient()) {
                wc.Headers[HttpRequestHeader.UserAgent] = WII_USER_AGENT;
                wc.DownloadProgressChanged += DownloadProgressChanged;
                wc.DownloadDataCompleted += downloadDataCompleted;
                wc.DownloadDataCompleted += DownloadDataCompleted;
                var data = await wc.DownloadDataTaskAsync(new Uri(url));
            }
        }

        private static void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Toolbelt.Form1?.UpdateProgress(e.ProgressPercentage, e.BytesReceived, e.TotalBytesToReceive);
        }

        private static void DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {

        }
    }
}