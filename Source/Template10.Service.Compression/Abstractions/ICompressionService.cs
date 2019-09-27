namespace Template10.Service.Compression
{
    public interface ICompressionService
    {
        string Unzip(string value, CompressionMethods method = CompressionMethods.gzip);
        string Zip(string value, CompressionMethods method = CompressionMethods.gzip);
    }
}
