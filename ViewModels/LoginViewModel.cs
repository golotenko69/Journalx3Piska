using Journalx3Piska.View;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Journalx3Piska.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private readonly SchoolContext _context = new SchoolContext();

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand((Action<object>)ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            string password = parameter as string;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            //string passwordHash = HashPassword(password);

            var user = _context.Users.FirstOrDefault(u =>
                u.Username == Username && u.PasswordHash == password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            Window window = null;

            switch (user.Role)
            {
                case "Директор":
                    window = new MainWindow();
                    break;
                case "Учитель":
                    window = new GradeViewForTeacher();
                    break;
                case "Ученик":
                    window = new GradeViewForStudent();
                    break;
            }

            if (window != null)
            {
                window.Show();
                Application.Current.Windows[0]?.Close();
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    
}