using System;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;

namespace Minimal.Services.TileService
{
    public class PinCommand : BindableBase
    {
        private string _Icon;
        public string Icon { get { return _Icon; } private set { Set(ref _Icon, value); } }

        private string _Label;
        public string Label { get { return _Label; } private set { Set(ref _Label, value); } }

        private Func<Task<bool>> _IsPinned;
        public Func<Task<bool>> IsPinned { get; set; }

        public Func<Task<bool>> Unpin { get; set; }
        public Func<Task<bool>> Pin { get; set; }

        public DelegateCommand Command => new DelegateCommand(ExecuteCommand);
        async void ExecuteCommand()
        {
            var value = await IsPinned();
            if (value)
                await Unpin();
            else
                await Pin();
            value = await IsPinned();
            Icon = value ? "\uE77A" : "\uE840";
            Label = value ? "Unpin" : "Pin";
        }
    }
}

