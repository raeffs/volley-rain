namespace VolleyRain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameDetails", "IsCommited", c => c.Boolean(nullable: false));
            AddColumn("dbo.GameDetails", "SetsWonHome", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "SetsWonGuest", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set1PointsHome", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set1PointsGuest", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set2PointsHome", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set2PointsGuest", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set3PointsHome", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set3PointsGuest", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set4PointsHome", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set4PointsGuest", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set5PointsHome", c => c.Int(nullable: false));
            AddColumn("dbo.GameDetails", "Set5PointsGuest", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameDetails", "Set5PointsGuest");
            DropColumn("dbo.GameDetails", "Set5PointsHome");
            DropColumn("dbo.GameDetails", "Set4PointsGuest");
            DropColumn("dbo.GameDetails", "Set4PointsHome");
            DropColumn("dbo.GameDetails", "Set3PointsGuest");
            DropColumn("dbo.GameDetails", "Set3PointsHome");
            DropColumn("dbo.GameDetails", "Set2PointsGuest");
            DropColumn("dbo.GameDetails", "Set2PointsHome");
            DropColumn("dbo.GameDetails", "Set1PointsGuest");
            DropColumn("dbo.GameDetails", "Set1PointsHome");
            DropColumn("dbo.GameDetails", "SetsWonGuest");
            DropColumn("dbo.GameDetails", "SetsWonHome");
            DropColumn("dbo.GameDetails", "IsCommited");
        }
    }
}
