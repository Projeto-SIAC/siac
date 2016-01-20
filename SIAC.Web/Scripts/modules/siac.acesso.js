siac.Acesso = siac.Acesso || {};

siac.Acesso.Visitante = (function () {
    function iniciar() {
        $('.atualizar.button').click(function () {
            verificar();
        });
	}

    function verificar() {
        $('.ui.error.message .list').html('');
        var validado = false;
        if (!$('#txtNovaSenha').val()) {
            $('.ui.error.message .list').append('<li>É necessário inserir a nova senha</li>')
        }
        else if (!$('#txtConfirmaNovaSenha').val()) {
            $('.ui.error.message .list').append('<li>É necessário inserir a confirmação da nova senha</li>')
        }
        else if ($('#txtNovaSenha').val() != $('#txtConfirmaNovaSenha').val()) {
            $('.ui.error.message .list').append('<li>As senhas informadas não conferem</li>')
        }
        else if ($('#txtNovaSenha').val() == $('#txtConfirmaNovaSenha').val()) {
            validado = true;
        }

        if (validado) {
            atualizarSenha();
        }
        else {
            $('form').addClass('error');
        }
	}

	function atualizarSenha() {
	    $('form').submit();
	}

    return {
        iniciar: iniciar
    }
})();