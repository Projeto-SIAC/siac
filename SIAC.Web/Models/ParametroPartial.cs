using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace SIAC.Models
{
    public partial class Parametro
    {
        public enum NumeracaoPadrao {
            INDO_ARABICO = 1,
            ROMANOS = 2,
            CAIXA_ALTA = 3,
            CAIXA_BAIXA = 4
        }

        public int[] OcupacaoCoordenadorAvi => JsonConvert.DeserializeObject<int[]>(parametro.CoordenadorAVI).Union(new int[] { Ocupacao.COORDENADOR_AVI }).ToArray();

        private static dbSIACEntities contexto => Repositorio.GetInstance();

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
            temp.NotaUsoSimulado = parametro.NotaUsoSimulado.Trim();

            /* CONFIGURAÇÃO SMTP */
            temp.SmtpEnderecoHost = parametro.SmtpEnderecoHost;
            temp.SmtpPorta = parametro.SmtpPorta;
            temp.SmtpFlagSSL = parametro.SmtpFlagSSL;
            temp.SmtpUsuario = parametro.SmtpUsuario;
            temp.SmtpSenha = parametro.SmtpSenha;

            contexto.SaveChanges();
            parametro = contexto.Parametro.FirstOrDefault();
        }

        public static void AtualizarOcupacoesCoordenadores(int[] ocupacoes)
        {
            parametro.CoordenadorAVI = JsonConvert.SerializeObject(ocupacoes);
            contexto.Parametro.FirstOrDefault().CoordenadorAVI = parametro.CoordenadorAVI;
            contexto.SaveChanges();
        }

        public async static Task<Parametro> ObterAsync() => await contexto.Parametro.FindAsync(1);
    }
}