using System;
using System.Linq;
using System.Windows.Input;

namespace Journalx3Piska.ViewModels
{
    public class LoginViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly SchoolContext _context;

        public event Action LoginSuccess;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _context = new SchoolContext();
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            // Пока просто проверка что поля не пустые
            // Потом можно подключить реальную проверку по БД
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Введите логин";
                return;
            }

            // Успешный вход
            LoginSuccess?.Invoke();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
    }
}