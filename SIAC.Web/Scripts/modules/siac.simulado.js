siac.Simulado = siac.Simulado || {};

siac.Simulado.Novo = (function () {
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
        $('#txtModalDescricao').val($('#txtDescricao').val());

        $modal.modal('show');
    }

    return {
        iniciar: iniciar
    }
})();

siac.Simulado.Provas = (function () {
    function iniciar() {
        $('.ui.informacoes.modal').modal();


        //$('.ui.termo.modal').modal({ closable: false }).modal('show');
        //$('.cancelar.button').popup({ on: 'click' });
        //$('.ui.confirmar.modal')
        // .modal({
        //     onApprove: function () {
        //         $('form').submit();
        //     }
        // });

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        //$('.prosseguir.button').click(function () {
        //    prosseguir();
        //});
    }

    function prosseguir() {
        //var $errorList = $('form .error.message .list');

        //$errorList.html('');
        //$('form').removeClass('error');

        //var valido = true;

        //if (!$('#txtTitulo').val().trim()) {
        //    $errorList.append('<li>Insira o título</li>');
        //    valido = false;
        //}

        //if (valido) {
        //    confirmar();
        //}
        //else {
        //    $('form').addClass('error');
        //    $('html, body').animate({
        //        scrollTop: $('form .error.message').offset().top
        //    }, 1000);
        //}
    }

    function confirmar() {
        //$modal = $('.ui.confirmar.modal');

        //$('#txtModalTitulo').val($('#txtTitulo').val());
        //$('#txtModalDescricao').val($('#txtDescricao').val());

        //$modal.modal('show');
    }

    return {
        iniciar: iniciar
    }
})();

siac.Simulado.Datas = (function () {

    function iniciar() {
        $('.ui.informacoes.modal').modal();

        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal').modal({
            onApprove: function () {
                $('form').addClass('loading').submit();
            }
        });

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        $('.confirmar.button').click(function () {
            confirmar();
            return false;
        });
    }

    function validar() {
        var inicioInscricao = $('#txtInicioInscricao').val(),
            terminoInscricao = $('#txtTerminoInscricao').val(),
            qteVagas = $('#txtQteVagas').val(),
            retorno = true;

        lstErro = $('form .error.message .list');
        lstErro.html('');
        $('form').removeClass('error');

        if (!inicioInscricao) {
            lstErro.append('<li>Especifique a data de início das inscrições</li>');
            retorno = false;
        } else {
            var strDate = siac.Utilitario.formatarData(inicioInscricao + 'T00:00:00');
            if (!siac.Utilitario.dataEFuturo(strDate)) {
                lstErro.append('<li>Especifique uma data de início futura</li>');
                retorno = false;
            }
        }

        if (!terminoInscricao) {
            lstErro.append('<li>Especifique a data de término das inscrições</li>');
            retorno = false;
        } else {
            var strDate = siac.Utilitario.formatarData(terminoInscricao + 'T00:00:00');
            if (!siac.Utilitario.dataEFuturo(strDate)) {
                lstErro.append('<li>Especifique uma data de término futura</li>');
                retorno = false;
            }
        }

        if (inicioInscricao && terminoInscricao) {
            var strDateA = inicioInscricao + 'T00:00:00';
            var strDateB = terminoInscricao + 'T00:00:00';
            if (siac.Utilitario.compararData(strDateA, strDateB) >= 0) {
                lstErro.append('<li>Especifique uma data de término posterior à data de início</li>');
                retorno = false;
            }
        }

        if (!qteVagas || qteVagas <= 0) {
            lstErro.append('<li>Especifique a quantidade de vagas das inscrições</li>');
            retorno = false;
        }

        return retorno;
    }

    function confirmar() {
        var $form = $('form');
        if (validar()) {
            var $div = $('<div class="ui form"></div>');
            $div.append($form.html());
            var lstInput = $div.find(':input');
            $div.find('.button').remove();
            $div.find('#txtInicioInscricao').attr('value', $form.find('#txtInicioInscricao').val());
            $div.find('#txtTerminoInscricao').attr('value', $form.find('#txtTerminoInscricao').val());
            $div.find('#txtQteVagas').attr('value', $form.find('#txtQteVagas').val());
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

siac.Simulado.Salas = (function () {
    var _blocos = [],
        _salas = [],
        ddlCampus = $('[name=ddlCampus]'),
        ddlBloco = $('[name=ddlBloco]'),
        ddlSala = $('[name=ddlSala]');

    function iniciar() {
        _blocos = JSON.parse($('script#blocos').html());
        _salas = JSON.parse($('script#salas').html());
        $('script#salas').remove();
        $('script#blocos').remove();

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        $('.ui.dropdown').dropdown();
        $('[data-content]').popup();

        ddlCampus.off('change').change(function () {
            atualizarBlocos()
        });
        ddlBloco.off('change').change(function () {
            atualizarSalas()
        });
    };

    function atualizarBlocos() {
        var codCampus = ddlCampus.val(),
            label = 'Bloco';

        ddlBloco.html('<option value="">'+label+'</option>');

        for (var i = 0, length = _blocos.length; i < length; i++) {
            if (_blocos[i].Campus == codCampus) {
                ddlBloco.append('<option value="' + _blocos[i].CodBloco + '">' + _blocos[i].Descricao + '</option>');
            }
        }

        ddlBloco
            .dropdown('refresh')
            .dropdown('set placeholder text', label)
            .dropdown('set value', '')
        ;

        atualizarSalas();
    };

    function atualizarSalas() {
        var codBloco = ddlBloco.val(),
            label = 'Sala';

        ddlSala.html('<option value="">' + label + '</option>');

        if (codBloco) {
            for (var i = 0, length = _salas.length; i < length; i++) {
                if (_salas[i].CodBloco == codBloco) {
                    ddlSala.append('<option value="' + _salas[i].CodSala + '">' + _salas[i].Descricao + '</option>');
                }
            }
        }

        ddlSala
            .dropdown('refresh')
            .dropdown('set placeholder text', label)
            .dropdown('set value', '')
        ;
    }


    return {
        iniciar: iniciar
    }
})();