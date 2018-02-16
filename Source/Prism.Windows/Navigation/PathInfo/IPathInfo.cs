using System;

namespace Prism.Windows.Navigation
{
    public interface IPathInfo
    {
        int Index { get; }
        string Key { get; }
        Type View { get; }
        NavigationParameters Parameters { get; }
        string QueryString { get; }
        Type ViewModel { get; }

        string ToString();
    }
}