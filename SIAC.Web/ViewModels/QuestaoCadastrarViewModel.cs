using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class QuestaoCadastrarViewModel
    {
        public string Captcha { get; set; }
        public string Termo { get; set; }
        public List<Models.Disciplina> Disciplinas { get; set; }
        public List<Models.TipoQuestao> Tipos { get; set; }
        public List<Models.TipoAnexo> TiposAnexo { get; set; }
        public List<Models.Dificuldade> Dificuldades { get; set; }
    }
}