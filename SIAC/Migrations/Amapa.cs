using SIAC.Models;
using System.Data.Entity.Migrations;

namespace SIAC.Migrations
{
    public class Amapa
    {
        public static void Semear(Contexto context)
        {
            #region Amapa

            context.Estado.AddOrUpdate(
                e => new { e.CodPais, e.CodEstado },
                new Estado { CodPais = 1, CodEstado = 4, Descricao = "Amapá", Sigla = "AP" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 1, Descricao = "Laranjal Do Jari" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 2, Descricao = "Calcoene" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 3, Descricao = "Mazagao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 4, Descricao = "Cutias" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 5, Descricao = "Pracuuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 6, Descricao = "Vitoria Do Jari" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 7, Descricao = "Pedra Branca Do Amapari" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 8, Descricao = "Ferreira Gomes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 9, Descricao = "Serra Do Navio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 10, Descricao = "Tartarugalzinho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 11, Descricao = "Porto Grande" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 12, Descricao = "Itaubal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 13, Descricao = "Oiapoque" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 14, Descricao = "Santana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 15, Descricao = "Amapa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 4, CodMunicipio = 16, Descricao = "Macapa" }
            );
            #endregion
        }
    }
}