var siac = siac || (function () {
    function iniciar() {
        $(function () {        
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
        });
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
	    else if (pathname == '/configuracoes') {
	        siac.Configuracoes.iniciar();
	    }
	    else if (pathname == '/historico/autoavaliacao') {
	        siac.Autoavaliacao.Index.iniciar();
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
	function dataEFuturo(strData) {
		timeDataAgora = new Date().getTime();
		timeData = Date.parse(strData);		
	    return (timeData > timeDataAgora);
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

	function substituirTodos(texto, valorVelho,valorNovo) {
	    var _texto = texto;
	    while (_texto.indexOf(valorVelho) > -1) {
	        _texto = _texto.replace(valorVelho, valorNovo);
	    };
	    return _texto;
	}

	return {
		compararData: compararData,
		dataEFuturo: dataEFuturo,
		encurtarTextoEm: encurtarTextoEm,
		quebrarLinhaEm: quebrarLinhaEm,
		substituirTodos: substituirTodos
	}
})();

siac.iniciar();