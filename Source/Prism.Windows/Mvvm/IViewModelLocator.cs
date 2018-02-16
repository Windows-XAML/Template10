using System;

namespace Prism.Windows.Mvvm
{
    public interface IViewModelLocator
    {
        Func<string, Type> ViewFactory { get; set; }
        Func<Type, Type> ViewModelFactory { get; set; }
        bool TryUseFactories(string key, out (string Key, Type View, Type ViewModel) info);
    }
}