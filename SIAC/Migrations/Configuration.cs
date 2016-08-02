namespace SIAC.Migrations
{
    using Helpers;
    using Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SIAC.Models.Contexto";
        }

        protected override void Seed(Contexto context)
        {
            const string InstituicaoRazaoSocial = "Instituto Federal de Educação, Ciência e Tecnologia do Rio Grande do Norte";
            const string InstituicaoNomeFantasia = "Insituto Federal do Rio Grande do Norte";
            const string InstituicaoSigla = "IFRN";
            const string InstituicaoCNPJ = "10877412001059";
            const string InstituicaoPortal = "http://portal.ifrn.edu.br";

            SemearEstruturas(context);
            SemearLocalidades(context);
            SemearParametros(context);

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

            context.PessoaFisica.Find(1).Ocupacao.Add(context.Ocupacao.Find(Ocupacao.SUPERUSUARIO));
            context.PessoaFisica.Find(1).Categoria.Add(context.Categoria.Find(Categoria.SUPERUSUARIO));

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
                    CodCategoria = Categoria.SUPERUSUARIO,
                    DtCadastro = System.DateTime.Now
                }
            );
        }

        private void SemearEstruturas(Contexto context)
        {
            context.Categoria.AddOrUpdate(
                c => c.CodCategoria,
                new Categoria
                {
                    CodCategoria = Categoria.SUPERUSUARIO,
                    Descricao = "Superusuário"
                },
                new Categoria
                {
                    CodCategoria = Categoria.ALUNO,
                    Descricao = "Aluno"
                },
                new Categoria
                {
                    CodCategoria = Categoria.PROFESSOR,
                    Descricao = "Professor"
                },
                new Categoria
                {
                    CodCategoria = Categoria.COLABORADOR,
                    Descricao = "Colaborador"
                },
                new Categoria
                {
                    CodCategoria = Categoria.VISITANTE,
                    Descricao = "Visitante"
                }
            );

            context.Ocupacao.AddOrUpdate(
                o => o.CodOcupacao,
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.SUPERUSUARIO,
                    Descricao = "Superusuário"
                },
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.REITOR,
                    Descricao = "Reitor"
                },
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.PRO_REITOR,
                    Descricao = "Pró-Reitor"
                },
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.DIRETOR_GERAL,
                    Descricao = "Diretor-Geral"
                },
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.DIRETOR,
                    Descricao = "Diretor"
                },
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.COORDENADOR,
                    Descricao = "Coordenador"
                },
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.COORDENADOR_AVI,
                    Descricao = "Coordenador de Avaliação Institucional"
                },
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.COORDENADOR_SIMULADO,
                    Descricao = "Coordenador de Simulado"
                },
                new Ocupacao
                {
                    CodOcupacao = Ocupacao.COLABORADOR_SIMULADO,
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
                    CodAviTipoPublico = AviTipoPublico.INSTITUICAO,
                    Descricao = "Instituição"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = AviTipoPublico.REITORIA,
                    Descricao = "Reitoria"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = AviTipoPublico.PRO_REITORIA,
                    Descricao = "Pró-Reitoria"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = AviTipoPublico.CAMPUS,
                    Descricao = "Campus"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = AviTipoPublico.DIRETORIA,
                    Descricao = "Diretoria"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = AviTipoPublico.CURSO,
                    Descricao = "Curso"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = AviTipoPublico.TURMA,
                    Descricao = "Turma"
                },
                new AviTipoPublico
                {
                    CodAviTipoPublico = AviTipoPublico.PESSOA,
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
                    CodTipoAvaliacao = TipoAvaliacao.AUTOAVALIACAO,
                    Descricao = "Autoavaliação",
                    Sigla = "AUTO"
                },
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = TipoAvaliacao.ACADEMICA,
                    Descricao = "Avaliação Acadêmica",
                    Sigla = "ACAD"
                },
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = TipoAvaliacao.CERTIFICACAO,
                    Descricao = "Avaliação de Certificação",
                    Sigla = "CERT"
                },
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = TipoAvaliacao.REPOSICAO,
                    Descricao = "Avaliação de Reposição",
                    Sigla = "REPO"
                },
                new TipoAvaliacao
                {
                    CodTipoAvaliacao = TipoAvaliacao.AVI,
                    Descricao = "Avaliação Institucional",
                    Sigla = "AVI"
                }
            );

            context.TipoAnexo.AddOrUpdate(
                t => t.CodTipoAnexo,
                new TipoAnexo
                {
                    CodTipoAnexo = TipoAnexo.IMAGEM,
                    Descricao = "Imagem"
                },
                new TipoAnexo
                {
                    CodTipoAnexo = TipoAnexo.CODIGO,
                    Descricao = "Código"
                },
                new TipoAnexo
                {
                    CodTipoAnexo = TipoAnexo.TEXTO,
                    Descricao = "Texto"
                }
            );
        }

        private void SemearLocalidades(Contexto context)
        {
            context.Pais.AddOrUpdate(
                p => p.CodPais,
                new Pais { CodPais = 1, Descricao = "Brasil", Sigla = "BRA" }
            );

            //Acre.Semear(context);
            //Alagoas.Semear(context);
            //Amapa.Semear(context);
            //Amazonas.Semear(context);
            //Bahia.Semear(context);
            //Ceara.Semear(context);
            //DestritoFederal.Semear(context);
            //EspiritoSanto.Semear(context);
            //Goias.Semear(context);
            //Maranhao.Semear(context);
            //MatoGrosso.Semear(context);
            //MatoGrossoDoSul.Semear(context);
            //MinasGerais.Semear(context);
            //Para.Semear(context);
            //Paraiba.Semear(context);
            //Parana.Semear(context);
            //Pernambuco.Semear(context);
            //Piaui.Semear(context);
            //RioDeJaneiro.Semear(context);
            RioGrandeDoNorte.Semear(context);
            //RioGrandeDoSul.Semear(context);
            //Rondonia.Semear(context);
            //Roraima.Semear(context);
            //SantaCatarina.Semear(context);
            //SaoPaulo.Semear(context);
            //Sergipe.Semear(context);
            //Tocantins.Semear(context);

        }

        private void SemearParametros(Contexto context)
        {
            if (context.Parametro.Find(1) == null)
            {
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
                        CoordenadorAVI = Newtonsoft.Json.JsonConvert.SerializeObject(new int[] {
                        Ocupacao.SUPERUSUARIO,
                        Ocupacao.REITOR,
                        Ocupacao.PRO_REITOR,
                        Ocupacao.DIRETOR_GERAL,
                        Ocupacao.DIRETOR
                        }),
                        SmtpEnderecoHost = "nao.esqueca.de.atualizar", // ex.: smpt.live.com
                        SmtpFlagSSL = false,
                        SmtpPorta = 25,
                        SmtpUsuario = Helpers.Criptografia.Base64Encode("usuario"),
                        SmtpSenha = Helpers.Criptografia.Base64Encode("senha")
                    }
                );
            }
        }
    }
}