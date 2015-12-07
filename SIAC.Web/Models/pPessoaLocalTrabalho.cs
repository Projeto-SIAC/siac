using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class PessoaLocalTrabalho
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static List<PessoaFisica> ListarPorInstituicao(int codInstituicao)
        {
            return contexto.PessoaLocalTrabalho
                .Where(plt => plt.CodInstituicao == codInstituicao)
                .Select(plt => plt.PessoaFisica)
                .ToList();
        }

        public static List<PessoaFisica> ListarPorCampus(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codCampus = int.Parse(codigos[1]);

            return contexto.PessoaLocalTrabalho
                .Where(plt => plt.CodInstituicao == codInstituicao && plt.CodCampus == codCampus)
                .Select(plt => plt.PessoaFisica)
                .ToList();

        }
    }
}