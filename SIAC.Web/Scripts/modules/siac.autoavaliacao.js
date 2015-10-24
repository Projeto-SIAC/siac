siac.Autoavaliacao = siac.Autoavaliacao || {};

siac.Autoavaliacao.Index = (function () {
    var controleTimeout;

    var pagina = 1;
    var ordenar = "data_desc";
    var categorias = [];
    var dificuldade = "";
    var disciplina = "";
    var pesquisa = "";

    function iniciar() {
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
                pagina++;
                listar();
            }
        });
        $('.ui.dropdown').dropdown();

        $('.pesquisa input').keyup(function () {
            var _this = this;
            if (controleTimeout) {
                clearTimeout(controleTimeout);
            }
            controleTimeout = setTimeout(function () {
                pesquisa = _this.value;
                listar();
            }, 500);
        });

        $('.button.topo').click(function () {
            topo();
        });

        $('.categoria.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            var _categoria = $_this.attr('data-categoria');
            if ($_this.hasClass('active')) {
                var _tempCategorias = categorias;
                categorias = [];
                for (var i = 0, length = _tempCategorias.length; i < length; i++) {
                    if (_tempCategorias[i] != _categoria) {
                        categorias.push(_tempCategorias[i]);
                    }
                }
                $_this.removeClass('active');
            }
            else {
                categorias.push(_categoria);
                $_this.addClass('active');
            }
            listar();
        });

        $('.dificuldade.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            dificuldade = $_this.attr('data-dificuldade');
            listar();
        });

        $('.disciplina.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            disciplina = $_this.attr('data-disciplina');            
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

        listar();    
    }

    function listar() {
        $cards = $('.ui.cards');
        $cards.parent().addClass('loading');
        $.ajax({
            url: '/Historico/Autoavaliacao/Listar',
            data: {
                pagina: pagina,
                ordenar: ordenar,
                dificuldade: dificuldade,
                disciplina: disciplina,
                categorias: categorias,
                pesquisa: pesquisa
            },
            method: 'POST',
            success: function (partial) {
                if (pagina == 1) {
                    $cards.html(partial);
                }
                else {
                    $cards.append(partial);
                }
            },
            complete: function () {
                $cards.parent().removeClass('loading');
            }
        });
    }

    function topo() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    }

    function arquivar(strCodigo) {
        $.ajax({
            type: 'POST',
            data: { codigo: strCodigo },
            url: "/Dashboard/Autoavaliacao/Arquivar",
            success: function () {
                window.location.href = '/Historico/Autoavaliacao';
            },
            error: function () {
                siac.mensagem('Não foi possível arquivar a autoavaliação.')
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();