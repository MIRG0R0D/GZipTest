using System;

namespace GZipTest
{
    public class Decompressor : IWorker <Block, Block>
    {
        public Block Work(Block inputData)
        {
            if (inputData == null) throw new ArgumentNullException("input data is null");
            return new Block(inputData.Number, GZip.Decompress(inputData.Data));
        }
    }
}
