using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class AvaliacaoResultadoViewModel
    {
        public Models.Avaliacao Avaliacao { get; set; }
        public double Porcentagem { get; set; }
        public Dictionary<string, double> Desempenho { get; set; } = new Dictionary<string, double>();
    }
}