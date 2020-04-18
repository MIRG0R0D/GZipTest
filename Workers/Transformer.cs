using System.IO.Compression;
using System.Threading;

namespace GZipTest
{
    public class Transformer<TInput, TOutput>
    {
        private readonly int DEFAULT_THREAD_COUNT = 4;

        private IWorker<Block, Block> worker;
        private readonly int threadsCount;
        ThreadSafeCollection<Block> inputQueue = new ThreadSafeCollection<Block>();
        private Thread[] threads ;
        private IWriter<Block> writer;
        private CancellationToken cancellationToken;
        private CancellationTokenSource completeToken;

        public Transformer(CompressionMode compressionMode, IWriter<Block> writer, CancellationToken cancellationToken, int threadsCount)
        {
            worker = WorkerFactory.GetWorker(compressionMode);
            completeToken = new CancellationTokenSource();            
            this.writer = writer;
            this.cancellationToken = cancellationToken;
            this.threadsCount = threadsCount <= 0 ? DEFAULT_THREAD_COUNT : threadsCount;

            InitThreads();
        }

        public void Push(Block data)
        {
            inputQueue.Add(data);
        }
        public void CompleteAdding()
        {
            completeToken.Cancel();
            foreach(Thread thread in threads)
            {
                thread.Join();
            }
            return;
        }

        private void InitThreads()
        {
            threads = new Thread[threadsCount];
            for(int i = 0; i < threadsCount; i++)
            {
                Thread tempThread = new Thread(threadFunc);
                tempThread.Name = $"thread_transformer_{i}";
                tempThread.Start();
                threads[i] = tempThread;
            }
        }

        private  void threadFunc()
        {
            //Console.WriteLine($"Worker {Thread.CurrentThread.Name} is starting.");
            while (true)
            {
                if (inputQueue.TryTake(out Block block))
                {
                    //Console.WriteLine($"Worker {Thread.CurrentThread.Name} is processing item { block.Number}");
                    Block processedBlock = worker.Work(block);
                    writer.WriteData(processedBlock);
                }
                else
                {
                    if (completeToken.IsCancellationRequested || cancellationToken.IsCancellationRequested)
                    {
                        //Console.WriteLine($"Worker {Thread.CurrentThread.Name} is stopping.");
                        return;
                    }
                    continue;
                }
            }
        }
    }
}
