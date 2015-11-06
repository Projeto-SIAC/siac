using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AvalAuto
    {     
        public List<Disciplina> Disciplina
        {
            get
            {
                return Avaliacao.AvaliacaoTema.Select(at => at.Tema.Disciplina).Distinct().ToList();
            }
        }

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(AvalAuto AvalAuto)
        {
            contexto.AvalAuto.Add(AvalAuto);
            contexto.SaveChanges();
        }

        public static List<AvalAuto> ListarPorPessoa(int codPessoaFisica)
        {
            return contexto.AvalAuto.Where(auto => auto.CodPessoaFisica == codPessoaFisica).OrderByDescending(auto => auto.Avaliacao.DtCadastro).ToList();
        }

        public static AvalAuto ListarPorCodigoAvaliacao(string codigo)
        {
            int numIdentificador = int.Parse(codigo.Substring(codigo.Length - 4));
            codigo = codigo.Remove(codigo.Length - 4);
            int semestre = int.Parse(codigo.Substring(codigo.Length - 1));
            codigo = codigo.Remove(codigo.Length - 1);
            int ano = int.Parse(codigo.Substring(codigo.Length - 4));
            codigo = codigo.Remove(codigo.Length - 4);

            int codTipoAvaliacao = TipoAvaliacao.ListarPorSigla(codigo).CodTipoAvaliacao;

            AvalAuto avalAuto = contexto.AvalAuto.FirstOrDefault(auto => auto.Ano == ano && auto.Semestre == semestre && auto.NumIdentificador == numIdentificador && auto.CodTipoAvaliacao == codTipoAvaliacao);

            return avalAuto;
        }

        public static List<AvalAuto> ListarNaoRealizadaPorPessoa(int codPessoaFisica)
        {
            List<AvalAuto> lst = contexto.AvalAuto.Where(auto => auto.CodPessoaFisica == codPessoaFisica).ToList();
            return lst.Where(a => a.Avaliacao.FlagPendente).ToList();
        }        
    }
}