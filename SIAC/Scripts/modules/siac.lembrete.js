siac.Lembrete = siac.Lembrete || (function () {
    var _matricula, _hub;

    function iniciar() {
        alertify.set('notifier', 'position', 'bottom-left');

        _matricula = $('[data-matricula]').data('matricula');

        var pathname = window.location.pathname.toLowerCase();

        _hub = $.connection.lembreteHub;

        _hub.client.receberNotificacoes = function (data) {
            if (data) {
                for (var i in data) {
                    alertify.notify(data[i]['Mensagem'], data[i]['Estilo'], data[i]['Tempo']);
                }
            }
        };

        _hub.client.receberMenu = function (data) {
            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    if (data[key] > 0) {
                        $('.menu [data-lembrete=' + key + '] .icon').remove();
                        $('.menu [data-lembrete=' + key + ']').append('<div class="ui label">' + data[key] + '</div>');
                    }
                }
            }
        };

        _hub.client.receberContadores = function (data) {
            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    if (data[key] > 0) {
                        $('[data-lembrete=' + key + ']').append('<div class="ui floating small red label">' + data[key] + '</div>');
                    }
                }
            }
        };

        _hub.client.receberLembretes = function (data) {
            if (data) {
                for (var i in data) {
                    if (data[i]['Botao']) {
                        var str = '<p>' + data[i]['Mensagem'] + '</p>' +
                                   '<a href="' + data[i]['Url'] + '" class="ui black basic button">' + data[i]['Botao'] + '</a>';
                        alertify.notify(str, 'label', 0, function (clicado) { hub.server.lembreteVisualizado(_matricula, clicado, data[i]['Id']) });
                    }
                    else {
                        alertify.notify(data[i]['Mensagem'], 'label', 0, function (clicado) { hub.server.lembreteVisualizado(_matricula, clicado, data[i]['Id']) });
                    }
                }
            }
        }

        $.connection.hub.start().done(function () {
            _hub.server.recuperarNotificacoes(_matricula);
            _hub.server.recuperarMenu(_matricula);
            if (pathname == "/principal") {
                _hub.server.recuperarContadoresPrincipal(_matricula);
            }
            else if (pathname == "/institucional") {
                _hub.server.recuperarContadoresInstitucional(_matricula);
            }
            _hub.server.recuperarLembretes(_matricula);
            $(document).ajaxComplete(function () {
                recuperarNotificacoes();
            });
        });
    }

    function recuperarNotificacoes() {
        _hub.server.recuperarNotificacoes(_matricula);
    }

    return {
        iniciar: iniciar,
        recuperarNotificacoes: recuperarNotificacoes
    }
})();

siac.Lembrete.Notificacoes = siac.Lembrete.Notificacoes || (function () {
    function exibir(mensagem, estilo, tempo) {
        var mapaEstilo = {
            'normal': 'label',
            'positivo': 'green',
            'negativo': 'red',
            'info': 'blue'
        }
        if (mensagem) {
            if (!(/(normal|positivo|negativo|info)/.test(estilo))) {
                estilo = 'normal';
            }
            return alertify.notify(mensagem, mapaEstilo[estilo], tempo);
        }
    }

    return { exibir: exibir }
})();