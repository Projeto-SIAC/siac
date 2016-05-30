using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalPessoaResultado
    {
        [NotMapped]
        public bool FlagParcial => Avaliacao.PessoaResposta.Where(r=>!r.RespNota.HasValue && r.CodPessoaFisica == CodPessoaFisica).Count() > 0;

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static void Inserir(AvalPessoaResultado avalPessoaResultado)
        {
            contexto.AvalPessoaResultado.Add(avalPessoaResultado);
            contexto.SaveChanges();
        }
    }
}