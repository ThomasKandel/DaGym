namespace DaGym.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EfterheltnymodellGymClassView : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GymClasses", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GymClasses", "Name", c => c.String());
        }
    }
}
