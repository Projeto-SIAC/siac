using System.Collections.Generic;
using System.Linq;

namespace SIAC.Models
{
    public partial class AvalAuto
    {
        public List<Disciplina> Disciplina => this.Avaliacao.AvaliacaoTema.Select(at => at.Tema.Disciplina).Distinct().ToList();

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static void Inserir(AvalAuto AvalAuto)
        {
            contexto.AvalAuto.Add(AvalAuto);
            contexto.SaveChanges();
        }

        public static List<AvalAuto> ListarPorPessoa(int codPessoaFisica) => contexto.AvalAuto.Where(auto => auto.CodPessoaFisica == codPessoaFisica).OrderByDescending(auto => auto.Avaliacao.DtCadastro).ToList();

        public static AvalAuto ListarPorCodigoAvaliacao(string codigo) => Avaliacao.ListarPorCodigoAvaliacao(codigo)?.AvalAuto;

        public static List<AvalAuto> ListarNaoRealizadaPorPessoa(int codPessoaFisica)
        {
            List<AvalAuto> autoavaliacoes = contexto.AvalAuto.Where(auto => auto.CodPessoaFisica == codPessoaFisica).ToList();
            return autoavaliacoes.Where(a => a.Avaliacao.FlagPendente).ToList();
        }
    }
}