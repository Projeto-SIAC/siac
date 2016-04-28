using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Simulado
    {
        private static dbSIACEntities contexto = Repositorio.GetInstance();

        public string Codigo => $"SIMUL{Ano}{NumIdentificador.ToString("00000")}";

        public static void Inserir(Simulado simulado)
        {
            contexto.Simulado.Add(simulado);
            contexto.SaveChanges();
        }

        public static int ObterNumIdentificador() => contexto.Simulado.Where(s => s.Ano == DateTime.Now.Year).Count() + 1;

        public static Simulado ListarPorCodigo(string codigo)
        {
            int numIdentificador = int.Parse(codigo.Substring(codigo.Length - 5));
            codigo = codigo.Remove(codigo.Length - 5);
            int ano = int.Parse(codigo.Substring(codigo.Length - 4));

            return contexto.Simulado.FirstOrDefault(s => s.Ano == ano && s.NumIdentificador == numIdentificador);
        }
    }
}