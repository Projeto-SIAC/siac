using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class AvalPessoaResultado
    {
        public bool FlagParcial
        {
            get
            {
                return Avaliacao.PessoaResposta.Where(r=>!r.RespNota.HasValue).Count() > 0;
            }
        }

        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static void Inserir(AvalPessoaResultado avalPessoaResultado)
        {
            contexto.AvalPessoaResultado.Add(avalPessoaResultado);
            contexto.SaveChanges();
        }
    }
}