using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Prism.Navigation
{
    public interface INavigationParametersInteral
    {
        void AddInternalParameter(string key, object value);
    }

    public interface INavigationParameters
    {
        Uri NavigationUri { get; }
        NavigationMode NavigationMode { get; }

        void Add(string key, object value);
        IEnumerable<string> Keys { get; }
        bool ContainsKey(string key);
        T GetValue<T>(string key);
        IEnumerable<T> GetValues<T>(string key);
        bool TryGetValue<T>(string key, out T value);
        int Count { get; }

        object this[string key] { get; }
    }
}