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

siac.Usuario.Detalhe = (function () {
    var _controlePartial, _controleQte = 10, _controleAjax;

    var pagina = 1;

    function iniciar() {
        $('.ui.modal').modal();
        $('.carregar.button').click(function () {
            if ($('.table tbody tr').length == (_controleQte * pagina)) {
                pagina++;
                listar();
            }
        });
        $('.acesso.table .expandir.button').click(function () {
            var $this = $(this);
            expandir($this);
        });
        listar();
    }

    function expandir($this) {
        $this.addClass('loading');
        var $tr = $this.parents('tr');
        var matricula = window.location.pathname.split('/configuracoes/usuario/detalhe/')[1];
        var acesso = $tr.data('acesso');
        var header = $tr.find('time').attr('title');
        $.ajax({
            url: '/configuracoes/usuario/listaracessopagina',
            type: 'post',
            data: {
                matricula: matricula,
                codOrdem: acesso
            },
            success: function (partial) {
                if (partial) {
                    var $modal = $('.acesso.modal');
                    $modal.find('.header').text(header);
                    $modal.find('.content').html(partial);
                    $modal.modal('show');
                }
                $modal.find('i').popup();
            },
            error: function () {
                siac.mensagem('Ocorreu um erro durante sua solicitação. Tente novamente mais tarde, por favor.');
            },
            complete: function () {
                $this.removeClass('loading');
            }
        });
    }

    function listar() {
        var matricula = window.location.pathname.split('/configuracoes/usuario/detalhe/')[1];
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        $table = $('.acesso.table');
        $table.parent().addClass('loading');
        _controleAjax = $.ajax({
            url: '/configuracoes/usuario/listaracesso',
            data: {
                matricula: matricula,
                pagina: pagina
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
                $('.acesso.table .expandir.button').click(function () {
                    var $this = $(this);
                    expandir($this);
                });
                if ($('.table tbody tr').length < (_controleQte * pagina)) {
                    $('.carregar.button').parents('tfoot').hide();
                }
                else {
                    $('.carregar.button').parents('tfoot').show();
                }
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();