using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GZipTest
{
    public class GZip
    {
        public static byte[] Compress(byte[] rawData)
        {

            using (MemoryStream rawStream = new MemoryStream(rawData))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (GZipStream compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        rawStream.CopyTo(compressionStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }
    

    public static byte[] Decompress(byte[] compressedData)
    {
        using (MemoryStream compressedStream = new MemoryStream(compressedData))
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (GZipStream decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(outputStream);
                }
                return outputStream.ToArray();
            }
        }
    }
}
}
