using System;

namespace Prism.Windows.Navigation
{
    public interface IPageRegistry
    {
        void Register(string pageKey, Type pageType, Type viewModelType);
        bool TryGetInfo(string pageKey, out (string Key, Type PageType, Type ViewModelType) info);
        bool TryGetInfo(Type pageType, out (string Key, Type PageType, Type ViewModelType) info);
    }
}