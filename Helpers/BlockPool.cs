namespace GZipTest
{
    public static class BlockPool
    {
        private static ThreadSafeCollection<Block> blocks = new ThreadSafeCollection<Block>();

        public static int Created { get; private set; }
        public static int Returned { get; private set; }
        public static int Reusing { get; private set; }

        /// <summary>
        /// getting block from pool
        /// </summary>
        /// <returns>block ready to use</returns>
        public static Block GetBlock()
        {
            if (blocks.TryTake(out Block block))
            {
                Reusing++;
                return block;
            }
            else
            {
                Created++;
                return new Block();
            }
        }

        /// <summary>
        /// returning useless block to pool
        /// </summary>
        /// <param name="block">block to reuse</param>
        public static void ReturnObject(Block block)
        {
            Returned++;
            block.Reset();
            blocks.Add(block);
        }



    }
}
