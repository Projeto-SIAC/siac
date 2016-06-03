using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class AtalhoViewModel
    {
        public string Descricao { get; set; }
        public string Icone { get; set; }
        public string Url { get; set; }
        public string Mensagem { get; set; }
        public string Lembrete { get; set; }

        public AtalhoViewModel(string descricao, string icone, string url, string mensagem = null, string lembrete = null)
        {
            this.Descricao = descricao;
            this.Icone = icone;
            this.Url = url;
            this.Mensagem = String.IsNullOrEmpty(mensagem) ? $"Abrir {descricao}" : mensagem;
            this.Lembrete = lembrete;
        }
    }
}