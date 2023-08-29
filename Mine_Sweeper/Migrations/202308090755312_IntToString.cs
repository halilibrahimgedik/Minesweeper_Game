namespace Mine_Sweeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "FinishTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "FinishTime", c => c.Int(nullable: false));
        }
    }
}
