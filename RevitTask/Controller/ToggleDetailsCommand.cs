using System;
using System.Windows.Input;

namespace RevitTask.Controller
{
    internal class ToggleDetailsCommand: ICommand
    {
        private readonly Action executeAction;

        public ToggleDetailsCommand(Action executeAction)
        {
            this.executeAction = executeAction;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            executeAction?.Invoke();
        }
    }
}
