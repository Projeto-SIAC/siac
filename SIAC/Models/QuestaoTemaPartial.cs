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
using System.Linq;

namespace SIAC.Models
{
    public partial class QuestaoTema
    {
        private static Contexto contexto => Repositorio.GetInstance();

        private static int ParamTempoInatividade = Parametro.Obter().TempoInatividade;

        public static List<QuestaoTema> LimparRepeticao(List<QuestaoTema> retorno, List<QuestaoTema> lst1 = null, List<QuestaoTema> lst2 = null)
        {
            List<int> codigos1 = new List<int>();
            List<int> codigos2 = new List<int>();

            codigos1 = lst1 != null ? (from qt in lst1 select qt.CodQuestao).ToList() : codigos1;
            codigos2 = lst2 != null ? (from qt in lst2 select qt.CodQuestao).ToList() : codigos2;

            List<QuestaoTema> ret = new List<QuestaoTema>();

            foreach (var item in retorno)
            {
                if (!codigos1.Contains(item.CodQuestao))
                {
                    if (!codigos2.Contains(item.CodQuestao))
                    {
                        if (PrazoValido(item))
                        {
                            ret.Add(item);
                        }
                    }
                }
            }

            return ret;
        }

        public static bool PrazoValido(QuestaoTema questao)
        {
            Questao q = Questao.ListarPorCodigo(questao.CodQuestao);
            if (q.DtUltimoUso.HasValue)
            {
                DateTime prazo = questao.Questao.DtUltimoUso.Value.AddDays(ParamTempoInatividade);
                if (DateTime.Now >= prazo)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }
    }
}