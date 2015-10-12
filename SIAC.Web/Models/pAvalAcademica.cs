using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class AvalAcademica
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }
        
        public static void Inserir(AvalAcademica avalAcademica)
        {
            contexto.AvalAcademica.Add(avalAcademica);
            contexto.SaveChanges();
        }

        public static void Agendar(AvalAcademica avalAcad)
        {
            AvalAcademica temp = contexto.AvalAcademica.FirstOrDefault(acad => acad.Ano == avalAcad.Ano && acad.Semestre == avalAcad.Semestre && acad.NumIdentificador == avalAcad.NumIdentificador && acad.CodTipoAvaliacao == avalAcad.CodTipoAvaliacao);

            temp.Turma = avalAcad.Turma;
            temp.Sala = avalAcad.Sala;
            temp.Avaliacao.DtAplicacao = avalAcad.Avaliacao.DtAplicacao;
            temp.Avaliacao.Duracao = avalAcad.Avaliacao.Duracao;

            contexto.SaveChanges();
        }

        public static List<AvalAcademica> ListarPorProfessor(int codProfessor)
        {
            return contexto.AvalAcademica.Where(ac => ac.CodProfessor == codProfessor)
                                         .OrderBy(ac => ac.NumTurma)
                                         .ThenByDescending(ac=>ac.NumIdentificador).ToList();
        }

        public static List<AvalAcademica> ListarAgendadaPorProfessor(int codProfessor)
        {
            return contexto.AvalAcademica
                .Where(a => a.CodProfessor == codProfessor && a.Avaliacao.DtAplicacao.HasValue && a.Avaliacao.AvalPessoaResultado.Count == 0)
                .OrderBy(a => a.Avaliacao.DtAplicacao)
                .ToList();
        }

        public static List<AvalAcademica> ListarPorAluno(int codAluno)
        {
            return contexto.AvalAcademica
                .Where(a => a.Turma.TurmaDiscAluno.Where(t => t.CodAluno == codAluno).Count() > 0)
                .OrderByDescending(a => a.Avaliacao.DtCadastro)
                .ToList();
        }

        public static List<AvalAcademica> ListarAgendadaPorAluno(int codAluno)
        {          
            return contexto.AvalAcademica
                .Where(a => a.Turma.TurmaDiscAluno.Where(t => t.CodAluno == codAluno).Count() > 0 && a.Avaliacao.DtAplicacao.HasValue && a.Avaliacao.AvalPessoaResultado.Count == 0)
                .OrderBy(a => a.Avaliacao.DtAplicacao)
                .ToList();
        }

        public static AvalAcademica ListarPorCodigoAvaliacao(string codigo)
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

                AvalAcademica avalAcademica = contexto.AvalAcademica.FirstOrDefault(acad => acad.Ano == ano && acad.Semestre == semestre && acad.NumIdentificador == numIdentificador && acad.CodTipoAvaliacao == codTipoAvaliacao);

                return avalAcademica;
            }
            return null;
        }

        public static bool AlternarLiberar(string codAvaliacao)
        {
            AvalAcademica avalAcad = ListarPorCodigoAvaliacao(codAvaliacao);
            avalAcad.Avaliacao.FlagLiberada = !avalAcad.Avaliacao.FlagLiberada;
            contexto.SaveChanges();
            return avalAcad.Avaliacao.FlagLiberada;
        }

        public static void Persistir()
        {
            contexto.SaveChanges();
        }
    }
}