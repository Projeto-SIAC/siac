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
    public partial class Diretoria
    {
        [NotMapped]
        public string CodComposto => $"{CodInstituicao}.{CodCampus}.{CodDiretoria}";

        [NotMapped]
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                /*Alunos*/
                pessoas.AddRange(PessoaFisica.ListarPorDiretoria(this.CodComposto));

                /*Diretor*/
                pessoas.Add(this.Colaborador.Usuario.PessoaFisica);

                /*Professores e Colaboradores*/
                foreach (var plt in this.PessoaLocalTrabalho)
                {
                    pessoas.Add(plt.PessoaFisica);
                }

                return pessoas;
            }
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static void Inserir(Diretoria diretoria)
        {
            List<Diretoria> diretorias = contexto.Campus.Find(diretoria.CodInstituicao, diretoria.CodCampus).Diretoria.ToList();
            int id = diretorias.Count > 0 ? diretorias.Max(d => d.CodDiretoria) + 1 : 1;

            diretoria.CodDiretoria = id;
            contexto.Diretoria.Add(diretoria);
            contexto.SaveChanges();
        }

        public static List<Diretoria> ListarOrdenadamente() => contexto.Diretoria.OrderBy(d => d.Sigla).ToList();

        public static Diretoria ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codCampus = int.Parse(codigos[1]);
            int codDiretoria = int.Parse(codigos[2]);

            return contexto.Diretoria
                .FirstOrDefault(d => d.CodInstituicao == codInstituicao
                    && d.CodCampus == codCampus
                    && d.CodDiretoria == codDiretoria);
        }
    }
}