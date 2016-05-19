using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class SimCandidatoProva
    {
        public string EscorePadronizadoString => this.EscorePadronizado?.ToString(".000");
    }
}