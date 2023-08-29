namespace Mine_Sweeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bestScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BestScore", c => c.Int(nullable: false));
            DropColumn("dbo.Users", "BestTimeScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "BestTimeScore", c => c.String());
            DropColumn("dbo.Users", "BestScore");
        }
    }
}
