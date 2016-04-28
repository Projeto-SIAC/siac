using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class InscricaoIndexViewModel
    {
        public List<Simulado> Simulados { get; set; } = new List<Simulado>();
    }
}