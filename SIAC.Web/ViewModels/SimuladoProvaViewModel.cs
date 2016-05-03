using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class SimuladoProvaViewModel
    {
        public Simulado Simulado { get; set; }
        public SimProva Prova { get; set; } = new SimProva();
        public List<Disciplina> Disciplinas { get; set; }
    }
}