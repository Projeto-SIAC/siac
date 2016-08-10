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
        public string RecaptchaSiteKey => (string)Helpers.Configuracoes.Recuperar("SIAC_RECAPTCHA_SITE_KEY");
        public string Termo { get; set; }
        public List<Disciplina> Disciplinas { get; set; }
        public List<TipoQuestao> Tipos { get; set; }
        public List<TipoAnexo> TiposAnexo { get; set; }
        public List<Dificuldade> Dificuldades { get; set; }
    }
}