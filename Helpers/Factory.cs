using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    public static class ReaderFactory
    {
        public static IReader<Block> GetReader(CompressionMode compressionMode, BinaryReader source, int blockSize)
        {
            if (compressionMode == CompressionMode.Compress)
                 return new DataRead(source, blockSize);
            else return new ZipRead (source);
        }
    }
    public static class WriterFactory
    {
        public static IWriter<Block> GetWriter(CompressionMode compressionMode, BinaryWriter target)
        {
            if (compressionMode == CompressionMode.Compress)
                 return new ZipWriter (target);
            else return new DataWriter(target);
        }
    }
    public static class WorkerFactory
    {
        public static IWorker <Block, Block> GetWorker(CompressionMode compressionMode)
        {
            if (compressionMode == CompressionMode.Compress)
                 return new Compressor();
            else return new Decompressor();
        }
    }
}
