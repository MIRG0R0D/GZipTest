using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    public class ThreadSafeCollection
    {
        private Queue<Block> queue = new Queue<Block>();
        private static object locker = new object();

        public void Add(Block block)
        {
            lock (locker)
            {
                queue.Enqueue(block);
            }
        }
        public bool TryTake (out Block block)
        {
            lock (locker)
            {
                if (queue.Count > 0)
                {
                    block = queue.Dequeue();
                    return true;
                }
                else
                {
                    block = null;
                    return false;
                }
            }
        }


    }
}
