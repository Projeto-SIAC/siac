using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SIAC.Models
{
    public partial class Parametro
    {
        public int[] OcupacaoCoordenadorAvi => Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(parametro.CoordenadorAVI).Union(new int[] { Sistema.CodOcupacaoCoordenadorAvi }).ToArray();

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        private static Parametro parametro;

        private Parametro() { }

        public static Parametro Obter()
        {
            if (parametro == null)
            {
                using (var e = new dbSIACEntities())
                {
                    parametro = e.Parametro.FirstOrDefault();
                }
            }
            return parametro;
        }

        public static void Atualizar(Parametro parametro)
        {
            Parametro temp = contexto.Parametro.FirstOrDefault();

            temp.TempoInatividade = parametro.TempoInatividade;
            temp.NumeracaoQuestao = parametro.NumeracaoQuestao;
            temp.NumeracaoAlternativa = parametro.NumeracaoAlternativa;
            temp.QteSemestres = parametro.QteSemestres;
            temp.TermoResponsabilidade = parametro.TermoResponsabilidade.Trim();
            temp.ValorNotaMedia = parametro.ValorNotaMedia;
            temp.NotaUsoAcademica = parametro.NotaUsoAcademica.Trim();
            temp.NotaUsoCertificacao = parametro.NotaUsoCertificacao.Trim();
            temp.NotaUsoInstitucional = parametro.NotaUsoInstitucional.Trim();
            temp.NotaUsoReposicao = parametro.NotaUsoReposicao.Trim();

            contexto.SaveChanges();
        }

        public static void AtualizarOcupacoesCoordenadores(int[] ocupacoes)
        {
            parametro.CoordenadorAVI = Newtonsoft.Json.JsonConvert.SerializeObject(ocupacoes);
            contexto.Parametro.FirstOrDefault().CoordenadorAVI = parametro.CoordenadorAVI;
            contexto.SaveChanges();
        }

        public async static Task<Parametro> ObterAsync()
        {
            return await contexto.Parametro.FindAsync(1);
        }
    }
}