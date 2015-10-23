using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public partial class Categoria
    {
        private static dbSIACEntities contexto { get { return Repositorio.GetInstance(); } }

        public static Categoria ListarPorCodigo(int codCategoria)
        {
            return contexto.Categoria.FirstOrDefault(c => c.CodCategoria == codCategoria);
        }
    }
}