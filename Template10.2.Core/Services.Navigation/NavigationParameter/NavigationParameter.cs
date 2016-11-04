namespace Template10.Services.Navigation
{
    public class NavigationParameter : INavigationParameter
    {
        object _value;
        public NavigationParameter(object value)
        {
            this._value = value;
        }
        public bool HasValue => _value != null;
        public T GetValue<T>() => HasValue ? (T)_value : default(T);
    }
}