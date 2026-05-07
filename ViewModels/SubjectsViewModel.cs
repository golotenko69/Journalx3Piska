using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;


namespace Journalx3Piska.ViewModels
{
    public class SubjectsViewModel : INotifyPropertyChanged
    {
        private SchoolContext _context = new SchoolContext();


        public ObservableCollection<Subject> Subjects { get; set; }

        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
            }
        }
        public ObservableCollection<Teacher> Teachers { get; set; }

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher

        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                OnPropertyChanged(nameof(SelectedTeacher));
            }
        }
        public ICommand OpenDialogCommand { get; }
        public ICommand CloseDialogCommand { get; }

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set
            {
                _isDialogOpen = value;
                OnPropertyChanged(nameof(IsDialogOpen));
            }
        }

        private string _newSubjectName;

        private string _newTeacherName;
        public string NewTeacherName
        {
            get => _newTeacherName;
            set
            {
                _newTeacherName = value;
                OnPropertyChanged(nameof(NewTeacherName));
            }
        }

        private Subject _selectedSubjectForAdd;
        public Subject SelectedSubjectForAdd
        {
            get => _selectedSubjectForAdd;
            set
            {
                _selectedSubjectForAdd = value;

                // если выбрали из списка — подставляем имя
                if (value != null)
                    NewSubjectName = value.SubjectName;

                OnPropertyChanged(nameof(SelectedSubjectForAdd));
            }
        }
        public string NewSubjectName
        {
            get => _newSubjectName;
            set
            {
                _newSubjectName = value;
                OnPropertyChanged(nameof(NewSubjectName));
            }
        }

        public ICommand AddSubjectCommand { get; }
        public ICommand DeleteSubjectCommand { get; }

        public SubjectsViewModel()
        {
            LoadData();

            OpenDialogCommand = new RelayCommand(() => IsDialogOpen = true);
            CloseDialogCommand = new RelayCommand(() => IsDialogOpen = false);

            AddSubjectCommand = new RelayCommand(AddSubject); // 🔥 ВОТ ЭТО ТЫ ЗАБЫЛ
            DeleteSubjectCommand = new RelayCommand(DeleteSubject);
        }

        private void LoadData()
        {
            var list = _context.Subjects
                               .Include(s => s.Teacher)
                               .ToList();

            MessageBox.Show($"Найдено: {list.Count}");

            Subjects = new ObservableCollection<Subject>(list);
            OnPropertyChanged(nameof(Subjects));

            Teachers = new ObservableCollection<Teacher>(_context.Teachers.ToList());
            OnPropertyChanged(nameof(Teachers));
        }



        private void AddSubject()
        {
            if (string.IsNullOrWhiteSpace(NewSubjectName))
                return;

            var subjectName = NewSubjectName.Trim();

            // проверка дубля
            var existingSubject = _context.Subjects
                .FirstOrDefault(s => s.SubjectName == subjectName);

            if (existingSubject != null)
            {
                MessageBox.Show("Такой предмет уже существует");
                return;
            }

            // --- TEACHER ---
            Teacher teacher = SelectedTeacher;

            if (teacher == null && !string.IsNullOrWhiteSpace(NewTeacherName))
            {
                var teacherName = NewTeacherName.Trim();

                teacher = _context.Teachers
                    .FirstOrDefault(t => t.FullName == teacherName);

                if (teacher == null)
                {
                    teacher = new Teacher
                    {
                        FullName = teacherName,
                        UserID = 1
                    };

                    _context.Teachers.Add(teacher);
                    _context.SaveChanges();
                }
            }

            if (teacher == null)
                return;

            // --- SUBJECT ---
            var subject = new Subject
            {
                SubjectName = subjectName,
                TeacherID = teacher.TeacherID
            };

            _context.Subjects.Add(subject);
            _context.SaveChanges();

            // очистка
            NewSubjectName = "";
            NewTeacherName = "";
            SelectedTeacher = null;
            SelectedSubjectForAdd = null;

            LoadData();
            IsDialogOpen = false;

            OnPropertyChanged(nameof(NewSubjectName));
            OnPropertyChanged(nameof(NewTeacherName));
            OnPropertyChanged(nameof(SelectedTeacher));
            OnPropertyChanged(nameof(SelectedSubjectForAdd));
        }



        private void DeleteSubject()
        {
            if (SelectedSubject == null)
                return;

            var result = MessageBox.Show(
                $"Удалить предмет \"{SelectedSubject.SubjectName}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            _context.Subjects.Remove(SelectedSubject);
            _context.SaveChanges();

            LoadData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

