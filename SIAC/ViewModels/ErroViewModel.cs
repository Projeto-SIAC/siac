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
namespace SIAC.ViewModels
{
    public class ErroIndexViewModel
    {
        public string Codigo { get; set; } = "desconhecido";
        public string Titulo { get; set; } = "Volte ao início";
        public string Mensagem { get; set; } = "Ocorreu um erro inesperado";

        public ErroIndexViewModel()
        {
        }

        public ErroIndexViewModel(string codigo, string titulo, string mensagem)
        {
            this.Codigo = codigo;
            this.Titulo = titulo;
            this.Mensagem = mensagem;
        }
    }
}