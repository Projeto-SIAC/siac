using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SIAC.Migrations
{
    public class Para
    {
        public static void Semear(Contexto context)
        {
            #region Para
            context.Estado.AddOrUpdate(
                e => new { e.CodPais, e.CodEstado },
                new Estado { CodPais = 1, CodEstado = 14, Descricao = "Pará", Sigla = "PA" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 1, Descricao = "Jacareacanga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 2, Descricao = "Agua Azul Do Norte" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 3, Descricao = "Santa Barbara Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 4, Descricao = "Abel Figueiredo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 5, Descricao = "Cumaru Do Norte" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 6, Descricao = "Aurora Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 7, Descricao = "Nova Esperanca Do Piria" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 8, Descricao = "Tailandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 9, Descricao = "Bujaru" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 10, Descricao = "Acara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 11, Descricao = "Alenquer" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 12, Descricao = "Juruti" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 13, Descricao = "Anajas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 14, Descricao = "Cameta" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 15, Descricao = "Bagre" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 16, Descricao = "Barcarena" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 17, Descricao = "Benevides" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 18, Descricao = "Abaetetuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 19, Descricao = "Capitao Poco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 20, Descricao = "Chaves" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 21, Descricao = "Curuca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 22, Descricao = "Gurupa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 23, Descricao = "Irituia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 24, Descricao = "Magalhaes Barata" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 25, Descricao = "Itupiranga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 26, Descricao = "Augusto Correa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 27, Descricao = "Sao Domingos Do Araguaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 28, Descricao = "Maracana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 29, Descricao = "Marapanim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 30, Descricao = "Mocajuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 31, Descricao = "Monte Alegre" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 32, Descricao = "Nova Timboteua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 33, Descricao = "Oeiras Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 34, Descricao = "Ourem" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 35, Descricao = "Peixe-Boi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 36, Descricao = "Prainha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 37, Descricao = "Salinopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 38, Descricao = "Santarem Novo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 39, Descricao = "Sao Caetano De Odivelas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 40, Descricao = "Sao Joao Do Araguaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 41, Descricao = "Sao Sebastiao Da Boa Vista" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 42, Descricao = "Viseu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 43, Descricao = "Rio Maria" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 44, Descricao = "Bom Jesus Do Tocantins" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 45, Descricao = "Concordia Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 46, Descricao = "Medicilandia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 47, Descricao = "Parauapebas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 48, Descricao = "Ruropolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 49, Descricao = "Goianesia Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 50, Descricao = "Sao Geraldo Do Araguaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 51, Descricao = "Ulianopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 52, Descricao = "Santa Maria Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 53, Descricao = "Novo Repartimento" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 54, Descricao = "Vitoria Do Xingu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 55, Descricao = "Canaa Dos Carajas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 56, Descricao = "Belterra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 57, Descricao = "Floresta Do Araguaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 58, Descricao = "Nova Ipixuna" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 59, Descricao = "Placas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 60, Descricao = "Sao Joao Da Ponta" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 61, Descricao = "Tracuateua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 62, Descricao = "Ananindeua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 63, Descricao = "Tucuma" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 64, Descricao = "Anapu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 65, Descricao = "Curralinho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 66, Descricao = "Pau D'Arco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 67, Descricao = "Aveiro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 68, Descricao = "Capanema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 69, Descricao = "Castanhal" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 70, Descricao = "Baiao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 71, Descricao = "Bannach" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 72, Descricao = "Bonito" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 73, Descricao = "Brasil Novo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 74, Descricao = "Breves" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 75, Descricao = "Cachoeira Do Arari" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 76, Descricao = "Cachoeira Do Piria" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 77, Descricao = "Colares" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 78, Descricao = "Curua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 79, Descricao = "Faro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 80, Descricao = "Garrafao Do Norte" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 81, Descricao = "Igarape-Acu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 82, Descricao = "Inhangapi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 83, Descricao = "Ipixuna Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 84, Descricao = "Jacunda" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 85, Descricao = "Limoeiro Do Ajuru" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 86, Descricao = "Moju" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 87, Descricao = "Muana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 88, Descricao = "Novo Progresso" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 89, Descricao = "Pacaja" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 90, Descricao = "Picarra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 91, Descricao = "Ponta De Pedras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 92, Descricao = "Porto De Moz" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 93, Descricao = "Primavera" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 94, Descricao = "Rondon Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 95, Descricao = "Santa Isabel Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 96, Descricao = "Santa Luzia Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 97, Descricao = "Santa Maria Das Barreiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 98, Descricao = "Santo Antonio Do Taua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 99, Descricao = "Sao Francisco Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 100, Descricao = "Sao Joao De Pirabas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 101, Descricao = "Sapucaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 102, Descricao = "Senador Jose Porfirio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 103, Descricao = "Tucurui" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 104, Descricao = "Terra Santa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 105, Descricao = "Tome-Acu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 106, Descricao = "Uruara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 107, Descricao = "Vigia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 108, Descricao = "Xinguara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 109, Descricao = "Terra Alta" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 110, Descricao = "Salvaterra" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 111, Descricao = "Sao Domingos Do Capim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 112, Descricao = "Altamira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 113, Descricao = "Afua" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 114, Descricao = "Igarape-Miri" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 115, Descricao = "Portel" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 116, Descricao = "Sao Miguel Do Guama" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 117, Descricao = "Quatipuru" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 118, Descricao = "Eldorado Dos Carajas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 119, Descricao = "Palestina Do Para" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 120, Descricao = "Trairao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 121, Descricao = "Dom Eliseu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 122, Descricao = "Mae Do Rio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 123, Descricao = "Redencao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 124, Descricao = "Santa Cruz Do Arari" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 125, Descricao = "Melgaco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 126, Descricao = "Santana Do Araguaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 127, Descricao = "Oriximina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 128, Descricao = "Soure" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 129, Descricao = "Maraba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 130, Descricao = "Marituba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 131, Descricao = "Braganca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 132, Descricao = "Brejo Grande Do Araguaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 133, Descricao = "Almeirim" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 134, Descricao = "Obidos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 135, Descricao = "Curionopolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 136, Descricao = "Ourilandia Do Norte" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 137, Descricao = "Sao Felix Do Xingu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 138, Descricao = "Breu Branco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 139, Descricao = "Santarem" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 140, Descricao = "Itaituba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 141, Descricao = "Conceicao Do Araguaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 142, Descricao = "Paragominas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 143, Descricao = "Belem" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 14, CodMunicipio = 144, Descricao = "Mojuí Dos Campos" }
            );

            #endregion
        }
    }
}