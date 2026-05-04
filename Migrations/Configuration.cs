namespace Journalx3Piska.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SchoolContext context)
        {
            // Проверяем, есть ли уже данные
            if (context.Users.Any())
            {
                return;
            }

            // USERS
            var director = new User { Username = "director1", PasswordHash = "pass", Role = "Директор" };
            var teacher1 = new User { Username = "teacher1", PasswordHash = "pass", Role = "Учитель" };
            var teacher2 = new User { Username = "teacher2", PasswordHash = "pass", Role = "Учитель" };
            var teacher3 = new User { Username = "teacher3", PasswordHash = "pass", Role = "Учитель" };

            var student1 = new User { Username = "student1", PasswordHash = "pass", Role = "Ученик" };
            var student2 = new User { Username = "student2", PasswordHash = "pass", Role = "Ученик" };
            var student3 = new User { Username = "student3", PasswordHash = "pass", Role = "Ученик" };
            var student4 = new User { Username = "student4", PasswordHash = "pass", Role = "Ученик" };
            var student5 = new User { Username = "student5", PasswordHash = "pass", Role = "Ученик" };
            var student6 = new User { Username = "student6", PasswordHash = "pass", Role = "Ученик" };
            var student7 = new User { Username = "student7", PasswordHash = "pass", Role = "Ученик" };
            var student8 = new User { Username = "student8", PasswordHash = "pass", Role = "Ученик" };
            var student9 = new User { Username = "student9", PasswordHash = "pass", Role = "Ученик" };
            var student10 = new User { Username = "student10", PasswordHash = "pass", Role = "Ученик" };
            var student11 = new User { Username = "student11", PasswordHash = "pass", Role = "Ученик" };
            var student12 = new User { Username = "student12", PasswordHash = "pass", Role = "Ученик" };
            var student13 = new User { Username = "student13", PasswordHash = "pass", Role = "Ученик" };
            var student14 = new User { Username = "student14", PasswordHash = "pass", Role = "Ученик" };
            var student15 = new User { Username = "student15", PasswordHash = "pass", Role = "Ученик" };

            context.Users.AddRange(new[] { director, teacher1, teacher2, teacher3,
        student1, student2, student3, student4, student5, student6, student7,
        student8, student9, student10, student11, student12, student13, student14, student15 });
            context.SaveChanges();

            // TEACHERS - используем реальные объекты User
            var teacherIvanov = new Teacher { FullName = "Иванов И.И.", User = teacher1 };
            var teacherPetrov = new Teacher { FullName = "Петров П.П.", User = teacher2 };
            var teacherSidorov = new Teacher { FullName = "Сидоров С.С.", User = teacher3 };

            context.Teachers.AddRange(new[] { teacherIvanov, teacherPetrov, teacherSidorov });
            context.SaveChanges();

            // STUDENTS - используем реальные объекты User
            var studentAlexey = new Student { FullName = "Алексей Смирнов", User = student1 };
            var studentIvan = new Student { FullName = "Иван Кузнецов", User = student2 };
            var studentDmitry = new Student { FullName = "Дмитрий Попов", User = student3 };
            var studentSergey = new Student { FullName = "Сергей Васильев", User = student4 };
            var studentAndrey = new Student { FullName = "Андрей Соколов", User = student5 };
            var studentNikita = new Student { FullName = "Никита Михайлов", User = student6 };
            var studentEgor = new Student { FullName = "Егор Новиков", User = student7 };
            var studentMaxim = new Student { FullName = "Максим Федоров", User = student8 };
            var studentArtem = new Student { FullName = "Артем Морозов", User = student9 };
            var studentDaniil = new Student { FullName = "Даниил Волков", User = student10 };
            var studentIlya = new Student { FullName = "Илья Алексеев", User = student11 };
            var studentKirill = new Student { FullName = "Кирилл Лебедев", User = student12 };
            var studentRoman = new Student { FullName = "Роман Семенов", User = student13 };
            var studentPavel = new Student { FullName = "Павел Егоров", User = student14 };
            var studentVladimir = new Student { FullName = "Владимир Павлов", User = student15 };

            context.Students.AddRange(new[] {
        studentAlexey, studentIvan, studentDmitry, studentSergey, studentAndrey,
        studentNikita, studentEgor, studentMaxim, studentArtem, studentDaniil,
        studentIlya, studentKirill, studentRoman, studentPavel, studentVladimir
    });
            context.SaveChanges();

            // SUBJECTS - используем реальные объекты Teacher
            var math = new Subject { SubjectName = "Математика", Teacher = teacherIvanov };
            var physics = new Subject { SubjectName = "Физика", Teacher = teacherPetrov };
            var chemistry = new Subject { SubjectName = "Химия", Teacher = teacherSidorov };
            var history = new Subject { SubjectName = "История", Teacher = teacherIvanov };
            var english = new Subject { SubjectName = "Английский", Teacher = teacherPetrov };

            context.Subjects.AddRange(new[] { math, physics, chemistry, history, english });
            context.SaveChanges();

            // GRADES - используем реальные объекты Student и Subject
            context.Grades.AddRange(new[]
            {
        new Grade { Student = studentAlexey, Subject = math, GradeValue = 5, GradeDate = DateTime.Parse("2026-05-01") },
        new Grade { Student = studentAlexey, Subject = physics, GradeValue = 4, GradeDate = DateTime.Parse("2026-05-02") },
        new Grade { Student = studentIvan, Subject = math, GradeValue = 3, GradeDate = DateTime.Parse("2026-05-01") },
        new Grade { Student = studentIvan, Subject = chemistry, GradeValue = 5, GradeDate = DateTime.Parse("2026-05-02") },
        new Grade { Student = studentDmitry, Subject = physics, GradeValue = 4, GradeDate = DateTime.Parse("2026-05-01") },
        new Grade { Student = studentDmitry, Subject = history, GradeValue = 5, GradeDate = DateTime.Parse("2026-05-03") }
    });

            context.SaveChanges();
        }
    }
}
