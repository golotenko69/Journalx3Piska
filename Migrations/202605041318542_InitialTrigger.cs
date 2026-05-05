namespace Journalx3Piska.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialTrigger : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "AverageGrade", c => c.Double(nullable: false));
            Sql(@"
        CREATE TRIGGER trg_UpdateAverageGrade
        ON Grades
        AFTER INSERT, UPDATE, DELETE
        AS
        BEGIN
            UPDATE Students
            SET AverageGrade = (SELECT AVG(CAST(GradeValue AS FLOAT)) 
                                FROM Grades 
                                WHERE Grades.StudentID = Students.StudentID)
            WHERE StudentID IN (SELECT StudentID FROM inserted UNION SELECT StudentID FROM deleted)
        END");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "AverageGrade");
        }
    }
}
