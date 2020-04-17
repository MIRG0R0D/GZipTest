using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZipTest
{
    public class DataRead : IReader<Block>
    {
        private BinaryReader inputStream;
        private int counter = 0;
        private int blockSize;

        public DataRead(BinaryReader inputStream, int blockSize)
        {
            if (blockSize <= 0) throw new ArgumentNullException("Block size can not be <= 0");
            if (inputStream == null) throw new ArgumentNullException("Source stream is null");

            this.inputStream = inputStream;
            this.blockSize = blockSize;
        }

        public bool ReadData(out Block block)
        {
            if (inputStream.BaseStream.Position != inputStream.BaseStream.Length)
            {
                byte[] buffer = inputStream.ReadBytes(blockSize);
                block = new Block(counter++, buffer);
                return true;
            }
            else
            {
                block = null;
                return false;
            }
        }
    }
}
