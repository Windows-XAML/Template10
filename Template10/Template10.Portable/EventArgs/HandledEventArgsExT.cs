namespace Template10.Common
{
    public class HandledEventArgsEx<T> : HandledEventArgsEx
    {
        public HandledEventArgsEx(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}
