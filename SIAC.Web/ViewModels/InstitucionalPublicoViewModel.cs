using SIAC.Models;
using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class InstitucionalPublicoViewModel
    {
        public AvalAvi Avi { get; set; }
        public List<AviTipoPublico> TiposPublico { get; set; }
    }
}