using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GZipTest
{
    public class DataRead : IReader<Block>
    {
        private const int DEFAULT_BLOCK_SIZE = 1048576;//todo make own, find out value meaning

        private BinaryReader inputStream;
        private int counter = 0;
        private int blockSize;
        
        public DataRead(BinaryReader inputStream, int blockSize = DEFAULT_BLOCK_SIZE)
        {
            if (blockSize < 1) throw new ArgumentNullException("Block size must be > 0");
            if (inputStream == null) throw new ArgumentNullException("Source stream is null");

            this.inputStream = inputStream;
            this.blockSize = blockSize;
        }

        public Block ReadData()
        {
            if (inputStream.BaseStream.Position != inputStream.BaseStream.Length)
            {
                byte[] buffer = inputStream.ReadBytes(blockSize);
                return new Block(counter++, buffer);
            }
            else return null;
        }
    }
}
