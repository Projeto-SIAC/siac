using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class InstitucionalPublicoViewModel
    {
        public Models.AvalAvi Avi { get; set; }
        public List<Models.AviTipoPublico> TiposPublico { get; set; }
    }
}