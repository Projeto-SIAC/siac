using System.Linq;

namespace SIAC.Models
{
    public partial class dbSIACEntities
    {
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
    }
}