namespace SIAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizaQuestaoEAlternativa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Alternativa", "Enunciado", c => c.String(nullable: false));
            AlterColumn("dbo.Questao", "Enunciado", c => c.String(nullable: false));
            AlterColumn("dbo.Questao", "ChaveDeResposta", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questao", "ChaveDeResposta", c => c.String(unicode: false, storeType: "text"));
            AlterColumn("dbo.Questao", "Enunciado", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Alternativa", "Enunciado", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
