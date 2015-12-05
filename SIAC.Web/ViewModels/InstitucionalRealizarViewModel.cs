using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class InstitucionalRealizarViewModel
    {
        public Models.AvalAvi Avi { get; set; }
        public List<Models.AviQuestaoPessoaResposta> Respostas { get; set; }
    }
}