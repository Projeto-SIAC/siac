using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Colaborador
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(Colaborador colaborador)
        {
            contexto.Colaborador.Add(colaborador);
            contexto.SaveChanges();
        }

        public static List<Colaborador> ListarOrdenadamente() => contexto.Colaborador.OrderBy(c => c.Usuario.PessoaFisica.Nome).ToList();

        public static Colaborador ListarPorMatricula(string matricula) => contexto.Colaborador.FirstOrDefault(c => c.MatrColaborador == matricula);

        public static Colaborador ListarPorCodigo(int codigo) => contexto.Colaborador.Find(codigo);
    }
}