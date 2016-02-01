using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Template10.Mvvm;

namespace Template10.Validation
{
    public interface IProperty : IBindable
    {
        event EventHandler ValueChanged;

        void Revert();

        void MarkAsClean();

        ObservableCollection<string> Errors { get; }

        bool IsValid { get; }

        bool IsDirty { get; }

        bool IsOriginalSet { get; }
    }

    public interface IProperty<T> : IProperty
    {
        T OriginalValue { get; set; }

        T Value { get; set; }
    }
}
