namespace Mine_Sweeper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameModeAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "GameMode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "GameMode");
        }
    }
}
