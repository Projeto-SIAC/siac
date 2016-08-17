using System.ComponentModel.DataAnnotations.Schema;

namespace SIAC.Models
{
    public partial class SimCandidatoProva
    {
        [NotMapped]
        public string EscorePadronizadoString => this.EscorePadronizado?.ToString("0.000");
    }
}