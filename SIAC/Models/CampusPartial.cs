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
    public partial class Campus
    {
        [NotMapped]
        public string CodComposto => $"{CodInstituicao}.{CodCampus}";

        [NotMapped]
        public List<PessoaFisica> Pessoas
        {
            get
            {
                List<PessoaFisica> pessoas = new List<PessoaFisica>();

                /*Alunos*/
                pessoas.AddRange(PessoaFisica.ListarPorCampus(this.CodComposto));

                /*Diretor-geral*/
                pessoas.Add(this.Colaborador.Usuario.PessoaFisica);

                /*Professores e Colaboradores*/
                pessoas.AddRange(Models.PessoaLocalTrabalho.ListarPorCampus(this.CodComposto));

                return pessoas;
            }
        }

        private static Contexto contexto => Repositorio.GetInstance();

        public static List<Campus> ListarOrdenadamente() => contexto.Campus.OrderBy(c => c.Sigla).ToList();

        public static void Inserir(Campus campus)
        {
            List<Campus> campi = contexto.Instituicao.Find(campus.CodInstituicao).Campus.ToList();
            int codCampus = campi.Count > 0 ? campi.Max(c => c.CodCampus) + 1 : 1;

            campus.CodCampus = codCampus;

            contexto.Campus.Add(campus);
            contexto.SaveChanges();
        }

        public static Campus ListarPorCodigo(string codComposto)
        {
            string[] codigos = codComposto.Split('.');
            int codInstituicao = int.Parse(codigos[0]);
            int codCampus = int.Parse(codigos[1]);

            return contexto.Campus.FirstOrDefault(c => c.CodInstituicao == codInstituicao && c.CodCampus == codCampus);
        }

        public static void TrocarDiretor(Campus campus, int codDiretorNovo)
        {
            Colaborador diretorAnterior = Colaborador.ListarPorCodigo(campus.CodColaboradorDiretor);
            Colaborador diretorNovo = Colaborador.ListarPorCodigo(codDiretorNovo);

            PessoaFisica.RemoverOcupacao(diretorAnterior.Usuario.CodPessoaFisica, Ocupacao.DIRETOR_GERAL);
            PessoaFisica.AdicionarOcupacao(diretorNovo.Usuario.CodPessoaFisica, Ocupacao.DIRETOR_GERAL);

            campus.Colaborador = diretorNovo;

            contexto.SaveChanges();
        }

        public static void Remover(Campus campus)
        {
            contexto.Campus.Remove(campus);
            contexto.SaveChanges();
        }
    }
}