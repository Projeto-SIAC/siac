siac.Utilitario = siac.Utilitario || (function () {
	// return 1 = a is bigger than b, 0 = a and b are same, -1 = a is smaller than b
	function compararData(strDateA, strDateB) {
		var timeDateA = Date.parse(strDateA);
		var timeDateB = Date.parse(strDateB);
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

	function validarCPF(strCPF) {
		var Soma;
		var Resto;
		Soma = 0;
		if (strCPF == "00000000000") return false;
		for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
		Resto = (Soma * 10) % 11; if ((Resto == 10) || (Resto == 11)) Resto = 0;
		if (Resto != parseInt(strCPF.substring(9, 10))) return false;
		Soma = 0;
		for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
		Resto = (Soma * 10) % 11; if ((Resto == 10) || (Resto == 11)) Resto = 0;
		if (Resto != parseInt(strCPF.substring(10, 11))) return false;
		return true;
	}

	String.prototype.encurtarTextoEm = function (length) {
		var text = '';
		var str = this;
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

	String.prototype.quebrarLinhaEm = function (indiceMaximo) {
		texto = this;
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

	String.prototype.substituirTodos = function (valorVelho, valorNovo) {
		var _texto = this;
		while (_texto.indexOf(valorVelho) > -1) {
			_texto = _texto.replace(valorVelho, valorNovo);
		};
		return _texto;
	}

	String.prototype.minutosParaStringTempo = function () {
		var minutos = this;
		if (minutos > 59) {
			var strHoras = Math.floor(minutos / 60);
			var strMinutos = (minutos % 60);
			return ('0' + strHoras).slice(-2) + 'h' + ('0' + strMinutos).slice(-2) + 'min';
		}
		else {
			return '00h' + ('0' + minutos).slice(-2) + 'min';
		}
	}

	Number.prototype.inteiroParaLetraMinuscula = function () {
		var numero = this;
		var letras = " abcdefghijklmnopqrstuvxwyz";
		if (numero < letras.length) {
			return letras[numero];
		}
		else {
			return numero;
		}
	}

	return {
		compararData: compararData,
		dataEFuturo: dataEFuturo,
		validarCPF: validarCPF
	}
})();