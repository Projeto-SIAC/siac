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
        public List<Disciplina> Disciplinas { get; set; }
    }
}