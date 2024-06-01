using System;
using System.Windows.Input;

namespace SWE_TourPlanner_WPF.ViewLayer
{
    public class ICommandHandler : ICommand
    {
        private Action _action;
        private Func<bool> _canExecute;

        public ICommandHandler(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
