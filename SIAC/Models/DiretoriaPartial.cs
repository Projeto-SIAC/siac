using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Diretoria
    {
        [NotMapped]
        public string CodComposto => $"{CodInstituicao}.{CodCampus}.{CodDiretoria}";

        [NotMapped]
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                /*Alunos*/
                pessoas.AddRange(PessoaFisica.ListarPorDiretoria(this.CodComposto));

                /*Diretor*/
                pessoas.Add(this.Colaborador.Usuario.PessoaFisica);

                /*Professores e Colaboradores*/
                foreach (var plt in this.PessoaLocalTrabalho)
                {
                    pessoas.Add(plt.PessoaFisica);
                }

                return pessoas;
            }
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(Diretoria diretoria)
        {
            if (diretoria.Campus != null)
            {
                diretoria.CodCampus = diretoria.Campus.CodCampus;
                diretoria.CodInstituicao = diretoria.Campus.CodInstituicao;
            }
            List<Diretoria> diretorias = contexto.Campus.Find(diretoria.CodInstituicao, diretoria.CodCampus).Diretoria.ToList();
            int id = diretorias.Count > 0 ? diretorias.Max(d => d.CodDiretoria) + 1 : 1;

            diretoria.CodDiretoria = id;
            contexto.Diretoria.Add(diretoria);
            contexto.SaveChanges();
        }

        public static List<Diretoria> ListarOrdenadamente() => contexto.Diretoria.OrderBy(d => d.Sigla).ToList();

        public static Diretoria ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codCampus = int.Parse(codigos[1]);
            int codDiretoria = int.Parse(codigos[2]);

            return contexto.Diretoria
                .FirstOrDefault(d => d.CodInstituicao == codInstituicao
                    && d.CodCampus == codCampus
                    && d.CodDiretoria == codDiretoria);
        }
    }
}