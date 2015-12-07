using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class ProReitoria
    {
        public string CodComposto => $"{CodInstituicao}.{CodProReitoria}";
        
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();
                
                /*Pro-reitor*/
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

        public static List<ProReitoria> ListarOrdenadamente() => contexto.ProReitoria.OrderBy(c => c.Sigla).ToList();

        public static ProReitoria ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codProReitoria = int.Parse(codigos[1]);

            return contexto.ProReitoria.FirstOrDefault(pr => pr.CodInstituicao == codInstituicao
                                                         && pr.CodProReitoria == codProReitoria);
        }
    }
}