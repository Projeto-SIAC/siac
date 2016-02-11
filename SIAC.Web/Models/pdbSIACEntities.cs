using System;
using System.Configuration;
using System.Linq;

namespace SIAC.Models
{
    public partial class dbSIACEntities
    {
        public dbSIACEntities()
            : base(Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? ConfigurationManager.AppSettings["CONNECTION_STRING"])
        {

        }

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