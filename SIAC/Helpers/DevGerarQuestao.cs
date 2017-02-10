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
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Helpers
{
    public class DevGerarQuestao
    {
        public static List<Questao> GerarQuestao(int qte)
        {
            List<Questao> lstQuestao = new List<Questao>();
            var leroLero = new LeroLero();
            List<Professor> lstProfessor = Professor.ListarOrdenadamente();
            List<TipoQuestao> lstTipoQuestao = TipoQuestao.ListarOrdenadamente();
            List<Dificuldade> lstDificuldade = Dificuldade.ListarOrdenadamente();
            for (int i = 0; i < qte; i++)
            {
                var questao = new Questao();
                questao.DtCadastro = DateTime.Now;
                questao.Professor = lstProfessor.ElementAt(Sistema.Random.Next(lstProfessor.Count));
                questao.Dificuldade = lstDificuldade.ElementAt(Sistema.Random.Next(lstDificuldade.Count));
                questao.TipoQuestao = lstTipoQuestao.ElementAt(Sistema.Random.Next(lstTipoQuestao.Count));
                questao.Enunciado = leroLero.Paragrafo();
                if (Sistema.Random.Next(2) == 1)
                {
                    questao.Objetivo = leroLero.Paragrafo();
                }
                List<Disciplina> lstDisciplina = Professor.ObterDisciplinas(questao.Professor.CodProfessor);
                if (lstDisciplina.Count == 0)
                {
                    qte++;
                    continue;
                }
                Disciplina disciplina = lstDisciplina.ElementAt(Sistema.Random.Next(lstDisciplina.Count));
                List<Tema> lstTema = Tema.ListarPorDisciplina(disciplina.CodDisciplina);
                if (lstTema.Count == 0)
                {
                    qte++;
                    continue;
                }
                int qteTema = lstTema.Count > 4 ? Sistema.Random.Next(1, 5) : Sistema.Random.Next(1, lstTema.Count);
                for (int j = 0; j < qteTema; j++)
                {
                    var index = Sistema.Random.Next(lstTema.Count);
                    questao.QuestaoTema.Add(
                        new QuestaoTema
                        {
                            Tema = lstTema.ElementAt(index)
                        }
                    );
                    lstTema.Remove(lstTema.ElementAt(index));
                }
                if (questao.TipoQuestao.CodTipoQuestao == 1)
                {
                    int qteAlternativa = Sistema.Random.Next(3, 6);
                    for (int j = 0; j < qteAlternativa; j++)
                    {
                        questao.Alternativa.Add(
                            new Alternativa
                            {
                                CodOrdem = j,
                                Enunciado = leroLero.Paragrafo(),
                                Comentario = Sistema.Random.Next(2) == 1 ? leroLero.Paragrafo() : null
                            }
                        );
                    }
                    questao.Alternativa.ElementAt(Sistema.Random.Next(questao.Alternativa.Count)).FlagGabarito = true;
                }
                else
                {
                    questao.ChaveDeResposta = leroLero.Paragrafo();
                    if (Sistema.Random.Next(2) == 1)
                    {
                        questao.Comentario = leroLero.Paragrafo();
                    }
                }
                lstQuestao.Add(questao);
            }
            return lstQuestao;
        }
    }
}