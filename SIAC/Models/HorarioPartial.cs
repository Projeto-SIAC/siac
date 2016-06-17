using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Horario
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Horario> ListarOrdenadamente() => contexto.Horario.OrderBy(h => h.CodTurno).OrderBy(h => h.CodGrupo).ToList();

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