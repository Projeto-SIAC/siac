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

        if (pathname.startsWith('/dashboard/')) {
            if (pathname.indexOf('/questao') >= 0) {
                if (/\/dashboard\/questao\/cadastrar/.test(pathname)) {
                    siac.Questao.Cadastrar.iniciar();
                }
            }
            else if (pathname.indexOf('/autoavaliacao') >= 0) {
                if (/\/dashboard\/autoavaliacao\/realizar\/auto[0-9]+$/.test(pathname)) {
                    siac.Autoavaliacao.Realizar.iniciar();
                }
                else if (/\/dashboard\/autoavaliacao\/resultado\/auto[0-9]+$/.test(pathname)) {
                    siac.Autoavaliacao.Resultado.iniciar();
                }
                else if (pathname == '/dashboard/autoavaliacao/gerar') {
                    siac.Autoavaliacao.Gerar.iniciar();
                }
            }
            else if (pathname.indexOf('/academica') >= 0) {
                if (pathname == '/dashboard/avaliacao/academica/gerar') {
                    siac.Academica.Gerar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/academica\/agendar\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Agendar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/academica\/resultado\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Resultado.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/academica\/configurar\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Configurar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/academica\/realizar\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Realizar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/academica\/acompanhar\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Acompanhar.iniciar();
                }
            }
            else if (pathname.indexOf('/reposicao') >= 0) {
                if (/\/dashboard\/avaliacao\/reposicao\/justificar\/acad[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Justificar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/reposicao\/configurar\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Configurar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/reposicao\/agendar\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Agendar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/reposicao\/realizar\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Realizar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/reposicao\/acompanhar\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Acompanhar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/reposicao\/resultado\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Resultado.iniciar();
                }
            }
            else if (pathname.indexOf('/certificacao') >= 0) {
                if (pathname == '/dashboard/avaliacao/certificacao/gerar') {
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
                else if (/\/dashboard\/avaliacao\/certificacao\/realizar\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Realizar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/certificacao\/acompanhar\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Acompanhar.iniciar();
                }
                else if (/\/dashboard\/avaliacao\/certificacao\/resultado\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Resultado.iniciar();
                }
            }
        }
        else if (pathname.startsWith('/historico/')) {
            if (pathname.indexOf('/questao') >= 0) {
                if (pathname == '/historico/questao') {
                    siac.Questao.Index.iniciar();
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
            }
            else if (pathname.indexOf('/autoavaliacao') >= 0) {
                if (pathname == '/historico/autoavaliacao') {
                    siac.Autoavaliacao.Index.iniciar();
                }
                else if (/\/historico\/autoavaliacao\/detalhe\/auto[0-9]+$/.test(pathname)) {
                    siac.Autoavaliacao.Detalhe.iniciar();
                }
            }
            else if (pathname.indexOf('/academica') >= 0) {
                if (pathname == '/historico/avaliacao/academica') {
                    siac.Academica.Index.iniciar();
                }
                else if (/\/historico\/avaliacao\/academica\/detalhe\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Detalhe.iniciar();
                }
                else if (/\/historico\/avaliacao\/academica\/agendada\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Agendada.iniciar();
                }
                else if (/\/historico\/avaliacao\/academica\/corrigir\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Corrigir.iniciar();
                }
            }
            else if (pathname.indexOf('/reposicao') >= 0) {
                if (pathname == '/historico/avaliacao/reposicao') {
                    siac.Reposicao.Index.iniciar();
                }
                else if (/\/historico\/avaliacao\/reposicao\/agendada\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Agendada.iniciar();
                }
                else if (/\/historico\/avaliacao\/reposicao\/corrigir\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Corrigir.iniciar();
                }
                else if (/\/historico\/avaliacao\/reposicao\/detalhe\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Detalhe.iniciar();
                }
            }
            else if (pathname.indexOf('/certificacao') >= 0) {
                if (pathname == '/historico/avaliacao/certificacao') {
                    siac.Certificacao.Index.iniciar();
                }
                else if (/\/historico\/avaliacao\/certificacao\/agendada\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Agendada.iniciar();
                }
                else if (/\/historico\/avaliacao\/certificacao\/corrigir\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Corrigir.iniciar();
                }
                else if (/\/historico\/avaliacao\/certificacao\/detalhe\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Detalhe.iniciar();
                }
                else if (/\/historico\/avaliacao\/certificacao\/detalhe\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Detalhe.iniciar();
                }
            }
        }
        else if (pathname.startsWith('/configuracoes/')) {
            if (/\/configuracoes\/parametros/.test(pathname)) {
                siac.Configuracoes.Parametros.iniciar();
            }
            else if (pathname.indexOf('/visitante') >= 0) {
                if (/\/configuracoes\/visitante\/cadastrar$/.test(pathname)) {
                    siac.Visitante.Cadastrar.iniciar();
                }
                else if (/\/configuracoes\/visitante$/.test(pathname)) {
                    siac.Visitante.Index.iniciar();
                }
                else if (/\/configuracoes\/visitante\/detalhe\/vis[0-9]+$/.test(pathname)) {
                    siac.Visitante.Detalhe.iniciar();
                }
            }
            else if (/\/configuracoes\/opinioes/.test(pathname)) {
                siac.Configuracoes.Opinioes.iniciar();
            }
            else if (/\/configuracoes\/institucional\/coordenadores/.test(pathname)) {
                siac.Configuracoes.Institucional.iniciar();
            }
        }
        else if (pathname.startsWith('/institucional/')) {
            if (pathname == '/institucional/gerar') {
                siac.Institucional.Gerar.iniciar();
            }
            else if (/\/institucional\/configuracao(|\/indicador|\/categoria|\/modulo)$/.test(pathname)) {
                siac.Institucional.Configuracao.iniciar();
            }
            else if (/\/institucional\/questionario\/avi[0-9]+$/.test(pathname)) {
                siac.Institucional.Questionario.iniciar();
            }
            else if (/\/institucional\/configurar\/avi[0-9]+$/.test(pathname)) {
                siac.Institucional.Configurar.iniciar();
            }
            else if (/\/institucional\/publico\/avi[0-9]+$/.test(pathname)) {
                siac.Institucional.Publico.iniciar();
            }
            else if (/\/institucional\/agendar\/avi[0-9]+$/.test(pathname)) {
                siac.Institucional.Agendar.iniciar();
            }
        }
        else if (pathname.startsWith('/perfil')) {
            siac.Perfil.iniciar();
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

    function dataEPassado(strData) {
        timeDataAgora = new Date().getTime();
        timeData = Date.parse(strData);
        return (timeData < timeDataAgora);
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
        dataEPassado: dataEPassado,
        validarCPF: validarCPF
    }
})();

siac.Anexo = siac.Anexo || (function () {
    function iniciar() {
        $('.card.anexo.imagem').off().click(function () {
            var $this = $(this);
            var source = $this.find('img').attr('src');
            var legenda = $this.find('.header').text();
            $description = $this.find('.description');
            var fonte = $description.data('fonte') ? $description.data('fonte') : $description.text();

            expandirImagem(source, legenda, fonte);
        });
        $('.card.anexo.codigo').off().click(function () {
            var $this = $(this);
            var codigo = $this.find('textarea.codigo').val();

            $description = $this.find(':not(.extra).content .description');
            var observacao = $description.data('observacao') ? $description.data('observacao') : $description.text();

            $description = $this.find('.extra.content .description');
            var fonte = $description.data('fonte') ? $description.data('fonte') : $description.text();

            expandirCodigo(codigo, observacao, fonte);
        });
    }

    function expandirCodigo(codigo, observacao, fonte) {
        $('.ui.anexo.modal').remove();

        codigo = codigo.substituirTodos('&', '&gt;');
        codigo = codigo.substituirTodos('<', '&lt;');
        codigo = codigo.substituirTodos('>', '&gt;');

        $modal = $('<div></div>').addClass('ui large anexo basic modal');

        $modal.append($('<div class="ui centered header"></div>').text(observacao));
        $modal.append($('<div class="content"></div>').html($('<pre></pre>').html($('<code></code>').html(codigo))));
        $modal.append($('<div class="description" style="text-align:center;"></div>').text(fonte));

        $('body').append($modal);

        $modal.modal('show');

        $modal.click(function () {
            $(this).modal('hide');
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