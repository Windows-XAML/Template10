using System;

namespace Prism.Windows.Navigation
{
    public interface IPathInfo
    {
        int Index { get; set; }
        string PageKey { get; set; }
        Type PageType { get; }
        NavigationParameters Parameters { get; }
        string QueryString { get; }
        Type ViewModelType { get; }

        string ToString();
    }
}