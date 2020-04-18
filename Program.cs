using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    class Program
    {

        private const int DEFAULT_BLOCK_SIZE = 5242880; //5 MB

        private static string sourceFile, targetFile;
        private static int threadCount, blockSize;
        private static CompressionMode compressionMode;
        public static int Main(string[] args)
        {
#if DEBUG
            //compression
            args = new string[] { CompressionMode.Compress.ToString(), "D://testHuge.pdf", "D://testCompressed.mft"};//mft = MyFileType
            //decompression
            //args = new string[] { CompressionMode.Decompress.ToString(), "D://testCompressed.mft" , "D://testDecompress.pdf"};
#endif
            try
            {
                blockSize = DEFAULT_BLOCK_SIZE;
                threadCount = Environment.ProcessorCount;
                checkInputs(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                using (Stream inputStream = new FileStream(sourceFile, FileMode.Open))
                using (Stream outputStream = new FileStream(targetFile, FileMode.Create))
                {
                    ZipWorker zipWorker = new ZipWorker(compressionMode, new BinaryReader(inputStream), new BinaryWriter(outputStream), blockSize, threadCount);
                    return zipWorker.RunZipWorker();
                }
            }
            catch (OutOfMemoryException outMemory)
            {
                Console.WriteLine($"OutOfMemoryException: try to reduce block size (the 4-th parameter) or thread count (the 5-th).");
                Console.WriteLine($"Current block size is:   {blockSize}");
                Console.WriteLine($"Current thread count is: {threadCount}");
                Console.WriteLine(outMemory.ToString());
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
            finally
            {
                stopwatch.Stop();
#if DEBUG
                Console.WriteLine($"Done in {stopwatch.Elapsed} seconds;");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
#endif
            }
        }

        private static void checkInputs(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("Invalid Arguments count");
            }

            try
            {
                compressionMode = (CompressionMode)Enum.Parse(typeof(CompressionMode), args[0]);
            }
            catch (Exception)
            {
                throw new ArgumentException($"Invalid compression type {args[0]}");
            }

            if (File.Exists(args[1]))
                sourceFile = args[1];
            else
                throw new ArgumentException($"Input file does not exist {args[1]}");

            try
            {
                if (Directory.Exists(Path.GetDirectoryName(args[2])))
                    targetFile = args[2];
                else
                    throw new Exception();
            }
            catch
            {
                throw new ArgumentException($"Invalid output file name {args[2]}");
            }

            if (args.Length >= 4)
            {
                if (int.TryParse(args[3], out int block) && block > 0)
                {
                    blockSize = block;
                }
                else throw new ArgumentException($"Invalid block size {args[3]}");
            }
            if (args.Length >= 5)
            {
                if (int.TryParse(args[4], out int threadC) && threadC> 0)
                {
                    threadCount = threadC;
                }
                else throw new ArgumentException($"Invalid thread count {args[4]}");
            }
        }
    }
}