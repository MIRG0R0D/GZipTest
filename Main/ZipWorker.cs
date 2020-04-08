using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace GZipTest
{
    public class ZipWorker
    {
        private CompressionMode compressionMode;
        private BinaryReader sourceStream;
        private BinaryWriter targetStream;
        private IReader <Block> dataReader;
        private Transformer<Block, Block> transformer;
        private IWriter<Block> writer;
        public ZipWorker(CompressionMode compressionMode, BinaryReader sourceStream, BinaryWriter targetStream)
        {
            if (sourceStream == null) throw new ArgumentNullException("Source stream is null");
            if (targetStream == null) throw new ArgumentNullException("Target stream is null");
            this.compressionMode = compressionMode;
            this.sourceStream = sourceStream;
            this.targetStream = targetStream;

            //init
            //add dataReader , dataWorker, dataWriter           

            dataReader = new DataRead(sourceStream);//use factory and CompressionMode
            writer = new ZipWriter(targetStream);  //use factory and CompressionMode
            transformer = new Transformer<Block, Block>(compressionMode, writer);
        }
        public int RunZipWorker()
        {
            while (true)//change condition
            {
                Block dataBlock = dataReader.ReadData();
                transformer.Push(dataBlock);
            }

            transformer.CompleteAdding();

            return 0;
        }
    }
}
