using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;

namespace Journalx3Piska.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand ShowGradesCommand { get; }

        public ICommand ShowSubjectsCommand { get; }

        public ICommand ExitCommand { get; }


        public MainWindowViewModel()
        {
            ShowGradesCommand = new RelayCommand(ShowGrades);
            ShowSubjectsCommand = new RelayCommand(() =>
            {
                CurrentViewModel = new SubjectsViewModel();
            });
            ExitCommand = new RelayCommand(Exit); // ← добавь
            ShowGrades();
        }

        private void ShowGrades()
        {
            CurrentViewModel = new GradesViewModel();
        }

        private void Exit()
        {
            var loginWindow = new Journalx3Piska.View.LoginView();
            loginWindow.Show();

            // Закрываем MainWindow
            foreach (System.Windows.Window w in System.Windows.Application.Current.Windows)
            {
                if (w is MainWindow)
                {
                    w.Close();
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}