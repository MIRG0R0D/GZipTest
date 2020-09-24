using System.Collections.Generic;

namespace GZipTest
{
    public class ThreadSafeCollection <T>
    {
        private Queue<T> queue = new Queue<T>();
        private object locker = new object();

        /// <summary>
        /// Count of collection elements
        /// </summary>
        public int Count { get { return queue.Count; } }

        /// <summary>
        /// add element to the collection
        /// </summary>
        /// <param name="block"></param>
        public void Add(T block)
        {
            lock (locker)
            {
                queue.Enqueue(block);
            }
        }

        /// <summary>
        /// Try to take element from the collection
        /// </summary>
        /// <param name="block">returned element</param>
        /// <returns>true if success</returns>
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
                    block = default;
                    return false;
                }
            }
        }


    }
}
