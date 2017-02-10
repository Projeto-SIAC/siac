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
    public partial class SimCandidato
    {
        [NotMapped]
        public string NumInscricaoRepresentacao => this.NumInscricao.ToString("d4");

        [NotMapped]
        public string EscorePadronizadoFinalString => this.EscorePadronizadoFinal?.ToString("0.000");

        [NotMapped]
        public int? ClassificacaoPosicao
        {
            get
            {
                List<SimCandidato> listagem = this.Simulado.Classificacao;

                int posicao = 0;
                decimal? ultimoEscore = -1;

                if (listagem.FirstOrDefault(a => a.CodCandidato == this.CodCandidato) != null)
                {
                    foreach (var sc in listagem)
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