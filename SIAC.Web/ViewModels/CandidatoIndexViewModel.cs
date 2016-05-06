using SIAC.Helpers;
using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class CandidatoIndexViewModel
    {
        public Candidato Candidato => Sessao.Candidato;

        public List<Simulado> Inscritos { get; set; } = new List<Simulado>();

        public List<Simulado> Passados { get; set; } = new List<Simulado>();
    }
}