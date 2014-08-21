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
        }

        public override void Down()
        {
            DropColumn("dbo.TeamMembership", "IsAdmin");
            DropColumn("dbo.TeamMembership", "IsCoach");
            RenameColumn("dbo.TeamMembership", "UserID", "User_ID");
            RenameColumn("dbo.TeamMembership", "TeamID", "Team_ID");
        }
    }
}
