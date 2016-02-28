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
            errorList.append('<li>Selecione o tipo das questões da sua certificação</li>');
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
        var $modal = $('.ui.confirmar.modal');
        var $ddlDisciplina = $('#ddlDisciplina :selected');
        var $ddlTipo = $('#ddlTipo');
        var $table = $modal.find('tbody').html('');

        var $tr = $('<tr></tr>');
        var $tdDisciplina = $('<td></td>').html('<b>' + $ddlDisciplina.text() + '</b>');
        var $tdTemas = $('<td class="ui labels"></td>');

        var $ddlTemas = $('#ddlTemas :selected');
        
        for (var i = 0; i < $ddlTemas.length; i++) {
            $tdTemas.append($('<div class="ui tag label"></div>').text($ddlTemas.eq(i).text()));
        }

        var $tdQteQuestoes = $('<td class="ui labels"></td>');

        if ($ddlTipo.val() == 1 || $ddlTipo.val() == 3) {
            $tdQteQuestoes.append($('<div class="ui label"></div>').html('Objetiva<div class="detail">' + $('#txtQteObjetiva').val() + '</div>'));
        }
        if ($ddlTipo.val() == 2 || $ddlTipo.val() == 3) {
            $tdQteQuestoes.append($('<div class="ui label"></div>').html('Discursiva<div class="detail">' + $('#txtQteDiscursiva').val() + '</div>'));
        }
        var $tdDificuldade = $('<td></td>').text($('#ddlDificuldade :selected').text());

        $table.append($tr.append($tdDisciplina).append($tdTemas).append($tdQteQuestoes).append($tdDificuldade));

        $modal.modal('show');
    }

    function listarTemas(codDisciplina) {
        var disciplinas = $('#ddlDisciplinas');
        var ddlTemas = $('#ddlTemas');

        ddlTemas.parent().addClass('loading');
        $.ajax({
            type: 'POST',
            url: '/tema/recuperartemasporcoddisciplinatemquestao',
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
        _codAvaliacao = window.location.pathname.toLowerCase().match(/cert[0-9]+$/)[0];

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
                    url: '/principal/avaliacao/certificacao/configurar/',
                    data: {
                        codigo: _codAvaliacao,
                        questoes: _arrayQuestoes
                    },
                    success: function (data) {
                        if (data) {
                            window.location.href = "/principal/avaliacao/certificacao/agendar/" + _codAvaliacao;
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
            if (_arrayQuestoes.length == parseInt(_qteMaxDiscursiva) + parseInt(_qteMaxObjetiva)) {
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
            var $_this = $(this);
            var tipo = $_this.parents('.card').find('.label').last().text();
            var id = $_this.parents('.card').attr('id');
            if ($_this.html() == "Adicionar") {
                adicionarQuestao(id, tipo);
            } else if ($_this.html() == "Remover") {
                removerQuestao(id, tipo);
            }
        });

        $('.ui.accordion').accordion({
            animateChildren: false
        });

        atualizarQuantidadeView();

    }

    function carregarQuestoes() {
        var temas = $('#ddlTemas').val();
        var dificuldade = $('#ddlDificuldade').val();
        var tipo = $('#ddlTipo').val();
        if (temas && dificuldade && tipo) {
            var $resultado = $('.resultado .cards');
            $resultado.addClass('form loading')
            $.ajax({
                type: 'POST',
                url: '/principal/avaliacao/certificacao/carregarquestoes/',
                data: {
                    codigo: _codAvaliacao,
                    temas: temas,
                    dificuldade: dificuldade,
                    tipo: tipo
                },
                success: function (data) {
                    if (data) {
                        var $cards = $(data);
                        $resultado.html($cards);

                        $resultado.find('.card').map(function () {
                            var $card = $(this);
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
                        var $_this = $(this);
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
                        mostrarQuestao(codQuestao, this);
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
            var $card = $('#' + codQuestao + '.card');
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
        var $card = $('#' + codQuestao + '.card');
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
        var $_this = $(_this);
        $_this.addClass('loading');
        $.ajax({
            type: 'POST',
            url: '/principal/avaliacao/certificacao/carregarquestao/',
            data: {
                codQuestao: codQuestao
            },
            success: function (data) {
                if (_arrayQuestoes.indexOf(codQuestao) > -1) codQuestao = _arrayQuestoes.indexOf(codQuestao) + 1;
                var $modal = $('.questao.modal');
                $modal.find('.header').html('Questão ' + codQuestao);
                $modal.find('.segment').html(data);
                $('.accordion').accordion({
                    onChange: function () {
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
        var $modal = $('.confirmar.modal');
        $.ajax({
            type: 'POST',
            url: '/principal/avaliacao/certificacao/carregarlistaquestaodetalhe',
            data: {
                codQuestoes: _arrayQuestoes
            },
            success: function (data) {
                if (data) {
                    $modal.find('.ui.accordion')
                        .html(data)
                        .accordion({
                            onChange: function () {
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
        var $disc = $('.quantidade.disc.label');
        var $obj = $('.quantidade.obj.label');

        $disc.find('.detail').text(_qteDiscursiva);
        $obj.find('.detail').text(_qteObjetiva);

        if (_qteObjetiva == _qteMaxObjetiva) {
                $obj.addClass('green');
        }
        else $obj.removeClass('green');

        if (_qteDiscursiva == _qteMaxDiscursiva) {
            $disc.addClass('green');
        }
        else $disc.removeClass('green');
    }

    siac.Anexo.iniciar();

    return {
        iniciar: iniciar
    }

})();

siac.Certificacao.Agendar = (function () {
    var _notificacao;

    function iniciar() {
        $('.ui.informacoes.modal').modal();
        $('#txtData, #txtHoraInicio, #txtHoraTermino').change(function () {
            var $data = $('#txtData');
            var $horaInicio = $('#txtHoraInicio');
            var $horaTermino = $('#txtHoraTermino');
            if ($data.val() && $horaInicio.val() && $horaTermino.val()) {
                var start = siac.Utilitario.formatarData($data.val() + "T" + $horaInicio.val());
                var end = siac.Utilitario.formatarData($data.val() + "T" + $horaTermino.val());
                $.ajax({
                    data: {
                        start: start,
                        end: end
                    },
                    type: 'POST',
                    url: '/principal/agenda/conflitos',
                    success: function (response) {
                        if (response && response.length > 0) {
                            var li = "";
                            for (x = 0, length = response.length; x < length; x++) {
                                li += "<li>" + response[x].title + "</li>";
                            }
                            novaNotificacao = siac.Lembrete.Notificacoes.exibir('<p>Há inconsistência nos agendamentos. Por favor, verifique antes de continuar.</p>' +
                                '<ul>' + li + '</ul>' +
                                '<a class="ui inverted button" href="/principal/agenda" target="_blank">Abrir</a>', 'negativo', 0)
                            if (_notificacao) {
                                _notificacao.dismiss()
                            }
                            _notificacao = novaNotificacao;
                        }
                        else {
                            if (_notificacao) {
                                _notificacao.dismiss()
                            }
                        }
                    }
                });
            }
        });
        $('.ui.dropdown').dropdown();
        $('.ui.accordion').accordion({ animateChildren: false });
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

        $('#txtData, #txtHoraInicio, #txtHoraTermino, #ddlTurma, #ddlSala').change(function () {
            atualizarBotaoConfirmar();
        });
    }

    function validar() {
        var retorno = true;

        var lstErro = $('form .error.message .list');
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
            var strDate = siac.Utilitario.formatarData($('#txtData').val() + 'T' + $('#txtHoraInicio').val());
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
            var strDateA = $('#txtData').val() + 'T' + $('#txtHoraInicio').val();
            var strDateB = $('#txtData').val() + 'T' + $('#txtHoraTermino').val();
            if (siac.Utilitario.compararData(strDateA, strDateB) >= 0) {
                lstErro.append('<li>Especifique uma hora de término maior que a hora de início</li>');
                retorno = false;
            }
        }

        if ($('#txtHoraTermino').val()) {
            var splitedVal = $('#txtHoraTermino').val().split(':');
            var hora = splitedVal[0],
                minuto = splitedVal[1];
            if (hora < 0 || hora > 23 || minuto > 59 || minuto < 0) {
                lstErro.append('<li>Especifique a hora de término válida</li>');
                retorno = false;
            }
        }

        if ($('#txtHoraInicio').val()) {
            var splitedVal = $('#txtHoraInicio').val().split(':');
            var hora = splitedVal[0],
                minuto = splitedVal[1];
            if (hora < 0 || hora > 23 || minuto > 59 || minuto < 0) {
                lstErro.append('<li>Especifique a hora de início válida</li>');
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
        var  $form = $('form');
        if (validar()) {
            var $div = $('<div class="ui form"></div>');
            $div.append($form.html());
            var lstInput = $div.find(':input');
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

    function atualizarBotaoConfirmar() {
        var $btnConfirmar = $('.confirmar.button');
        if (validar()) {
            $btnConfirmar.removeClass('disabled');
        } else {
            $btnConfirmar.addClass('disabled');
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
    var categoria = "";
    var disciplina = "";
    var pesquisa = "";

    function iniciar() {
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() * 0.50) {
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
            categoria = $_this.attr('data-categoria');
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
        $cards.parent().append('<div class="ui active centered inline text loader">Carregando</div>');
        _controleAjax = $.ajax({
            url: '/principal/avaliacao/certificacao/listar',
            data: {
                pagina: pagina,
                ordenar: ordenar,
                disciplina: disciplina,
                categoria: categoria,
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
                    $('.cards .card.hidden').transition({
                        animation: 'pulse',
                        duration: 500,
                        interval: 200
                    });
                }
            },
            complete: function () {
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

        $('.ui.accordion').accordion({
            animateChildren: false
        });

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
            url: '/principal/avaliacao/certificacao/filtrar/' + filtro,
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
        var $buscar = $('.ui.search');
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

                var $tbody = $('table.selecionados > tbody');
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
            var $elemento = $('[data-categoria]');
            _categoriaUsuario = $elemento.attr('data-categoria');
            $elemento.removeAttr('data-categoria');
        }

        if (!_codAvaliacao) {
            _codAvaliacao = window.location.pathname.toLowerCase().match(/cert[0-9]+$/)[0];
        }

        $('.ui.accordion').accordion({
            animateChildren: false
        });

        $('.arquivar.button').click(function () {
            var $_this = $(this);
            $_this.addClass('loading');
            $.ajax({
                url: '/principal/avaliacao/certificacao/arquivar/' + _codAvaliacao,
                type: 'POST',
                success: function (data) {
                    if (data) {
                        window.location.href = '/principal/avaliacao/certificacao/detalhe/' + _codAvaliacao;
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
                url: '/principal/avaliacao/certificacao/contagemregressiva',
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
                    url: '/principal/avaliacao/certificacao/contagemregressiva',
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
                    url: '/principal/avaliacao/certificacao/contagemregressiva',
                    data: { codAvaliacao: _codAvaliacao },
                    success: function (data) {
                        if (data.FlagLiberada == false) {
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
                        url: '/principal/avaliacao/certificacao/alternarliberar',
                        type: 'POST',
                        data: { codAvaliacao: _codAvaliacao },
                        success: function (data) {
                            if (data == true) {
                                certHub.server.liberar(_codAvaliacao, true);
                                $('.liberar.button').addClass('active').removeClass('loading').text('Liberada');
                                $.ajax({
                                    type: 'POST',
                                    url: '/principal/avaliacao/certificacao/contagemregressiva',
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

siac.Certificacao.Realizar = (function () {
    var _codAvaliacao, _matriculaUsuario, _dtTermino;
    var _controleInterval, _controleRestante, _controleNotificacao;

    function iniciar() {
        var $elemento;

        $elemento = $('[data-usuario]');
        _matriculaUsuario = $elemento.attr('data-usuario');
        $elemento.removeAttr('data-usuario');

        $elemento = $('[data-termino]');
        _dtTermino = $elemento.attr('data-termino').toString().toDateObject();
        $elemento.removeAttr('data-termino');;

        _codAvaliacao = window.location.pathname.toLowerCase().match(/cert[0-9]+$/)[0];

        window.onbeforeunload = function () {
            return 'Você está realizando uma avaliação.';
        };

        $('.ui.sticky').sticky();

        setInterval(function () {
            $.ajax({
                type: 'GET',
                url: '/acesso/conectado'
            });
        }, 1000 * 60 * 15);

        $('a[href]').on('click', function () {
            $('.ui.confirmar.modal').modal('show');
            $('.ui.confirmar.modal #txtRef').val($(this).attr('href'));
            return false;
        });

        $('.ui.checkbox')
            .checkbox()
        ;

        $('.ui.informacoes.modal')
            .modal()
        ;

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        $('.ui.confirmar.modal')
            .modal({
                onApprove: function () {
                    desistir($('.ui.confirmar.modal #txtRef').val());
                },
                onDeny: function () {
                    $('.ui.segment.loading').removeClass('loading');
                }
            })
        ;

        $('.ui.accordion')
            .accordion({
                animateChildren: false
            })
        ;

        $('.trigger.button')
            .popup({
                inline: true,
                on: 'click'
            })
        ;

        $('.ui.gabarito.modal')
            .modal({
                onApprove: function () {
                    $.ajax({
                        type: 'GET',
                        url: "/acesso/conectado",
                        success: function () {
                            finalizar();
                            $(this).modal('hide');
                        },
                        error: function () {
                            siac.mensagem('Conecte-se à internet antes de confirmar.');
                        }
                    });
                    return false;
                }
            })
        ;

        siac.Anexo.iniciar();

        $('.desistir.button').click(function () {
            desistir();
        });

        var date = new Date();
        $('#lblHoraInicio').text(("0" + (date.getHours())).slice(-2) + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min');
        $('#lblHoraTermino').text(("0" + (_dtTermino.getHours())).slice(-2) + 'h' + ("0" + (_dtTermino.getMinutes())).slice(-2) + 'min');

        relogio();
        temporizador();
        conectarHub(_codAvaliacao, _matriculaUsuario);
    }

    function relogio() {
        setInterval(function () {
            date = new Date();
            $('#lblHoraAgora').text(("0" + (date.getHours())).slice(-2) + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min');
        }, 1000);
    }

    function temporizador() {
        setInterval(function () {
            var offset = _dtTermino.getTimezoneOffset() * 60 * 1000;
            var timeRestante = (_dtTermino.getTime() + offset) - (new Date().getTime() + offset);

            if (timeRestante > 0) {
                var date = new Date();
                date.setTime(timeRestante);
                var offsetDate = date.getTimezoneOffset() * 60 * 1000;
                date.setTime(date.getTime() + offsetDate);
                var txtRestante = ("0" + date.getHours()).slice(-2) + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min';
                $('#lblHoraRestante').text(txtRestante);
                if (txtRestante != _controleRestante) {
                    $('#lblHoraRestante').parent().transition('flash');
                }
                _controleRestante = txtRestante;
                if (timeRestante < 1000 * 60 * 5 && !$('#lblHoraRestante').parent().hasClass('red')) {
                    $('#lblHoraRestante').parent().addClass('red');
                }
            }
            else {
                alert('O tempo de aplicação acabou, sua prova será enviada automaticamente.');
                $('.ui.global.loader').parent().dimmer('show');
                finalizar();
            }
        }, 1000);
    }

    function finalizar() {
        window.onbeforeunload = function () {
            $('.ui.global.loader').parent().dimmer('show');
        };
        $('form').submit();
    }

    function verificar() {
        var $Objects = $('textarea[name^="txtResposta"], input[name^="rdoResposta"]');
        var retorno = true;
        for (var i = 0, length = $Objects.length; i < length; i++) {
            var _this = $Objects.eq(i);
            var $label = _this.closest('.questao').find('.ui.label');
            if (_this.attr('name').indexOf('rdo') > -1) {
                if ($('input[name="' + _this.attr('name') + '"]:checked').length === 0) {
                    $label.removeAttr('style').addClass('red').html('Não respondida').transition('tada');
                    retorno = false;
                }
            }
            else if (!_this.val()) {
                $label.removeAttr('style').addClass('red').html('Não respondida').transition('tada');
                retorno = false;
            }
        }
        return retorno;
    }

    function confirmar() {
        var $modal = $('.ui.gabarito.modal');
        var $basicSegment = $('form .ui.basic.segment').clone();

        $basicSegment.removeAttr('style');
        $modal.find('.content').html($('<div class="ui form"></div>').append($basicSegment));
        $modalBasicSegment = $modal.find('.ui.basic.segment');
        $modal.find('.ui.accordion').accordion({
            onChange: function () {
                $('.ui.gabarito.modal').modal('refresh');
            },
            animateChildren: false
        });

        var $lstOriginalTextarea = $('form .ui.basic.segment').find('textarea');
        var $lstCloneTextarea = $modalBasicSegment.find('textarea');
        for (var i = 0; i < $lstOriginalTextarea.length; i++) {
            $lstCloneTextarea.eq(i).val($lstOriginalTextarea.eq(i).val());
        }
        var $lstInput = $modalBasicSegment.find(':input,a');
        for (var i = 0; i < $lstInput.length; i++) {
            $lstInput.eq(i)
                .attr({
                    'readonly': 'readonly'
                })
                .removeAttr('id name href onchange onclick')
                .off()
            ;
        }
        $modal.modal('show');
    }

    function desistir(url) {
        $('.ui.global.loader').parent().dimmer('show');
        $.ajax({
            url: '/principal/avaliacao/certificacao/desistir/' + _codAvaliacao,
            type: 'POST',
            success: function () {
                window.onbeforeunload = function () {
                    $('.ui.global.loader').parent().dimmer('show');
                };
                if (!url) {
                    url = '/principal';
                }
                window.location.href = url;
            },
            error: function () {
                $('.ui.global.loader').parent().dimmer('hide');
                siac.mensagem('Ocorreu um erro na tentativa de desistência');
            }
        });
    }

    function conectarHub(codAval, usrMatr) {
        var certHub = $.connection.certificacaoHub;
        $.connection.hub.start().done(function () {
            certHub.server.avaliadoConectou(codAval, usrMatr);

            finalizar = function () {
                window.onbeforeunload = function () {
                    $('.ui.global.loader').parent().dimmer('show');
                };
                certHub.server.avaliadoFinalizou(codAval, usrMatr);
                $('form').submit();
            };

            $('.ui.continuar.modal')
                .modal({
                    onApprove: function () {
                        certHub.server.avaliadoVerificando(codAval, usrMatr);
                        confirmar();
                    },
                    onDeny: function () {
                        $('html, body').animate({
                            scrollTop: $(".questao .label.red").offset().top
                        }, 1000);
                    }
                })
            ;

            $('.finalizar.button').click(function () {
                if (verificar()) {
                    certHub.server.avaliadoVerificando(codAval, usrMatr);
                    confirmar();
                }
                else {
                    $('.continuar.modal').modal('show');
                }
            });

            var enviarMsg = function enviarMsg(_this) {
                var $msg = $('#txtChatMensagem');
                $msg.val($msg.val().trim())
                if ($msg.val()) {
                    var mensagem = $msg.val().quebrarLinhaEm(30);
                    certHub.server.chatAvaliadoEnvia(codAval, usrMatr, $msg.val());
                    $('.chat.popup .content .comments').append('\
                            <div class="comment" style="float:right;clear:both;">\
                                <div class="content">\
                                <div class="ui right pointing label">\
                                    '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');
                    $msg.val('');
                    var $comments = $('.chat.popup .content .comments');
                    $($comments).scrollTop($comments.children().last().offset().top);
                }
            };
            $('.enviar.icon').on('click', function () { enviarMsg(this) });
            $('#txtChatMensagem').keypress(function (e) {
                if (e.which == 13) {
                    enviarMsg(this);
                    return false;
                }
            });

            $('textarea[name^="txtResposta"], input[name^="rdoResposta"]').change(function () {
                var $label = $(this).closest('.questao').find('.ui.label');
                if ($(this).val()) {
                    $label.removeClass('red');
                    $label.removeAttr('style');
                    $label.html('Respondida');
                    var questao;
                    if ($(this).attr('name').indexOf('rdo') > -1) {
                        questao = $(this).attr('name').split('rdoResposta')[1];
                        $label.find('.detail').remove();
                        $label.append($('<div class="detail"></div>').text($('input[name="' + $(this).attr('name') + '"]:checked').next().find('b').text()));
                    }
                    else {
                        questao = $(this).attr('name').split('txtResposta')[1];
                    }
                    certHub.server.responderQuestao(codAval, usrMatr, questao, true);
                }
                else {
                    var questao;
                    if ($(this).attr('name').indexOf('rdo') > -1) {
                        questao = $(this).attr('name').split('rdoResposta')[1];
                    }
                    else {
                        questao = $(this).attr('name').split('txtResposta')[1];
                    }
                    certHub.server.responderQuestao(codAval, usrMatr, questao, false);
                    $label.attr('style', 'display:none');
                }
            });

            $(window).on("blur focus", function (e) {
                var prevType = $(this).data("prevType");

                if (prevType != e.type) {
                    switch (e.type) {
                        case "blur":
                            certHub.server.focoAvaliacao(codAval, usrMatr, false);
                            break;
                        case "focus":
                            certHub.server.focoAvaliacao(codAval, usrMatr, true);
                            break;
                    }
                }

                $(this).data("prevType", e.type);
            })
        });

        certHub.client.prorrogar = function (duracao) {
            _dtTermino = new Date(_dtTermino.getTime() + duracao * 60000);
            $('#lblHoraTermino').text(("0" + (_dtTermino.getHours())).slice(-2) + 'h' + ("0" + (_dtTermino.getMinutes())).slice(-2) + 'min');
            siac.Lembrete.Notificacoes.exibir('O professor prorrogou a duração em ' + duracao + ' minutos', 'info');
        }

        certHub.client.alertar = function (mensagem) {
            var timeAntes = new Date();
            alert(mensagem);
            var timeDepois = new Date();
            if ((timeDepois - timeAntes) < 350) {
                siac.mensagem(mensagem, 'O professor disse...');
            }
        }

        certHub.client.enviarAval = function (codAvaliacao) {
            html2canvas(document.body, {
                onrendered: function (canvas) {
                    var c = $(canvas);
                    c.attr('id', 'mycanvas').hide();
                    $('body').append(c);
                    var canvas = document.getElementById("mycanvas");
                    data = canvas.toDataURL("image/png");
                    $.ajax({
                        type: 'POST',
                        url: '/principal/avaliacao/certificacao/printar',
                        data: {
                            codAvaliacao: codAval,
                            imageData: data
                        },
                        success: function () {
                            certHub.server.avalEnviada(codAvaliacao, usrMatr);
                        },
                        error: function () {
                            // adicionar erro ao Feed do professor
                        }
                    });

                    c.remove();
                }
            });
        };

        var chatQteMensagem = 0;
        certHub.client.chatAvaliadoRecebe = function (mensagem) {
            if (mensagem) {
                mensagem = mensagem.quebrarLinhaEm(30);
                $('.chat.popup .comments').append('\
                            <div class="comment" style="float:left;clear:both;">\
                                <div class="content">\
                                <div class="ui left pointing label">\
                                    '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');
                var $comments = $('.chat.popup .comments');
                $($comments).animate({
                    scrollTop: $comments.children().last().offset().top
                }, 0);

                if (!$('.chat.popup').hasClass('visible')) {
                    var $btnChat = $('.icon.chat.button');
                    var $lblQteMsg = $('#lblQteMsg');
                    $lblQteMsg.remove();
                    chatQteMensagem++;
                    $btnChat.addClass('blue').html('<i class="icon comments outline"></i> ' + chatQteMensagem);
                    document.title = 'Você recebeu uma mensagem - SIAC';
                    if (chatQteMensagem > 1) {
                        document.title = 'Você recebeu ' + chatQteMensagem + ' mensagens - SIAC';
                    }
                    if (_controleNotificacao)
                        _controleNotificacao.dismiss()
                    _controleNotificacao = siac.Lembrete.Notificacoes.exibir('Você recebeu uma mensagem');
                }
            }
        };
        $('.icon.chat.button').on('click', function () {
            chatQteMensagem = 0;
            $('#lblQteMsg').remove();
            $(this).removeClass('blue').html('<i class="icon comments outline"></i>');
            document.title = 'Realizar ' + _codAvaliacao;
        });

    }

    return {
        iniciar: iniciar
    }
})();

siac.Certificacao.Acompanhar = (function () {
    var _codAvaliacao, _matriculaUsuario, _dtTermino;

    function iniciar() {
        var $elemento = $('[data-usuario]');
        _codAvaliacao = window.location.pathname.toLowerCase().match(/cert[0-9]+$/)[0];
        _matriculaUsuario = $elemento.attr('data-usuario');
        $elemento.removeAttr('data-usuario');

        $elemento = $('[data-termino]');
        _dtTermino = $elemento.attr('data-termino').toString().toDateObject();
        $elemento.removeAttr('data-termino');
        
        conectarHub(_codAvaliacao, _matriculaUsuario);

        $('.ui.accordion')
            .accordion({
                animateChildren: false
            })
        ;

        $('.ui.progress')
            .progress({
                label: 'ratio',
                text: {
                    ratio: '{value} de {total}'
                }
            })
        ;

        $('.trigger.button')
            .popup({
                inline: true,
                on: 'click'
            })
        ;

        $('.modal').modal();
    }

    function conectarHub(codAval, usrMatr) {
        var certHub = $.connection.certificacaoHub;
        $.connection.hub.start().done(function () {

            var enviarMsg = function enviarMsg(_this) {
                var $content = $(_this).parents('.content[id]');
                var matr = $content.attr('id');
                var $msg = $('#' + matr + 'msg');
                $msg.val($msg.val().trim());
                if ($msg.val()) {
                    var mensagem = $msg.val().quebrarLinhaEm(30);
                    certHub.server.chatProfessorEnvia(codAval, matr, $msg.val());
                    $('#' + matr + '.content .chat.popup .comments').append('\
                            <div class="comment" style="float:right;clear:both;">\
                                <div class="content">\
                                <div class="ui right pointing label">\
                                    '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');
                    $msg.val('');
                    var $comments = $content.find('.comments');
                    $($comments).animate({
                        scrollTop: $comments.children().last().offset().top
                    }, 0);
                }
            };

            prorrogar = function prorrogar(duracao, observacao) {
                if (duracao > 0) {
                    certHub.server.prorrogar(codAval, duracao, observacao);
                }
            }

            $('.prorrogar.button').click(function () {
                $('.prorrogar.modal').modal('show');
            });
            $('.prorrogar.modal').modal({
                onApprove: function () {
                    var duracao = $('#txtProrrogarDuracao').val();
                    var observacao = $('#txtProrrogarObservacao').val();
                    if (duracao > 0) {
                        prorrogar(duracao, observacao);
                        _dtTermino = new Date(_dtTermino.getTime() + duracao * 60000);
                        $('#lblHoraTermino').text(("0" + (_dtTermino.getHours())).slice(-2) + 'h' + ("0" + (_dtTermino.getMinutes())).slice(-2) + 'min');
                        $('#txtProrrogarDuracao').val('');
                        $('#txtProrrogarObservacao').val('');
                    }
                    else {
                        siac.mensagem('Insira uma quantidade válida de minutos para a prorrogação.', 'Prorrogar Avaliação');
                    }
                }
            });

            certHub.server.professorConectou(codAval, usrMatr);

            $('.prova.button').on('click', function () {
                var matr = $(this).parent().attr('id');
                certHub.server.requererAval(codAval, matr);
                $('#' + matr + '.content .prova.button').addClass('loading').removeClass('transition visible');
            });

            $('.alertar.button').on('click', function () {
                var mensagem = $(this).prev().find('textarea').val();
                var matr = $(this).parent().parent().parent().attr('id');
                certHub.server.alertar(codAval, mensagem, matr);
                $(this).prev().find('textarea').val('');
                $(this).popup('hide all');
            });

            $('.enviar.icon').on('click', function () { enviarMsg(this) });
            $('.chat input').keypress(function (e) {
                if (e.which == 13) {
                    enviarMsg(this);
                    return false;
                }
            });


            setInterval(function () {
                lstMatr = $('.accordion')
                            .find('.title')
                            .map(function () {
                                certHub.server.feed(codAval, this.id);
                            });
            }, 3000);
        });

        certHub.client.chatProfessorRecebe = function (usrMatricula, mensagem) {
            var $content = $('#' + usrMatricula + '.content[id]');
            if (mensagem) {
                mensagem = mensagem.quebrarLinhaEm(30);
                $('#' + usrMatricula + '.content .chat.popup .comments').append('\
                            <div class="comment" style="float:left;clear:both;">\
                                <div class="content">\
                                <div class="ui left pointing label">\
                                    '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');
                var $comments = $content.find('.comments');
                $($comments).animate({
                    scrollTop: $comments.children().last().offset().top
                }, 0);

                chatQteMensagem = 0;
                if ($('#' + usrMatricula + 'lblQteMsg').length) { chatQteMensagem = $('#' + usrMatricula + 'lblQteMsg').data('qte') }
                if (!$('#' + usrMatricula + '.content .chat.popup').hasClass('visible')) {
                    var $accordionChat = $('#' + usrMatricula + '.title');
                    var $lblQteMsg = $('#' + usrMatricula + 'lblQteMsg');
                    $lblQteMsg.remove();
                    $accordionChat.append($('<div></div>').attr('id', usrMatricula + 'lblQteMsg').addClass('ui small blue label').append('<i class="ui comments icon"></i>'));
                    chatQteMensagem++;
                    $('#' + usrMatricula + 'lblQteMsg').data('qte', chatQteMensagem).append(chatQteMensagem);
                }
            }
        };

        $('.icon.chat.button').on('click', function () {
            var matr = ($(this).parents('.content[id]').attr('id'));
            $('#' + matr + 'lblQteMsg').remove();
        });

        certHub.client.atualizarFeed = function (alnMatricula, lstEvento) {
            var $feed = $('#' + alnMatricula + '.content .feed');
            $feed.html('');
            $feed.append('<h4 class="ui header">Atividade</h4>');
            if (lstEvento) {
                for (var i = lstEvento.length - 1; i > -1; i--) {
                    $feed.append('\
                        <div class="event">\
                            <div class="label">\
                                <i class="'+ lstEvento[i].Icone + ' icon"></i>\
                            </div>\
                            <div class="content">\
                                <div class="summary">\
                                    ' + lstEvento[i].Descricao + '\
                                    <div class="date" title="'+ lstEvento[i].DataCompleta + '">' + lstEvento[i].Data + '</div>\
                                </div>\
                            </div>\
                        </div>\
                    ');
                }

                $('#' + alnMatricula + 'lblInfo').remove();
                if (lstEvento[lstEvento.length - 1].Icone == "red warning sign" && !$('#' + alnMatricula + 'lblWarning').length && !$('#' + alnMatricula).hasClass('active')) {
                    $accordionChat = $('#' + alnMatricula + '.title');
                    $accordionChat.append($('<i></i>').attr('id', alnMatricula + 'lblWarning').addClass('red warning sign icon'));
                }

                else if (!$('#' + alnMatricula + 'lblWarning').length) {
                    var $accordionChat = $('#' + alnMatricula + '.title');
                    $accordionChat.append($('<i></i>').attr('id', alnMatricula + 'lblInfo').addClass(lstEvento[lstEvento.length - 1].Icone + ' icon'));
                }
            }
        }

        $('.accordion .title').on('click', function () {
            var matr = ($(this).attr('id'));
            $('#' + matr + 'lblWarning').remove();
        });

        certHub.client.conectarAvaliado = function (usrMatricula) {
            $('#' + usrMatricula + '.title .small.label').remove();
            $('#' + usrMatricula + '.title .status.label').removeClass('red').addClass('green');
            $('#' + usrMatricula + '.content .button').removeClass('disabled');
        };

        certHub.client.desconectarAvaliado = function (usrMatricula) {
            $('#' + usrMatricula + '.title').append('<div class="ui small label">Desconectado</div>');
            $('#' + usrMatricula + '.title .status.label').removeClass('green');
            $('#' + usrMatricula + '.content .button').addClass('disabled');
        };

        certHub.client.avaliadoFinalizou = function (usrMatricula) {
            $('#' + usrMatricula + '.title').append('<div class="ui small label">Finalizou</div>');
            $('#' + usrMatricula + '.title .status.label').removeClass('green').addClass('red');
            $('#' + usrMatricula + '.content .button').addClass('disabled');
        };

        certHub.client.receberAval = function (alnMatricula) {
            $.ajax({
                type: 'POST',
                url: '/principal/avaliacao/certificacao/printar',
                data: {
                    codAvaliacao: codAval
                },
                success: function (data) {
                    $('.printscreen.modal .header').text($('#' + alnMatricula + ' .nome').text() + ' (' + alnMatricula + ')');
                    $('.printscreen.modal .content').css({
                        'background-image': 'url(\'' + data + '\')'
                    });
                    $('.printscreen.modal .nova.guia.button').attr('href', data);
                    $('.printscreen.modal').modal('show').modal('refresh');
                    $('#' + alnMatricula + '.content .prova.button').removeClass('loading');
                },
                error: function () {
                    siac.mensagem('Ocorreu um erro inesperado. Tente novamente mais tarde.');
                }
            });
        };

        certHub.client.atualizarProgresso = function (alnMatricula, value) {
            if (value > 0) {
                $('#' + alnMatricula + '.content .progress')
                    .progress({
                        value: value,
                        label: 'ratio',
                        text: {
                            ratio: '{value} de {total}'
                        }
                    })
                ;
            }
        }

        certHub.client.respondeuQuestao = function (alnMatricula, questao, flag) {
            var $questao = $('#' + alnMatricula + '.content [data-questao="' + questao + '"]');
            $questao.removeClass('positive').find('i').remove();
            if (flag) {
                $questao.addClass('green');
            }
        }

        certHub.client.listarChat = function (alnMatricula, mensagens) {
            for (var i = 0, length = mensagens.length; i < length; i++) {
                if (mensagens[i].FlagAutor) {
                    certHub.client.chatProfessorRecebe(alnMatricula, mensagens[i].Texto);
                }
                else {
                    var mensagem = mensagens[i].Texto.quebrarLinhaEm(30);
                    $('#' + alnMatricula + '.content .chat.popup .comments').append('\
                        <div class="comment" style="float:right;clear:both;">\
                            <div class="content">\
                            <div class="ui right pointing label">\
                                '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');

                    var $comments = $('#' + alnMatricula + '.content .comments');
                    $($comments).scrollTop($comments.children().last().offset().top);
                }
            }
        }
    }

    return {
        iniciar: iniciar
    }
})();

siac.Certificacao.Resultado = (function () {
    function iniciar() {
        $('.ui.accordion').accordion({ animateChildren: false });
        $('[data-content], [data-html]').popup();
        siac.Anexo.iniciar();
    }

    return {
        iniciar: iniciar
    }
})();

siac.Certificacao.Corrigir = (function () {
    var _dicQuestoes = {};
    var _codAvaliacao;

    function iniciar() {
        _codAvaliacao = window.location.pathname.toLowerCase().match(/cert[0-9]+$/)[0];

        var $elementoJson = $('code.questoes');
        _dicQuestoes = JSON.parse($elementoJson.html());
        $elementoJson.remove();

        $('.ui.accordion').accordion({ animateChildren: false });
        $('.ui.modal').modal();
        $('.ui.dropdown').dropdown();

        $('.ui.button.informacoes').click(function () {
            $('.modal.informacoes').modal('show');
        });

        $('.ui.button.corrigir').click(function () {
            $('.modal.corrigir').modal('show');
        });

        $('.ui.dimmer').css('z-index', 1);

        $('#ddlCorrecaoModo').change(function () {
            var modo = $(this).val();
            var $ddlCorrecaoValor = $('#ddlCorrecaoValor');
            $ddlCorrecaoValor.parent().addClass('loading');
            $('.correcao.conteudo').html('');
            $ddlCorrecaoValor.dropdown('set placeholder text', 'Selecione...');

            if (modo == 'aluno') {
                $.ajax({
                    cache: false,
                    type: 'POST',
                    url: '/principal/avaliacao/certificacao/carregaralunos/' + _codAvaliacao,
                    success: function (data) {
                        $ddlCorrecaoValor.html('<option value="">Selecione o avaliado</option>');
                        $ddlCorrecaoValor.parents('.field').find('label').text('Selecione o avaliado');
                        for (i = 0, length = data.length; i < length; i++) {
                            if (data[i].FlagCorrecaoPendente) {
                                $ddlCorrecaoValor.append('<option value="' + data[i].Matricula + '">' + data[i].Nome + '</option>');
                            }
                            else {
                                $ddlCorrecaoValor.append('<option value="' + data[i].Matricula + '">' + data[i].Nome + ' (corrigido)</option>');
                            }
                        }
                        $ddlCorrecaoValor.parent().removeClass('loading').removeClass('disabled');
                        $ddlCorrecaoValor.dropdown('set selected', -1);
                    },
                    error: function () {
                        siac.mensagem('Ocorreu um erro.');
                        $ddlCorrecaoValor.parent().removeClass('loading');
                    }
                });
            }
            else if (modo == 'questao') {
                $.ajax({
                    cache: false,
                    type: 'POST',
                    url: '/principal/avaliacao/certificacao/carregarquestoesdiscursivas/' + _codAvaliacao,
                    success: function (data) {
                        $ddlCorrecaoValor.parents('.field').find('label').text('Selecione a questão');
                        $ddlCorrecaoValor.html('');
                        $ddlCorrecaoValor.append('<option value="">Selecione a questão</option>');
                        for (i = 0, length = data.length; i < length; i++) {
                            if (data[i].flagCorrecaoPendente) {
                                $ddlCorrecaoValor.append('<option value="' + data[i].codQuestao + '">' + getIndiceQuestao(data[i].codQuestao) + '. ' + data[i].questaoEnunciado.encurtarTextoEm(80) + '</option>');
                            }
                            else {
                                $ddlCorrecaoValor.append('<option value="' + data[i].codQuestao + '">' + getIndiceQuestao(data[i].codQuestao) + '. ' + data[i].questaoEnunciado.encurtarTextoEm(80) + ' (corrigida)</option>');
                            }
                        }
                        $ddlCorrecaoValor.parent().removeClass('loading').removeClass('disabled');
                    },
                    error: function () {
                        siac.mensagem('Ocorreu um erro.');
                        $ddlCorrecaoValor.parent().removeClass('loading');
                    }
                });
            }
        });

        $('#ddlCorrecaoValor').change(function () {
            var _this = this;
            var $modal = $(_this).parents('.modal')
            var modo = $('#ddlCorrecaoModo').val();
            var valor = $(_this).val();
            if (valor) {
                var $conteudo = $('.correcao.conteudo');
                $('.modal.corrigir form').addClass('loading');
                $(_this).parent().addClass('loading');
                if (modo == 'aluno') {
                    var $conteudoQuestao = $('#templateCorrecaoAluno');
                    $.ajax({
                        type: 'POST',
                        url: '/principal/avaliacao/certificacao/carregarrespostasdiscursivas/' + _codAvaliacao,
                        data: {
                            matrAluno: valor
                        },
                        success: function (data) {
                            $('.correcao.conteudo').html('');
                            for (i = 0, length = data.length; i < length; i++) {
                                var $conteudoQuestaoClone = $conteudoQuestao.clone();
                                $conteudoQuestaoClone.removeAttr('id').removeAttr('hidden');
                                $conteudoQuestaoClone.html($conteudoQuestao.html());

                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{matrAluno}', valor));
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{codQuestao}', data[i].codQuestao));
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{questaoEnunciado}', data[i].questaoEnunciado));
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{questaoIndice}', getIndiceQuestao(data[i].codQuestao)));
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{questaoChaveResposta}', data[i].questaoChaveResposta));
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{alunoResposta}', data[i].alunoResposta));
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{correcaoComentario}', data[i].correcaoComentario));

                                $conteudo.append($conteudoQuestaoClone);

                                if (data[i].flagCorrigida) {
                                    $conteudo.append($conteudoQuestaoClone);
                                    $conteudoQuestaoClone.find('.dimmer').dimmer('show');
                                    $conteudoQuestaoClone.find('.notaObtida').val(data[i].notaObtida);
                                }
                                else {
                                    $conteudo.prepend($conteudoQuestaoClone);
                                }
                            }

                            $modal.modal('refresh');
                            $(_this).parent().removeClass('loading');

                            $('.button.corrigir.aluno').click(function () {
                                corrigirQuestao(this);
                            });
                        },
                        error: function () {
                            siac.mensagem('Ocorreu um erro.');
                            $(_this).parent().removeClass('loading');
                        },
                        complete: function () {
                            $('.modal.corrigir form').removeClass('loading');
                        }

                    });
                }
                else if (modo == 'questao') {
                    $conteudoQuestao = $('#templateCorrecaoQuestao');
                    $.ajax({
                        type: 'POST',
                        url: '/principal/avaliacao/certificacao/carregarrespostasporquestao/' + _codAvaliacao,
                        data: {
                            codQuestao: valor
                        },
                        success: function (data) {
                            $('.correcao.conteudo').html('');
                            if (data) {
                                var $conteudoQuestaoClone = $conteudoQuestao.clone().removeAttr('id').removeAttr('hidden');;
                                $conteudoQuestaoClone.find('table').remove();
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{questaoEnunciado}', data[0].questaoEnunciado));
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{questaoIndice}', getIndiceQuestao(data[0].codQuestao)));
                                $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{questaoChaveResposta}', data[0].questaoChaveResposta));
                                var $conteudoQuestaoTemp = $conteudoQuestaoClone;

                                for (i = 0, length = data.length; i < length; i++) {
                                    $conteudoQuestaoClone = $conteudoQuestao.clone();
                                    $conteudoQuestaoClone.removeAttr('id').removeAttr('hidden');
                                    $conteudoQuestaoClone.html($conteudoQuestaoClone.find('table').parent()).attr('id', 'aln' + data[i].alunoMatricula);

                                    $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{matrAluno}', valor));
                                    $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{alunoNome}', data[i].alunoNome));
                                    $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{codQuestao}', data[i].codQuestao));
                                    $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{alunoResposta}', data[i].alunoResposta));
                                    $conteudoQuestaoClone.html($conteudoQuestaoClone.html().substituirTodos('{correcaoComentario}', data[i].correcaoComentario));

                                    if (data[i].flagCorrigida) {
                                        $conteudo.append($conteudoQuestaoClone);
                                        $conteudoQuestaoClone.find('.dimmer').dimmer('show');
                                        $conteudoQuestaoClone.find('.notaObtida').val(data[i].notaObtida);
                                    }
                                    else {
                                        $conteudo.prepend($conteudoQuestaoClone);
                                    }
                                }

                                $conteudo.prepend($conteudoQuestaoTemp);

                                $modal.modal('refresh');
                                $(_this).parent().removeClass('loading');

                                $('.button.corrigir.aluno').click(function () {
                                    corrigirQuestao(this);
                                });
                            }
                        },
                        error: function () {
                            siac.mensagem('Ocorreu um erro.');
                            $(_this).parent().removeClass('loading');
                        },
                        complete: function () {
                            $('.modal.corrigir form').removeClass('loading');
                        }
                    });
                }
            }
        });

        $('.label[data-questao]').click(function () {
            var $this = $(this);
            var codQuestao = $this.data('questao');
            var matr = $this.parents('[id]').attr('id');

            $('.modal.corrigir').modal('show');
            $('.modal.corrigir form').addClass('loading');
            $('#ddlCorrecaoModo').dropdown('set selected', 'aluno').change();
            window.setTimeout(function () {
                $('#ddlCorrecaoValor').dropdown('set selected', matr);
            }, 1000);
        });
    }

    function getIndiceQuestao(codQuestao) {
        return _dicQuestoes[codQuestao];
    }

    function corrigirQuestao(_this) {
        var modo = $('#ddlCorrecaoModo').val();
        var matrAluno = $('#ddlCorrecaoValor').val();
        var codQuestao = $(_this).parents('[id]').attr('id');
        var id = codQuestao;

        if (modo == "questao") {
            matrAluno = $(_this).parents('[id]').attr('id');;
            codQuestao = $('#ddlCorrecaoValor').val();
            id = matrAluno;
            matrAluno = matrAluno.split('aln')[1];
        }

        var notaObtida = $('#' + id + ' .notaObtida').val();
        var correcaoComentario = $('#' + id + ' .correcaoComentario').val();
        $('#' + id + ' .button.corrigir.aluno').addClass('loading');

        $.ajax({
            type: 'POST',
            url: '/principal/avaliacao/certificacao/corrigirquestaoaluno/' + _codAvaliacao,
            data: {
                matrAluno: matrAluno,
                codQuestao: codQuestao,
                notaObtida: notaObtida,
                profObservacao: correcaoComentario
            },
            success: function (data) {
                $('#' + id + ' .button.corrigir.aluno').removeClass('loading');
                $('#' + id).find('.segment').dimmer('show');
            },
            error: function (data) {
                siac.mensagem(data);
                $('#' + id + ' .button.corrigir.aluno').removeClass('loading');
            },
            complete: function () {
                var questoesQteTotal = $('.corrigir .correcao.conteudo .dimmer').length;
                var questoesQteCorrigidas = $('.corrigir .correcao.conteudo .dimmer.active').length;

                if (modo == "aluno") {
                    if (questoesQteCorrigidas == questoesQteTotal - 1) {
                        var itemAtual = $('#ddlCorrecaoValor').dropdown('get text')[0];
                        if (itemAtual.indexOf('(corrigido)') < 0) {
                            $('#ddlCorrecaoValor').dropdown('set text', itemAtual + ' (corrigido)');
                            $('#ddlCorrecaoValor option[value="' + matrAluno + '"]').text(itemAtual + ' (corrigido)');
                        }
                    }
                }
                else if (modo == "questao") {
                    if (questoesQteCorrigidas == questoesQteTotal - 2) {
                        var itemAtual = $('#ddlCorrecaoValor').dropdown('get text')[0];
                        if (itemAtual.indexOf('(corrigida)') < 0) {
                            $('#ddlCorrecaoValor').dropdown('set text', itemAtual + ' (corrigida)');
                            $('#ddlCorrecaoValor option[value="' + codQuestao + '"]').text(itemAtual + ' (corrigida)');
                        }
                    }
                }
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Certificacao.Detalhe = (function () {
    var _codAvaliacao, _controleAjax;

    function iniciar() {

        _codAvaliacao = window.location.pathname.toLowerCase().match(/cert[0-9]+$/)[0];

        $('.ui.accordion').accordion({
            animateChildren: false,
            onOpen: function () {
                var $content = $('.questao.content.active');
                var $canvas = $content.find('canvas').get(0);
                if ($content && $canvas) {
                    var ctx = $canvas.getContext("2d");
                    var data = JSON.parse($content.find('code.dados').html());
                    var chart = new Chart(ctx).Doughnut(data);
                }
            }
        });

        $('.ui.dropdown').dropdown();

        $('.arquivar.button').click(function () {
            var $_this = $(this);
            $_this.addClass('loading');
            $.ajax({
                url: '/principal/avaliacao/certificacao/arquivar/' + _codAvaliacao,
                type: 'POST',
                success: function (data) {
                    window.location.reload();
                    if (data) {
                        $_this.addClass('active').text('Arquivada');
                    }
                    else {
                        $_this.removeClass('active').text('Arquivar');
                    }
                },
                error: function () {
                    siac.mensagem('Ocorreu um erro inesperado na tentativa de arquivar a avaliação.', 'Tente novamente')
                },
                complete: function () {
                    $_this.removeClass('loading');
                }
            });
        });

        $('[data-content], [data-html]').popup();

        $('.aluno.dropdown').change(function () {
            var $this = $(this);
            var codPessoa = $this.find(':selected').val();

            if (_controleAjax && _controleAjax.readyState != 4) {
                _controleAjax.abort();
            }

            var $partial = $('div.partial');
            $('.loader.global').parent().addClass('active');

            _controleAjax = $.ajax({
                url: '/principal/avaliacao/certificacao/detalheindividual/' + _codAvaliacao,
                data: {
                    pessoa: codPessoa
                },
                type: 'POST',
                success: function (partial) {
                    $partial.html(partial);
                },
                error: function () {
                    siac.mensagem('Ocorreu um erro inesperado.');
                },
                complete: function () {
                    $('.partial .ui.accordion').accordion();
                    $('[data-content], [data-html]').popup();
                    $('.loader.global').parent().removeClass('active');
                    siac.Anexo.iniciar();
                }
            });
        });

        $('.ui.modal').modal();

        siac.Anexo.iniciar();

        $('.corrigir.button').popup({
            title: 'Corrigir avaliação',
            content: 'Esta avaliação possui correções pendentes.'
        }).popup('show');

        $('[data-ordenar-id]').click(function () {
            var $this = $(this);
            ordenar(this, $this.data('ordenarId'), $this.data('ordenarTipo'))
        });
    }

    function ordenar(contexto, id, tipo) {
        var valores = [];
        var $tabela = $(contexto).closest('table');
        var $corpo = $tabela.find('tbody');
        var $linhas = $corpo.find('tr').clone();

        for (var i = 0, length = $linhas.length; i < length; i++) {
            valores.push($linhas.eq(i).find('[data-ordenar=' + id + ']').attr('data-ordenar-valor'));
        }
        if (tipo.toLowerCase() == 'desc') {
            valores.sort().reverse();
            $(contexto).data('ordenarTipo', 'asc');
        }
        else {
            valores.sort();
            $(contexto).data('ordenarTipo', 'desc');
        }

        $corpo.html('');

        for (var i = 0, length = valores.length; i < length; i++) {
            $corpo.append($linhas.find('[data-ordenar-valor="' + valores[i] + '"]').parents('tr'));
        }
    }

    return {
        iniciar: iniciar
    }
})();