namespace Mine_Sweeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class last : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "BestTimeScore", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "BestTimeScore", c => c.Int(nullable: false));
        }
    }
}
