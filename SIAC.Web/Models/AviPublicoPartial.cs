using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIAC.Models
{
    public partial class AviPublico
    {
        [NotMapped]
        public string CodPublico
        {
            get
            {
                switch (this.CodAviTipoPublico)
                {
                    case AviTipoPublico.INSTITUICAO:
                        return this.Instituicao.CodInstituicao.ToString();

                    case AviTipoPublico.REITORIA:
                        return this.Reitoria.CodComposto;

                    case AviTipoPublico.PRO_REITORIA:
                        return this.ProReitoria.CodComposto;

                    case AviTipoPublico.CAMPUS:
                        return this.Campus.CodComposto;

                    case AviTipoPublico.DIRETORIA:
                        return this.Diretoria.CodComposto;

                    case AviTipoPublico.CURSO:
                        return this.Curso.CodCurso.ToString();

                    case AviTipoPublico.TURMA:
                        return this.Turma.CodTurma;

                    case AviTipoPublico.PESSOA:
                        return this.PessoaFisica.CodPessoa.ToString();

                    default:
                        return String.Empty;
                }
            }
        }

        [NotMapped]
        public string Descricao
        {
            get
            {
                switch (this.CodAviTipoPublico)
                {
                    case AviTipoPublico.INSTITUICAO:
                        return this.Instituicao.PessoaJuridica.NomeFantasia;

                    case AviTipoPublico.REITORIA:
                        return this.Reitoria.PessoaJuridica.NomeFantasia;

                    case AviTipoPublico.PRO_REITORIA:
                        return this.ProReitoria.PessoaJuridica.NomeFantasia;

                    case AviTipoPublico.CAMPUS:
                        return this.Campus.PessoaJuridica.NomeFantasia;

                    case AviTipoPublico.DIRETORIA:
                        return this.Diretoria.PessoaJuridica.NomeFantasia;

                    case AviTipoPublico.CURSO:
                        return this.Curso.Descricao;

                    case AviTipoPublico.TURMA:
                        return $"{this.Turma.Curso.Descricao} ({this.Turma.CodTurma})";

                    case AviTipoPublico.PESSOA:
                        return this.PessoaFisica.Nome;

                    default:
                        return String.Empty;
                }
            }
        }
    }
}