namespace FinalTodo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeUntil2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Todoes", "TimeUntil", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Todoes", "TimeUntil", c => c.Time(nullable: false, precision: 7));
        }
    }
}
