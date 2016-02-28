siac.Institucional = siac.Institucional || {};

siac.Institucional.Configuracao = (function () {
	function iniciar() {

		$('.ui.dropdown').dropdown();
		$('.ui.accordion').accordion({ animateChildren: false });

		$('.tabular.menu .item').tab({
			history: true,
			historyType: 'state',
			path: '/institucional/configuracao'
		});
	}

	return {
		iniciar: iniciar
	}
})();

siac.Institucional.Gerar = (function () {
    function iniciar() {
        $('.ui.modal').modal();
        $('.ui.termo.modal').modal({ closable: false }).modal('show');
        $('.cancelar.button').popup({ on: 'click' });
        $('.ui.confirmar.modal')
         .modal({
             onApprove: function () {
                 $('form').submit();
             }
         });

        $('.prosseguir.button').click(function () {
            prosseguir();
        });
    }

    function prosseguir() {
        var $errorList = $('form .error.message .list');

        $errorList.html('');
        $('form').removeClass('error');

        var valido = true;

        if (!$('#txtTitulo').val().trim()) {
            $errorList.append('<li>Insira o título</li>');
            valido = false;
        }

        if (!($('#txtObjetivo').val().trim())) {
            $errorList.append('<li>Insira o objetivo</li>');
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

        $('#txtModalTitulo').val($('#txtTitulo').val());
        $('#txtModalObjetivo').val($('#txtObjetivo').val());

        $modal.modal('show');
    }

    return {
        iniciar: iniciar
    }
})();

siac.Institucional.Questionario = (function () {
    var _codAvaliacao;

    function iniciar() {
        _codAvaliacao = window.location.pathname.toLowerCase().match(/avi[0-9]+$/)[0];
        $('.ui.dropdown').dropdown();
        $('.tabular.menu .item').tab();
        $('h3').popup();
        $('.ui.accordion').accordion({ animateChildren: false });
        $('.floated.remover.button').popup({ on: 'click', inline: true });
        $('.floated.editar.button').click(function () {
            abrirModalEditar(this);
        })
        $('.tab.questoes .remover.button.tiny').click(function () {
            deletarQuestao(this);
        });
        $('.ui.checkbox').checkbox({
            onChange: function () {
                var checked = $(this).is(':checked');
                if (checked) {
                    $('#txtAlternativaDiscursiva').prop('readonly', false).parent().removeClass('disabled');

                } else {
                    $('#txtAlternativaDiscursiva').prop('readonly', true).parent().addClass('disabled');
                }
            }
        });

        //Adicionando Alternativas por default
        for (var i = 0; i < 5; i++) {
            adicionarAlternativa();
        }

        //Funções
        $('#ddlTipo').change(function () {
            mostrarCamposPorTipo();
        });
        $('.questao.objetiva .adicionar.button').click(function () {
            adicionarAlternativa();
        });
        $('.adicionar.questao.button').click(function () {
            verificar();
        })
        $('.prosseguir.button').click(function () {
            prosseguir();
        });
    }

    function adicionarAlternativa() {
        var i = $('.ui.alternativas.accordion .title').length + 1;
        $('.ui.alternativas.accordion').append(
                        '<div class="title">' +
                            '<input id="txtAlternativaIndex" value="' + i + '" hidden />' +
                            '<i class="dropdown icon"></i>Alternativa ' + i +
                        '</div>' +
                        '<div class="content ui segment">' +
                            '<div class="field required">' +
                                '<label data-ajuda="Insira abaixo o enunciado desta alternativa" for="txtAlternativaEnunciado' + i + '">Insira o enunciado</label>' +
                                '<textarea id="txtAlternativaEnunciado' + i + '" name="txtAlternativaEnunciado' + i + '" rows="2" placeholder="Enunciado..."></textarea>' +
                            '</div>' +
                            '<div class="field">' +
                                '<div class="ui button">' +
                                    'Remover' +
                                '</div>' +
                                '<div class="ui special popup">' +
                                    '<div class="header">Tem certeza?</div>' +
                                    '<div class="content"><p>Essa ação não poderá ser desfeita.</p>' +
                                    '<div class="ui right aligned remover button tiny">Sim, remover</div>' +
                                '</div>' +
                            '</div>' +
                        '</div>' +
                    '</div>');
        $('#txtQtdAlternativas').val(i);
        $('.ui.alternativas.accordion .remover.button').off().click(function () {
            removerAlternativa(this);
        });
        $('.ui.alternativas.accordion .button').popup({ inline: true, on: 'click', position: 'right center' });
        $('.ui.alternativas.accordion').accordion({ animateChildren: false });

    }

    function renomearAlternativas() {
        var list = $('.ui.alternativas.accordion .title');
        var listContent = $('.ui.alternativas.accordion .content.segment');
        for (var i = 0; i < list.length; i++) {
            var j = list.eq(i).find('#txtAlternativaIndex').val();
            list.eq(i).html('<input id="txtAlternativaIndex" value="' + (i + 1) + '" hidden /><i class="dropdown icon"></i>Alternativa ' + (i + 1));

            /* RENOMEAR LABELS, INPUTS e TEXTAREAS */
            listContent.eq(i).find('label[for="txtAlternativaEnunciado' + j + '"]').attr('for', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAlternativaEnunciado' + j + '"]').attr('name', 'txtAlternativaEnunciado' + (i + 1)).attr('id', 'txtAlternativaEnunciado' + (i + 1));
        }
    }

    function removerAlternativa(button) {
        var i = $('.ui.alternativas.accordion .title').length;
        if (i > 2) {
            var content = $(button).parent().parent().parent().parent();
            var title = $(content).prev();
            title.remove();
            content.remove();
            renomearAlternativas();
            i--;
        } else {
            siac.aviso('É preciso ter, no mínimo, duas alternativas por questão', 'red');
        }
        $('#txtQtdAlternativas').val(i);
    }

    function mostrarCamposPorTipo() {
        var tipo = $('#ddlTipo').val();
        $('.questao.objetiva').hide();
        if (tipo == 1) {
            $('.questao.objetiva').show();
            $('.segment.questao.objetiva').attr('style', '');
        }
        else if (tipo == 2) {
            $('.questao.objetiva').hide();
        }
    }

    function verificar() {
        $('form.cadastro').removeClass('error');
        $('.ui.error.message .list').html('');
        var validado = false;
        if (!$('#ddlModulo').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar um módulo</li>')
        }
        if (!$('#ddlCategoria').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar uma categoria</li>')
        }
        if (!$('#ddlIndicador').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar um indicador</li>')
        }

        if (!$('#ddlTipo').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar um tipo</li>')
        }

        if (!$('#txtEnunciado').val()) {
            $('.ui.error.message .list').append('<li>É necessário preencher o enunciado</li>')
        }

        if ($('#ddlTipo').val()) {
            var tipo = $('#ddlTipo').val();

            if (tipo == 1) {
                var qteAlternativas = $('#txtQtdAlternativas').val();
                var ok = true;
                for (var i = 0; i < qteAlternativas; i++) {
                    if ($('#txtAlternativaEnunciado' + (i + 1)).val() == '') {
                        ok = false;
                    }
                }
                if ($('#chkAlternativaDiscursiva').is(':checked')) {
                    if (!$('#txtAlternativaDiscursiva').val()) {
                        ok = false;
                    }
                }
                if (ok) {
                    validado = true;
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher os enunciados de todas as alternativas</li>')
                }
            }
            else if (tipo == 2) {
                if ($('#txtEnunciado').val()) {
                    validado = true;
                }
            }
        }

        if (validado) {
            inserirQuestao();
        }
        else $('form.cadastro').addClass('error');
    }

    function adicionarQuestao(questaoId) {
        //OBTENDO DADOS DA QUESTÃO
        var moduloCod = $('#ddlModulo :selected').val();
        var modulo = $('#ddlModulo :selected').text();
        var categoriaCod = $('#ddlCategoria').val();
        var categoria = $('#ddlCategoria :selected').text();
        var indicadorCod = $('#ddlIndicador').val();
        var indicador = $('#ddlIndicador :selected').text();
        var tipo = $('#ddlTipo').val();
        var enunciado = $('#txtEnunciado').val();
        var observacao = $('#txtObservacao').val();

        var TEMPLATE_QUESTAO_HTML = '<div class="title">' +
                                        '<i class="dropdown icon"></i>' +
                                        modulo +
                                    '</div>' +
                                    '<div class="content" data-modulo="' + moduloCod + '">' +
                                        '<div class="accordion">' +
                                            '<div class="title">' +
                                                '<i class="dropdown icon"></i>' +
                                                categoria +
                                            '</div>' +
                                            '<div class="content" data-categoria="' + categoriaCod + '">' +
                                                '<div class="accordion">' +
                                                    '<div class="title">' +
                                                        '<i class="dropdown icon"></i>' +
                                                         indicador +
                                                    '</div>' +
                                                    '<div class="content" data-indicador="' + indicadorCod + '">' +
                                                        '<div class="accordion">' +
                                                            '<div class="title">' +
                                                                '<i class="dropdown icon"></i>' +
                                                                     enunciado.encurtarTextoEm(50) +
                                                            '</div>' +
                                                            '<div class="content" data-questao="' + questaoId + '">' +
                                                                '<div class="ui segment">' +
                                                                    '<div class="ui right floated remover button">Remover</div>' +
                                                                    '<div class="ui popup">' +
                                                                        '<div class="header">Tem certeza?</div>' +
                                                                        '<div style="padding:0!important;" class="content">' +
                                                                            '<p>Essa ação não poderá ser desfeita.</p>' +
                                                                            '<div class="ui right aligned remover button tiny">Sim, remover</div>' +
                                                                        '</div>' +
                                                                    '</div>' +
                                                                    '<div class="ui right floated editar button">Editar</div>' +
                                                                    '<h3 class="ui dividing header" data-content="' + observacao + '">' + enunciado + '</h3>' +
                                                                    '<div class="ui very relaxed list">' +
                                                                    '</div>' +
                                                                '</div>' +
                                                            '</div>' +
                                                        '</div>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</div>' +
                                        '</div>' +
                                    '</div>';
        var $template = $(TEMPLATE_QUESTAO_HTML);

        if (tipo == 1) {
            var list = $('.ui.alternativas.accordion .title');
            var listContent = $('.ui.alternativas.accordion .content.segment');
            for (var i = 0; i < list.length; i++) {
                var j = list.eq(i).find('#txtAlternativaIndex').val();

                var enunciado = listContent.eq(i).find('textarea[name="txtAlternativaEnunciado' + j + '"]').val();
                var alternativa = '<div class="item"><b>' + j + ')</b> ' + enunciado + '</div>';
                $template.find('.very.relaxed.list').append(alternativa);
            }
            if ($('#chkAlternativaDiscursiva').is(':checked')) {
                var alternativaDiscursiva = $('#txtAlternativaDiscursiva').val();
                var alternativa = '<div class="item"><b>' + (list.length + 1) + ')</b> ' + alternativaDiscursiva + '<div class="ui left pointing label">Alternativa discursiva</div></div>';
                $template.find('.very.relaxed.list').append(alternativa);
            }
        }
        else if (tipo == 2) {
            $template.find('.very.relaxed.list').remove();
            $template.find('.ui.segment').append('<input class="input" placeholder="Resposta" type="text" readonly/>');
        }

        //Obtendo possíveis accordions questões por módulos
        var $questoesModulo = $('[data-modulo=' + moduloCod + ']');
        var $questoesCategoria = $('[data-categoria=' + categoriaCod + ']');
        var $questoesIndicador = $('[data-indicador=' + indicadorCod + ']');

        var $local = $('.tab.questoes .fluid.styled.accordion');
        var $questao = $template;
        var $localNovo = $questoesModulo;
        var questaoIndice = 1;

        if ($localNovo.length > 0) {
            $local = $localNovo;
            $localNovo = $local.find('[data-categoria=' + categoriaCod + ']');
            questaoIndice = $local.length + 1;
            if ($localNovo.length > 0) {
                $local = $localNovo;
                $localNovo = $local.find('[data-indicador=' + indicadorCod + ']');
                questaoIndice = $local.length + 1;
                if ($localNovo.length > 0) {
                    $local = $localNovo;
                    $questao = $template.find('[data-indicador=' + indicadorCod + ']').find('.accordion').html();
                }
                else {
                    $questao = $template.find('[data-categoria=' + categoriaCod + ']').find('.accordion').html();
                }
            }
            else {
                $questao = $template.find('.accordion').html();
            }
            $local.children('.accordion').append($questao);
        }
        else {
            $local.append($questao);
        }
        $('h3').popup();
        $('.floated.remover.button').popup({ on: 'click', inline: true });
        $('.tab.questoes .remover.button.tiny').click(function () {
            deletarQuestao(this);
        });
        $('.tab.questoes .editar.button').click(function () {
            abrirModalEditar(this);
        });
        $('.ui.accordion').accordion({ animateChildren: false });
        siac.aviso('Questão adicionada com sucesso!', 'green');
    }

    function removerQuestao(button) {
        var content = $(button).parent().parent().parent().parent();
        var title = $(content).prev();
        var modulo = content.parents('[data-modulo]');
        var categoria = content.parents('[data-categoria]');
        var indicador = content.parents('[data-indicador]');
        title.remove();
        content.remove();

        if (indicador.find('.title').length <= 0) {
            indicador.prev().remove();
            indicador.remove();
            if (categoria.find('.title').length <= 0) {
                categoria.prev().remove();
                categoria.remove();
                if (modulo.find('.title').length <= 0) {
                    modulo.prev().remove();
                    modulo.remove();
                }
            }
        }
        siac.aviso('Questão removida com sucesso!', 'red');
    }

    function inserirQuestao() {
        $('.adicionar.questao.button').addClass('loading');
        var form = $('form.cadastro').serialize();
        $.ajax({
            type: 'POST',
            url: '/institucional/cadastrarquestao/' + _codAvaliacao,
            data: form,
            dataType: 'json',
            success: function (questaoId) {
                adicionarQuestao(questaoId);
            },
            error: function () {
                siac.mensagem('Erro na adição de questão');
            },
            complete: function () {
                $('.adicionar.questao.button').removeClass('loading');
            }
        });
    }

    function deletarQuestao(button) {
        var content = $(button).parent().parent().parent().parent();

        var modulo = content.parents('[data-modulo]').data('modulo');
        var categoria = content.parents('[data-categoria]').data('categoria');
        var indicador = content.parents('[data-indicador]').data('indicador');
        var ordem = content.data('questao');

        $(button).addClass('loading');
        $.ajax({
            type: 'POST',
            url: '/institucional/removerquestao/' + _codAvaliacao,
            data: {
                modulo: modulo,
                categoria: categoria,
                indicador: indicador,
                ordem: ordem
            },
            success: function () {
                removerQuestao(button);
            },
            error: function () {
                siac.mensagem('Erro na remoção de questão');
            },
            complete: function () {
                $(button).removeClass('loading');
            }
        });
    }

    function abrirModalEditar(button) {
        $('.ui.editar.modal .alternativas').html('');
        $('.ui.editar.modal form').removeClass('error');
        var $content = $(button).parent().parent();

        var enunciado = $content.find('h3').text();
        var observacao = $content.find('h3').data('content');

        $('#txtEditarEnunciado').val(enunciado);
        $('#txtEditarObservacao').val(observacao);

        var qteAlternativas = $content.find('.item').length;
        if (qteAlternativas > -1) {
            var indice = 1;
            $content.find('.item').map(function () {
                //Obter somente o texto do elemento 'DIV' Pai
                var alternativa = $(this).clone().children().remove().end().text().trim();
                
                var $ALTERNATIVA_TEMPLATE = $('<div class="field required">' +
                                                '<label for="txtEditarAlternativa' + indice + '">Alternativa '+indice+'</label>' +
                                                '<textarea id="txtEditarAlternativa' + indice + '" name="txtEditarAlternativa' + indice + '" rows="2" required placeholder="Alternativa...">'+alternativa+'</textarea>' +
                                            '</div>');

                //Caso a alternativa seja discursiva
                if ($(this).find('.label').length > 0) {
                    $ALTERNATIVA_TEMPLATE.find('label').text('Alternativa Discursiva').attr('for', 'txtEditarAlternativaDiscursiva');
                    $ALTERNATIVA_TEMPLATE.find('textarea').attr('id', 'txtEditarAlternativaDiscursiva').attr('name', 'txtEditarAlternativaDiscursiva');
                }

                $('.ui.editar.modal .alternativas').append($ALTERNATIVA_TEMPLATE);
                indice++;
            })
        }

        var modulo = $content.parents('[data-modulo]').data('modulo');
        var categoria = $content.parents('[data-categoria]').data('categoria');
        var indicador = $content.parents('[data-indicador]').data('indicador');
        var ordem = $content.data('questao');
        $('.ui.editar.modal').modal({
            onApprove: function () {
                $modal = $(this);
                var validado = true;
                $modal.find(':input').map(function () {
                    if (!$(this).val() && $(this).attr('id') != 'txtEditarObservacao') {
                        $modal.find('.error.message .list').html('<li>Todos os campos obrigatórios devem ser preenchidos!');
                        $modal.find('form').addClass('error');
                        validado = false;
                    }
                });
                if (validado) {
                    $modal.find('form').removeClass('error');
                    editarQuestao(modulo, categoria, indicador, ordem, $content);
                }
                return false;
            }
        }).modal('show');   
    }

    function editarQuestao(modulo, categoria, indicador, ordem,$content) {
        var $button = $('.editar.modal .approve.button');
        $button.addClass('loading');
        var form = $('form.edicao').serialize();
        $.ajax({
            type: 'POST',
            url: '/institucional/editarquestao/' + _codAvaliacao+'?modulo='+modulo+'&categoria='+categoria+'&indicador='+indicador+'&ordem='+ordem,
            data: form,
            dataType: 'json',
            success: function (form) {
                $content.find('h3').attr('data-content', $('#txtEditarObservacao').val()).popup().text($('#txtEditarEnunciado').val());

                qteAlternativa = $content.find('.relaxed.list .item').length;

                if (qteAlternativa > 0) {
                    var indice = 1;
                    $content.find('.relaxed.list .item').map(function () {
                        var $alternativa = $(this);
                        var alternativaFormTexto = '';

                        if ($alternativa.find('.label').length > 0) { //Caso seja discursiva
                            alternativaFormTexto = $('#txtEditarAlternativaDiscursiva').val();
                            $alternativa.html('<b>' + indice + ')</b> ' + alternativaFormTexto + '<div class="ui left pointing label">Alternativa discursiva</div>');
                        } else { //Caso seja objetiva
                            alternativaFormTexto = $('#txtEditarAlternativa' + indice).val();
                            $alternativa.html('<b>' + indice + ')</b> ' + alternativaFormTexto);
                        }
                        indice++;
                    });
                }

            },
            error: function (erro) {
                siac.mensagem('Erro na edição de questão');
                alert(erro);
            },
            complete: function () {
                $button.removeClass('loading');
                $('.ui.editar.modal').modal('hide');
            }
        });
    }

    function prosseguir() {
        $('form.cadastro').removeClass('error');
        var qteQuestoes = $('.tab.questoes .accordion .title').length;

        if (qteQuestoes <= 0) {
            $('.ui.error.message .list').html('<li>É necessário cadastrar o questionário antes de prosseguir</li>');
            $('form.cadastro').addClass('error');
        }
        else {
            $('.basic.confirmar.modal').modal({
                onApprove: function () {
                    window.location.href = '/institucional/configurar/' + _codAvaliacao;
                }
            }).modal('show');
        }
    }
    return {
        iniciar: iniciar
    }
})();

siac.Institucional.Configurar = (function () {
    var _codAvaliacao;
    function iniciar() {
        _codAvaliacao = window.location.pathname.toLowerCase().match(/avi[0-9]+$/)[0];

        $('.ui.accordion').accordion({ animateChildren: false });
        $('.cancelar.button').popup({ on: 'click', inline: true });
        $('.prosseguir.button').click(function () { prosseguir(); });
        $('h3').popup();

        $('.subir.button').click(function () {
            subirElemento(this);
            return false;
        });
        $('.descer.button').click(function () {
            descerElemento(this);
            return false;
        })
    }

    function prosseguir() {
        $('.basic.confirmar.modal').modal({
            onApprove: function () {
                $('.prosseguir.button').addClass('loading');
                $('.ui.global.loader').parent().dimmer('show');
                $.ajax({
                    type: 'POST',
                    url: '/institucional/configurar/' + _codAvaliacao,
                    data: {
                        questoes: obterQuestoesOrdem()
                    },
                    success: function () {
                        window.location.href = '/institucional/publico/' + _codAvaliacao;
                    },
                    error: function () {
                        $('.ui.global.loader').parent().dimmer('hide');
                    },
                    complete: function () {
                        $('.prosseguir.button').removeClass('loading');
                    }
                })
            }
        }).modal('show');
    }

    function subirElemento(button) {
        var $title = $(button).parent();
        var $content = $title.next();
                
        $title.insertBefore($title.prev().prev());
        $content.insertBefore($content.prev().prev());

    }

    function descerElemento(button) {
        var $title = $(button).parent();
        var $content = $title.next();

        $title.insertAfter($title.next().next().next());
        $content.insertAfter($content.next().next().next());

    }

    function obterQuestoesOrdem() {
        var _arrayQuestoesOrdem = [];
        $('[data-questao]').map(function () {
            var $questao = $(this);

            var modulo = $questao.parents('[data-modulo]').data('modulo');
            var categoria = $questao.parents('[data-categoria]').data('categoria');
            var indicador = $questao.parents('[data-indicador]').data('indicador');
            var questao = $questao.data('questao');

            _arrayQuestoesOrdem.push(modulo + '.' + categoria + '.' + indicador + '.' + questao);
        });
        return _arrayQuestoesOrdem;
    }

	return {
		iniciar: iniciar
	}
})();

siac.Institucional.Publico = (function () {
    var _codAvaliacao;
    var _controleAjax;
    var _results = [];
    var _content = [];
    var _result;
    function iniciar() {
        _codAvaliacao = window.location.pathname.toLowerCase().match(/avi[0-9]+$/)[0];
        $('.cancelar.button').popup({ on: 'click', inline: true });
        $('.ui.modal').modal();
        $('.ui.dropdown').dropdown();
        $('.ui.search').search();
        $('.ui.accordion').accordion({
            animateChildren: false
        });
        
        $('#ddlFiltro').change(function () {
            filtrar($(this).val());
        });

        $('.selecionar.button').click(function () {
            selecionar();
        });

        $('.remover.button').off().click(function () {
            remover($(this));
        });

        $('.prosseguir.button').click(function () {
            if (!_results || _results.length <= 0) {
                siac.aviso('Você ainda não selecionou os avaliados ou grupos.', 'red', 'warning sign');
            }
            else {
                prosseguir();
            }
        });
    }

    function prosseguir() {
        $('.basic.confirmar.modal').modal({
            onApprove: function () {
                salvar();
            }
        }).modal('show');
    }

    function filtrar(filtro) {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }

        $buscar = $('.ui.search');

        $buscar.addClass('loading');

        _controleAjax = $.ajax({
            type: 'POST',
            url: '/institucional/filtrarpublico/' + filtro,
            success: function (data) {
                _content = data;
                $buscar.search({
                      source: _content,
                      onSelect: function onSelect(result, response) {
                          _result = result;
                          $('.selecionar.button').removeClass('disabled');
                      },
                      minCharacters: 3
                  });
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
            url: '/institucional/salvarpublico/'+_codAvaliacao,
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

siac.Institucional.Agendar = (function () {
    var _codAvaliacao;
    function iniciar() {
        _codAvaliacao = window.location.pathname.toLowerCase().match(/avi[0-9]+$/)[0];
        $('.cancelar.button').popup({ on: 'click', inline: true });

        $('.prosseguir.button').click(function () {
            verificar();
        });
    }

    function verificar() {
        $('form').removeClass('error');
        $('.error.message .list').html('');
        var dataInicio = $('#txtDataInicio').val();
        var dataTermino = $('#txtDataTermino').val();
        var valido = true;

        if (!dataInicio) {
            $('.error.message .list').append('<li>Preencha a Data de Início</li>');
            valido = false;
        }
        if (!dataTermino) {
            $('.error.message .list').append('<li>Preencha a Data de Término</li>');
            valido = false;
        }
        if (siac.Utilitario.compararData(dataInicio, dataTermino) == 1) {
            $('.error.message .list').append('<li>A Data de Início não pode ser posterior à Data de Término</li>');
            valido = false;
        }
        else if (siac.Utilitario.compararData(dataInicio,new Date().toJSON().slice(0,10)) == -1) {
            $('.error.message .list').append('<li>A Data de Início não pode ser inferior à Data de Hoje</li>');
            valido = false;
        }

        if (valido) {
            prosseguir();
        }
        else {
            $('.form').addClass('error');
        }
    }

    function prosseguir() {
        $('.confirmar.modal').modal({
            onApprove: function () {
                $('form').submit();
            }
        }).modal('show');
    }

    return {
        iniciar: iniciar
    }
})();

siac.Institucional.Realizar = (function () {
    var _codAvaliacao;
    function iniciar() {
        _codAvaliacao = window.location.pathname.toLowerCase().match(/avi[0-9]+$/)[0];
        $('.informacoes.button').click(function () {
            $('.ui.accordion').accordion({
                animateChildren: false,
                onChange: function () {
                    $('.informacoes.modal').modal('refresh');
                }
            });
            $('.informacoes.modal').modal('show');
        });
        $('.ui.accordion').accordion({ animateChildren: false });
        $('.ui.modal').modal();

        $('input[type="radio"]').change(function () {
            var _questao = $(this).parents('[data-questao]');
            var alternativa = $(this).data('alternativa');
            var _alternativaDiscursiva = _questao.find($('input[type=text]'));
            
            if(_alternativaDiscursiva.attr('id') == alternativa){
                _alternativaDiscursiva.removeAttr('readonly').focus();
            }
            else {
                _alternativaDiscursiva.attr('readonly','readonly');
                enviarRespostaObjetiva(this);
            }
        });

        $('input[type="text"]').change(function () {
            var _questao = $(this).parents('[data-questao]');
            var id = $(this).attr('id');

            var $input = _questao.find('[data-alternativa=' + id + ']');

            if ($input.is(':checked')) {
                enviarRespostaAlternativaDiscursiva(this);
            }
        });

        $('textarea').change(function () {
            enviarRespostaDiscursiva(this);
        });
    }

    function enviarRespostaObjetiva(input) {
        var questaoOrdem = $(input).parents('[data-questao]').data('questao');
        var alternativa = $(input).val();
        $.ajax({
            type: 'POST',
            url: '/institucional/enviarrespostaobjetiva/' + _codAvaliacao,
            data: {
                ordem: questaoOrdem,
                alternativa: alternativa
            },
            beforeSend: function () {
                siac.Lembrete.Notificacoes.exibir('Salvando respostas...')
            },
            success: function () {
                siac.Lembrete.Notificacoes.exibir('Resposta salvas.', 'positivo')
            }
        })
    }

    function enviarRespostaDiscursiva(textarea) {
        var questaoOrdem = $(textarea).parents('[data-questao]').data('questao');
        var resposta = $(textarea).val();
        $.ajax({
            type: 'POST',
            url: '/institucional/enviarrespostadiscursiva/' + _codAvaliacao,
            data: {
                ordem: questaoOrdem,
                resposta: resposta
            },
            beforeSend: function() {
                siac.Lembrete.Notificacoes.exibir('Salvando respostas...')
            },
            success: function () {
                siac.Lembrete.Notificacoes.exibir('Resposta salvas.', 'positivo')
            }
        })
    }

    function enviarRespostaAlternativaDiscursiva(input) {
        var _questao = $(input).parents('[data-questao]');
        var questaoOrdem = _questao.data('questao');
        var resposta = $(input).val();
        var alternativa = _questao.find(':checked').val();
        $.ajax({
            type: 'POST',
            url: '/institucional/enviaralternativadiscursiva/' + _codAvaliacao,
            data: {
                ordem: questaoOrdem,
                alternativa: alternativa,
                resposta: resposta
            },
            beforeSend: function () {
                siac.Lembrete.Notificacoes.exibir('Salvando respostas...')
            },
            success: function () {
                siac.Lembrete.Notificacoes.exibir('Resposta salvas.', 'positivo')
            }
        })
    }

    return {
        iniciar: iniciar
    }
})();

siac.Institucional.Historico = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 12, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var categoria = "";
    var pesquisa = "";

    function iniciar() {
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() * 0.5) {
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

        $('.categoria.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            categoria = $_this.attr('data-categoria');
            $_this.closest('.segment').addClass('loading');
            listar();
        });

        $('.ordenar.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            ordenar = $_this.attr('data-ordenar');
            $_this.closest('.segment').addClass('loading');
            listar();
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
            method: 'POST',
            url: '/institucional/listar',
            data: {
                pagina: pagina,
                pesquisa: pesquisa,
                ordenar: ordenar,
                categoria: categoria
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
                    $('.cards .card.hidden').transition({
                        animation: 'pulse',
                        duration: 500,
                        interval: 200
                    });
                }
            },
            complete: function () {
                $('.segment.loading').removeClass('loading');
                $('.input.loading').removeClass('loading');
                $cards.parent().find('.active.loader').remove();
                ativarInformacaoCard();
            }
        });
    }

    function topo() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    }

    function carregarInformacao(header) {
        var codigo = $(header).closest('[data-avi]').data('avi');
        var $_card = $(header).parent().parent();
        $_card.dimmer('show');
        $.ajax({
            type: 'POST',
            url: '/institucional/informacao/' + codigo,
            success: function (view) {
                $('.informacoes.modal').remove();
                $('body').append(view);
                $('.ui.accordion').accordion({
                    animateChildren: false,
                    onChange: function () {
                        $('.informacoes.modal').modal('refresh');
                    }
                });
                $('.informacoes.modal').modal('show');
            },
            error: function (erro) {
                siac.mensagem('Ocorreu um erro na operação.')
            },
            complete: function (view) {
                $_card.dimmer('hide');
            }
        })
    }

    function ativarInformacaoCard() {
        $('.card a.header').off().click(function () {
            carregarInformacao(this);
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Institucional.Resultado = (function () {
    var _codAvaliacao;
    function iniciar() {
        _codAvaliacao = window.location.pathname.toLowerCase().match(/avi[0-9]+$/)[0];

        $('.informacoes.button').click(function () {
            $('.ui.accordion').accordion({
                animateChildren: false,
                onChange: function () {
                    $('.informacoes.modal').modal('refresh');
                }
            })
            $('.informacoes.modal').modal('show');
        });
        $('.ui.accordion').accordion({
            animateChildren: false,
            onOpen: function () {
                if (!$(this).parents('.accordion').hasClass('respostas')) {
                    var $content = $('.content[data-questao].active');
                    var $canvas = $content.find('canvas').get(0);
                    if ($content && $canvas) {
                        var ctx = $canvas.getContext("2d");
                        var data = JSON.parse($content.find('code.dados').html());
                        var chart = new Chart(ctx).Doughnut(data);
                    }
                }
            }
        });
        $('h3').popup();
    }
    return {
        iniciar: iniciar
    }
})();