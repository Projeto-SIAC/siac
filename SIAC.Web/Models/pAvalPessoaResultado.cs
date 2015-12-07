using System.Linq;

namespace SIAC.Models
{
    public partial class AvalPessoaResultado
    {
        public bool FlagParcial => Avaliacao.PessoaResposta.Where(r=>!r.RespNota.HasValue && r.CodPessoaFisica == CodPessoaFisica).Count() > 0;

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static void Inserir(AvalPessoaResultado avalPessoaResultado)
        {
            contexto.AvalPessoaResultado.Add(avalPessoaResultado);
            contexto.SaveChanges();
        }
    }
}