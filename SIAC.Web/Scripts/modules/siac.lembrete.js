siac.Lembrete = siac.Lembrete || {};

siac.Lembrete.iniciar = function () {
    //alertify.set('notifier', 'position', 'top-right');

    siac.Lembrete.Notificacoes.iniciar();
    siac.Lembrete.Lembretes.iniciar();
    siac.Lembrete.Menu.iniciar();

    var pathname = window.location.pathname.toLowerCase();
    if (pathname == "/principal") {
        siac.Lembrete.Principal.iniciar();
    }
    else if (pathname == "/institucional") {
        siac.Lembrete.Institucional.iniciar();
    }
};

siac.Lembrete.Notificacoes = siac.Lembrete.Notificacoes || (function () {
    function iniciar() {
        $.ajax({
            url: '/lembrete/notificacoes',
            type: 'post',
            cache: true,
            success: function (data) {
                if (data) {
                    for (var i in data) {
                        alertify.notify(data[i]['Mensagem'], data[i]['Estilo'], data[i]['Tempo']);
                    }
                }
            }
        });
    }

    function exibir(mensagem, estilo, tempo) {
        var mapaEstilo = {
            'normal': 'label',
            'positivo': 'green',
            'negativo': 'red',
            'info': 'info'
        }
        if (mensagem) {
            if (!(/(normal|positivo|negativo|info)/.test(estilo))) {
                estilo = 'normal';
            }
            alertify.notify(mensagem, mapaEstilo[estilo], tempo);
        }
    }

    return { iniciar: iniciar, exibir: exibir }
})();

siac.Lembrete.Lembretes = siac.Lembrete.Lembretes || (function () {
    function iniciar() {
        $.ajax({
            url: '/lembrete/lembretes',
            type: 'post',
            cache: true,
            success: function (data) {
                if (data) {
                    for (var i in data) {
                        if (data[i]['Botao']) {
                            var str = '<p>'+data[i]['Mensagem']+'</p>'+
                                       '<a href="' + data[i]['Url'] + '" class="ui black basic button">' + data[i]['Botao'] + '</a>';
                            alertify.notify(str, 'label', 0, function (clicado) { console.log('clicado'); lembreteVisualizado(clicado, data[i]['Id']) });
                        }
                        else {
                            alertify.notify(data[i]['Mensagem'], 'label', 0, function (clicado) { console.log('clicado'); lembreteVisualizado(clicado, data[i]['Id']) });
                        }
                    }
                }                
            }
        });
    }

    function lembreteVisualizado(clicado, id) {
        console.log('evento');
        if (clicado) {
            $.ajax({
                url: '/lembrete/lembretevisualizado',
                type: 'post',
                data: { id: id}
            });
        }
    }

    return { iniciar: iniciar }
})();

siac.Lembrete.Menu = siac.Lembrete.Menu || (function () {
    function iniciar() {
        $.ajax({
            url: '/lembrete/menu',
            type: 'post',
            cache: true,
            success: function (data) {
                for (var key in data) {
                    if (data.hasOwnProperty(key)) {
                        if (data[key] > 0) {
                            $('.menu [data-lembrete=' + key + '] .icon').remove();
                            $('.menu [data-lembrete=' + key + ']').append('<div class="ui label">' + data[key] + '</div>');
                        }
                    }
                }
            }
        });
    }

    return { iniciar: iniciar }
})();

siac.Lembrete.Principal = siac.Lembrete.Principal || (function () {
    function iniciar() {
        contadores();
    }

    function contadores() {
        $.ajax({
            url: '/lembrete/principal',
            type: 'post',
            cache: true,
            success: function (data) {
                for (var key in data) {
                    if (data.hasOwnProperty(key)) {
                        if (data[key] > 0) {
                            $('[data-lembrete=' + key + ']').append('<div class="ui floating small red label">' + data[key] + '</div>');
                        }
                    }
                }
            }
        });
    }

    return { iniciar: iniciar }
})();

siac.Lembrete.Institucional = siac.Lembrete.Institucional || (function () {
    function iniciar() {
        contadores();
    }

    function contadores() {
        $.ajax({
            url: '/lembrete/institucional',
            type: 'post',
            cache: true,
            success: function (data) {
                for (var key in data) {
                    if (data.hasOwnProperty(key)) {
                        if (data[key] > 0) {
                            $('[data-lembrete=' + key + ']').append('<div class="ui floating small red label">' + data[key] + '</div>');
                        }
                    }
                }
            }
        });
    }

    return { iniciar: iniciar }
})();