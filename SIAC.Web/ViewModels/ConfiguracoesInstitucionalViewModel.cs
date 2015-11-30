using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class ConfiguracoesInstitucionalViewModel
    {
        public List<Ocupacao> Ocupacoes { get; set; }
        public List<PessoaFisica> Coordenadores { get; set; }
    }
}