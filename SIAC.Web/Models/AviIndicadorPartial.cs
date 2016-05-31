using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AviIndicador
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<AviIndicador> ListarOrdenadamente() => contexto.AviIndicador.OrderBy(i => i.Descricao).ToList();

        public static void Inserir(AviIndicador indicador)
        {
            contexto.AviIndicador.Add(indicador);
            contexto.SaveChanges();
        }
    }
}