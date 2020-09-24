using System;

namespace GZipTest
{
    public class Decompressor : IWorker <Block, Block>
    {
        /// <summary>
        /// decompressing data with GZip
        /// </summary>
        /// <param name="dataBlock">Compressed data</param>
        /// <returns>original data</returns>
        public Block Work(Block dataBlock)
        {
            if (dataBlock == null) throw new ArgumentNullException("input data is null");
            dataBlock.Data = GZip.Decompress(dataBlock.Data);
            return dataBlock; 
        }
    }
}
