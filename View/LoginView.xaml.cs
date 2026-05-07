using System.Windows;
using Journalx3Piska.ViewModels;

namespace Journalx3Piska.View
{
    public partial class LoginView : Window 
    {
        public LoginView()
        {
            InitializeComponent();
            var vm = new LoginViewModel();
            
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                // Мы берем пароль напрямую из PasswordBox и передаем его в ExecuteLogin
                string password = PasswordBox.Password;
                viewModel.LoginCommand.Execute(password);
            }
        }
    }
}