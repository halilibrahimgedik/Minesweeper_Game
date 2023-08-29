namespace Mine_Sweeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedNameOfBestTimeScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "BestTimeScore", c => c.String());
            DropColumn("dbo.Users", "BestScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "BestScore", c => c.String());
            DropColumn("dbo.Users", "BestTimeScore");
        }
    }
}
