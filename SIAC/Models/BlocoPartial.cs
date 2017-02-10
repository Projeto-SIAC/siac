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
    public partial class Bloco
    {
        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Bloco> ListarOrdenadamente() => contexto.Bloco.OrderBy(b => b.Descricao).OrderBy(b => b.Campus.Sigla).ToList();

        public static Bloco ListarPorCodigo(int codBloco) => contexto.Bloco.Find(codBloco);

        public static void Inserir(Bloco bloco)
        {
            contexto.Bloco.Add(bloco);
            contexto.SaveChanges();
        }
    }
}