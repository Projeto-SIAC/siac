using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Professor
    {
        private static DataClassesSIACDataContext contexto = DataContextSIAC.GetInstance();

        public static Professor ListarPorMatricula(string matricula)
        {
            return contexto.Professor.SingleOrDefault(p => p.MatrProfessor == matricula);
        }
    }
}