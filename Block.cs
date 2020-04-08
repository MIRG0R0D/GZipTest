using System;
using System.Collections.Generic;
using System.Text;

namespace GZipTest
{
    public class Block
    {
        public int Number;
        public byte[] Data;

        public Block(int number, byte[] data)
        {
            Number = number;
            Data = data;
        }
    }
}
