using System.Collections.Generic;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class AvaliacaoGerarViewModel
    {
        public List<Disciplina> Disciplinas { get; set; }
        public List<Dificuldade> Dificuldades { get; set; }
        public string Termo { get; set; }
    }
}