namespace SIAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alternativa",
                c => new
                    {
                        CodQuestao = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        Enunciado = c.String(nullable: false, maxLength: 250, unicode: false),
                        Comentario = c.String(maxLength: 250, unicode: false),
                        FlagGabarito = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.CodQuestao, t.CodOrdem })
                .ForeignKey("dbo.Questao", t => t.CodQuestao)
                .Index(t => t.CodQuestao);
            
            CreateTable(
                "dbo.Questao",
                c => new
                    {
                        CodQuestao = c.Int(nullable: false, identity: true),
                        CodProfessor = c.Int(nullable: false),
                        CodDificuldade = c.Int(nullable: false),
                        CodTipoQuestao = c.Int(nullable: false),
                        Enunciado = c.String(nullable: false, maxLength: 250, unicode: false),
                        Objetivo = c.String(maxLength: 250, unicode: false),
                        Comentario = c.String(maxLength: 250, unicode: false),
                        ChaveDeResposta = c.String(unicode: false, storeType: "text"),
                        DtCadastro = c.DateTime(nullable: false),
                        DtUltimoUso = c.DateTime(),
                        FlagArquivo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CodQuestao)
                .ForeignKey("dbo.Professor", t => t.CodProfessor)
                .ForeignKey("dbo.Dificuldade", t => t.CodDificuldade)
                .ForeignKey("dbo.TipoQuestao", t => t.CodTipoQuestao)
                .Index(t => t.CodProfessor)
                .Index(t => t.CodDificuldade)
                .Index(t => t.CodTipoQuestao);
            
            CreateTable(
                "dbo.Dificuldade",
                c => new
                    {
                        CodDificuldade = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 20, unicode: false),
                        Comentario = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.CodDificuldade);
            
            CreateTable(
                "dbo.AvalAuto",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodPessoaFisica = c.Int(nullable: false),
                        CodDificuldade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica)
                .ForeignKey("dbo.Avaliacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.Dificuldade", t => t.CodDificuldade)
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => t.CodPessoaFisica)
                .Index(t => t.CodDificuldade);
            
            CreateTable(
                "dbo.Avaliacao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        DtCadastro = c.DateTime(nullable: false),
                        DtAplicacao = c.DateTime(),
                        Duracao = c.Int(),
                        FlagLiberada = c.Boolean(nullable: false),
                        FlagArquivo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.TipoAvaliacao", t => t.CodTipoAvaliacao)
                .Index(t => t.CodTipoAvaliacao);
            
            CreateTable(
                "dbo.AvalAcademica",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodTurno = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        NumTurma = c.Int(nullable: false),
                        Periodo = c.Int(nullable: false),
                        CodCurso = c.Int(nullable: false),
                        CodSala = c.Int(nullable: false),
                        CodProfessor = c.Int(nullable: false),
                        CodDisciplina = c.Int(nullable: false),
                        Valor = c.Double(),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.Disciplina", t => t.CodDisciplina)
                .ForeignKey("dbo.Sala", t => t.CodSala)
                .ForeignKey("dbo.Turma", t => new { t.CodCurso, t.Periodo, t.CodTurno, t.NumTurma })
                .ForeignKey("dbo.Professor", t => t.CodProfessor)
                .ForeignKey("dbo.Avaliacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.CodCurso, t.Periodo, t.CodTurno, t.NumTurma })
                .Index(t => t.CodSala)
                .Index(t => t.CodProfessor)
                .Index(t => t.CodDisciplina);
            
            CreateTable(
                "dbo.Disciplina",
                c => new
                    {
                        CodDisciplina = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        Sigla = c.String(maxLength: 10, unicode: false),
                        FlagEletivaOptativa = c.Boolean(),
                        FlagFlexivel = c.Boolean(),
                        CodDiscIFRN = c.Int(),
                    })
                .PrimaryKey(t => t.CodDisciplina);
            
            CreateTable(
                "dbo.AvalCertificacao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodSala = c.Int(nullable: false),
                        CodProfessor = c.Int(nullable: false),
                        CodDisciplina = c.Int(nullable: false),
                        Valor = c.Double(),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.Sala", t => t.CodSala)
                .ForeignKey("dbo.Professor", t => t.CodProfessor)
                .ForeignKey("dbo.Disciplina", t => t.CodDisciplina)
                .ForeignKey("dbo.Avaliacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => t.CodSala)
                .Index(t => t.CodProfessor)
                .Index(t => t.CodDisciplina);
            
            CreateTable(
                "dbo.PessoaFisica",
                c => new
                    {
                        CodPessoa = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100, unicode: false),
                        DtNascimento = c.DateTime(storeType: "date"),
                        Cpf = c.String(maxLength: 11, fixedLength: true, unicode: false),
                        Sexo = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.CodPessoa)
                .ForeignKey("dbo.Pessoa", t => t.CodPessoa)
                .Index(t => t.CodPessoa);
            
            CreateTable(
                "dbo.AvalPessoaResultado",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodPessoaFisica = c.Int(nullable: false),
                        HoraTermino = c.DateTime(),
                        QteAcertoObj = c.Int(),
                        Nota = c.Double(),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodPessoaFisica })
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica)
                .ForeignKey("dbo.Avaliacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => t.CodPessoaFisica);
            
            CreateTable(
                "dbo.Justificacao",
                c => new
                    {
                        CodProfessor = c.Int(nullable: false),
                        CodJustificacao = c.Int(nullable: false),
                        CodPessoaFisica = c.Int(nullable: false),
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        DtCadastro = c.DateTime(nullable: false),
                        DtConfirmacao = c.DateTime(),
                        Descricao = c.String(nullable: false, unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => new { t.CodProfessor, t.CodJustificacao })
                .ForeignKey("dbo.Professor", t => t.CodProfessor)
                .ForeignKey("dbo.AvalPessoaResultado", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodPessoaFisica })
                .Index(t => t.CodProfessor)
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodPessoaFisica });
            
            CreateTable(
                "dbo.AvalAcadReposicao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodSala = c.Int(nullable: false),
                        Valor = c.Double(),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.Sala", t => t.CodSala)
                .ForeignKey("dbo.Avaliacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => t.CodSala);
            
            CreateTable(
                "dbo.Sala",
                c => new
                    {
                        CodSala = c.Int(nullable: false, identity: true),
                        CodBloco = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        Sigla = c.String(maxLength: 15, unicode: false),
                        RefLocal = c.String(maxLength: 255, unicode: false),
                        Observacao = c.String(maxLength: 140, unicode: false),
                        Capacidade = c.Int(),
                    })
                .PrimaryKey(t => t.CodSala)
                .ForeignKey("dbo.Bloco", t => t.CodBloco)
                .Index(t => t.CodBloco);
            
            CreateTable(
                "dbo.Bloco",
                c => new
                    {
                        CodBloco = c.Int(nullable: false, identity: true),
                        CodInstituicao = c.Int(),
                        CodCampus = c.Int(),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        Sigla = c.String(maxLength: 15, unicode: false),
                        RefLocal = c.String(maxLength: 255, unicode: false),
                        Observacao = c.String(maxLength: 140, unicode: false),
                    })
                .PrimaryKey(t => t.CodBloco)
                .ForeignKey("dbo.Campus", t => new { t.CodInstituicao, t.CodCampus })
                .Index(t => new { t.CodInstituicao, t.CodCampus });
            
            CreateTable(
                "dbo.Campus",
                c => new
                    {
                        CodInstituicao = c.Int(nullable: false),
                        CodCampus = c.Int(nullable: false),
                        CodPessoaJuridica = c.Int(nullable: false),
                        CodColaboradorDiretor = c.Int(nullable: false),
                        Sigla = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodInstituicao, t.CodCampus })
                .ForeignKey("dbo.Colaborador", t => t.CodColaboradorDiretor)
                .ForeignKey("dbo.PessoaJuridica", t => t.CodPessoaJuridica)
                .ForeignKey("dbo.Instituicao", t => t.CodInstituicao)
                .Index(t => t.CodInstituicao)
                .Index(t => t.CodPessoaJuridica)
                .Index(t => t.CodColaboradorDiretor);
            
            CreateTable(
                "dbo.AviPublico",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        CodAviTipoPublico = c.Int(nullable: false),
                        Turma_CodCurso = c.Int(),
                        Turma_Periodo = c.Int(),
                        Turma_CodTurno = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Turma_NumTurma = c.Int(),
                        Diretoria_CodInstituicao = c.Int(),
                        Diretoria_CodCampus = c.Int(),
                        Diretoria_CodDiretoria = c.Int(),
                        Instituicao_CodInstituicao = c.Int(),
                        ProReitoria_CodInstituicao = c.Int(),
                        ProReitoria_CodProReitoria = c.Int(),
                        Reitoria_CodInstituicao = c.Int(),
                        Reitoria_CodReitoria = c.Int(),
                        Curso_CodCurso = c.Int(),
                        Campus_CodInstituicao = c.Int(),
                        Campus_CodCampus = c.Int(),
                        PessoaFisica_CodPessoa = c.Int(),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodOrdem, t.CodAviTipoPublico })
                .ForeignKey("dbo.AvalAvi", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.Turma", t => new { t.Turma_CodCurso, t.Turma_Periodo, t.Turma_CodTurno, t.Turma_NumTurma })
                .ForeignKey("dbo.Diretoria", t => new { t.Diretoria_CodInstituicao, t.Diretoria_CodCampus, t.Diretoria_CodDiretoria })
                .ForeignKey("dbo.Instituicao", t => t.Instituicao_CodInstituicao)
                .ForeignKey("dbo.ProReitoria", t => new { t.ProReitoria_CodInstituicao, t.ProReitoria_CodProReitoria })
                .ForeignKey("dbo.Reitoria", t => new { t.Reitoria_CodInstituicao, t.Reitoria_CodReitoria })
                .ForeignKey("dbo.Curso", t => t.Curso_CodCurso)
                .ForeignKey("dbo.AviTipoPublico", t => t.CodAviTipoPublico)
                .ForeignKey("dbo.Campus", t => new { t.Campus_CodInstituicao, t.Campus_CodCampus })
                .ForeignKey("dbo.PessoaFisica", t => t.PessoaFisica_CodPessoa)
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => t.CodAviTipoPublico)
                .Index(t => new { t.Turma_CodCurso, t.Turma_Periodo, t.Turma_CodTurno, t.Turma_NumTurma })
                .Index(t => new { t.Diretoria_CodInstituicao, t.Diretoria_CodCampus, t.Diretoria_CodDiretoria })
                .Index(t => t.Instituicao_CodInstituicao)
                .Index(t => new { t.ProReitoria_CodInstituicao, t.ProReitoria_CodProReitoria })
                .Index(t => new { t.Reitoria_CodInstituicao, t.Reitoria_CodReitoria })
                .Index(t => t.Curso_CodCurso)
                .Index(t => new { t.Campus_CodInstituicao, t.Campus_CodCampus })
                .Index(t => t.PessoaFisica_CodPessoa);
            
            CreateTable(
                "dbo.AvalAvi",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodColabCoordenador = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 250, unicode: false),
                        Objetivo = c.String(nullable: false, unicode: false, storeType: "text"),
                        DtTermino = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.Colaborador", t => t.CodColabCoordenador)
                .ForeignKey("dbo.Avaliacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => t.CodColabCoordenador);
            
            CreateTable(
                "dbo.AviQuestao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodAviModulo = c.Int(nullable: false),
                        CodAviCategoria = c.Int(nullable: false),
                        CodAviIndicador = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        Enunciado = c.String(nullable: false, maxLength: 255, unicode: false),
                        Observacao = c.String(maxLength: 255, unicode: false),
                        FlagDiscursiva = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodAviModulo, t.CodAviCategoria, t.CodAviIndicador, t.CodOrdem })
                .ForeignKey("dbo.AviCategoria", t => t.CodAviCategoria)
                .ForeignKey("dbo.AviIndicador", t => t.CodAviIndicador)
                .ForeignKey("dbo.AviModulo", t => t.CodAviModulo)
                .ForeignKey("dbo.AvalAvi", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => t.CodAviModulo)
                .Index(t => t.CodAviCategoria)
                .Index(t => t.CodAviIndicador);
            
            CreateTable(
                "dbo.AviCategoria",
                c => new
                    {
                        CodAviCategoria = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        Observacao = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.CodAviCategoria);
            
            CreateTable(
                "dbo.AviIndicador",
                c => new
                    {
                        CodAviIndicador = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        Observacao = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.CodAviIndicador);
            
            CreateTable(
                "dbo.AviModulo",
                c => new
                    {
                        CodAviModulo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        Objetivo = c.String(unicode: false, storeType: "text"),
                        Observacao = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.CodAviModulo);
            
            CreateTable(
                "dbo.AviQuestaoAlternativa",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodAviModulo = c.Int(nullable: false),
                        CodAviCategoria = c.Int(nullable: false),
                        CodAviIndicador = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        CodAlternativa = c.Int(nullable: false),
                        Enunciado = c.String(nullable: false, maxLength: 255, unicode: false),
                        FlagAlternativaDiscursiva = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodAviModulo, t.CodAviCategoria, t.CodAviIndicador, t.CodOrdem, t.CodAlternativa })
                .ForeignKey("dbo.AviQuestao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodAviModulo, t.CodAviCategoria, t.CodAviIndicador, t.CodOrdem })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodAviModulo, t.CodAviCategoria, t.CodAviIndicador, t.CodOrdem });
            
            CreateTable(
                "dbo.AviQuestaoPessoaResposta",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodAviModulo = c.Int(nullable: false),
                        CodAviCategoria = c.Int(nullable: false),
                        CodAviIndicador = c.Int(nullable: false),
                        CodPessoaFisica = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        CodRespostaOrdem = c.Int(nullable: false),
                        RespAlternativa = c.Int(),
                        RespDiscursiva = c.String(unicode: false, storeType: "text"),
                        RespData = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodAviModulo, t.CodAviCategoria, t.CodAviIndicador, t.CodPessoaFisica, t.CodOrdem, t.CodRespostaOrdem })
                .ForeignKey("dbo.AviQuestao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodAviModulo, t.CodAviCategoria, t.CodAviIndicador, t.CodOrdem })
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica)
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodAviModulo, t.CodAviCategoria, t.CodAviIndicador, t.CodOrdem })
                .Index(t => t.CodPessoaFisica);
            
            CreateTable(
                "dbo.Colaborador",
                c => new
                    {
                        CodColaborador = c.Int(nullable: false, identity: true),
                        MatrColaborador = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.CodColaborador)
                .ForeignKey("dbo.Usuario", t => t.MatrColaborador)
                .Index(t => t.MatrColaborador);
            
            CreateTable(
                "dbo.Curso",
                c => new
                    {
                        CodCurso = c.Int(nullable: false),
                        CodColabCoordenador = c.Int(nullable: false),
                        CodNivelEnsino = c.Int(nullable: false),
                        CodDiretoria = c.Int(nullable: false),
                        CodCampus = c.Int(nullable: false),
                        CodInstituicao = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        Sigla = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.CodCurso)
                .ForeignKey("dbo.Diretoria", t => new { t.CodInstituicao, t.CodCampus, t.CodDiretoria })
                .ForeignKey("dbo.NivelEnsino", t => t.CodNivelEnsino)
                .ForeignKey("dbo.Colaborador", t => t.CodColabCoordenador)
                .Index(t => t.CodColabCoordenador)
                .Index(t => t.CodNivelEnsino)
                .Index(t => new { t.CodInstituicao, t.CodCampus, t.CodDiretoria });
            
            CreateTable(
                "dbo.Aluno",
                c => new
                    {
                        CodAluno = c.Int(nullable: false, identity: true),
                        CodCurso = c.Int(nullable: false),
                        MatrAluno = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.CodAluno)
                .ForeignKey("dbo.Usuario", t => t.MatrAluno)
                .ForeignKey("dbo.Curso", t => t.CodCurso)
                .Index(t => t.CodCurso)
                .Index(t => t.MatrAluno);
            
            CreateTable(
                "dbo.TurmaDiscAluno",
                c => new
                    {
                        AnoLetivo = c.Int(nullable: false),
                        SemestreLetivo = c.Int(nullable: false),
                        CodCurso = c.Int(nullable: false),
                        Periodo = c.Int(nullable: false),
                        CodTurno = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        NumTurma = c.Int(nullable: false),
                        CodDisciplina = c.Int(nullable: false),
                        CodAluno = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AnoLetivo, t.SemestreLetivo, t.CodCurso, t.Periodo, t.CodTurno, t.NumTurma, t.CodDisciplina, t.CodAluno })
                .ForeignKey("dbo.Turma", t => new { t.CodCurso, t.Periodo, t.CodTurno, t.NumTurma })
                .ForeignKey("dbo.Aluno", t => t.CodAluno)
                .ForeignKey("dbo.Disciplina", t => t.CodDisciplina)
                .Index(t => new { t.CodCurso, t.Periodo, t.CodTurno, t.NumTurma })
                .Index(t => t.CodDisciplina)
                .Index(t => t.CodAluno);
            
            CreateTable(
                "dbo.Turma",
                c => new
                    {
                        CodCurso = c.Int(nullable: false),
                        Periodo = c.Int(nullable: false),
                        CodTurno = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        NumTurma = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodCurso, t.Periodo, t.CodTurno, t.NumTurma })
                .ForeignKey("dbo.Turno", t => t.CodTurno)
                .ForeignKey("dbo.Curso", t => t.CodCurso)
                .Index(t => t.CodCurso)
                .Index(t => t.CodTurno);
            
            CreateTable(
                "dbo.TurmaDiscProfHorario",
                c => new
                    {
                        AnoLetivo = c.Int(nullable: false),
                        SemestreLetivo = c.Int(nullable: false),
                        CodCurso = c.Int(nullable: false),
                        Periodo = c.Int(nullable: false),
                        CodGrupo = c.Int(nullable: false),
                        CodTurno = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        NumTurma = c.Int(nullable: false),
                        CodDisciplina = c.Int(nullable: false),
                        CodProfessor = c.Int(nullable: false),
                        CodDia = c.Int(nullable: false),
                        CodHorario = c.Int(nullable: false),
                        CodSala = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AnoLetivo, t.SemestreLetivo, t.CodCurso, t.Periodo, t.CodGrupo, t.CodTurno, t.NumTurma, t.CodDisciplina, t.CodProfessor, t.CodDia, t.CodHorario })
                .ForeignKey("dbo.DiaSemana", t => t.CodDia)
                .ForeignKey("dbo.Horario", t => new { t.CodGrupo, t.CodTurno, t.CodHorario })
                .ForeignKey("dbo.Professor", t => t.CodProfessor)
                .ForeignKey("dbo.Turma", t => new { t.CodCurso, t.Periodo, t.CodTurno, t.NumTurma })
                .ForeignKey("dbo.Sala", t => t.CodSala)
                .ForeignKey("dbo.Disciplina", t => t.CodDisciplina)
                .Index(t => new { t.CodCurso, t.Periodo, t.CodTurno, t.NumTurma })
                .Index(t => new { t.CodGrupo, t.CodTurno, t.CodHorario })
                .Index(t => t.CodDisciplina)
                .Index(t => t.CodProfessor)
                .Index(t => t.CodDia)
                .Index(t => t.CodSala);
            
            CreateTable(
                "dbo.DiaSemana",
                c => new
                    {
                        CodDia = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.CodDia);
            
            CreateTable(
                "dbo.Horario",
                c => new
                    {
                        CodGrupo = c.Int(nullable: false),
                        CodTurno = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        CodHorario = c.Int(nullable: false),
                        HoraInicio = c.DateTime(),
                        HoraTermino = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.CodGrupo, t.CodTurno, t.CodHorario })
                .ForeignKey("dbo.Turno", t => t.CodTurno)
                .Index(t => t.CodTurno);
            
            CreateTable(
                "dbo.Turno",
                c => new
                    {
                        CodTurno = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.CodTurno);
            
            CreateTable(
                "dbo.SimDiaRealizacao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodDiaRealizacao = c.Int(nullable: false),
                        DtRealizacao = c.DateTime(nullable: false),
                        CodTurno = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        Duracao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao })
                .ForeignKey("dbo.Simulado", t => new { t.Ano, t.NumIdentificador })
                .ForeignKey("dbo.Turno", t => t.CodTurno)
                .Index(t => new { t.Ano, t.NumIdentificador })
                .Index(t => t.CodTurno);
            
            CreateTable(
                "dbo.SimProva",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodDiaRealizacao = c.Int(nullable: false),
                        CodProva = c.Int(nullable: false),
                        CodProfessor = c.Int(),
                        CodDisciplina = c.Int(nullable: false),
                        QteQuestoes = c.Int(nullable: false),
                        Titulo = c.String(maxLength: 200, unicode: false),
                        Descricao = c.String(maxLength: 500, unicode: false),
                        MediaAritmeticaAcerto = c.Decimal(precision: 8, scale: 3),
                        DesvioPadraoAcerto = c.Decimal(precision: 8, scale: 3),
                        Peso = c.Int(),
                    })
                .PrimaryKey(t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao, t.CodProva })
                .ForeignKey("dbo.Professor", t => t.CodProfessor)
                .ForeignKey("dbo.SimDiaRealizacao", t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao })
                .ForeignKey("dbo.Disciplina", t => t.CodDisciplina)
                .Index(t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao })
                .Index(t => t.CodProfessor)
                .Index(t => t.CodDisciplina);
            
            CreateTable(
                "dbo.Professor",
                c => new
                    {
                        CodProfessor = c.Int(nullable: false, identity: true),
                        MatrProfessor = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.CodProfessor)
                .ForeignKey("dbo.Usuario", t => t.MatrProfessor)
                .Index(t => t.MatrProfessor);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Matricula = c.String(nullable: false, maxLength: 20, unicode: false),
                        CodPessoaFisica = c.Int(nullable: false),
                        CodCategoria = c.Int(nullable: false),
                        Senha = c.String(nullable: false, maxLength: 64, fixedLength: true, unicode: false),
                        DtCadastro = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Matricula)
                .ForeignKey("dbo.Categoria", t => t.CodCategoria)
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica)
                .Index(t => t.CodPessoaFisica)
                .Index(t => t.CodCategoria);
            
            CreateTable(
                "dbo.Candidato",
                c => new
                    {
                        CodCandidato = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 255, unicode: false),
                        Cpf = c.String(nullable: false, maxLength: 11, fixedLength: true, unicode: false),
                        RgNumero = c.Int(),
                        RgOrgao = c.String(maxLength: 20, unicode: false),
                        RgDtExpedicao = c.DateTime(storeType: "date"),
                        Email = c.String(nullable: false, maxLength: 200, unicode: false),
                        Senha = c.String(nullable: false, maxLength: 64, fixedLength: true, unicode: false),
                        DtCadastro = c.DateTime(nullable: false),
                        DtNascimento = c.DateTime(storeType: "date"),
                        Sexo = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        Matricula = c.String(maxLength: 20, unicode: false),
                        TelefoneFixo = c.String(maxLength: 20, unicode: false),
                        TelefoneCelular = c.String(maxLength: 20, unicode: false),
                        CodPais = c.Int(),
                        CodEstado = c.Int(),
                        CodMunicipio = c.Int(),
                        FlagAdventista = c.Boolean(),
                        FlagNecessidadeEspecial = c.Boolean(),
                        DescricaoNecessidadeEspecial = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.CodCandidato)
                .ForeignKey("dbo.Municipio", t => new { t.CodPais, t.CodEstado, t.CodMunicipio })
                .ForeignKey("dbo.Usuario", t => t.Matricula)
                .Index(t => t.Matricula)
                .Index(t => new { t.CodPais, t.CodEstado, t.CodMunicipio });
            
            CreateTable(
                "dbo.Municipio",
                c => new
                    {
                        CodPais = c.Int(nullable: false),
                        CodEstado = c.Int(nullable: false),
                        CodMunicipio = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodPais, t.CodEstado, t.CodMunicipio })
                .ForeignKey("dbo.Estado", t => new { t.CodPais, t.CodEstado })
                .Index(t => new { t.CodPais, t.CodEstado });
            
            CreateTable(
                "dbo.Bairro",
                c => new
                    {
                        CodPais = c.Int(nullable: false),
                        CodEstado = c.Int(nullable: false),
                        CodMunicipio = c.Int(nullable: false),
                        CodBairro = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodPais, t.CodEstado, t.CodMunicipio, t.CodBairro })
                .ForeignKey("dbo.Municipio", t => new { t.CodPais, t.CodEstado, t.CodMunicipio })
                .Index(t => new { t.CodPais, t.CodEstado, t.CodMunicipio });
            
            CreateTable(
                "dbo.Endereco",
                c => new
                    {
                        CodPessoa = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        CodMunicipio = c.Int(nullable: false),
                        CodBairro = c.Int(nullable: false),
                        CodPais = c.Int(nullable: false),
                        CodEstado = c.Int(nullable: false),
                        Logradouro = c.String(nullable: false, maxLength: 100, unicode: false),
                        Numero = c.String(nullable: false, maxLength: 10, unicode: false),
                        Complemento = c.String(maxLength: 140, unicode: false),
                        Cep = c.Int(),
                    })
                .PrimaryKey(t => new { t.CodPessoa, t.CodOrdem })
                .ForeignKey("dbo.Pessoa", t => t.CodPessoa)
                .ForeignKey("dbo.Bairro", t => new { t.CodPais, t.CodEstado, t.CodMunicipio, t.CodBairro })
                .Index(t => t.CodPessoa)
                .Index(t => new { t.CodPais, t.CodEstado, t.CodMunicipio, t.CodBairro });
            
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        CodPessoa = c.Int(nullable: false, identity: true),
                        TipoPessoa = c.String(maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.CodPessoa);
            
            CreateTable(
                "dbo.Email",
                c => new
                    {
                        CodPessoa = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        CodTipoContato = c.Int(nullable: false),
                        Email = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodPessoa, t.CodOrdem })
                .ForeignKey("dbo.TipoContato", t => t.CodTipoContato)
                .ForeignKey("dbo.Pessoa", t => t.CodPessoa)
                .Index(t => t.CodPessoa)
                .Index(t => t.CodTipoContato);
            
            CreateTable(
                "dbo.TipoContato",
                c => new
                    {
                        CodTipoContato = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.CodTipoContato);
            
            CreateTable(
                "dbo.Telefone",
                c => new
                    {
                        CodPessoa = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        CodTipoContato = c.Int(nullable: false),
                        CodDDI = c.Int(),
                        CodDDD = c.Int(),
                        Numero = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodPessoa, t.CodOrdem })
                .ForeignKey("dbo.TipoContato", t => t.CodTipoContato)
                .ForeignKey("dbo.Pessoa", t => t.CodPessoa)
                .Index(t => t.CodPessoa)
                .Index(t => t.CodTipoContato);
            
            CreateTable(
                "dbo.PessoaJuridica",
                c => new
                    {
                        CodPessoa = c.Int(nullable: false),
                        RazaoSocial = c.String(maxLength: 255, unicode: false),
                        NomeFantasia = c.String(nullable: false, maxLength: 100, unicode: false),
                        Cnpj = c.String(maxLength: 15, fixedLength: true, unicode: false),
                        Portal = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.CodPessoa)
                .ForeignKey("dbo.Pessoa", t => t.CodPessoa)
                .Index(t => t.CodPessoa);
            
            CreateTable(
                "dbo.Diretoria",
                c => new
                    {
                        CodInstituicao = c.Int(nullable: false),
                        CodCampus = c.Int(nullable: false),
                        CodDiretoria = c.Int(nullable: false),
                        CodPessoaJuridica = c.Int(nullable: false),
                        CodColaboradorDiretor = c.Int(nullable: false),
                        Sigla = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodInstituicao, t.CodCampus, t.CodDiretoria })
                .ForeignKey("dbo.PessoaJuridica", t => t.CodPessoaJuridica)
                .ForeignKey("dbo.Colaborador", t => t.CodColaboradorDiretor)
                .ForeignKey("dbo.Campus", t => new { t.CodInstituicao, t.CodCampus })
                .Index(t => new { t.CodInstituicao, t.CodCampus })
                .Index(t => t.CodPessoaJuridica)
                .Index(t => t.CodColaboradorDiretor);
            
            CreateTable(
                "dbo.PessoaLocalTrabalho",
                c => new
                    {
                        CodPessoa = c.Int(nullable: false),
                        CodInstituicao = c.Int(),
                        CodReitoria = c.Int(),
                        CodProReitoria = c.Int(),
                        CodCampus = c.Int(),
                        CodDiretoria = c.Int(),
                    })
                .PrimaryKey(t => t.CodPessoa)
                .ForeignKey("dbo.Instituicao", t => t.CodInstituicao)
                .ForeignKey("dbo.ProReitoria", t => new { t.CodInstituicao, t.CodProReitoria })
                .ForeignKey("dbo.Reitoria", t => new { t.CodInstituicao, t.CodReitoria })
                .ForeignKey("dbo.Diretoria", t => new { t.CodInstituicao, t.CodCampus, t.CodDiretoria })
                .ForeignKey("dbo.Campus", t => new { t.CodInstituicao, t.CodCampus })
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoa)
                .Index(t => t.CodPessoa)
                .Index(t => t.CodInstituicao)
                .Index(t => new { t.CodInstituicao, t.CodProReitoria })
                .Index(t => new { t.CodInstituicao, t.CodReitoria })
                .Index(t => new { t.CodInstituicao, t.CodCampus, t.CodDiretoria });
            
            CreateTable(
                "dbo.Instituicao",
                c => new
                    {
                        CodInstituicao = c.Int(nullable: false, identity: true),
                        CodPessoaJuridica = c.Int(nullable: false),
                        Sigla = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.CodInstituicao)
                .ForeignKey("dbo.PessoaJuridica", t => t.CodPessoaJuridica)
                .Index(t => t.CodPessoaJuridica);
            
            CreateTable(
                "dbo.ProReitoria",
                c => new
                    {
                        CodInstituicao = c.Int(nullable: false),
                        CodProReitoria = c.Int(nullable: false),
                        CodPessoaJuridica = c.Int(nullable: false),
                        CodColaboradorProReitor = c.Int(nullable: false),
                        Sigla = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodInstituicao, t.CodProReitoria })
                .ForeignKey("dbo.Instituicao", t => t.CodInstituicao)
                .ForeignKey("dbo.PessoaJuridica", t => t.CodPessoaJuridica)
                .ForeignKey("dbo.Colaborador", t => t.CodColaboradorProReitor)
                .Index(t => t.CodInstituicao)
                .Index(t => t.CodPessoaJuridica)
                .Index(t => t.CodColaboradorProReitor);
            
            CreateTable(
                "dbo.Reitoria",
                c => new
                    {
                        CodInstituicao = c.Int(nullable: false),
                        CodReitoria = c.Int(nullable: false),
                        CodPessoaJuridica = c.Int(nullable: false),
                        CodColaboradorReitor = c.Int(nullable: false),
                        Sigla = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodInstituicao, t.CodReitoria })
                .ForeignKey("dbo.Instituicao", t => t.CodInstituicao)
                .ForeignKey("dbo.PessoaJuridica", t => t.CodPessoaJuridica)
                .ForeignKey("dbo.Colaborador", t => t.CodColaboradorReitor)
                .Index(t => t.CodInstituicao)
                .Index(t => t.CodPessoaJuridica)
                .Index(t => t.CodColaboradorReitor);
            
            CreateTable(
                "dbo.Estado",
                c => new
                    {
                        CodPais = c.Int(nullable: false),
                        CodEstado = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        Sigla = c.String(maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodPais, t.CodEstado })
                .ForeignKey("dbo.Pais", t => t.CodPais)
                .Index(t => t.CodPais);
            
            CreateTable(
                "dbo.Pais",
                c => new
                    {
                        CodPais = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 50, unicode: false),
                        Sigla = c.String(maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => t.CodPais);
            
            CreateTable(
                "dbo.SimCandidato",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodCandidato = c.Int(nullable: false),
                        NumInscricao = c.Int(nullable: false),
                        DtInscricao = c.DateTime(nullable: false),
                        CodSala = c.Int(),
                        EscorePadronizadoFinal = c.Decimal(precision: 8, scale: 3),
                        NumeroMascara = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.NumIdentificador, t.CodCandidato })
                .ForeignKey("dbo.SimSala", t => new { t.Ano, t.NumIdentificador, t.CodSala })
                .ForeignKey("dbo.Simulado", t => new { t.Ano, t.NumIdentificador })
                .ForeignKey("dbo.Candidato", t => t.CodCandidato)
                .Index(t => new { t.Ano, t.NumIdentificador, t.CodSala })
                .Index(t => t.CodCandidato);
            
            CreateTable(
                "dbo.SimCandidatoProva",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodCandidato = c.Int(nullable: false),
                        CodDiaRealizacao = c.Int(nullable: false),
                        CodProva = c.Int(nullable: false),
                        NotaDiscursiva = c.Decimal(precision: 8, scale: 3),
                        QteAcertos = c.Int(),
                        FlagPresente = c.Boolean(),
                        EscorePadronizado = c.Decimal(precision: 8, scale: 3),
                        Observacoes = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => new { t.Ano, t.NumIdentificador, t.CodCandidato, t.CodDiaRealizacao, t.CodProva })
                .ForeignKey("dbo.SimCandidato", t => new { t.Ano, t.NumIdentificador, t.CodCandidato })
                .ForeignKey("dbo.SimProva", t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao, t.CodProva })
                .Index(t => new { t.Ano, t.NumIdentificador, t.CodCandidato })
                .Index(t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao, t.CodProva });
            
            CreateTable(
                "dbo.SimCandidatoQuestao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodCandidato = c.Int(nullable: false),
                        CodDiaRealizacao = c.Int(nullable: false),
                        CodProva = c.Int(nullable: false),
                        CodQuestao = c.Int(nullable: false),
                        RespAlternativa = c.Int(),
                        RespDiscursiva = c.String(unicode: false, storeType: "text"),
                        NotaDiscursiva = c.Decimal(precision: 8, scale: 3),
                    })
                .PrimaryKey(t => new { t.Ano, t.NumIdentificador, t.CodCandidato, t.CodDiaRealizacao, t.CodProva, t.CodQuestao })
                .ForeignKey("dbo.SimProvaQuestao", t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao, t.CodProva, t.CodQuestao })
                .ForeignKey("dbo.SimCandidatoProva", t => new { t.Ano, t.NumIdentificador, t.CodCandidato, t.CodDiaRealizacao, t.CodProva })
                .Index(t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao, t.CodProva, t.CodQuestao })
                .Index(t => new { t.Ano, t.NumIdentificador, t.CodCandidato, t.CodDiaRealizacao, t.CodProva });
            
            CreateTable(
                "dbo.SimProvaQuestao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodDiaRealizacao = c.Int(nullable: false),
                        CodProva = c.Int(nullable: false),
                        CodQuestao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao, t.CodProva, t.CodQuestao })
                .ForeignKey("dbo.SimProva", t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao, t.CodProva })
                .ForeignKey("dbo.Questao", t => t.CodQuestao)
                .Index(t => new { t.Ano, t.NumIdentificador, t.CodDiaRealizacao, t.CodProva })
                .Index(t => t.CodQuestao);
            
            CreateTable(
                "dbo.SimSala",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodSala = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.NumIdentificador, t.CodSala })
                .ForeignKey("dbo.Simulado", t => new { t.Ano, t.NumIdentificador })
                .ForeignKey("dbo.Sala", t => t.CodSala)
                .Index(t => new { t.Ano, t.NumIdentificador })
                .Index(t => t.CodSala);
            
            CreateTable(
                "dbo.Simulado",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 250, unicode: false),
                        Descricao = c.String(unicode: false, storeType: "text"),
                        CodColaborador = c.Int(nullable: false),
                        DtInicioInscricao = c.DateTime(),
                        DtTerminoInscricao = c.DateTime(),
                        DtCadastro = c.DateTime(nullable: false),
                        QteVagas = c.Int(),
                        FlagSimuladoEncerrado = c.Boolean(nullable: false),
                        FlagInscricaoEncerrado = c.Boolean(nullable: false),
                        FlagProvaEncerrada = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.NumIdentificador })
                .ForeignKey("dbo.Colaborador", t => t.CodColaborador)
                .Index(t => t.CodColaborador);
            
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        CodCategoria = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                    })
                .PrimaryKey(t => t.CodCategoria);
            
            CreateTable(
                "dbo.UsuarioAcesso",
                c => new
                    {
                        Matricula = c.String(nullable: false, maxLength: 20, unicode: false),
                        CodOrdem = c.Int(nullable: false),
                        DtAcesso = c.DateTime(nullable: false),
                        IpAcesso = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => new { t.Matricula, t.CodOrdem })
                .ForeignKey("dbo.Usuario", t => t.Matricula)
                .Index(t => t.Matricula);
            
            CreateTable(
                "dbo.UsuarioAcessoPagina",
                c => new
                    {
                        Matricula = c.String(nullable: false, maxLength: 20, unicode: false),
                        CodOrdem = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        Pagina = c.String(nullable: false, maxLength: 200, unicode: false),
                        DtAbertura = c.DateTime(nullable: false),
                        PaginaReferencia = c.String(maxLength: 200, unicode: false),
                        Dados = c.String(unicode: false, storeType: "text"),
                    })
                .PrimaryKey(t => new { t.Matricula, t.CodOrdem, t.NumIdentificador })
                .ForeignKey("dbo.UsuarioAcesso", t => new { t.Matricula, t.CodOrdem })
                .Index(t => new { t.Matricula, t.CodOrdem });
            
            CreateTable(
                "dbo.UsuarioOpiniao",
                c => new
                    {
                        Matricula = c.String(nullable: false, maxLength: 20, unicode: false),
                        CodOrdem = c.Int(nullable: false),
                        Opiniao = c.String(nullable: false, unicode: false, storeType: "text"),
                        DtEnvio = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.Matricula, t.CodOrdem })
                .ForeignKey("dbo.Usuario", t => t.Matricula)
                .Index(t => t.Matricula);
            
            CreateTable(
                "dbo.Visitante",
                c => new
                    {
                        CodVisitante = c.Int(nullable: false),
                        MatrVisitante = c.String(nullable: false, maxLength: 20, unicode: false),
                        DtValidade = c.DateTime(),
                    })
                .PrimaryKey(t => t.CodVisitante)
                .ForeignKey("dbo.Usuario", t => t.MatrVisitante)
                .Index(t => t.MatrVisitante);
            
            CreateTable(
                "dbo.MatrizCurricular",
                c => new
                    {
                        CodCurso = c.Int(nullable: false),
                        CodMatriz = c.Int(nullable: false),
                        CargaHoraria = c.Int(),
                    })
                .PrimaryKey(t => new { t.CodCurso, t.CodMatriz })
                .ForeignKey("dbo.Curso", t => t.CodCurso)
                .Index(t => t.CodCurso);
            
            CreateTable(
                "dbo.MatrizCurricularDisciplina",
                c => new
                    {
                        CodCurso = c.Int(nullable: false),
                        Periodo = c.Int(nullable: false),
                        CodMatriz = c.Int(nullable: false),
                        CodDisciplina = c.Int(nullable: false),
                        DiscCargaHoraria = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CodCurso, t.Periodo, t.CodMatriz, t.CodDisciplina })
                .ForeignKey("dbo.MatrizCurricular", t => new { t.CodCurso, t.CodMatriz })
                .ForeignKey("dbo.Disciplina", t => t.CodDisciplina)
                .Index(t => new { t.CodCurso, t.CodMatriz })
                .Index(t => t.CodDisciplina);
            
            CreateTable(
                "dbo.NivelEnsino",
                c => new
                    {
                        CodNivelEnsino = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 40, unicode: false),
                        Sigla = c.String(maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => t.CodNivelEnsino);
            
            CreateTable(
                "dbo.PessoaFormacao",
                c => new
                    {
                        CodPessoaFisica = c.Int(nullable: false),
                        CodArea = c.Int(nullable: false),
                        CodNivelEnsino = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        Curso = c.String(nullable: false, maxLength: 50, unicode: false),
                        Ano = c.Int(),
                    })
                .PrimaryKey(t => new { t.CodPessoaFisica, t.CodArea, t.CodNivelEnsino, t.CodOrdem })
                .ForeignKey("dbo.Area", t => t.CodArea)
                .ForeignKey("dbo.NivelEnsino", t => t.CodNivelEnsino)
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica)
                .Index(t => t.CodPessoaFisica)
                .Index(t => t.CodArea)
                .Index(t => t.CodNivelEnsino);
            
            CreateTable(
                "dbo.Area",
                c => new
                    {
                        CodArea = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.CodArea);
            
            CreateTable(
                "dbo.AviTipoPublico",
                c => new
                    {
                        CodAviTipoPublico = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 35, unicode: false),
                    })
                .PrimaryKey(t => t.CodAviTipoPublico);
            
            CreateTable(
                "dbo.AvalQuesPessoaResposta",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodDisciplina = c.Int(nullable: false),
                        CodTema = c.Int(nullable: false),
                        CodQuestao = c.Int(nullable: false),
                        CodPessoaFisica = c.Int(nullable: false),
                        RespAlternativa = c.Int(),
                        RespDiscursiva = c.String(unicode: false, storeType: "text"),
                        RespComentario = c.String(maxLength: 250, unicode: false),
                        RespNota = c.Double(),
                        ProfObservacao = c.String(maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodDisciplina, t.CodTema, t.CodQuestao, t.CodPessoaFisica })
                .ForeignKey("dbo.AvalTemaQuestao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodDisciplina, t.CodTema, t.CodQuestao })
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica)
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodDisciplina, t.CodTema, t.CodQuestao })
                .Index(t => t.CodPessoaFisica);
            
            CreateTable(
                "dbo.AvalTemaQuestao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodDisciplina = c.Int(nullable: false),
                        CodTema = c.Int(nullable: false),
                        CodQuestao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodDisciplina, t.CodTema, t.CodQuestao })
                .ForeignKey("dbo.AvaliacaoTema", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodDisciplina, t.CodTema })
                .ForeignKey("dbo.QuestaoTema", t => new { t.CodDisciplina, t.CodTema, t.CodQuestao })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodDisciplina, t.CodTema })
                .Index(t => new { t.CodDisciplina, t.CodTema, t.CodQuestao });
            
            CreateTable(
                "dbo.AvaliacaoTema",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodDisciplina = c.Int(nullable: false),
                        CodTema = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodDisciplina, t.CodTema })
                .ForeignKey("dbo.Tema", t => new { t.CodDisciplina, t.CodTema })
                .ForeignKey("dbo.Avaliacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.CodDisciplina, t.CodTema });
            
            CreateTable(
                "dbo.Tema",
                c => new
                    {
                        CodDisciplina = c.Int(nullable: false),
                        CodTema = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 100, unicode: false),
                        Comentario = c.String(maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodDisciplina, t.CodTema })
                .ForeignKey("dbo.Disciplina", t => t.CodDisciplina)
                .Index(t => t.CodDisciplina);
            
            CreateTable(
                "dbo.QuestaoTema",
                c => new
                    {
                        CodDisciplina = c.Int(nullable: false),
                        CodTema = c.Int(nullable: false),
                        CodQuestao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CodDisciplina, t.CodTema, t.CodQuestao })
                .ForeignKey("dbo.Tema", t => new { t.CodDisciplina, t.CodTema })
                .ForeignKey("dbo.Questao", t => t.CodQuestao)
                .Index(t => new { t.CodDisciplina, t.CodTema })
                .Index(t => t.CodQuestao);
            
            CreateTable(
                "dbo.Ocupacao",
                c => new
                    {
                        CodOcupacao = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 40, unicode: false),
                    })
                .PrimaryKey(t => t.CodOcupacao);
            
            CreateTable(
                "dbo.AvaliacaoProrrogacao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        DtProrrogacao = c.DateTime(nullable: false),
                        DuracaoAnterior = c.Int(),
                        DuracaoNova = c.Int(),
                        Observacao = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.DtProrrogacao })
                .ForeignKey("dbo.Avaliacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador });
            
            CreateTable(
                "dbo.TipoAvaliacao",
                c => new
                    {
                        CodTipoAvaliacao = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 30, unicode: false),
                        Sigla = c.String(maxLength: 4, unicode: false),
                    })
                .PrimaryKey(t => t.CodTipoAvaliacao);
            
            CreateTable(
                "dbo.QuestaoAnexo",
                c => new
                    {
                        CodQuestao = c.Int(nullable: false),
                        CodOrdem = c.Int(nullable: false),
                        CodTipoAnexo = c.Int(nullable: false),
                        Anexo = c.Binary(nullable: false, storeType: "image"),
                        Legenda = c.String(maxLength: 250, unicode: false),
                        Fonte = c.String(maxLength: 250, unicode: false),
                    })
                .PrimaryKey(t => new { t.CodQuestao, t.CodOrdem, t.CodTipoAnexo })
                .ForeignKey("dbo.TipoAnexo", t => t.CodTipoAnexo)
                .ForeignKey("dbo.Questao", t => t.CodQuestao)
                .Index(t => t.CodQuestao)
                .Index(t => t.CodTipoAnexo);
            
            CreateTable(
                "dbo.TipoAnexo",
                c => new
                    {
                        CodTipoAnexo = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.CodTipoAnexo);
            
            CreateTable(
                "dbo.TipoQuestao",
                c => new
                    {
                        CodTipoQuestao = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 15, unicode: false),
                        Sigla = c.String(maxLength: 4, unicode: false),
                    })
                .PrimaryKey(t => t.CodTipoQuestao);
            
            CreateTable(
                "dbo.Parametro",
                c => new
                    {
                        CodParametro = c.Int(nullable: false, identity: true),
                        TempoInatividade = c.Int(nullable: false),
                        NumeracaoQuestao = c.Int(nullable: false),
                        NumeracaoAlternativa = c.Int(nullable: false),
                        QteSemestres = c.Int(nullable: false),
                        PeriodoLetivoAnoAtual = c.Int(nullable: false),
                        PeriodoLetivoSemestreAtual = c.Int(nullable: false),
                        TermoResponsabilidade = c.String(nullable: false, unicode: false, storeType: "text"),
                        NotaUsoAcademica = c.String(nullable: false, unicode: false, storeType: "text"),
                        ValorNotaMedia = c.Double(nullable: false),
                        NotaUsoReposicao = c.String(nullable: false, unicode: false, storeType: "text"),
                        NotaUsoCertificacao = c.String(nullable: false, unicode: false, storeType: "text"),
                        NotaUsoInstitucional = c.String(nullable: false, unicode: false, storeType: "text"),
                        CoordenadorAVI = c.String(nullable: false, unicode: false, storeType: "text"),
                        NotaUsoSimulado = c.String(nullable: false, unicode: false, storeType: "text"),
                        SmtpEnderecoHost = c.String(nullable: false, maxLength: 200, unicode: false),
                        SmtpPorta = c.Int(nullable: false),
                        SmtpUsuario = c.String(nullable: false, maxLength: 200, unicode: false),
                        SmtpSenha = c.String(nullable: false, maxLength: 200, unicode: false),
                        SmtpFlagSSL = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CodParametro);
            
            CreateTable(
                "dbo.AvalCertPessoa",
                c => new
                    {
                        CodPessoaFisica = c.Int(nullable: false),
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CodPessoaFisica, t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica, cascadeDelete: true)
                .ForeignKey("dbo.AvalCertificacao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador }, cascadeDelete: true)
                .Index(t => t.CodPessoaFisica)
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador });
            
            CreateTable(
                "dbo.AvalAcadRepoJustificacao",
                c => new
                    {
                        Ano = c.Int(nullable: false),
                        Semestre = c.Int(nullable: false),
                        CodTipoAvaliacao = c.Int(nullable: false),
                        NumIdentificador = c.Int(nullable: false),
                        CodProfessor = c.Int(nullable: false),
                        CodJustificacao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador, t.CodProfessor, t.CodJustificacao })
                .ForeignKey("dbo.AvalAcadReposicao", t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador }, cascadeDelete: true)
                .ForeignKey("dbo.Justificacao", t => new { t.CodProfessor, t.CodJustificacao }, cascadeDelete: true)
                .Index(t => new { t.Ano, t.Semestre, t.CodTipoAvaliacao, t.NumIdentificador })
                .Index(t => new { t.CodProfessor, t.CodJustificacao });
            
            CreateTable(
                "dbo.PessoaCategoria",
                c => new
                    {
                        CodCategoria = c.Int(nullable: false),
                        CodPessoaFisica = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CodCategoria, t.CodPessoaFisica })
                .ForeignKey("dbo.Categoria", t => t.CodCategoria, cascadeDelete: true)
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica, cascadeDelete: true)
                .Index(t => t.CodCategoria)
                .Index(t => t.CodPessoaFisica);
            
            CreateTable(
                "dbo.PessoaOcupacao",
                c => new
                    {
                        CodOcupacao = c.Int(nullable: false),
                        CodPessoaFisica = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CodOcupacao, t.CodPessoaFisica })
                .ForeignKey("dbo.Ocupacao", t => t.CodOcupacao, cascadeDelete: true)
                .ForeignKey("dbo.PessoaFisica", t => t.CodPessoaFisica, cascadeDelete: true)
                .Index(t => t.CodOcupacao)
                .Index(t => t.CodPessoaFisica);
            
            CreateTable(
                "dbo.ProfessorDisciplina",
                c => new
                    {
                        CodDisciplina = c.Int(nullable: false),
                        CodProfessor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CodDisciplina, t.CodProfessor })
                .ForeignKey("dbo.Disciplina", t => t.CodDisciplina, cascadeDelete: true)
                .ForeignKey("dbo.Professor", t => t.CodProfessor, cascadeDelete: true)
                .Index(t => t.CodDisciplina)
                .Index(t => t.CodProfessor);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questao", "CodTipoQuestao", "dbo.TipoQuestao");
            DropForeignKey("dbo.SimProvaQuestao", "CodQuestao", "dbo.Questao");
            DropForeignKey("dbo.QuestaoTema", "CodQuestao", "dbo.Questao");
            DropForeignKey("dbo.QuestaoAnexo", "CodQuestao", "dbo.Questao");
            DropForeignKey("dbo.QuestaoAnexo", "CodTipoAnexo", "dbo.TipoAnexo");
            DropForeignKey("dbo.Questao", "CodDificuldade", "dbo.Dificuldade");
            DropForeignKey("dbo.AvalAuto", "CodDificuldade", "dbo.Dificuldade");
            DropForeignKey("dbo.Avaliacao", "CodTipoAvaliacao", "dbo.TipoAvaliacao");
            DropForeignKey("dbo.AvalPessoaResultado", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.Avaliacao");
            DropForeignKey("dbo.AvaliacaoTema", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.Avaliacao");
            DropForeignKey("dbo.AvaliacaoProrrogacao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.Avaliacao");
            DropForeignKey("dbo.AvalCertificacao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.Avaliacao");
            DropForeignKey("dbo.AvalAvi", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.Avaliacao");
            DropForeignKey("dbo.AvalAuto", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.Avaliacao");
            DropForeignKey("dbo.AvalAcadReposicao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.Avaliacao");
            DropForeignKey("dbo.AvalAcademica", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.Avaliacao");
            DropForeignKey("dbo.TurmaDiscProfHorario", "CodDisciplina", "dbo.Disciplina");
            DropForeignKey("dbo.TurmaDiscAluno", "CodDisciplina", "dbo.Disciplina");
            DropForeignKey("dbo.Tema", "CodDisciplina", "dbo.Disciplina");
            DropForeignKey("dbo.SimProva", "CodDisciplina", "dbo.Disciplina");
            DropForeignKey("dbo.ProfessorDisciplina", "CodProfessor", "dbo.Professor");
            DropForeignKey("dbo.ProfessorDisciplina", "CodDisciplina", "dbo.Disciplina");
            DropForeignKey("dbo.MatrizCurricularDisciplina", "CodDisciplina", "dbo.Disciplina");
            DropForeignKey("dbo.AvalCertificacao", "CodDisciplina", "dbo.Disciplina");
            DropForeignKey("dbo.Usuario", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.PessoaLocalTrabalho", "CodPessoa", "dbo.PessoaFisica");
            DropForeignKey("dbo.PessoaFormacao", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.PessoaOcupacao", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.PessoaOcupacao", "CodOcupacao", "dbo.Ocupacao");
            DropForeignKey("dbo.AviQuestaoPessoaResposta", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.AvalQuesPessoaResposta", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.AvalQuesPessoaResposta", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodDisciplina", "CodTema", "CodQuestao" }, "dbo.AvalTemaQuestao");
            DropForeignKey("dbo.QuestaoTema", new[] { "CodDisciplina", "CodTema" }, "dbo.Tema");
            DropForeignKey("dbo.AvalTemaQuestao", new[] { "CodDisciplina", "CodTema", "CodQuestao" }, "dbo.QuestaoTema");
            DropForeignKey("dbo.AvaliacaoTema", new[] { "CodDisciplina", "CodTema" }, "dbo.Tema");
            DropForeignKey("dbo.AvalTemaQuestao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodDisciplina", "CodTema" }, "dbo.AvaliacaoTema");
            DropForeignKey("dbo.AvalPessoaResultado", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.Justificacao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodPessoaFisica" }, "dbo.AvalPessoaResultado");
            DropForeignKey("dbo.TurmaDiscProfHorario", "CodSala", "dbo.Sala");
            DropForeignKey("dbo.SimSala", "CodSala", "dbo.Sala");
            DropForeignKey("dbo.Sala", "CodBloco", "dbo.Bloco");
            DropForeignKey("dbo.PessoaLocalTrabalho", new[] { "CodInstituicao", "CodCampus" }, "dbo.Campus");
            DropForeignKey("dbo.Diretoria", new[] { "CodInstituicao", "CodCampus" }, "dbo.Campus");
            DropForeignKey("dbo.Bloco", new[] { "CodInstituicao", "CodCampus" }, "dbo.Campus");
            DropForeignKey("dbo.AviPublico", "PessoaFisica_CodPessoa", "dbo.PessoaFisica");
            DropForeignKey("dbo.AviPublico", new[] { "Campus_CodInstituicao", "Campus_CodCampus" }, "dbo.Campus");
            DropForeignKey("dbo.AviPublico", "CodAviTipoPublico", "dbo.AviTipoPublico");
            DropForeignKey("dbo.Simulado", "CodColaborador", "dbo.Colaborador");
            DropForeignKey("dbo.Reitoria", "CodColaboradorReitor", "dbo.Colaborador");
            DropForeignKey("dbo.ProReitoria", "CodColaboradorProReitor", "dbo.Colaborador");
            DropForeignKey("dbo.Diretoria", "CodColaboradorDiretor", "dbo.Colaborador");
            DropForeignKey("dbo.Curso", "CodColabCoordenador", "dbo.Colaborador");
            DropForeignKey("dbo.Turma", "CodCurso", "dbo.Curso");
            DropForeignKey("dbo.PessoaFormacao", "CodNivelEnsino", "dbo.NivelEnsino");
            DropForeignKey("dbo.PessoaFormacao", "CodArea", "dbo.Area");
            DropForeignKey("dbo.Curso", "CodNivelEnsino", "dbo.NivelEnsino");
            DropForeignKey("dbo.MatrizCurricular", "CodCurso", "dbo.Curso");
            DropForeignKey("dbo.MatrizCurricularDisciplina", new[] { "CodCurso", "CodMatriz" }, "dbo.MatrizCurricular");
            DropForeignKey("dbo.AviPublico", "Curso_CodCurso", "dbo.Curso");
            DropForeignKey("dbo.Aluno", "CodCurso", "dbo.Curso");
            DropForeignKey("dbo.TurmaDiscAluno", "CodAluno", "dbo.Aluno");
            DropForeignKey("dbo.TurmaDiscProfHorario", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" }, "dbo.Turma");
            DropForeignKey("dbo.Turma", "CodTurno", "dbo.Turno");
            DropForeignKey("dbo.SimDiaRealizacao", "CodTurno", "dbo.Turno");
            DropForeignKey("dbo.SimProva", new[] { "Ano", "NumIdentificador", "CodDiaRealizacao" }, "dbo.SimDiaRealizacao");
            DropForeignKey("dbo.SimProvaQuestao", new[] { "Ano", "NumIdentificador", "CodDiaRealizacao", "CodProva" }, "dbo.SimProva");
            DropForeignKey("dbo.SimCandidatoProva", new[] { "Ano", "NumIdentificador", "CodDiaRealizacao", "CodProva" }, "dbo.SimProva");
            DropForeignKey("dbo.Visitante", "MatrVisitante", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioOpiniao", "Matricula", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioAcesso", "Matricula", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioAcessoPagina", new[] { "Matricula", "CodOrdem" }, "dbo.UsuarioAcesso");
            DropForeignKey("dbo.Professor", "MatrProfessor", "dbo.Usuario");
            DropForeignKey("dbo.Colaborador", "MatrColaborador", "dbo.Usuario");
            DropForeignKey("dbo.Usuario", "CodCategoria", "dbo.Categoria");
            DropForeignKey("dbo.PessoaCategoria", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.PessoaCategoria", "CodCategoria", "dbo.Categoria");
            DropForeignKey("dbo.Candidato", "Matricula", "dbo.Usuario");
            DropForeignKey("dbo.SimCandidato", "CodCandidato", "dbo.Candidato");
            DropForeignKey("dbo.SimSala", new[] { "Ano", "NumIdentificador" }, "dbo.Simulado");
            DropForeignKey("dbo.SimDiaRealizacao", new[] { "Ano", "NumIdentificador" }, "dbo.Simulado");
            DropForeignKey("dbo.SimCandidato", new[] { "Ano", "NumIdentificador" }, "dbo.Simulado");
            DropForeignKey("dbo.SimCandidato", new[] { "Ano", "NumIdentificador", "CodSala" }, "dbo.SimSala");
            DropForeignKey("dbo.SimCandidatoProva", new[] { "Ano", "NumIdentificador", "CodCandidato" }, "dbo.SimCandidato");
            DropForeignKey("dbo.SimCandidatoQuestao", new[] { "Ano", "NumIdentificador", "CodCandidato", "CodDiaRealizacao", "CodProva" }, "dbo.SimCandidatoProva");
            DropForeignKey("dbo.SimCandidatoQuestao", new[] { "Ano", "NumIdentificador", "CodDiaRealizacao", "CodProva", "CodQuestao" }, "dbo.SimProvaQuestao");
            DropForeignKey("dbo.Estado", "CodPais", "dbo.Pais");
            DropForeignKey("dbo.Municipio", new[] { "CodPais", "CodEstado" }, "dbo.Estado");
            DropForeignKey("dbo.Candidato", new[] { "CodPais", "CodEstado", "CodMunicipio" }, "dbo.Municipio");
            DropForeignKey("dbo.Bairro", new[] { "CodPais", "CodEstado", "CodMunicipio" }, "dbo.Municipio");
            DropForeignKey("dbo.Endereco", new[] { "CodPais", "CodEstado", "CodMunicipio", "CodBairro" }, "dbo.Bairro");
            DropForeignKey("dbo.Telefone", "CodPessoa", "dbo.Pessoa");
            DropForeignKey("dbo.PessoaJuridica", "CodPessoa", "dbo.Pessoa");
            DropForeignKey("dbo.Reitoria", "CodPessoaJuridica", "dbo.PessoaJuridica");
            DropForeignKey("dbo.ProReitoria", "CodPessoaJuridica", "dbo.PessoaJuridica");
            DropForeignKey("dbo.Instituicao", "CodPessoaJuridica", "dbo.PessoaJuridica");
            DropForeignKey("dbo.Diretoria", "CodPessoaJuridica", "dbo.PessoaJuridica");
            DropForeignKey("dbo.PessoaLocalTrabalho", new[] { "CodInstituicao", "CodCampus", "CodDiretoria" }, "dbo.Diretoria");
            DropForeignKey("dbo.Reitoria", "CodInstituicao", "dbo.Instituicao");
            DropForeignKey("dbo.PessoaLocalTrabalho", new[] { "CodInstituicao", "CodReitoria" }, "dbo.Reitoria");
            DropForeignKey("dbo.AviPublico", new[] { "Reitoria_CodInstituicao", "Reitoria_CodReitoria" }, "dbo.Reitoria");
            DropForeignKey("dbo.ProReitoria", "CodInstituicao", "dbo.Instituicao");
            DropForeignKey("dbo.PessoaLocalTrabalho", new[] { "CodInstituicao", "CodProReitoria" }, "dbo.ProReitoria");
            DropForeignKey("dbo.AviPublico", new[] { "ProReitoria_CodInstituicao", "ProReitoria_CodProReitoria" }, "dbo.ProReitoria");
            DropForeignKey("dbo.PessoaLocalTrabalho", "CodInstituicao", "dbo.Instituicao");
            DropForeignKey("dbo.Campus", "CodInstituicao", "dbo.Instituicao");
            DropForeignKey("dbo.AviPublico", "Instituicao_CodInstituicao", "dbo.Instituicao");
            DropForeignKey("dbo.Curso", new[] { "CodInstituicao", "CodCampus", "CodDiretoria" }, "dbo.Diretoria");
            DropForeignKey("dbo.AviPublico", new[] { "Diretoria_CodInstituicao", "Diretoria_CodCampus", "Diretoria_CodDiretoria" }, "dbo.Diretoria");
            DropForeignKey("dbo.Campus", "CodPessoaJuridica", "dbo.PessoaJuridica");
            DropForeignKey("dbo.PessoaFisica", "CodPessoa", "dbo.Pessoa");
            DropForeignKey("dbo.Endereco", "CodPessoa", "dbo.Pessoa");
            DropForeignKey("dbo.Email", "CodPessoa", "dbo.Pessoa");
            DropForeignKey("dbo.Telefone", "CodTipoContato", "dbo.TipoContato");
            DropForeignKey("dbo.Email", "CodTipoContato", "dbo.TipoContato");
            DropForeignKey("dbo.Aluno", "MatrAluno", "dbo.Usuario");
            DropForeignKey("dbo.TurmaDiscProfHorario", "CodProfessor", "dbo.Professor");
            DropForeignKey("dbo.SimProva", "CodProfessor", "dbo.Professor");
            DropForeignKey("dbo.Questao", "CodProfessor", "dbo.Professor");
            DropForeignKey("dbo.Justificacao", "CodProfessor", "dbo.Professor");
            DropForeignKey("dbo.AvalCertificacao", "CodProfessor", "dbo.Professor");
            DropForeignKey("dbo.AvalAcademica", "CodProfessor", "dbo.Professor");
            DropForeignKey("dbo.Horario", "CodTurno", "dbo.Turno");
            DropForeignKey("dbo.TurmaDiscProfHorario", new[] { "CodGrupo", "CodTurno", "CodHorario" }, "dbo.Horario");
            DropForeignKey("dbo.TurmaDiscProfHorario", "CodDia", "dbo.DiaSemana");
            DropForeignKey("dbo.TurmaDiscAluno", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" }, "dbo.Turma");
            DropForeignKey("dbo.AviPublico", new[] { "Turma_CodCurso", "Turma_Periodo", "Turma_CodTurno", "Turma_NumTurma" }, "dbo.Turma");
            DropForeignKey("dbo.AvalAcademica", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" }, "dbo.Turma");
            DropForeignKey("dbo.Campus", "CodColaboradorDiretor", "dbo.Colaborador");
            DropForeignKey("dbo.AvalAvi", "CodColabCoordenador", "dbo.Colaborador");
            DropForeignKey("dbo.AviQuestao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.AvalAvi");
            DropForeignKey("dbo.AviQuestaoPessoaResposta", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodAviModulo", "CodAviCategoria", "CodAviIndicador", "CodOrdem" }, "dbo.AviQuestao");
            DropForeignKey("dbo.AviQuestaoAlternativa", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodAviModulo", "CodAviCategoria", "CodAviIndicador", "CodOrdem" }, "dbo.AviQuestao");
            DropForeignKey("dbo.AviQuestao", "CodAviModulo", "dbo.AviModulo");
            DropForeignKey("dbo.AviQuestao", "CodAviIndicador", "dbo.AviIndicador");
            DropForeignKey("dbo.AviQuestao", "CodAviCategoria", "dbo.AviCategoria");
            DropForeignKey("dbo.AviPublico", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.AvalAvi");
            DropForeignKey("dbo.AvalCertificacao", "CodSala", "dbo.Sala");
            DropForeignKey("dbo.AvalAcadReposicao", "CodSala", "dbo.Sala");
            DropForeignKey("dbo.AvalAcademica", "CodSala", "dbo.Sala");
            DropForeignKey("dbo.AvalAcadRepoJustificacao", new[] { "CodProfessor", "CodJustificacao" }, "dbo.Justificacao");
            DropForeignKey("dbo.AvalAcadRepoJustificacao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.AvalAcadReposicao");
            DropForeignKey("dbo.AvalCertPessoa", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }, "dbo.AvalCertificacao");
            DropForeignKey("dbo.AvalCertPessoa", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.AvalAuto", "CodPessoaFisica", "dbo.PessoaFisica");
            DropForeignKey("dbo.AvalAcademica", "CodDisciplina", "dbo.Disciplina");
            DropForeignKey("dbo.Alternativa", "CodQuestao", "dbo.Questao");
            DropIndex("dbo.ProfessorDisciplina", new[] { "CodProfessor" });
            DropIndex("dbo.ProfessorDisciplina", new[] { "CodDisciplina" });
            DropIndex("dbo.PessoaOcupacao", new[] { "CodPessoaFisica" });
            DropIndex("dbo.PessoaOcupacao", new[] { "CodOcupacao" });
            DropIndex("dbo.PessoaCategoria", new[] { "CodPessoaFisica" });
            DropIndex("dbo.PessoaCategoria", new[] { "CodCategoria" });
            DropIndex("dbo.AvalAcadRepoJustificacao", new[] { "CodProfessor", "CodJustificacao" });
            DropIndex("dbo.AvalAcadRepoJustificacao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.AvalCertPessoa", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.AvalCertPessoa", new[] { "CodPessoaFisica" });
            DropIndex("dbo.QuestaoAnexo", new[] { "CodTipoAnexo" });
            DropIndex("dbo.QuestaoAnexo", new[] { "CodQuestao" });
            DropIndex("dbo.AvaliacaoProrrogacao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.QuestaoTema", new[] { "CodQuestao" });
            DropIndex("dbo.QuestaoTema", new[] { "CodDisciplina", "CodTema" });
            DropIndex("dbo.Tema", new[] { "CodDisciplina" });
            DropIndex("dbo.AvaliacaoTema", new[] { "CodDisciplina", "CodTema" });
            DropIndex("dbo.AvaliacaoTema", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.AvalTemaQuestao", new[] { "CodDisciplina", "CodTema", "CodQuestao" });
            DropIndex("dbo.AvalTemaQuestao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodDisciplina", "CodTema" });
            DropIndex("dbo.AvalQuesPessoaResposta", new[] { "CodPessoaFisica" });
            DropIndex("dbo.AvalQuesPessoaResposta", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodDisciplina", "CodTema", "CodQuestao" });
            DropIndex("dbo.PessoaFormacao", new[] { "CodNivelEnsino" });
            DropIndex("dbo.PessoaFormacao", new[] { "CodArea" });
            DropIndex("dbo.PessoaFormacao", new[] { "CodPessoaFisica" });
            DropIndex("dbo.MatrizCurricularDisciplina", new[] { "CodDisciplina" });
            DropIndex("dbo.MatrizCurricularDisciplina", new[] { "CodCurso", "CodMatriz" });
            DropIndex("dbo.MatrizCurricular", new[] { "CodCurso" });
            DropIndex("dbo.Visitante", new[] { "MatrVisitante" });
            DropIndex("dbo.UsuarioOpiniao", new[] { "Matricula" });
            DropIndex("dbo.UsuarioAcessoPagina", new[] { "Matricula", "CodOrdem" });
            DropIndex("dbo.UsuarioAcesso", new[] { "Matricula" });
            DropIndex("dbo.Simulado", new[] { "CodColaborador" });
            DropIndex("dbo.SimSala", new[] { "CodSala" });
            DropIndex("dbo.SimSala", new[] { "Ano", "NumIdentificador" });
            DropIndex("dbo.SimProvaQuestao", new[] { "CodQuestao" });
            DropIndex("dbo.SimProvaQuestao", new[] { "Ano", "NumIdentificador", "CodDiaRealizacao", "CodProva" });
            DropIndex("dbo.SimCandidatoQuestao", new[] { "Ano", "NumIdentificador", "CodCandidato", "CodDiaRealizacao", "CodProva" });
            DropIndex("dbo.SimCandidatoQuestao", new[] { "Ano", "NumIdentificador", "CodDiaRealizacao", "CodProva", "CodQuestao" });
            DropIndex("dbo.SimCandidatoProva", new[] { "Ano", "NumIdentificador", "CodDiaRealizacao", "CodProva" });
            DropIndex("dbo.SimCandidatoProva", new[] { "Ano", "NumIdentificador", "CodCandidato" });
            DropIndex("dbo.SimCandidato", new[] { "CodCandidato" });
            DropIndex("dbo.SimCandidato", new[] { "Ano", "NumIdentificador", "CodSala" });
            DropIndex("dbo.Estado", new[] { "CodPais" });
            DropIndex("dbo.Reitoria", new[] { "CodColaboradorReitor" });
            DropIndex("dbo.Reitoria", new[] { "CodPessoaJuridica" });
            DropIndex("dbo.Reitoria", new[] { "CodInstituicao" });
            DropIndex("dbo.ProReitoria", new[] { "CodColaboradorProReitor" });
            DropIndex("dbo.ProReitoria", new[] { "CodPessoaJuridica" });
            DropIndex("dbo.ProReitoria", new[] { "CodInstituicao" });
            DropIndex("dbo.Instituicao", new[] { "CodPessoaJuridica" });
            DropIndex("dbo.PessoaLocalTrabalho", new[] { "CodInstituicao", "CodCampus", "CodDiretoria" });
            DropIndex("dbo.PessoaLocalTrabalho", new[] { "CodInstituicao", "CodReitoria" });
            DropIndex("dbo.PessoaLocalTrabalho", new[] { "CodInstituicao", "CodProReitoria" });
            DropIndex("dbo.PessoaLocalTrabalho", new[] { "CodInstituicao" });
            DropIndex("dbo.PessoaLocalTrabalho", new[] { "CodPessoa" });
            DropIndex("dbo.Diretoria", new[] { "CodColaboradorDiretor" });
            DropIndex("dbo.Diretoria", new[] { "CodPessoaJuridica" });
            DropIndex("dbo.Diretoria", new[] { "CodInstituicao", "CodCampus" });
            DropIndex("dbo.PessoaJuridica", new[] { "CodPessoa" });
            DropIndex("dbo.Telefone", new[] { "CodTipoContato" });
            DropIndex("dbo.Telefone", new[] { "CodPessoa" });
            DropIndex("dbo.Email", new[] { "CodTipoContato" });
            DropIndex("dbo.Email", new[] { "CodPessoa" });
            DropIndex("dbo.Endereco", new[] { "CodPais", "CodEstado", "CodMunicipio", "CodBairro" });
            DropIndex("dbo.Endereco", new[] { "CodPessoa" });
            DropIndex("dbo.Bairro", new[] { "CodPais", "CodEstado", "CodMunicipio" });
            DropIndex("dbo.Municipio", new[] { "CodPais", "CodEstado" });
            DropIndex("dbo.Candidato", new[] { "CodPais", "CodEstado", "CodMunicipio" });
            DropIndex("dbo.Candidato", new[] { "Matricula" });
            DropIndex("dbo.Usuario", new[] { "CodCategoria" });
            DropIndex("dbo.Usuario", new[] { "CodPessoaFisica" });
            DropIndex("dbo.Professor", new[] { "MatrProfessor" });
            DropIndex("dbo.SimProva", new[] { "CodDisciplina" });
            DropIndex("dbo.SimProva", new[] { "CodProfessor" });
            DropIndex("dbo.SimProva", new[] { "Ano", "NumIdentificador", "CodDiaRealizacao" });
            DropIndex("dbo.SimDiaRealizacao", new[] { "CodTurno" });
            DropIndex("dbo.SimDiaRealizacao", new[] { "Ano", "NumIdentificador" });
            DropIndex("dbo.Horario", new[] { "CodTurno" });
            DropIndex("dbo.TurmaDiscProfHorario", new[] { "CodSala" });
            DropIndex("dbo.TurmaDiscProfHorario", new[] { "CodDia" });
            DropIndex("dbo.TurmaDiscProfHorario", new[] { "CodProfessor" });
            DropIndex("dbo.TurmaDiscProfHorario", new[] { "CodDisciplina" });
            DropIndex("dbo.TurmaDiscProfHorario", new[] { "CodGrupo", "CodTurno", "CodHorario" });
            DropIndex("dbo.TurmaDiscProfHorario", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" });
            DropIndex("dbo.Turma", new[] { "CodTurno" });
            DropIndex("dbo.Turma", new[] { "CodCurso" });
            DropIndex("dbo.TurmaDiscAluno", new[] { "CodAluno" });
            DropIndex("dbo.TurmaDiscAluno", new[] { "CodDisciplina" });
            DropIndex("dbo.TurmaDiscAluno", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" });
            DropIndex("dbo.Aluno", new[] { "MatrAluno" });
            DropIndex("dbo.Aluno", new[] { "CodCurso" });
            DropIndex("dbo.Curso", new[] { "CodInstituicao", "CodCampus", "CodDiretoria" });
            DropIndex("dbo.Curso", new[] { "CodNivelEnsino" });
            DropIndex("dbo.Curso", new[] { "CodColabCoordenador" });
            DropIndex("dbo.Colaborador", new[] { "MatrColaborador" });
            DropIndex("dbo.AviQuestaoPessoaResposta", new[] { "CodPessoaFisica" });
            DropIndex("dbo.AviQuestaoPessoaResposta", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodAviModulo", "CodAviCategoria", "CodAviIndicador", "CodOrdem" });
            DropIndex("dbo.AviQuestaoAlternativa", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodAviModulo", "CodAviCategoria", "CodAviIndicador", "CodOrdem" });
            DropIndex("dbo.AviQuestao", new[] { "CodAviIndicador" });
            DropIndex("dbo.AviQuestao", new[] { "CodAviCategoria" });
            DropIndex("dbo.AviQuestao", new[] { "CodAviModulo" });
            DropIndex("dbo.AviQuestao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.AvalAvi", new[] { "CodColabCoordenador" });
            DropIndex("dbo.AvalAvi", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.AviPublico", new[] { "PessoaFisica_CodPessoa" });
            DropIndex("dbo.AviPublico", new[] { "Campus_CodInstituicao", "Campus_CodCampus" });
            DropIndex("dbo.AviPublico", new[] { "Curso_CodCurso" });
            DropIndex("dbo.AviPublico", new[] { "Reitoria_CodInstituicao", "Reitoria_CodReitoria" });
            DropIndex("dbo.AviPublico", new[] { "ProReitoria_CodInstituicao", "ProReitoria_CodProReitoria" });
            DropIndex("dbo.AviPublico", new[] { "Instituicao_CodInstituicao" });
            DropIndex("dbo.AviPublico", new[] { "Diretoria_CodInstituicao", "Diretoria_CodCampus", "Diretoria_CodDiretoria" });
            DropIndex("dbo.AviPublico", new[] { "Turma_CodCurso", "Turma_Periodo", "Turma_CodTurno", "Turma_NumTurma" });
            DropIndex("dbo.AviPublico", new[] { "CodAviTipoPublico" });
            DropIndex("dbo.AviPublico", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.Campus", new[] { "CodColaboradorDiretor" });
            DropIndex("dbo.Campus", new[] { "CodPessoaJuridica" });
            DropIndex("dbo.Campus", new[] { "CodInstituicao" });
            DropIndex("dbo.Bloco", new[] { "CodInstituicao", "CodCampus" });
            DropIndex("dbo.Sala", new[] { "CodBloco" });
            DropIndex("dbo.AvalAcadReposicao", new[] { "CodSala" });
            DropIndex("dbo.AvalAcadReposicao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.Justificacao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador", "CodPessoaFisica" });
            DropIndex("dbo.Justificacao", new[] { "CodProfessor" });
            DropIndex("dbo.AvalPessoaResultado", new[] { "CodPessoaFisica" });
            DropIndex("dbo.AvalPessoaResultado", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.PessoaFisica", new[] { "CodPessoa" });
            DropIndex("dbo.AvalCertificacao", new[] { "CodDisciplina" });
            DropIndex("dbo.AvalCertificacao", new[] { "CodProfessor" });
            DropIndex("dbo.AvalCertificacao", new[] { "CodSala" });
            DropIndex("dbo.AvalCertificacao", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.AvalAcademica", new[] { "CodDisciplina" });
            DropIndex("dbo.AvalAcademica", new[] { "CodProfessor" });
            DropIndex("dbo.AvalAcademica", new[] { "CodSala" });
            DropIndex("dbo.AvalAcademica", new[] { "CodCurso", "Periodo", "CodTurno", "NumTurma" });
            DropIndex("dbo.AvalAcademica", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.Avaliacao", new[] { "CodTipoAvaliacao" });
            DropIndex("dbo.AvalAuto", new[] { "CodDificuldade" });
            DropIndex("dbo.AvalAuto", new[] { "CodPessoaFisica" });
            DropIndex("dbo.AvalAuto", new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" });
            DropIndex("dbo.Questao", new[] { "CodTipoQuestao" });
            DropIndex("dbo.Questao", new[] { "CodDificuldade" });
            DropIndex("dbo.Questao", new[] { "CodProfessor" });
            DropIndex("dbo.Alternativa", new[] { "CodQuestao" });
            DropTable("dbo.ProfessorDisciplina");
            DropTable("dbo.PessoaOcupacao");
            DropTable("dbo.PessoaCategoria");
            DropTable("dbo.AvalAcadRepoJustificacao");
            DropTable("dbo.AvalCertPessoa");
            DropTable("dbo.Parametro");
            DropTable("dbo.TipoQuestao");
            DropTable("dbo.TipoAnexo");
            DropTable("dbo.QuestaoAnexo");
            DropTable("dbo.TipoAvaliacao");
            DropTable("dbo.AvaliacaoProrrogacao");
            DropTable("dbo.Ocupacao");
            DropTable("dbo.QuestaoTema");
            DropTable("dbo.Tema");
            DropTable("dbo.AvaliacaoTema");
            DropTable("dbo.AvalTemaQuestao");
            DropTable("dbo.AvalQuesPessoaResposta");
            DropTable("dbo.AviTipoPublico");
            DropTable("dbo.Area");
            DropTable("dbo.PessoaFormacao");
            DropTable("dbo.NivelEnsino");
            DropTable("dbo.MatrizCurricularDisciplina");
            DropTable("dbo.MatrizCurricular");
            DropTable("dbo.Visitante");
            DropTable("dbo.UsuarioOpiniao");
            DropTable("dbo.UsuarioAcessoPagina");
            DropTable("dbo.UsuarioAcesso");
            DropTable("dbo.Categoria");
            DropTable("dbo.Simulado");
            DropTable("dbo.SimSala");
            DropTable("dbo.SimProvaQuestao");
            DropTable("dbo.SimCandidatoQuestao");
            DropTable("dbo.SimCandidatoProva");
            DropTable("dbo.SimCandidato");
            DropTable("dbo.Pais");
            DropTable("dbo.Estado");
            DropTable("dbo.Reitoria");
            DropTable("dbo.ProReitoria");
            DropTable("dbo.Instituicao");
            DropTable("dbo.PessoaLocalTrabalho");
            DropTable("dbo.Diretoria");
            DropTable("dbo.PessoaJuridica");
            DropTable("dbo.Telefone");
            DropTable("dbo.TipoContato");
            DropTable("dbo.Email");
            DropTable("dbo.Pessoa");
            DropTable("dbo.Endereco");
            DropTable("dbo.Bairro");
            DropTable("dbo.Municipio");
            DropTable("dbo.Candidato");
            DropTable("dbo.Usuario");
            DropTable("dbo.Professor");
            DropTable("dbo.SimProva");
            DropTable("dbo.SimDiaRealizacao");
            DropTable("dbo.Turno");
            DropTable("dbo.Horario");
            DropTable("dbo.DiaSemana");
            DropTable("dbo.TurmaDiscProfHorario");
            DropTable("dbo.Turma");
            DropTable("dbo.TurmaDiscAluno");
            DropTable("dbo.Aluno");
            DropTable("dbo.Curso");
            DropTable("dbo.Colaborador");
            DropTable("dbo.AviQuestaoPessoaResposta");
            DropTable("dbo.AviQuestaoAlternativa");
            DropTable("dbo.AviModulo");
            DropTable("dbo.AviIndicador");
            DropTable("dbo.AviCategoria");
            DropTable("dbo.AviQuestao");
            DropTable("dbo.AvalAvi");
            DropTable("dbo.AviPublico");
            DropTable("dbo.Campus");
            DropTable("dbo.Bloco");
            DropTable("dbo.Sala");
            DropTable("dbo.AvalAcadReposicao");
            DropTable("dbo.Justificacao");
            DropTable("dbo.AvalPessoaResultado");
            DropTable("dbo.PessoaFisica");
            DropTable("dbo.AvalCertificacao");
            DropTable("dbo.Disciplina");
            DropTable("dbo.AvalAcademica");
            DropTable("dbo.Avaliacao");
            DropTable("dbo.AvalAuto");
            DropTable("dbo.Dificuldade");
            DropTable("dbo.Questao");
            DropTable("dbo.Alternativa");
        }
    }
}
