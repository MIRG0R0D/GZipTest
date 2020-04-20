using System.Collections.Generic;

namespace GZipTest
{
    public class ThreadSafeCollection <T>
    {
        private Queue<T> queue = new Queue<T>();
        private object locker = new object();

        public int Count { get { return queue.Count; } }

        public void Add(T block)
        {
            lock (locker)
            {
                queue.Enqueue(block);
            }
        }
        public bool TryTake (out T block)
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
                    block = default(T);
                    return false;
                }
            }
        }


    }
}
