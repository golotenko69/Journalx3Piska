using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int UserID { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; }

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; }

    [Required]
    public string Role { get; set; } // Директор / Учитель / Ученик
}