siac.Visitante = siac.Visitante || {};

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

siac.Visitante.Index = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 10, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var categorias = [];
    var pesquisa = "";

    function iniciar() {
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
            var _categoria = $_this.data('categoria');
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

        $('.ordenar.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            ordenar = $_this.attr('data-ordenar');
            listar();

            $('.ordenar.item').removeClass('active');
            $_this.addClass('active');
        });

        $('.carregar.button').click(function () {
            if ($('.table tbody tr').length == (_controleQte * pagina)) {
                pagina++;
                listar();
            }
        });

        listar();
    };

    function listar() {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        $table = $('.ui.table');
        $table.parent().addClass('loading');
        _controleAjax = $.ajax({
            url: '/configuracoes/visitante/listar',
            data: {
                pagina: pagina,
                ordenar: ordenar,
                categorias: categorias,
                pesquisa: pesquisa
            },
            method: 'POST',
            success: function (partial) {
                if (partial != _controlePartial) {
                    if (pagina == 1) {
                        $table.find('tbody').html(partial);
                    }
                    else {
                        $table.find('tbody').append(partial);
                    }
                    _controlePartial = partial;
                }
            },
            complete: function () {
                $table.parent().removeClass('loading');
                if ($('.table tbody tr').length < (_controleQte * pagina)) {
                    $('.carregar.button').parents('tfoot').hide();
                }
                else {
                    $('.carregar.button').parents('tfoot').show();
                }
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

siac.Visitante.Detalhe = (function () {
    function iniciar() {
        $('.ui.validade.modal').modal({
            onApprove: function () {
                if (verificar()) alterarValidade();
                else {
                    $('.validade.modal .form').addClass('error');
                    return false;
                }
            }
        });
        $('.ui.checkbox').checkbox();
        $('[name=chkDtValidade]').change(function () {
            $('[name=txtDtValidade]').prop('disabled', !($(this).is(':checked')));
        });
        $('.validade.button').click(function () {
            $('.validade.modal').modal('show');
        });
    }

    function verificar() {
        var $form = $('.validade.modal .form');
        $form.removeClass('error');
        $form.find('.message').remove();
        var $message = $('<div class="ui error message"></div>');
        $message.append($('<div class="header"></div>').text('Verifique os seguintes erros'));
        var $list = $('<ul class="list"></ul>');

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

    function alterarValidade() {
        var matricula = window.location.pathname.match(/vis[0-9]+/)[0];
        var chkDtValidade = $('[name=chkDtValidade]').is(':checked');
        var txtDtValidade = $('[name=txtDtValidade]').val();
        $.ajax({
            url: '/configuracoes/visitante/alterarvalidade',
            type: 'post',
            data: {
                matricula: matricula,
                chkDtValidade: chkDtValidade,
                txtDtValidade: txtDtValidade
            },
            success: function () {
                window.location.reload();
            },
            error: function () {
                siac.mensagem('Por favor, recarrega a página e tente novamente.', 'Ocorreu um erro desconhecido');
            },
            notificacoes: false
        });
    }

    return { iniciar: iniciar }
})();