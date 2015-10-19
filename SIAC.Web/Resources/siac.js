var siac = siac || (function () {
	function iniciar() {
		document.onreadystatechange = function () {
			if (document.readyState == "complete") {
				$('.ui.global.loader').parent().removeClass('active');
			}
		}

		window.onbeforeunload = function () { $('.ui.global.loader').parent().addClass('active'); };

		$('.ui.mensagem.modal')
			.modal({
				allowMultiple: true
			})
	    ;

		carregar();
	}

	function carregar() {
	    var pathname = window.location.pathname.toLowerCase();
	    if (pathname == '/historico/questao') {
	        siac.Questao.Index.iniciar();
	    }
	    else if (pathname == '/dashboard/questao/cadastrar') {
	        siac.Questao.Cadastrar.iniciar();
	    }
	    else if (pathname == '/historico/questao/gerar') {
	        siac.Questao.Gerar.iniciar();
	    }
	    else if (/\/historico\/questao\/[0-9]+$/.test(pathname)) {
	        siac.Questao.Detalhe.iniciar();
	    }
	    else if (/\/historico\/questao\/editar\/[0-9]+$/.test(pathname)) {
	        siac.Questao.Editar.iniciar();
	    }
	}

	function mensagem(mensagem, titulo) {
	    if (mensagem) {
	        if (titulo) {
	            $('.ui.mensagem.modal .header')
					.text(titulo)
	            ;
	        }
	        $('.ui.mensagem.modal .content')
				.text(mensagem)
	        ;
	        $('.ui.mensagem.modal')
				.modal('show')
	        ;
	    }
	}

	return {
	    iniciar: iniciar,
		mensagem: mensagem
	}
})();

siac.Utilitario = (function () {
	// return 1 = a is bigger than b, 0 = a and b are same, -1 = a is smaller than b
	function compararData(strDateA, strDateB) {
		timeDateA = Date.parse(strDateA);
		timeDateB = Date.parse(strDateB);
		if (timeDateA > timeDateB) {
			return 1;
		}
		else if (timeDateA == timeDateB) {
			return 0;
		}
		else {
			return -1;
		}
	}

	// return true or false
	function dataEFuturo(strDate) {
		timeDateNow = new Date().getTime();
		timeDate = Date.parse(strDate);
		if (timeDate > timeDateNow) {
			return true;
		}
		return false;
	}

	function encurtarTextoEm(str, length) {
		var text = '';

		if (str.length > length) {
			text = str.substring(0, length);
			var afterText = str.substring(length);
			if (afterText.indexOf(' ') > -1) {
				afterText = afterText.substring(0, afterText.indexOf(' '));
				afterText = afterText + '...';
			}
			text = text + afterText;
		}
		else {
			text = str;
		}

		return text;
	}

	function quebrarLinhaEm(texto, indiceMaximo) {
		texto = texto.trim();
		if (texto.length > indiceMaximo) {
			var tempMensagem = texto;
			var qteLinha = Math.ceil(texto.length / indiceMaximo);
			mensagem = '';
			for (var i = 0; i <= qteLinha; i++) {
				tempMensagem = tempMensagem.trim();
				if (tempMensagem.length > indiceMaximo) {
					var indice = tempMensagem.substr(0, indiceMaximo).lastIndexOf(' ');
					if (indice == -1) {
						indice = indiceMaximo;
					}
					mensagem += tempMensagem.substr(0, indice) + '<br/>';
					tempMensagem = tempMensagem.substring(indice, tempMensagem.length);
				}
				else {
					mensagem += tempMensagem;
					break;
				}
			}

			return mensagem;
		}
		return texto;
	}

	return {
		compararData: compararData,
		dataEFuturo: dataEFuturo,
		encurtarTextoEm: encurtarTextoEm,
		quebrarLinhaEm: quebrarLinhaEm
	}
})();

siac.Questao = siac.Questao || {};

siac.Questao.Index = (function () {
    var jsnQuestoesGeral, jsnQuestoesFiltro;

    function iniciar() {
        $('.ui.cards').parent().addClass('loading');       
        
        window.onbeforeunload = function () { $ajax.abort(); $('.ui.global.loader').parent().addClass('active'); };
        
        $ajax = $.ajax({
            url: '/Historico/Questao/Minhas',
            method: 'GET',
            success: function (questoes) {
                jsnQuestoesGeral = questoes;
                jsnQuestoesFiltro = questoes;
                minhasQuestoes();
            },
            error: function () {
                siac.mensagem("Erro ao recuperar as questões");
                $('.ui.cards').parent().removeClass('loading');
            }
        }); 
    };

    function topo() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    }

    function filtro(strTexto) {
        if (strTexto) {
            strTexto = strTexto.toLowerCase().split(';');
            jsnQuestoesFiltro = [];
            for (var j = 0; j < strTexto.length; j++) {
                strTexto[j] = strTexto[j].trim();
                if (strTexto[j].indexOf('+') > -1) {
                    lstFiltro = jsnQuestoesGeral;
                    termos = strTexto[j].split('+');
                    for (var k = 0; k < termos.length; k++) {
                        termos[k] = termos[k].trim();
                        if (termos[k]) {
                            var lstFiltroAnd = [];
                            for (var i = 0; i < lstFiltro.length; i++) {
                                if (lstFiltro[i].Enunciado.toLowerCase().indexOf(termos[k]) > -1) {
                                    lstFiltroAnd.push(lstFiltro[i]);
                                }
                                else if (lstFiltro[i].Disciplina.toLowerCase().indexOf(termos[k]) > -1) {
                                    lstFiltroAnd.push(lstFiltro[i]);
                                }
                                else if (lstFiltro[i].TipoQuestao.toLowerCase().indexOf(termos[k]) > -1) {
                                    lstFiltroAnd.push(lstFiltro[i]);
                                }
                                else if (lstFiltro[i].Dificuldade.toLowerCase().indexOf(termos[k]) > -1) {
                                    lstFiltroAnd.push(lstFiltro[i]);
                                }
                                else if (lstFiltro[i].DtCadastro.toLowerCase().indexOf(termos[k]) > -1) {
                                    lstFiltroAnd.push(lstFiltro[i]);
                                }
                                else {
                                    for (var l = 0; l < lstFiltro[i].Temas.length; l++) {
                                        if (lstFiltro[i].Temas[l].toLowerCase().indexOf(termos[k]) > -1) {
                                            lstFiltroAnd.push(lstFiltro[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                            lstFiltro = lstFiltroAnd;
                        }
                    }
                    jsnQuestoesFiltro = jsnQuestoesFiltro.concat(lstFiltro);
                }
                else if (strTexto[j]) {
                    for (var i = 0; i < jsnQuestoesGeral.length; i++) {
                        if (jsnQuestoesGeral[i].Enunciado.toLowerCase().indexOf(strTexto[j]) > -1) {
                            jsnQuestoesFiltro.push(jsnQuestoesGeral[i]);
                        }
                        else if (jsnQuestoesGeral[i].Disciplina.toLowerCase().indexOf(strTexto[j]) > -1) {
                            jsnQuestoesFiltro.push(jsnQuestoesGeral[i]);
                        }
                        else if (jsnQuestoesGeral[i].TipoQuestao.toLowerCase().indexOf(strTexto[j]) > -1) {
                            jsnQuestoesFiltro.push(jsnQuestoesGeral[i]);
                        }
                        else if (jsnQuestoesGeral[i].Dificuldade.toLowerCase().indexOf(strTexto[j]) > -1) {
                            jsnQuestoesFiltro.push(jsnQuestoesGeral[i]);
                        }
                        else if (jsnQuestoesGeral[i].DtCadastro.toLowerCase().indexOf(strTexto[j]) > -1) {
                            jsnQuestoesFiltro.push(jsnQuestoesGeral[i]);
                        }
                        else {
                            for (var k = 0; k < jsnQuestoesGeral[i].Temas.length; k++) {
                                if (jsnQuestoesGeral[i].Temas[k].toLowerCase().indexOf(strTexto[j]) > -1) {
                                    jsnQuestoesFiltro.push(jsnQuestoesGeral[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            var strCod = '';
            for (var i = 0; i < jsnQuestoesFiltro.length; i++) {
                if (strCod.indexOf(jsnQuestoesFiltro[i].CodQuestao) == -1) {
                    strCod += jsnQuestoesFiltro[i].CodQuestao + ';';
                    continue;
                }
            }
            var lstQuestoesTemp = jsnQuestoesFiltro;
            var strCodTemp = '';
            jsnQuestoesFiltro = [];
            for (var i = 0; i < lstQuestoesTemp.length; i++) {
                if (strCodTemp.indexOf(lstQuestoesTemp[i].CodQuestao) == -1) {
                    if (strCod.indexOf(lstQuestoesTemp[i].CodQuestao) > -1) {
                        jsnQuestoesFiltro.push(lstQuestoesTemp[i]);
                        strCodTemp += lstQuestoesTemp[i].CodQuestao + ';';
                    }
                }
            }
        }
        else {
            jsnQuestoesFiltro = jsnQuestoesGeral;
        }
        minhasQuestoes();
    }

    function minhasQuestoes(pagina) {
        $('.ui.cards').parent().addClass('loading');
        var qtePorPagina = 20;
        var qtePaginacao = 7;
        var length = jsnQuestoesFiltro.length;
        if (pagina == null) {
            pagina = 1;
        }
        var count = pagina * qtePorPagina;
        if (count > length) {
            count = length;
        }
        $('.ui.cards').html('');
        for (var i = ((qtePorPagina * pagina) - qtePorPagina) ; i < count; i++) {
            $('.ui.cards').append('\
                        <a class="ui card" href="/Historico/Questao/' + jsnQuestoesFiltro[i].CodQuestao + '">\
                            <div class="content">\
                                <div class="header">' + siac.Utilitario.encurtarTextoEm(jsnQuestoesFiltro[i].Enunciado, 140) + '</div>\
                                <div class="meta"><span title="' + jsnQuestoesFiltro[i].DtCadastro + '">' + jsnQuestoesFiltro[i].DtCadastroTempo + '</span></div>\
                                <div class="description ui labels">\
                                    <span class="ui label">' + jsnQuestoesFiltro[i].Disciplina + '</span>\
                                    <span class="ui label">' + jsnQuestoesFiltro[i].Dificuldade + '</span>\
                                    <span class="ui label">' + jsnQuestoesFiltro[i].TipoQuestao + '</span>\
                                </div>\
                            </div>\
                        </a>\
                        ');
        }

        $('.ui.pagination.menu').html('');
        var i = pagina - 3;
        for (i; i <= (Math.ceil(length / qtePorPagina)) - 3 && $('.ui.pagination.menu .item').length < 5; i++) {
            if (i < 1) {
                continue;
            }
            if (i == pagina) {
                $('.ui.pagination.menu').append('<a class="active item" onclick="siac.Questao.Index.topo(); siac.Questao.Index.minhasQuestoes(' + i + ')">' + i + '</a>');
                continue;
            }
            $('.ui.pagination.menu').append('<a class="item" onclick="siac.Questao.Index.topo(); siac.Questao.Index.minhasQuestoes(' + i + ')">' + i + '</a>');
        }
        if ($('.ui.pagination.menu .item').length < 5) {
            for (i; $('.ui.pagination.menu .item').length < 5 && i <= (Math.ceil(length / qtePorPagina)) ; i++) {
                if (i < 1) {
                    continue;
                }
                if (i == pagina) {
                    $('.ui.pagination.menu').append('<a class="active item" onclick="siac.Questao.Index.topo(); siac.Questao.Index.minhasQuestoes(' + i + ')">' + i + '</a>');
                    continue;
                }
                $('.ui.pagination.menu').append('<a class="item" onclick="siac.Questao.Index.topo(); siac.Questao.Index.minhasQuestoes(' + i + ')">' + i + '</a>');
            }
            if (pagina == (Math.ceil(length / qtePorPagina))) {
                if (!((i - 5) < 1)) {
                    $('.ui.pagination.menu').prepend('<a class="item" onclick="siac.Questao.Index.topo(); siac.Questao.Index.minhasQuestoes(' + (i - 5) + ')">' + (i - 5) + '</a>');
                }
            }
        }
        $('.ui.cards').parent().removeClass('loading');
    }

    return {
        iniciar: iniciar,
        topo: topo,
        filtro: filtro,
        minhasQuestoes: minhasQuestoes
    }
})();

siac.Questao.Cadastrar = (function () {
    function iniciar() {
        $('.ui.checkbox').checkbox();

        $('.ui.accordion').accordion();

        $('.ui.dropdown').dropdown();

        $('.tabular.menu .item').tab();

        $('.ui.termo.modal')
            .modal({
                closable: false,
                onApprove: function () {
                    $('#btnPesquisa').popup('show')
                }
            }).modal('show');

        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal')
          .modal({
              onApprove: function () {
                  $('.ui.confirmar.modal .approve').addClass('loading');
                  checarCaptcha();
                  return false;
              }
          });

        $('.ui.confirmar.modal .ui.accordion').accordion({
            onChange: function () {
                $('.ui.confirmar.modal').modal('refresh');
            }
        });

        $('#btnPesquisa').popup({
            title: 'Pesquisar questão',
            content: 'Clique aqui para descobrir se sua questão já foi cadastrada',
            duration: 100
        });

        $('.ui.pesquisa.modal').modal();

        mostrarCamposPorTipo();

        mostrarOpcaoAnexos();

        for (var i = 0; i < 5; i++) {
            adicionarAlternativa();
        }

        // Eventos
        $('.prosseguir.button').click(function () {
            verificar();
        });

        $('#btnPesquisa').click(function () {
            $('.ui.pesquisa.modal').modal('show');
        });

        $('.pesquisa.modal .pesquisar.button').click(function () {
            palavraChave($('#txtPalavraChave').val());
            return false;
        });

        $('.captcha.icon').click(function () {
            novoCaptcha();
        });

        $('#ddlDisciplina').change(function () {
            recuperarTemasPorCodDisciplina();
        });

        $('#ddlTipo').change(function () {
            mostrarCamposPorTipo();
        });

        $('#chkAnexos').change(function () {
            mostrarOpcaoAnexos();
        });

        $('.questao.objetiva .adicionar.button').click(function () {
            adicionarAlternativa();
        });

        $('.anexos .adicionar.button').click(function () {
            adicionarAnexo();
        });
    }

    function recuperarTemasPorCodDisciplina() {
        var selecionado = $('#ddlDisciplina :selected').val();
        var ddlTema = $('#ddlTema');
        ddlTema.parent().addClass('loading');
        $.ajax({
            cache: false,
            type: 'GET',
            url: '/Tema/RecuperarTemasPorCodDisciplina',
            data: { "codDisciplina": selecionado },
            success: function (data) {
                ddlTema.html('');
                ddlTema.parent().find('.label').remove();
                $.each(data, function (id, option) {
                    ddlTema.append($('<option></option>').val(option.CodTema).html(option.Descricao));
                });
                ddlTema.parent().removeClass('loading');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                siac.mensagem('Falha ao recuperar temas.');
                ddlTema.parent().removeClass('loading');
            }
        });
    }

    function mostrarCamposPorTipo() {
        var tipo = $('#ddlTipo :selected').val();
        $('.questao').hide();
        if (tipo == 1) {
            $('.questao.objetiva').show();
            $('.segment.questao.objetiva').attr('style', '');
        }
        else if (tipo == 2) {
            $('.questao.discursiva').show();
        }
    }

    function mostrarOpcaoAnexos() {
        var chk = $('#chkAnexos');
        if (chk.is(':checked')) {
            $('.anexos').show();
            $('.segment.anexos').attr('style', '');
        }
        else {
            $('.anexos').hide();
        }
    }

    function marcarCorreta(chk) {
        $chk = $(chk);
        $lstCheckboxes = $('.ui.alternativas.accordion .content input[type="checkbox"]');
        $('.ui.alternativas.accordion .title .label').attr('style', 'display:none');
        for (var i = 0; i < $lstCheckboxes.length; i++) {
            if ($chk.is(':checked')) {
                if ($chk.attr('id') == $lstCheckboxes.eq(i).attr('id')) {
                    $('.ui.alternativas.accordion .title.active .label').removeAttr('style');
                    continue;
                }
                $lstCheckboxes.eq(i).attr({ 'readonly': 'readonly', 'disabled': 'disabled' });
                $lstCheckboxes.eq(i).parent().addClass('disabled');
            }
            else {
                $lstCheckboxes.eq(i).removeAttr('readonly disabled');
                $lstCheckboxes.eq(i).parent().removeClass('disabled');
            }
        }
    }

    function carregarImagem(file) {
        var ind = $(file).parent().parent().parent().prev().find('#txtAnexoIndex').val();
        var pre = $('#preImagem' + ind);
        var arq = $('#fleAnexo' + ind)[0].files[0];
        var reader = new FileReader();

        if (arq.size < 1000000) {
            reader.onloadend = function () {
                pre.attr('src', reader.result);
            }

            if (arq) {
                reader.readAsDataURL(arq);
            } else {
                preview.attr('src', '');
            }
        }
        else {
            siac.mensagem("Selecione um arquivo menor.");
        }
    }

    function adicionarAlternativa() {
        var i = $('.ui.alternativas.accordion .title').length + 1;
        $('.ui.alternativas.accordion').append('\
                        <div class="title">\
                            <input id="txtAlternativaIndex" value="'+ i + '" hidden />\
                            <i class="dropdown icon"></i>Alternativa ' + i + '\
                            <div class="ui small label" style="display:none">Correta</div>\
                        </div>\
                        <div class="content ui segment">\
                            <div class="field required">\
                                <label for="txtAlternativaEnunciado' + i + '">Enunciado</label>\
                                <textarea id="txtAlternativaEnunciado' + i + '" name="txtAlternativaEnunciado' + i + '" rows="2" placeholder="Enunciado..."></textarea>\
                            </div>\
                            <div class="field">\
                                <label for="txtAlternativaComentario' + i + '">Comentário</label>\
                                <textarea id="txtAlternativaComentario' + i + '" name="txtAlternativaComentario' + i + '" rows="2" placeholder="Comentário..."></textarea>\
                            </div>\
                            <div class="field">\
                                <div class="ui toggle checkbox">\
                                    <input id="chkAlternativaCorreta' + i + '" name="chkAlternativaCorreta' + i + '" type="checkbox" tabindex="0" class="hidden">\
                                    <label for="chkAlternativaCorreta' + i + '">Correta</label>\
                                </div>\
                            </div>\
                            <div class="field">\
                                <div class="ui button">\
                                    Remover\
                                </div>\
                                <div class="ui special popup">\
                                    <div class="header">Tem certeza?</div>\
                                    <div class="content"><p>Essa ação não poderá ser desfeita.</p>\
                                    <div class="ui right aligned remover button tiny">Sim, remover</div>\
                                </div>\
                            </div>\
                        </div>\
                    </div>');
        $('#txtQtdAlternativas').val($('.ui.alternativas.accordion .title').length);
        $('[id^="chkAlternativaCorreta"]').off().change(function () {
            marcarCorreta(this);
        });
        $('.ui.alternativas.accordion .remover.button').off().click(function () {
            removerAlternativa(this);
        });
        $('.ui.checkbox').checkbox();
        $('.ui.alternativas.accordion .button').popup({ inline: true, on: 'click', position: 'right center' });
    }

    function renomearAlternativas() {
        var list = $('.ui.alternativas.accordion .title');
        var listContent = $('.ui.alternativas.accordion .content.segment');
        for (var i = 0; i < list.length; i++) {
            var j = list.eq(i).find('#txtAlternativaIndex').val();
            list.eq(i).html('<input id="txtAlternativaIndex" value="' + (i + 1) + '" hidden /><i class="dropdown icon"></i>Alternativa ' + (i + 1) + '<div class="ui small label" style="display:none">Correta</div>');
            if (listContent.eq(i).find('input[name="chkAlternativaCorreta' + j + '"]').is(':checked')) {
                console.log('entrou');
                list.eq(i).find('.label').removeAttr('style');
            }
            /* RENOMEAR LABELS, INPUTS e TEXTAREAS */
            listContent.eq(i).find('label[for="txtAlternativaEnunciado' + j + '"]').attr('for', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('label[for="txtAlternativaComentario' + j + '"]').attr('for', 'txtAlternativaComentario' + (i + 1));
            listContent.eq(i).find('label[for="chkAlternativaCorreta' + j + '"]').attr('for', 'chkAlternativaCorreta' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAlternativaEnunciado' + j + '"]').attr('name', 'txtAlternativaEnunciado' + (i + 1)).attr('id', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAlternativaComentario' + j + '"]').attr('name', 'txtAlternativaComentario' + (i + 1)).attr('id', 'txtAlternativaComentario' + (i + 1));
            listContent.eq(i).find('input[name="chkAlternativaCorreta' + j + '"]').attr('name', 'chkAlternativaCorreta' + (i + 1)).attr('id', 'chkAlternativaCorreta' + (i + 1));
        }
    }

    function removerAlternativa(button) {
        var i = $('.ui.alternativas.accordion .title').length;
        if (i > 2) {
            var content = $(button).parent().parent().parent().parent();
            var title = $(content).prev();
            title.remove();
            content.remove();
            renomearAlternativas();
        }
        $('#txtQtdAlternativas').val($('.ui.alternativas.accordion .title').length);
    }

    function adicionarAnexo() {
        var tipoAnexo = $('#ddlTiposAnexo').val();
        var tipoAnexoDescricao = $('#ddlTiposAnexo :selected').text();
        var i = $('.ui.anexos.accordion .title').length + 1;
        if (tipoAnexo == 1) {
            $('.ui.anexos.accordion').append('\
                        <div class="title">\
                            <input id="txtAnexoIndex" value="'+ i + '" class="disabled" readonly hidden/>\
                            <input name="txtAnexoTipo' + i + '" id="txtAnexoTipo' + i + '" value="' + tipoAnexo + '" hidden />\
                            <i class="dropdown icon"></i>Anexo ' + i + '\
                            <span class="ui label" id="txtAnexoTipoDescricao">'+ tipoAnexoDescricao + '</span>\
                        </div>\
                        <div class="content ui segment">\
                            <div class="field required">\
                                <div class="ui card"><div class="image"><img name="preImagem' + i + '" id="preImagem' + i + '" alt="Previsualização" src="" /></div>\
                                <input id="fleAnexo' + i + '" name="fleAnexo' + i + '"type="file" class="ui button" accept="image/*" /></div>\
                            </div>\
                            <div class="field required">\
                                <label for="txtAnexoLegenda' + i + '">Legenda</label>\
                                <textarea maxlength="250" id="txtAnexoLegenda' + i + '" name="txtAnexoLegenda' + i + '" rows="2" placeholder="Legenda..."></textarea>\
                            </div>\
                            <div class="field">\
                                <label for="txtAnexoFonte' + i + '">Fonte</label>\
                                <input maxlength="250" type="text" id="txtAnexoFonte' + i + '" name="txtAnexoFonte' + i + '" placeholder="Fonte..."></textarea>\
                            </div>\
                            <div class="field">\
                                <div class="ui button">\
                                    Remover\
                                </div>\
                                <div class="ui special popup">\
                                    <div class="header">Tem certeza?</div>\
                                    <div class="content"><p>Essa ação não poderá ser desfeita.</p>\
                                    <div class="ui right aligned remover button tiny">Sim, remover</div>\
                                </div>\
                            </div>\
                        </div>\
                    </div>');
        }
        $('.anexos.accordion [id^="fleAnexo"]').off().change(function () {
            carregarImagem(this);
        });
        $('#txtQtdAnexos').val($('.ui.anexos.accordion .title').length);
        $('.anexos.accordion .remover.button').off().click(function () {
            removerAnexo(this);
        });
        $('.ui.anexos.accordion .button').popup({ inline: true, on: 'click', position: 'right center' });
    }

    function renomearAnexos() {
        var list = $('.ui.anexos.accordion .title');
        var listContent = $('.ui.anexos.accordion .content.segment');
        for (var i = 0; i < list.length; i++) {
            var j = list.eq(i).find('#txtAnexoIndex').val();
            var tipoAnexo = list.eq(i).find('#txtAnexoTipo' + j).val();
            var tipoAnexoDescricao = list.eq(i).find('#txtAnexoTipoDescricao').text();
            list.eq(i).html('<input id="txtAnexoIndex" value="' + (i + 1) + '" class="disabled" readonly hidden/><i class="dropdown icon"></i>Anexo ' + (i + 1) + '<input name="txtAnexoTipo' + (i + 1) + '" id="txtAnexoTipo' + (i + 1) + '" value="' + tipoAnexo + '" hidden /><span class="ui label" id="txtAnexoTipoDescricao">' + tipoAnexoDescricao + '</span>');
            listContent.eq(i).find('label[for="txtAnexoLegenda' + j + '"]').attr('for', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('label[for="txtAnexoFonte' + j + '"]').attr('for', 'txtAlternativaComentario' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAnexoLegenda' + j + '"]').attr('name', 'txtAnexoLegenda' + (i + 1)).attr('id', 'txtAnexoLegenda' + (i + 1));
            listContent.eq(i).find('input[name="txtAnexoFonte' + j + '"]').attr('name', 'txtAnexoFonte' + (i + 1)).attr('id', 'txtAnexoFonte' + (i + 1));
            listContent.eq(i).find('input[name="fleAnexo' + j + '"]').attr('name', 'fleAnexo' + (i + 1)).attr('id', 'fleAnexo' + (i + 1));
            listContent.eq(i).find('img[name="preImagem' + j + '"]').attr('name', 'preImagem' + (i + 1)).attr('id', 'preImagem' + (i + 1));
        }
    }

    function removerAnexo(button) {
        var content = $(button).parent().parent().parent().parent();
        var title = $(content).prev();
        title.remove();
        content.remove();
        renomearAnexos();
        $('#txtQtdAnexos').val($('.ui.anexos.accordion .title').length);
    }

    function verificar() {
        $('.ui.error.message .list').html('');
        var validado = false;
        if (!$('#ddlDisciplina').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar uma disciplina</li>')
        }

        if (!$('#ddlTema :selected').length > 0) {
            $('.ui.error.message .list').append('<li>É necessário selecionar pelo menos um tema</li>')
        }
        if (!$('#ddlDificuldade').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar uma dificuldade</li>')
        }

        if (!$('#ddlTipo').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar um tipo</li>')
        }

        if (!$('#txtEnunciado').val()) {
            $('.ui.error.message .list').append('<li>É necessário preencher o enunciado</li>')
        }

        if ($('#ddlTipo').val()) {
            var tipo = $('#ddlTipo').val();

            if (tipo == 1) {
                var qteAlternativas = $('#txtQtdAlternativas').val();
                var ok = true;
                for (var i = 0; i < qteAlternativas; i++) {
                    if ($('#txtAlternativaEnunciado' + (i + 1)).val() == '') {
                        ok = false;
                    }
                }
                if (ok) {
                    ok = false;
                    for (var i = 0; i < qteAlternativas; i++) {
                        if ($('#chkAlternativaCorreta' + (i + 1)).is(':checked')) {
                            ok = true;
                        }
                    }
                    if (ok) {
                        if ($('#chkAnexos').is(':checked')) {
                            var qteAnexos = $('#txtQtdAnexos').val();
                            for (var i = 0; i < qteAnexos; i++) {
                                if ($('#fleAnexo' + (i + 1))[0].files.length == 0) {
                                    ok = false;
                                }
                            }
                            if (ok) {
                                validado = true;
                            }
                            else {
                                $('.ui.error.message .list').append('<li>É necessário selecionar os arquivos dos anexos</li>')
                            }
                        }
                        else {
                            validado = true;
                        }
                    }
                    else {
                        $('.ui.error.message .list').append('<li>É necessário selecionar pelo menos umas das alternativas como correta</li>')
                    }
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher os enunciados de todas as alternativas</li>')
                }
            }
            else if (tipo == 2) {
                if ($('#txtChaveDeResposta').val() != '') {
                    if ($('#chkAnexos').is(':checked')) {
                        var ok = true;
                        var qteAnexos = $('#txtQtdAnexos').val();
                        for (var i = 0; i < qteAnexos; i++) {
                            if ($('#fleAnexo' + (i + 1))[0].files.length == 0) {
                                ok = false;
                            }
                        }
                        if (ok) {
                            validado = true;
                        }
                        else {
                            $('.ui.error.message .list').append('<li>É necessário selecionar os arquivos dos anexos</li>')
                        }
                    }
                    else {
                        validado = true;
                    }
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher a chave de resposta</li>')
                }
            }
        }

        if (validado) confirmar();
        else $('#frmQuestao').addClass('error');
    }

    function confirmar() {
        $('#mdlDisciplina').text($('#ddlDisciplina :selected').text());
        $('#mdlDificuldade').text($('#ddlDificuldade :selected').text());
        var temas = $('#ddlTema :selected');
        $('#mdlTagTemas').html('');
        for (var i = 0; i < temas.length; i++) {
            $('#mdlTagTemas').append('<div class="ui tag label">' + temas.eq(i).text() + '</div>');
        }
        $('#mdlEnunciado').text($('#txtEnunciado').val()).attr('data-html', '<b>Objetivo</b>: ' + $('#txtObjetivo').val());
        var tipo = $('#ddlTipo').val();
        if (tipo == 1) {
            var qteAlternativas = $('#txtQtdAlternativas').val();
            $('#mdlListAlternativas').html('');
            for (var i = 0; i < qteAlternativas; i++) {
                if ($('#chkAlternativaCorreta' + (i + 1)).is(':checked')) {
                    $('#mdlListAlternativas')
                    .append('<div class="item">\
                                    <div class="middle aligned content" data-html="<b>Comentário</b>: ' + $('#txtAlternativaComentario' + (i + 1)).val() + '">\
                                        <b>' + (i + 1) + '</b>) ' + $('#txtAlternativaEnunciado' + (i + 1)).val() + '<a class="ui green label">Correta</a>\
                                    </div>\
                                </div>'
                    );
                    continue;
                }
                $('#mdlListAlternativas')
                    .append('<div class="item">\
                                    <div class="middle aligned content" data-html="<b>Comentário</b>: ' + $('#txtAlternativaComentario' + (i + 1)).val() + '">\
                                        <b>' + (i + 1) + '</b>) ' + $('#txtAlternativaEnunciado' + (i + 1)).val() + '\
                                    </div>\
                                </div>'
                    );
            }
            $('#mdlChaveDeResposta').attr('style', 'display:none');
            $('#mdlListAlternativas').removeAttr('style');
        }
        else if (tipo == 2) {
            $('#mdlChaveDeResposta').html('<i>' + $('#txtChaveDeResposta').val() + '</i><a class="ui green label">Chave de Resposta</a>').attr('data-html', '<b>Comentário</b>: ' + $('#txtComentario').val());
            $('#mdlListAlternativas').attr('style', 'display:none');
            $('#mdlChaveDeResposta').removeAttr('style');
        }
        if ($('#chkAnexos').is(':checked')) {
            var qteAnexos = $('#txtQtdAnexos').val();
            $('#mdlCardAnexos').html('');
            for (var i = 0; i < qteAnexos; i++) {
                var tipoAnexo = $('#txtAnexoTipo' + (i + 1)).val();
                if (tipoAnexo == 1) {
                    $('#mdlCardAnexos')
                        .append('<div class="ui card">\
                                        <div class="image">\
                                            <img src="' + $('#preImagem' + (i + 1)).attr('src') + '" />\
                                        </div>\
                                        <div class="content">\
                                            <div class="header">\
                                                ' + $('#txtAnexoLegenda' + (i + 1)).val() + '\
                                            </div>\
                                            <div class="description">\
                                                ' + $('#txtAnexoFonte' + (i + 1)).val() + '\
                                            </div>\
                                        </div>\
                                    </div>'
                    );
                }
            }
            $('#mdlAccordionAnexos').removeAttr('style');
        }
        else {
            $('#mdlAccordionAnexos').attr('style', 'display:none');
        }
        $('.ui.confirmar.modal').modal('show');
        $('.ui.confirmar.modal div').popup();
    }

    function palavraChave(palavras) {
        palavras = palavras.toLowerCase();

        if (palavras.indexOf(";") > -1) {
            palavras = palavras.split(';');
        } else {
            palavras[0] = palavras;
        }
        $('.ui.pesquisa.modal .pesquisar').addClass('loading');
        $.ajax({
            type: 'POST',
            data: { "palavras": palavras },
            url: '/PalavrasChave',
            success: function (data) {
                $('.ui.pesquisa.modal .pesquisar').removeClass('loading');
                $('#divQuestoes').html('');
                if (data.length != 0) {
                    $('#divQuestoes').append('<div class="ui label"> Resultado(s)<div class="detail">' + data.length + '</div></div>');
                    for (var i = 0; i < data.length; i++) {
                        $('#divQuestoes').append('\
                        <div class="item">\
                            <div class="content">\
                                <a href="/Historico/Questao/' + data[i].CodQuestao + '" class="header">' + siac.Utilitario.encurtarTextoEm(data[i].Enunciado, 140) + '</a>\
                                <div class="description ui labels">\
                                    <span class="ui label">' + data[i].Disciplina + '</span>\
                                    <span class="ui label">' + data[i].Dificuldade + '</span>\
                                    <span class="ui label">' + data[i].TipoQuestao + '</span>\
                                    <span class="ui label">' + data[i].Professor + '</span>\
                                </div>\
                            </div>\
                        </div>\
                        ');
                    }
                } else {
                    $('#divQuestoes').append('\
                        <div class="item">\
                            <div class="content">\
                            <div class="header">Nenhuma questão foi encontrada</div>\
                            </div>\
                        </div>\
                        ');
                }
                $('.ui.pesquisa.modal').modal('refresh');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                siac.mensagem('Erro na pesquisa. Por favor, tente novamente.');
            }
        });
    }

    function checarCaptcha() {
        $.ajax({
            type: 'GET',
            data: { captcha: $('#txtCaptcha').val() },
            url: "/Dashboard/Questao/ChequeCaptcha",
            success: function (resp) {
                if (resp == "true") {
                    $('.ui.pesquisa.modal .pesquisar').removeClass('loading');
                    $('.ui.confirmar.modal').modal('hide');
                    $('#txtCaptcha').next().prop('class', 'checkmark green icon');
                    $('#frmQuestao').addClass('loading').submit();
                }
                else {
                    $('.ui.pesquisa.modal .pesquisar').removeClass('loading');
                    $('.ui.confirmar.modal').modal('show');
                    $('#txtCaptcha').next().prop('class', 'remove red icon');
                }
            },
            error: function () {
                $('.ui.pesquisa.modal .pesquisar').removeClass('loading');
                $('#txtCaptcha').next().prop('class', 'remove icon');
                siac.mensagem("Ocorreu um erro ao verificar o captcha.", "Resultado do Captcha");
            }
        });
    }

    function novoCaptcha() {
        $.ajax({
            type: 'GET',
            url: "/Dashboard/Questao/NovoCaptcha",
            success: function (strBase64) {
                if (strBase64) {
                    $('#imgCaptcha').attr('src', 'data:image/png;base64,' + strBase64);
                }
            },
            error: function () {
                siac.mensagem("Ocorreu um erro ao atualizar o captcha, tente novamente.", "Erro");
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Questao.Detalhe = (function () {
    function iniciar() {
        $('.ui.accordion')
            .accordion()
        ;
        $('div,p')
            .popup()
        ;
        $('.button')
        .popup({
            on: 'click'
        })
        ;
        $('a.card').click(function () {
            $card = $(this);
            src = $card.find('img').attr('src');
            legenda = $card.find('.header').text();
            fonte = $card.find('.description').text();

            $modal = $('.ui.anexo.modal');

            $modal.find('.header').text(legenda);
            $modal.find('img.image').attr('src', src);
            $modal.find('.description').html(fonte);

            $modal.modal('show');
        });
        $('.arquivar.button').click(function () {
            var _this = this;
            $(_this).addClass('loading');
            $.ajax({
                url: '/Historico/Questao/Arquivar/'+$(_this).attr('data-questao'),
                type: 'POST',
                success: function (flag) {
                    if (flag) {
                        $(_this).addClass('active').text('Arquivada');
                    }
                    else {
                        $(_this).removeClass('active').text('Arquivar');
                    }
                    $(_this).removeClass('loading');
                },
                error: function () {
                    $(_this).removeClass('loading');
                }
            });
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Questao.Editar = (function () {
    function iniciar() {
        $('.disabled').parent().popup({
            html: '<i class="icon red warning"></i>Este campo não pode ser modificado',
            variation: 'wide'
        });
        $('.ui.checkbox').checkbox();
        $('.ui.accordion').accordion();
        $('.ui.modal .ui.accordion').accordion({
            onChange: function() { $('.ui.confirmar.modal').modal('refresh'); }
        });
        $('.ui.dropdown').dropdown();
        $('.tabular.menu .item').tab();
        $('.cancelar.button').popup({ on: 'click' });
        $('.ui.confirmar.modal')
          .modal({
              onApprove: function () {
                  $('#frmQuestao').addClass('loading').submit();
              }
          });

        $('.prosseguir.button').click(function (){
            verificar();
        });        
    }

    function confirmar() {
        $('#mdlEnunciado').text($('#txtEnunciado').val()).attr('data-html', '<b>Objetivo</b>: ' + $('#txtObjetivo').val());
        var tipo = $('#frmQuestao').attr('data-questao-tipo');
        if (tipo == 1) {
            var qteAlternativas = $('.alternativas .title').length;
            $('#mdlListAlternativas').html('');
            for (var i = 0; i < qteAlternativas; i++) {
                if ($('#chkAlternativaCorreta' + (i + 1)).is(':checked')) {
                    $('#mdlListAlternativas')
                    .append('<div class="item">\
                                    <div class="middle aligned content" data-html="<b>Comentário</b>: ' + $('#txtAlternativaComentario' + (i + 1)).val() + '">\
                                        <b>' + (i + 1) + '</b>) ' + $('#txtAlternativaEnunciado' + (i + 1)).val() + '<a class="ui green label">Correta</a>\
                                    </div>\
                                </div>'
                    );
                    continue;
                }
                $('#mdlListAlternativas')
                    .append('<div class="item">\
                                    <div class="middle aligned content" data-html="<b>Comentário</b>: ' + $('#txtAlternativaComentario' + (i + 1)).val() + '">\
                                        <b>' + (i + 1) + '</b>) ' + $('#txtAlternativaEnunciado' + (i + 1)).val() + '\
                                    </div>\
                                </div>'
                    );
            }
            $('#mdlChaveDeResposta').attr('style', 'display:none');
            $('#mdlListAlternativas').removeAttr('style');
        }
        else if (tipo == 2) {
            $('#mdlChaveDeResposta').html('<i>' + $('#txtChaveDeResposta').val() + '</i><a class="ui green label">Chave de Resposta</a>').attr('data-html', '<b>Comentário</b>: ' + $('#txtComentario').val());
            $('#mdlListAlternativas').attr('style', 'display:none');
            $('#mdlChaveDeResposta').removeAttr('style');
        }
        var qteAnexos = $('#frmQuestao').attr('data-questao-anexo');
        if (qteAnexos > 0) {
            $('#mdlCardAnexos').html('');
            for (var i = 0; i < qteAnexos; i++) {
                var tipoAnexo = $('#txtAnexoTipo' + (i + 1)).val();
                if (tipoAnexo == 1) {
                    $('#mdlCardAnexos')
                        .append('<div class="ui card">\
                                        <div class="image">\
                                            <img src="' + $('#preImagem' + (i + 1)).attr('src') + '" />\
                                        </div>\
                                        <div class="content">\
                                            <div class="header">\
                                                ' + $('#txtAnexoLegenda' + (i + 1)).val() + '\
                                            </div>\
                                            <div class="description">\
                                            ' + $('#txtAnexoFonte' + (i + 1)).val() + '\
                                            </div>\
                                        </div>\
                                    </div>'
                    );
                }
            }
            $('#mdlAccordionAnexos').removeAttr('style');
        }
        else {
            $('#mdlAccordionAnexos').attr('style', 'display:none');
        }
        $('.ui.confirmar.modal').modal('show');
        $('.ui.confirmar.modal div').popup();
    }

    function verificar() {
        $('.ui.error.message .list').html('');
        var validado = false;
        var tipo = $('#frmQuestao').attr('data-questao-tipo');
        if ($('#txtEnunciado').val() != '') {
            if (tipo == 1) {
                var qteAlternativas = $('.alternativas .title').length;
                var ok = true;
                for (var i = 0; i < qteAlternativas; i++) {
                    if ($('#txtAlternativaEnunciado' + (i + 1)).val() == '') {
                        ok = false;
                    }
                }
                if (ok) {
                    ok = false;
                    for (var i = 0; i < qteAlternativas; i++) {
                        if ($('#chkAlternativaCorreta' + (i + 1)).is(':checked')) {
                            ok = true;
                        }
                    }
                    if (ok) {
                        validado = true;
                    }
                    else {
                        $('.ui.error.message .list').append('<li>É necessário selecionar pelo menos umas das alternativas como correta</li>')
                    }
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher os enunciados de todas as alternativas</li>')
                }
            }
            else if (tipo == 2) {
                if ($('#txtChaveDeResposta').val() != '') {
                    validado = true;
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher a chave de resposta</li>')
                }
            }
        }
        else {
            $('.ui.error.message .list').append('<li>É necessário preencher o enunciado</li>')
        }

        if (validado) confirmar();
        else $('#frmQuestao').addClass('error');
    }

    return {
        iniciar: iniciar
    }
})();

siac.Questao.Gerar = (function () {
    function iniciar() {
        $('.gerar.button').click(function () {
            gerar();
        });
    }

    function gerar() {
        $('div.ui.form').parent().addClass('loading');
        strQte = $('#txtQuantidade').val();
        $.ajax({
            type: 'GET',
            url: '/Dashboard/Questao/Gerar',
            data: { "strQte": strQte },
            success: function (data) {
                console.log(data);
                $questoes = $('div.questoes');
                $questoes.html('');
                for (var i = 0; i < data.length; i++) {
                    $questao = $('<div class="ui segment"></div>');

                    $labels = $('<div class="ui labels"></div>');
                    $labelProfessor = $('<div class="ui label">\
                                                Professor\
                                                <div class="detail">\
                                                    '+ data[i].Professor + '\
                                                </div>\
                                            </div>');
                    $labelDisciplina = $('<div class="ui label">\
                                                Disciplina\
                                                <div class="detail">\
                                                    '+ data[i].Disciplina + '\
                                                </div>\
                                            </div>');
                    $labelDificuldade = $('<div class="ui label" data-html="<b>Comentário</b>: ' + data[i].Dificuldade.Comentario + '">\
                                                Dificuldade\
                                                <div class="detail">\
                                                    '+ data[i].Dificuldade.Descricao + '\
                                                </div>\
                                            </div>');
                    $labels.append($labelProfessor).append($labelDisciplina).append($labelDificuldade);

                    for (var j = 0; j < data[i].Tema.length; j++) {
                        $labels.append($('<div class="ui tag label" data-html="<b>Comentário</b>: ' + data[i].Tema[j].Comentario + '"></div>').text(data[i].Tema[j].Descricao));
                    }

                    $enunciado = $('<div class="ui dividing header" data-html="<b>Objetivo</b>: ' + data[i].Objetivo + '"></div>');
                    $enunciado.text(data[i].Enunciado);

                    $questao.append($labels);
                    $questao.append($enunciado);

                    if (data[i].TipoQuestao == 1) {
                        $lstAlternativa = $('<div class="ui very relaxed list"></div>');
                        for (var j = 0; j < data[i].Alternativa.length; j++) {
                            $alternativa = $('<div class="item"></div>');
                            $content = $('<div class="middle aligned content" data-html="<b>Comentário</b>: ' + data[i].Alternativa[j].Comentario + '">');
                            $content.append('<b>' + (j + 1) + '</b>) ');
                            $content.append(data[i].Alternativa[j].Enunciado);
                            if (data[i].Alternativa[j].FlagGabarito) {
                                if (data[i].Alternativa[j].FlagGabarito == true) {
                                    $content.append('<a class="ui green label">Correta</a>');
                                }
                            }
                            $alternativa.append($content);
                            $lstAlternativa.append($alternativa);
                        }
                        $questao.append($lstAlternativa);
                    }
                    else {
                        $chaveDeResposta = $('<div data-html="<b>Comentário</b>: ' + data[i].Comentario + '"></div>');
                        $chaveDeResposta.html('<a class="ui green ribbon label">Chave de Resposta</a>' + data[i].ChaveDeResposta);
                        $questao.append($chaveDeResposta);
                    }

                    $questoes.append($questao);
                }
                $('div,p')
                    .popup()
                ;
                $('div.ui.form').parent().removeClass('loading');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('div.ui.form').parent().removeClass('loading');
                siac.mensagem('Erro gerando as questões.');
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.iniciar();