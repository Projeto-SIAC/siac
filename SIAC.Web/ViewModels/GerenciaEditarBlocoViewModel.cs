using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class GerenciaEditarBlocoViewModel
    {
        public Bloco Bloco { get; set; }
        public List<Campus> Campi { get; set; } = new List<Campus>();
    }
}