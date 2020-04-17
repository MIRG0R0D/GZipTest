using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    public class DataWriter : IWriter<Block>
    {

        private Dictionary<int, Block> dictToWrite;
        private BinaryWriter outputStream;
        private static object locker = new object();
        int position;

        public DataWriter(BinaryWriter outputStream)
        {
            if (outputStream == null) throw new ArgumentNullException("Target stream is null");
            this.outputStream = outputStream;
            dictToWrite = new Dictionary<int, Block>();
            position = 0;
        }

        public void WriteData(Block data)
        {
            lock (locker)
            {
                if (data.Number < position || dictToWrite.ContainsKey(data.Number))
                {
                    throw new Exception($"Duplicate key to write {data.Number}");
                }

                dictToWrite.Add(data.Number, data);

                while (dictToWrite.ContainsKey(position))
                {
                    outputStream.Write(dictToWrite[position].Data);
                    dictToWrite.Remove(position++);
                }
            }
        }
    }
}
