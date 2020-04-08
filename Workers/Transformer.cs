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
        private IWorker<Block, Block> worker;
        BlockingCollection<Block> inputQueue = new BlockingCollection<Block>();
        //
        private readonly List<Thread> threads = new List<Thread>();
        private int threadsCount = 4;
        private IWriter<Block> writer;

        public Transformer(CompressionMode compressionMode, IWriter<Block> writer)
        {
            switch (compressionMode)
            {
                case CompressionMode.Compress:
                    worker = new Compressor();
                    break;
                case CompressionMode.Decompress:
                    worker = new Decompressor();
                    break;
                default: throw new ArgumentException("unknown compression mode");
            }
            this.writer = writer;
            Init();
        }

        public void Push(Block data)
        {
            inputQueue.Add(data);
        }
        public void CompleteAdding()
        {
            inputQueue.CompleteAdding();
        }

        public void ConnectWriter(IWriter<Block> writer)
        {

        }


        private void Init()
        {
            for(int i = 0; i < threadsCount; i++)
            {
                Thread myThread = new Thread(threadFunc);
                myThread.Name = $"thread_{i}";
                threads.Add(myThread);                
                myThread.Start();
            }
        }

        private void threadFinished()
        {
            if (threads.All(x => !x.IsAlive))
            {
                writer.CompleteAdding();
            }
        }

        private  void threadFunc()
        {
            Console.WriteLine($"Worker {Thread.CurrentThread.Name} is starting.");

            foreach (Block workItem in inputQueue.GetConsumingEnumerable())
            {
                Console.WriteLine($"Worker {Thread.CurrentThread.Name} is processing item { workItem.Number}");
                Block processedBlock = worker.Work(workItem);
                writer.WriteData(processedBlock);
            }
            Console.WriteLine($"Worker {Thread.CurrentThread.Name} is stopping.");
        }




    }
}
