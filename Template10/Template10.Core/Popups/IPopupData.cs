using System.ComponentModel;

namespace Template10.Popups
{
    public interface IPopupData : INotifyPropertyChanged
    {
        System.Windows.Input.ICommand Close { get; }
        string Text { get; set; }
        bool IsActive { get; set; }
    }
}