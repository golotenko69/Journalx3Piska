using System;
using System.ComponentModel.DataAnnotations;

public class Grade
{
    [Key]
    public int GradeID { get; set; }
    public int StudentID { get; set; }
    public Student Student { get; set; }
    public int SubjectID { get; set; }
    public Subject Subject { get; set; }

    [Range(1, 5)]
    public int GradeValue { get; set; }

    public DateTime GradeDate { get; set; }
}