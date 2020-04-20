# GZipTest
is a command line tool for block-by-block compressing and decompressing of files using class System.IO.Compression.GzipStream.
Application effectively parallel and synchronize blocks processing in multicore environment and able to process files, that are larger than available RAM size.

On success program return 0, otherwise 1.

## Use the following command line arguments:
* compressing: GZipTest.exe compress [original file name] [archive file name]
* decompressing: GZipTest.exe decompress [archive file name] [decompressing file name]

Additional command line arguments:
* GZipTest.exe compress [original file name] [archive file name] [block size] [thread count];
Default block size is 1048576 B, default thread count equals processor count

## Implemented strategies: 
*  DataRead : IReader - reads blocks from the input stream.
*  ZipRead : IReader- reads blocks from the input stream in *MyFileType* format;
*  Compressor / Decompressor : IWorker - compresses/decompresses the data block;
*  DataWriter : IWriter-  writes processed blocks to the output stream in the correct order;
*  ZipWriter : IWriter- writes compressed blocks to the output stream in *MyFileType* format.
*  Transformer - creates threads to process blocks from inputQueue and pass them to writer
*  Gzip - process byte array by compressing / decompressing with GZipStream 
*  ZipWorker - creates instances of reader, writer and transformer. Read blocks from reader and passing them to transformer




