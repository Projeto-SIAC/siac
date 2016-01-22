using SIAC.Models;
using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class ConfiguracoesInstitucionalViewModel
    {
        public List<Ocupacao> Ocupacoes { get; set; }
        public List<PessoaFisica> Coordenadores { get; set; }
    }
}