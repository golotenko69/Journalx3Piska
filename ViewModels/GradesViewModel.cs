using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Journalx3Piska.ViewModels
{
    // =========================
    // CONVERTERS
    // =========================

    public class EditModeToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Редактирование оценки" : "Выставление оценки";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EditModeToButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Сохранить" : "Добавить";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // =========================
    // VIEWMODEL
    // =========================

    internal class GradesViewModel : INotifyPropertyChanged
    {
        private readonly SchoolContext _context;

        public ObservableCollection<GradeView> Grades { get; set; }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Subject> Subjects { get; set; }
        public ObservableCollection<Subject> SubjectsForAdd { get; set; }

        public List<int> GradesList { get; set; }

        public GradesViewModel()
        {
            _context = new SchoolContext();

            GradesList = new List<int> { 2, 3, 4, 5 };

            OpenGradeDialogCommand = new RelayCommand(OpenAddDialog);
            // В конструкторе вашей ViewModel
            RelayCommand relayCommand = new RelayCommand(() => { IsDialogOpen = false; });
            CloseDialogCommand = relayCommand;

            SaveGradeCommand = new RelayCommand(SaveGrade);
            EditGradeCommand = new RelayCommand(EditGrade);
            DeleteGradeCommand = new RelayCommand(DeleteGrade);

            LoadData();
        }

        // =========================
        // STATE
        // =========================

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get { return _isDialogOpen; }
            set
            {
                _isDialogOpen = value;
                OnPropertyChanged(nameof(IsDialogOpen));
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
            }
        }

        // =========================
        // SELECTED (ВАЖНО!)
        // =========================

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get { return _selectedSubject; }
            set
            {
                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
            }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private int _selectedGrade;
        public int SelectedGrade
        {
            get { return _selectedGrade; }
            set
            {
                _selectedGrade = value;
                OnPropertyChanged(nameof(SelectedGrade));
            }
        }

        private GradeView _selectedGradeItem;
        public GradeView SelectedGradeItem
        {
            get { return _selectedGradeItem; }
            set
            {
                _selectedGradeItem = value;
                OnPropertyChanged(nameof(SelectedGradeItem));
            }
        }

        private Subject _selectedFilterSubject;
        public Subject SelectedFilterSubject
        {
            get { return _selectedFilterSubject; }
            set
            {
                _selectedFilterSubject = value;
                OnPropertyChanged(nameof(SelectedFilterSubject));
                FilterGrades();
            }
        }

        // =========================
        // COMMANDS
        // =========================

        public ICommand OpenGradeDialogCommand { get; }
        public ICommand CloseDialogCommand { get; }
        public ICommand SaveGradeCommand { get; }
        public ICommand EditGradeCommand { get; }
        public ICommand DeleteGradeCommand { get; }

        // =========================
        // LOAD DATA
        // =========================

        private void LoadData()
        {
            Students = new ObservableCollection<Student>(_context.Students.ToList());

            var subjects = _context.Subjects.ToList();

            SubjectsForAdd = new ObservableCollection<Subject>(subjects);

            subjects.Insert(0, new Subject
            {
                SubjectID = 0,
                SubjectName = "Все предметы"
            });

            Subjects = new ObservableCollection<Subject>(subjects);

            RefreshGrades();

            SelectedFilterSubject = Subjects.FirstOrDefault();

            OnPropertyChanged(nameof(Students));
            OnPropertyChanged(nameof(Subjects));
        }

        private void RefreshGrades()
        {
            SelectedGradeItem = null;
            var data = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .ToList();

            var avg = data
                .GroupBy(g => new { g.StudentID, g.SubjectID })
                .ToDictionary(
                    g => g.Key.StudentID + "_" + g.Key.SubjectID,
                    g => Math.Round(g.Average(x => x.GradeValue), 2)
                );

            Grades = new ObservableCollection<GradeView>(
                data.Select(g =>
                {
                    string key = g.StudentID + "_" + g.SubjectID;

                    return new GradeView
                    {
                        GradeID = g.GradeID,
                        StudentName = g.Student != null ? g.Student.FullName : "нет",
                        SubjectName = g.Subject != null ? g.Subject.SubjectName : "нет",
                        Date = g.GradeDate,
                        Value = g.GradeValue,
                        AverageGrade = avg.ContainsKey(key) ? avg[key] : 0
                    };
                })
            );

            OnPropertyChanged(nameof(Grades));
        }

        // =========================
        // ADD
        // =========================

        private void OpenAddDialog()
        {
            IsEditMode = false;

            SelectedGradeItem = null;
            SelectedStudent = null;
            SelectedSubject = null;
            SelectedDate = null;
            SelectedGrade = 0;

            IsDialogOpen = true;
        }

        // =========================
        // SAVE (ADD + EDIT)
        // =========================

        private void SaveGrade()
        {
            if (SelectedStudent == null || SelectedSubject == null || SelectedDate == null)
                return;

            Grade grade;

            if (IsEditMode && SelectedGradeItem != null)
            {
                grade = _context.Grades.FirstOrDefault(g => g.GradeID == SelectedGradeItem.GradeID);
                if (grade == null) return;
            }
            else
            {
                grade = new Grade();
                _context.Grades.Add(grade);
            }

            grade.StudentID = SelectedStudent.StudentID;
            grade.SubjectID = SelectedSubject.SubjectID;
            grade.GradeDate = SelectedDate.Value;
            grade.GradeValue = SelectedGrade;

            // Detach чтобы EF не трогал Student при SaveChanges
            var entry = _context.Entry(SelectedStudent);
            if (entry.State != System.Data.Entity.EntityState.Detached)
                entry.State = System.Data.Entity.EntityState.Detached;

            _context.SaveChanges();

            RefreshGrades();
            IsDialogOpen = false;
        }

        // =========================
        // EDIT (КЛЮЧЕВОЙ МОМЕНТ)
        // =========================

        private void EditGrade()
        {
            if (SelectedGradeItem == null)
                return;

            var grade = _context.Grades
                .FirstOrDefault(g => g.GradeID == SelectedGradeItem.GradeID);

            if (grade == null)
                return;

            SelectedStudent = Students.FirstOrDefault(s => s.StudentID == grade.StudentID);
            SelectedSubject = SubjectsForAdd.FirstOrDefault(s => s.SubjectID == grade.SubjectID);
            SelectedDate = grade.GradeDate;
            SelectedGrade = grade.GradeValue;

            IsEditMode = true;
            IsDialogOpen = true;
        }

        // =========================
        // DELETE
        // =========================

        private void DeleteGrade()
        {
            if (SelectedGradeItem == null)
                return;

            int idToDelete = SelectedGradeItem.GradeID;
            SelectedGradeItem = null;

            var grade = _context.Grades.FirstOrDefault(g => g.GradeID == idToDelete);
            if (grade == null)
                return;

            _context.Grades.Remove(grade);
            _context.SaveChanges();

            RefreshGrades();
            FilterGrades();
        }

        // =========================
        // FILTER
        // =========================

        private void FilterGrades()
        {
            SelectedGradeItem = null;
            var query = _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .AsQueryable();

            if (SelectedFilterSubject != null && SelectedFilterSubject.SubjectID != 0)
            {
                query = query.Where(g => g.SubjectID == SelectedFilterSubject.SubjectID);
            }

            var data = query.ToList();

            var avg = data
                .GroupBy(g => new { g.StudentID, g.SubjectID })
                .ToDictionary(
                    g => g.Key.StudentID + "_" + g.Key.SubjectID,
                    g => Math.Round(g.Average(x => x.GradeValue), 2)
                );

            Grades = new ObservableCollection<GradeView>(
                data.Select(g =>
                {
                    string key = g.StudentID + "_" + g.SubjectID;

                    return new GradeView
                    {
                        GradeID = g.GradeID,
                        StudentName = g.Student != null ? g.Student.FullName : "нет",
                        SubjectName = g.Subject != null ? g.Subject.SubjectName : "нет",
                        Date = g.GradeDate,
                        Value = g.GradeValue,
                        AverageGrade = avg.ContainsKey(key) ? avg[key] : 0
                    };
                })
            );

            OnPropertyChanged(nameof(Grades));
        }

        // =========================
        // INotify
        // =========================

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}