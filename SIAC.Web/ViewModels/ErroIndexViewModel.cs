namespace SIAC.ViewModels
{
    public class ErroIndexViewModel
    {
        public string Codigo { get; set; } = "desconhecido";
        public string Titulo { get; set; } = "Volte ao início";
        public string Mensagem { get; set; } = "Ocorreu um erro inesperado";

        public ErroIndexViewModel() { }

        public ErroIndexViewModel(string codigo, string titulo, string mensagem)
        {
            this.Codigo = codigo;
            this.Titulo = titulo;
            this.Mensagem = mensagem;
        }
    }
}