using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Instituicao
    {
        private static Contexto contexto => Repositorio.GetInstance();

        [NotMapped]
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                /*Alunos*/
                foreach (var campus in this.Campus)
                    pessoas.AddRange(PessoaFisica.ListarPorCampus(campus.CodComposto));

                /*Professores e Colaboradores*/
                pessoas.AddRange(Models.PessoaLocalTrabalho.ListarPorInstituicao(this.CodInstituicao));

                return pessoas;
            }
        }

        public static List<Instituicao> ListarOrdenadamente() => contexto.Instituicao.OrderBy(ins => ins.Sigla).ToList();

        public static Instituicao ListarPorCodigo(int codInstituicao) => contexto.Instituicao.Find(codInstituicao);
    }
}