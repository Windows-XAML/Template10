using System;

namespace Prism.Windows.Mvvm
{
    public interface IMvvmLocator
    {
        Func<string, Type> FindView { get; set; }
        Func<Type, Type> FindViewModel { get; set; }
    }
}
