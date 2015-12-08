siac.Lembrete = siac.Lembrete || {};

siac.Lembrete.iniciar = function () {
    var pathname = window.location.pathname.toLowerCase();
    siac.Lembrete.Menu.iniciar();
    if (pathname == "/dashboard") {
        siac.Lembrete.Dashboard.iniciar();
    }
    else if (pathname == "/institucional") {
        siac.Lembrete.Institucional.iniciar();
    }
};

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

siac.Lembrete.Dashboard = siac.Lembrete.Dashboard || (function () {
    function iniciar() {
        contadores();
    }

    function contadores() {
        $.ajax({
            url: '/lembrete/dashboard',
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