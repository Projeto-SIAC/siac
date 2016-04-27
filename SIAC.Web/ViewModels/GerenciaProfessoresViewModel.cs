using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIAC.Models;
using Newtonsoft.Json;

namespace SIAC.ViewModels
{
    public class GerenciaProfessoresViewModel
    {
        public List<Professor> Professores { get; set; } = new List<Professor>();
    }
}