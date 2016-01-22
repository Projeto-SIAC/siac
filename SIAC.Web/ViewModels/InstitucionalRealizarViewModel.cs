using System.Collections.Generic;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class InstitucionalRealizarViewModel
    {
        public AvalAvi Avi { get; set; }
        public List<AviQuestaoPessoaResposta> Respostas { get; set; }
    }
}