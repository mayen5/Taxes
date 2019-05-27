namespace Taxes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PropertyTypes",
                c => new
                    {
                        PropertyTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PropertyTypeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PropertyTypes");
        }
    }
}
