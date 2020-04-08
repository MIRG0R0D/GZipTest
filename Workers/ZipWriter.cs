using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZipTest
{
    public class ZipWriter : IWriter<Block>
    {
        private BlockingCollection<Block> outputQueue = new BlockingCollection<Block>();
        private BinaryWriter outputStream;
        public ZipWriter(BinaryWriter outputStream)
        {
            if (outputStream == null) throw new ArgumentNullException("Target stream is null");
            this.outputStream = outputStream;
        }

        public void CompleteAdding()
        {
            outputQueue.CompleteAdding();
        }

        public void WriteData(Block data)
        {
            outputStream.Write(data.Number);
            outputStream.Write(data.Data.Length);
            outputStream.Write(data.Data);
        }
    }
}
