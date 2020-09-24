using System;
using System.IO;

namespace GZipTest
{
    public class ZipWriter : IWriter<Block>
    {
        private BinaryWriter outputStream;
        
        private static object locker = new object();
        public ZipWriter(BinaryWriter outputStream)
        {
            if (outputStream == null) throw new ArgumentNullException("Target stream is null");
            this.outputStream = outputStream;
            outputStream.Write("MyFileType format; ©Mirg0r0d");
        }

        /// <summary>
        /// write compressed data to stream
        /// </summary>
        /// <param name="data">compressed data</param>
        public void WriteData(Block data)
        {
            lock (locker)
            {
                outputStream.Write(data.Number);
                outputStream.Write(data.Data.Length);
                outputStream.Write(data.Data);
            }
            BlockPool.ReturnObject(data);
        }
    }
}
