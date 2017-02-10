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
using SIAC.Helpers;
using System.Collections.Generic;

namespace SIAC.Models
{
    public class Lembrete
    {
        public const string NORMAL = "label";
        public const string POSITIVO = "green";
        public const string NEGATIVO = "red";
        public const string INFO = "blue";

        public static void AdicionarNotificacao(string mensagem, string estilo = NORMAL, int tempo = 5)
        {
            Sistema.Notificacoes[Sessao.UsuarioMatricula].Add(new Dictionary<string, string> {
                { "Mensagem", mensagem },
                { "Estilo", estilo },
                { "Tempo", tempo.ToString() }
            });
        }
    }
}