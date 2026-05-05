namespace Journalx3Piska.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AndrewLox : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Students", "AverageGrade");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "AverageGrade", c => c.Double(nullable: false));
        }
    }
}
