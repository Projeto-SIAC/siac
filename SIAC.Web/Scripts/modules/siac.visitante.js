﻿siac.Visitante = siac.Visitante || {};

siac.Visitante.Cadastrar = (function () {
    function iniciar() {
        $('.ui.dropdown').dropdown();
        $('.ui.checkbox').checkbox();
        $('.ui.confirmar.modal').modal({
            onApprove: function () {
                $('form.form').submit();
            }
        });

        $('[name=chkDtValidade]').change(function () {
            $('[name=txtDtValidade]').prop('disabled', !($(this).is(':checked')));
        });

        $('.cancelar.button').popup({
            inline: true,
            on: "click"
        });

        $('.continuar.button').click(function () {
            if (verificar()) {
                continuar();
            }
            else {
                $('form.form').addClass('error');
            }
        });

        $('[name=txtCpf]').change(function () {
            var $this = $(this);
            var txtCpf = $this.val().trim().substituirTodos('.', '').substituirTodos('-', '');
            if (txtCpf) {
                if (siac.Utilitario.validarCPF(txtCpf)) {
                    $.ajax({
                        url: '/configuracoes/visitante/carregarpessoa',
                        type: 'post',
                        data: {
                            cpf: txtCpf
                        },
                        success: function (response) {
                            if (response) {
                                $('[name=txtNome]').val(response.Nome);
                                $('[name=txtDtNasc]').val(response.DtNasc);
                                $('[name=ddlSexo]').dropdown('set selected', response.Sexo);
                            }
                        }
                    });
                }
            }
        });
    }

    function verificar() {
        var $form = $('form.form');
        $form.removeClass('error');
        $form.find('.message').remove();
        var $message = $('<div class="ui error message"></div>');
        $message.append($('<div class="header"></div>').text('Verifique os seguintes erros'));
        var $list = $('<ul class="list"></ul>');

        // Nome
        var txtNome = $('[name=txtNome]').val().trim();
        if (txtNome) {
            if (txtNome.indexOf(' ') < 0) {
                $list.append($('<li></li>').html('Informe o <b>nome completo</b>'));
            }
        }
        else {
            $list.append($('<li></li>').html('Informe o <b>nome</b>'));
        }

        // CPF
        var txtCpf = $('[name=txtCpf]').val().substituirTodos('.', '').substituirTodos('-', '');
        if (txtCpf) {
            if (!siac.Utilitario.validarCPF(txtCpf)) {
                $list.append($('<li></li>').html('Informe um <b>CPF válido</b>'));
            }
        }
        else {
            $list.append($('<li></li>').html('Informe o <b>CPF</b>'));
        }

        // Data de Nascimento
        var txtDtNasc = $('[name=txtDtNasc]').val();
        if (txtDtNasc) {
            if (siac.Utilitario.dataEFuturo(txtDtNasc)) {
                $list.append($('<li></li>').html('Informe uma <b>data de nascimento válida</b>'));
            }
        }

        // Data de Validade
        if ($('[name=chkDtValidade]').is(':checked')) {
            var txtDtValidade = $('[name=txtDtValidade]').val();
            if (!siac.Utilitario.dataEFuturo(txtDtValidade)) {
                $list.append($('<li></li>').html('Informe uma <b>data de validade futura</b>'));
            }
        }

        $message.append($list);

        $form.prepend($message);

        return $list.children().length == 0;
    }

    function continuar() {
        var $form = $('form.form').clone();

        var $modal = $('.confirmar.modal');

        $form.find('.message, .button, .popup').remove();

        $modal.find('.form').html($form.children());

        var lstInput = $modal.find(':input');

        for (var i = 0, length = lstInput.length; i < length; i++) {
            lstInput.eq(i)
                .prop('readonly', true)
                .removeAttr('href onchange onclick id name')
                .off()
            ;
        }

        $modal.modal('show');
    }

    return { iniciar: iniciar }
})();