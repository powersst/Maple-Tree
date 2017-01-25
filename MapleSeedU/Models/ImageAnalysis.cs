// Project: MapleSeed-Lite
// File: PictureAnalysis.cs
// Updated By: Jared
// 

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace MapleSeedU.Models
{
    public static class ImageAnalysis
    {
        public static byte[] ToBytes(this BitmapSource bitmapSource)
        {
            if (bitmapSource == null) return null;
            using (var stream = new MemoryStream()) {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        public static BitmapSource FromBytes(byte[] data)
        {
            if (data == null) return null;
            var stream = new MemoryStream(data);
            var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.OnDemand);
            return decoder.Frames[0];
        }

        private static Bitmap GetBitmap(BitmapSource source)
        {
            var bmp = new Bitmap(
                source.PixelWidth,
                source.PixelHeight,
                PixelFormat.Format24bppRgb);
            var data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            source.CopyPixels(
                Int32Rect.Empty,
                data.Scan0,
                data.Height * data.Stride,
                data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static Color GetRandomColour(BitmapSource bitmapSource)
        {
            while (true)
                try {
                    var random = new Random();
                    var x = random.Next(0, 1280);
                    var y = random.Next(0, 720);
                    var color = GetBitmap(bitmapSource).GetPixel(x, y);

                    if (color.Name == "0") return GetRandomColour(bitmapSource);
                    if (color.Name == "ffffff") return GetRandomColour(bitmapSource);

                    return color;
                }
                catch {
                    return Color.FromArgb(0xffffff);
                }
        }

        public static void SaveImage(BitmapSource bmp, string filename)
        {
            using (var fs = File.OpenWrite(filename)) {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(fs);
            }
        }
    }
}