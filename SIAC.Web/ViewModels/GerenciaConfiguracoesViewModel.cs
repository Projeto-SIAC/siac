using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.ViewModels
{
    public class GerenciaConfiguracoesViewModel
    {
        public string SmtpEnderecoHost { get; set; }
        public int SmtpPorta { get; set; }
        public bool SmptFlagSSL { get; set; }
        public string SmtpUsuario { get; set; }
        public string SmtpSenha { get; set; }
    }
}