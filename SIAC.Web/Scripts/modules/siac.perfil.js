siac.Perfil = siac.Perfil || (function () {
    function iniciar() {
        $('#estatisticas').load('/perfil/estatisticas');
        $('.ui.opiniao.modal').modal({
            onApprove: function () {
                if (verificar())
                    enviarOpiniao();
                else {
                    $('.opiniao.modal .form').addClass('error');
                    return false;
                }
            }
        });
        $('.opiniao.button').click(function () {
            $('.opiniao.modal').modal('show');
        });
    }

    function verificar() {
            var $form = $('.opiniao.modal .form');
            $form.removeClass('error');
            $form.find('.message').remove();
            var $message = $('<div class="ui error message"></div>');
            $message.append($('<div class="header"></div>').text('Verifique os seguintes erros'));
            var $list = $('<ul class="list"></ul>');

            if (!$('#txtOpiniao').val().trim()) {
                $list.append($('<li></li>').html('Insira uma <b>opinião</b>'));
            }

            $message.append($list);

            $form.prepend($message);

            return $list.children().length == 0;
    }

    function enviarOpiniao() {
        var txtOpiniao = $('#txtOpiniao').val().trim();
        if (txtOpiniao) {
            $.ajax({
                url: '/perfil/enviaropiniao',
                type: 'post',
                data: { opiniao: txtOpiniao },
                success: function () {
                    siac.aviso('Opinião registrada com sucesso!', 'green');
                },
                error: function () {
                    siac.aviso('Ocorreu um erro na tentativa de registrar opinião. Por favor, tente novamente mais tarde.', 'red');
                },
                complete: function () {
                    $('#txtOpiniao').val('');
                }
            });
        }
    }

    return {
        iniciar: iniciar
    }
})();