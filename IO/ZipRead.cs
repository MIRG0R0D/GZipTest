using System;
using System.IO;

namespace GZipTest
{
    public class ZipRead : IReader<Block>
    {

        private BinaryReader inputStream;

        public ZipRead(BinaryReader inputStream)
        {
            this.inputStream = inputStream;
            if (!IsFileHeaderCorrect()) throw new FileLoadException("File header corrupted or missing");
            
        }

        /// <summary>
        /// check for file header correctness
        /// </summary>
        /// <returns>tru if correct</returns>
        private bool IsFileHeaderCorrect()
        {
            return inputStream.ReadString().Contains("MyFileType");
        }

        /// <summary>
        /// read block of compressed data
        /// </summary>
        /// <param name="block">compressed data</param>
        /// <returns>true if success</returns>
        public bool ReadData(out Block block)
        {
            if (inputStream.BaseStream.Position != inputStream.BaseStream.Length)
            {
                try
                {
                    block = BlockPool.GetBlock();
                    block.Number = inputStream.ReadInt32();
                    int blockSize = inputStream.ReadInt32();
                    block.Data = inputStream.ReadBytes(blockSize);

                    //if (block.Data.Length != blockSize) throw new Exception("data block length doesn't match");
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else {
                block = null;
                return false;
            }
        }
    }
}
