using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Bloco
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Bloco> ListarOrdenadamente() => contexto.Bloco.OrderBy(b => b.Descricao).OrderBy(b => b.Campus.Sigla).ToList();

        public static Bloco ListarPorCodigo(int codBloco) => contexto.Bloco.Find(codBloco);

        public static void Inserir(Bloco bloco)
        {
            contexto.Bloco.Add(bloco);
            contexto.SaveChanges();
        }
    }
}