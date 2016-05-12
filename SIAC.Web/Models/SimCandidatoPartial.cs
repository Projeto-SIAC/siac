using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class SimCandidato
    {
        public string NumInscricaoRepresentacao => this.NumInscricao.ToString("d4");
    }
}