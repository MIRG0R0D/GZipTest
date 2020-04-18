using System;

namespace GZipTest
{
    public class Compressor : IWorker<Block, Block>
    {
        public Block Work(Block inputData)
        {
            if (inputData == null) throw new ArgumentNullException("input data is null");
            return new Block(inputData.Number, GZip.Compress(inputData.Data));
        }
    }
}
