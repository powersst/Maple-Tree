using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TgaLib
{
    /// <summary>
    /// Represents TGA image.
    /// </summary>
    public class TgaImage
    {
        #region properties

        /// <summary>
        /// Gets or sets a header.
        /// </summary>
        public Header Header { get; set; }

        /// <summary>
        /// Gets or sets an image ID.
        /// </summary>
        public byte[] ImageID { get; set; }

        /// <summary>
        /// Gets or sets a color map(palette).
        /// </summary>
        public byte[] ColorMap { get; set; }

        /// <summary>
        /// Gets or sets an image bytes array.
        /// </summary>
        public byte[] ImageBytes { get; set; }

        /// <summary>
        /// Gets or sets a developer area.
        /// </summary>
        public DeveloperArea DeveloperArea { get; set; }

        /// <summary>
        /// Gets or sets an extension area.
        /// </summary>
        public ExtensionArea ExtensionArea { get; set; }

        /// <summary>
        /// Gets or sets a footer.
        /// </summary>
        public Footer Footer { get; set; }

        #endregion  // properties


        #region constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        public TgaImage(BinaryReader reader)
        {
            Header = new Header(reader);

            ImageID = new byte[Header.IDLength];
            reader.Read(ImageID, 0, ImageID.Length);

            var bytesPerPixel = GetBytesPerPixel();

            ColorMap = new byte[Header.ColorMapLength * bytesPerPixel];
            reader.Read(ColorMap, 0, ColorMap.Length);

            ImageBytes = new byte[Header.Width * Header.Height * bytesPerPixel];
            ReadImageBytes(reader);

            if (Footer.HasFooter(reader))
            {
                Footer = new Footer(reader);

                if (Footer.ExtensionAreaOffset != 0)
                {
                    ExtensionArea = new ExtensionArea(reader, Footer.ExtensionAreaOffset);
                }

                if (Footer.DeveloperDirectoryOffset != 0)
                {
                    DeveloperArea = new DeveloperArea(reader, Footer.DeveloperDirectoryOffset);
                }
            }
        }

        #endregion  // constructors


        #region public methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>Returns a string that represents the current object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("[Header]");
            sb.AppendFormat("{0}", Header);
            sb.AppendLine();

            sb.AppendLine("[Extension Area]");
            if (Footer != null)
            {
                sb.AppendFormat("{0}", ExtensionArea);
            }
            else
            {
                sb.AppendLine("No extension area");
            }
            sb.AppendLine();

            sb.AppendLine("[Footer]");
            if (Footer != null)
            {
                sb.AppendFormat("{0}", Footer);
            }
            else
            {
                sb.AppendLine("No footer");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets a bitmap image.
        /// </summary>
        /// <returns>Returns a bitmap image.</returns>
        public BitmapSource GetBitmap()
        {
            int width = Header.Width;
            int height = Header.Height;
            var dpi = 96d;
            var pixelFormat = GetPixelFormat();
            var stride = GetBytesPerPixel() * width;
            var source = BitmapSource.Create(width, height, dpi, dpi, pixelFormat, null, ImageBytes, stride);
            source.Freeze();

            var transformedSource = Transform(source);

            return transformedSource;
        }

        #endregion  // public methods


        #region private methods

        /// <summary>
        /// Gets a pixel format of TGA image.
        /// </summary>
        /// <returns>Returns a pixel format of TGA image.</returns>
        private PixelFormat GetPixelFormat()
        {
            switch (Header.ImageType)
            {
                case ImageTypes.ColorMapped:
                case ImageTypes.CompressedColorMapped:
                    {
                        // color depth of color-mapped image is defined in the palette
                        switch (Header.ColorMapDepth)
                        {
                            case ColorDepth.Bpp15:
                            case ColorDepth.Bpp16:
                                return PixelFormats.Bgr555;

                            case ColorDepth.Bpp24:
                                return PixelFormats.Bgr24;

                            case ColorDepth.Bpp32:
                                return PixelFormats.Bgra32;

                            default:
                                throw new NotSupportedException(string.Format("Color depth isn't supported({0}bpp).", Header.ColorMapDepth));
                        }
                    }

                case ImageTypes.TrueColor:
                case ImageTypes.CompressedTrueColor:
                    {
                        switch (Header.PixelDepth)
                        {
                            case ColorDepth.Bpp15:
                            case ColorDepth.Bpp16:
                                return PixelFormats.Bgr555;

                            case ColorDepth.Bpp24:
                                return PixelFormats.Bgr24;

                            case ColorDepth.Bpp32:
                                return PixelFormats.Bgra32;

                            default:
                                throw new NotSupportedException(string.Format("Color depth isn't supported({0}bpp).", Header.PixelDepth));
                        }
                    }

                case ImageTypes.Monochrome:
                case ImageTypes.CompressedMonochrome:
                    {
                        switch (Header.PixelDepth)
                        {
                            case ColorDepth.Bpp8:
                                return PixelFormats.Gray8;

                            default:
                                throw new NotSupportedException(string.Format("Color depth isn't supported({0}bpp).", Header.PixelDepth));
                        }
                    }

                default:
                    throw new NotSupportedException(
                        string.Format("Image type \"{0}({1})\" isn't supported.", Header.ImageType, ImageTypes.ToFormattedText(Header.ImageType)));
            }
        }

        /// <summary>
        /// Gets bytes per pixel.
        /// </summary>
        /// <returns>Returns bytes per pixel.</returns>
        private int GetBytesPerPixel()
        {
            var pixelFormat = GetPixelFormat();
            return (pixelFormat.BitsPerPixel + 7) / 8;
        }

        /// <summary>
        /// Read an image data.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        private void ReadImageBytes(BinaryReader reader)
        {
            switch (Header.ImageType)
            {
                case ImageTypes.ColorMapped:
                case ImageTypes.TrueColor:
                case ImageTypes.Monochrome:
                    ReadUncompressedData(reader);
                    break;

                case ImageTypes.CompressedColorMapped:
                case ImageTypes.CompressedTrueColor:
                case ImageTypes.CompressedMonochrome:
                    DecodeRunLengthEncoding(reader);
                    break;

                default:
                    throw new NotSupportedException(
                        string.Format("Image type \"{0}({1})\" isn't supported.", Header.ImageType, ImageTypes.ToFormattedText(Header.ImageType)));
            }
        }

        /// <summary>
        /// Reads an uncompressed image data.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        private void ReadUncompressedData(BinaryReader reader)
        {
            // Use a pixel depth, not a color depth. So don't use GetBytesPerPixel().
            // (Pixel data is an index data, if an image type is color-mapped.)
            var bytesPerPixel = (Header.PixelDepth + 7) / 8;

            int numberOfPixels = Header.Width * Header.Height;

            for (int i = 0; i < numberOfPixels; ++i)
            {
                var pixelData = ExtractPixelData(reader.ReadBytes(bytesPerPixel));
                Array.Copy(pixelData, 0, ImageBytes, i * pixelData.Length, pixelData.Length);
            }
        }

        /// <summary>
        /// Decode a run-length encoded data.
        /// </summary>
        /// <param name="reader">A binary reader that contains TGA file. Caller must dipose the binary reader.</param>
        private void DecodeRunLengthEncoding(BinaryReader reader)
        {
            // most significant bit of repetitionCountField deetermins whether run-length packet or raw packet.
            const byte RunLengthPacketMask = 0x80;
            // rest of repetitionCountField represents number of pixels encoded by the packet - 1
            // (actual nmber of pixels encoded by the packet is repetitionCountField + 1)
            const byte RepetitionCountMask = 0x7F;

            // Use a pixel depth, not a color depth. So don't use GetBytesPerPixel().
            // (Pixel data is an index data, if an image type is color-mapped.)
            var bytesPerPixel = (Header.PixelDepth + 7) / 8;

            var numberOfPixels = Header.Width * Header.Height;
            int repetitionCount = 0;
            for (int processedPixels = 0; processedPixels < numberOfPixels; processedPixels += repetitionCount)
            {
                var repetitionCountField = reader.ReadByte();
                bool isRunLengthPacket = ((repetitionCountField & RunLengthPacketMask) != 0x00);
                repetitionCount = (repetitionCountField & RepetitionCountMask) + 1;

                if (isRunLengthPacket)
                {
                    // Run-length packet
                    var pixelData = ExtractPixelData(reader.ReadBytes(bytesPerPixel));
                    // Repeats same pixel data
                    for (int i = 0; i < repetitionCount; ++i)
                    {
                        Array.Copy(pixelData, 0, ImageBytes, (processedPixels + i) * pixelData.Length, pixelData.Length);
                    }
                }
                else
                {
                    // Raw packet
                    // Repeats different pixel data
                    for (int i = 0; i < repetitionCount; ++i)
                    {
                        var pixelData = ExtractPixelData(reader.ReadBytes(bytesPerPixel));
                        Array.Copy(pixelData, 0, ImageBytes, (processedPixels + i) * pixelData.Length, pixelData.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Extracts a pixel data.
        /// </summary>
        /// <param name="rawPixelData">A raw pixel data in the TGA file.</param>
        /// <returns>
        /// Returns a pixel data in the palette, if an image type is color-mapped.
        /// Returns a raw pixel data, if an image type is RGB or grayscale.
        /// </returns>
        private byte[] ExtractPixelData(byte[] rawPixelData)
        {
            byte[] pixelData = null;

            switch (Header.ImageType)
            {
                case ImageTypes.ColorMapped:
                case ImageTypes.CompressedColorMapped:
                    {
                        // Extracts a pixel data in the palette.
                        var paletteIndex = GetPaletteIndex(rawPixelData);
                        var bytesPerPixel = GetBytesPerPixel();
                        var realPixelData = new byte[bytesPerPixel];
                        Array.Copy(ColorMap,
                                   (Header.ColorMapStart + paletteIndex) * bytesPerPixel,
                                   realPixelData,
                                   0,
                                   realPixelData.Length);
                        pixelData = realPixelData;
                    }
                    break;

                case ImageTypes.TrueColor:
                case ImageTypes.Monochrome:
                case ImageTypes.CompressedTrueColor:
                case ImageTypes.CompressedMonochrome:
                    // Returns a raw pixel data as is.
                    pixelData = rawPixelData;
                    break;

                default:
                    throw new NotSupportedException(
                        string.Format("Image type \"{0}({1})\" isn't supported.", Header.ImageType, ImageTypes.ToFormattedText(Header.ImageType)));
            }

            // Overwrite an alpha with 0xFF, when the color depth has an alpha value,
            // but an alpha bits is not specified in the header.
            if (HasAlpha() && IgnoreAlpha())
            {
                pixelData[ArgbOffset.Alpha] = 0xFF;
            }

            return pixelData;
        }

        /// <summary>
        /// Gets a palette index.
        /// </summary>
        /// <param name="indexData">An index data.</param>
        /// <returns>Returns a palette index.</returns>
        private long GetPaletteIndex(byte[] indexData)
        {
            switch (indexData.Length)
            {
                case 1:
                    return indexData[0];

                case 2:
                    return BitConverter.ToUInt16(indexData, 0);

                case 4:
                    return BitConverter.ToUInt32(indexData, 0);

                default:
                    throw new NotSupportedException(string.Format("A byte length of index data is not supported({0}bytes).", indexData.Length));
            }
        }

        /// <summary>
        /// Gets whether has an alpha value or not.
        /// </summary>
        /// <returns>
        /// Returns true, if TGA image has an alpha value.
        /// Returns false, if TGA image don't have an alpha value.
        /// </returns>
        private bool HasAlpha()
        {
            return GetBytesPerPixel() == 4;
        }

        /// <summary>
        /// Gets whether ignore an alpha value or not.
        /// </summary>
        /// <returns>
        /// Returns true, if an alpha value should be ignored.
        /// Returns false, if an alpha value shouldn't be ignored.
        /// </returns>
        private bool IgnoreAlpha()
        {
            if (!HasAlpha())
            {
                throw new InvalidOperationException("Image don't have an alpha value.");
            }

            return (Header.AttributeBits != 0);
        }

        /// <summary>
        /// Transforms an image according to <see cref="Header.ImageOrigin"/>.
        /// </summary>
        /// <param name="source">A source image.</param>
        /// <returns>Returns a transformed image.</returns>
        private BitmapSource Transform(BitmapSource source)
        {
            double scaleX = 1.0;
            double scaleY = 1.0;

            switch (Header.ImageOrigin)
            {
                case ImageOriginTypes.BottomLeft:
                    scaleX = 1.0;
                    scaleY = -1.0;
                    break;

                case ImageOriginTypes.BottomRight:
                    scaleX = -1.0;
                    scaleY = -1.0;
                    break;

                case ImageOriginTypes.TopLeft:
                    scaleX = 1.0;
                    scaleY = 1.0;
                    break;

                case ImageOriginTypes.TopRight:
                    scaleX = -1.0;
                    scaleY = 1.0;
                    break;

                default:
                    throw new NotSupportedException(string.Format("Image origin \"{0}\" isn't supported.", Header.ImageOrigin));
            }

            var transform = new ScaleTransform(scaleX, scaleY, 0.5, 0.5);
            return new TransformedBitmap(source, transform);
        }

        #endregion  // private methods
    }
}
