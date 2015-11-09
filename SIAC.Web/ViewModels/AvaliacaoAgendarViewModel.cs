using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class AvaliacaoAgendarViewModel
    {
        public Models.Avaliacao Avaliacao { get; set; }
        public List<Models.Sala> Salas { get; set; }
        public List<Models.Turma> Turmas { get; set; }
    }
}