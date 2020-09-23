using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace GZipTest
{
    public class ZipWorker
    {
        private const int DEFAULT_THREAD_COUNT = 1;
        private readonly int threadsCount;

        private IReader <Block> dataReader;
        private Transformer<Block, Block> transformer;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;


        public ZipWorker(CompressionMode compressionMode, IReader<Block> dataReader, IWriter<Block> dataWriter, int threadsCount)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            this.dataReader = dataReader;
            this.threadsCount = threadsCount <= 0 ? DEFAULT_THREAD_COUNT : threadsCount;


            transformer = new Transformer<Block, Block>(compressionMode, dataWriter, cancellationToken, this.threadsCount);
        }
        public int RunZipWorker()
        {
            try
            {
                while (true)
                {
                    if (transformer.QueueCount > threadsCount) continue;
                    if (!dataReader.ReadData(out Block dataBlock))
                    {
                        transformer.CompleteAdding();
                        break;
                    }
                    transformer.Push(dataBlock);
                }
            }catch (Exception e)
            {
                cancellationTokenSource.Cancel();
                throw e;
            }
            return 0;
        }
    }
}
