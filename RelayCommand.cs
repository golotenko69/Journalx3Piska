using System;
using System.Windows.Input;

namespace Journalx3Piska
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        // Конструктор для методов С параметром: RelayCommand(p => Method(p))
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Конструктор для методов БЕЗ параметров: RelayCommand(() => Method())
        // Мы просто оборачиваем Action в Action<object>, игнорируя входящий параметр
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(p => execute(), p => canExecute == null || canExecute())
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}