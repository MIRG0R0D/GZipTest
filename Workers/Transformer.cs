using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;

namespace GZipTest
{
    public class Transformer<TInput, TOutput>
    {
        private readonly int DEFAULT_THREAD_COUNT = 4;

        private IWorker<Block, Block> worker;
        private readonly int threadsCount;
        ThreadSafeCollection inputQueue = new ThreadSafeCollection();
        private Thread[] threads ;
        private IWriter<Block> writer;
        private CancellationToken cancellationToken;
        private CancellationTokenSource completeToken;

        public Transformer(CompressionMode compressionMode, IWriter<Block> writer, CancellationToken cancellationToken, int threadsCount)
        {
            worker = WorkerFactory.GetWorker(compressionMode);
            this.writer = writer;
            this.cancellationToken = cancellationToken;
            this.completeToken = new CancellationTokenSource();
            
            this.threadsCount = threadsCount <= 0 ? DEFAULT_THREAD_COUNT : threadsCount;

            Init();
        }

        public void Push(Block data)
        {
            inputQueue.Add(data);
        }
        public void CompleteAdding()
        {
            completeToken.Cancel();
            while (true)
            {
                if (!threads.Any(x => x.IsAlive))
                {
                    return;
                }
            }
        }

        private void Init()
        {
            List<Thread> threadsList = new List<Thread>();
            for(int i = 0; i < threadsCount; i++)
            {
                Thread myThread = new Thread(threadFunc);
                myThread.Name = $"thread_transformer_{i}";
                threadsList.Add(myThread);                
                myThread.Start();
            }
            threads = threadsList.ToArray();
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
