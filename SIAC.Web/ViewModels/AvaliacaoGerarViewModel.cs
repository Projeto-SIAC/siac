using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class AvaliacaoGerarViewModel
    {
        public List<Models.Disciplina> Disciplinas { get; set; }
        public List<Models.Dificuldade> Dificuldades { get; set; }
        public string Termo { get; set; }
    }
}