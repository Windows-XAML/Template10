using System;
using System.Collections.Generic;

namespace Template10.Common
{
    public class ServicesManager<T>
    {
        public ServicesManager(Func<ServicesManager<T>, T> current) => Current = () => current(this);

        List<T> _items = new List<T>();

        public T[] Instances => _items.ToArray();

        public void Add(T item) => _items.Add(item);

        public void Remove(T item) => _items.Remove(item);

        public void Clear() => _items.Clear();

        public Func<T> Current { get; private set; } = () => default(T);
    }
}
