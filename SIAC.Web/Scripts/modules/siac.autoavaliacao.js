siac.Autoavaliacao = siac.Autoavaliacao || {};

siac.Autoavaliacao.Index = (function () {
    var _controleTimeout;

    var pagina = 1;
    var ordenar = "data_desc";
    var categorias = [];
    var dificuldade = "";
    var disciplina = "";
    var pesquisa = "";

    function iniciar() {
        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
                pagina++;
                listar();
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

        $('.dificuldade.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            dificuldade = $_this.attr('data-dificuldade');
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
            url: '/Historico/Autoavaliacao/Listar',
            data: {
                pagina: pagina,
                ordenar: ordenar,
                dificuldade: dificuldade,
                disciplina: disciplina,
                categorias: categorias,
                pesquisa: pesquisa
            },
            method: 'POST',
            success: function (partial) {
                if (pagina == 1) {
                    $cards.html(partial);
                }
                else {
                    $cards.append(partial);
                }
            },
            complete: function () {
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

    function arquivar(strCodigo) {
        $.ajax({
            type: 'POST',
            data: { codigo: strCodigo },
            url: "/Dashboard/Autoavaliacao/Arquivar",
            success: function () {
                window.location.href = '/Historico/Autoavaliacao';
            },
            error: function () {
                siac.mensagem('Não foi possível arquivar a autoavaliação.')
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Autoavaliacao.Detalhe = (function () {
    var _codAvaliacao;

    function iniciar() {
        _codAvaliacao = $('[data-avaliacao]').attr('data-avaliacao');

        $('.ui.accordion').accordion({
            animateChildren: false
        });

        $('.label, div').popup();

        $('.arquivar.button').click(function () {
            var $_this = $(this);
            $_this.addClass('loading');
            $.ajax({
                type: 'POST',
                data: { codigo: _codAvaliacao },
                url: '/Dashboard/Autoavaliacao/Arquivar',
                success: function () {
                    window.location.href = '/Historico/Autoavaliacao/Detalhe/' + _codAvaliacao;
                },
                error: function () {
                    siac.mensagem('Não foi possível arquivar a autoavaliação.')
                },
                complete: function () {
                    $(_this).removeClass('loading');
                }
            });
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Autoavaliacao.Resultado = (function () {
    function iniciar() {
        $('.ui.accordion')
            .accordion({
                animateChildren: false
            })
        ;
        $('.label, div')
            .popup()
        ;
    }

    return {
        iniciar: iniciar
    }
})();

siac.Autoavaliacao.Realizar = (function () {
    var _codAvaliacao;
    var date;
    var duracao;
    var timerId;
    var conectado = false;

    function iniciar() {
        _codAvaliacao = $('[data-avaliacao]').attr('data-avaliacao');

        (function () {
            setInterval(function () {
                $.ajax({
                    type: 'GET',
                    url: '/Acesso/Conectado'
                });
            }, 1000 * 60 * 15);
        })();

        definirTamanhoDiv();

        $('a[href]').on('click', function () {
            $('.ui.confirmar.modal').modal('show');
            $('.ui.confirmar.modal #txtRef').val($(this).attr('href'));
            return false;
        });

        $('.ui.checkbox')
            .checkbox()
        ;

        $('.ui.verificar.modal')
            .modal({
                closable: false,
                transition: 'vertical flip',
                blurring: true,
                onHide: function () {
                    window.onbeforeunload = function () {
                        return 'Você está realizando uma autoavaliação.';
                    };
                }
            })
            .modal('show')
        ;

        $('.ui.cronometrar.modal')
            .modal({
                closable: false,
                onApprove: function () {
                    date = new Date();
                    $('#lblHoraInicio').text(date.getHours() + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min');
                    $('#lblHoraAgora').text(date.getHours() + 'h' + ('0' + (date.getMinutes())).slice(-2) + 'min');
                    setTimeout(relogio(), ((60 - date.getSeconds()) * 1000));
                    if ($('#chkCronometrar').is(':checked')) {
                        duracao = $('#txtDuracao').val();
                        $('#lblHoraDuracao').text(siac.Utilitario.minutosParaStringTempo(duracao));
                        $('#lblHoraRestante').text(siac.Utilitario.minutosParaStringTempo(duracao));
                        temporizador();
                    }
                    else {
                        $('#lblHoraRestante').parent().remove();
                        $('#lblHoraDuracao').parent().remove();
                    }
                }
            })
            .modal('attach events', '.ui.verificar.modal .iniciar.button')
        ;

        $('.ui.informacoes.modal')
            .modal()
        ;

        $('.ui.anexo.modal')
            .modal()
        ;

        $('.ui.confirmar.modal')
            .modal({
                onApprove: function () {
                    window.onbeforeunload = function () {
                        $('.ui.global.loader').parent().addClass('active');
                    };
                    window.location.href = $('.ui.confirmar.modal #txtRef').val();
                },
                onDeny: function () {
                    $('.ui.segment.loading').removeClass('loading');
                }
            })
        ;

        $('.ui.accordion')
            .accordion({
                exclusive: false,
                animateChildren: false
            })
        ;

        $('.ui.gabarito.modal')
            .modal({
                onShow: function () {
                    testeConexao();
                },
                onApprove: function () {
                    if (conectado) {
                        window.onbeforeunload = null;
                        $('form').addClass('loading').submit();
                    }
                    else {
                        siac.mensagem('Conecte-se à internet antes de confirmar.')
                        return false;
                    }
                }
            })
        ;

        $('.message .close')
          .on('click', function () {
              $(this)
                .closest('.message')
                .transition('fade')
              ;
          })
        ;

        $('.arquivar.button').click(function () {
            arquivar();
        });

        $('.finalizar.button').click(function () {
            submitForm();
        });
    }

    function definirTamanhoDiv() {
        var tamTela = $(window).height();
        var tamDesejado = tamTela * 0.6;
        $('.ui.accordion').
            css('max-height', tamDesejado + 'px').
            css('overflow-y', 'scroll');
    }

    function relogio() {
        setInterval(function () { date = new Date(); $('#lblHoraAgora').text(date.getHours() + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min'); }, 1000);
    }

    function temporizador() {
        timerId = setInterval(function () {
            if (duracao > 0) {
                duracao = duracao - 1;
                $('#lblHoraRestante').text(siac.Utilitario.minutosParaStringTempo(duracao)).parent().transition('flash');
                if (duracao < 6 && !$('#lblHoraRestante').parent().hasClass('red')) {
                    $('#lblHoraRestante').parent().addClass('red');
                }
            }
            else {
                clearInterval(timerId);
            }
        }, 60000);
    }

    function testeConexao() {
        $.ajax({
            type: 'GET',
            url: "/Acesso/Conectado",
            success: function () {
                conectado = true;
            },
            error: function () {
                conectado = false;
            }
        });
    }

    function expandirImagem(card) {
        card = $(card);
        src = card.find('img').attr('src');
        legenda = card.find('.header').text();
        fonte = card.find('.description').text();
        modal = $('.ui.anexo.modal');

        modal.find('.header').text(legenda);
        modal.find('img.image').attr('src', src);
        modal.find('.description').html(fonte);

        modal.modal('show');
    }

    function gabarito(codQuestao, resposta, obj, multi) {
        $label = $('.ui.accordion').find('#txtCodQuestao[value="' + codQuestao + '"]').parent().find('.ui.label');
        if (resposta != '') {
            $label.removeClass('red');
            $label.removeAttr('style');
            $label.addClass('teal');
            $label.html('Respondida');
            if (obj) {
                $label.append($('<div class="detail"></div>').text(resposta));
            }
        }
        else {
            $label.attr('style', 'display:none')
        }
        definirTamanhoDiv();
    }

    function submitForm() {
        $('form').removeClass('error');
        var finalizada = true;
        var lstNaoRespondidas = [];
        $lstTitle = $('.ui.accordion > .title');
        for (var i = 0; i < $lstTitle.length; i++) {
            var codQuestao = $lstTitle.eq(i).find('#txtCodQuestao').val();
            var txtResposta = $lstTitle.eq(i).next().find('#txtResposta' + codQuestao).val();
            if (txtResposta == '') {
                finalizada = false;
                lstNaoRespondidas.push(codQuestao);
            }
            else if (!txtResposta) {
                if (!$lstTitle.eq(i).next().find('input[name="rdoResposta' + codQuestao + '"]').is(':checked')) {
                    finalizada = false;
                    lstNaoRespondidas.push(codQuestao);
                }
            }
        }
        if (finalizada) {
            verificarConfirmar();
        }
        else {
            for (var i = 0; i < lstNaoRespondidas.length; i++) {
                for (var j = 0; j < $lstTitle.length; j++) {
                    if ($lstTitle.eq(j).find('#txtCodQuestao').val() == lstNaoRespondidas[i]) {
                        $lstTitle.eq(j).find('.ui.label').removeAttr('style').removeClass('teal').addClass('red').html('Não respondida').transition('tada');

                    }
                }
            }
            $('form').addClass('error');
            $('form .message').removeClass('hidden');
            definirTamanhoDiv();
        }
    }

    function verificarConfirmar() {
        $modal = $('.ui.gabarito.modal');

        $accordion = $('form .ui.accordion').clone();

        $accordion.removeAttr('style');

        $modal.find('.content').html($('<div class="ui form"></div>').append($accordion));

        $modalAccordion = $modal.find('.ui.accordion');

        $modalAccordion.accordion({
            onChange: function () { $('.ui.gabarito.modal').modal('refresh'); },
            animateChildren: false
        });

        $lstInput = $modalAccordion.find(':input,a');

        for (var i = 0; i < $lstInput.length; i++) {
            $lstInput.eq(i)
                .attr({
                    'readonly': 'readonly'
                })
                .removeAttr('href')
                .removeAttr('onchange')
                .removeAttr('onclick')
                .off()
            ;
        }

        $modal.modal('show');
    }

    function arquivar() {
        $.ajax({
            type: 'POST',
            data: { codigo: _codAvaliacao },
            url: "/Dashboard/Autoavaliacao/Arquivar",
            success: function () {
                window.onbeforeunload = function () { $('.ui.global.loader').parent().addClass('active'); };
                window.location.href = '/Historico/Autoavaliacao/Detalhe/' + _codAvaliacao;
            },
            error: function () {
                siac.mensagem('Não foi possível arquivar a autoavaliação.')
            }
        });
    }

    return {
        iniciar: iniciar,
        gabarito: gabarito,
        expandirImagem: expandirImagem
    }
})();