namespace Journalx3Piska.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
        CREATE PROCEDURE sp_AddGrade
            @StudentId INT,
            @SubjectId INT,
            @Value INT
        AS
        BEGIN
            INSERT INTO Grades (StudentID, SubjectID, GradeValue, GradeDate)
            VALUES (@StudentId, @SubjectId, @Value, GETDATE())
        END");
        }
        
        public override void Down()
        {
        }
    }
}
