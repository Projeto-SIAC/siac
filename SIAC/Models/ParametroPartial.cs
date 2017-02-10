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
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SIAC.Models
{
    public partial class Parametro
    {
        public enum NumeracaoPadrao
        {
            INDO_ARABICO = 1,
            ROMANOS = 2,
            CAIXA_ALTA = 3,
            CAIXA_BAIXA = 4
        }

        [NotMapped]
        public int[] OcupacaoCoordenadorAvi => JsonConvert.DeserializeObject<int[]>(parametro.CoordenadorAVI).Union(new int[] { Ocupacao.COORDENADOR_AVI }).ToArray();

        private static Contexto contexto => Repositorio.GetInstance();

        private static Parametro parametro;

        public static Parametro Obter()
        {
            if (parametro == null)
            {
                using (var e = new Contexto())
                    parametro = e.Parametro.FirstOrDefault();
            }
            return parametro;
        }

        public static void Atualizar(Parametro p)
        {
            Parametro temp = contexto.Parametro.FirstOrDefault();

            temp.TempoInatividade = p.TempoInatividade;
            temp.NumeracaoQuestao = p.NumeracaoQuestao;
            temp.NumeracaoAlternativa = p.NumeracaoAlternativa;
            temp.QteSemestres = p.QteSemestres;
            temp.TermoResponsabilidade = p.TermoResponsabilidade.Trim();
            temp.ValorNotaMedia = p.ValorNotaMedia;
            temp.NotaUsoAcademica = p.NotaUsoAcademica.Trim();
            temp.NotaUsoCertificacao = p.NotaUsoCertificacao.Trim();
            temp.NotaUsoInstitucional = p.NotaUsoInstitucional.Trim();
            temp.NotaUsoReposicao = p.NotaUsoReposicao.Trim();
            temp.NotaUsoSimulado = p.NotaUsoSimulado.Trim();

            /* CONFIGURAÇÃO SMTP */
            temp.SmtpEnderecoHost = p.SmtpEnderecoHost;
            temp.SmtpPorta = p.SmtpPorta;
            temp.SmtpFlagSSL = p.SmtpFlagSSL;
            temp.SmtpUsuario = p.SmtpUsuario;
            temp.SmtpSenha = p.SmtpSenha;

            contexto.SaveChanges();
            parametro = null;
        }

        public static void AtualizarOcupacoesCoordenadores(int[] ocupacoes)
        {
            var ocupacoesAvi = ocupacoes.ToList();
            ocupacoesAvi.Add(Ocupacao.SUPERUSUARIO);
            parametro.CoordenadorAVI = JsonConvert.SerializeObject(ocupacoesAvi);
            contexto.Parametro.FirstOrDefault().CoordenadorAVI = parametro.CoordenadorAVI;
            contexto.SaveChanges();
        }

        public async static Task<Parametro> ObterAsync() => await contexto.Parametro.FindAsync(1);
    }
}