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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Instituicao
    {
        private static Contexto contexto => Repositorio.GetInstance();

        [NotMapped]
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                /*Alunos*/
                foreach (var campus in this.Campus)
                    pessoas.AddRange(PessoaFisica.ListarPorCampus(campus.CodComposto));

                /*Professores e Colaboradores*/
                pessoas.AddRange(Models.PessoaLocalTrabalho.ListarPorInstituicao(this.CodInstituicao));

                return pessoas;
            }
        }

        public static List<Instituicao> ListarOrdenadamente() => contexto.Instituicao.OrderBy(ins => ins.Sigla).ToList();

        public static Instituicao ListarPorCodigo(int codInstituicao) => contexto.Instituicao.Find(codInstituicao);
    }
}