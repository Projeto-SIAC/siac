using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Models;
using Newtonsoft.Json;

namespace SIAC.ViewModels
{
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