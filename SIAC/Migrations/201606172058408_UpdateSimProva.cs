namespace SIAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSimProva : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SimProva", "FlagRedacao", c => c.Boolean(nullable: false));
            AddColumn("dbo.SimProva", "OrdemDesempate", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SimProva", "OrdemDesempate");
            DropColumn("dbo.SimProva", "FlagRedacao");
        }
    }
}
