siac.Reposicao = siac.Reposicao || {};

siac.Reposicao.Justificar = (function () {
    var _justificacao = {};

    function iniciar() {
        $('.ui.dropdown').dropdown();
        $('.ui.checkbox').checkbox();
        $('.ui.accordion').accordion();
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