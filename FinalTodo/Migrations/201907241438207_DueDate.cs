namespace FinalTodo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DueDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Todoes", "DueDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Todoes", "DueDate");
        }
    }
}
