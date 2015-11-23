using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIAC.Models
{
    public class MapAviModulo
    {
        public AviModulo Modulo { get; set; }
        public List<MapAviCategoria> Categorias { get; set; } = new List<MapAviCategoria>();
    }

    public class MapAviCategoria
    {
        public AviCategoria Categoria { get; set; }
        public List<MapAviIndicador> Indicadores { get; set; } = new List<MapAviIndicador>();
    }

    public class MapAviIndicador
    {
        public AviIndicador Indicador { get; set; }
        public List<AviQuestao> Questoes { get; set; } = new List<AviQuestao>();
    }
}