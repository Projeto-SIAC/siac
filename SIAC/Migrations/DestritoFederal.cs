using SIAC.Models;
using System.Data.Entity.Migrations;

namespace SIAC.Migrations
{
    public class DestritoFederal
    {
        public static void Semear(Contexto context)
        {
            #region DestritoFederal

            context.Estado.AddOrUpdate(
                e => new { e.CodPais, e.CodEstado },
                new Estado { CodPais = 1, CodEstado = 7, Descricao = "Distrito Federal", Sigla = "DF" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 7, CodMunicipio = 1, Descricao = "Brasilia" }
            );

            #endregion DestritoFederal
        }
    }
}