using System.Collections.Generic;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class CertificacaoConfigurarViewModel
    {
        public Avaliacao Avaliacao { get; set; }
        public List<Dificuldade> Dificuldades { get; set; }
        public List<TipoQuestao> TiposQuestao { get; set; }
    }
}