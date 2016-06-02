namespace SIAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequiredFields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AvalAcademica", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" });
            DropIndex("dbo.AvalAcademica", new[] { "CodSala" });
            DropIndex("dbo.AvalCertificacao", new[] { "CodSala" });
            DropIndex("dbo.AvalAcadReposicao", new[] { "CodSala" });
            AlterColumn("dbo.AvalAcademica", "CodTurno", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AlterColumn("dbo.AvalAcademica", "NumTurma", c => c.Int());
            AlterColumn("dbo.AvalAcademica", "Periodo", c => c.Int());
            AlterColumn("dbo.AvalAcademica", "CodCurso", c => c.Int());
            AlterColumn("dbo.AvalAcademica", "CodSala", c => c.Int());
            AlterColumn("dbo.AvalCertificacao", "CodSala", c => c.Int());
            AlterColumn("dbo.AvalAcadReposicao", "CodSala", c => c.Int());
            AlterColumn("dbo.AvaliacaoProrrogacao", "DuracaoAnterior", c => c.Int(nullable: false));
            AlterColumn("dbo.AvaliacaoProrrogacao", "DuracaoNova", c => c.Int(nullable: false));
            CreateIndex("dbo.AvalAcademica", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" });
            CreateIndex("dbo.AvalAcademica", "CodSala");
            CreateIndex("dbo.AvalCertificacao", "CodSala");
            CreateIndex("dbo.AvalAcadReposicao", "CodSala");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AvalAcadReposicao", new[] { "CodSala" });
            DropIndex("dbo.AvalCertificacao", new[] { "CodSala" });
            DropIndex("dbo.AvalAcademica", new[] { "CodSala" });
            DropIndex("dbo.AvalAcademica", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" });
            AlterColumn("dbo.AvaliacaoProrrogacao", "DuracaoNova", c => c.Int());
            AlterColumn("dbo.AvaliacaoProrrogacao", "DuracaoAnterior", c => c.Int());
            AlterColumn("dbo.AvalAcadReposicao", "CodSala", c => c.Int(nullable: false));
            AlterColumn("dbo.AvalCertificacao", "CodSala", c => c.Int(nullable: false));
            AlterColumn("dbo.AvalAcademica", "CodSala", c => c.Int(nullable: false));
            AlterColumn("dbo.AvalAcademica", "CodCurso", c => c.Int(nullable: false));
            AlterColumn("dbo.AvalAcademica", "Periodo", c => c.Int(nullable: false));
            AlterColumn("dbo.AvalAcademica", "NumTurma", c => c.Int(nullable: false));
            AlterColumn("dbo.AvalAcademica", "CodTurno", c => c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false));
            CreateIndex("dbo.AvalAcadReposicao", "CodSala");
            CreateIndex("dbo.AvalCertificacao", "CodSala");
            CreateIndex("dbo.AvalAcademica", "CodSala");
            CreateIndex("dbo.AvalAcademica", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" });
        }
    }
}
