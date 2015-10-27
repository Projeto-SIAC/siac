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
        else if (/\/historico\/questao\/detalhe\/[0-9]+$/.test(pathname)) {
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
        else if (/\/historico\/autoavaliacao\/detalhe\/auto[0-9]+$/.test(pathname)) {
            siac.Autoavaliacao.Detalhe.iniciar();
        }
        else if (/\/dashboard\/autoavaliacao\/realizar\/auto[0-9]+$/.test(pathname)) {
            siac.Autoavaliacao.Realizar.iniciar();
        }
        else if (/\/dashboard\/autoavaliacao\/resultado\/auto[0-9]+$/.test(pathname)) {
            siac.Autoavaliacao.Resultado.iniciar();
        }
        else if (pathname == '/dashboard/autoavaliacao/gerar') {
            siac.Autoavaliacao.Gerar.iniciar();
        }
        else if (pathname == '/dashboard/avaliacao/academica/gerar') {
            siac.Academica.Gerar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/academica\/agendar\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Agendar.iniciar();
        }
        else if (/\/historico\/avaliacao\/academica\/detalhe\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Detalhe.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/academica\/resultado\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Resultado.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/academica\/configurar\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Configurar.iniciar();
        }
        else if (pathname == '/historico/avaliacao/academica/agendada') {
            siac.Academica.Agendada.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/academica\/realizar\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Realizar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/academica\/acompanhar\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Acompanhar.iniciar();
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

    function substituirTodos(texto, valorVelho, valorNovo) {
        var _texto = texto;
        while (_texto.indexOf(valorVelho) > -1) {
            _texto = _texto.replace(valorVelho, valorNovo);
        };
        return _texto;
    }

    function minutosParaStringTempo(minutos) {
        if (minutos > 59) {
            var strHoras = Math.floor(minutos / 60);
            var strMinutos = (minutos % 60);
            return ('0' + strHoras).slice(-2) + 'h' + ('0' + strMinutos).slice(-2) + 'min';
        }
        else {
            return '00h' + ('0' + minutos).slice(-2) + 'min';
        }
    }

    return {
        compararData: compararData,
        dataEFuturo: dataEFuturo,
        encurtarTextoEm: encurtarTextoEm,
        quebrarLinhaEm: quebrarLinhaEm,
        substituirTodos: substituirTodos,
        minutosParaStringTempo: minutosParaStringTempo
    }
})();

siac.iniciar();