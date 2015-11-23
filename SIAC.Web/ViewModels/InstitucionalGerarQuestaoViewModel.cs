using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class InstitucionalGerarQuestaoViewModel
    {
        public List<Models.AviModulo> Modulos { get; set; }
        public List<Models.AviCategoria> Categorias { get; set; }
        public List<Models.AviIndicador> Indicadores { get; set; }
        public List<Models.TipoQuestao> Tipos { get; set; }
        public Models.AvalAvi Avi { get; set; }
    }
}