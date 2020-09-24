using System;

namespace GZipTest
{
    public class Compressor : IWorker<Block, Block>
    {
        /// <summary>
        /// compressing data with GZip
        /// </summary>
        /// <param name="dataBlock">original data</param>
        /// <returns>compressed data</returns>
        public Block Work(Block dataBlock)
        {
            if (dataBlock == null) throw new ArgumentNullException("input data is null");
            dataBlock.Data = GZip.Compress(dataBlock.Data);
            return dataBlock;
        }
    }
}
