using SIAC.Models;

namespace SIAC.ViewModels
{
    public class ImpressaoAvaliacaoViewModel
    {
        public Avaliacao Avaliacao { get; set; }
        public string Titulo { get; set; }
        public string Instituicao { get; set; }
        public string Professor { get; set; }
        public string[] Campos { get; set; } = new string[0];
        public string[] Instrucoes { get; set; } = new string[0];
        public bool Arquivar { get; set; }
    }
}