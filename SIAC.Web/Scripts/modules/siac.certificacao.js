siac.Certificacao = siac.Certificacao || {};

siac.Certificacao.Gerar = (function () {

    function iniciar() {
        $('.ui.dropdown').dropdown();
        $('.ui.modal').modal();
        $('.ui.termo.modal').modal({ closable: false }).modal('show');
        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal').modal({
            onApprove: function () {
                $('form').addClass('loading').submit();
            }
        });

        $('.prosseguir.button').click(function () {
            prosseguir()
        });

        $('#ddlDisciplina').change(function () {
            listarTemas(this.value);
        })

        $('#ddlTipo').change(function () {
            mostrarOpcoesPorTipo(this.value);
        })


    }

    function prosseguir() {
        var errorList = $('form .error.message .list');

        errorList.html('');
        $('form').removeClass('error');

        var valido = true;

        if (!$('#ddlDisciplina').val()) {
            errorList.append('<li>Selecione uma disciplina</li>');
            valido = false;
        }

        if (!$('#ddlTemas').val()) {
            errorList.append('<li>Selecione ao menos um tema</li>');
            valido = false;
        }

        if (!$('#ddlTipo').val()) {
            errorList.append('<li>Selecione o tipo das questões da sua avaliação acadêmica</li>');
            valido = false;
        }

        if (($('#ddlTipo').val() == 1 || $('#ddlTipo').val() == 3) && !$('#txtQteObjetiva').val()) {
            errorList.append('<li>Indique a quantidade das questões objetivas</li>');
            valido = false;
        }
        if (($('#ddlTipo').val() == 2 || $('#ddlTipo').val() == 3) && !$('#txtQteDiscursiva').val()) {
            errorList.append('<li>Indique a quantidade das questões discursivas</li>');
            valido = false;
        }

        if (!$('#ddlDificuldade').val()) {
            errorList.append('<li>Selecione a dificuldade das questões</li>');
            valido = false;
        }


        if (valido) {
            confirmar();
        }
        else {
            $('form').addClass('error');
            $('html, body').animate({
                scrollTop: $('form .error.message').offset().top
            }, 1000);
        }
    }

    function confirmar() {
        $modal = $('.ui.confirmar.modal');
        $ddlDisciplina = $('#ddlDisciplina :selected');
        $ddlTipo = $('#ddlTipo');
        $table = $modal.find('tbody').html('');

        $tr = $('<tr></tr>');
        $tdDisciplina = $('<td></td>').html('<b>' + $ddlDisciplina.text() + '</b>');
        $tdTemas = $('<td class="ui labels"></td>');

        $ddlTemas = $('#ddlTemas :selected');
        for (var i = 0; i < $ddlTemas.length; i++) {
            $tdTemas.append($('<div class="ui tag label"></div>').text($ddlTemas.eq(i).text()));
        }
        $tdQteQuestoes = $('<td class="ui labels"></td>');
        if ($ddlTipo.val() == 1 || $ddlTipo.val() == 3) {
            $tdQteQuestoes.append($('<div class="ui label"></div>').html('Objetiva<div class="detail">' + $('#txtQteObjetiva').val() + '</div>'));
        }
        if ($ddlTipo.val() == 2 || $ddlTipo.val() == 3) {
            $tdQteQuestoes.append($('<div class="ui label"></div>').html('Discursiva<div class="detail">' + $('#txtQteDiscursiva').val() + '</div>'));
        }
        $tdDificuldade = $('<td></td>').text($('#ddlDificuldade :selected').text());

        $table.append($tr.append($tdDisciplina).append($tdTemas).append($tdQteQuestoes).append($tdDificuldade));

        $modal.modal('show');
    }

    function listarTemas(codDisciplina) {
        var disciplinas = $('#ddlDisciplinas');
        var ddlTemas = $('#ddlTemas');

        ddlTemas.parent().addClass('loading');
        $.ajax({
            type: 'POST',
            url: '/Tema/RecuperarTemasPorCodDisciplinaTemQuestao',
            data: { 'codDisciplina': codDisciplina },
            success: function (data) {
                ddlTemas.html('');
                ddlTemas.val(ddlTemas.prop('defaultSelected'));
                $.each(data, function (id, item) {
                    ddlTemas.append('<option value="' + item.CodTema + '">' + item.Descricao + '</option>');
                });

                ddlTemas.parent().removeClass('loading');
            },
            error: function () {
                siac.mensagem("Erro no carregamento de Temas", "Erro");
                ddlTemas.parent().removeClass('loading');
            }
        });
    }

    function mostrarOpcoesPorTipo(tipoAvaliacao) {
        var txtQteObjetiva = $('#txtQteObjetiva');
        var txtQteDiscursiva = $('#txtQteDiscursiva');

        if (tipoAvaliacao == 1) {
            txtQteObjetiva.prop('disabled', false);
            txtQteDiscursiva.prop('disabled', true);
        }
        else if (tipoAvaliacao == 2) {
            txtQteObjetiva.prop('disabled', true);
            txtQteDiscursiva.prop('disabled', false);
        }
        else {
            txtQteObjetiva.prop('disabled', false);
            txtQteDiscursiva.prop('disabled', false);
        }
    }

    return {
        iniciar: iniciar
    }

})();

siac.Certificacao.Configurar = (function () {
    var _codAvaliacao;
    var _arrayQuestoes = [];
    var _qteObjetiva = 0;
    var _qteDiscursiva = 0;
    var _qteMaxObjetiva;
    var _qteMaxDiscursiva;

    var _OBJ = "Objetiva";
    var _DISC = "Discursiva";
    var _ADD = "Adicionar";
    var _REM = "Remover"

    var _controleModal;

    function iniciar() {
        //Obtendo Dados da Página
        $elemento = $('[data-avaliacao]');
        if (!_codAvaliacao && $elemento) {
            _codAvaliacao = $elemento.data('avaliacao');
            $elemento.removeAttr("data-avaliacao");
        }
        _qteMaxObjetiva = $('.informacoes .objetiva.label .detail').html();
        _qteMaxDiscursiva = $('.informacoes .discursiva.label .detail').html();

        $('.questoes.modal .card').map(function () {
            _arrayQuestoes.push($(this).attr('id'))
            if ($(this).find('.tipo.label').text() == _OBJ) _qteObjetiva++;
            else if ($(this).find('.tipo.label').text() == _DISC) _qteDiscursiva++;
        });
        //Fim da Obtenção de Dados

        $('.questao.modal').modal({
            onDeny: function () {
                if (_controleModal) {
                    _controleModal.modal('show');
                }
            }
        });

        $('.informacoes.button').click(function () {
            $('.informacoes.modal').modal('show');
        });

        $('.ui.dropdown').dropdown().change(function () {
            carregarQuestoes();
        });

        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal').modal({
            onApprove: function () {
                $('.confirmar.button').addClass('loading');
                $.ajax({
                    type: 'POST',
                    url: '/dashboard/avaliacao/certificacao/Configurar/',
                    data: {
                        codigo: _codAvaliacao,
                        questoes: _arrayQuestoes
                    },
                    success: function (data) {
                        if (data) {
                            window.location.href = "/dashboard/avaliacao/certificacao/agendar/" + _codAvaliacao;
                        }
                    },
                    error: function (data) {
                        siac.aviso('error', 'red');
                        $('.ui.global.loader').parent().dimmer('hide');
                    }
                });
            }
        });

        $('.confirmar.button').click(function () {
            if (_arrayQuestoes.length >= _qteMaxDiscursiva + _qteMaxObjetiva) {
                prosseguir($(this));
            }
            else siac.aviso('Você ainda não selecionou questões suficientes para esta avaliação', 'red');
        });

        $('.ui.questoes.button').click(function () {
            $('.questoes.modal').modal('show');
        });

        $('.ui.detalhe.button').click(function () {
            var codQuestao = $(this).parents('.card').attr('id');
            _controleModal = $(this).parents('.modal');
            mostrarQuestao(codQuestao, this);
        });

        $('.ui.acao.button').click(function () {
            $_this = $(this);
            var tipo = $_this.parents('.card').find('.label').last().text();
            var id = $_this.parents('.card').attr('id');
            if ($_this.html() == "Adicionar") {
                adicionarQuestao(id, tipo);
            } else if ($_this.html() == "Remover") {
                removerQuestao(id, tipo);
            }
        });

        atualizarQuantidadeView();

    }

    function carregarQuestoes() {
        var temas = $('#ddlTemas').val();
        var dificuldade = $('#ddlDificuldade').val();
        var tipo = $('#ddlTipo').val();
        if (temas && dificuldade && tipo) {
            $resultado = $('.resultado .cards');
            $resultado.addClass('form loading')
            $.ajax({
                type: 'POST',
                url: '/dashboard/avaliacao/certificacao/CarregarQuestoes/',
                data: {
                    codigo: _codAvaliacao,
                    temas: temas,
                    dificuldade: dificuldade,
                    tipo: tipo
                },
                success: function (data) {
                    if (data) {
                        $cards = $(data);
                        $resultado.html($cards);

                        $resultado.find('.card').map(function () {
                            $card = $(this);
                            var id = $card.attr('id');
                            if (_arrayQuestoes.indexOf(id) > -1) {
                                $card.find('.acao.button').text(_REM);
                            }
                        });
                    }
                },
                error: function (data) {
                    siac.mensagem(data, 'Error');
                },
                complete: function () {
                    $resultado.removeClass('form loading');

                    $resultado.find('.acao.button').off('click').click(function () {
                        $_this = $(this);
                        var tipo = $_this.parents('.card').find('.tipo.label').text();
                        if ($_this.html() == _ADD) {
                            var id = $_this.parents('.card').attr('id');
                            adicionarQuestao(id, tipo);
                        } else if ($_this.html() == _REM) {
                            var id = $_this.parents('.card').attr('id');
                            removerQuestao(id, tipo);
                        }
                    });

                    $('.ui.detalhe.button').click(function () {
                        var codQuestao = $(this).parents('.card').attr('id');
                        _controleModal = $(this).parents('.modal');
                        mostrarQuestao(codQuestao,this);
                    });
                }
            });
        }
    }

    function adicionarQuestao(codQuestao, tipo) {
        var podeAdicionar = false;
        if (tipo == _OBJ) {
            if (_qteObjetiva < _qteMaxObjetiva) {
                podeAdicionar = true;
                _qteObjetiva++;
            }
            else
                siac.aviso('Você não pode adicionar mais questões objetivas', 'red');

        } else if (tipo == _DISC) {
            if (_qteDiscursiva < _qteMaxDiscursiva) {
                podeAdicionar = true;
                _qteDiscursiva++;
            }
            else
                siac.aviso('Você não pode adicionar mais questões discursivas', 'red');
        }
        if (podeAdicionar) {
            $card = $('#' + codQuestao + '.card');
            $card.find('.acao.button').text(_REM).off('click').click(function () {
                removerQuestao(codQuestao, tipo);
            });
            _arrayQuestoes.push(codQuestao);
            $card.transition({
                onHide: function () {
                    $('.questoes.modal .cards').append($card);
                    siac.aviso('Questão adicionada', 'green');
                    atualizarQuantidadeView();
                }
            }).transition('scale');
        }
    }

    function removerQuestao(codQuestao, tipo) {
        $card = $('#' + codQuestao + '.card');
        var index = _arrayQuestoes.indexOf(codQuestao);
        if (index > -1) {
            _arrayQuestoes.splice(index, 1);
            if (tipo == _OBJ) _qteObjetiva--;
            else if (tipo == _DISC) _qteDiscursiva--;
        }
        $card.transition({
            onHide: function () {
                $card.remove();
                siac.aviso('Questão removida', 'red');
                atualizarQuantidadeView();
            }
        }).transition('scale');
    }

    function mostrarQuestao(codQuestao, _this) {
        $_this = $(_this);
        $_this.addClass('loading');
        $.ajax({
            type: 'POST',
            url: '/dashboard/avaliacao/certificacao/CarregarQuestao/',
            data: {
                codQuestao: codQuestao
            },
            success: function (data) {
                if (_arrayQuestoes.indexOf(codQuestao) > -1) codQuestao = _arrayQuestoes.indexOf(codQuestao) + 1;
                $modal = $('.questao.modal');
                $modal.find('.header').html('Questão ' + codQuestao);
                $modal.find('.segment').html(data);
                $('.accordion').accordion({
                    onChange:function(){
                        $modal.modal('refresh');
                    }
                });
                $modal.modal('show');
            },
            error: function (data) {
                siac.mensagem(data, 'Error');
            },
            complete: function () {
                $_this.removeClass('loading');
                $('.card.anexo.imagem').off().click(function () {
                    expandirModalImagem($(this));
                });
            }
        });
    }

    function prosseguir($_this) {
        $_this.addClass('loading');
        $modal = $('.confirmar.modal');
        $.ajax({
            type: 'POST',
            url: '/dashboard/avaliacao/certificacao/CarregarListaQuestaoDetalhe',
            data: {
                codQuestoes: _arrayQuestoes
            },
            success: function (data) {
                if (data) {
                    $modal.find('.ui.accordion')
                        .html(data)
                        .accordion({
                            onChange:function(){
                                $modal.modal('refresh');
                            }
                        });
                }
            },
            error: function (data) {
                siac.mensagem(data, 'Error');
            },
            complete: function () {
                $_this.removeClass('loading');
                $modal.modal('show');
                $('.card.anexo.imagem').off().click(function () {
                    expandirModalImagem($(this));
                });
            }
        });
    }

    function atualizarQuantidadeView() {
        $disc = $('.quantidade.disc.label');
        $obj = $('.quantidade.obj.label');

        $disc.find('.detail').text(_qteDiscursiva);
        $obj.find('.detail').text(_qteObjetiva);

        {
            if (_qteObjetiva == _qteMaxObjetiva) {
                $obj.addClass('green');
            }
            else $obj.removeClass('green');
        }

        {
            if (_qteDiscursiva == _qteMaxDiscursiva) {
                $disc.addClass('green');
            }
            else $disc.removeClass('green');
        }

    }

    siac.Anexo.iniciar();

    return {
        iniciar: iniciar
    }

})();

siac.Certificacao.Agendar = (function () {
    function iniciar() {
        $('.ui.informacoes.modal')
            .modal()
        ;

        $('.ui.dropdown')
            .dropdown()
        ;

        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal')
            .modal({
                onApprove: function () {
                    $('form').submit();
                }
            })
        ;

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal')
                .modal('show')
            ;
        });

        $('.confirmar.button').click(function () {
            confirmar();
            return false;
        });
    }
    function validar() {
        retorno = true;

        lstErro = $('form .error.message .list');
        lstErro.html('');
        $('form').removeClass('error');

        if (!$('#txtData').val()) {
            lstErro.append('<li>Especifique a data de aplicação</li>');
            retorno = false;
        }

        if (!$('#txtHoraInicio').val()) {
            lstErro.append('<li>Especifique a hora de ínicio</li>');
            retorno = false;
        }

        if ($('#txtData').val() && $('#txtHoraInicio').val()) {
            strDate = $('#txtData').val() + ' ' + $('#txtHoraInicio').val();
            if (!siac.Utilitario.dataEFuturo(strDate)) {
                lstErro.append('<li>Especifique uma data de aplicação futura</li>');
                retorno = false;
            }
        }

        if (!$('#txtHoraTermino').val()) {
            lstErro.append('<li>Especifique a hora de término</li>');
            retorno = false;
        }

        if ($('#txtData').val() && $('#txtHoraInicio').val() && $('#txtHoraTermino').val()) {
            strDateA = $('#txtData').val() + ' ' + $('#txtHoraInicio').val();
            strDateB = $('#txtData').val() + ' ' + $('#txtHoraTermino').val();
            if (siac.Utilitario.compararData(strDateA, strDateB) >= 0) {
                lstErro.append('<li>Especifique uma hora de término maior que a hora de início</li>');
                retorno = false;
            }
        }

        if (!$('#ddlSala').val()) {
            lstErro.append('<li>Selecione uma sala</li>');
            retorno = false;
        }

        return retorno;
    }

    function confirmar() {
        $form = $('form');
        if (validar()) {
            $div = $('<div class="ui form"></div>');
            $div.append($form.html());
            lstInput = $div.find(':input');
            $div.find('.button').remove();
            $div.find('#txtData').attr('value', $form.find('#txtData').val());
            $div.find('#txtHoraInicio').attr('value', $form.find('#txtHoraInicio').val());
            $div.find('#txtHoraTermino').attr('value', $form.find('#txtHoraTermino').val());
            for (var i = 0; i < lstInput.length; i++) {
                lstInput.eq(i)
                    .removeAttr('id')
                    .removeAttr('name')
                    .removeAttr('required')
                    .prop('readonly', true)
                ;
            }
            $('.ui.confirmar.modal .content').html('').append($div);
            $('.ui.confirmar.modal')
                .modal('show')
            ;
        }
        else {
            $form.addClass('error');
        }
    }

    return {
        iniciar: iniciar
    }
})();

siac.Certificacao.Index = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 12, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var categorias = [];
    var disciplina = "";
    var pesquisa = "";

    function iniciar() {
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
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
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        $cards = $('.ui.cards');
        $cards.parent().addClass('loading');
        _controleAjax = $.ajax({
            url: '/Dashboard/Avaliacao/Certificacao/Listar',
            data: {
                pagina: pagina,
                ordenar: ordenar,
                disciplina: disciplina,
                categorias: categorias,
                pesquisa: pesquisa
            },
            method: 'POST',
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

    return {
        iniciar: iniciar
    }
})();

siac.Certificacao.Avaliados = (function () {
    var _controleAjax;
    var _results = [];
    var _content = [];
    var _result;

    function iniciar() {
        $('.ui.modal').modal();
        $('.ui.checkbox').checkbox();
        $('.ui.dropdown').dropdown();

        $('.ui.continuar.modal').modal({ closable: false }).modal('show');

        $('#ddlFiltro').change(function () {
            filtrar($(this).val());
        });

        $('.ui.search')
          .search()
        ;

        $('.selecionar.button').click(function () {
            selecionar();
        });

        $('.cancelar.button').popup({ inline: true, on: 'click' });

        $('.ui.confirmar.modal').modal({
            onApprove: function () {
                salvar();
            }
        });

        $('.informacoes.button').click(function () {
            $('.informacoes.modal').modal('show');
        });

        $('.salvar.button').click(function () {
            if (!_results || _results.length <= 0) {
                siac.aviso('Você ainda não selecionou os avaliados ou grupos.', 'red', 'warning sign');
            }
            else {
                confirmar();
            }
        });
    }

    function filtrar(filtro) {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }

        $buscar = $('.ui.search');

        $buscar.addClass('loading');

        _controleAjax = $.ajax({
            type: 'POST',
            url: '/dashboard/avaliacao/certificacao/filtrar/'+filtro,
            success: function (data) {
                _content = data;
                $buscar
                  .search({
                      source: _content,
                      onSelect: function onSelect(result, response) {
                          _result = result;
                          $('.selecionar.button').removeClass('disabled');
                      },
                      minCharacters: 3                  
                  })
                ;
                $buscar.find('input').val('');
            },
            error: function () {
                siac.mensagem('Um erro ocorreu.');
            },
            complete: function () {
                $('.selecionar.button').addClass('disabled');
                $buscar.removeClass('loading');
            }
        });
    }

    function selecionar() {
        $buscar = $('.ui.search');
        $buscar.find('input').val('');
        if (_result) {
            var flagSelecionado = false;
            for (var i = 0, length = _results.length; i < length; i++) {
                if (_results[i].id == _result.id) {
                    flagSelecionado = true;
                    siac.aviso(_result.title + ' já foi selecionado!');
                }
            }

            if (!flagSelecionado) {
                _results.push(_result);

                $tbody = $('table.selecionados > tbody');
                $tbody
                    .append($('<tr data-selecionado="' + _result.id + '"></tr>')
                        .append($('<td></td>')
                            .text(_result.title)
                        )
                        .append($('<td></td>')
                            .append($('<a class="ui label"></a>')
                                .text(_result.category)
                            )

                        )
                        .append($('<td></td>')
                            .append($('<div class="ui small icon remover button"></div>')
                                .html('<i class="remove icon"></i> Remover')
                            )
                        )
                    )
                ;
            }

            _result = null;
        }

        $('.remover.button').off().click(function () {
            remover($(this));
        });

        $('.selecionar.button').addClass('disabled');
    }

    function remover($this) {
        var $tr = $this.parents('tr');
        var id = $tr.data('selecionado');
        $tr.remove();
        var _tempResults = _results;
        _results = [];
        for (var i = 0, length = _tempResults.length; i < length; i++) {
            if (_tempResults[i].id != id) {
                _results.push(_tempResults[i]);
            }
        }
    }

    function confirmar() {
        var $table = $('table.selecionados').clone();

        $table.removeClass('selecionados');
        $table.find('.button').remove();

        var $model = $('.confirmar.modal');
        $model.find('.content').html($table);

        $model.modal('show');
    }

    function salvar() {
        $('.ui.global.loader').parent().dimmer('show');

        $.ajax({
            type: 'post',
            url: window.location.pathname,
            data: {
                selecao: _results
            },
            success: function (url) {
                window.location.href = url;
            },
            error: function () {
                siac.mensagem('Ocorreu um erro ao tentar conectar com o Servidor.');
                $('.ui.global.loader').parent().dimmer('hide');
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Certificacao.Agendada = (function () {
    var _codAvaliacao, _categoriaUsuario;

    function iniciar() {
        if (!_categoriaUsuario) {
            $elemento = $('[data-categoria]');
            _categoriaUsuario = $elemento.attr('data-categoria');
            $elemento.removeAttr('data-categoria');
        }

        if (!_codAvaliacao) {
            _codAvaliacao = window.location.pathname.match(/cert[0-9]+$/)[0];
        }

        $('.ui.accordion').accordion({
            animateChildren: false
        });

        $('.arquivar.button').click(function () {
            var $_this = $(this);
            $_this.addClass('loading');
            $.ajax({
                url: '/Dashboard/Avaliacao/Certificacao/Arquivar/' + _codAvaliacao,
                type: 'POST',
                success: function (data) {
                    if (data) {
                        window.location.href = '/dashboard/avaliacao/certificacao/detalhe/' + _codAvaliacao;
                    }
                },
                error: function () {
                    $_this.removeClass('loading');
                    siac.mensagem('Ocorreu um erro inesperado na tentativa de arquivar a avaliação.', 'Tente novamente')
                }
            });
        });

        siac.Anexo.iniciar();

        abrirHub();

        contagemRegressiva(1000);
    }

    function contagemRegressiva(intervalo) {
        setTimeout(function () {
            $.ajax({
                type: 'POST',
                url: '/Dashboard/Avaliacao/Certificacao/ContagemRegressiva',
                data: { codAvaliacao: _codAvaliacao },
                success: function (data) {
                    $('#contagem').text(data.Tempo).parent().transition('flash');
                    if (data.Tempo == 'Agora' && data.Intervalo == 0 && data.FlagLiberada == true) {
                        $('.configurar.button').hide();
                        $('.reagendar.button').hide();
                        $('.iniciar.button').removeClass('disabled');
                        $('.acompanhar.button').removeClass('disabled');
                    }
                    else if (data.Tempo == 'Agora' && data.Intervalo == 0 && data.FlagLiberada == false) {
                        $('.configurar.button').show();
                        $('.reagendar.button').show();
                        $('.iniciar.button').addClass('disabled');
                        $('.acompanhar.button').addClass('disabled');
                    }

                    if (data.Intervalo > 0) {
                        contagemRegressiva(data.Intervalo);
                    }
                }
            });
        }, intervalo);
    }

    function abrirHub() {
        var certHub = $.connection.certificacaoHub;
        if (_categoriaUsuario == 1) {
            certHub.client.liberar = function (strCodigo) {
                $.ajax({
                    type: 'POST',
                    url: '/Dashboard/Avaliacao/Certificacao/ContagemRegressiva',
                    data: { codAvaliacao: _codAvaliacao },
                    success: function (data) {
                        alert('O professor liberou a avaliação.');
                        if (data.Tempo == 'Agora' && data.Intervalo == 0 && data.FlagLiberada == true) {
                            $('.iniciar.button').removeClass('disabled').text('Iniciar');
                            $('#mensagem').html('\
                                        <i class="checkmark icon"></i>\
                                        <div class="content">\
                                            Seu professor liberou a prova\
                                            <div class="sub header">Você já pode iniciar</div>\
                                        </div>'
                            );
                        }
                        else if (data.FlagLiberada == true) {
                            $('.iniciar.button').addClass('disabled');
                            $('#mensagem').html('\
                                        <i class="checkmark icon"></i>\
                                        <div class="content">\
                                            Seu professor liberou a prova\
                                            <div class="sub header">Você poderá iniciar assim que chegar a hora de aplicação</div>\
                                        </div>'
                            );
                        }
                    }
                });
            };
            certHub.client.bloquear = function (strCodigo) {
                $.ajax({
                    type: 'POST',
                    url: '/Dashboard/Avaliacao/Certificacao/ContagemRegressiva',
                    data: { codAvaliacao: _codAvaliacao },
                    success: function (data) {
                        if (data.Tempo == 'Agora' && data.Intervalo == 0 && data.FlagLiberada == false) {
                            alert('O professor bloqueou a avaliação.');
                            $('.iniciar.button').addClass('disabled');
                            $('#mensagem').html('\
                                        <i class="clock icon"></i>\
                                        <div class="content">\
                                            Seu professor não liberou a prova ainda\
                                            <div class="sub header">Aguarde a liberação da avaliação para iniciar</div>\
                                        </div>'
                            );
                        }
                    }
                });
            };
            $.connection.hub.start().done(function () {
                certHub.server.realizar(_codAvaliacao);
            });
        }
        else if (_categoriaUsuario == 2) {
            $.connection.hub.start().done(function () {
                $('.liberar.button').click(function () {
                    $('.liberar.button').addClass('loading');
                    $.ajax({
                        url: '/Dashboard/Avaliacao/Certificacao/AlternarLiberar',
                        type: 'POST',
                        data: { codAvaliacao: _codAvaliacao },
                        success: function (data) {
                            if (data == true) {
                                certHub.server.liberar(_codAvaliacao, true);
                                $('.liberar.button').addClass('active').removeClass('loading').text('Liberada');
                                $.ajax({
                                    type: 'GET',
                                    url: '/Dashboard/Avaliacao/Certificacao/ContagemRegressiva',
                                    data: { codAvaliacao: _codAvaliacao },
                                    success: function (data) {
                                        if (data.Tempo == 'Agora' && data.Intervalo == 0 && data.FlagLiberada == true) {
                                            $('.reagendar.button').hide();
                                            $('.configurar.button').hide();
                                            $('.acompanhar.button').removeClass('disabled');
                                        }
                                        else {
                                            $('.configurar.button').show();
                                            $('.reagendar.button').show();
                                            $('.acompanhar.button').addClass('disabled');
                                        }
                                    }
                                });
                            }
                            else {
                                certHub.server.liberar(_codAvaliacao, false);
                                $('.configurar.button').show();
                                $('.reagendar.button').show();
                                $('.liberar.button').removeClass('active').removeClass('loading').text('Liberar');
                            }
                        },
                        error: function () {
                            $('.liberar.button').removeClass('loading');
                            siac.mensagem('Ocorreu um erro inesperado durante o processo. Verifique sua internet e tente novamente.', 'Liberar avaliação');
                        }
                    });
                });
            });
        }
    };

    return {
        iniciar: iniciar
    }
})();