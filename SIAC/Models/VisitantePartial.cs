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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Visitante
    {
        private static Contexto contexto => Repositorio.GetInstance();

        [NotMapped]
        public bool FlagAtivo => this.DtValidade.HasValue ? (this.DtValidade.Value > DateTime.Now) : true;

        public static int ProxCodigo
        {
            get
            {
                int cod;
                if (!Sistema.ProxCodVisitante.HasValue)
                {
                    var v = contexto.Visitante.ToList();
                    if (v.Count > 0)
                    {
                        Sistema.ProxCodVisitante = v.Max(vis => vis.CodVisitante) + 1;
                    }
                    else
                    {
                        Sistema.ProxCodVisitante = 0;
                    }
                }
                else
                {
                    Sistema.ProxCodVisitante++;
                }
                cod = Sistema.ProxCodVisitante.Value;
                return cod;
            }
        }

        public static void Inserir(Visitante visitante)
        {
            contexto.Visitante.Add(visitante);
            contexto.SaveChanges();
        }

        public static List<Visitante> Listar() => contexto.Visitante.ToList();

        public static Visitante ListarPorMatricula(string matricula) =>
            contexto.Visitante.FirstOrDefault(v => v.MatrVisitante.ToLower() == matricula.ToLower());
    }
}