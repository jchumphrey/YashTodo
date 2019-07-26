namespace FinalTodo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeUntil : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Todoes", "TimeUntil", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Todoes", "TimeUntil");
        }
    }
}
