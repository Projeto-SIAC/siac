using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class PessoaFisica
    {
        public string PrimeiroNome => this.Nome.Split(' ').First();
        public string UltimoNome => this.Nome.Split(' ').Last();

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static int Inserir(PessoaFisica pessoaFisica)
        {
            contexto.PessoaFisica.Add(pessoaFisica);
            contexto.SaveChanges();
            return pessoaFisica.CodPessoa;
        }

        public static void AdicionarOcupacao(int codPessoaFisica, int codOcupacao)
        {
            var pessoa = ListarPorCodigo(codPessoaFisica);
            pessoa?.Ocupacao.Add(Models.Ocupacao.ListarPorCodigo(codOcupacao));
            contexto.SaveChanges();
        }

        public static void RemoverOcupacao(int codPessoaFisica, int codOcupacao)
        {
            var pessoa = ListarPorCodigo(codPessoaFisica);
            pessoa?.Ocupacao.Remove(Models.Ocupacao.ListarPorCodigo(codOcupacao));
            contexto.SaveChanges();
        }

        public static List<PessoaFisica> Listar()
        {
            return contexto.PessoaFisica.ToList();
        }

        public static PessoaFisica ListarPorCpf(string cpf)
        {
            return contexto.PessoaFisica.FirstOrDefault(p => p.Cpf == cpf);
        }

        public static PessoaFisica ListarPorCodigo(int codPessoaFisica)
        {
            return contexto.PessoaFisica.FirstOrDefault(p=>p.CodPessoa == codPessoaFisica);
        }

        public static List<PessoaFisica> ListarPorTurma(string codTurma)
        {
            var dtHoje = DateTime.Now;
            var ano = dtHoje.Year;
            var semestre = dtHoje.Month > 6 ? 2 : 1;
            return Turma.ListarPorCodigo(codTurma).TurmaDiscAluno.Where(a=>a.AnoLetivo == ano && a.SemestreLetivo == semestre).Select(a => a.Aluno.Usuario.PessoaFisica).ToList();
        }

        public static List<PessoaFisica> ListarPorCurso(int codCurso)
        {
            var dtHoje = DateTime.Now;
            var ano = dtHoje.Year;
            var semestre = dtHoje.Month > 6 ? 2 : 1;
            var lstPessoaFisica = new List<PessoaFisica>();
            foreach (var turma in Curso.ListarPorCodigo(codCurso).Turma)
            {
                lstPessoaFisica.AddRange(turma.TurmaDiscAluno.Where(a => a.AnoLetivo == ano && a.SemestreLetivo == semestre).Select(a => a.Aluno.Usuario.PessoaFisica).ToList());
            }
            return lstPessoaFisica;
        }

        public static List<PessoaFisica> ListarPorDiretoria(string codComposto)
        {
            var dtHoje = DateTime.Now;
            var ano = dtHoje.Year;
            var semestre = dtHoje.Month > 6 ? 2 : 1;

            var lstPessoaFisica = new List<PessoaFisica>();

            foreach (var curso in Diretoria.ListarPorCodigo(codComposto).Curso)
            {
                foreach (var turma in curso.Turma)
                {
                    lstPessoaFisica.AddRange(turma.TurmaDiscAluno.Where(a => a.AnoLetivo == ano && a.SemestreLetivo == semestre).Select(a => a.Aluno.Usuario.PessoaFisica).ToList());
                }
            }

            return lstPessoaFisica;
        }

        public static List<PessoaFisica> ListarPorCampus(string codComposto)
        {
            var dtHoje = DateTime.Now;
            var ano = dtHoje.Year;
            var semestre = dtHoje.Month > 6 ? 2 : 1;

            var lstPessoaFisica = new List<PessoaFisica>();

            foreach (var diretoria in Campus.ListarPorCodigo(codComposto).Diretoria)
            {
                foreach (var curso in diretoria.Curso)
                {
                    foreach (var turma in curso.Turma)
                    {
                        lstPessoaFisica.AddRange(turma.TurmaDiscAluno.Where(a => a.AnoLetivo == ano && a.SemestreLetivo == semestre).Select(a => a.Aluno.Usuario.PessoaFisica).ToList());
                    }
                }
            }

            return lstPessoaFisica;
        }

        //public static List<PessoaFisica> ListarPorProReitoria(string codComposto)
        //{

        //    var dtHoje = DateTime.Now;
        //    var ano = dtHoje.Year;
        //    var semestre = dtHoje.Month > 6 ? 2 : 1;

        //    var lstPessoaFisica = new List<PessoaFisica>();


        //    return lstPessoaFisica;
        //}

        //public static List<PessoaFisica> ListarPorReitoria(string codComposto)
        //{

        //    var dtHoje = DateTime.Now;
        //    var ano = dtHoje.Year;
        //    var semestre = dtHoje.Month > 6 ? 2 : 1;

        //    var lstPessoaFisica = new List<PessoaFisica>();


        //    return lstPessoaFisica;
        //}

        //public static List<PessoaFisica> ListarPorInstituicao(int codInstituicao)
        //{
        //    var dtHoje = DateTime.Now;
        //    var ano = dtHoje.Year;
        //    var semestre = dtHoje.Month > 6 ? 2 : 1;

        //    var lstPessoaFisica = new List<PessoaFisica>();



        //    return lstPessoaFisica;
        //}
    }
}