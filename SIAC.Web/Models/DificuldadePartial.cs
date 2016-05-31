using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Dificuldade
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Dificuldade> ListarOrdenadamente() => contexto.Dificuldade.OrderBy(d => d.CodDificuldade).ToList();

        public static Dificuldade ListarPorCodigo(int codDificuldade) => contexto.Dificuldade.Find(codDificuldade);
    }
}