using System;
using System.Collections.Generic;

namespace Template10.Navigation
{
    public interface IPageKeyRegistry
    {
        void Add(string key, Type type);
    }

    public class PageKeyRegistry : Dictionary<string, Type>, IPageKeyRegistry { }
}

