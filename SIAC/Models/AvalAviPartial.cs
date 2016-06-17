using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalAvi
    {
        [NotMapped]
        public List<AviQuestao> Questoes
        {
            get
            {
                List<AviQuestao> questoes = new List<AviQuestao>();

                questoes = contexto.AviQuestao
                    .Where(q => q.Ano == this.Ano
                        && q.Semestre == this.Semestre
                        && q.CodTipoAvaliacao == this.CodTipoAvaliacao
                        && q.NumIdentificador == this.NumIdentificador)
                    .OrderBy(q => q.CodOrdem)
                    .ToList();

                return questoes;
            }
        }

        [NotMapped]
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                foreach (AviPublico publico in this.Publico)
                {
                    switch (publico.CodAviTipoPublico)
                    {
                        case AviTipoPublico.INSTITUICAO:
                            pessoas.AddRange(publico.Instituicao.Pessoas);
                            break;

                        case AviTipoPublico.REITORIA:
                            pessoas.AddRange(publico.Reitoria.Pessoas);
                            break;

                        case AviTipoPublico.PRO_REITORIA:
                            pessoas.AddRange(publico.ProReitoria.Pessoas);
                            break;

                        case AviTipoPublico.CAMPUS:
                            pessoas.AddRange(publico.Campus.Pessoas);
                            break;

                        case AviTipoPublico.DIRETORIA:
                            pessoas.AddRange(publico.Diretoria.Pessoas);
                            break;

                        case AviTipoPublico.CURSO:
                            pessoas.AddRange(PessoaFisica.ListarPorCurso(publico.Curso.CodCurso));
                            break;

                        case AviTipoPublico.TURMA:
                            pessoas.AddRange(PessoaFisica.ListarPorTurma(publico.Turma.CodTurma));
                            break;

                        case AviTipoPublico.PESSOA:
                            pessoas.Add(publico.PessoaFisica);
                            break;

                        default:
                            break;
                    }
                }

                return pessoas.Distinct().ToList();
            }
        }

        [NotMapped]
        public List<MapAviModulo> MapQuestoes
        {
            get
            {
                List<MapAviModulo> modulos = new List<MapAviModulo>();
                List<AviQuestao> lstQuestao = this.Questoes;
                List<AviModulo> lstModulo = lstQuestao.Select(a => a.AviModulo).Distinct().ToList();

                foreach (AviModulo m in lstModulo)
                {
                    MapAviModulo modulo = new MapAviModulo();
                    modulo.Modulo = m;
                    List<AviCategoria> categorias = lstQuestao.Where(a => a.CodAviModulo == m.CodAviModulo).Select(a => a.AviCategoria).Distinct().ToList();
                    foreach (AviCategoria c in categorias)
                    {
                        MapAviCategoria categoria = new MapAviCategoria();
                        categoria.Categoria = c;
                        List<AviIndicador> indicadores = lstQuestao.Where(a => a.CodAviModulo == m.CodAviModulo && a.CodAviCategoria == c.CodAviCategoria).Select(a => a.AviIndicador).Distinct().ToList();
                        foreach (AviIndicador i in indicadores)
                        {
                            MapAviIndicador indicador = new MapAviIndicador();
                            indicador.Indicador = i;

                            List<AviQuestao> questoesIndicador = lstQuestao.Where(a => a.CodAviModulo == m.CodAviModulo && a.CodAviCategoria == c.CodAviCategoria && a.CodAviIndicador == i.CodAviIndicador).ToList();

                            indicador.Questoes.AddRange(questoesIndicador);

                            categoria.Indicadores.Add(indicador);
                        }

                        modulo.Categorias.Add(categoria);
                    }
                    modulos.Add(modulo);
                }

                return modulos;
            }
        }

        [NotMapped]
        public List<AviPublico> Publico => this.FlagPublico ? this.AviPublico.ToList() : null;

        [NotMapped]
        public bool FlagQuestionario => this.Questoes.Count > 0;

        [NotMapped]
        public bool FlagPublico => this.AviPublico.Count > 0;

        [NotMapped]
        public bool FlagAgendada => this.Avaliacao.DtAplicacao.HasValue && this.DtTermino.HasValue;

        [NotMapped]
        public bool FlagRealizada => this.Questoes.FirstOrDefault(a => a.AviQuestaoPessoaResposta.Count > 0) != null;

        [NotMapped]
        public bool FlagAndamento => this.FlagAgendada && this.Avaliacao.DtAplicacao.Value <= DateTime.Now && this.DtTermino.Value >= DateTime.Now;

        [NotMapped]
        public bool FlagConcluida => this.DtTermino < DateTime.Now;

        [NotMapped]
        public int QteQuestoes => this.Questoes.Count;

        private static Contexto contexto => Repositorio.GetInstance();

        public AviQuestao ObterQuestao(int modulo, int categoria, int indicador, int ordem) =>
            this.AviQuestao.FirstOrDefault(q => q.CodAviModulo == modulo
                && q.CodAviCategoria == categoria
                && q.CodAviIndicador == indicador
                && q.CodOrdem == ordem);

        public AviQuestao ObterQuestao(int ordem) => this.AviQuestao.FirstOrDefault(q => q.CodOrdem == ordem);

        public static void Inserir(AvalAvi avi)
        {
            contexto.AvalAvi.Add(avi);
            contexto.SaveChanges();
        }

        public static List<AvalAvi> Listar() => contexto.AvalAvi.ToList();

        public static List<AvalAvi> ListarPorColaborador(string matricula)
        {
            Colaborador colaborador = Colaborador.ListarPorMatricula(matricula);

            if (colaborador != null)
                return contexto.AvalAvi.Where(avi => avi.CodColabCoordenador == colaborador.CodColaborador).ToList();
            return new List<AvalAvi>();
        }

        public static List<AvalAvi> ListarPorUsuario(string matricula)
        {
            PessoaFisica pessoa = PessoaFisica.ListarPorMatricula(matricula);

            if (pessoa != null)
            {
                List<AvalAvi> institucionais = contexto.AvalAvi.Where(avi => avi.Avaliacao.DtAplicacao <= DateTime.Now && avi.DtTermino >= DateTime.Now).ToList();

                List<AvalAvi> retorno = new List<AvalAvi>();

                foreach (AvalAvi avi in institucionais)
                    if (avi.Pessoas.FirstOrDefault(p => p.CodPessoa == pessoa.CodPessoa) != null)
                        retorno.Add(avi);
                return retorno;
            }
            return new List<AvalAvi>();
        }

        public static AvalAvi ListarPorCodigoAvaliacao(string codigo) =>
            Avaliacao.ListarPorCodigoAvaliacao(codigo)?.AvalAvi;

        public void OrdenarQuestoes(string[] questoes)
        {
            List<AviQuestao> aviQuestoes = this.Questoes;
            List<AviQuestao> aviQuestoesNova = new List<Models.AviQuestao>();
            if (questoes.Length > 0)
            {
                for (int i = 0; i < questoes.Length; i++)
                {
                    string[] valores = questoes[i].Split('.');

                    int modulo = int.Parse(valores[0]);
                    int categoria = int.Parse(valores[1]);
                    int indicador = int.Parse(valores[2]);
                    int ordem = int.Parse(valores[3]);

                    AviQuestao questaoAntiga = aviQuestoes
                        .FirstOrDefault(q => q.CodAviModulo == modulo
                            && q.CodAviCategoria == categoria
                            && q.CodAviIndicador == indicador
                            && q.CodOrdem == ordem);

                    if (questaoAntiga != null)
                    {
                        AviQuestao questaoNova = questaoAntiga;
                        List<AviQuestaoAlternativa> alternativas = questaoNova.AviQuestaoAlternativa.ToList();
                        Models.AviQuestao.Remover(questaoAntiga);
                        questaoNova.CodOrdem = i + 1;
                        if (alternativas.Count > 0)
                        {
                            foreach (AviQuestaoAlternativa alternativa in alternativas)
                            {
                                alternativa.CodOrdem = questaoNova.CodOrdem;
                                questaoNova.AviQuestaoAlternativa.Add(alternativa);
                            }
                        }
                        aviQuestoesNova.Add(questaoNova);
                    }
                }

                if (aviQuestoesNova.Count > 0)
                {
                    contexto.AviQuestao.AddRange(aviQuestoesNova);
                    contexto.SaveChanges();
                }
            }
        }

        public void InserirPublico(List<Selecao> publico)
        {
            int ordem = 1;
            foreach (Selecao item in publico)
            {
                switch (item.category)
                {
                    case "Pessoa":
                        AviPublico pessoa = new AviPublico
                        {
                            CodAviTipoPublico = 8,
                            CodOrdem = ordem,
                            PessoaFisica = PessoaFisica.ListarPorCodigo(int.Parse(item.id))
                        };
                        this.AviPublico.Add(pessoa);
                        break;

                    case "Turma":
                        AviPublico turma = new AviPublico
                        {
                            CodAviTipoPublico = 7,
                            CodOrdem = ordem,
                            Turma = Turma.ListarPorCodigo(item.id)
                        };
                        this.AviPublico.Add(turma);
                        break;

                    case "Curso":
                        AviPublico curso = new AviPublico
                        {
                            CodAviTipoPublico = 6,
                            CodOrdem = ordem,
                            Curso = Curso.ListarPorCodigo(int.Parse(item.id))
                        };
                        this.AviPublico.Add(curso);
                        break;

                    case "Diretoria":
                        AviPublico diretoria = new AviPublico
                        {
                            CodAviTipoPublico = 5,
                            CodOrdem = ordem,
                            Diretoria = Diretoria.ListarPorCodigo(item.id)
                        };
                        this.AviPublico.Add(diretoria);
                        break;

                    case "Campus":
                        AviPublico campus = new AviPublico
                        {
                            CodAviTipoPublico = 4,
                            CodOrdem = ordem,
                            Campus = Campus.ListarPorCodigo(item.id)
                        };
                        this.AviPublico.Add(campus);
                        break;

                    case "Pró-Reitoria":
                        AviPublico proReitoria = new AviPublico
                        {
                            CodAviTipoPublico = 3,
                            CodOrdem = ordem,
                            ProReitoria = ProReitoria.ListarPorCodigo(item.id)
                        };
                        this.AviPublico.Add(proReitoria);
                        break;

                    case "Reitoria":
                        AviPublico reitoria = new AviPublico
                        {
                            CodAviTipoPublico = 2,
                            CodOrdem = ordem,
                            Reitoria = Reitoria.ListarPorCodigo(item.id)
                        };
                        this.AviPublico.Add(reitoria);
                        break;

                    case "Instituição":
                        AviPublico instituicao = new AviPublico
                        {
                            CodAviTipoPublico = 1,
                            CodOrdem = ordem,
                            Instituicao = Instituicao.ListarPorCodigo(int.Parse(item.id))
                        };
                        this.AviPublico.Add(instituicao);
                        break;

                    default:
                        break;
                }
                ordem++;
            }
            contexto.SaveChanges();
        }
    }
}