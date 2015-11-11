using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class CertificacaoConfigurarViewModel
    {
        public Models.Avaliacao Avaliacao { get; set; }
        public List<Models.Dificuldade> Dificuldades { get; set; }
        public List<Models.TipoQuestao> TiposQuestao { get; set; }
    }
}