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