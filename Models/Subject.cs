using System.ComponentModel.DataAnnotations;

public class Subject
{
    [Key]
    public int SubjectID { get; set; }

    [Required, MaxLength(50)]
    public string SubjectName { get; set; }

    public int TeacherID { get; set; }
    public Teacher Teacher { get; set; }
}