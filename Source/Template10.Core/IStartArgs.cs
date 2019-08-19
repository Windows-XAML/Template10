namespace Template10
{
    public interface IStartArgs
    {
        object Arguments { get; }
        StartCauses StartCause { get; }
        StartKinds StartKind { get; }
    }
}
