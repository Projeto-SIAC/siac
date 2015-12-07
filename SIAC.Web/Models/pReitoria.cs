using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class Reitoria
    {
        public string CodComposto => $"{CodInstituicao}.{CodReitoria}";
        
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                /*Reitor*/
                pessoas.Add(this.Colaborador.Usuario.PessoaFisica);

                /*Professores e Colaboradores*/
                foreach (PessoaLocalTrabalho plt in this.PessoaLocalTrabalho)
                {
                    pessoas.Add(plt.PessoaFisica);
                }

                return pessoas;
            }
        }
        
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<Reitoria> ListarOrdenadamente() => contexto.Reitoria.OrderBy(c => c.Sigla).ToList();

        public static Reitoria ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codReitoria = int.Parse(codigos[1]);

            return contexto.Reitoria.FirstOrDefault(r => r.CodInstituicao == codInstituicao
                                                    && r.CodReitoria == codReitoria);
        }
    }
}