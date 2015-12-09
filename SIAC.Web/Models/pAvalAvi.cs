using System;
using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalAvi
    {
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

        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                foreach (AviPublico publico in this.Publico)
                {
                    switch (publico.CodAviTipoPublico)
                    {
                        case 1: /*Institução*/
                            pessoas.AddRange(publico.Instituicao.Pessoas);
                            break;
                        case 2: /*Reitoria*/
                            pessoas.AddRange(publico.Reitoria.Pessoas);
                            break;
                        case 3: /*Pró-Reitoria*/
                            pessoas.AddRange(publico.ProReitoria.Pessoas);
                            break;
                        case 4: /*Campus*/
                            pessoas.AddRange(publico.Campus.Pessoas);
                            break;
                        case 5: /*Diretoria*/
                            pessoas.AddRange(publico.Diretoria.Pessoas);
                            break;
                        case 6: /*Curso*/
                            pessoas.AddRange(PessoaFisica.ListarPorCurso(publico.Curso.CodCurso));
                            break;
                        case 7: /*Turma*/
                            pessoas.AddRange(PessoaFisica.ListarPorTurma(publico.Turma.CodTurma));
                            break;
                        case 8: /*Pessoa*/
                            pessoas.Add(publico.PessoaFisica);
                            break;
                        default: /*Default*/
                            break;
                    }
                }

                return pessoas.Distinct().ToList();
            }
        }

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

        public List<AviPublico> Publico => this.FlagPublico ? this.AviPublico.ToList() : null;
        
        public bool FlagQuestionario => this.Questoes.Count > 0;

        public bool FlagPublico => this.AviPublico.Count > 0;

        public bool FlagAgendada => this.Avaliacao.DtAplicacao.HasValue && this.DtTermino.HasValue;

        public bool FlagRealizada => this.Questoes.FirstOrDefault(a => a.AviQuestaoPessoaResposta.Count > 0) != null;

        public bool FlagAndamento => this.FlagAgendada && this.Avaliacao.DtAplicacao.Value <= DateTime.Now && this.DtTermino.Value >= DateTime.Now;

        public bool FlagConcluida => this.DtTermino < DateTime.Now;

        public int QteQuestoes => this.Questoes.Count;

        private static dbSIACEntities contexto => Repositorio.GetInstance();

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
            Models.Colaborador colaborador = Models.Colaborador.ListarPorMatricula(matricula);

            if (colaborador != null)
            {
                return contexto.AvalAvi.Where(avi => avi.CodColabCoordenador == colaborador.CodColaborador).ToList();
            }
            return new List<AvalAvi>();
        }

        public static List<AvalAvi> ListarPorUsuario(string matricula)
        {
            PessoaFisica pessoa = PessoaFisica.ListarPorMatricula(matricula);

            if (pessoa != null)
            {
                List<AvalAvi> institucionais = contexto.AvalAvi.Where(avi =>avi.Avaliacao.DtAplicacao <= DateTime.Now && avi.DtTermino >= DateTime.Now).ToList();

                List<AvalAvi> retorno = new List<AvalAvi>();

                foreach (AvalAvi avi in institucionais)
                {
                    if (avi.Pessoas.Contains(pessoa))
                    {
                        retorno.Add(avi);
                    }
                }
                return retorno;
            }
            return new List<AvalAvi>();
        }

        public static AvalAvi ListarPorCodigoAvaliacao(string codigo)
        {
            int numIdentificador = 0;
            int semestre = 0;
            int ano = 0;

            if (codigo.Length == 12)
            {

                int.TryParse(codigo.Substring(codigo.Length - 4), out numIdentificador);
                codigo = codigo.Remove(codigo.Length - 4);
                int.TryParse(codigo.Substring(codigo.Length - 1), out semestre);
                codigo = codigo.Remove(codigo.Length - 1);
                int.TryParse(codigo.Substring(codigo.Length - 4), out ano);
                codigo = codigo.Remove(codigo.Length - 4);

                int codTipoAvaliacao = TipoAvaliacao.ListarPorSigla(codigo).CodTipoAvaliacao;

                AvalAvi avalAvi = contexto.AvalAvi.FirstOrDefault(avi => avi.Ano == ano && avi.Semestre == semestre && avi.NumIdentificador == numIdentificador && avi.CodTipoAvaliacao == codTipoAvaliacao);

                return avalAvi;
            }
            return null;
        }

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

                    AviQuestao questaoAntiga = aviQuestoes.FirstOrDefault(q => q.CodAviModulo == modulo
                                                                           && q.CodAviCategoria == categoria
                                                                           && q.CodAviIndicador == indicador
                                                                           && q.CodOrdem == ordem);

                    if (questaoAntiga != null)
                    {
                        AviQuestao questaoNova = questaoAntiga;
                        List<AviQuestaoAlternativa> alternativas = questaoNova.AviQuestaoAlternativa.ToList();
                        Models.AviQuestao.Remover(questaoAntiga);
                        //contexto.AviQuestao.Remove(questaoAntiga); Não funcionou assim '-'
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
                    //this.Questoes.Clear();
                    //contexto.AviQuestao.RemoveRange(contexto.AviQuestao.Where(aq => aq.Ano == this.Ano
                    //                           && aq.Semestre == this.Semestre
                    //                           && aq.CodTipoAvaliacao == this.CodTipoAvaliacao
                    //                           && aq.NumIdentificador == this.NumIdentificador).ToList());

                    //contexto.AviQuestao.RemoveRange(aviQuestoes);
                    contexto.AviQuestao.AddRange(aviQuestoesNova);

                    contexto.SaveChanges();
                }
            }
        }

        public void InserirPublico(List<Selecao> publico)
        {
            int ordem = 1;
            foreach (var item in publico)
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