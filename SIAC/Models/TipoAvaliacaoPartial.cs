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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class TipoAvaliacao
    {
        public const int AUTOAVALIACAO = 1;
        public const int ACADEMICA = 2;
        public const int CERTIFICACAO = 3;
        public const int REPOSICAO = 4;
        public const int AVI = 5;

        [NotMapped]
        public string DescricaoCurta => CodTipoAvaliacao == 4 ? Descricao : Descricao.Split(' ').Last();

        private static Contexto contexto => Repositorio.GetInstance();

        public static TipoAvaliacao ListarPorCodigo(int codTipoAvaliacao) => contexto.TipoAvaliacao.Find(codTipoAvaliacao);

        public static TipoAvaliacao ListarPorSigla(string sigla) => contexto.TipoAvaliacao.FirstOrDefault(ta => ta.Sigla == sigla);
    }
}