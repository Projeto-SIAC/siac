using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class SimuladoRespostasCandidatoViewModel
    {
        public Simulado Simulado { get; set; }
        public Candidato Candidato { get; set; }
        public SimProva Prova { get; set; }
        public List<SimProvaQuestao> Questoes { get; set; }
        public List<SimCandidatoQuestao> Respostas { get; set; }
    }
}