using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest
{
    public class ZipRead : IReader<Block>
    {

        private BinaryReader inputStream;

        public ZipRead(BinaryReader inputStream)
        {
            this.inputStream = inputStream;
        }

        public bool ReadData(out Block block)
        {
            if (inputStream.BaseStream.Position != inputStream.BaseStream.Length)
            {
                try
                {
                    int blockNumber = inputStream.ReadInt32();
                    int blockSize = inputStream.ReadInt32();
                    byte[] buffer = inputStream.ReadBytes(blockSize);
                    //todo compare blockSize vs buffer length
                    block = new Block(blockNumber, buffer);
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else {
                block = null;
                return false;
            }
        }
    }
}
