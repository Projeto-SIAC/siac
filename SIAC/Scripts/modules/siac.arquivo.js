siac.Arquivo = siac.Arquivo || {};

siac.Arquivo.Abertos = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 12, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var categoria = "abertos";
    var pesquisa = "";

    function iniciar() {
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() * 0.75) {
                if ($('.cards .card').length == (_controleQte * pagina)) {
                    pagina++;
                    listar();
                }
            }
        });

        $('.ui.dropdown').dropdown();

        $('.pesquisa input').keyup(function () {
            var _this = this;
            if (_controleTimeout) {
                clearTimeout(_controleTimeout);
            }
            _controleTimeout = setTimeout(function () {
                pesquisa = _this.value;
                pagina = 1;
                $(_this).closest('.input').addClass('loading');
                listar();
            }, 500);
        });

        $('.button.topo').click(function () {
            topo();
        });

        $('.ordenar.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            ordenar = $_this.attr('data-ordenar');
            $_this.closest('.segment').addClass('loading');
            listar();
            $('.ordenar.item').removeClass('active');
            $_this.addClass('active');
        });

        listar();
    }

    function listar() {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        var $cards = $('.ui.cards');
        $cards.parent().append('<div class="ui active centered inline text loader">Carregando</div>');
        _controleAjax = $.ajax({
            method: 'POST',
            url: $cards.data('action'),
            data: {
                pagina: pagina,
                ordenar: ordenar,
                categoria: categoria,
                pesquisa: pesquisa
            },
            success: function (partial) {
                if (partial != _controlePartial) {
                    if (pagina == 1) {
                        $cards.html(partial);
                    }
                    else {
                        $cards.append(partial);
                    }
                    _controlePartial = partial;
                }
            },
            complete: function () {
                $('.segment.loading').removeClass('loading');
                $('.input.loading').removeClass('loading');
                $cards.parent().find('.loader').remove();
            }
        });
    }

    function topo() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    }

    return {
        iniciar: iniciar
    }
})();

siac.Arquivo.Encerrados = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 12, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var categoria = "encerrados";
    var pesquisa = "";

    function iniciar() {
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() * 0.75) {
                if ($('.cards .card').length == (_controleQte * pagina)) {
                    pagina++;
                    listar();
                }
            }
        });

        $('.ui.dropdown').dropdown();

        $('.pesquisa input').keyup(function () {
            var _this = this;
            if (_controleTimeout) {
                clearTimeout(_controleTimeout);
            }
            _controleTimeout = setTimeout(function () {
                pesquisa = _this.value;
                pagina = 1;
                $(_this).closest('.input').addClass('loading');
                listar();
            }, 500);
        });

        $('.button.topo').click(function () {
            topo();
        });

        $('.ordenar.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            ordenar = $_this.attr('data-ordenar');
            $_this.closest('.segment').addClass('loading');
            listar();
            $('.ordenar.item').removeClass('active');
            $_this.addClass('active');
        });

        listar();
    }

    function listar() {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        var $cards = $('.ui.cards');
        $cards.parent().append('<div class="ui active centered inline text loader">Carregando</div>');
        _controleAjax = $.ajax({
            method: 'POST',
            url: $cards.data('action'),
            data: {
                pagina: pagina,
                ordenar: ordenar,
                categoria: categoria,
                pesquisa: pesquisa
            },
            success: function (partial) {
                if (partial != _controlePartial) {
                    if (pagina == 1) {
                        $cards.html(partial);
                    }
                    else {
                        $cards.append(partial);
                    }
                    _controlePartial = partial;
                }
            },
            complete: function () {
                $('.segment.loading').removeClass('loading');
                $('.input.loading').removeClass('loading');
                $cards.parent().find('.loader').remove();
            }
        });
    }

    function topo() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    }

    return {
        iniciar: iniciar
    }
})();