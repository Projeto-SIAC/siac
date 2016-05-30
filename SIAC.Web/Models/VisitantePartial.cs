using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SIAC.Models
{
    public partial class Visitante
    {
        private static dbSIACEntities contexto => Repositorio.GetInstance();

        [NotMapped]
        public bool FlagAtivo => this.DtValidade.HasValue ? (this.DtValidade.Value > DateTime.Now) : true;

        public static int ProxCodigo
        {
            get
            {
                int cod;
                if (!Sistema.ProxCodVisitante.HasValue)
                {
                    var v = contexto.Visitante.ToList();
                    if (v.Count > 0)
                    {
                        Sistema.ProxCodVisitante = v.Max(vis => vis.CodVisitante) + 1;
                    }
                    else
                    {
                        Sistema.ProxCodVisitante = 0;
                    }
                }
                else
                {
                    Sistema.ProxCodVisitante++;
                }
                cod = Sistema.ProxCodVisitante.Value;
                return cod;
            }
        }

        public static void Inserir(Visitante visitante)
        {
            contexto.Visitante.Add(visitante);
            contexto.SaveChanges();
        }

        public static List<Visitante> Listar() => contexto.Visitante.ToList();

        public static Visitante ListarPorMatricula(string matricula) =>
            contexto.Visitante.FirstOrDefault(v => v.MatrVisitante.ToLower() == matricula.ToLower());
    }
}