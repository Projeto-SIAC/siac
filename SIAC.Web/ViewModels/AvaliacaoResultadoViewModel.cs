using System.Collections.Generic;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class AvaliacaoResultadoViewModel
    {
        public Avaliacao Avaliacao { get; set; }
        public double Porcentagem { get; set; }
        public Dictionary<string, double> Desempenho { get; set; } = new Dictionary<string, double>();
    }
}