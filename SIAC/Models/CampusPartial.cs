using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Campus
    {
        [NotMapped]
        public string CodComposto => $"{CodInstituicao}.{CodCampus}";

        [NotMapped]
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                /*Alunos*/
                pessoas.AddRange(PessoaFisica.ListarPorCampus(this.CodComposto));

                /*Diretor-geral*/
                pessoas.Add(this.Colaborador.Usuario.PessoaFisica);

                /*Professores e Colaboradores*/
                pessoas.AddRange(Models.PessoaLocalTrabalho.ListarPorCampus(this.CodComposto));

                return pessoas;
            }
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Campus> ListarOrdenadamente() => contexto.Campus.OrderBy(c => c.Sigla).ToList();

        public static void Inserir(Campus campus)
        {
            List<Campus> campi = contexto.Instituicao.Find(campus.CodInstituicao).Campus.ToList();
            int codCampus = campi.Count > 0 ? campi.Max(c => c.CodCampus) + 1 : 1;

            campus.CodCampus = codCampus;

            contexto.Campus.Add(campus);
            contexto.SaveChanges();
        }

        public static Campus ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codCampus = int.Parse(codigos[1]);

            return contexto.Campus.FirstOrDefault(c => c.CodInstituicao == codInstituicao && c.CodCampus == codCampus);
        }
    }
}