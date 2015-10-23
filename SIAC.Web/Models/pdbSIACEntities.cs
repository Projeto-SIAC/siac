using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class dbSIACEntities
    {
        public override int SaveChanges()
        {
            Sistema.AlertarMudanca.AddRange(Sistema.MatriculaAtivo);
            Sistema.AlertarMudanca = Sistema.AlertarMudanca.Distinct().ToList();
            Sistema.AlertarMudanca.Remove(Helpers.Sessao.UsuarioMatricula);
            return base.SaveChanges();
        }
    }
}