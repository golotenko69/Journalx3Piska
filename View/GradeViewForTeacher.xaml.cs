using Journalx3Piska.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Journalx3Piska.View
{
    /// <summary>
    /// Логика взаимодействия для GradeViewForTeacher.xaml
    /// </summary>
    public partial class GradeViewForTeacher : Window
    {
        public GradeViewForTeacher()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
