namespace MyProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        ContactName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        ProjectStatusId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Duration = c.String(),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        StatusId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.WorkLogs",
                c => new
                    {
                        WorkLogId = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        TaskId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ElapsedMinutes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WorkLogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ClientId", "dbo.Clients");
            DropIndex("dbo.Tasks", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "ClientId" });
            DropTable("dbo.WorkLogs");
            DropTable("dbo.Tasks");
            DropTable("dbo.Projects");
            DropTable("dbo.Clients");
        }
    }
}
