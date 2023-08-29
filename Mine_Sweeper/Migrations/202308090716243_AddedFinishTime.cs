namespace Mine_Sweeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFinishTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FinishTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "FinishTime");
        }
    }
}
