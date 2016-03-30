using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Template10.Common;
using Template10.Mvvm;
using Template10.Utils;
using Windows.Foundation.Collections;

namespace Template10.Validation
{
    public abstract class ModelBase : BindableBase, IModel
    {
        public ModelBase()
        {
            Properties.MapChanged += (s, e) =>
            {
                if (e.CollectionChange.Equals(CollectionChange.ItemInserted))
                    Properties[e.Key].ValueChanged += (sender, args) =>
                    {
                        RaisePropertyChanged(e.Key);
                        RaisePropertyChanged(nameof(IsDirty));
                        RaisePropertyChanged(nameof(IsValid));
                    };
            };
        }

        protected T Read<T>([CallerMemberName]string propertyName = null)
        {
            if (!Properties.ContainsKey(propertyName))
                Properties.Add(propertyName, new Property<T>());
            return (Properties[propertyName] as IProperty<T>).Value;
        }

        protected void Write<T>(T value, [CallerMemberName]string propertyName = null, bool validateAfter = true)
        {
            if (!Properties.ContainsKey(propertyName))
                Properties.Add(propertyName, new Property<T>());
            var property = (Properties[propertyName] as IProperty<T>);
            var previous = property.Value;
            if (!property.IsOriginalSet || !Equals(value, previous))
            {
                property.Value = value;
                if (validateAfter) Validate();
            }
        }

        public bool Validate()
        {
            Properties.Values.ForEach(p => p.Errors.Clear());
            Validator?.Invoke(this);
            Errors.AddRange(Properties.Values.SelectMany(x => x.Errors), true);
            RaisePropertyChanged(nameof(IsValid));
            return IsValid;
        }

        public void Revert()
        {
            Properties.ForEach(x => x.Value.Revert());
            Validate();
        }

        public void MarkAsClean()
        {
            Properties.ForEach(x => x.Value.MarkAsClean());
            Validate();
        }

        public ObservableDictionary<string, IProperty> Properties { get; }
            = new ObservableDictionary<string, IProperty>();

        public ObservableCollection<string> Errors { get; }
            = new ObservableCollection<string>();

        public Action<IModel> Validator { set; get; }

        public bool IsValid => !Errors.Any();

        public bool IsDirty { get; }
    }
}
