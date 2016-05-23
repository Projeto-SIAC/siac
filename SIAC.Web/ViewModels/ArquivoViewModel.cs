using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class ArquivoIndexViewModel
    {
        public bool MaisAbertos { get; set; }
        public List<Simulado> Abertos { get; set; } = new List<Simulado>();

        public bool MaisEncerrados { get; set; }
        public List<Simulado> Encerrados { get; set; } = new List<Simulado>();
    }
}