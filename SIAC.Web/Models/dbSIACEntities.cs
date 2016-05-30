namespace SIAC.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class dbSIACEntities : DbContext
    {
        public dbSIACEntities()
            : base("name=SIACEntities")
        {
            Database.SetInitializer(new SIACInitializer());
        }

        public int SaveChanges(bool alertar = true)
        {
            if (alertar)
            {
                Sistema.AlertarMudanca.AddRange(Sistema.UsuarioAtivo.Keys);
                Sistema.AlertarMudanca = Sistema.AlertarMudanca.Distinct().ToList();
                Sistema.AlertarMudanca.Remove(Helpers.Sessao.UsuarioMatricula);
            }
            return base.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Helpers.Sessao.Remover("dbSIACEntities");
            }
            base.Dispose(disposing);
        }

        public virtual DbSet<Alternativa> Alternativa { get; set; }
        public virtual DbSet<Aluno> Aluno { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AvalAcademica> AvalAcademica { get; set; }
        public virtual DbSet<AvalAcadReposicao> AvalAcadReposicao { get; set; }
        public virtual DbSet<AvalAuto> AvalAuto { get; set; }
        public virtual DbSet<AvalAvi> AvalAvi { get; set; }
        public virtual DbSet<AvalCertificacao> AvalCertificacao { get; set; }
        public virtual DbSet<Avaliacao> Avaliacao { get; set; }
        public virtual DbSet<AvaliacaoProrrogacao> AvaliacaoProrrogacao { get; set; }
        public virtual DbSet<AvaliacaoTema> AvaliacaoTema { get; set; }
        public virtual DbSet<AvalPessoaResultado> AvalPessoaResultado { get; set; }
        public virtual DbSet<AvalQuesPessoaResposta> AvalQuesPessoaResposta { get; set; }
        public virtual DbSet<AvalTemaQuestao> AvalTemaQuestao { get; set; }
        public virtual DbSet<AviCategoria> AviCategoria { get; set; }
        public virtual DbSet<AviIndicador> AviIndicador { get; set; }
        public virtual DbSet<AviModulo> AviModulo { get; set; }
        public virtual DbSet<AviPublico> AviPublico { get; set; }
        public virtual DbSet<AviQuestao> AviQuestao { get; set; }
        public virtual DbSet<AviQuestaoAlternativa> AviQuestaoAlternativa { get; set; }
        public virtual DbSet<AviQuestaoPessoaResposta> AviQuestaoPessoaResposta { get; set; }
        public virtual DbSet<AviTipoPublico> AviTipoPublico { get; set; }
        public virtual DbSet<Bairro> Bairro { get; set; }
        public virtual DbSet<Bloco> Bloco { get; set; }
        public virtual DbSet<Campus> Campus { get; set; }
        public virtual DbSet<Candidato> Candidato { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Colaborador> Colaborador { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<DiaSemana> DiaSemana { get; set; }
        public virtual DbSet<Dificuldade> Dificuldade { get; set; }
        public virtual DbSet<Diretoria> Diretoria { get; set; }
        public virtual DbSet<Disciplina> Disciplina { get; set; }
        public virtual DbSet<Email> Email { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<Horario> Horario { get; set; }
        public virtual DbSet<Instituicao> Instituicao { get; set; }
        public virtual DbSet<Justificacao> Justificacao { get; set; }
        public virtual DbSet<MatrizCurricular> MatrizCurricular { get; set; }
        public virtual DbSet<MatrizCurricularDisciplina> MatrizCurricularDisciplina { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<NivelEnsino> NivelEnsino { get; set; }
        public virtual DbSet<Ocupacao> Ocupacao { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Parametro> Parametro { get; set; }
        public virtual DbSet<Pessoa> Pessoa { get; set; }
        public virtual DbSet<PessoaFisica> PessoaFisica { get; set; }
        public virtual DbSet<PessoaFormacao> PessoaFormacao { get; set; }
        public virtual DbSet<PessoaJuridica> PessoaJuridica { get; set; }
        public virtual DbSet<PessoaLocalTrabalho> PessoaLocalTrabalho { get; set; }
        public virtual DbSet<Professor> Professor { get; set; }
        public virtual DbSet<ProReitoria> ProReitoria { get; set; }
        public virtual DbSet<Questao> Questao { get; set; }
        public virtual DbSet<QuestaoAnexo> QuestaoAnexo { get; set; }
        public virtual DbSet<QuestaoTema> QuestaoTema { get; set; }
        public virtual DbSet<Reitoria> Reitoria { get; set; }
        public virtual DbSet<Sala> Sala { get; set; }
        public virtual DbSet<SimCandidato> SimCandidato { get; set; }
        public virtual DbSet<SimCandidatoProva> SimCandidatoProva { get; set; }
        public virtual DbSet<SimCandidatoQuestao> SimCandidatoQuestao { get; set; }
        public virtual DbSet<SimDiaRealizacao> SimDiaRealizacao { get; set; }
        public virtual DbSet<SimProva> SimProva { get; set; }
        public virtual DbSet<SimProvaQuestao> SimProvaQuestao { get; set; }
        public virtual DbSet<SimSala> SimSala { get; set; }
        public virtual DbSet<Simulado> Simulado { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }
        public virtual DbSet<Tema> Tema { get; set; }
        public virtual DbSet<TipoAnexo> TipoAnexo { get; set; }
        public virtual DbSet<TipoAvaliacao> TipoAvaliacao { get; set; }
        public virtual DbSet<TipoContato> TipoContato { get; set; }
        public virtual DbSet<TipoQuestao> TipoQuestao { get; set; }
        public virtual DbSet<Turma> Turma { get; set; }
        public virtual DbSet<TurmaDiscAluno> TurmaDiscAluno { get; set; }
        public virtual DbSet<TurmaDiscProfHorario> TurmaDiscProfHorario { get; set; }
        public virtual DbSet<Turno> Turno { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioAcesso> UsuarioAcesso { get; set; }
        public virtual DbSet<UsuarioAcessoPagina> UsuarioAcessoPagina { get; set; }
        public virtual DbSet<UsuarioOpiniao> UsuarioOpiniao { get; set; }
        public virtual DbSet<Visitante> Visitante { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alternativa>()
                .Property(e => e.Enunciado)
                .IsUnicode(false);

            modelBuilder.Entity<Alternativa>()
                .Property(e => e.Comentario)
                .IsUnicode(false);

            modelBuilder.Entity<Aluno>()
                .Property(e => e.MatrAluno)
                .IsUnicode(false);

            modelBuilder.Entity<Aluno>()
                .HasMany(e => e.TurmaDiscAluno)
                .WithRequired(e => e.Aluno)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.PessoaFormacao)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AvalAcademica>()
                .Property(e => e.CodTurno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AvalAcadReposicao>()
                .HasMany(e => e.Justificacao)
                .WithMany(e => e.AvalAcadReposicao)
                .Map(m => m.ToTable("AvalAcadRepoJustificacao").MapLeftKey(new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }).MapRightKey(new[] { "CodProfessor", "CodJustificacao" }));

            modelBuilder.Entity<AvalAvi>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<AvalAvi>()
                .Property(e => e.Objetivo)
                .IsUnicode(false);

            modelBuilder.Entity<AvalAvi>()
                .HasMany(e => e.AviPublico)
                .WithRequired(e => e.AvalAvi)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AvalAvi>()
                .HasMany(e => e.AviQuestao)
                .WithRequired(e => e.AvalAvi)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Avaliacao>()
                .HasOptional(e => e.AvalAcademica)
                .WithRequired(e => e.Avaliacao);

            modelBuilder.Entity<Avaliacao>()
                .HasOptional(e => e.AvalAcadReposicao)
                .WithRequired(e => e.Avaliacao);

            modelBuilder.Entity<Avaliacao>()
                .HasOptional(e => e.AvalAuto)
                .WithRequired(e => e.Avaliacao);

            modelBuilder.Entity<Avaliacao>()
                .HasOptional(e => e.AvalAvi)
                .WithRequired(e => e.Avaliacao);

            modelBuilder.Entity<Avaliacao>()
                .HasOptional(e => e.AvalCertificacao)
                .WithRequired(e => e.Avaliacao);

            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.AvaliacaoProrrogacao)
                .WithRequired(e => e.Avaliacao)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.AvaliacaoTema)
                .WithRequired(e => e.Avaliacao)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Avaliacao>()
                .HasMany(e => e.AvalPessoaResultado)
                .WithRequired(e => e.Avaliacao)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AvaliacaoProrrogacao>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<AvaliacaoTema>()
                .HasMany(e => e.AvalTemaQuestao)
                .WithRequired(e => e.AvaliacaoTema)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador, e.CodDisciplina, e.CodTema })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AvalPessoaResultado>()
                .HasMany(e => e.Justificacao)
                .WithRequired(e => e.AvalPessoaResultado)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador, e.CodPessoaFisica })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AvalQuesPessoaResposta>()
                .Property(e => e.RespDiscursiva)
                .IsUnicode(false);

            modelBuilder.Entity<AvalQuesPessoaResposta>()
                .Property(e => e.RespComentario)
                .IsUnicode(false);

            modelBuilder.Entity<AvalQuesPessoaResposta>()
                .Property(e => e.ProfObservacao)
                .IsUnicode(false);

            modelBuilder.Entity<AvalTemaQuestao>()
                .HasMany(e => e.AvalQuesPessoaResposta)
                .WithRequired(e => e.AvalTemaQuestao)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador, e.CodDisciplina, e.CodTema, e.CodQuestao })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AviCategoria>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<AviCategoria>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<AviCategoria>()
                .HasMany(e => e.AviQuestao)
                .WithRequired(e => e.AviCategoria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AviIndicador>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<AviIndicador>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<AviIndicador>()
                .HasMany(e => e.AviQuestao)
                .WithRequired(e => e.AviIndicador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AviModulo>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<AviModulo>()
                .Property(e => e.Objetivo)
                .IsUnicode(false);

            modelBuilder.Entity<AviModulo>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<AviModulo>()
                .HasMany(e => e.AviQuestao)
                .WithRequired(e => e.AviModulo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AviQuestao>()
                .Property(e => e.Enunciado)
                .IsUnicode(false);

            modelBuilder.Entity<AviQuestao>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<AviQuestao>()
                .HasMany(e => e.AviQuestaoAlternativa)
                .WithRequired(e => e.AviQuestao)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador, e.CodAviModulo, e.CodAviCategoria, e.CodAviIndicador, e.CodOrdem })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AviQuestao>()
                .HasMany(e => e.AviQuestaoPessoaResposta)
                .WithRequired(e => e.AviQuestao)
                .HasForeignKey(e => new { e.Ano, e.Semestre, e.CodTipoAvaliacao, e.NumIdentificador, e.CodAviModulo, e.CodAviCategoria, e.CodAviIndicador, e.CodOrdem })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AviQuestaoAlternativa>()
                .Property(e => e.Enunciado)
                .IsUnicode(false);

            modelBuilder.Entity<AviQuestaoPessoaResposta>()
                .Property(e => e.RespDiscursiva)
                .IsUnicode(false);

            modelBuilder.Entity<AviTipoPublico>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<AviTipoPublico>()
                .HasMany(e => e.AviPublico)
                .WithRequired(e => e.AviTipoPublico)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bairro>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Bairro>()
                .HasMany(e => e.Endereco)
                .WithRequired(e => e.Bairro)
                .HasForeignKey(e => new { e.CodPais, e.CodEstado, e.CodMunicipio, e.CodBairro })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bloco>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Bloco>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Bloco>()
                .Property(e => e.RefLocal)
                .IsUnicode(false);

            modelBuilder.Entity<Bloco>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<Bloco>()
                .HasMany(e => e.Sala)
                .WithRequired(e => e.Bloco)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Campus>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Campus>()
                .HasMany(e => e.Bloco)
                .WithOptional(e => e.Campus)
                .HasForeignKey(e => new { e.CodInstituicao, e.CodCampus });

            modelBuilder.Entity<Campus>()
                .HasMany(e => e.Diretoria)
                .WithRequired(e => e.Campus)
                .HasForeignKey(e => new { e.CodInstituicao, e.CodCampus })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Campus>()
                .HasMany(e => e.PessoaLocalTrabalho)
                .WithOptional(e => e.Campus)
                .HasForeignKey(e => new { e.CodInstituicao, e.CodCampus });

            modelBuilder.Entity<Candidato>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.Cpf)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.RgOrgao)
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.Senha)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.Sexo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.Matricula)
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.TelefoneFixo)
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.TelefoneCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .Property(e => e.DescricaoNecessidadeEspecial)
                .IsUnicode(false);

            modelBuilder.Entity<Candidato>()
                .HasMany(e => e.SimCandidato)
                .WithRequired(e => e.Candidato)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categoria>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.Categoria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.PessoaFisica)
                .WithMany(e => e.Categoria)
                .Map(m => m.ToTable("PessoaCategoria").MapLeftKey("CodCategoria").MapRightKey("CodPessoaFisica"));

            modelBuilder.Entity<Colaborador>()
                .Property(e => e.MatrColaborador)
                .IsUnicode(false);

            modelBuilder.Entity<Colaborador>()
                .HasMany(e => e.AvalAvi)
                .WithRequired(e => e.Colaborador)
                .HasForeignKey(e => e.CodColabCoordenador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Colaborador>()
                .HasMany(e => e.Campus)
                .WithRequired(e => e.Colaborador)
                .HasForeignKey(e => e.CodColaboradorDiretor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Colaborador>()
                .HasMany(e => e.Curso)
                .WithRequired(e => e.Colaborador)
                .HasForeignKey(e => e.CodColabCoordenador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Colaborador>()
                .HasMany(e => e.Diretoria)
                .WithRequired(e => e.Colaborador)
                .HasForeignKey(e => e.CodColaboradorDiretor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Colaborador>()
                .HasMany(e => e.ProReitoria)
                .WithRequired(e => e.Colaborador)
                .HasForeignKey(e => e.CodColaboradorProReitor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Colaborador>()
                .HasMany(e => e.Reitoria)
                .WithRequired(e => e.Colaborador)
                .HasForeignKey(e => e.CodColaboradorReitor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Colaborador>()
                .HasMany(e => e.Simulado)
                .WithRequired(e => e.Colaborador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Curso>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.Aluno)
                .WithRequired(e => e.Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.MatrizCurricular)
                .WithRequired(e => e.Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.Turma)
                .WithRequired(e => e.Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DiaSemana>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<DiaSemana>()
                .HasMany(e => e.TurmaDiscProfHorario)
                .WithRequired(e => e.DiaSemana)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dificuldade>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Dificuldade>()
                .Property(e => e.Comentario)
                .IsUnicode(false);

            modelBuilder.Entity<Dificuldade>()
                .HasMany(e => e.AvalAuto)
                .WithRequired(e => e.Dificuldade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Dificuldade>()
                .HasMany(e => e.Questao)
                .WithRequired(e => e.Dificuldade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Diretoria>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Diretoria>()
                .HasMany(e => e.Curso)
                .WithRequired(e => e.Diretoria)
                .HasForeignKey(e => new { e.CodInstituicao, e.CodCampus, e.CodDiretoria })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Diretoria>()
                .HasMany(e => e.PessoaLocalTrabalho)
                .WithOptional(e => e.Diretoria)
                .HasForeignKey(e => new { e.CodInstituicao, e.CodCampus, e.CodDiretoria });

            modelBuilder.Entity<Disciplina>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Disciplina>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Disciplina>()
                .HasMany(e => e.AvalAcademica)
                .WithRequired(e => e.Disciplina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disciplina>()
                .HasMany(e => e.AvalCertificacao)
                .WithRequired(e => e.Disciplina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disciplina>()
                .HasMany(e => e.MatrizCurricularDisciplina)
                .WithRequired(e => e.Disciplina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disciplina>()
                .HasMany(e => e.SimProva)
                .WithRequired(e => e.Disciplina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disciplina>()
                .HasMany(e => e.Tema)
                .WithRequired(e => e.Disciplina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disciplina>()
                .HasMany(e => e.TurmaDiscProfHorario)
                .WithRequired(e => e.Disciplina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disciplina>()
                .HasMany(e => e.TurmaDiscAluno)
                .WithRequired(e => e.Disciplina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disciplina>()
                .HasMany(e => e.Professor)
                .WithMany(e => e.Disciplina)
                .Map(m => m.ToTable("ProfessorDisciplina").MapLeftKey("CodDisciplina").MapRightKey("CodProfessor"));

            modelBuilder.Entity<Email>()
                .Property(e => e.Email1)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Logradouro)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Numero)
                .IsUnicode(false);

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<Estado>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Estado>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Estado>()
                .HasMany(e => e.Municipio)
                .WithRequired(e => e.Estado)
                .HasForeignKey(e => new { e.CodPais, e.CodEstado })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Horario>()
                .Property(e => e.CodTurno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Horario>()
                .HasMany(e => e.TurmaDiscProfHorario)
                .WithRequired(e => e.Horario)
                .HasForeignKey(e => new { e.CodGrupo, e.CodTurno, e.CodHorario })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Instituicao>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Instituicao>()
                .HasMany(e => e.Campus)
                .WithRequired(e => e.Instituicao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Instituicao>()
                .HasMany(e => e.ProReitoria)
                .WithRequired(e => e.Instituicao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Instituicao>()
                .HasMany(e => e.Reitoria)
                .WithRequired(e => e.Instituicao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Justificacao>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<MatrizCurricular>()
                .HasMany(e => e.MatrizCurricularDisciplina)
                .WithRequired(e => e.MatrizCurricular)
                .HasForeignKey(e => new { e.CodCurso, e.CodMatriz })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Municipio>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Municipio>()
                .HasMany(e => e.Bairro)
                .WithRequired(e => e.Municipio)
                .HasForeignKey(e => new { e.CodPais, e.CodEstado, e.CodMunicipio })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Municipio>()
                .HasMany(e => e.Candidato)
                .WithOptional(e => e.Municipio)
                .HasForeignKey(e => new { e.CodPais, e.CodEstado, e.CodMunicipio });

            modelBuilder.Entity<NivelEnsino>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<NivelEnsino>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<NivelEnsino>()
                .HasMany(e => e.Curso)
                .WithRequired(e => e.NivelEnsino)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NivelEnsino>()
                .HasMany(e => e.PessoaFormacao)
                .WithRequired(e => e.NivelEnsino)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ocupacao>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Ocupacao>()
                .HasMany(e => e.PessoaFisica)
                .WithMany(e => e.Ocupacao)
                .Map(m => m.ToTable("PessoaOcupacao").MapLeftKey("CodOcupacao").MapRightKey("CodPessoaFisica"));

            modelBuilder.Entity<Pais>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Pais>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Pais>()
                .HasMany(e => e.Estado)
                .WithRequired(e => e.Pais)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.TermoResponsabilidade)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.NotaUsoAcademica)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.NotaUsoReposicao)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.NotaUsoCertificacao)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.NotaUsoInstitucional)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.CoordenadorAVI)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.NotaUsoSimulado)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.SmtpEnderecoHost)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.SmtpUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Parametro>()
                .Property(e => e.SmtpSenha)
                .IsUnicode(false);

            modelBuilder.Entity<Pessoa>()
                .Property(e => e.TipoPessoa)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Pessoa>()
                .HasMany(e => e.Email)
                .WithRequired(e => e.Pessoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pessoa>()
                .HasMany(e => e.Endereco)
                .WithRequired(e => e.Pessoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pessoa>()
                .HasOptional(e => e.PessoaFisica)
                .WithRequired(e => e.Pessoa);

            modelBuilder.Entity<Pessoa>()
                .HasOptional(e => e.PessoaJuridica)
                .WithRequired(e => e.Pessoa);

            modelBuilder.Entity<Pessoa>()
                .HasMany(e => e.Telefone)
                .WithRequired(e => e.Pessoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaFisica>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaFisica>()
                .Property(e => e.Cpf)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PessoaFisica>()
                .Property(e => e.Sexo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PessoaFisica>()
                .HasMany(e => e.AvalAuto)
                .WithRequired(e => e.PessoaFisica)
                .HasForeignKey(e => e.CodPessoaFisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaFisica>()
                .HasMany(e => e.AvalPessoaResultado)
                .WithRequired(e => e.PessoaFisica)
                .HasForeignKey(e => e.CodPessoaFisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaFisica>()
                .HasMany(e => e.AvalQuesPessoaResposta)
                .WithRequired(e => e.PessoaFisica)
                .HasForeignKey(e => e.CodPessoaFisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaFisica>()
                .HasMany(e => e.AviQuestaoPessoaResposta)
                .WithRequired(e => e.PessoaFisica)
                .HasForeignKey(e => e.CodPessoaFisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaFisica>()
                .HasMany(e => e.PessoaFormacao)
                .WithRequired(e => e.PessoaFisica)
                .HasForeignKey(e => e.CodPessoaFisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaFisica>()
                .HasOptional(e => e.PessoaLocalTrabalho)
                .WithRequired(e => e.PessoaFisica);

            modelBuilder.Entity<PessoaFisica>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.PessoaFisica)
                .HasForeignKey(e => e.CodPessoaFisica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaFisica>()
                .HasMany(e => e.AvalCertificacao)
                .WithMany(e => e.PessoaFisica)
                .Map(m => m.ToTable("AvalCertPessoa").MapLeftKey("CodPessoaFisica").MapRightKey(new[] { "Ano", "Semestre", "CodTipoAvaliacao", "NumIdentificador" }));

            modelBuilder.Entity<PessoaFormacao>()
                .Property(e => e.Curso)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridica>()
                .Property(e => e.RazaoSocial)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridica>()
                .Property(e => e.NomeFantasia)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridica>()
                .Property(e => e.Cnpj)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridica>()
                .Property(e => e.Portal)
                .IsUnicode(false);

            modelBuilder.Entity<PessoaJuridica>()
                .HasMany(e => e.Campus)
                .WithRequired(e => e.PessoaJuridica)
                .HasForeignKey(e => e.CodPessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridica>()
                .HasMany(e => e.Diretoria)
                .WithRequired(e => e.PessoaJuridica)
                .HasForeignKey(e => e.CodPessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridica>()
                .HasMany(e => e.Instituicao)
                .WithRequired(e => e.PessoaJuridica)
                .HasForeignKey(e => e.CodPessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridica>()
                .HasMany(e => e.ProReitoria)
                .WithRequired(e => e.PessoaJuridica)
                .HasForeignKey(e => e.CodPessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PessoaJuridica>()
                .HasMany(e => e.Reitoria)
                .WithRequired(e => e.PessoaJuridica)
                .HasForeignKey(e => e.CodPessoaJuridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Professor>()
                .Property(e => e.MatrProfessor)
                .IsUnicode(false);

            modelBuilder.Entity<Professor>()
                .HasMany(e => e.AvalAcademica)
                .WithRequired(e => e.Professor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Professor>()
                .HasMany(e => e.AvalCertificacao)
                .WithRequired(e => e.Professor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Professor>()
                .HasMany(e => e.Justificacao)
                .WithRequired(e => e.Professor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Professor>()
                .HasMany(e => e.Questao)
                .WithRequired(e => e.Professor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Professor>()
                .HasMany(e => e.TurmaDiscProfHorario)
                .WithRequired(e => e.Professor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProReitoria>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<ProReitoria>()
                .HasMany(e => e.PessoaLocalTrabalho)
                .WithOptional(e => e.ProReitoria)
                .HasForeignKey(e => new { e.CodInstituicao, e.CodProReitoria });

            modelBuilder.Entity<Questao>()
                .Property(e => e.Enunciado)
                .IsUnicode(false);

            modelBuilder.Entity<Questao>()
                .Property(e => e.Objetivo)
                .IsUnicode(false);

            modelBuilder.Entity<Questao>()
                .Property(e => e.Comentario)
                .IsUnicode(false);

            modelBuilder.Entity<Questao>()
                .Property(e => e.ChaveDeResposta)
                .IsUnicode(false);

            modelBuilder.Entity<Questao>()
                .HasMany(e => e.Alternativa)
                .WithRequired(e => e.Questao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Questao>()
                .HasMany(e => e.QuestaoAnexo)
                .WithRequired(e => e.Questao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Questao>()
                .HasMany(e => e.QuestaoTema)
                .WithRequired(e => e.Questao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Questao>()
                .HasMany(e => e.SimProvaQuestao)
                .WithRequired(e => e.Questao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QuestaoAnexo>()
                .Property(e => e.Legenda)
                .IsUnicode(false);

            modelBuilder.Entity<QuestaoAnexo>()
                .Property(e => e.Fonte)
                .IsUnicode(false);

            modelBuilder.Entity<QuestaoTema>()
                .HasMany(e => e.AvalTemaQuestao)
                .WithRequired(e => e.QuestaoTema)
                .HasForeignKey(e => new { e.CodDisciplina, e.CodTema, e.CodQuestao })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reitoria>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Reitoria>()
                .HasMany(e => e.PessoaLocalTrabalho)
                .WithOptional(e => e.Reitoria)
                .HasForeignKey(e => new { e.CodInstituicao, e.CodReitoria });

            modelBuilder.Entity<Sala>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Sala>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<Sala>()
                .Property(e => e.RefLocal)
                .IsUnicode(false);

            modelBuilder.Entity<Sala>()
                .Property(e => e.Observacao)
                .IsUnicode(false);

            modelBuilder.Entity<Sala>()
                .HasMany(e => e.AvalAcademica)
                .WithRequired(e => e.Sala)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sala>()
                .HasMany(e => e.AvalAcadReposicao)
                .WithRequired(e => e.Sala)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sala>()
                .HasMany(e => e.AvalCertificacao)
                .WithRequired(e => e.Sala)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sala>()
                .HasMany(e => e.SimSala)
                .WithRequired(e => e.Sala)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sala>()
                .HasMany(e => e.TurmaDiscProfHorario)
                .WithRequired(e => e.Sala)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimCandidato>()
                .Property(e => e.EscorePadronizadoFinal)
                .HasPrecision(8, 3);

            modelBuilder.Entity<SimCandidato>()
                .Property(e => e.NumeroMascara)
                .IsUnicode(false);

            modelBuilder.Entity<SimCandidato>()
                .HasMany(e => e.SimCandidatoProva)
                .WithRequired(e => e.SimCandidato)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador, e.CodCandidato })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimCandidatoProva>()
                .Property(e => e.NotaDiscursiva)
                .HasPrecision(8, 3);

            modelBuilder.Entity<SimCandidatoProva>()
                .Property(e => e.EscorePadronizado)
                .HasPrecision(8, 3);

            modelBuilder.Entity<SimCandidatoProva>()
                .Property(e => e.Observacoes)
                .IsUnicode(false);

            modelBuilder.Entity<SimCandidatoProva>()
                .HasMany(e => e.SimCandidatoQuestao)
                .WithRequired(e => e.SimCandidatoProva)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador, e.CodCandidato, e.CodDiaRealizacao, e.CodProva })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimCandidatoQuestao>()
                .Property(e => e.RespDiscursiva)
                .IsUnicode(false);

            modelBuilder.Entity<SimCandidatoQuestao>()
                .Property(e => e.NotaDiscursiva)
                .HasPrecision(8, 3);

            modelBuilder.Entity<SimDiaRealizacao>()
                .Property(e => e.CodTurno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SimDiaRealizacao>()
                .HasMany(e => e.SimProva)
                .WithRequired(e => e.SimDiaRealizacao)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador, e.CodDiaRealizacao })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimProva>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<SimProva>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<SimProva>()
                .Property(e => e.MediaAritmeticaAcerto)
                .HasPrecision(8, 3);

            modelBuilder.Entity<SimProva>()
                .Property(e => e.DesvioPadraoAcerto)
                .HasPrecision(8, 3);

            modelBuilder.Entity<SimProva>()
                .HasMany(e => e.SimCandidatoProva)
                .WithRequired(e => e.SimProva)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador, e.CodDiaRealizacao, e.CodProva })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimProva>()
                .HasMany(e => e.SimProvaQuestao)
                .WithRequired(e => e.SimProva)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador, e.CodDiaRealizacao, e.CodProva })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimProvaQuestao>()
                .HasMany(e => e.SimCandidatoQuestao)
                .WithRequired(e => e.SimProvaQuestao)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador, e.CodDiaRealizacao, e.CodProva, e.CodQuestao })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimSala>()
                .HasMany(e => e.SimCandidato)
                .WithOptional(e => e.SimSala)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador, e.CodSala });

            modelBuilder.Entity<Simulado>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Simulado>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Simulado>()
                .HasMany(e => e.SimCandidato)
                .WithRequired(e => e.Simulado)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Simulado>()
                .HasMany(e => e.SimDiaRealizacao)
                .WithRequired(e => e.Simulado)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Simulado>()
                .HasMany(e => e.SimSala)
                .WithRequired(e => e.Simulado)
                .HasForeignKey(e => new { e.Ano, e.NumIdentificador })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Telefone>()
                .Property(e => e.Numero)
                .IsUnicode(false);

            modelBuilder.Entity<Tema>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Tema>()
                .Property(e => e.Comentario)
                .IsUnicode(false);

            modelBuilder.Entity<Tema>()
                .HasMany(e => e.AvaliacaoTema)
                .WithRequired(e => e.Tema)
                .HasForeignKey(e => new { e.CodDisciplina, e.CodTema })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tema>()
                .HasMany(e => e.QuestaoTema)
                .WithRequired(e => e.Tema)
                .HasForeignKey(e => new { e.CodDisciplina, e.CodTema })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoAnexo>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TipoAnexo>()
                .HasMany(e => e.QuestaoAnexo)
                .WithRequired(e => e.TipoAnexo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoAvaliacao>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TipoAvaliacao>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<TipoAvaliacao>()
                .HasMany(e => e.Avaliacao)
                .WithRequired(e => e.TipoAvaliacao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoContato>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TipoContato>()
                .HasMany(e => e.Email)
                .WithRequired(e => e.TipoContato)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoContato>()
                .HasMany(e => e.Telefone)
                .WithRequired(e => e.TipoContato)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TipoQuestao>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TipoQuestao>()
                .Property(e => e.Sigla)
                .IsUnicode(false);

            modelBuilder.Entity<TipoQuestao>()
                .HasMany(e => e.Questao)
                .WithRequired(e => e.TipoQuestao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Turma>()
                .Property(e => e.CodTurno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Turma>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Turma>()
                .HasMany(e => e.AvalAcademica)
                .WithRequired(e => e.Turma)
                .HasForeignKey(e => new { e.CodCurso, e.Periodo, e.CodTurno, e.NumTurma })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Turma>()
                .HasMany(e => e.TurmaDiscAluno)
                .WithRequired(e => e.Turma)
                .HasForeignKey(e => new { e.CodCurso, e.Periodo, e.CodTurno, e.NumTurma })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Turma>()
                .HasMany(e => e.TurmaDiscProfHorario)
                .WithRequired(e => e.Turma)
                .HasForeignKey(e => new { e.CodCurso, e.Periodo, e.CodTurno, e.NumTurma })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TurmaDiscAluno>()
                .Property(e => e.CodTurno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TurmaDiscProfHorario>()
                .Property(e => e.CodTurno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Turno>()
                .Property(e => e.CodTurno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Turno>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Turno>()
                .HasMany(e => e.Horario)
                .WithRequired(e => e.Turno)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Turno>()
                .HasMany(e => e.SimDiaRealizacao)
                .WithRequired(e => e.Turno)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Turno>()
                .HasMany(e => e.Turma)
                .WithRequired(e => e.Turno)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Matricula)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Senha)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Aluno)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.MatrAluno)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Colaborador)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.MatrColaborador)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Professor)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.MatrProfessor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.UsuarioAcesso)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.UsuarioOpiniao)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Visitante)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.MatrVisitante)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UsuarioAcesso>()
                .Property(e => e.Matricula)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioAcesso>()
                .Property(e => e.IpAcesso)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioAcesso>()
                .HasMany(e => e.UsuarioAcessoPagina)
                .WithRequired(e => e.UsuarioAcesso)
                .HasForeignKey(e => new { e.Matricula, e.CodOrdem })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UsuarioAcessoPagina>()
                .Property(e => e.Matricula)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioAcessoPagina>()
                .Property(e => e.Pagina)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioAcessoPagina>()
                .Property(e => e.PaginaReferencia)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioAcessoPagina>()
                .Property(e => e.Dados)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioOpiniao>()
                .Property(e => e.Matricula)
                .IsUnicode(false);

            modelBuilder.Entity<UsuarioOpiniao>()
                .Property(e => e.Opiniao)
                .IsUnicode(false);

            modelBuilder.Entity<Visitante>()
                .Property(e => e.MatrVisitante)
                .IsUnicode(false);
        }
    }

    public class SIACInitializer : CreateDatabaseIfNotExists<dbSIACEntities>
    {
        protected override void Seed(dbSIACEntities context)
        {
            base.Seed(context);
        }
    }
}
