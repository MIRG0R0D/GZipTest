using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace GZipTest
{
    public interface IReader<T>
    {
        bool ReadData(out T result);
    }

    public interface IWriter<T>
    {
        void WriteData(T data);
    }

    public interface IWorker<Tinput, Toutput>
    {
        Toutput Work(Tinput inputData);
    }
}
