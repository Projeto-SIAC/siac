using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SIAC.Migrations
{
    public class SaoPaulo
    {
        public static void Semear(Contexto context)
        {
            #region SaoPaulo
            context.Estado.AddOrUpdate(
                e => new { e.CodPais, e.CodEstado },
                new Estado { CodPais = 1, CodEstado = 26, Descricao = "São Paulo", Sigla = "SP" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 1, Descricao = "Monte Alto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 2, Descricao = "Santo Antonio Do Aracangua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 3, Descricao = "Americana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 4, Descricao = "Piratininga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 5, Descricao = "Mira Estrela" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 6, Descricao = "Pedro De Toledo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 7, Descricao = "Canas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 8, Descricao = "Jumirim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 9, Descricao = "Nova Castilho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 10, Descricao = "Santa Salete" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 11, Descricao = "Trabiju" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 12, Descricao = "Riversul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 13, Descricao = "Ipaussu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 14, Descricao = "Ribeirao Preto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 15, Descricao = "Adolfo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 16, Descricao = "Taciba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 17, Descricao = "Dois Corregos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 18, Descricao = "Engenheiro Coelho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 19, Descricao = "Holambra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 20, Descricao = "Tuiuti" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 21, Descricao = "Vargem" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 22, Descricao = "Bertioga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 23, Descricao = "Ilha Comprida" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 24, Descricao = "Elisiario" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 25, Descricao = "Novais" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 26, Descricao = "Aspasia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 27, Descricao = "Alambari" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 28, Descricao = "Aluminio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 29, Descricao = "Potim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 30, Descricao = "Carapicuiba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 31, Descricao = "Torre De Pedra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 32, Descricao = "Fernandopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 33, Descricao = "Agudos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 34, Descricao = "Pacaembu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 35, Descricao = "Ibira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 36, Descricao = "Estrela D'Oeste" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 37, Descricao = "Barra Bonita" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 38, Descricao = "Brauna" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 39, Descricao = "Atibaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 40, Descricao = "Suzano" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 41, Descricao = "Boraceia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 42, Descricao = "Bauru" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 43, Descricao = "Cubatao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 44, Descricao = "Aguas Da Prata" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 45, Descricao = "Aguas De Sao Pedro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 46, Descricao = "Alfredo Marcondes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 47, Descricao = "Altinopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 48, Descricao = "Alvaro De Carvalho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 49, Descricao = "Catanduva" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 50, Descricao = "Brotas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 51, Descricao = "Analandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 52, Descricao = "General Salgado" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 53, Descricao = "Aparecida D'Oeste" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 54, Descricao = "Aramina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 55, Descricao = "Arealva" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 56, Descricao = "Areiopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 57, Descricao = "Tanabi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 58, Descricao = "Auriflama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 59, Descricao = "Avanhandava" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 60, Descricao = "Bady Bassitt" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 61, Descricao = "Barao De Antonina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 62, Descricao = "Barrinha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 63, Descricao = "Americo De Campos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 64, Descricao = "Buritama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 65, Descricao = "Biritiba-Mirim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 66, Descricao = "Borborema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 67, Descricao = "Candido Rodrigues" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 68, Descricao = "Divinolandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 69, Descricao = "Cabralia Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 70, Descricao = "Angatuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 71, Descricao = "Caiabu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 72, Descricao = "Caiua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 73, Descricao = "Cajobi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 74, Descricao = "Cananeia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 75, Descricao = "Capela Do Alto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 76, Descricao = "Caraguatatuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 77, Descricao = "Cassia Dos Coqueiros" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 78, Descricao = "Serra Azul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 79, Descricao = "Cerquilho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 80, Descricao = "Charqueada" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 81, Descricao = "Conchas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 82, Descricao = "Cosmorama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 83, Descricao = "Cravinhos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 84, Descricao = "Descalvado" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 85, Descricao = "Dourado" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 86, Descricao = "Ibirarema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 87, Descricao = "Elias Fausto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 88, Descricao = "Fernando Prestes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 89, Descricao = "Florinia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 90, Descricao = "Jau" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 91, Descricao = "Flora Rica" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 92, Descricao = "Mairipora" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 93, Descricao = "Francisco Morato" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 94, Descricao = "Gabriel Monteiro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 95, Descricao = "Garca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 96, Descricao = "Glicerio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 97, Descricao = "Guaimbe" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 98, Descricao = "Guararema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 99, Descricao = "Guarei" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 100, Descricao = "Duartina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 101, Descricao = "Mendonca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 102, Descricao = "Iacanga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 103, Descricao = "Macaubal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 104, Descricao = "Ibiuna" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 105, Descricao = "Iepe" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 106, Descricao = "Igarapava" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 107, Descricao = "Ipeuna" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 108, Descricao = "Ipua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 109, Descricao = "Irapua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 110, Descricao = "Itaju" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 111, Descricao = "Echapora" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 112, Descricao = "Itapura" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 113, Descricao = "Itarare" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 114, Descricao = "Itatiba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 115, Descricao = "Itirapina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 116, Descricao = "Ituverava" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 117, Descricao = "Jaborandi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 118, Descricao = "Jales" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 119, Descricao = "Jaci" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 120, Descricao = "Jardinopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 121, Descricao = "Joanopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 122, Descricao = "Lagoinha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 123, Descricao = "Lavinia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 124, Descricao = "Mirassolandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 125, Descricao = "Lindoia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 126, Descricao = "Luis Antonio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 127, Descricao = "Lutecia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 128, Descricao = "Magda" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 129, Descricao = "Mariapolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 130, Descricao = "Marinopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 131, Descricao = "Miguelopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 132, Descricao = "Mirandopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 133, Descricao = "Mirassol" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 134, Descricao = "Moji Mirim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 135, Descricao = "Monte Aprazivel" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 136, Descricao = "Monte Castelo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 137, Descricao = "Monteiro Lobato" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 138, Descricao = "Narandiba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 139, Descricao = "Nazare Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 140, Descricao = "Neves Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 141, Descricao = "Nipoa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 142, Descricao = "Nova Europa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 143, Descricao = "Nova Guataporanga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 144, Descricao = "Ocaucu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 145, Descricao = "Oriente" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 146, Descricao = "Orlandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 147, Descricao = "Oscar Bressane" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 148, Descricao = "Ourinhos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 149, Descricao = "Palmares Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 150, Descricao = "Penapolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 151, Descricao = "Redencao Da Serra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 152, Descricao = "Sebastianopolis Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 153, Descricao = "Paranapua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 154, Descricao = "Pardinho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 155, Descricao = "Presidente Venceslau" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 156, Descricao = "Quata" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 157, Descricao = "Pedranopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 158, Descricao = "Palmital" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 159, Descricao = "Pereiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 160, Descricao = "Piacatu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 161, Descricao = "Pindorama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 162, Descricao = "Piquerobi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 163, Descricao = "Espirito Santo Do Pinhal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 164, Descricao = "Pirangi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 165, Descricao = "Paulinia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 166, Descricao = "Poa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 167, Descricao = "Pompeia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 168, Descricao = "Pontal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 169, Descricao = "Porto Feliz" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 170, Descricao = "Porto Ferreira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 171, Descricao = "Presidente Alves" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 172, Descricao = "Rafard" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 173, Descricao = "Sao Manuel" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 174, Descricao = "Restinga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 175, Descricao = "Ribeirao Bonito" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 176, Descricao = "Ribeirao Corrente" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 177, Descricao = "Cardoso" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 178, Descricao = "Rincao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 179, Descricao = "Rio Das Pedras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 180, Descricao = "Sabino" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 181, Descricao = "Sales" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 182, Descricao = "Salesopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 183, Descricao = "Aguas De Santa Barbara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 184, Descricao = "Santa Cruz Das Palmeiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 185, Descricao = "Santa Ernestina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 186, Descricao = "Andradina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 187, Descricao = "Santa Mercedes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 188, Descricao = "Paraguacu Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 189, Descricao = "Santa Rosa De Viterbo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 190, Descricao = "Santopolis Do Aguapei" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 191, Descricao = "Sao Bento Do Sapucai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 192, Descricao = "Sao Joao Das Duas Pontes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 193, Descricao = "Sao Joao Do Pau D'Alho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 194, Descricao = "Sao Jose Da Bela Vista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 195, Descricao = "Demais Municipios" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 196, Descricao = "Sao Simao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 197, Descricao = "Serra Negra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 198, Descricao = "Sertaozinho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 199, Descricao = "Severinia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 200, Descricao = "Taiacu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 201, Descricao = "Tapirai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 202, Descricao = "Ubarana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 203, Descricao = "Tarabai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 204, Descricao = "Taquaritinga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 205, Descricao = "Torrinha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 206, Descricao = "Tres Fronteiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 207, Descricao = "Tupi Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 208, Descricao = "Vargem Grande Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 209, Descricao = "Borebi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 210, Descricao = "Embauba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 211, Descricao = "Euclides Da Cunha Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 212, Descricao = "Iaras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 213, Descricao = "Rosana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 214, Descricao = "Turmalina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 215, Descricao = "Votorantim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 216, Descricao = "Uru" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 217, Descricao = "Valparaiso" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 218, Descricao = "Cedral" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 219, Descricao = "Guaraci" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 220, Descricao = "Presidente Prudente" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 221, Descricao = "Lourdes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 222, Descricao = "Sorocaba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 223, Descricao = "Araraquara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 224, Descricao = "Jacupiranga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 225, Descricao = "Olimpia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 226, Descricao = "Guzolandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 227, Descricao = "Pirapozinho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 228, Descricao = "Juquitiba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 229, Descricao = "Santos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 230, Descricao = "Alvares Florence" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 231, Descricao = "Sao Jose Do Rio Preto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 232, Descricao = "Lucelia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 233, Descricao = "Osasco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 234, Descricao = "Salto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 235, Descricao = "Barueri" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 236, Descricao = "Artur Nogueira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 237, Descricao = "Sao Pedro Do Turvo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 238, Descricao = "Santa Gertrudes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 239, Descricao = "Sao Bernardo Do Campo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 240, Descricao = "Indiana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 241, Descricao = "Vargem Grande Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 242, Descricao = "Pontalinda" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 243, Descricao = "Pontes Gestal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 244, Descricao = "Pracinha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 245, Descricao = "Praia Grande" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 246, Descricao = "Presidente Bernardes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 247, Descricao = "Taboao Da Serra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 248, Descricao = "Serrana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 249, Descricao = "Taquaral" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 250, Descricao = "Regente Feijo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 251, Descricao = "Ribeira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 252, Descricao = "Ribeirao Branco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 253, Descricao = "Ribeirao Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 254, Descricao = "Rinopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 255, Descricao = "Santa Cruz Da Conceicao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 256, Descricao = "Sao Roque" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 257, Descricao = "Sagres" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 258, Descricao = "Sales Oliveira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 259, Descricao = "Saltinho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 260, Descricao = "Sandovalina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 261, Descricao = "Rio Grande Da Serra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 262, Descricao = "Santa Cruz Da Esperanca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 263, Descricao = "Taiuva" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 264, Descricao = "Santana Da Ponte Pensa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 265, Descricao = "Valinhos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 266, Descricao = "Santo Antonio Da Alegria" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 267, Descricao = "Santo Expedito" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 268, Descricao = "Queiroz" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 269, Descricao = "Sao Joaquim Da Barra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 270, Descricao = "Urania" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 271, Descricao = "Sao Miguel Arcanjo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 272, Descricao = "Sao Pedro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 273, Descricao = "Salto De Pirapora" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 274, Descricao = "Promissao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 275, Descricao = "Sarutaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 276, Descricao = "Sete Barras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 277, Descricao = "Silveiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 278, Descricao = "Santa Isabel" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 279, Descricao = "Sumare" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 280, Descricao = "Suzanapolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 281, Descricao = "Tabapua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 282, Descricao = "Taguai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 283, Descricao = "Vera Cruz" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 284, Descricao = "Taquarituba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 285, Descricao = "Tejupa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 286, Descricao = "Terra Roxa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 287, Descricao = "Timburi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 288, Descricao = "Tremembe" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 289, Descricao = "Tupa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 290, Descricao = "Uchoa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 291, Descricao = "Turiuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 292, Descricao = "Urupes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 293, Descricao = "Viradouro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 294, Descricao = "Vista Alegre Do Alto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 295, Descricao = "Vitoria Brasil" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 296, Descricao = "Santa Rita D'Oeste" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 297, Descricao = "Pedrinhas Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 298, Descricao = "Zacarias" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 299, Descricao = "Anhumas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 300, Descricao = "Balbinos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 301, Descricao = "Gaviao Peixoto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 302, Descricao = "Nhandeara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 303, Descricao = "Bofete" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 304, Descricao = "Cesario Lange" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 305, Descricao = "Candido Mota" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 306, Descricao = "Adamantina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 307, Descricao = "Alto Alegre" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 308, Descricao = "Alvares Machado" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 309, Descricao = "Aparecida" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 310, Descricao = "Aracoiaba Da Serra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 311, Descricao = "Arandu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 312, Descricao = "Arapei" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 313, Descricao = "Araras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 314, Descricao = "Avai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 315, Descricao = "Avare" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 316, Descricao = "Barbosa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 317, Descricao = "Caieiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 318, Descricao = "Bebedouro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 319, Descricao = "Bernardino De Campos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 320, Descricao = "Birigui" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 321, Descricao = "Brejo Alegre" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 322, Descricao = "Brodowski" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 323, Descricao = "Buri" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 324, Descricao = "Buritizal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 325, Descricao = "Cabreuva" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 326, Descricao = "Cafelandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 327, Descricao = "Cajamar" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 328, Descricao = "Cajati" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 329, Descricao = "Cajuru" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 330, Descricao = "Campos Novos Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 331, Descricao = "Canitar" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 332, Descricao = "Capao Bonito" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 333, Descricao = "Capivari" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 334, Descricao = "Catigua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 335, Descricao = "Conchal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 336, Descricao = "Cordeiropolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 337, Descricao = "Cristais Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 338, Descricao = "Cruzeiro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 339, Descricao = "Cunha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 340, Descricao = "Dobrada" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 341, Descricao = "Dolcinopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 342, Descricao = "Dracena" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 343, Descricao = "Dumont" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 344, Descricao = "Emilianopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 345, Descricao = "Estiva Gerbi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 346, Descricao = "Estrela Do Norte" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 347, Descricao = "Fernao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 348, Descricao = "Florida Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 349, Descricao = "Laranjal Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 350, Descricao = "Martinopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 351, Descricao = "Gastao Vidigal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 352, Descricao = "Getulina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 353, Descricao = "Guaicara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 354, Descricao = "Guapiara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 355, Descricao = "Guaracai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 356, Descricao = "Guararapes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 357, Descricao = "Igarata" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 358, Descricao = "Guatapara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 359, Descricao = "Peruibe" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 360, Descricao = "Iacri" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 361, Descricao = "Ibitinga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 362, Descricao = "Igaracu Do Tiete" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 363, Descricao = "Inubia Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 364, Descricao = "Leme" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 365, Descricao = "Ipigua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 366, Descricao = "Iracemapolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 367, Descricao = "Eldorado" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 368, Descricao = "Franca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 369, Descricao = "Itajobi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 370, Descricao = "Itanhaem" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 371, Descricao = "Itaoca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 372, Descricao = "Itapolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 373, Descricao = "Itapui" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 374, Descricao = "Itai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 375, Descricao = "Itatinga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 376, Descricao = "Itirapua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 377, Descricao = "Irapuru" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 378, Descricao = "Jarinu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 379, Descricao = "Jeriquara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 380, Descricao = "Joao Ramalho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 381, Descricao = "Julio Mesquita" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 382, Descricao = "Junqueiropolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 383, Descricao = "Lins" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 384, Descricao = "Louveira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 385, Descricao = "Lucianopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 386, Descricao = "Luiziania" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 387, Descricao = "Macatuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 388, Descricao = "Macedonia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 389, Descricao = "Mairinque" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 390, Descricao = "Itaquaquecetuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 391, Descricao = "Marapoama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 392, Descricao = "Miracatu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 393, Descricao = "Mirante Do Paranapanema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 394, Descricao = "Mococa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 395, Descricao = "Monte Azul Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 396, Descricao = "Morro Agudo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 397, Descricao = "Nantes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 398, Descricao = "Nova Campina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 399, Descricao = "Nova Canaa Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 400, Descricao = "Nova Independencia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 401, Descricao = "Oleo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 402, Descricao = "Osvaldo Cruz" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 403, Descricao = "Ouroeste" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 404, Descricao = "Panorama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 405, Descricao = "Paraibuna" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 406, Descricao = "Paranapanema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 407, Descricao = "Nova Alianca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 408, Descricao = "Parisi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 409, Descricao = "Pauliceia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 410, Descricao = "Paulo De Faria" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 411, Descricao = "Pedregulho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 412, Descricao = "Monte Alegre Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 413, Descricao = "Piedade" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 414, Descricao = "Pinhalzinho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 415, Descricao = "Pirajui" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 416, Descricao = "Pirapora Do Bom Jesus" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 417, Descricao = "Platina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 418, Descricao = "Poloni" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 419, Descricao = "Pongai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 420, Descricao = "Natividade Da Serra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 421, Descricao = "Parapua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 422, Descricao = "Franco Da Rocha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 423, Descricao = "Pratania" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 424, Descricao = "Piraju" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 425, Descricao = "Bastos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 426, Descricao = "Guapiacu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 427, Descricao = "Icem" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 428, Descricao = "Ouro Verde" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 429, Descricao = "Ribeirao Pires" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 430, Descricao = "Santa Rita Do Passa Quatro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 431, Descricao = "Corumbatai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 432, Descricao = "Itariri" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 433, Descricao = "Bilac" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 434, Descricao = "Herculandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 435, Descricao = "Lavrinhas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 436, Descricao = "Nuporanga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 437, Descricao = "Paraiso" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 438, Descricao = "Guaira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 439, Descricao = "Indaiatuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 440, Descricao = "Itapetininga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 441, Descricao = "Bananal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 442, Descricao = "Itabera" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 443, Descricao = "Campo Limpo Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 444, Descricao = "Braganca Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 445, Descricao = "Americo Brasiliense" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 446, Descricao = "Aracariguama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 447, Descricao = "Arco-Iris" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 448, Descricao = "Ariranha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 449, Descricao = "Bom Sucesso De Itarare" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 450, Descricao = "Cachoeira Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 451, Descricao = "Cosmopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 452, Descricao = "Dirce Reis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 453, Descricao = "Fartura" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 454, Descricao = "Guaranta" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 455, Descricao = "Ibate" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 456, Descricao = "Itapirapua Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 457, Descricao = "Itupeva" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 458, Descricao = "Jundiai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 459, Descricao = "Sao Jose Do Rio Pardo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 460, Descricao = "Maracai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 461, Descricao = "Mineiros Do Tiete" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 462, Descricao = "Moncoes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 463, Descricao = "Mongagua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 464, Descricao = "Morungaba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 465, Descricao = "Murutinga Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 466, Descricao = "Orindiuva" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 467, Descricao = "Palestina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 468, Descricao = "Pariquera-Acu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 469, Descricao = "Paulistania" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 470, Descricao = "Santa Lucia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 471, Descricao = "Pilar Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 472, Descricao = "Socorro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 473, Descricao = "Porangaba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 474, Descricao = "Presidente Epitacio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 475, Descricao = "Quadra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 476, Descricao = "Lupercio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 477, Descricao = "Rifaina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 478, Descricao = "Planalto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 479, Descricao = "Rubiacea" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 480, Descricao = "Salmourao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 481, Descricao = "Santa Adelia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 482, Descricao = "Santa Branca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 483, Descricao = "Santa Maria Da Serra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 484, Descricao = "Ubirajara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 485, Descricao = "Santo Antonio Do Jardim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 486, Descricao = "Nova Granada" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 487, Descricao = "Varzea Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 488, Descricao = "Tiete" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 489, Descricao = "Piquete" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 490, Descricao = "Tambau" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 491, Descricao = "Lorena" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 492, Descricao = "Ribeirao Dos Indios" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 493, Descricao = "Sao Joao De Iracema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 494, Descricao = "Mesopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 495, Descricao = "Barra Do Chapeu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 496, Descricao = "Piracaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 497, Descricao = "Nova Luzitania" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 498, Descricao = "Bora" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 499, Descricao = "Coronel Macedo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 500, Descricao = "Pedra Bela" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 501, Descricao = "Sao Francisco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 502, Descricao = "Sud Mennucci" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 503, Descricao = "Aruja" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 504, Descricao = "Colombia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 505, Descricao = "Assis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 506, Descricao = "Itobi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 507, Descricao = "Palmeira D'Oeste" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 508, Descricao = "Itapeva" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 509, Descricao = "Santa Fe Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 510, Descricao = "Ribeirao Grande" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 511, Descricao = "Areias" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 512, Descricao = "Boa Esperanca Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 513, Descricao = "Cacapava" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 514, Descricao = "Cerqueira Cesar" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 515, Descricao = "Coroados" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 516, Descricao = "Guara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 517, Descricao = "Iporanga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 518, Descricao = "Jose Bonifacio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 519, Descricao = "Novo Horizonte" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 520, Descricao = "Potirendaba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 521, Descricao = "Sarapui" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 522, Descricao = "Itapecerica Da Serra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 523, Descricao = "Guarani D'Oeste" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 524, Descricao = "Rubineia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 525, Descricao = "Meridiano" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 526, Descricao = "Teodoro Sampaio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 527, Descricao = "Caconde" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 528, Descricao = "Sao Lourenco Da Serra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 529, Descricao = "Monte Mor" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 530, Descricao = "Santo Anastacio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 531, Descricao = "Santana De Parnaiba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 532, Descricao = "Hortolandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 533, Descricao = "Nova Odessa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 534, Descricao = "Ilhabela" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 535, Descricao = "Taruma" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 536, Descricao = "Rancharia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 537, Descricao = "Tabatinga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 538, Descricao = "Balsamo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 539, Descricao = "Sao Caetano Do Sul" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 540, Descricao = "Guaratingueta" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 541, Descricao = "Ubatuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 542, Descricao = "Pereira Barreto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 543, Descricao = "Itapevi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 544, Descricao = "Sao Jose Dos Campos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 545, Descricao = "Floreal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 546, Descricao = "Amparo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 547, Descricao = "Manduri" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 548, Descricao = "Vinhedo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 549, Descricao = "Mogi Das Cruzes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 550, Descricao = "Campos Do Jordao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 551, Descricao = "Jandira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 552, Descricao = "Onda Verde" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 553, Descricao = "Altair" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 554, Descricao = "Mombuca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 555, Descricao = "Colina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 556, Descricao = "Pirassununga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 557, Descricao = "Batatais" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 558, Descricao = "Embu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 559, Descricao = "Jacarei" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 560, Descricao = "Matao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 561, Descricao = "Reginopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 562, Descricao = "Santa Barbara D'Oeste" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 563, Descricao = "Embu-Guacu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 564, Descricao = "Santa Clara D'Oeste" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 565, Descricao = "Votuporanga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 566, Descricao = "Barra Do Turvo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 567, Descricao = "Guariba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 568, Descricao = "Patrocinio Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 569, Descricao = "Jaboticabal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 570, Descricao = "Barretos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 571, Descricao = "Rio Claro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 572, Descricao = "Boituva" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 573, Descricao = "Sao Jose Do Barreiro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 574, Descricao = "Sao Sebastiao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 575, Descricao = "Anhembi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 576, Descricao = "Taquarivai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 577, Descricao = "Cotia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 578, Descricao = "Bom Jesus Dos Perdoes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 579, Descricao = "Pederneiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 580, Descricao = "Aguai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 581, Descricao = "Registro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 582, Descricao = "Diadema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 583, Descricao = "Guarulhos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 584, Descricao = "Castilho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 585, Descricao = "Tapiratiba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 586, Descricao = "Itu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 587, Descricao = "Pedreira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 588, Descricao = "Juquia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 589, Descricao = "Aracatuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 590, Descricao = "Taubate" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 591, Descricao = "Sao Vicente" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 592, Descricao = "Pradopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 593, Descricao = "Casa Branca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 594, Descricao = "Ipero" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 595, Descricao = "Sao Sebastiao Da Grama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 596, Descricao = "Pindamonhangaba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 597, Descricao = "Roseira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 598, Descricao = "Itaporanga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 599, Descricao = "Chavantes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 600, Descricao = "Piracicaba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 601, Descricao = "Santa Albertina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 602, Descricao = "Sao Carlos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 603, Descricao = "Iguape" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 604, Descricao = "Maua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 605, Descricao = "Santo Antonio De Posse" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 606, Descricao = "Bento De Abreu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 607, Descricao = "Santo Antonio Do Pinhal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 608, Descricao = "Queluz" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 609, Descricao = "Alvinlandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 610, Descricao = "Campina Do Monte Alegre" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 611, Descricao = "Quintana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 612, Descricao = "Lencois Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 613, Descricao = "Mogi Guacu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 614, Descricao = "Ilha Solteira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 615, Descricao = "Guaruja" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 616, Descricao = "Maraba Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 617, Descricao = "Santa Cruz Do Rio Pardo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 618, Descricao = "Indiapora" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 619, Descricao = "Valentim Gentil" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 620, Descricao = "Salto Grande" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 621, Descricao = "Motuca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 622, Descricao = "Ferraz De Vasconcelos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 623, Descricao = "Jaguariuna" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 624, Descricao = "Uniao Paulista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 625, Descricao = "Espirito Santo Do Turvo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 626, Descricao = "Apiai" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 627, Descricao = "Marilia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 628, Descricao = "Riolandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 629, Descricao = "Aguas De Lindoia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 630, Descricao = "Bariri" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 631, Descricao = "Populina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 632, Descricao = "Botucatu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 633, Descricao = "Limeira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 634, Descricao = "Jambeiro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 635, Descricao = "Sao Joao Da Boa Vista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 636, Descricao = "Cruzalia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 637, Descricao = "Sao Luis Do Paraitinga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 638, Descricao = "Galia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 639, Descricao = "Bocaina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 640, Descricao = "Campinas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 641, Descricao = "Pitangueiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 642, Descricao = "Clementina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 643, Descricao = "Sao Paulo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 644, Descricao = "Itapira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 645, Descricao = "Tatui" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 26, CodMunicipio = 646, Descricao = "Santo Andre" }
            );

            #endregion
        }
    }
}