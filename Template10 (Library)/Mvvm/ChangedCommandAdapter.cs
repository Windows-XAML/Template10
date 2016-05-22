using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Template10.Mvvm
{
    /// <summary>
    /// Use this to adapt any <see cref="ICommand"/> to <see cref="IChangedCommand"/> with posibility to manually initiate 
    /// <see cref="CanExecuteChanged"/>
    /// </summary>
    public class ChangedCommandAdapter:IChangedCommand
    {
        private readonly ICommand command;
        private readonly Action raiseCanExecuteChangedAction;

        public ChangedCommandAdapter(ICommand command, Action raiseCanExecuteChangedAction)
        {
            if(command == null)
                throw new ArgumentNullException(nameof(command));
            this.command = command;
            this.raiseCanExecuteChangedAction = raiseCanExecuteChangedAction;
        }

        public bool CanExecute(object parameter)
        {
            return command.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            command.Execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { command.CanExecuteChanged += value; }
            remove { command.CanExecuteChanged -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            raiseCanExecuteChangedAction?.Invoke();
        }
    }
}
