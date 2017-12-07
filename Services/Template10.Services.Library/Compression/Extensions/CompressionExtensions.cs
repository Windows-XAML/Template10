using System;

namespace Template10.Extensions
{
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    internal static class CompressionExtensions
    {
        public static string Compress(this string text, CompressionMethods method = CompressionMethods.gzip)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (method == CompressionMethods.gzip)
            {
                return Zip(text);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public static string Decompress(this string text, CompressionMethods method = CompressionMethods.gzip)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (method == CompressionMethods.gzip)
            {
                return Unzip(text);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private static string Zip(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gZipStream.Write(buffer, 0, buffer.Length);
                }
                memoryStream.Position = 0;
                var compressedData = new byte[memoryStream.Length];
                memoryStream.Read(compressedData, 0, compressedData.Length);
                var gZipBuffer = new byte[compressedData.Length + 4];
                Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
                return Convert.ToBase64String(gZipBuffer);
            }
        }

        private static string Unzip(string text)
        {
            var gZipBuffer = Convert.FromBase64String(text);
            using (var memoryStream = new MemoryStream())
            {
                var dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
                var buffer = new byte[dataLength];
                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }
                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}