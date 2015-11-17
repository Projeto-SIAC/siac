using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class AcessoIndexViewModel
    {
        public string Matricula { get; set; }
        public bool Erro { get; set; }
        public string[] Mensagens { get; set; } = new string[] { };
    }
}