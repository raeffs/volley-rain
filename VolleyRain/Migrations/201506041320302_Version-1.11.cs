namespace VolleyRain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version111 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamMembership", "IsTemporary", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamMembership", "IsTemporary");
        }
    }
}
