namespace GZipTest
{
    public interface IWriter<T>
    {
        void WriteData(T data);
    }
}
