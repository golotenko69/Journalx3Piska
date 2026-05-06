using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Journalx3Piska.ViewModels
{
    public class SubjectsViewModel : INotifyPropertyChanged
    {
        private SchoolContext _context;

        public ObservableCollection<Subject> Subjects { get; set; }

        public ICommand RefreshCommand { get; }

        public SubjectsViewModel()
        {
            _context = new SchoolContext();

            RefreshCommand = new RelayCommand(LoadSubjects);

            LoadSubjects();
        }

        private void LoadSubjects()
        {
            Subjects = new ObservableCollection<Subject>(
                _context.Subjects.ToList()
            );

            OnPropertyChanged(nameof(Subjects));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

