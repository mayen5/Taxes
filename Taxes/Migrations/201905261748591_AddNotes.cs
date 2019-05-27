namespace Taxes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PropertyTypes", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PropertyTypes", "Notes");
        }
    }
}
