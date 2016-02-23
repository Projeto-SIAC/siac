using System.Collections.Generic;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class AvaliacaoIndexViewModel
    {
        public List<Dificuldade> Dificuldades { get; set; } = new List<Dificuldade>();
        public List<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
    }
}