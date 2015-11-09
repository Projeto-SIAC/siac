using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AvalCertificacao
    {
        public List<PessoaFisica> PessoasRealizaram
        {
            get
            {
                List<PessoaFisica> result = new List<PessoaFisica>();
                foreach (PessoaFisica a in this.PessoaFisica)
                {
                    var lstRespostas = this.Avaliacao.PessoaResposta.Where(p => p.CodPessoaFisica == a.CodPessoa);
                    if (lstRespostas.Count() > 0)
                        result.Add(a);
                }
                return result;
            }
        }

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(AvalCertificacao avalCertificacao)
        {
            contexto.AvalCertificacao.Add(avalCertificacao);
            contexto.SaveChanges();
        }

        public static AvalCertificacao ListarPorCodigoAvaliacao(string codigo)
        {
            int numIdentificador = 0;
            int semestre = 0;
            int ano = 0;

            if (codigo.Length == 13)
            {

                int.TryParse(codigo.Substring(codigo.Length - 4), out numIdentificador);
                codigo = codigo.Remove(codigo.Length - 4);
                int.TryParse(codigo.Substring(codigo.Length - 1), out semestre);
                codigo = codigo.Remove(codigo.Length - 1);
                int.TryParse(codigo.Substring(codigo.Length - 4), out ano);
                codigo = codigo.Remove(codigo.Length - 4);

                int codTipoAvaliacao = TipoAvaliacao.ListarPorSigla(codigo).CodTipoAvaliacao;

                AvalCertificacao avalCert = contexto.AvalCertificacao.FirstOrDefault(acad => acad.Ano == ano && acad.Semestre == semestre && acad.NumIdentificador == numIdentificador && acad.CodTipoAvaliacao == codTipoAvaliacao);

                return avalCert;
            }
            return null;
        }

        internal static List<AvalCertificacao> ListarPorPessoa(int codPessoaFisica)
        {
            return contexto.AvalCertificacao.Where(ac => ac.PessoaFisica.FirstOrDefault(p=>p.CodPessoa == codPessoaFisica) != null)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();
        }

        public static List<AvalCertificacao> ListarPorProfessor(int codProfessor)
        {
            return contexto.AvalCertificacao.Where(ac => ac.CodProfessor == codProfessor)
                                         .OrderByDescending(ac => ac.Avaliacao.DtCadastro)
                                         .ToList();
        }
    }
}