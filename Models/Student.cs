using System.ComponentModel.DataAnnotations;

public class Student
{
    [Key]
    public int StudentID { get; set; }

    public double AverageGrade { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; }

    public int UserID { get; set; }
    public User User { get; set; }
}