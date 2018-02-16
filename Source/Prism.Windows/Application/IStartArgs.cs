namespace Prism.Windows
{
    public interface IStartArgs
    {
        object Arguments { get; }
        StartCauses StartCause { get; }
    }
}
