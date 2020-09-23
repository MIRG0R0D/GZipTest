using System.IO.Compression;
using System.Threading;

namespace GZipTest
{
    public class Transformer<TInput, TOutput>
    {
        private MyThreadPool threadPool;

        private IWorker<Block, Block> worker;
        private ThreadSafeCollection<Block> inputQueue = new ThreadSafeCollection<Block>();

        public int QueueCount { get { return inputQueue.Count; } }

        public Transformer(CompressionMode compressionMode, IWriter<Block> writer, CancellationToken cancellationToken, int threadsCount)
        {
            worker = WorkerFactory.GetWorker(compressionMode);
            threadPool = new MyThreadPool(cancellationToken, inputQueue, writer, worker);
            threadPool.InitThreads(threadsCount);
        }

        public void Push(Block data)
        {
            inputQueue.Add(data);
        }
        public void CompleteAdding()
        {
            threadPool.Complete();
        }
    }
}
