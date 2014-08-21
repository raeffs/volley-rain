namespace VolleyRain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Version15 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.TeamMembership", "Team_ID", "TeamID");
            RenameColumn("dbo.TeamMembership", "User_ID", "UserID");
            AddColumn("dbo.TeamMembership", "IsCoach", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.TeamMembership", "IsAdmin", c => c.Boolean(nullable: false, defaultValue: false));

            //DropForeignKey("dbo.TeamMembership", "Team_ID", "dbo.Team");
            //DropForeignKey("dbo.TeamMembership", "User_ID", "dbo.User");
            //DropIndex("dbo.TeamMembership", new[] { "Team_ID" });
            //DropIndex("dbo.TeamMembership", new[] { "User_ID" });
            //CreateTable(
            //    "dbo.TeamMembership",
            //    c => new
            //        {
            //            TeamID = c.Int(nullable: false),
            //            UserID = c.Int(nullable: false),
            //            IsCoach = c.Boolean(nullable: false),
            //            IsAdmin = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.TeamID, t.UserID })
            //    .ForeignKey("dbo.Team", t => t.TeamID, cascadeDelete: true)
            //    .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
            //    .Index(t => t.TeamID)
            //    .Index(t => t.UserID);

            //DropTable("dbo.TeamMembership");
        }

        public override void Down()
        {
            DropColumn("dbo.TeamMembership", "IsAdmin");
            DropColumn("dbo.TeamMembership", "IsCoach");
            RenameColumn("dbo.TeamMembership", "UserID", "User_ID");
            RenameColumn("dbo.TeamMembership", "TeamID", "Team_ID");

            //CreateTable(
            //    "dbo.TeamMembership",
            //    c => new
            //        {
            //            Team_ID = c.Int(nullable: false),
            //            User_ID = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.Team_ID, t.User_ID });

            //DropForeignKey("dbo.TeamMembership", "UserID", "dbo.User");
            //DropForeignKey("dbo.TeamMembership", "TeamID", "dbo.Team");
            //DropIndex("dbo.TeamMembership", new[] { "UserID" });
            //DropIndex("dbo.TeamMembership", new[] { "TeamID" });
            //DropTable("dbo.TeamMembership");
            //CreateIndex("dbo.TeamMembership", "User_ID");
            //CreateIndex("dbo.TeamMembership", "Team_ID");
            //AddForeignKey("dbo.TeamMembership", "User_ID", "dbo.User", "ID", cascadeDelete: true);
            //AddForeignKey("dbo.TeamMembership", "Team_ID", "dbo.Team", "ID", cascadeDelete: true);
        }
    }
}
