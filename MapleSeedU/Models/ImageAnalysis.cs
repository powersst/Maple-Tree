// Project: MapleSeed-Lite
// File: PictureAnalysis.cs
// Updated By: Jared
// 

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace MapleSeedU.Models
{
    public static class ImageAnalysis
    {
        private static Bitmap GetBitmap(BitmapSource source)
        {
            var bmp = new Bitmap(
                source.PixelWidth,
                source.PixelHeight,
                PixelFormat.Format32bppPArgb);
            var data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppPArgb);
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
                    var x = new Random().Next(0, bitmapSource.PixelWidth);
                    var y = new Random().Next(0, bitmapSource.PixelHeight);
                    var color = GetBitmap(bitmapSource).GetPixel(x, y);

                    if (color.R == 255 && color.G == 255 && color.B == 255) return Color.FromArgb(0xffffff);
                    if (color.R == 0 && color.G == 0 && color.B == 0) return Color.FromArgb(0xffffff);

                    return color;
                }
                catch {
                    return Color.FromArgb(0xffffff);
                }
        }
    }
}