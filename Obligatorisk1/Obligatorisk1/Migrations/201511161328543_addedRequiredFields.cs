namespace Obligatorisk1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRequiredFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Components", "ComponentName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Components", "ComponentName", c => c.String());
        }
    }
}
