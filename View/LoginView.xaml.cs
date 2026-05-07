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
            vm.LoginSuccess += () =>
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            };
            DataContext = vm;
        }
    }
}