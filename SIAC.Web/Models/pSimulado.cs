using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Simulado
    {
        private static dbSIACEntities contexto = Repositorio.GetInstance();

        public static void Inserir(Simulado simulado)
        {
            contexto.Simulado.Add(simulado);
            contexto.SaveChanges();
        }

        public static int ObterNumIdentificador() => contexto.Simulado.Where(s => s.Ano == DateTime.Now.Year).Count() + 1;
    }
}