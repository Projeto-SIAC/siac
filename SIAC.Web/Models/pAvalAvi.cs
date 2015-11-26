using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AvalAvi
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public List<AviQuestao> Questoes
        {
            get
            {
                List<AviQuestao> questoes = new List<AviQuestao>();

                questoes = contexto.AviQuestao.Where(q => q.Ano == this.Ano
                                                       && q.Semestre == this.Semestre
                                                       && q.CodTipoAvaliacao == this.CodTipoAvaliacao
                                                       && q.NumIdentificador == this.NumIdentificador)
                                                       .OrderBy(q=>q.CodAviModulo)
                                                       .ThenBy(q => q.CodAviCategoria)
                                                       .ThenBy(q => q.CodAviIndicador)
                                                       .ThenBy(q => q.CodOrdem)
                                                       .ToList();

                return questoes;
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

        public AviQuestao ObterQuestao(int modulo, int categoria, int indicador, int ordem)
        {
            return this.AviQuestao.FirstOrDefault(q => q.CodAviModulo == modulo
                                                  && q.CodAviCategoria == categoria
                                                  && q.CodAviIndicador == indicador
                                                  && q.CodOrdem == ordem);
        }

        public static void Inserir(AvalAvi avi)
        {
            contexto.AvalAvi.Add(avi);
            contexto.SaveChanges();
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
    }
}