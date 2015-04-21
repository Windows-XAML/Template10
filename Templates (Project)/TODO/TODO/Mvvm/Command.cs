using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm
{
    // command is an implementation ICommand for buttons
    public class Command
        : GalaSoft.MvvmLight.Command.RelayCommand
    {
        public Command(Action execute) : base(execute) { }
        public Command(Action execute, Func<bool> canExecute) : base(execute, canExecute) { }
    }

    // command<T> is an implementation of ICommand with parameters for buttons
    public class Command<T>
        : GalaSoft.MvvmLight.Command.RelayCommand<T>
    {
        public Command(Action<T> execute) : base(execute) { }
        public Command(Action<T> execute, Func<T, bool> canExecute) : base(execute, canExecute) { }
    }
}

