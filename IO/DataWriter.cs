﻿using System;
using System.Collections.Generic;
using System.IO;

namespace GZipTest
{
    public class DataWriter : IWriter<Block>
    {

        private Dictionary<int, byte[]> dictToWrite; 
        private BinaryWriter outputStream;
        private static object locker = new object();
        private int position;

        public DataWriter(BinaryWriter outputStream)
        {
            if (outputStream == null) throw new ArgumentNullException("Target stream is null");
            this.outputStream = outputStream;
            dictToWrite = new Dictionary<int, byte[]>();
            position = 0;
        }

        /// <summary>
        /// writing decompressed data to file
        /// </summary>
        /// <param name="data">decompressed data</param>
        public void WriteData(Block data)
        {
            lock (locker)
            {
                if (data.Number < position || dictToWrite.ContainsKey(data.Number))
                {
                    throw new Exception($"Duplicate key to write {data.Number}");
                }

                dictToWrite.Add(data.Number, data.Data);

                while (dictToWrite.ContainsKey(position))
                {
                    outputStream.Write(dictToWrite[position]);
                    dictToWrite.Remove(position++);
                }
                BlockPool.ReturnObject(data);
            }
        }
    }
}
