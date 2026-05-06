using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.Entity;

namespace Journalx3Piska.ViewModels
{
    internal class GradesViewModel : INotifyPropertyChanged
    {
        private SchoolContext _context;

        public ObservableCollection<GradeView> Grades { get; set; }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Subject> Subjects { get; set; }

        public List<int> GradesList { get; set; } = new List<int> { 2, 3, 4, 5 };

        public Student SelectedStudent { get; set; }
        public Subject SelectedSubject { get; set; }
        public DateTime? SelectedDate { get; set; }
        public int SelectedGrade { get; set; }

        public ICommand AddGradeCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private Subject _selectedFilterSubject;
        public Subject SelectedFilterSubject
        {
            get => _selectedFilterSubject;
            set
            {
                _selectedFilterSubject = value;
                OnPropertyChanged(nameof(SelectedFilterSubject));
                FilterGrades();
            }
        }

        public ICommand FilterCommand { get; }

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

        public ICommand OpenGradeDialogCommand { get; }
        public ICommand CloseDialogCommand { get; }

        public GradesViewModel()
        {
            _context = new SchoolContext();

            LoadData();

            OpenGradeDialogCommand = new RelayCommand(() => IsDialogOpen = true);
            CloseDialogCommand = new RelayCommand(() => IsDialogOpen = false);
            AddGradeCommand = new RelayCommand(AddGrade);
            FilterCommand = new RelayCommand(FilterGrades);
        }

        private void LoadData()
        {
            Students = new ObservableCollection<Student>(_context.Students.ToList());

            var subjectsFromDb = _context.Subjects.ToList();

            subjectsFromDb.Insert(0, new Subject
            {
                SubjectID = 0,
                SubjectName = "Все предметы"
            });

            Subjects = new ObservableCollection<Subject>(subjectsFromDb);

            var gradesFromDb = _context.Grades
                .Include(g => g.Subject)
                .Include(g => g.Student)
                .ToList();

            var avgDict = gradesFromDb
                .GroupBy(g => g.StudentID)
                .ToDictionary(
                    g => g.Key,
                    g => Math.Round(g.Average(x => x.GradeValue), 2)
                );

            SelectedFilterSubject = Subjects.FirstOrDefault();

            Grades = new ObservableCollection<GradeView>(
                gradesFromDb.Select(g => new GradeView
                {
                    StudentName = g.Student != null ? g.Student.FullName : "нет",
                    SubjectName = g.Subject != null ? g.Subject.SubjectName : "нет",
                    Date = g.GradeDate,
                    Value = g.GradeValue,
                    AverageGrade = avgDict.ContainsKey(g.StudentID)
                        ? avgDict[g.StudentID]
                        : 0
                })
            );

            OnPropertyChanged(nameof(Students));
            OnPropertyChanged(nameof(Subjects));
            OnPropertyChanged(nameof(Grades));
        }

        private void AddGrade()
        {
            if (SelectedStudent == null || SelectedSubject == null || SelectedDate == null)
                return;

            var grade = new Grade
            {
                StudentID = SelectedStudent.StudentID,
                SubjectID = SelectedSubject.SubjectID,
                GradeDate = SelectedDate.Value,
                GradeValue = SelectedGrade
            };

            _context.Grades.Add(grade);
            _context.SaveChanges();

            LoadData();

            IsDialogOpen = false;
        }

        private void FilterGrades()
        {
            var query = _context.Grades
                .Include(g => g.Subject)
                .Include(g => g.Student)
                .AsQueryable();

            if (SelectedFilterSubject != null && SelectedFilterSubject.SubjectID != 0)
            {
                query = query.Where(g => g.SubjectID == SelectedFilterSubject.SubjectID);
            }

            var gradesFromDb = query.ToList();

            var avgDict = gradesFromDb
                .GroupBy(g => new { g.StudentID, g.SubjectID })
                .ToDictionary(
                    g => $"{g.Key.StudentID}_{g.Key.SubjectID}",
                    g => Math.Round(g.Average(x => x.GradeValue), 2)
                );

            Grades = new ObservableCollection<GradeView>(
                gradesFromDb.Select(g =>
                {
                    var key = $"{g.StudentID}_{g.SubjectID}";

                    return new GradeView
                    {
                        StudentName = g.Student != null ? g.Student.FullName : "нет",
                        SubjectName = g.Subject != null ? g.Subject.SubjectName : "нет",
                        Date = g.GradeDate,
                        Value = g.GradeValue,
                        AverageGrade = avgDict.ContainsKey(key)
                            ? avgDict[key]
                            : 0
                    };
                })
            );

            OnPropertyChanged(nameof(Grades));
        }
    }
}
