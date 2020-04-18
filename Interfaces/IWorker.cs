namespace GZipTest
{
    public interface IWorker<Tinput, Toutput>
    {
        Toutput Work(Tinput inputData);
    }
}
