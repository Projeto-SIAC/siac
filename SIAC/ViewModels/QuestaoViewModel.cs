using SIAC.Models;
using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class QuestaoIndexViewModel
    {
        public List<Disciplina> Disciplinas { get; set; }
        public List<Dificuldade> Dificuldades { get; set; }
    }

    public class QuestaoCadastrarViewModel
    {
        public string Captcha { get; set; }
        public string Termo { get; set; }
        public List<Disciplina> Disciplinas { get; set; }
        public List<TipoQuestao> Tipos { get; set; }
        public List<TipoAnexo> TiposAnexo { get; set; }
        public List<Dificuldade> Dificuldades { get; set; }
    }
}