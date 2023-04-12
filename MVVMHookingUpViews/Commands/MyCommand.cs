using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMHookingUpViews.Commands
{
    class MyICommand : ICommand
    {
        public readonly Predicate<object> canExecute;
        public readonly Action<object> execute;
        public MyICommand()
        { }
        public MyICommand(Action<object> _execute)
        {
            execute = _execute;
        }
        public MyICommand(Predicate<object> _canexecute, Action<object> _execute)
       : this()
        {
            canExecute = _canexecute;
            execute = _execute;
        }
        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            else
            {
                return canExecute(parameter);
            }
        }

#pragma warning disable CS0067 // The event 'MyICommand.CanExecuteChanged' is never used
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // The event 'MyICommand.CanExecuteChanged' is never used
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
