using System;

namespace Template10.Popups
{
    public class Command : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action _command;

        public Command(Action command)
        {
            _command = command;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _command();
        }
    }
}