using System;

namespace Prism.Windows.Navigation
{
    public interface IPageRegistry
    {
        void Register(string key, (Type View, Type ViewModel) info);
        bool TryGetRegistration(Type view, out (string Key, Type View, Type ViewModel) info);
        bool TryGetRegistration(string key, out (string Key, Type View, Type ViewModel) info);
    }
}