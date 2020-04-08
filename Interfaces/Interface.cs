using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace GZipTest
{
    public interface IReader<T>
    {
        T ReadData();
    }

    public interface IWriter<T>
    {
        void WriteData(T data);
        void CompleteAdding();
    }

    public interface IWorker<Tinput, Toutput>
    {
        Toutput Work(Tinput inputData);
    }
}
