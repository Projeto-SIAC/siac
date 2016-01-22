using System.Collections.Generic;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class AutoavaliacaoDetalheViewModel
    {
        public Avaliacao Avaliacao { get; set; }
        public Dictionary<string, double> Desempenho { get; set; } = new Dictionary<string, double>();
        public double Porcentagem { get; set; }
    }
}