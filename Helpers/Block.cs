﻿using System.Collections.Generic;

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
        public override bool Equals(object obj)
        {
            return obj is Block block &&
                   Number == block.Number &&
                   EqualityComparer<byte[]>.Default.Equals(Data, block.Data);
        }

        public override int GetHashCode()
        {
            int hashCode = 1249563269;
            hashCode = hashCode * -1521134295 + Number.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(Data);
            return hashCode;
        }
    }
}
