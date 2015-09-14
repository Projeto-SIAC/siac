using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Disciplina
    {
        private static DataClassesSIACDataContext contexto = DataContextSIAC.GetInstance();
        
        public static List<Disciplina> ListarOrdenadamente()
        {
            return contexto.Disciplina.OrderBy(d => d.Descricao).ToList();
        }
    }
}