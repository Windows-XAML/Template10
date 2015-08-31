using System;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;

namespace Minimal.Services.SecondaryTilesService
{
    public class PinCommand : BindableBase
    {
        bool _isPinned = false;
        public bool IsPinned
        {
            get { return _isPinned; }
            set
            {
                Set(ref _isPinned, value);
                RaisePropertyChanged(nameof(Icon));
                RaisePropertyChanged(nameof(Label));
            }
        }

        public string Icon => (IsPinned) ? "\uE77A" : "\uE840"; 

        public string Label => (IsPinned) ? "Unpin" : "Pin";

        public DelegateCommand Command => new DelegateCommand(ExecuteCommand, CanExecute);
        void ExecuteCommand()
        {
            if (IsPinned)
                IsPinned = !Unpin?.Invoke() ?? true;
            else
                IsPinned = Pin?.Invoke() ?? false;
        }

        public Func<bool> Pin { get; set; }
        public Func<bool> Unpin { get; set; }
        public Func<bool> CanExecute { get; set; } = () => true;
    }
}

