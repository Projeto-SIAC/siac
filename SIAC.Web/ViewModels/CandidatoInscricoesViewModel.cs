using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class CandidatoInscricoesViewModel
    {
        public const int QtePorPagina = 10;
        public List<Simulado> Simulados { get; set; }
        public int PaginaAtual { get; set; }
        public bool TemProxima { get; set; }
        public bool TemAnterior => PaginaAtual > 1;
    }
}