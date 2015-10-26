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
    var _controleInterval;
    var _controleRestante;

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

        definirAlturaDiv();

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
                        temporizador(new Date(date.getTime() + duracao * 60 * 1000));
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

        $('.arquivar.button').click(function () {
            arquivar();
        });

        $('.finalizar.button').click(function () {
            finalizar();
        });

        $('.card.anexo.imagem').click(function () {
            expandirImagem(this);
        });

        $('textarea[name^="txtResposta"], input[name^="rdoResposta"]').change(function () {
            var $_this = $(this);
            $label = $_this.parents('.content').prev().find('.ui.label');
            if ($_this.val()) {
                $label.removeClass('red');
                $label.removeAttr('style');
                $label.html('Respondida');
                if ($_this.attr('name').indexOf('rdo') > -1) {
                    $label.find('.detail').remove();
                    $label.append($('<div class="detail"></div>').text($('input[name="' + $_this.attr('name') + '"]:checked').next().find('b').text()));
                }
            }
            else {
                $label.attr('style', 'display:none');
            }
        });

        $('.ui.gabarito.modal')
            .modal({
                onApprove: function () {
                    $.ajax({
                        type: 'GET',
                        url: "/Acesso/Conectado",
                        success: function () {
                            window.onbeforeunload = function () {
                                $('.ui.global.loader').parent().addClass('active');
                            };
                            $('form').submit();
                        },
                        error: function () {
                            siac.mensagem('Conecte-se à internet antes de confirmar.')
                        }
                    });
                    return false;
                }
            })
        ;
    }

    function definirAlturaDiv() {
        var tamTela = $(window).height();
        var tamDesejado = tamTela * 0.6;
        $('.ui.accordion').css({
            'max-height': tamDesejado + 'px',
            'overflow-y': 'auto'
        });
    }

    function relogio() {
        setInterval(function () {
            var date = new Date();
            $('#lblHoraAgora').text(date.getHours() + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min');
        }, 1000);
    }

    function temporizador(dtTermino) {
        _controleInterval = setInterval(function () {
            var offset = dtTermino.getTimezoneOffset() * 60 * 1000;
            var timeRestante = (dtTermino.getTime() + offset) - (new Date().getTime() + offset);

            if (timeRestante > 0) {
                var date = new Date();
                date.setTime(timeRestante);
                var offsetDate = date.getTimezoneOffset() * 60 * 1000;
                date.setTime(date.getTime() + offsetDate);
                var txtRestante = date.getHours() + 'h' + ("0" + (date.getMinutes())).slice(-2) + 'min';
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
                siac.mensagem('O tempo definido por você acabou.', 'O tempo acabou.');
                clearInterval(_controleInterval);
            }
        }, 1000);
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

    function finalizar() {
        var finalizada = true;
        $objects = $('textarea[name^="txtResposta"], input[name^="rdoResposta"]');
        for (var i = 0, length = $objects.length; i < length; i++) {
            var _this = $objects.eq(i);
            $label = _this.parents('.content').prev().find('.ui.label');
            if (_this.attr('name').indexOf('rdo') > -1) {
                if ($('input[name="' + _this.attr('name') + '"]:checked').length === 0) {
                    $label.removeAttr('style').addClass('red').html('Não respondida').transition('tada');
                    finalizada = false;
                }
            }
            else if (!_this.val()) {
                $label.removeAttr('style').addClass('red').html('Não respondida').transition('tada');
                finalizada = false;
            }
        }

        if (finalizada) {
            confirmar();
        }
        else {
            definirAlturaDiv();
            $('html, body').animate({
                scrollTop: $(".title .label.red").offset().top
            }, 1000);
        }
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
        iniciar: iniciar
    }
})();