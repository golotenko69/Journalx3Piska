using System.ComponentModel.DataAnnotations;

public class Teacher
{
    [Key]
    public int TeacherID { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; }

    public int UserID { get; set; }
    public User User { get; set; }
}