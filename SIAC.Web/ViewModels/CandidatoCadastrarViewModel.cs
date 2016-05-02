using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class CandidatoCadastrarViewModel
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string SenhaConfirmacao { get; set; }
        public string Mensagem { get; set; }
        public bool TemMensagem => !String.IsNullOrEmpty(Mensagem);
    }
}