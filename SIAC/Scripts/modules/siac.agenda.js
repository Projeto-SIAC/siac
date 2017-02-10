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
siac.Agenda = siac.Agenda || (function () {
    function iniciar() {
        $('#calendar').fullCalendar({
            header: {
                left: 'title',
                center: '',
                right: 'today agendaDay,agendaWeek,month prev,next'
            },
            loading: function (isLoading, view) {
                if (isLoading) {
                    $('.agenda.segment').dimmer({
                        closable: false
                    }).dimmer('show');
                }
                else {
                    $('.agenda.segment').dimmer('hide');
                }
            },
            lang: 'pt-br',
            eventSources: [
                {
                    url: '/principal/agenda/certificacoes',
                    type: 'POST',
                    color: '#009688'
                },
                {
                    url: '/principal/agenda/academicas',
                    type: 'POST',
                    color: '#2196F3'
                },
                {
                    url: '/principal/agenda/reposicoes',
                    type: 'POST',
                    color: '#EF5350'
                },
                {
                    url: '/principal/agenda/horarios',
                    type: 'POST',
                    color: '#4CAF50',
                    cache: true,
                    rendering: 'background'
                }
            ]
        });
    }

    return {
        iniciar: iniciar
    }
})();