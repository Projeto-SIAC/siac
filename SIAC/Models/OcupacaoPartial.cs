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
using System.Linq;

namespace SIAC.Models
{
    public partial class Ocupacao
    {
        public const int SUPERUSUARIO = 0;
        public const int REITOR = 1;
        public const int PRO_REITOR = 2;
        public const int DIRETOR_GERAL = 3;
        public const int DIRETOR = 4;
        public const int COORDENADOR = 5;
        public const int COORDENADOR_AVI = 6;
        public const int COORDENADOR_SIMULADO = 7;
        public const int COLABORADOR_SIMULADO = 8;

        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Ocupacao> Listar() => contexto.Ocupacao.ToList();

        public static Ocupacao ListarPorCodigo(int codOcupacao) => contexto.Ocupacao.Find(codOcupacao);
    }
}