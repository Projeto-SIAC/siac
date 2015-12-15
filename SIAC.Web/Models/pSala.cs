using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Sala
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Sala> ListarOrdenadamente() => contexto.Sala.OrderBy(s => s.Descricao).ToList();

        public static Sala ListarPorCodigo(int codSala) => contexto.Sala.Find(codSala);

        public static void Inserir(Sala sala)
        {
            contexto.Sala.Add(sala);
            contexto.SaveChanges();
        }
    }
}