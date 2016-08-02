using SIAC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SIAC.Migrations
{
    public class Alagoas
    {
        public static void Semear(Contexto context)
        {
            #region Alagoas

            context.Estado.AddOrUpdate(
                e => new { e.CodPais, e.CodEstado },
                new Estado { CodPais = 1, CodEstado = 2, Descricao = "Alagoas", Sigla = "AL" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 1, Descricao = "Teotonio Vilela" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 2, Descricao = "Capela" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 3, Descricao = "Jequia Da Praia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 4, Descricao = "Campestre" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 5, Descricao = "Piranhas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 6, Descricao = "Agua Branca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 7, Descricao = "Anadia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 8, Descricao = "Barra De Sao Miguel" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 9, Descricao = "Campo Alegre" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 10, Descricao = "Senador Rui Palmeira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 11, Descricao = "Jaramataia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 12, Descricao = "Carneiros" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 13, Descricao = "Dois Riachos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 14, Descricao = "Inhapi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 15, Descricao = "Jacuipe" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 16, Descricao = "Jundia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 17, Descricao = "Lagoa Da Canoa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 18, Descricao = "Demais Municipios" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 19, Descricao = "Maribondo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 20, Descricao = "Mar Vermelho" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 21, Descricao = "Matriz De Camaragibe" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 22, Descricao = "Minador Do Negrao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 23, Descricao = "Murici" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 24, Descricao = "Novo Lino" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 25, Descricao = "Olho D'Agua Do Casado" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 26, Descricao = "Pindoba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 27, Descricao = "Piacabucu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 28, Descricao = "Porto De Pedras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 29, Descricao = "Rio Largo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 30, Descricao = "Sao Luis Do Quitunde" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 31, Descricao = "Satuba" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 32, Descricao = "Craibas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 33, Descricao = "Paripueira" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 34, Descricao = "Pariconha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 35, Descricao = "Penedo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 36, Descricao = "Messias" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 37, Descricao = "Barra De Santo Antonio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 38, Descricao = "Joaquim Gomes" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 39, Descricao = "Branquinha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 40, Descricao = "Campo Grande" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 41, Descricao = "Monteiropolis" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 42, Descricao = "Feira Grande" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 43, Descricao = "Ibateguara" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 44, Descricao = "Japaratinga" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 45, Descricao = "Major Isidoro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 46, Descricao = "Limoeiro De Anadia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 47, Descricao = "Mata Grande" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 48, Descricao = "Olho D'Agua Grande" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 49, Descricao = "Igreja Nova" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 50, Descricao = "Passo De Camaragibe" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 51, Descricao = "Belo Monte" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 52, Descricao = "Porto Calvo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 53, Descricao = "Roteiro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 54, Descricao = "Sao Bras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 55, Descricao = "Tanque D'Arca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 56, Descricao = "Delmiro Gouveia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 57, Descricao = "Ouro Branco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 58, Descricao = "Vicosa" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 59, Descricao = "Marechal Deodoro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 60, Descricao = "Maragogi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 61, Descricao = "Sao Jose Da Laje" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 62, Descricao = "Sao Miguel Dos Campos" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 63, Descricao = "Uniao Dos Palmares" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 64, Descricao = "Cajueiro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 65, Descricao = "Palestina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 66, Descricao = "Belem" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 67, Descricao = "Sao Miguel Dos Milagres" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 68, Descricao = "Paulo Jacinto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 69, Descricao = "Jacare Dos Homens" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 70, Descricao = "Sao Sebastiao" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 71, Descricao = "Colonia Leopoldina" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 72, Descricao = "Santana Do Mundau" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 73, Descricao = "Quebrangulo" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 74, Descricao = "Olivenca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 75, Descricao = "Cha Preta" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 76, Descricao = "Atalaia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 77, Descricao = "Coite Do Noia" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 78, Descricao = "Pao De Acucar" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 79, Descricao = "Traipu" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 80, Descricao = "Batalha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 81, Descricao = "Boca Da Mata" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 82, Descricao = "Pilar" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 83, Descricao = "Cacimbinhas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 84, Descricao = "Taquarana" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 85, Descricao = "Santana Do Ipanema" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 86, Descricao = "Santa Luzia Do Norte" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 87, Descricao = "Coqueiro Seco" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 88, Descricao = "Maravilha" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 89, Descricao = "Arapiraca" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 90, Descricao = "Palmeira Dos Indios" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 91, Descricao = "Poco Das Trincheiras" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 92, Descricao = "Igaci" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 93, Descricao = "Olho D'Agua Das Flores" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 94, Descricao = "Junqueiro" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 95, Descricao = "Girau Do Ponciano" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 96, Descricao = "Estrela De Alagoas" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 97, Descricao = "Canapi" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 98, Descricao = "Feliz Deserto" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 99, Descricao = "Sao Jose Da Tapera" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 100, Descricao = "Coruripe" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 101, Descricao = "Maceio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 102, Descricao = "Porto Real Do Colegio" }
            );

            context.Municipio.AddOrUpdate(
                m => new { m.CodPais, m.CodEstado, m.CodMunicipio },
                new Municipio { CodPais = 1, CodEstado = 2, CodMunicipio = 103, Descricao = "Flexeiras" }
            );

            #endregion
        }
    }
}