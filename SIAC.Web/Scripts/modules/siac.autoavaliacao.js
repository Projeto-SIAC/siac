siac.Autoavaliacao = siac.Autoavaliacao || {};

siac.Autoavaliacao.Index = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 10, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var categorias = [];
    var dificuldade = "";
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
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        $cards = $('.ui.cards');
        $cards.parent().addClass('loading');
        _controleAjax = $.ajax({
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
                $cards.parent().removeClass('loading');
                adicionarEventoArquivar();
            }
        });
    }

    function topo() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    }

    function adicionarEventoArquivar() {
        $('.arquivar.button').click(function () {
            var $this = $(this);
            var codAvaliacao = $this.parents('[data-avaliacao]').attr('data-avaliacao');
            $this.addClass('loading');
            $.ajax({
                type: 'POST',
                data: { codigo: codAvaliacao },
                url: '/Dashboard/Autoavaliacao/Arquivar',
                success: function () {
                    window.location.href = '/historico/autoavaliacao/detalhe/' + codAvaliacao;
                },
                error: function () {
                    siac.mensagem('Não foi possível arquivar a autoavaliação.')
                    $this.removeClass('loading');
                }
            });
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Autoavaliacao.Detalhe = (function () {
    var _codAvaliacao;

    function iniciar() {
        $elemento = $('[data-avaliacao]');
        _codAvaliacao = $elemento.attr('data-avaliacao');
        $elemento.removeAttr('data-avaliacao');

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
                    window.location.href = '/historico/autoavaliacao/detalhe/' + _codAvaliacao;
                },
                error: function () {
                    siac.mensagem('Não foi possível arquivar a autoavaliação.')
                },
                complete: function () {
                    $(_this).removeClass('loading');
                }
            });
        });

        $('.card.anexo.imagem').off().click(function () {
            var $this = $(this);
            var source = $this.find('img').attr('src');
            var legenda = $this.find('.header').text();
            $description = $this.find('.description');
            var fonte = $description.attr("data-fonte") ? $description.data('fonte') : $description.text();

            siac.Anexo.expandirImagem(source, legenda, fonte);
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

        $('.card.anexo.imagem').off().click(function () {
            var $this = $(this);
            var source = $this.find('img').attr('src');
            var legenda = $this.find('.header').text();
            $description = $this.find('.description');
            var fonte = $description.attr("data-fonte") ? $description.data('fonte') : $description.text();

            siac.Anexo.expandirImagem(source, legenda, fonte);
        });
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
        $elemento = $('[data-avaliacao]');
        _codAvaliacao = $elemento.attr('data-avaliacao');
        $elemento.removeAttr('data-avaliacao');

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
                        $('#lblHoraDuracao').text(duracao.minutosParaStringTempo());
                        $('#lblHoraRestante').text(duracao.minutosParaStringTempo());
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
                animateChildren: false
            })
        ;

        $('.arquivar.button').click(function () {
            arquivar();
        });

        $('.finalizar.button').click(function () {
            finalizar();
        });

        $('.card.anexo.imagem').off().click(function () {
            var $this = $(this);
            var source = $this.find('img').attr('src');
            var legenda = $this.find('.header').text();
            $description = $this.find('.description');
            var fonte = $description.attr("data-fonte") ? $description.data('fonte') : $description.text();

            siac.Anexo.expandirImagem(source, legenda, fonte);
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
                window.location.href = '/historico/autoavaliacao/detalhe/' + _codAvaliacao;
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

siac.Autoavaliacao.Gerar = (function () {
    var _jsonDificuldades;

    function iniciar() {
        $json = $('code.json');
        _jsonDificuldades = JSON.parse($json.html());
        $json.remove();

        $('.ui.dropdown').dropdown();

        $('.ui.accordion').accordion({
            animateChildren: false
        });
        
        $('.prosseguir.button').click(function () {
            prosseguir();
        });
        
        $('.ui.confirmar.modal')
          .modal({
              onApprove: function () {
                  $('form').submit();
              }
          })
        ;

        $('#ddlTipo').change(function () {
            mostrarOpcoesPorTipo();
        });

        $('#ddlDisciplinas').parent().mouseout(function () {
            $('.ui.dropdown').dropdown();
        });

        $('#ddlDisciplinas').change(function () {
            ajustarFormulario();            
        });

        $('.cancelar.button').popup({
            on: 'click'
        });
    }

    function prosseguir() {
        var validado = false;
        $list = $('form .error.message .list');
        $list.html('');

        if ($('#ddlDisciplinas :selected').length > 0) {
            var discs = $('#ddlDisciplinas').val();
            var ok = true;
            for (var i = 0; i < discs.length; i++) {
                if (!$('#ddlTemas' + discs[i] + ' :selected').length > 0) {
                    $list.append('<li>Selecione pelo menos um tema para ' + $('#ddlDisciplinas option[value="' + discs[i] + '"]').text() + '</li>');
                    validado = false;
                }

                if (!$('#ddlDificuldade' + discs[i]).val()) {
                    $list.append('<li>Selecione a dificuldade para ' + $('#ddlDisciplinas option[value="' + discs[i] + '"]').text() + '</li>');
                    validado = false;
                }

                if (!$('#ddlTipo').val()) {
                    $list.html('<li>Selecione o tipo das questões</li>');
                    ok = false;
                    validado = false;
                }
                else {
                    ok = true;
                    if ($('#ddlTipo').val() == '1') {
                        if (!$('#txtQteObjetiva' + discs[i]).val()) {
                            ok = false;
                            validado = false;
                            $list.append('<li>Preencha a quantidade das questões para ' + $('#ddlDisciplinas option[value="' + discs[i] + '"]').text() + '</li>');
                        }
                    }
                    else if ($('#ddlTipo').val() == '2') {
                        if (!$('#txtQteDiscursiva' + discs[i]).val()) {
                            ok = false;
                            validado = false;
                            $list.append('<li>Preencha a quantidade das questões para ' + $('#ddlDisciplinas option[value="' + discs[i] + '"]').text() + '</li>');
                        }
                    }
                    else if ($('#ddlTipo').val() == '3') {
                        if (!$('#txtQteDiscursiva' + discs[i]).val() || !$('#txtQteObjetiva' + discs[i]).val()) {
                            ok = false;
                            validado = false;
                            $list.append('<li>Preencha a quantidade das questões para ' + $('#ddlDisciplinas option[value="' + discs[i] + '"]').text() + '</li>');
                        }
                    }
                }

                if (ok == true) {
                    validado = true;
                }
                else {
                    validado = false;
                }
            }
        }
        else {
            $list.append('<li>Selecione pelo menos uma disciplina</li>');
            validado = false;
        }

        if (validado) {
            confirmar()
        }
        else {
            $('form').addClass('error');
            $('html, body').animate({
                scrollTop: $('form .error.message').offset().top
            }, 1000);
        }
    }

    function recuperarTemasPorDisciplina(selecionado) {
        var ddlTema = $('#ddlTemas' + selecionado);
        $.ajax({
            cache: false,
            type: 'POST',
            url: '/Tema/RecuperarTemasPorCodDisciplinaTemQuestao',
            data: { "codDisciplina": selecionado },
            success: function (data) {
                ddlTema.html('');
                ddlTema.parent().find('.label').remove();
                for (var i = 0, length = data.length; i < length; i++) {
                    ddlTema.append($('<option></option>').val(data[i].CodTema).html(data[i].Descricao));
                }
            },
            error: function () {
                siac.mensagem('Falha ao recuperar temas.');
            }
        });
    }

    function ajustarFormulario() {
        var discs = $('#ddlDisciplinas').val();
        $('.ui.accordion').html('');
        var temAccordion = $('.ui.tema.accordion');
        var difAccordion = $('.ui.dificuldade.accordion');
        var qteAccordion = $('.ui.quantidade.accordion');
        var dificuldades;
        for (var j = 0, length = _jsonDificuldades.length; j < length; j++) {
            dificuldades += '<option value="' + _jsonDificuldades[j].Codigo + '">' + _jsonDificuldades[j].Descricao + '</option>';
        }
        for (var i = 0; i < discs.length; i++) {
            var disciplina = $('#ddlDisciplinas option[value="' + discs[i] + '"]').text();
            temAccordion.append('<div class="title"><i class="icon dropdown"></i>' + disciplina + '</div>'+
                                '<div class="content">'+
                                    '<div class="field">'+
                                        '<select required name="ddlTemas' + discs[i] + '" id="ddlTemas' + discs[i] + '" class="ui search dropdown" multiple>'+
                                            '<option value="">Tema</option>'+
                                        '</select>'+
                                    '</div>'+
                                '</div>');
            recuperarTemasPorDisciplina(discs[i]);
            
            difAccordion.append('<div class="title">'+
                                    '<i class="icon dropdown"></i>' + disciplina +
                                '</div>' +
                                '<div class="content">'+
                                    '<div class="field">'+
                                        '<select required id="ddlDificuldade' + discs[i] + '" name="ddlDificuldade' + discs[i] + '" class="ui search dropdown">'+
                                            '<option value="">Dificuldade</option>'+dificuldades+
                                        '</select>' +
                                    '</div>' +
                                '</div>');
            qteAccordion.append('<div class="title"><i class="icon dropdown"></i>' + disciplina + '</div>'+
                                '<div class="content">'+
                                    '<div class="two fields">'+
                                        '<div class="field objetiva"><label>Objetivas</label>'+
                                            '<input required id="txtQteObjetiva' + discs[i] + '" name="txtQteObjetiva' + discs[i] + '" data-mask="0#" type="number" placeholder="Quantidade de objetivas" min="1" />'+
                                        '</div>'+
                                        '<div class="field discursiva"><label>Discursivas</label>'+
                                            '<input required id="txtQteDiscursiva' + discs[i] + '" name="txtQteDiscursiva' + discs[i] + '" data-mask="0#" type="number" placeholder="Quantidade de discursivas" min="1" />'+
                                        '</div>'+
                                    '</div>'+
                                '</div>');
        };
        mostrarOpcoesPorTipo();
    }

    function mostrarOpcoesPorTipo() {
        var tipo = $('#ddlTipo').val();
        switch (tipo) {
            case '1':
                $('.discursiva').addClass('disabled').attr('style', 'pointer-events: none');
                $('.objetiva').removeClass('disabled').removeAttr('style');
                break;
            case '2':
                $('.discursiva').removeClass('disabled').removeAttr('style');
                $('.objetiva').addClass('disabled').attr('style', 'pointer-events: none');
                break;
            case '3':
                $('.discursiva').removeClass('disabled').removeAttr('style');
                $('.objetiva').removeClass('disabled').removeAttr('style');
                break;
            default:
                $('.discursiva').addClass('disabled').attr('style', 'pointer-events: none');
                $('.objetiva').addClass('disabled').attr('style', 'pointer-events: none');
        }
    }

    function confirmar() {
        $modal = $('.ui.confirmar.modal');
        $ddlDisciplinas = $('#ddlDisciplinas :selected');
        $ddlTipo = $('#ddlTipo');
        $table = $modal.find('tbody').html('');
        for (var i = 0; i < $ddlDisciplinas.length; i++) {
            $tr = $('<tr></tr>');
            $tdDisciplina = $('<td></td>').html('<b>' + $ddlDisciplinas.eq(i).text() + '</b>');
            $tdTemas = $('<td class="ui labels"></td>');
            $ddlTemas = $('#ddlTemas' + $ddlDisciplinas.eq(i).val() + ' :selected');
            for (var j = 0; j < $ddlTemas.length; j++) {
                $tdTemas.append($('<div class="ui tag label"></div>').text($ddlTemas.eq(j).text()));
            }
            $tdQteQuestoes = $('<td class="ui labels"></td>');
            if ($ddlTipo.val() == 1 || $ddlTipo.val() == 3) {
                $tdQteQuestoes.append($('<div class="ui label"></div>').html('Objetiva<div class="detail">' + $('#txtQteObjetiva' + $ddlDisciplinas.eq(i).val()).val() + '</div>'));
            }
            if ($ddlTipo.val() == 2 || $ddlTipo.val() == 3) {
                $tdQteQuestoes.append($('<div class="ui label"></div>').html('Discursiva<div class="detail">' + $('#txtQteDiscursiva' + $ddlDisciplinas.eq(i).val()).val() + '</div>'));
            }
            $tdDificuldade = $('<td></td>').text($('#ddlDificuldade' + $ddlDisciplinas.eq(i).val() + ' :selected').text());
            $table.append($tr.append($tdDisciplina).append($tdTemas).append($tdQteQuestoes).append($tdDificuldade));
        }
        $modal.modal('show');
    }

    return {
        iniciar: iniciar
    }
})();