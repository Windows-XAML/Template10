using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Template10.Common;
using Template10.Mvvm;

namespace Template10.Validation
{
    public interface IModel : IBindable
    {
        bool Validate();

        void Revert();

        void MarkAsClean();

        ObservableDictionary<string, IProperty> Properties { get; }

        ObservableCollection<string> Errors { get; }

        Action<IModel> Validator { set; get; }

        bool IsValid { get; }

        bool IsDirty { get; }
    }
}
