siac.Certificacao = siac.Certificacao || {};

siac.Certificacao.Gerar = (function () {

	function iniciar() {
		$('.ui.dropdown').dropdown();
		$('.ui.modal').modal();
	    $('.ui.termo.modal').modal({ closable: false }).modal('show');
		$('.cancelar.button').popup({ on: 'click' });

		$('.ui.confirmar.modal').modal({
			onApprove: function () {
				$('form').addClass('loading').submit();
			}
		});

		$('.prosseguir.button').click(function () {
			prosseguir()
		});

		$('#ddlDisciplina').change(function () {
			listarTemas(this.value);
		})

		$('#ddlTipo').change(function () {
			mostrarOpcoesPorTipo(this.value);
		})


	}

	function prosseguir() {
		var errorList = $('form .error.message .list');

		errorList.html('');
		$('form').removeClass('error');

		var valido = true;

		if (!$('#ddlDisciplina').val()) {
			errorList.append('<li>Selecione uma disciplina</li>');
			valido = false;
		}

		if (!$('#ddlTemas').val()) {
			errorList.append('<li>Selecione ao menos um tema</li>');
			valido = false;
		}

		if (!$('#ddlTipo').val()) {
			errorList.append('<li>Selecione o tipo das questões da sua avaliação acadêmica</li>');
			valido = false;
		}

		if (($('#ddlTipo').val() == 1 || $('#ddlTipo').val() == 3) && !$('#txtQteObjetiva').val()) {
			errorList.append('<li>Indique a quantidade das questões objetivas</li>');
			valido = false;
		}
		if (($('#ddlTipo').val() == 2 || $('#ddlTipo').val() == 3) && !$('#txtQteDiscursiva').val()) {
			errorList.append('<li>Indique a quantidade das questões discursivas</li>');
			valido = false;
		}

		if (!$('#ddlDificuldade').val()) {
			errorList.append('<li>Selecione a dificuldade das questões</li>');
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
		var disciplinas = $('#ddlDisciplinas');
		var ddlTemas = $('#ddlTemas');

		ddlTemas.parent().addClass('loading');
		$.ajax({
			type: 'POST',
			url: '/Tema/RecuperarTemasPorCodDisciplinaTemQuestao',
			data: { 'codDisciplina': codDisciplina },
			success: function (data) {
				ddlTemas.html('');
				ddlTemas.val(ddlTemas.prop('defaultSelected'));
				$.each(data, function (id, item) {
					ddlTemas.append('<option value="' + item.CodTema + '">' + item.Descricao + '</option>');
				});

				ddlTemas.parent().removeClass('loading');
			},
			error: function () {
				siac.mensagem("Erro no carregamento de Temas", "Erro");
				ddlTemas.parent().removeClass('loading');
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

siac.Certificacao.Configurar = (function () {
    var _codAvaliacao;
    var _arrayQuestoes = [];
    var _qteObjetiva = 0;
    var _qteDiscursiva = 0;
    var _qteMaxObjetiva;
    var _qteMaxDiscursiva;

    var _OBJ = "Objetiva";
    var _DISC = "Discursiva";
    var _ADD = "Adicionar";
    var _REM = "Remover"

    function iniciar() {
        //Obtendo Dados da Página
        $elemento = $('[data-avaliacao]');
        if (!_codAvaliacao && $elemento) {
            _codAvaliacao = $elemento.data('avaliacao');
            $elemento.removeAttr("data-avaliacao");
        }
        _qteMaxObjetiva = $('.informacoes .objetiva.label .detail').html();
        _qteMaxDiscursiva = $('.informacoes .discursiva.label .detail').html();
        
        $('.questoes.modal .card').map(function () {
            _arrayQuestoes.push($(this).attr('id'))
            if ($(this).find('.tipo.label').text() == _OBJ) _qteObjetiva++;
            else if ($(this).find('.tipo.label').text() == _DISC) _qteDiscursiva++;
            console.log(_arrayQuestoes);
        });
        //Fim da Obtenção de Dados

        $('.informacoes.button').click(function () {
            $('.informacoes.modal').modal('show');
        });

        $('.ui.dropdown').dropdown().change(function () {
            carregarQuestoes();
        });

        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal').modal({
                onApprove: function () {
                    console.log(_arrayQuestoes);
                    $.ajax({
                        type: 'POST',
                        url: '/dashboard/avaliacao/certificacao/Configurar/',
                        data: {
                            codigo: _codAvaliacao,
                            questoes: _arrayQuestoes
                        },
                        success: function (data) {
                            if (data) {
                                window.location.href = "/dashboard/avaliacao/certificacao/agendar/"+_codAvaliacao;
                            }
                        },
                        error: function (data) {
                            siac.aviso('error', 'red');
                        }
                    });
                }
            });

        $('.confirmar.button').click(function () {

            $questoes = $('.questoes.modal .cards').clone();
            $questoes.removeClass('three').addClass('one');
            $questoes.find('.attached.buttons').remove();

            $questoes.find('.card').map(function () {
                $header = $(this).find('.header');
                var text = $header.data('enunciado');
                text = text.substituirTodos('\n', '<br>');
                $header.html(text);
                $header.css('text-align', 'justify');
            })

            $('.confirmar.modal .content').html($questoes);

            $('.ui.confirmar.modal').modal('show');

            return false;
        });

        $('.ui.questoes.button').click(function(){
            $('.questoes.modal').modal('show');
        });

        $('.ui.detalhe.button').click(function () {
            var codQuestao = $(this).parents('.card').attr('id');
            mostrarQuestao(codQuestao, this);
            console.log(_arrayQuestoes);
        });

        $('.ui.acao.button').click(function () {
            $_this = $(this);
            var tipo = $_this.parents('.card').find('.label').last().text();
            var id = $_this.parents('.card').attr('id');
            if ($_this.html() == "Adicionar") {
                adicionarQuestao(id,tipo);
            } else if ($_this.html() == "Remover") {
                removerQuestao(id,tipo);
            }
        });

        atualizarQuantidadeView();
        
    }

    function carregarQuestoes() {
        var temas = $('#ddlTemas').val();
        var dificuldade = $('#ddlDificuldade').val();
        var tipo = $('#ddlTipo').val();
        console.log(temas);
        if (temas && dificuldade && tipo) {
            $resultado = $('.resultado .cards');
            $resultado.addClass('form loading')
            $.ajax({
                type: 'POST',
                url: '/dashboard/avaliacao/certificacao/CarregarQuestoes/',
                data: {
                    codigo: _codAvaliacao,
                    temas: temas,
                    dificuldade: dificuldade,
                    tipo: tipo
                },
                success: function (data) {
                    if (data) {
                        $cards = $(data);
                        $resultado.html($cards);
                        
                        $resultado.find('.card').map(function () {
                            $card = $(this);
                            var id = $card.attr('id');
                            if (_arrayQuestoes.indexOf(id) > -1) {
                                $card.find('.acao.button').text(_REM);
                            }
                        });
                    }
                },
                error: function (data) {
                    siac.mensagem(data,'Error');
                },
                complete: function () {
                    $resultado.removeClass('form loading');

                    $resultado.find('.acao.button').off('click').click(function () {
                        $_this = $(this);
                        var tipo = $_this.parents('.card').find('.tipo.label').text();
                        if ($_this.html() == _ADD) {
                            var id = $_this.parents('.card').attr('id');
                            adicionarQuestao(id,tipo);
                        } else if ($_this.html() == _REM) {
                            var id = $_this.parents('.card').attr('id');
                            removerQuestao(id,tipo);
                        }
                    });

                    $('.ui.detalhe.button').click(function () {
                        var codQuestao = $(this).parents('.card').attr('id');
                        mostrarQuestao(codQuestao,this);
                    });
                }
            });
        }
    }

    function adicionarQuestao(codQuestao, tipo) {
        var podeAdicionar = false;
        if (tipo == _OBJ) {
            if (_qteObjetiva < _qteMaxObjetiva) {
                podeAdicionar = true;
                _qteObjetiva++;
            }
            else
                siac.aviso('Você não pode adicionar mais questões objetivas', 'red');

        } else if (tipo == _DISC) {
            if (_qteDiscursiva < _qteMaxDiscursiva) {
                podeAdicionar = true;
                _qteDiscursiva++;
            }
            else
                siac.aviso('Você não pode adicionar mais questões discursivas', 'red');
        }
        if (podeAdicionar) {
            $card = $('#' + codQuestao + '.card');
            $card.find('.acao.button').text(_REM).off('click').click(function () {
                removerQuestao(codQuestao, tipo);
            });
            _arrayQuestoes.push(codQuestao);
            $card.transition({
                onHide: function () {
                    $('.questoes.modal .cards').append($card);
                    siac.aviso('Questão adicionada', 'green');
                    atualizarQuantidadeView();
                }
            }).transition('scale');
        }
    }

    function removerQuestao(codQuestao, tipo) {
        $card = $('#' + codQuestao + '.card');
        var index = _arrayQuestoes.indexOf(codQuestao);
        if (index > -1) {
            _arrayQuestoes.splice(index, 1);
            if (tipo == _OBJ) _qteObjetiva--;
            else if (tipo == _DISC) _qteDiscursiva--;
        }
        $card.transition({
            onHide: function () {
                $card.remove();
                siac.aviso('Questão removida', 'red');
                atualizarQuantidadeView();
            }
        }).transition('scale');
    }

    function mostrarQuestao(codQuestao,_this) {
        $_this = $(_this);
        $_this.addClass('loading');
        $.ajax({
            type: 'POST',
            url: '/dashboard/avaliacao/certificacao/CarregarQuestao/',
            data: {
                codQuestao: codQuestao
            },
            success: function (data) {
                if (_arrayQuestoes.indexOf(codQuestao) > -1) codQuestao = _arrayQuestoes.indexOf(codQuestao) + 1;
                $('.questao.modal .header').html('Questão ' + codQuestao);
                $('.questao.modal .segment').html(data);
                $('.accordion').accordion();
                $('.questao.modal').modal('show');
            },
            error: function (data) {
                siac.mensagem(data, 'Error');
            },
            complete: function () {
                $_this.removeClass('loading');
            }
        });
    }

    function atualizarQuantidadeView() {
        $disc = $('.quantidade.disc.label');
        $obj = $('.quantidade.obj.label');

        $disc.find('.detail').text(_qteDiscursiva);
        $obj.find('.detail').text(_qteObjetiva);
        
        {
            if (_qteObjetiva == _qteMaxObjetiva) {
                $obj.addClass('green');
            }
            else $obj.removeClass('green');
        }
        
        {
            if (_qteDiscursiva == _qteMaxDiscursiva) {
                $disc.addClass('green');
            }
            else $disc.removeClass('green');
        }

    }

    return {
        iniciar: iniciar
    }

})();

siac.Certificacao.Agendar = (function () {
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
                    $('form').submit();
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

siac.Certificacao.Index = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 12, _controleAjax;

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
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        $cards = $('.ui.cards');
        $cards.parent().addClass('loading');
        _controleAjax = $.ajax({
            url: '/Dashboard/Avaliacao/Certificacao/Listar',
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

siac.Certificacao.Pessoas = (function () {
    var _controleAjax;
    var _lstPessoaSelecionada = [];
    var _content = [];

    function iniciar() {
        var $code = $('code#pessoas');
        _lstPessoaSelecionada = JSON.parse($code.html());
        $code.remove();

        $('.ui.modal').modal();
        $('.ui.checkbox').checkbox();
        $('.ui.dropdown').dropdown();

        $('#ddlFiltro').change(function () {
            filtrar($(this).val());
            console.log('mudou');
        });

        $('.ui.search')
          .search({
              source: _content
          })
        ;
    }

    function filtrar(filtro) {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }

        $buscar = $('.ui.search');
            
        $buscar.addClass('loading');

        _controleAjax = $.ajax({
            data: {
                filtro: filtro
            },
            type: 'POST',
            url: '/dashboard/avaliacao/certificacao/filtrar',
            success: function (data) {
                _content = data;
                $buscar
                  .search({
                      source: _content,
                      onSelect: function onSelect(result, response) {
                          console.log(result);
                      }
                  })
                ;
            },
            error: function () {
                siac.mensagem('Um erro ocorreu.');
            },
            complete: function () {
                $buscar.removeClass('loading');
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();