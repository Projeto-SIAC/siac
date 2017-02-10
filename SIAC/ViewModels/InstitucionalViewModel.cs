/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
using SIAC.Models;
using System.Collections.Generic;

namespace SIAC.ViewModels
{
    public class InstitucionalRealizarViewModel
    {
        public AvalAvi Avi { get; set; }
        public List<AviQuestaoPessoaResposta> Respostas { get; set; }
    }

    public class InstitucionalPublicoViewModel
    {
        public AvalAvi Avi { get; set; }
        public List<AviTipoPublico> TiposPublico { get; set; }
    }

    public class InstitucionalGerarQuestaoViewModel
    {
        public List<AviModulo> Modulos { get; set; }
        public List<AviCategoria> Categorias { get; set; }
        public List<AviIndicador> Indicadores { get; set; }
        public List<TipoQuestao> Tipos { get; set; }
        public AvalAvi Avi { get; set; }
    }
}