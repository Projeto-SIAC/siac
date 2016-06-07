namespace SIAC.Migrations
{
    using Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SIAC.Models.dbSIACEntities";
        }

        protected override void Seed(Contexto context)
        {
            const string InstituicaoRazaoSocial = "Instituto Federal de Educação, Ciência e Tecnologia do Rio Grande do Norte";
            const string InstituicaoNomeFantasia = "Insituto Federal do Rio Grande do Norte";
            const string InstituicaoSigla = "IFRN";
            const string InstituicaoCNPJ = "10877412001059";
            const string InstituicaoPortal = "http://portal.ifrn.edu.br";

            #region Estrutura

            context.Categoria.AddOrUpdate(
                c => c.CodCategoria,
                new Categoria
                {
                    CodCategoria = 0,
                    Descricao = "Superusuário"
                },
                new Categoria
                {
                    CodCategoria = 1,
                    Descricao = "Aluno"
                },
                new Categoria
                {
                    CodCategoria = 2,
                    Descricao = "Professor"
                },
                new Categoria
                {
                    CodCategoria = 3,
                    Descricao = "Colaborador"
                },
                new Categoria
                {
                    CodCategoria = 4,
                    Descricao = "Visitante"
                }
            );

            context.Ocupacao.AddOrUpdate(
                o => o.CodOcupacao,
                new Ocupacao
                {
                    CodOcupacao = 0,
                    Descricao = "Superusuário"
                },
                new Ocupacao
                {
                    CodOcupacao = 1,
                    Descricao = "Reitor"
                },
                new Ocupacao
                {
                    CodOcupacao = 2,
                    Descricao = "Pró-Reitor"
                },
                new Ocupacao
                {
                    CodOcupacao = 3,
                    Descricao = "Diretor-Geral"
                },
                new Ocupacao
                {
                    CodOcupacao = 4,
                    Descricao = "Diretor"
                },
                new Ocupacao
                {
                    CodOcupacao = 5,
                    Descricao = "Coordenador"
                },
                new Ocupacao
                {
                    CodOcupacao = 6,
                    Descricao = "Coordenador de Avaliação Institucional"
                },
                new Ocupacao
                {
                    CodOcupacao = 7,
                    Descricao = "Coordenador de Simulado"
                },
                new Ocupacao
                {
                    CodOcupacao = 8,
                    Descricao = "Colaborador de Simulado"
                }
            );

            context.DiaSemana.AddOrUpdate(
                d => d.CodDia,
                new DiaSemana
                {
                    CodDia = 1,
                    Descricao = "Domingo"
                },
                new DiaSemana
                {
                    CodDia = 2,
                    Descricao = "Segunda-Feira"
                },
                new DiaSemana
                {
                    CodDia = 3,
                    Descricao = "Terça-Feira"
                },
                new DiaSemana
                {
                    CodDia = 4,
                    Descricao = "Quarta-Feira"
                },
                new DiaSemana
                {
                    CodDia = 5,
                    Descricao = "Quinta-Feira"
                },
                new DiaSemana
                {
                    CodDia = 6,
                    Descricao = "Sexta-Feira"
                },
                new DiaSemana
                {
                    CodDia = 7,
                    Descricao = "Sábado"
                }
            );

            context.NivelEnsino.AddOrUpdate(
                n => n.CodNivelEnsino,
                new NivelEnsino
                {
                    CodNivelEnsino = 1,
                    Descricao = "Pós-Doutorado",
                    Sigla = "PosDc"
                },
                new NivelEnsino
                {
                    CodNivelEnsino = 2,
                    Descricao = "Doutorado",
                    Sigla = "Doc"
                },
                new NivelEnsino
                {
                    CodNivelEnsino = 3,
                    Descricao = "Mestrado",
                    Sigla = "Mes"
                },
                new NivelEnsino
                {
                    CodNivelEnsino = 4,
                    Descricao = "Especialista",
                    Sigla = "Esp"
                },
                new NivelEnsino
                {
                    CodNivelEnsino = 5,
                    Descricao = "Graduaçao",
                    Sigla = "Grad"
                },
                new NivelEnsino
                {
                    CodNivelEnsino = 6,
                    Descricao = "Técnico",
                    Sigla = "Tec"
                }
            );

            context.Dificuldade.AddOrUpdate(
                d => d.CodDificuldade,
                new Dificuldade
                {
                    CodDificuldade = 1,
                    Descricao = "Fácil",
                    Comentario = "As questões desse grau devem exigir um conhecimento básico do assunto desejado, sem ter relação direta com outros assuntos."
                },
                new Dificuldade
                {
                    CodDificuldade = 2,
                    Descricao = "Médio",
                    Comentario = "As questões desse grau devem exigir um conhecimento básico do assunto desejado, mas existindo a necessidade de alguns conhecimentos anteriores."
                },
                new Dificuldade
                {
                    CodDificuldade = 3,
                    Descricao = "Complicado",
                    Comentario = "As questões desse grau devem exigir um conhecimento mais avançado do assunto desejado, exigindo a necessidade de conhecimentos anteriores."
                },
                new Dificuldade
                {
                    CodDificuldade = 4,
                    Descricao = "Difícil",
                    Comentario = "As questões desse grau devem exigir um conhecimento avançado e que exijam raciocínio lógico do assunto desejado e que também exijam a necessidade de conhecimentos anteriores."
                },
                new Dificuldade
                {
                    CodDificuldade = 5,
                    Descricao = "Complexo",
                    Comentario = "As questões desse grau devem exigir um conhecimento avançado, raciocínio lógico do assunto desejado, e que também exijam a necessidade de conhecimentos anteriores com a mesma exigência do assunto principal da questão."
                }
            );

            context.AviTipoPublico.AddOrUpdate(
                p => p.CodAviTipoPublico,
                new AviTipoPublico
                {
                    CodAviTipoPublico = 1,
                    Descricao = "Instituição"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = 2,
                    Descricao = "Reitoria"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = 3,
                    Descricao = "Pró-Reitoria"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = 4,
                    Descricao = "Campus"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = 5,
                    Descricao = "Diretoria"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = 6,
                    Descricao = "Curso"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = 7,
                    Descricao = "Turma"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = 8,
                    Descricao = "Pessoa"
                }
            );

            context.Area.AddOrUpdate(
                a => a.CodArea,
                new Area
                {
                    CodArea = 1,
                    Descricao = "Ciências Exatas e da Terra"
                },
                new Area
                {
                    CodArea = 2,
                    Descricao = "Ciências Biológicas"
                },
                new Area
                {
                    CodArea = 3,
                    Descricao = "Engenharias"
                },
                new Area
                {
                    CodArea = 4,
                    Descricao = "Ciências da Saúde"
                },
                new Area
                {
                    CodArea = 5,
                    Descricao = "Ciências Agrárias"
                },
                new Area
                {
                    CodArea = 6,
                    Descricao = "Ciências Sociais Aplicadas"
                },
                new Area
                {
                    CodArea = 7,
                    Descricao = "Ciências Humanas"
                },
                new Area
                {
                    CodArea = 8,
                    Descricao = "Linguística, Letras e Artes"
                }
            );

            context.Turno.AddOrUpdate(
                t => t.CodTurno,
                new Turno
                {
                    CodTurno = "M",
                    Descricao = "Matutino"
                },
                new Turno
                {
                    CodTurno = "V",
                    Descricao = "Vespertino"
                },
                new Turno
                {
                    CodTurno = "N",
                    Descricao = "Noturno"
                }
            );

            context.TipoQuestao.AddOrUpdate(
                t => t.CodTipoQuestao,
                new TipoQuestao
                {
                    CodTipoQuestao = 1,
                    Descricao = "Objetiva",
                    Sigla = "OBJ"
                },
                new TipoQuestao
                {
                    CodTipoQuestao = 2,
                    Descricao = "Discursiva",
                    Sigla = "DISC"
                }
            );

            context.TipoContato.AddOrUpdate(
                t => t.CodTipoContato,
                new TipoContato
                {
                    CodTipoContato = 1,
                    Descricao = "Pessoal"
                },
                new TipoContato
                {
                    CodTipoContato = 2,
                    Descricao = "Institucional"
                },
                new TipoContato
                {
                    CodTipoContato = 3,
                    Descricao = "Profissional"
                }
            );

            context.TipoAvaliacao.AddOrUpdate(
                t => t.CodTipoAvaliacao,
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = 1,
                    Descricao = "Autoavaliação",
                    Sigla = "AUTO"
                },
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = 2,
                    Descricao = "Avaliação Acadêmica",
                    Sigla = "ACAD"
                },
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = 3,
                    Descricao = "Avaliação de Certificação",
                    Sigla = "CERT"
                },
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = 4,
                    Descricao = "Avaliação de Reposição",
                    Sigla = "REPO"
                },
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = 5,
                    Descricao = "Avaliação Institucional",
                    Sigla = "AVI"
                }
            );

            context.TipoAnexo.AddOrUpdate(
                t => t.CodTipoAnexo,
                new TipoAnexo
                {
                    CodTipoAnexo = 1,
                    Descricao = "Imagem"
                },
                new TipoAnexo
                {
                    CodTipoAnexo = 2,
                    Descricao = "Código"
                }
            );

            #endregion Estrutura

            #region Parâmetros

            context.Parametro.AddOrUpdate(
                p => p.CodParametro,
                new Parametro
                {
                    CodParametro = 1,
                    PeriodoLetivoAnoAtual = System.DateTime.Today.Year,
                    PeriodoLetivoSemestreAtual = System.DateTime.Today.Month <= 6 ? 1 : 2,
                    QteSemestres = 2,
                    TempoInatividade = 15, // em dias
                    NumeracaoQuestao = (int)Parametro.NumeracaoPadrao.INDO_ARABICO,
                    NumeracaoAlternativa = (int)Parametro.NumeracaoPadrao.CAIXA_BAIXA,
                    ValorNotaMedia = 6,
                    TermoResponsabilidade = "Termo de Responsabilidade.\nNão esqueça de atualizar.",
                    NotaUsoAcademica = "Nota de Uso para Avaliação Acadêmica.\nNão esqueça de atualizar.",
                    NotaUsoReposicao = "Nota de Uso para Avaliação de Reposição.\nNão esqueça de atualizar.",
                    NotaUsoCertificacao = "Nota de Uso para Avaliação de Certificação.\nNão esqueça de atualizar.",
                    NotaUsoInstitucional = "Nota de Uso para Avaliação Institucional.\nNão esqueça de atualizar.",
                    NotaUsoSimulado = "Nota de Uso para Simulado.\nNão esqueça de atualizar.",
                    CoordenadorAVI = Newtonsoft.Json.JsonConvert.SerializeObject(new int[] { 0, 1, 2, 3, 4 }), // ocupações que são coordenadores de avaliação institucional
                    SmtpEnderecoHost = "voce.precisa.de.um.smpt", // ex.: smpt.live.com
                    SmtpFlagSSL = false,
                    SmtpPorta = 25,
                    SmtpUsuario = Helpers.Criptografia.Base64Encode("usuario"),
                    SmtpSenha = Helpers.Criptografia.Base64Encode("senha")
                }
            );

            #endregion Parâmetros

            #region Primeiras Pessoas

            if (!(context.Pessoa.Find(1)?.TipoPessoa == Pessoa.FISICA) && !(context.Pessoa.Find(2)?.TipoPessoa == Pessoa.JURIDICA))
            {
                context.Pessoa.AddOrUpdate(
                    p => p.CodPessoa,
                    new Pessoa
                    {
                        CodPessoa = 1,
                        TipoPessoa = Pessoa.FISICA
                    },
                    new Pessoa
                    {
                        CodPessoa = 2,
                        TipoPessoa = Pessoa.JURIDICA
                    }
                );
            }

            if (!(context.PessoaFisica.Find(1)?.Nome == "Superusuário"))
            {
                context.PessoaFisica.AddOrUpdate(
                    p => p.CodPessoa,
                    new PessoaFisica
                    {
                        CodPessoa = 1,
                        Nome = "Superusuário"
                    }
                );
            }

            context.PessoaFisica.Find(1).Ocupacao.Add(context.Ocupacao.Find(0));
            context.PessoaFisica.Find(1).Categoria.Add(context.Categoria.Find(0));

            if (!(context.PessoaJuridica.Find(2)?.Cnpj == InstituicaoCNPJ))
            {
                context.PessoaJuridica.AddOrUpdate(
                   p => p.CodPessoa,
                   new PessoaJuridica
                   {
                       CodPessoa = 2,
                       NomeFantasia = InstituicaoNomeFantasia,
                       RazaoSocial = InstituicaoRazaoSocial,
                       Cnpj = InstituicaoCNPJ,
                       Portal = InstituicaoPortal
                   }
               );
            }

            if (!(context.Instituicao.Find(1)?.Sigla == InstituicaoSigla))
            {
                context.Instituicao.AddOrUpdate(
                    i => i.CodInstituicao,
                    new Instituicao
                    {
                        CodInstituicao = 1,
                        CodPessoaJuridica = 2,
                        Sigla = InstituicaoSigla
                    }
                );
            }

            context.Usuario.AddOrUpdate(
                u => u.Matricula,
                new Usuario
                {
                    Matricula = "superusuario",
                    Senha = Helpers.Criptografia.RetornarHash("senha"),
                    CodPessoaFisica = 1,
                    CodCategoria = 0,
                    DtCadastro = System.DateTime.Now
                }
            );

            #endregion Primeiras Pessoas
        }
    }
}