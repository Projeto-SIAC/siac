using Newtonsoft.Json;
using SIAC.Models;
using System.Collections.Generic;
using System.Linq;

namespace SIAC.ViewModels
{
    public class GerenciaBlocosViewModel
    {
        public List<Campus> Campi { get; set; } = new List<Campus>();
        public List<Bloco> Blocos { get; set; } = new List<Bloco>();
    }

    public class GerenciaProfessoresViewModel
    {
        public List<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
        public List<Professor> Professores { get; set; } = new List<Professor>();
    }

    public class GerenciaEditarSalaViewModel
    {
        public List<Campus> Campi { get; set; } = new List<Campus>();
        public List<Bloco> Blocos { get; set; } = new List<Bloco>();
        public Sala Sala { get; set; }
    }

    public class GerenciaEditarProfessorViewModel
    {
        public List<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
        public Professor Professor { get; set; }
    }

    public class GerenciaEditarBlocoViewModel
    {
        public Bloco Bloco { get; set; }
        public List<Campus> Campi { get; set; } = new List<Campus>();
    }

    public class GerenciaDisciplinasViewModel
    {
        public List<Disciplina> Disciplinas { get; set; } = new List<Disciplina>();
    }

    public class GerenciaConfiguracoesViewModel
    {
        public string SmtpEnderecoHost { get; set; }
        public int SmtpPorta { get; set; }
        public bool SmptFlagSSL { get; set; }
        public string SmtpUsuario { get; set; }
        public string SmtpSenha { get; set; }
    }

    public class GerenciaSalasViewModel
    {
        public List<Campus> Campi { get; set; } = new List<Campus>();
        public List<Bloco> Blocos { get; set; } = new List<Bloco>();
        public List<Sala> Salas { get; set; } = new List<Sala>();

        public string BlocosEmJson => JsonConvert.SerializeObject(Blocos.Select(b => new
        {
            Campus = b.Campus.CodComposto,
            CodBloco = b.CodBloco,
            Descricao = b.Descricao,
            Sigla = b.Sigla,
            RefLocal = b.RefLocal,
            Observacao = b.Observacao
        }));
    }
}