siac.Reposicao = siac.Reposicao || {};

siac.Reposicao.Justificar = (function () {
    var _justificacao = {};

    function iniciar() {
        $('.ui.dropdown').dropdown();
        $('.ui.checkbox').checkbox();
        $('.ui.accordion').accordion({ animateChildren: false });
        $('.ui.cancelar.button').popup({ inline: true, on: "click" });

        $('.ui.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        $('.ui.prosseguir.button').click(function () {
            prosseguir();
        });

        $('input[name^="chkSelecionado"]').change(function () {
            var $this = $(this);
            if ($this.is(':checked')) {
                $this.parents('tr').addClass('active');
            }
            else {

                $this.parents('tr').removeClass('active');
            }
        });

        $('.ui.adicionar.button').click(function () {
            _justificacao.aluno = $('#ddlAluno').val();
            _justificacao.descricao = $('#txtJustificacao').val();
            _justificacao.cadastro = new Date().toISOString();
            verificar();
        });

        $('.ui.confirmar.modal').modal({
            onApprove: function () {
                _justificacao.confirmacao = new Date().toISOString();
                _justificacao.senha = $('#txtSenha').val();
                if (_justificacao.senha) {
                    confirmar();
                }
                else {
                    $('#txtSenha').transition('tada');
                    return false;
                }
            },
            onHidden: function () {
                $('#txtSenha').val('');
            }
        });

        $('.ui.nova.button').click(function () { gerar(true) });
        $('.ui.mesma.button').click(function () { gerar(false) });
    }

    function verificar() {
        $('form').removeClass('error');

        var $ul = $('form .error.message > ul');
        $ul.html('');

        var valido = true;

        if (!_justificacao.aluno) {
            $ul.append($('<li></li>').text('Selecione o aluno'));
            valido = false;
        }
        if (!_justificacao.descricao) {
            $ul.append($('<li></li>').text('Digite a justificação'));
            valido = false;
        }

        if (valido) {
            var $modal = $('.ui.confirmar.modal');

            $modal.find('.content > .form > .field').remove();

            var $form = $('form').clone();

            $form.find('.message').remove();
            $form.find('.button').parent().remove();

            lstInput = $form.find(':input');

            for (var i = 0, length = lstInput.length; i < length; i++) {
                lstInput.eq(i)
                    .removeAttr('id')
                    .removeAttr('name')
                    .removeAttr('required')
                    .prop('readonly', true)
                ;
            }

            $form.find('textarea').text(_justificacao.descricao);

            $modal.find('.content .form').prepend($form.html());

            $modal.modal('show');
        }
        else {
            $('form').addClass('error');
        }
    }

    function confirmar() {
        $.ajax({
            url: window.location.pathname,
            type: 'POST',
            data: {
                justificacao: _justificacao
            },
            success: function () {
                window.location.reload();
            },
            error: function () {
                siac.mensagem('Aconteceu um erro durante a requisição.')
            }
        });
    }

    function prosseguir() {
        var $lstChkSelecionado =  $('input[name^="chkSelecionado"]:checked');
        if ($lstChkSelecionado.length == 0) {
            siac.mensagem('Selecione os alunos que realizarão esta reposição.')
        }
        else {
            $('.prosseguir.modal').modal('show');
        }
    }

    function gerar(nova) {
        $('.modal').modal('hide');
        $('.ui.global.loader').parent().dimmer('show');

        var arrJustificaticoes = [];
        var lstSelecionado = $('input[name^="chkSelecionado"]:checked');
        for (var i = 0, length = lstSelecionado.length; i < length; i++) {
            arrJustificaticoes.push(lstSelecionado.eq(i).parents('tr[id]').attr('id'));
        }

        $.ajax({
            url: '/dashboard/avaliacao/reposicao/gerar/' + window.location.pathname.match(/acad[0-9]+$/)[0],
            type: 'POST',
            data: {
                justificaticoes: arrJustificaticoes,
                nova: nova
            },
            success: function (url) {
                window.location.href = url;
            },
            error: function () {
                siac.mensagem('Aconteceu um erro.');
            }
        });
    }

    return {
        iniciar: iniciar
    }

})();

siac.Reposicao.Configurar = (function () {
    var _codAvaliacao;

    function iniciar() {
        _codAvaliacao = window.location.pathname.match(/repo[0-9]+$/)[0];

        $('.ui.informacoes.modal').modal();

        $('div').popup();

        $('.cancelar.button').popup({
            on: 'click'
        });

        $('.right.floated.button').popup({
            inline: true,
            on: 'click'
        });

        $('.ui.accordion').accordion({
            animateChildren: false
        });

        $('.informacoes.button').click(function () {
            $('.informacoes.modal').modal('show');
        });

        $('.salvar.button').click(function () {
            salvar();
        });

        adicionarEventos();
    }

    function adicionarEventos() {
        $('.trocar.button').off().click(function () {
            var $_this = $(this);
            var codQuestao, indice, codTipoQuestao;

            codQuestao = $_this.parents('[data-questao]').attr('data-questao');
            indice = $_this.parents('[data-indice]').attr('data-indice');
            codTipoQuestao = $_this.parents('[data-tipo]').attr('data-tipo');

            obterNovaQuestao(codQuestao, indice, codTipoQuestao);
        });

        siac.Anexo.iniciar();

        $('.desfazer.button').off().click(function () {
            var $_this = $(this);
            var codQuestao, indice, codTipoQuestao;

            codQuestao = $_this.parents('[data-questao]').attr('data-questao');
            indice = $_this.parents('[data-indice]').attr('data-indice');
            codTipoQuestao = $_this.parents('[data-tipo]').attr('data-tipo');

            desfazer(codQuestao, indice, codTipoQuestao);
        });
    }

    function obterNovaQuestao(codQuestao, indice, codTipoQuestao) {
        var card = $('#CardQuestao' + indice);
        card.addClass('ui form loading');
        $('#CardQuestao' + indice + ' div').popup('hide');
        $.ajax({
            type: "POST",
            url: '/Dashboard/Avaliacao/Reposicao/TrocarQuestao',
            data: {
                codigoAvaliacao: _codAvaliacao,
                tipo: codTipoQuestao,
                indice: indice,
                codQuestao: codQuestao
            },
            success: function (data) {
                if (data) {
                    card.html(data);
                    $('.ui.accordion').accordion({ animateChildren: false });
                    $('.right.floated.button').popup({ on: 'click' });
                    $('.ui.button.disabled').removeClass('disabled');
                    $('#CardQuestao' + indice + ' .ui.desfazer.button').parents('.popup').prev().show();
                }
                card.removeClass('ui form loading');
            },
            error: function () {
                card.removeClass('ui form loading');
                siac.mensagem('Ocorreu um erro não esperado.');
            },
            complete: function () {
                adicionarEventos();
            }
        });
    }

    function salvar() {
        $('.button.salvar').addClass('loading');
        $('form').submit();
    }

    function desfazer(codQuestao, indice, codTipoQuestao) {
        var card = $('#CardQuestao' + indice);
        card.addClass('ui form loading');
        $('#CardQuestao' + indice + ' div').popup('hide');
        $.ajax({
            type: 'POST',
            url: '/Dashboard/Avaliacao/Reposicao/Desfazer',
            data: {
                codigoAvaliacao: _codAvaliacao,
                tipo: codTipoQuestao,
                indice: indice,
                codQuestao: codQuestao
            },
            success: function (data) {
                if (data) {
                    card.html(data);
                    $('.ui.accordion').accordion({ animateChildren: false });
                    $('.right.floated.button').popup({ on: 'click' });
                    $('#CardQuestao' + indice + ' .ui.desfazer.button').parents('.popup').prev().hide();
                }
                card.removeClass('ui form loading');
            },
            error: function () {
                card.removeClass('ui form loading');
                siac.mensagem('Ocorreu um erro não esperado.');
            },
            complete: function () {
                adicionarEventos();
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Reposicao.Agendar = (function () {
    function iniciar() {
        $('.ui.informacoes.modal')
            .modal()
        ;

        $('.ui.dropdown')
            .dropdown()
        ;
        
        $('.ui.accordion').accordion({ animateChildren: false });

        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal')
            .modal({
                onApprove: function () {
                    $('form').addClass('loading').submit();
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

siac.Reposicao.Index = (function () {
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
            url: '/Dashboard/Avaliacao/Reposicao/Listar',
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
                console.log(pesquisa);
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

siac.Reposicao.Agendada = (function () {
    var _codAvaliacao, _categoriaUsuario;

    function iniciar() {
        if (!_categoriaUsuario) {
            $elemento = $('[data-categoria]');
            _categoriaUsuario = $elemento.attr('data-categoria');
            $elemento.removeAttr('data-categoria');
        }

        if (!_codAvaliacao) {
            _codAvaliacao = window.location.pathname.match(/repo[0-9]+$/)[0];
        }

        $('.ui.accordion').accordion({
            animateChildren: false
        });

        $('.arquivar.button').click(function () {
            var $_this = $(this);
            $_this.addClass('loading');
            $.ajax({
                url: '/Dashboard/Avaliacao/Reposicao/Arquivar/' + _codAvaliacao,
                type: 'POST',
                success: function (data) {
                    if (data) {
                        window.location.href = '/dashboard/avaliacao/reposicao/detalhe/' + _codAvaliacao;
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
                url: '/Dashboard/Avaliacao/Reposicao/ContagemRegressiva',
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
        var hub = $.connection.reposicaoHub;
        if (_categoriaUsuario == 1) {
            hub.client.liberar = function (strCodigo) {
                $.ajax({
                    type: 'POST',
                    url: '/Dashboard/Avaliacao/Reposicao/ContagemRegressiva',
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
            hub.client.bloquear = function (strCodigo) {

                $.ajax({
                    type: 'POST',
                    url: '/Dashboard/Avaliacao/Reposicao/ContagemRegressiva',
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
                hub.server.realizar(_codAvaliacao);
            });
        }
        else if (_categoriaUsuario == 2) {
            $.connection.hub.start().done(function () {
                $('.liberar.button').click(function () {
                    $('.liberar.button').addClass('loading');
                    $.ajax({
                        url: '/Dashboard/Avaliacao/Reposicao/AlternarLiberar',
                        type: 'POST',
                        data: { codAvaliacao: _codAvaliacao },
                        success: function (data) {
                            if (data == true) {
                                hub.server.liberar(_codAvaliacao, true);
                                $('.liberar.button').addClass('active').removeClass('loading').text('Liberada');
                                $.ajax({
                                    type: 'POST',
                                    url: '/Dashboard/Avaliacao/Reposicao/ContagemRegressiva',
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
                                hub.server.liberar(_codAvaliacao, false);
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