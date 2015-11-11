using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class AutoavaliacaoDetalheViewModel
    {
        public Models.Avaliacao Avaliacao { get; set; }
        public Dictionary<string, double> Desempenho { get; set; } = new Dictionary<string, double>();
        public double Porcentagem { get; set; }
    }
}