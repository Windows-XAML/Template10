namespace Template10.Common
{
    public class ValueCancelEventArgs<T> : ValueEventArgs<T>
    {
        public ValueCancelEventArgs(T value)
            : base(value)
        {
        }

        public bool Cancel { get; set; }
    }
}