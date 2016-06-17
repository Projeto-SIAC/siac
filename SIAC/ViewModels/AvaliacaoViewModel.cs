using SIAC.Models;
using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class AvaliacaoAgendarViewModel
    {
        public Avaliacao Avaliacao { get; set; }
        public List<Sala> Salas { get; set; }
        public List<Turma> Turmas { get; set; }
    }

    public class AvaliacaoResultadoViewModel
    {
        public Avaliacao Avaliacao { get; set; }
        public double Porcentagem { get; set; }
        public Dictionary<string, double> Desempenho { get; set; } = new Dictionary<string, double>();
    }

    public class AvaliacaoGerarViewModel
    {
        public List<Disciplina> Disciplinas { get; set; }
        public List<Dificuldade> Dificuldades { get; set; }
        public string Termo { get; set; }
    }

    public class AvaliacaoIndexViewModel
    {
        public List<Dificuldade> Dificuldades { get; set; } = new List<Dificuldade>();
        public List<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
    }
}