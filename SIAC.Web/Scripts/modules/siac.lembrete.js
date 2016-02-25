siac.Lembrete = siac.Lembrete || {};

siac.Lembrete.iniciar = function () {
    alertify.set('notifier', 'position', 'bottom-left');

    var _matricula = $('[data-matricula]').data('matricula');

    var pathname = window.location.pathname.toLowerCase();

    var hub = $.connection.lembreteHub;

    hub.client.receberNotificacoes = function (data) {
        if (data) {
            for (var i in data) {
                alertify.notify(data[i]['Mensagem'], data[i]['Estilo'], data[i]['Tempo']);
            }
        }
    };

    hub.client.receberMenu = function (data) {
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                if (data[key] > 0) {
                    $('.menu [data-lembrete=' + key + '] .icon').remove();
                    $('.menu [data-lembrete=' + key + ']').append('<div class="ui label">' + data[key] + '</div>');
                }
            }
        }
    };

    hub.client.receberContadores = function (data) {
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                if (data[key] > 0) {
                    $('[data-lembrete=' + key + ']').append('<div class="ui floating small red label">' + data[key] + '</div>');
                }
            }
        }
    };

    hub.client.receberLembretes = function (data) {
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
        hub.server.recuperarNotificacoes(_matricula);
        hub.server.recuperarMenu(_matricula);
        if (pathname == "/principal") {
            hub.server.recuperarContadoresPrincipal(_matricula);
        }
        else if (pathname == "/institucional") {
            hub.server.recuperarContadoresInstitucional(_matricula);
        }
        hub.server.recuperarLembretes(_matricula);
    });
};

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