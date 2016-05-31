using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class SimCandidatoProva
    {
        [NotMapped]
        public string EscorePadronizadoString => this.EscorePadronizado?.ToString(".000");
    }
}