namespace dsignelixir.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ThumbImage = c.String(),
                        CategoryId = c.Int(nullable: false),
                        OrderNumber = c.Int(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Description = c.String(),
                        AlbumId = c.Int(nullable: false),
                        OrderNumber = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.AlbumId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 100),
                        PasswordHash = c.Binary(nullable: false, maxLength: 64),
                        PasswordSalt = c.Binary(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false, maxLength: 200),
                        Comment = c.String(maxLength: 200),
                        IsApproved = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastLogin = c.DateTime(),
                        DateLastActivity = c.DateTime(),
                        DateLastPasswordChange = c.DateTime(nullable: false),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.UserName);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.RoleName);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_RoleName = c.String(nullable: false, maxLength: 100),
                        User_UserName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => new { t.Role_RoleName, t.User_UserName })
                .ForeignKey("dbo.Roles", t => t.Role_RoleName, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserName, cascadeDelete: true)
                .Index(t => t.Role_RoleName)
                .Index(t => t.User_UserName);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RoleUsers", new[] { "User_UserName" });
            DropIndex("dbo.RoleUsers", new[] { "Role_RoleName" });
            DropIndex("dbo.Images", new[] { "AlbumId" });
            DropIndex("dbo.Albums", new[] { "CategoryId" });
            DropForeignKey("dbo.RoleUsers", "User_UserName", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_RoleName", "dbo.Roles");
            DropForeignKey("dbo.Images", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.Albums", "CategoryId", "dbo.Categories");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Images");
            DropTable("dbo.Categories");
            DropTable("dbo.Albums");
        }
    }
}
