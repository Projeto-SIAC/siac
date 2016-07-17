using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class SimProva
    {
        [NotMapped]
        public string CodComposto => $"{this.SimDiaRealizacao.Simulado.Codigo}.{this.CodDiaRealizacao}.{this.CodProva}";

        [NotMapped]
        public int QteQuestoesObjetivas =>
            this.SimProvaQuestao.Count(q => q.Questao.CodTipoQuestao == TipoQuestao.OBJETIVA);

        [NotMapped]
        public int QteQuestoesDiscursivas =>
            this.SimProvaQuestao.Count(q => q.Questao.CodTipoQuestao == TipoQuestao.DISCURSIVA);

        public bool AdicionarQuestao(int codQuestao)
        {
            var questao = Questao.ListarPorCodigo(codQuestao);
            if (questao.CodTipoQuestao == this.TipoQuestoes)
            {
                this.SimProvaQuestao.Add(new SimProvaQuestao()
                {
                    Questao = questao,
                });
                contexto.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoverQuestao(int codQuestao)
        {
            SimProvaQuestao simProvaQuestao = this.SimProvaQuestao.FirstOrDefault(q => q.CodQuestao == codQuestao);
            if (simProvaQuestao != null)
            {
                this.SimProvaQuestao.Remove(simProvaQuestao);
                contexto.SaveChanges();
                return true;
            }
            return false;
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static List<SimProva> ListarPorProfessor(int codProfessor) =>
            contexto.SimProva.Where(sp => sp.CodProfessor == codProfessor)
                .OrderBy(p => p.SimDiaRealizacao.DtRealizacao)
                .ToList();

        public static List<SimProva> ListarPorProfessor(string matricula)
        {
            Professor professor = Professor.ListarPorMatricula(matricula);
            if (professor != null)
                return contexto.SimProva.Where(sp => sp.CodProfessor == professor.CodProfessor)
                    .OrderBy(p => p.SimDiaRealizacao.DtRealizacao)
                    .ToList();
            else
                return new List<SimProva>();
        }
    }
}