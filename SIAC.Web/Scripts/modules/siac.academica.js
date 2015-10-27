siac.Academica = siac.Academica || {};

siac.Academica.Agendar = (function () {
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

        if (!$('#ddlTurma').val()) {
            lstErro.append('<li>Selecione uma turma</li>');
            retorno = false;
        }

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

siac.Academica.Detalhe = (function () {
    var _codAvaliacao;

    function iniciar() {
        $elemento = $('[data-avaliacao]');
        _codAvaliacao = $elemento.attr('data-avaliacao');
        $elemento.removeAttr('data-avaliacao');

        $('.ui.accordion').accordion({
            animateChildren: false,
            onOpen: function () {
                $content = $('.questao.content.active');
                if ($content) {
                    var ctx = $content.find('canvas').get(0).getContext("2d");
                    var data = JSON.parse($content.find('code.dados').html());
                    var chart = new Chart(ctx).Doughnut(data);
                }
            }
        });

        $('.arquivar.button').click(function () {
            var $_this = $(this);
            $_this.addClass('loading');
            $.ajax({
                url: '/Dashboard/Avaliacao/Academica/Arquivar/' + _codAvaliacao,
                type: 'POST',
                success: function (data) {
                    if (data) {
                        $_this.addClass('active').text('Arquivada');
                    }
                    else {
                        $_this.removeClass('active').text('Arquivar');
                    }
                },
                error: function () {
                    siac.mensagem('Ocorreu um erro inesperado na tentativa de arquivar a avaliação.', 'Tente novamente')
                },
                complete: function () {
                    $_this.removeClass('loading');
                }
            });
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Academica.Resultado = (function () {
    function iniciar() {
        $('.ui.accordion').accordion({ animateChildren: false });
        $('.label, div').popup();
    }

    return {
        iniciar: iniciar
    }
})();

siac.Academica.Gerar = (function () {
    function iniciar() {
        $('.ui.dropdown').dropdown();
        $('.ui.modal').modal();
        $('.ui.termo.modal').modal('show');
        $('.cancelar.button').popup({ on: 'click' });
        $('.ui.confirmar.modal')
         .modal({
             onApprove: function () {
                 $('form').submit();
             }
         });

        //<select id="ddlDisciplina" name="ddlDisciplina" class="ui search dropdown" onchange="listarTemas($(this).val())" required>
        $('#ddlDisciplina').change(function () {
            listarTemas(this.value);
        });

        //<select id="ddlTipo" name="ddlTipo" class="ui search dropdown" onchange="OpcoesPorTipo($(this).val())" required>
        $('#ddlTipo').change(function () {
            mostrarOpcoesPorTipo(this.value);
        });

        //<div onclick="prosseguir()" class="ui prosseguir button">Prosseguir</div>
        $('.prosseguir.button').click(function () {
            prosseguir();
        });
    }

    function prosseguir() {
        var $errorList = $('form .error.message .list');

        $errorList.html('');
        $('form').removeClass('error');

        var valido = true;

        if (!$('#ddlDisciplina').val()) {
            $errorList.append('<li>Selecione uma disciplina</li>');
            valido = false;
        }

        if (!$('#ddlTemas').val()) {
            $errorList.append('<li>Selecione ao menos um tema</li>');
            valido = false;
        }

        if (!$('#ddlTipo').val()) {
            $errorList.append('<li>Selecione o tipo das questões da sua avaliação acadêmica</li>');
            valido = false;
        }

        if (($('#ddlTipo').val() == 1 || $('#ddlTipo').val() == 3) && !$('#txtQteObjetiva').val()) {
            $errorList.append('<li>Indique a quantidade das questões objetivas</li>');
            valido = false;
        }
        if (($('#ddlTipo').val() == 2 || $('#ddlTipo').val() == 3) && !$('#txtQteDiscursiva').val()) {
            $errorList.append('<li>Indique a quantidade das questões discursivas</li>');
            valido = false;
        }

        if (!$('#ddlDificuldade').val()) {
            $errorList.append('<li>Selecione a dificuldade das questões</li>');
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
        $ddlDisciplina = $('#ddlDisciplina :selected');
        $ddlTipo = $('#ddlTipo');
        $table = $modal.find('tbody').html('');

        $tr = $('<tr></tr>');
        $tdDisciplina = $('<td></td>').html('<b>' + $ddlDisciplina.text() + '</b>');
        $tdTemas = $('<td class="ui labels"></td>');

        $ddlTemas = $('#ddlTemas :selected');
        for (var i = 0; i < $ddlTemas.length; i++) {
            $tdTemas.append($('<div class="ui tag label"></div>').text($ddlTemas.eq(i).text()));
        }
        $tdQteQuestoes = $('<td class="ui labels"></td>');
        if ($ddlTipo.val() == 1 || $ddlTipo.val() == 3) {
            $tdQteQuestoes.append($('<div class="ui label"></div>').html('Objetiva<div class="detail">' + $('#txtQteObjetiva').val() + '</div>'));
        }
        if ($ddlTipo.val() == 2 || $ddlTipo.val() == 3) {
            $tdQteQuestoes.append($('<div class="ui label"></div>').html('Discursiva<div class="detail">' + $('#txtQteDiscursiva').val() + '</div>'));
        }
        $tdDificuldade = $('<td></td>').text($('#ddlDificuldade :selected').text());

        $table.append($tr.append($tdDisciplina).append($tdTemas).append($tdQteQuestoes).append($tdDificuldade));

        $modal.modal('show');
    }

    function listarTemas(codDisciplina) {
        var $ddlTemas = $('#ddlTemas');
        $ddlTemas.parent().addClass('loading');
        $.ajax({
            type: 'POST',
            url: '/Tema/RecuperarTemasPorCodDisciplinaTemQuestao',
            data: { 'codDisciplina': codDisciplina },
            success: function (data) {
                $ddlTemas.html('');
                $ddlTemas.val($ddlTemas.prop('defaultSelected'));
                for (var i = 0, length = data.length; i < length; i++) {
                    $ddlTemas.append('<option value="' + data[i].CodTema + '">' + data[i].Descricao + '</option>');
                }
                $ddlTemas.parent().removeClass('loading');
            },
            error: function () {
                siac.mensagem("Erro no carregamento de temas.", "Erro");
                $ddlTemas.parent().removeClass('loading');
            }
        });
    }

    function mostrarOpcoesPorTipo(tipoAvaliacao) {
        var txtQteObjetiva = $('#txtQteObjetiva');
        var txtQteDiscursiva = $('#txtQteDiscursiva');

        if (tipoAvaliacao == 1) {
            txtQteObjetiva.prop('disabled', false);
            txtQteDiscursiva.prop('disabled', true);
        }
        else if (tipoAvaliacao == 2) {
            txtQteObjetiva.prop('disabled', true);
            txtQteDiscursiva.prop('disabled', false);
        }
        else {
            txtQteObjetiva.prop('disabled', false);
            txtQteDiscursiva.prop('disabled', false);
        }
    }

    return {
        iniciar: iniciar
    }
})();

siac.Academica.Configurar = (function () {
    var _codAvaliacao;

    function iniciar() {
        $elemento = $('[data-avaliacao]');
        _codAvaliacao = $elemento.attr('data-avaliacao');
        $elemento.removeAttr('data-avaliacao');

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

        $('.imprimir.button').click(function () {
            window.open('/Dashboard/Avaliacao/Academica/Imprimir/' + _codAvaliacao, '_blank');
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
            url: '/Dashboard/Avaliacao/Academica/TrocarQuestao',
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
            url: '/Dashboard/Avaliacao/Academica/Desfazer',
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

siac.Academica.Agendada = (function () {
    var _codAvaliacao, _categoriaUsuario;

    function iniciar() {
        if (!_categoriaUsuario) {
            $elemento = $('[data-categoria]');
            _categoriaUsuario = $elemento.attr('data-categoria');
            $elemento.removeAttr('data-categoria');
        }

        $('.ui.avaliacao.modal').modal({ closable: false });

        $('.card[data-avaliacao]').click(function () {
            detalhe($(this).attr('data-avaliacao'));
        });
    }

    function detalhe(strCodigo) {
        if ($('.ui.avaliacao.modal').prop('id') == strCodigo) {
            $('.ui.avaliacao.modal').modal('show');
        }
        else {
            $('.ui.global.loader').parent().addClass('active');
            $.ajax({
                url: '/Historico/Avaliacao/Academica/Agendada/' + strCodigo,
                method: 'POST',
                success: function (htmlModal) {
                    _codAvaliacao = strCodigo;
                    $('.ui.avaliacao.modal').attr('id', strCodigo).html('').append(htmlModal).modal('show');
                    $('.ui.global.loader').parent().removeClass('active');
                    $('.ui.avaliacao.modal .ui.accordion').accordion({
                        animateChildren: false,
                        onChange: function () { $('.ui.avaliacao.modal').modal('refresh'); }
                    });
                    $('.arquivar.button').click(function () {
                        var $_this = $(this);
                        $_this.addClass('loading');
                        $.ajax({
                            url: '/Dashboard/Avaliacao/Academica/Arquivar/' + _codAvaliacao,
                            type: 'POST',
                            success: function (data) {
                                if (data) {
                                    window.location.href = '/dashboard/avaliacao/academica/detalhe/' + _codAvaliacao;
                                }
                            },
                            error: function () {
                                $_this.removeClass('loading');
                                siac.mensagem('Ocorreu um erro inesperado na tentativa de arquivar a avaliação.', 'Tente novamente')
                            }
                        });
                    });
                    abrirHub();
                    contagemRegressiva(1000);
                },
                error: function (xhr) { siac.mensagem("Ocorreu um erro."); $('.ui.global.loader').parent().removeClass('active'); }
            });
        }
    }

    function contagemRegressiva(intervalo) {
        setTimeout(function () {
            $.ajax({
                type: 'GET',
                url: '/Dashboard/Avaliacao/Academica/ContagemRegressiva',
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
        var acadHub = $.connection.academicaHub;
        if (_categoriaUsuario == 1) {
            acadHub.client.liberar = function (strCodigo) {
                var $modal = $('#' + strCodigo);
                $.ajax({
                    type: 'GET',
                    url: '/Dashboard/Avaliacao/Academica/ContagemRegressiva',
                    data: { codAvaliacao: _codAvaliacao },
                    success: function (data) {
                        alert('O professor liberou a avaliação.');
                        if (data.Tempo == 'Agora' && data.Intervalo == 0 && data.FlagLiberada == true) {
                            $('.iniciar.button').removeClass('disabled').text('Iniciar');
                            $modal.find('#mensagem').html('\
                                        <i class="checkmark icon"></i>\
                                        <div class="content">\
                                            Seu professor liberou a prova\
                                            <div class="sub header">Você já pode iniciar</div>\
                                        </div>'
                            );
                        }
                        else if (data.FlagLiberada == true) {
                            $('.iniciar.button').addClass('disabled');
                            $modal.find('#mensagem').html('\
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
            acadHub.client.bloquear = function (strCodigo) {
                var $modal = $('#' + strCodigo);
                $.ajax({
                    type: 'GET',
                    url: '/Dashboard/Avaliacao/Academica/ContagemRegressiva',
                    data: { codAvaliacao: _codAvaliacao },
                    success: function (data) {
                        if (data.Tempo == 'Agora' && data.Intervalo == 0 && data.FlagLiberada == false) {
                            alert('O professor bloqueou a avaliação.');
                            $('.iniciar.button').addClass('disabled');
                            $modal.find('#mensagem').html('\
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
                acadHub.server.realizar(_codAvaliacao);
            });
        }
        else if (_categoriaUsuario == 2) {
            $.connection.hub.start().done(function () {
                $('.liberar.button').click(function () {
                    $('.liberar.button').addClass('loading');
                    $.ajax({
                        url: '/Dashboard/Avaliacao/Academica/AlternarLiberar',
                        type: 'POST',
                        data: { codAvaliacao: _codAvaliacao },
                        success: function (data) {
                            if (data == true) {
                                acadHub.server.liberar(_codAvaliacao, true);
                                $('.liberar.button').addClass('active').removeClass('loading').text('Liberada');
                                $.ajax({
                                    type: 'GET',
                                    url: '/Dashboard/Avaliacao/Academica/ContagemRegressiva',
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
                                acadHub.server.liberar(_codAvaliacao, false);
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

siac.Academica.Realizar = (function () {
    var _codAvaliacao, _matriculaUsuario, _dtTermino;
    var _controleInterval, _controleRestante;

    function iniciar() {
        var $elemento;

        $elemento = $('[data-avaliacao][data-usuario]');
        _codAvaliacao = $elemento.attr('data-avaliacao');
        _matriculaUsuario = $elemento.attr('data-usuario');
        $elemento.removeAttr('data-avaliacao data-usuario');

        $elemento = $('[data-termino]');
        _dtTermino = new Date();
        _dtTermino.setTime(Date.parse($elemento.attr('data-termino')));
        $elemento.removeAttr('data-termino');

        window.onbeforeunload = function () {
            return 'Você está realizando uma avaliação.';
        };

        setInterval(function () {
            $.ajax({
                type: 'GET',
                url: '/Acesso/Conectado'
            });
        }, 1000 * 60 * 15);

        $('a[href]').on('click', function () {
            $('.ui.confirmar.modal').modal('show');
            $('.ui.confirmar.modal #txtRef').val($(this).attr('href'));
            return false;
        });

        $('.ui.checkbox')
            .checkbox()
        ;

        $('.ui.informacoes.modal')
            .modal()
        ;

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        $('.ui.confirmar.modal')
            .modal({
                onApprove: function () {
                    desistir($('.ui.confirmar.modal #txtRef').val());
                },
                onDeny: function () {
                    $('.ui.segment.loading').removeClass('loading');
                }
            })
        ;

        $('.ui.anexo.modal')
            .modal()
        ;

        $('.ui.accordion')
            .accordion({
                animateChildren: false
            })
        ;

        $('.trigger.button')
            .popup({
                inline: true,
                on: 'click'
            })
        ;

        $('.ui.gabarito.modal')
            .modal({
                onApprove: function () {
                    $.ajax({
                        type: 'GET',
                        url: "/Acesso/Conectado",
                        success: function () {
                            finalizar();
                        },
                        error: function () {
                            siac.mensagem('Conecte-se à internet antes de confirmar.');
                        }
                    });
                    return false;
                }
            })
        ;

        $('.anexo.imagem.card').click(function () {
            expandirImagem(this);
        });

        $('.desistir.button').click(function () {
            desistir();
        });

        var date = new Date();
        $('#lblHoraInicio').text(date.getHours() + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min');
        $('#lblHoraTermino').text(_dtTermino.getHours() + 'h' + ("0" + (_dtTermino.getMinutes())).slice(-2) + 'min');

        relogio();
        temporizador(_dtTermino);
        conectarHub(_codAvaliacao, _matriculaUsuario);
    }

    function expandirImagem(card) {
        var $card = $(card);
        var src = $card.find('img').attr('src');
        var legenda = $card.find('.header').text();
        var fonte = $card.find('.description').text();
        $modal = $('.ui.anexo.modal');

        $modal.find('.header').text(legenda);
        $modal.find('img.image').attr('src', src);
        $modal.find('.description').html(fonte);

        $modal.modal('show');
    }

    function relogio() {
        setInterval(function () {
            date = new Date();
            $('#lblHoraAgora').text(date.getHours() + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min');
        }, 1000);
    }

    function temporizador(dtTermino) {
        setInterval(function () {
            var offset = dtTermino.getTimezoneOffset() * 60 * 1000;
            var timeRestante = (dtTermino.getTime() + offset) - (new Date().getTime() + offset);

            if (timeRestante > 0) {
                var date = new Date();
                date.setTime(timeRestante);
                var offsetDate = date.getTimezoneOffset() * 60 * 1000;
                date.setTime(date.getTime() + offsetDate);
                var txtRestante = ("0" + date.getHours()).slice(-2) + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min';
                $('#lblHoraRestante').text(txtRestante);
                if (txtRestante != _controleRestante) {
                    $('#lblHoraRestante').parent().transition('flash');
                }
                _controleRestante = txtRestante;
                if (timeRestante < 1000 * 60 * 5 && !$('#lblHoraRestante').parent().hasClass('red')) {
                    $('#lblHoraRestante').parent().addClass('red');
                }
            }
            else {
                alert('O tempo de aplicação acabou, sua prova será enviada automaticamente.');
                $('.ui.global.loader').parent().addClass('active');
                finalizar();
            }
        }, 1000);
    }

    function finalizar() {
        window.onbeforeunload = function () {
            $('.ui.global.loader').parent().addClass('active');
        };
        $('form').submit();
    }

    function verificar() {
        $Objects = $('textarea[name^="txtResposta"], input[name^="rdoResposta"]');
        var retorno = true;
        for (var i = 0, length = $Objects.length; i < length; i++) {
            var _this = $Objects.eq(i);
            $label = _this.parents('.content').prev().find('.ui.label');
            if (_this.attr('name').indexOf('rdo') > -1) {
                if ($('input[name="' + _this.attr('name') + '"]:checked').length === 0) {
                    $label.removeAttr('style').addClass('red').html('Não respondida').transition('tada');
                    retorno = false;
                }
            }
            else if (!_this.val()) {
                $label.removeAttr('style').addClass('red').html('Não respondida').transition('tada');
                retorno = false;
            }
        }
        return retorno;
    }

    function confirmar() {
        $modal = $('.ui.gabarito.modal');

        $accordion = $('form .ui.accordion').clone();

        $accordion.removeAttr('style');

        $modal.find('.content').html($('<div class="ui form"></div>').append($accordion));

        $modalAccordion = $modal.find('.ui.accordion');

        $modalAccordion.accordion({
            onChange: function () {
                $('.ui.gabarito.modal').modal('refresh');
            },
            animateChildren: false
        });

        $lstInput = $modalAccordion.find(':input,a');

        for (var i = 0, length = $lstInput.length; i < length; i++) {
            $lstInput.eq(i)
                .attr({
                    'readonly': 'readonly'
                })
                .removeAttr('href onchange onclick')
                .off()
            ;
        }

        $modal.modal('show');
    }

    function desistir(url) {
        $('.ui.global.loader').parent().addClass('active');
        $.ajax({
            url: '/Dashboard/Avaliacao/Academica/Desistir/' + _codAvaliacao,
            type: 'POST',
            success: function () {
                window.onbeforeunload = function () {
                    $('.ui.global.loader').parent().addClass('active');
                };
                if (!url) {
                    url = '/Dashboard';
                }
                window.location.href = url;
            },
            error: function () {
                $('.ui.global.loader').parent().removeClass('active');
                siac.mensagem('Ocorreu um erro na tentativa de desistência');
            }
        });
    }

    function conectarHub(avalAcad, usrMatr) {
        var acadHub = $.connection.academicaHub;
        $.connection.hub.start().done(function () {
            acadHub.server.alunoConectou(avalAcad, usrMatr);

            finalizar = function () {
                window.onbeforeunload = function () {
                    $('.ui.global.loader').parent().addClass('active');
                };
                acadHub.server.alunoFinalizou(avalAcad, usrMatr);
                $('form').submit();

            };

            $('.ui.continuar.modal')
                .modal({
                    onApprove: function () {
                        acadHub.server.alunoVerificando(avalAcad, usrMatr);
                        confirmar();
                    },
                    onDeny: function () {
                        $('html, body').animate({
                            scrollTop: $(".title .label.red").offset().top
                        }, 1000);
                    }
                })
            ;

            $('.finalizar.button').click(function () {
                if (verificar()) {
                    acadHub.server.alunoVerificando(avalAcad, usrMatr);
                    confirmar();
                }
                else {
                    $('.continuar.modal').modal('show');
                }
            });

            enviarMsg = function enviarMsg(_this) {
                $msg = $('#txtChatMensagem');
                $msg.val($msg.val().trim())
                if ($msg.val()) {
                    var mensagem = $msg.val();
                    mensagem = siac.Utilitario.quebrarLinhaEm(mensagem, 30);
                    acadHub.server.chatAlunoEnvia(avalAcad, usrMatr, $msg.val());
                    $('.chat.popup .content .comments').append('\
                            <div class="comment" style="float:right;clear:both;">\
                                <div class="content">\
                                <div class="ui right pointing label">\
                                    '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');
                    $msg.val('');
                    var $comments = $('.chat.popup .content .comments');
                    $($comments).animate({
                        scrollTop: $comments.children().last().offset().top
                    }, 100);
                }
            };
            $('.enviar.icon').on('click', function () { enviarMsg(this) });
            $('#txtChatMensagem').keypress(function (e) {
                if (e.which == 13) {
                    enviarMsg(this);
                    return false;
                }
            });

            $('textarea[name^="txtResposta"], input[name^="rdoResposta"]').change(function () {
                $label = $(this).parents('.content').prev().find('.ui.label');
                if ($(this).val()) {
                    $label.removeClass('red');
                    $label.removeAttr('style');
                    $label.html('Respondida');
                    var questao;
                    if ($(this).attr('name').indexOf('rdo') > -1) {
                        questao = $(this).attr('name').split('rdoResposta')[1];
                        $label.find('.detail').remove();
                        $label.append($('<div class="detail"></div>').text($('input[name="' + $(this).attr('name') + '"]:checked').next().find('b').text()));
                    }
                    else {
                        questao = $(this).attr('name').split('txtResposta')[1];
                    }
                    acadHub.server.responderQuestao(avalAcad, usrMatr, questao, true);
                }
                else {
                    var questao;
                    if ($(this).attr('name').indexOf('rdo') > -1) {
                        questao = $(this).attr('name').split('rdoResposta')[1];
                    }
                    else {
                        questao = $(this).attr('name').split('txtResposta')[1];
                    }
                    acadHub.server.responderQuestao(avalAcad, usrMatr, questao, false);
                    $label.attr('style', 'display:none');
                }
            });

            $(window).on("blur focus", function (e) {
                var prevType = $(this).data("prevType");

                if (prevType != e.type) {
                    switch (e.type) {
                        case "blur":
                            acadHub.server.focoAvaliacao(avalAcad, usrMatr, false);
                            break;
                        case "focus":
                            acadHub.server.focoAvaliacao(avalAcad, usrMatr, true);
                            break;
                    }
                }

                $(this).data("prevType", e.type);
            })
        });

        acadHub.client.alertar = function (mensagem) {
            var timeAntes = new Date();
            alert(mensagem);
            var timeDepois = new Date();
            if ((timeDepois - timeAntes) < 350) {
                siac.mensagem(mensagem, 'O professor disse...');
            }
        }

        acadHub.client.enviarAval = function (codAvaliacao) {
            html2canvas(document.body, {
                onrendered: function (canvas) {
                    var c = $(canvas);
                    c.attr('id', 'mycanvas').hide();
                    $('body').append(c);
                    var canvas = document.getElementById("mycanvas");
                    data = canvas.toDataURL("image/png");
                    $.ajax({
                        type: 'POST',
                        url: '/Dashboard/Avaliacao/Academica/Printar',
                        data: {
                            codAvaliacao: avalAcad,
                            imageData: data
                        },
                        success: function () {
                            acadHub.server.avalEnviada(codAvaliacao, usrMatr);
                        },
                        error: function () {
                            // adicionar erro ao Feed do professor
                        }
                    });

                    c.remove();
                }
            });
        };

        var chatQteMensagem = 0;
        acadHub.client.chatAlunoRecebe = function (mensagem) {
            if (mensagem) {
                mensagem = siac.Utilitario.quebrarLinhaEm(mensagem, 30);
                $('.chat.popup .comments').append('\
                            <div class="comment" style="float:left;clear:both;">\
                                <div class="content">\
                                <div class="ui left pointing label">\
                                    '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');
                var $comments = $('.chat.popup .comments');
                $($comments).animate({
                    scrollTop: $comments.children().last().offset().top
                }, 0);

                if (!$('.chat.popup').hasClass('visible')) {
                    $btnChat = $('.icon.chat.button');
                    $lblQteMsg = $('#lblQteMsg');
                    $lblQteMsg.remove();
                    chatQteMensagem++;
                    $btnChat.addClass('blue').html('<i class="icon comments outline"></i> ' + chatQteMensagem);
                    document.title = "Você recebeu uma mensagem - SIAC";
                    if (chatQteMensagem > 1) {
                        document.title = "Você recebeu " + chatQteMensagem + " mensagens - SIAC";
                    }
                }
            }
        };
        $('.icon.chat.button').on('click', function () {
            chatQteMensagem = 0;
            $('#lblQteMsg').remove();
            $(this).removeClass('blue').html('<i class="icon comments outline"></i>');
            document.title = 'Realizar ' + _codAvaliacao;
        });

    }

    return {
        iniciar: iniciar
    }
})();

siac.Academica.Acompanhar = (function () {
    var _codAvaliacao, _matriculaUsuario;

    function iniciar() {
        $elemento = $('[data-avaliacao][data-usuario]');
        _codAvaliacao = $elemento.attr('data-avaliacao');
        _matriculaUsuario = $elemento.attr('data-usuario');
        $elemento.removeAttr('data-avaliacao data-usuario');

        conectarHub(_codAvaliacao, _matriculaUsuario);

        $('.ui.accordion')
            .accordion({
                animateChildren: false
            })
        ;

        $('.ui.progress')
            .progress({
                label: 'ratio',
                text: {
                    ratio: '{value} de {total}'
                }
            })
        ;

        $('.trigger.button')
            .popup({
                inline: true,
                on: 'click'
            })
        ;
    }

    function conectarHub(avalAcad, usrMatr) {
        var acadHub = $.connection.academicaHub;
        $.connection.hub.start().done(function () {

            enviarMsg = function enviarMsg(_this) {
                $content = $(_this).parents('.content[id]');
                matr = $content.attr('id');
                $msg = $('#' + matr + 'msg');
                $msg.val($msg.val().trim());
                if ($msg.val()) {
                    var mensagem = $msg.val();
                    mensagem = siac.Utilitario.quebrarLinhaEm(mensagem, 30);
                    acadHub.server.chatProfessorEnvia(avalAcad, matr, $msg.val());
                    $('#' + matr + '.content .chat.popup .comments').append('\
                            <div class="comment" style="float:right;clear:both;">\
                                <div class="content">\
                                <div class="ui right pointing label">\
                                    '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');
                    $msg.val('');
                    var $comments = $content.find('.comments');
                    $($comments).animate({
                        scrollTop: $comments.children().last().offset().top
                    }, 0);
                }
            };

            acadHub.server.professorConectou(avalAcad, usrMatr);
            $('.prova.button').on('click', function () {
                matr = $(this).parent().attr('id');
                acadHub.server.requererAval(avalAcad, matr);
                $('#' + matr + '.content .prova.button').addClass('loading').removeClass('transition visible');
            });

            $('.alertar.button').on('click', function () {
                mensagem = $(this).prev().find('textarea').val();
                matr = $(this).parent().parent().parent().attr('id');
                acadHub.server.alertar(avalAcad, mensagem, matr);
                $(this).prev().find('textarea').val('');
                $(this).popup('hide all');
            });

            $('.enviar.icon').on('click', function () { enviarMsg(this) });
            $('.chat input').keypress(function (e) {
                if (e.which == 13) {
                    enviarMsg(this);
                    return false;
                }
            });


            setInterval(function () {
                lstMatr = $('.accordion')
                            .find('.title')
                            .map(function () {
                                acadHub.server.feed(avalAcad, this.id);
                            });
            }, 3000);
        });

        acadHub.client.chatProfessorRecebe = function (usrMatricula, mensagem) {
            $content = $('#' + usrMatricula + '.content[id]');
            if (mensagem) {
                mensagem = siac.Utilitario.quebrarLinhaEm(mensagem, 30);
                $('#' + usrMatricula + '.content .chat.popup .comments').append('\
                            <div class="comment" style="float:left;clear:both;">\
                                <div class="content">\
                                <div class="ui left pointing label">\
                                    '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');
                var $comments = $content.find('.comments');
                $($comments).animate({
                    scrollTop: $comments.children().last().offset().top
                }, 0);

                chatQteMensagem = 0;
                if ($('#' + usrMatricula + 'lblQteMsg').length) { chatQteMensagem = $('#' + usrMatricula + 'lblQteMsg').data('qte') }
                if (!$('#' + usrMatricula + '.content .chat.popup').hasClass('visible')) {
                    $accordionChat = $('#' + usrMatricula + '.title');
                    $lblQteMsg = $('#' + usrMatricula + 'lblQteMsg');
                    $lblQteMsg.remove();
                    $accordionChat.append($('<div></div>').attr('id', usrMatricula + 'lblQteMsg').addClass('ui small blue label').append('<i class="ui comments icon"></i>'));
                    chatQteMensagem++;
                    $('#' + usrMatricula + 'lblQteMsg').data('qte', chatQteMensagem).append(chatQteMensagem);
                }
            }
        };

        $('.icon.chat.button').on('click', function () {
            matr = ($(this).parents('.content[id]').attr('id'));
            $('#' + matr + 'lblQteMsg').remove();
        });

        acadHub.client.atualizarFeed = function (alnMatricula, lstEvento) {
            $feed = $('#' + alnMatricula + '.content .feed');
            $feed.html('');
            $feed.append('<h4 class="ui header">Atividade</h4>');
            if (lstEvento) {
                for (var i = lstEvento.length - 1; i > -1; i--) {
                    $feed.append('\
                        <div class="event">\
                            <div class="label">\
                                <i class="'+ lstEvento[i].Icone + ' icon"></i>\
                            </div>\
                            <div class="content">\
                                <div class="summary">\
                                    ' + lstEvento[i].Descricao + '\
                                    <div class="date" title="'+ lstEvento[i].DataCompleta + '">' + lstEvento[i].Data + '</div>\
                                </div>\
                            </div>\
                        </div>\
                    ');
                }

                $('#' + alnMatricula + 'lblInfo').remove();
                if (lstEvento[lstEvento.length - 1].Icone == "red warning sign" && !$('#' + alnMatricula + 'lblWarning').length && !$('#' + alnMatricula).hasClass('active')) {
                    $accordionChat = $('#' + alnMatricula + '.title');
                    $accordionChat.append($('<i></i>').attr('id', alnMatricula + 'lblWarning').addClass('red warning sign icon'));
                }

                else if (!$('#' + alnMatricula + 'lblWarning').length) {
                    $accordionChat = $('#' + alnMatricula + '.title');
                    $accordionChat.append($('<i></i>').attr('id', alnMatricula + 'lblInfo').addClass(lstEvento[lstEvento.length - 1].Icone + ' icon'));
                }
            }
        }

        $('.accordion .title').on('click', function () {
            matr = ($(this).attr('id'));
            $('#' + matr + 'lblWarning').remove();
        });

        acadHub.client.conectarAluno = function (usrMatricula) {
            $('#' + usrMatricula + '.title .small.label').remove();
            $('#' + usrMatricula + '.title .status.label').removeClass('red').addClass('green');
            $('#' + usrMatricula + '.content .button').removeClass('disabled');
        };

        acadHub.client.desconectarAluno = function (usrMatricula) {
            $('#' + usrMatricula + '.title').append('<div class="ui small label">Desconectado</div>');
            $('#' + usrMatricula + '.title .status.label').removeClass('green');
            $('#' + usrMatricula + '.content .button').addClass('disabled');
        };

        acadHub.client.alunoFinalizou = function (usrMatricula) {
            $('#' + usrMatricula + '.title').append('<div class="ui small label">Finalizou</div>');
            $('#' + usrMatricula + '.title .status.label').removeClass('green').addClass('red');
            $('#' + usrMatricula + '.content .button').addClass('disabled');
        };

        acadHub.client.receberAval = function (alnMatricula) {
            $.ajax({
                type: 'POST',
                url: '/Dashboard/Avaliacao/Academica/Printar',
                data: {
                    codAvaliacao: avalAcad
                },
                success: function (data) {
                    $('.printscreen.modal .header').text($('#' + alnMatricula + ' .nome').text() + ' (' + alnMatricula + ')');
                    $('.printscreen.modal .content').css({
                        'background-image': 'url(\'' + data + '\')'
                    });
                    $('.printscreen.modal .nova.guia.button').attr('href', data);
                    $('.printscreen.modal').modal('show').modal('refresh');
                    $('#' + alnMatricula + '.content .prova.button').removeClass('loading');
                },
                error: function () {
                    siac.mensagem('Ocorreu um erro inesperado. Tente novamente mais tarde.');
                }
            });
        };

        acadHub.client.atualizarProgresso = function (alnMatricula, value) {
            if (value > 0) {
                $('#' + alnMatricula + '.content .progress')
                    .progress({
                        value: value,
                        label: 'ratio',
                        text: {
                            ratio: '{value} de {total}'
                        }
                    })
                ;
            }
        }

        acadHub.client.respondeuQuestao = function (alnMatricula, questao, flag) {
            $questao = $('#' + alnMatricula + '.content [data-questao="' + questao + '"]');
            $questao.removeClass('positive').find('i').remove();
            if (flag) {
                $questao.addClass('green');
            }
        }

        acadHub.client.listarChat = function (alnMatricula, mensagens) {
            for (var i = 0, length = mensagens.length; i < length; i++) {
                if (mensagens[i].FlagAutor) {
                    acadHub.client.chatProfessorRecebe(alnMatricula, mensagens[i].Texto);
                }
                else {
                    var mensagem = mensagens[i].Texto;
                    mensagem = siac.Utilitario.quebrarLinhaEm(mensagem, 30);
                    $('#' + alnMatricula + '.content .chat.popup .comments').append('\
                        <div class="comment" style="float:right;clear:both;">\
                            <div class="content">\
                            <div class="ui right pointing label">\
                                '+ mensagem + '\
                                </div>\
                            </div>\
                        </div>\
                        ');

                    var $comments = $('#' + alnMatricula + '.content .comments');
                    $($comments).animate({
                        scrollTop: $comments.children().last().offset().top
                    }, 0);
                }
            }
        }
    }

    return {
        iniciar: iniciar
    }
})();

siac.Academica.Index = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 12;

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
        $cards = $('.ui.cards');
        $cards.parent().addClass('loading');
        $.ajax({
            url: '/Dashboard/Avaliacao/Academica/Listar',
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

siac.Academica.Corrigir = (function () {
    var _dicQuestoes = {};
    var _codAvaliacao;

    function iniciar() {
        var $elemento = $('[data-avaliacao]');
        _codAvaliacao = $elemento.attr('data-avaliacao');
        $elemento.removeAttr('data-avaliacao');

        var $elementoJson = $('code.questoes');
        _dicQuestoes = JSON.parse($elementoJson.html());
        $elementoJson.remove();

        $('.ui.accordion').accordion({ animateChildren: false });
        $('.ui.modal').modal();
        $('.ui.dropdown').dropdown();

        $('.ui.button.informacoes').click(function () {
            $('.modal.informacoes').modal('show');
        });

        $('.ui.button.corrigir').click(function () {
            $('.modal.corrigir').modal('show');
        });

        $('.ui.dimmer').css('z-index', 1);

        $('#ddlCorrecaoModo').change(function () {
            var modo = $(this).val();
            $ddlCorrecaoValor = $('#ddlCorrecaoValor');
            $ddlCorrecaoValor.parent().addClass('loading');
            $('.correcao.conteudo').html('');
            $ddlCorrecaoValor.dropdown('set placeholder text', 'Selecione...');

            if (modo == 'aluno') {
                $.ajax({
                    cache: true,
                    type: 'POST',
                    url: '/Dashboard/Avaliacao/Academica/CarregarAlunos/' + _codAvaliacao,
                    success: function (data) {
                        $ddlCorrecaoValor.html('<option value="">Selecione o aluno</option>');
                        $ddlCorrecaoValor.parents('.field').find('label').text('Selecione o aluno');
                        for (i = 0, length = data.length; i < length; i++) {
                            $ddlCorrecaoValor.append('<option value="' + data[i].Matricula + '">' + data[i].Nome + '</option>');
                        }
                        $ddlCorrecaoValor.parent().removeClass('loading').removeClass('disabled');
                        $ddlCorrecaoValor.dropdown('set selected', -1);

                    },
                    error: function () {
                        siac.mensagem('Ocorreu um erro.');
                        $ddlCorrecaoValor.parent().removeClass('loading');
                    }
                });
            }
            else if (modo == 'questao') {
                $.ajax({
                    cache: true,
                    type: 'POST',
                    url: '/Dashboard/Avaliacao/Academica/CarregarQuestoesDiscursivas/' + _codAvaliacao,
                    success: function (data) {
                        $ddlCorrecaoValor.parents('.field').find('label').text('Selecione a questão');
                        $ddlCorrecaoValor.html('');
                        $ddlCorrecaoValor.append('<option value="">Selecione a questão</option>');
                        for (i = 0, length = data.length; i < length; i++) {
                            $ddlCorrecaoValor.append('<option value="' + data[i].codQuestao + '">' + getIndiceQuestao(data[i].codQuestao) + '. ' + siac.Utilitario.encurtarTextoEm(data[i].questaoEnunciado, 80) + '</option>');
                        }
                        $ddlCorrecaoValor.parent().removeClass('loading').removeClass('disabled');
                    },
                    error: function () {
                        siac.mensagem('Ocorreu um erro.');
                        $ddlCorrecaoValor.parent().removeClass('loading');
                    }
                });
            }
        });

        $('#ddlCorrecaoValor').change(function () {
            var _this = this;
            $modal = $(_this).parents('.modal')
            var modo = $('#ddlCorrecaoModo').val();
            var valor = $(_this).val();
            if (valor) {
                $conteudo = $('.correcao.conteudo');
                $(_this).parent().addClass('loading');
                if (modo == 'aluno') {
                    $conteudoQuestao = $('#templateCorrecaoAluno');
                    $.ajax({
                        type: 'POST',
                        url: '/Dashboard/Avaliacao/Academica/CarregarRespostasDiscursivas/' + _codAvaliacao,
                        data: {
                            matrAluno: valor
                        },
                        success: function (data) {
                            $('.correcao.conteudo').html('');
                            for (i = 0, length = data.length; i < length; i++) {
                                $conteudoQuestaoClone = $conteudoQuestao.clone();
                                $conteudoQuestaoClone.removeAttr('id').removeAttr('hidden');
                                $conteudoQuestaoClone.html($conteudoQuestao.html());

                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{matrAluno}', valor));
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{codQuestao}', data[i].codQuestao));
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{questaoEnunciado}', data[i].questaoEnunciado));
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{questaoIndice}', getIndiceQuestao(data[i].codQuestao)));
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{questaoChaveResposta}', data[i].questaoChaveResposta));
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{alunoResposta}', data[i].alunoResposta));
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{correcaoComentario}', data[i].correcaoComentario));

                                $conteudo.append($conteudoQuestaoClone);

                                if (data[i].flagCorrigida) {
                                    $conteudo.append($conteudoQuestaoClone);
                                    $conteudoQuestaoClone.find('.dimmer').dimmer('show');
                                    $conteudoQuestaoClone.find('.notaObtida').val(data[i].notaObtida);
                                }
                                else {
                                    $conteudo.prepend($conteudoQuestaoClone);
                                }
                            }

                            $modal.modal('refresh');
                            $(_this).parent().removeClass('loading');

                            $('.button.corrigir.aluno').click(function () {
                                corrigirQuestao(this);
                            });
                        },
                        error: function () {
                            siac.mensagem('Ocorreu um erro.');
                            $(_this).parent().removeClass('loading');
                        }
                    });
                }
                else if (modo == 'questao') {
                    $conteudoQuestao = $('#templateCorrecaoQuestao');
                    $.ajax({
                        type: 'POST',
                        url: '/Dashboard/Avaliacao/Academica/CarregarRespostasPorQuestao/' + _codAvaliacao,
                        data: {
                            codQuestao: valor
                        },
                        success: function (data) {
                            $('.correcao.conteudo').html('');
                            if (data) {
                                $conteudoQuestaoClone = $conteudoQuestao.clone().removeAttr('id').removeAttr('hidden');;
                                $conteudoQuestaoClone.find('table').remove();
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{questaoEnunciado}', data[0].questaoEnunciado));
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{questaoIndice}', getIndiceQuestao(data[0].codQuestao)));
                                $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{questaoChaveResposta}', data[0].questaoChaveResposta));
                                var $conteudoQuestaoTemp = $conteudoQuestaoClone;

                                for (i = 0, length = data.length; i < length; i++) {
                                    $conteudoQuestaoClone = $conteudoQuestao.clone();
                                    $conteudoQuestaoClone.removeAttr('id').removeAttr('hidden');
                                    $conteudoQuestaoClone.html($conteudoQuestaoClone.find('table').parent()).attr('id', 'aln' + data[i].alunoMatricula);

                                    $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{matrAluno}', valor));
                                    $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{alunoNome}', data[i].alunoNome));
                                    $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{codQuestao}', data[i].codQuestao));
                                    $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{alunoResposta}', data[i].alunoResposta));
                                    $conteudoQuestaoClone.html(siac.Utilitario.substituirTodos($conteudoQuestaoClone.html(), '{correcaoComentario}', data[i].correcaoComentario));

                                    if (data[i].flagCorrigida) {
                                        $conteudo.append($conteudoQuestaoClone);
                                        $conteudoQuestaoClone.find('.dimmer').dimmer('show');
                                        $conteudoQuestaoClone.find('.notaObtida').val(data[i].notaObtida);
                                    }
                                    else {
                                        $conteudo.prepend($conteudoQuestaoClone);
                                    }
                                }

                                $conteudo.prepend($conteudoQuestaoTemp);

                                $modal.modal('refresh');
                                $(_this).parent().removeClass('loading');

                                $('.button.corrigir.aluno').click(function () {
                                    corrigirQuestao(this);
                                });
                            }
                        },
                        error: function () {
                            siac.mensagem('Ocorreu um erro.');
                            $(_this).parent().removeClass('loading');
                        }
                    });
                }
            }
        });
    }

    function getIndiceQuestao(codQuestao) {
        return _dicQuestoes[codQuestao];
    }

    function corrigirQuestao(_this) {
        modo = $('#ddlCorrecaoModo').val();

        matrAluno = $('#ddlCorrecaoValor').val();
        codQuestao = $(_this).parents('[id]').attr('id');
        id = codQuestao;

        if (modo == "questao") {
            matrAluno = $(_this).parents('[id]').attr('id');;
            codQuestao = $('#ddlCorrecaoValor').val();
            id = matrAluno;
            matrAluno = matrAluno.split('aln')[1];
        }
        notaObtida = $('#' + id + ' .notaObtida').val();
        correcaoComentario = $('#' + id + ' .correcaoComentario').val();
        $('#' + id + ' .button.corrigir.aluno').addClass('loading');

        $.ajax({
            type: 'POST',
            url: '/Dashboard/Avaliacao/Academica/CorrigirQuestaoAluno/' + _codAvaliacao,
            data: {
                matrAluno: matrAluno,
                codQuestao: codQuestao,
                notaObtida: notaObtida,
                profObservacao: correcaoComentario
            },
            success: function (data) {
                $('#' + id + ' .button.corrigir.aluno').removeClass('loading').addClass('disabled');
                $('#' + id).find('.segment').dimmer('show');
            },
            error: function (data) {
                siac.mensagem(data);
                $('#' + id + ' .button.corrigir.aluno').removeClass('loading');
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();