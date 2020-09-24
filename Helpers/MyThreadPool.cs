using System.Threading;

namespace GZipTest
{
    public class MyThreadPool
    {
        private CancellationToken cancellationToken;
        private CancellationToken completeToken;
        private CancellationTokenSource completeTokenSource;
        private ThreadSafeCollection<Block> inputQueue;
        private IWriter<Block> writer;
        private IWorker<Block, Block> worker;
        private Thread[] threads;

        public MyThreadPool(
            CancellationToken cancellationToken, 
            ThreadSafeCollection<Block> inputQueue, 
            IWriter<Block> writer, 
            IWorker<Block, Block> worker
            )
        {
            this.cancellationToken = cancellationToken;
            this.inputQueue = inputQueue;
            this.writer = writer;
            this.worker = worker;


            completeTokenSource = new CancellationTokenSource();
            completeToken = completeTokenSource.Token;
        }

        public void Complete()
        {
            completeTokenSource.Cancel();
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            return;
        }
        public void InitThreads(int threadsCount)
        {
            threads = new Thread[threadsCount];
            for (int i = 0; i < threadsCount; i++)
            {
                Thread tempThread = new Thread(ThreadFunc);
                tempThread.Name = $"thread_transformer_{i}";
                tempThread.Start();
                threads[i] = tempThread;
            }
        }
        private void ThreadFunc()
        {
            while (true)
            {
                if (inputQueue.TryTake(out Block block))
                {
                    Block processedBlock = worker.Work(block);
                    writer.WriteData(processedBlock);
                }
                else
                {
                    if (completeToken.IsCancellationRequested || cancellationToken.IsCancellationRequested)
                        return;
                    else
                        continue;
                }
            }
        }
    }
}
