using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Web.Models
{
    public partial class AvalAcademica
    {
        private static dbSIACEntities contexto = DataContextSIAC.GetInstance();
        
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
            return contexto.AvalAcademica.Where(ac => ac.CodProfessor == codProfessor).ToList();
        }

        public static AvalAcademica ListarPorCodigoAvaliacao(string codigo)
        {
            int numIdentificador = int.Parse(codigo.Substring(codigo.Length - 4));
            codigo = codigo.Remove(codigo.Length - 4);
            int semestre = int.Parse(codigo.Substring(codigo.Length - 1));
            codigo = codigo.Remove(codigo.Length - 1);
            int ano = int.Parse(codigo.Substring(codigo.Length - 4));
            codigo = codigo.Remove(codigo.Length - 4);

            int codTipoAvaliacao = TipoAvaliacao.ListarPorSigla(codigo).CodTipoAvaliacao;

            AvalAcademica avalAcademica = contexto.AvalAcademica.FirstOrDefault(acad => acad.Ano == ano && acad.Semestre == semestre && acad.NumIdentificador == numIdentificador && acad.CodTipoAvaliacao == codTipoAvaliacao);

            return avalAcademica;
        }
    }
}