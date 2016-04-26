using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class GerenciaEditarSalaViewModel
    {
        public List<Campus> Campi { get; set; } = new List<Campus>();
        public List<Bloco> Blocos { get; set; } = new List<Bloco>();
        public Sala Sala { get; set; }
    }
}