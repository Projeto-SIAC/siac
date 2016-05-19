using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class SimCandidato
    {
        public string NumInscricaoRepresentacao => this.NumInscricao.ToString("d4");

        public string EscorePadronizadoFinalString => this.EscorePadronizadoFinal?.ToString(".000");

        public int? ClassificacaoPosicao
        {
            get
            {
                List<SimCandidato> listagem = this.Simulado.Classificacao;

                int posicao = 0;
                decimal? ultimoEscore = -1;

                if (listagem.FirstOrDefault(a => a.CodCandidato == this.CodCandidato) != null)
                {
                    foreach (SimCandidato sc in listagem)
                    {
                        if (ultimoEscore != sc.EscorePadronizadoFinal)
                        {
                            posicao++;
                        }
                        if (sc.CodCandidato == this.CodCandidato)
                        {
                            return posicao;
                        }
                        ultimoEscore = sc.EscorePadronizadoFinal;
                    }
                }

                return null;
            }
        }
    }
}