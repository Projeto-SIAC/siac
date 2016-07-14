namespace SIAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSimProvaAddTipoQuestoes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SimProva", "TipoQuestoes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SimProva", "TipoQuestoes");
        }
    }
}
