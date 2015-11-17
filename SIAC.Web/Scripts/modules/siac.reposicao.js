siac.Reposicao = siac.Reposicao || {};

siac.Reposicao.Justificar = (function () {
    var _justificacao = {};

    function iniciar() {
        $('.ui.dropdown').dropdown();
        $('.ui.cancelar.button').popup({inline: true, on: "click"});

        $('.ui.adicionar.button').click(function () {
            _justificacao.aluno = $('#ddlAluno').val();
            _justificacao.descricao = $('#txtJustificacao').val();
            _justificacao.cadastro = new Date();
            verificar();
        });

        $('.ui.confirmar.modal').modal({
            onApprove: function () {
                _justificacao.confirmacao = new Date();
                _justificacao.senha = $('#txtSenha').val();
                confirmar();
            }
        });
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
            success: function() {
                window.location.reload();
            },
            error: function () {
                siac.mensagem('Aconteceu um erro durante a requisição.')
            }
        });
        // ajax goes here
    }

    return {
        iniciar: iniciar
    }

})();