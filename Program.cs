
using System;
using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    class Program
    {
        private static string sourceFile, targetFile;
        private static CompressionMode compressionMode;
        public static int Main(string[] args)
        {
#if DEBUG
            args = new string[] { CompressionMode.Compress.ToString(), "D://testPDF.pdf", "D://testUncompressed.mft" };//MyFileType
#endif
            try
            {
                checkInputs(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }

            try
            {
                using (Stream inputStream = new FileStream(sourceFile, FileMode.Open))
                using (Stream outputStream = new FileStream(targetFile, FileMode.Create))
                {
                    ZipWorker zipWorker = new ZipWorker(compressionMode, new BinaryReader(inputStream), new BinaryWriter(outputStream));
                    return zipWorker.RunZipWorker();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
        }

        private static void checkInputs(string[] args)
        {
            if (args.Length != 3)
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
        }
    }
}