/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
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