﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMHookingUpViews.ViewModel
{
    class DelegateCommand : ICommand
    {
        public readonly Predicate<object> canExecute;
        public readonly Action<object> execute;
        public DelegateCommand()
        { }
        public DelegateCommand(Predicate<object> _canexecute, Action<object> _execute)
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

        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}