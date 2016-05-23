using System.Collections.Generic;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class InstitucionalRealizarViewModel
    {
        public AvalAvi Avi { get; set; }
        public List<AviQuestaoPessoaResposta> Respostas { get; set; }
    }

    public class InstitucionalPublicoViewModel
    {
        public AvalAvi Avi { get; set; }
        public List<AviTipoPublico> TiposPublico { get; set; }
    }

    public class InstitucionalGerarQuestaoViewModel
    {
        public List<AviModulo> Modulos { get; set; }
        public List<AviCategoria> Categorias { get; set; }
        public List<AviIndicador> Indicadores { get; set; }
        public List<TipoQuestao> Tipos { get; set; }
        public AvalAvi Avi { get; set; }
    }
}