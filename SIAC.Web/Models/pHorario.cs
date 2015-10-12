using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class Horario
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static List<Horario> ListarOrdenadamente()
        {
            return contexto.Horario.OrderBy(h => h.CodTurno).OrderBy(h=>h.CodGrupo).ToList();
        }

        public static void Inserir(Horario horario)
        {
            contexto.Horario.Add(horario);
            contexto.SaveChanges();
        }

        public static void Inserir(List<Horario> horarios)
        {
            contexto.Horario.AddRange(horarios);
            contexto.SaveChanges();
        }
    }
}