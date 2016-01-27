using SIAC.Models;
using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class AutoavaliacaoRealizarViewModel
    {
        public AvalAuto Autoavaliacao { get; set; }
        public Dictionary<string, List<Questao>> Questoes { get; set; } = new Dictionary<string, List<Questao>>();
    }
}