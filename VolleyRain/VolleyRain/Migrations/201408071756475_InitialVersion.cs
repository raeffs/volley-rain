namespace VolleyRain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialVersion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendance",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Event_ID = c.Int(nullable: false),
                        Type_ID = c.Int(nullable: false),
                        User_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Event", t => t.Event_ID, cascadeDelete: true)
                .ForeignKey("dbo.AttendanceType", t => t.Type_ID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_ID, cascadeDelete: true)
                .Index(t => t.Event_ID)
                .Index(t => t.Type_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Location = c.String(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Team_ID = c.Int(),
                        Type_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Team", t => t.Team_ID)
                .ForeignKey("dbo.EventType", t => t.Type_ID, cascadeDelete: true)
                .Index(t => t.Team_ID)
                .Index(t => t.Type_ID);
            
            CreateTable(
                "dbo.GameDetails",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ExternalID = c.Int(nullable: false),
                        HomeTeam = c.String(nullable: false),
                        GuestTeam = c.String(nullable: false),
                        Hall = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Event", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ExternalID = c.Int(nullable: false),
                        ExternalGroupID = c.Int(nullable: false),
                        Season_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Season", t => t.Season_ID, cascadeDelete: true)
                .Index(t => t.Season_ID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Password = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsLockedOut = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsBuiltIn = c.Boolean(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        IsDefaultUserRole = c.Boolean(nullable: false),
                        IsDefaultAdminRole = c.Boolean(nullable: false),
                        IsDefaultTeamAdminRole = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Season",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EventType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ShortName = c.String(),
                        ColorCode = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AttendanceType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ShortName = c.String(nullable: false),
                        RepresentsAttendance = c.Boolean(nullable: false),
                        ColorCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Application = c.String(nullable: false),
                        Level = c.String(nullable: false),
                        Logger = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        SessionID = c.String(nullable: false),
                        ThreadID = c.Int(nullable: false),
                        UserIdentity = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NewsArticle",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Ranking",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExternalID = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        Team = c.String(),
                        NumberOfGames = c.Int(nullable: false),
                        SetsWon = c.Int(nullable: false),
                        SetsLost = c.Int(nullable: false),
                        SetQuotient = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BallsWon = c.Int(nullable: false),
                        BallsLost = c.Int(nullable: false),
                        BallQuotient = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Points = c.Int(nullable: false),
                        AssociatedTeam_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Team", t => t.AssociatedTeam_ID, cascadeDelete: true)
                .Index(t => t.AssociatedTeam_ID);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        User_ID = c.Int(nullable: false),
                        Role_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_ID, t.Role_ID })
                .ForeignKey("dbo.User", t => t.User_ID, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.Role_ID, cascadeDelete: true)
                .Index(t => t.User_ID)
                .Index(t => t.Role_ID);
            
            CreateTable(
                "dbo.TeamMembership",
                c => new
                    {
                        Team_ID = c.Int(nullable: false),
                        User_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_ID, t.User_ID })
                .ForeignKey("dbo.Team", t => t.Team_ID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_ID, cascadeDelete: true)
                .Index(t => t.Team_ID)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ranking", "AssociatedTeam_ID", "dbo.Team");
            DropForeignKey("dbo.Attendance", "User_ID", "dbo.User");
            DropForeignKey("dbo.Attendance", "Type_ID", "dbo.AttendanceType");
            DropForeignKey("dbo.Attendance", "Event_ID", "dbo.Event");
            DropForeignKey("dbo.Event", "Type_ID", "dbo.EventType");
            DropForeignKey("dbo.Event", "Team_ID", "dbo.Team");
            DropForeignKey("dbo.Team", "Season_ID", "dbo.Season");
            DropForeignKey("dbo.TeamMembership", "User_ID", "dbo.User");
            DropForeignKey("dbo.TeamMembership", "Team_ID", "dbo.Team");
            DropForeignKey("dbo.UserRole", "Role_ID", "dbo.Role");
            DropForeignKey("dbo.UserRole", "User_ID", "dbo.User");
            DropForeignKey("dbo.GameDetails", "ID", "dbo.Event");
            DropIndex("dbo.TeamMembership", new[] { "User_ID" });
            DropIndex("dbo.TeamMembership", new[] { "Team_ID" });
            DropIndex("dbo.UserRole", new[] { "Role_ID" });
            DropIndex("dbo.UserRole", new[] { "User_ID" });
            DropIndex("dbo.Ranking", new[] { "AssociatedTeam_ID" });
            DropIndex("dbo.Team", new[] { "Season_ID" });
            DropIndex("dbo.GameDetails", new[] { "ID" });
            DropIndex("dbo.Event", new[] { "Type_ID" });
            DropIndex("dbo.Event", new[] { "Team_ID" });
            DropIndex("dbo.Attendance", new[] { "User_ID" });
            DropIndex("dbo.Attendance", new[] { "Type_ID" });
            DropIndex("dbo.Attendance", new[] { "Event_ID" });
            DropTable("dbo.TeamMembership");
            DropTable("dbo.UserRole");
            DropTable("dbo.Ranking");
            DropTable("dbo.NewsArticle");
            DropTable("dbo.Log");
            DropTable("dbo.AttendanceType");
            DropTable("dbo.EventType");
            DropTable("dbo.Season");
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.Team");
            DropTable("dbo.GameDetails");
            DropTable("dbo.Event");
            DropTable("dbo.Attendance");
        }
    }
}
