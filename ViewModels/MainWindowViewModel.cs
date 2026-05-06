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

        public MainWindowViewModel()
        {
            ShowGradesCommand = new RelayCommand(ShowGrades);

            ShowSubjectsCommand = new RelayCommand(() =>
            {
                CurrentViewModel = new SubjectsViewModel();
            });

            // стартовый экран
            ShowGrades();
        }

        private void ShowGrades()
        {
            CurrentViewModel = new GradesViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}