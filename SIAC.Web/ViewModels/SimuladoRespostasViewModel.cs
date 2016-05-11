using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class SimuladoRespostasViewModel
    {
        public Simulado Simulado { get; set; }
        public List<SimProva> Provas { get; set; }
        public List<SimCandidato> Candidatos { get; set; }
    }
}