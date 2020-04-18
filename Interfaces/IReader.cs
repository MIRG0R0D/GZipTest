namespace GZipTest
{
    public interface IReader<T>
    {
        bool ReadData(out T result);
    }
}
