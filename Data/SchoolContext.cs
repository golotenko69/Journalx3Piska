using System.Data.Entity;

public class SchoolContext : DbContext
{
    public SchoolContext()
        : base("SchoolJournalConnection")
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        // Role constraint (как CHECK)
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(20);

        // UNIQUE: Student + Subject + Date
        modelBuilder.Entity<Grade>()
            .HasIndex(g => new { g.StudentID, g.SubjectID, g.GradeDate })
            .IsUnique();

        // Отключаем каскадное удаление для User-Student
        modelBuilder.Entity<Student>()
            .HasRequired(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserID)
            .WillCascadeOnDelete(false);

        // Отключаем каскадное удаление для User-Teacher
        modelBuilder.Entity<Teacher>()
            .HasRequired(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserID)
            .WillCascadeOnDelete(false);

        // Опционально: отключаем каскадное удаление для Teacher-Subject
        modelBuilder.Entity<Subject>()
            .HasRequired(s => s.Teacher)
            .WithMany()
            .HasForeignKey(s => s.TeacherID)
            .WillCascadeOnDelete(false);

        base.OnModelCreating(modelBuilder);
    }
}