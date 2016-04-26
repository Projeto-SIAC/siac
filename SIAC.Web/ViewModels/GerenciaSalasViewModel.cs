using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Models;

namespace SIAC.ViewModels
{
    public class GerenciaSalasViewModel
    {
        public List<Campus> Campi { get; set; } = new List<Campus>();
        public List<Bloco> Blocos { get; set; } = new List<Bloco>();
        public List<Sala> Salas { get; set; } = new List<Sala>();
    }
}