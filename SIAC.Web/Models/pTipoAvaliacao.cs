﻿using System.Linq;

namespace SIAC.Models
{
    public partial class TipoAvaliacao
    {
        public string DescricaoCurta => CodTipoAvaliacao == 4 ? Descricao : Descricao.Split(' ').Last();

        private static dbSIACEntities contexto => Repositorio.GetInstance();

        public static TipoAvaliacao ListarPorCodigo(int codTipoAvaliacao) => contexto.TipoAvaliacao.FirstOrDefault(ta => ta.CodTipoAvaliacao == codTipoAvaliacao);

        public static TipoAvaliacao ListarPorSigla(string sigla) => contexto.TipoAvaliacao.FirstOrDefault(ta => ta.Sigla == sigla);
    }
}