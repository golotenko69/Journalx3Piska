namespace Journalx3Piska.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeAverageGradeNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "AverageGrade", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "AverageGrade", c => c.Double(nullable: false));
        }
    }
}
