var siac = siac || (function () {
    function iniciar() {
        $(function () {
            document.onreadystatechange = function () {
                if (document.readyState == "complete") {
                    $('.ui.global.loader').parent().dimmer('hide');
                }
            }

            window.onbeforeunload = function () { $('.ui.global.loader').parent().dimmer('show'); };

            $(document).keydown(function (e) {
                if (e.keyCode == 27) {
                    $('.ui.global.loader').parent().dimmer('hide');
                }
            });
            
            $('.ui.global.loader').parent().click(function () {
                try {
                    window.stop();
                }
                catch (e) {
                    document.execCommand('Stop');
                }
                $('.ui.global.loader').parent().dimmer('hide');
            });

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
        else if (pathname == '/historico/avaliacao/academica') {
            siac.Academica.Index.iniciar();
        }
        else if (pathname == '/historico/avaliacao/certificacao') {
            siac.Certificacao.Index.iniciar();
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
        else if (/\/historico\/avaliacao\/academica\/agendada\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Agendada.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/academica\/realizar\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Realizar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/academica\/acompanhar\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Acompanhar.iniciar();
        }
        else if (/\/historico\/avaliacao\/academica\/corrigir\/acad[0-9]+$/.test(pathname)) {
            siac.Academica.Corrigir.iniciar();
        }
        else if (pathname == '/dashboard/avaliacao/certificacao/gerar') {
            siac.Certificacao.Gerar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/certificacao\/configurar\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Configurar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/certificacao\/agendar\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Agendar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/certificacao\/avaliados\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Avaliados.iniciar();
        }
        else if (/\/historico\/avaliacao\/certificacao\/agendada\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Agendada.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/certificacao\/realizar\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Realizar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/certificacao\/acompanhar\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Acompanhar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/certificacao\/resultado\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Resultado.iniciar();
        }
        else if (/\/historico\/avaliacao\/certificacao\/corrigir\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Corrigir.iniciar();
        }
        else if (/\/historico\/avaliacao\/certificacao\/detalhe\/cert[0-9]+$/.test(pathname)) {
            siac.Certificacao.Detalhe.iniciar();
        }
        else if (/\/institucional\/configurar(|\/indicador|\/categoria|\/modulo)$/.test(pathname)) {
            siac.Institucional.Configurar.iniciar();
        }
        else if (pathname == '/institucional/gerar') {
            siac.Institucional.Gerar.iniciar();            
        }
        else if (/\/dashboard\/avaliacao\/reposicao\/justificar\/acad[0-9]+$/.test(pathname)) {
            siac.Reposicao.Justificar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/reposicao\/configurar\/repo[0-9]+$/.test(pathname)) {
            siac.Reposicao.Configurar.iniciar();
        }
        else if (/\/dashboard\/avaliacao\/reposicao\/agendar\/repo[0-9]+$/.test(pathname)) {
            siac.Reposicao.Agendar.iniciar();
        }
        else if (pathname == '/historico/avaliacao/reposicao') {
            siac.Reposicao.Index.iniciar();
        }
    }

    var HTML_MENSAGEM_TEMPLATE = '<div class="ui small mensagem modal">' +
                    '<div class="header">{titulo}</div>' +
                    '<div class="content">{mensagem}</div>' +
                    '<div class="actions">' +
                        '<div class="ui cancel button">Fechar</div>' +
                    '</div>' +
                '</div>';

    function mensagem(mensagem, titulo) {
        if (mensagem) {
            var html = HTML_MENSAGEM_TEMPLATE;
            titulo ? html = html.replace('{titulo}', titulo) : html = html.replace('{titulo}', 'Mensagem');
            html = html.replace('{mensagem}', mensagem);
            $('body').append(html);
            $('.ui.mensagem.modal')
                .modal({
                    transition: 'fly down',
                    onHidden: function () {
                        $(this).remove();
                    }
                })
				.modal('show')
            ;
        }
    }

    var HTML_AVISO_TEMPLATE = '<div class="ui aviso sidebar top center aligned segment">' +
                                    '<h3 class="ui center aligned {cor} header">' +
                                        '<div class="content">' +
                                            '{mensagem}' +
                                        '</div>' +
                                    '</h3>' +
                                '</div>';

    function aviso(mensagem, cor, icone) {
        if (mensagem) {
            $html = $(HTML_AVISO_TEMPLATE);
            $html.html($html.html().substituirTodos('{mensagem}', mensagem));
            $html.html($html.html().substituirTodos('{cor}', cor));
            if (icone) $html.find('.header').addClass('small icon').prepend('<i class="' + icone + ' icon"></i>');
            $('body').prepend($html);
            $('.ui.aviso.sidebar')
                .sidebar({
                    dimPage: false,
                    transition: 'overlay',
                    onHidden: function () {
                        $(this).remove();
                    }
                })
                .sidebar('hide')
				.sidebar('show')
            ;
        }
    }

    return {
        iniciar: iniciar,
        mensagem: mensagem,
        aviso: aviso
    }
})();

siac.Utilitario = siac.Utilitario || (function () {
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

    return {
        compararData: compararData,
        dataEFuturo: dataEFuturo
    }
})();

siac.Anexo = siac.Anexo || (function () {
    function iniciar() {
        $('.card.anexo.imagem').off().click(function () {
            var $this = $(this);
            var source = $this.find('img').attr('src');
            var legenda = $this.find('.header').text();
            $description = $this.find('.description');
            var fonte = $description.attr("data-fonte") ? $description.data('fonte') : $description.text();

            expandirImagem(source, legenda, fonte);
        });
    }

    function expandirImagem(source, legenda, fonte) {
        $('.ui.anexo.modal').remove();

        $modal = $('<div></div>').addClass('ui large anexo basic modal');//.append($('<i></i>').addClass('close icon'));

        $modal.append($('<div class="ui centered header"></div>').text(legenda));
        $modal.append($('<div class="image content"></div>').html($('<img class="ui centered image" />').attr('src', source)));
        $modal.append($('<div class="description" style="text-align:center;"></div>').text(fonte));

        $('body').append($modal);

        $modal.modal('show');

        $modal.click(function () {
            $(this).modal('hide');
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.iniciar();