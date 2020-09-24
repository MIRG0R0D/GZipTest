using System;
using System.IO;

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

        /// <summary>
        /// read original data from input stream
        /// </summary>
        /// <param name="block">data converted to block</param>
        /// <returns>true if success</returns>
        public bool ReadData(out Block block)
        {
            if (inputStream.BaseStream.Position != inputStream.BaseStream.Length)
            {
                block = BlockPool.GetBlock();
                block.Data = inputStream.ReadBytes(blockSize);
                block.Number = counter++;
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
