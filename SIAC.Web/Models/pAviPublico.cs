using System;

namespace SIAC.Models
{
    public partial class AviPublico
    {
        public string CodPublico
        {
            get
            {
                switch (this.CodAviTipoPublico)
                {
                    case 1: /*Instituição*/
                        return this.Instituicao.CodInstituicao.ToString();
                    case 2: /*Reitoria*/
                        return this.Reitoria.CodComposto;
                    case 3: /*Pró-Reitoria*/
                        return this.ProReitoria.CodComposto;
                    case 4: /*Campus*/
                        return this.Campus.CodComposto;
                    case 5: /*Diretoria*/
                        return this.Diretoria.CodComposto;
                    case 6: /*Curso*/
                        return this.Curso.CodCurso.ToString();
                    case 7: /*Turma*/
                        return this.Turma.CodTurma;
                    case 8: /*Pessoa*/
                        return this.PessoaFisica.CodPessoa.ToString();
                    default:
                        return String.Empty;
                }
            }
        }

        public string Descricao
        {
            get
            {
                switch (this.CodAviTipoPublico)
                {
                    case 1: /*Instituição*/
                        return this.Instituicao.PessoaJuridica.NomeFantasia;
                    case 2: /*Reitoria*/
                        return this.Reitoria.PessoaJuridica.NomeFantasia;
                    case 3: /*Pró-Reitoria*/
                        return this.ProReitoria.PessoaJuridica.NomeFantasia;
                    case 4: /*Campus*/
                        return this.Campus.PessoaJuridica.NomeFantasia;
                    case 5: /*Diretoria*/
                        return this.Diretoria.PessoaJuridica.NomeFantasia;
                    case 6: /*Curso*/
                        return this.Curso.Descricao;
                    case 7: /*Turma*/
                        return $"{this.Turma.Curso.Descricao} ({this.Turma.CodTurma})";
                    case 8: /*Pessoa*/
                        return this.PessoaFisica.Nome;
                    default:
                        return String.Empty;
                }
            }
        }
    }
}