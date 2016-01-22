siac.Agenda = siac.Agenda || (function () {
    function iniciar() {
        $('#calendar').fullCalendar({
            header: {
                left: 'title',
                center: '',
                right: 'today agendaDay,agendaWeek,month prev,next'
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