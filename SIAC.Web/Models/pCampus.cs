using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Campus
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();

        public static List<Campus> ListarOrdenadamente()
        {
            return contexto.Campus.OrderBy(c => c.Sigla).ToList();
        }

        public static void Inserir(Campus campus)
        {
            contexto.Campus.Add(campus);
            contexto.SaveChanges();
        }
        
        public static Campus ListarPorCodigo(int codCampus)
        {
            return contexto.Campus.FirstOrDefault(c => c.CodCampus == codCampus);
        }
    }
}