namespace Obligatorisk1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComponentName = c.String(),
                        ComponentInfo = c.String(),
                        CategoryId = c.Int(nullable: false),
                        Datasheet = c.String(),
                        Image = c.Binary(),
                        ManufacturerLink = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.ComponentComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        User_Id = c.Int(),
                        Component_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Components", t => t.Component_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Component_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudieNumber = c.String(),
                        Name = c.String(),
                        MobileNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpecificComponents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComponentNumber = c.Int(nullable: false),
                        SerieNr = c.String(),
                        LoanInformation_Id = c.Int(),
                        Component_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoanInformations", t => t.LoanInformation_Id)
                .ForeignKey("dbo.Components", t => t.Component_Id)
                .Index(t => t.LoanInformation_Id)
                .Index(t => t.Component_Id);
            
            CreateTable(
                "dbo.LoanInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoanDate = c.DateTime(),
                        ReturnDate = c.DateTime(),
                        IsEmailSend = c.String(),
                        ReservationDate = c.DateTime(),
                        ReservationId = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpecificComponents", "Component_Id", "dbo.Components");
            DropForeignKey("dbo.SpecificComponents", "LoanInformation_Id", "dbo.LoanInformations");
            DropForeignKey("dbo.LoanInformations", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ComponentComments", "Component_Id", "dbo.Components");
            DropForeignKey("dbo.ComponentComments", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Components", "CategoryId", "dbo.Categories");
            DropIndex("dbo.LoanInformations", new[] { "User_Id" });
            DropIndex("dbo.SpecificComponents", new[] { "Component_Id" });
            DropIndex("dbo.SpecificComponents", new[] { "LoanInformation_Id" });
            DropIndex("dbo.ComponentComments", new[] { "Component_Id" });
            DropIndex("dbo.ComponentComments", new[] { "User_Id" });
            DropIndex("dbo.Components", new[] { "CategoryId" });
            DropTable("dbo.LoanInformations");
            DropTable("dbo.SpecificComponents");
            DropTable("dbo.Users");
            DropTable("dbo.ComponentComments");
            DropTable("dbo.Components");
            DropTable("dbo.Categories");
        }
    }
}
