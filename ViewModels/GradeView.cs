using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journalx3Piska.ViewModels
{
    public class GradeView
    {
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public double AverageGrade { get; set; }
    }
}

