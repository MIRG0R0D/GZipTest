using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace GZipTest
{
    public class ZipWorker
    {
        private IReader <Block> dataReader;
        private Transformer<Block, Block> transformer;
        private IWriter<Block> dataWriter;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        public ZipWorker(CompressionMode compressionMode, BinaryReader sourceStream, BinaryWriter targetStream, int blockSize, int threadCount)
        {
            if (sourceStream == null) throw new ArgumentNullException("Source stream is null");
            if (targetStream == null) throw new ArgumentNullException("Target stream is null");

            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            
            dataReader = ReaderFactory.GetReader(compressionMode, sourceStream, blockSize);
            dataWriter = WriterFactory.GetWriter(compressionMode, targetStream);
            transformer = new Transformer<Block, Block>(compressionMode, dataWriter, cancellationToken, threadCount);
        }
        public int RunZipWorker()
        {
            try
            {
                while (true)
                {
                    if (transformer.QueueCount > transformer.ThreadCount) continue;
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
