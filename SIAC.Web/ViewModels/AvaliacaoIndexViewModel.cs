using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class AvaliacaoIndexViewModel
    {
        public List<Models.Dificuldade> Dificuldades { get; set; }
        public List<Models.Disciplina> Disciplinas { get; set; }
    }
}