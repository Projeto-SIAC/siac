siac.Usuario = siac.Usuario || {};

siac.Usuario.Index = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 10, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var categoria = "";
    var pesquisa = "";

    function iniciar() {
        $('.ui.dropdown').dropdown();

        $('.pesquisa input').keyup(function () {
            var _this = this;
            if (_controleTimeout) {
                clearTimeout(_controleTimeout);
            }
            _controleTimeout = setTimeout(function () {
                pesquisa = _this.value;
                pagina = 1;
                listar();
            }, 500);
        });

        $('.button.topo').click(function () {
            topo();
        });

        $('.categoria.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            categoria = $_this.attr('data-categoria');
            listar();
        });

        $('.ordenar.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            ordenar = $_this.attr('data-ordenar');
            listar();

            $('.ordenar.item').removeClass('active');
            $_this.addClass('active');
        });

        $('.carregar.button').click(function () {
            if ($('.table tbody tr').length == (_controleQte * pagina)) {
                pagina++;
                listar();
            }
        });

        listar();
    };

    function listar() {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        $table = $('.ui.table');
        $table.parent().addClass('loading');
        _controleAjax = $.ajax({
            url: '/configuracoes/usuario/listar',
            data: {
                pagina: pagina,
                ordenar: ordenar,
                categoria: categoria,
                pesquisa: pesquisa
            },
            method: 'POST',
            success: function (partial) {
                if (partial != _controlePartial && partial.indexOf('<html') < 0) {
                    if (pagina == 1) {
                        $table.find('tbody').html(partial);
                    }
                    else {
                        $table.find('tbody').append(partial);
                    }
                    _controlePartial = partial;
                }
            },
            complete: function () {
                $table.parent().removeClass('loading');
                if ($('.table tbody tr').length < (_controleQte * pagina)) {
                    $('.carregar.button').parents('tfoot').hide();
                }
                else {
                    $('.carregar.button').parents('tfoot').show();
                }
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
