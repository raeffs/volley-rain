namespace VolleyRain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version110 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        Event_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Event", t => t.Event_ID, cascadeDelete: true)
                .Index(t => t.Event_ID);
            
            AddColumn("dbo.NewsArticle", "IsPublic", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Team", "ExternalID", c => c.Int());
            AlterColumn("dbo.Team", "ExternalGroupID", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "Event_ID", "dbo.Event");
            DropIndex("dbo.Comment", new[] { "Event_ID" });
            AlterColumn("dbo.Team", "ExternalGroupID", c => c.Int(nullable: false));
            AlterColumn("dbo.Team", "ExternalID", c => c.Int(nullable: false));
            DropColumn("dbo.NewsArticle", "IsPublic");
            DropTable("dbo.Comment");
        }
    }
}
