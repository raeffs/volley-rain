namespace VolleyRain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Version17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttendanceType", "IsUserSelectable", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Document", "Description", c => c.String(nullable: false, defaultValue: string.Empty));
            AddColumn("dbo.Document", "Timestamp", c => c.DateTime(nullable: false, defaultValue: new DateTime(2014, 8, 11, 19, 10, 0)));
        }

        public override void Down()
        {
            DropColumn("dbo.Document", "Timestamp");
            DropColumn("dbo.Document", "Description");
            DropColumn("dbo.AttendanceType", "IsUserSelectable");
        }
    }
}
