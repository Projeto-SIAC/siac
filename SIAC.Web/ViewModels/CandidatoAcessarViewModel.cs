using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class CandidatoAcessarViewModel
    {
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public string Mensagem { get; set; }
        public bool TemMensagem => !String.IsNullOrEmpty(Mensagem);
    }
}