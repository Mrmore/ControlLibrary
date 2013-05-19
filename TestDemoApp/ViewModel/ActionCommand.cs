using System;
using System.Windows.Input;

namespace TestDemoApp.ViewModel
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> action;

        public ActionCommand(Action action)
        {
            this.action = o => action();
        }

        public ActionCommand(Action<object> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}