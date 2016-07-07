namespace SIAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSimProvaPeso : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SimProva", "Peso", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SimProva", "Peso", c => c.Int());
        }
    }
}
