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

            //$('.ui.global.loader').parent().click(function () {
            //    try {
            //        window.stop();
            //    }
            //    catch (e) {
            //        document.execCommand('Stop');
            //    }
            //    $('.ui.global.loader').parent().dimmer('hide');
            //});

            carregar();
            siac.Lembrete.iniciar();
            habilitarAjuda();
        });
    }

    function carregar() {
        var pathname = window.location.pathname.toLowerCase();
        if (pathname.indexOf('/principal/') == 0) {
            if (pathname.indexOf('/questao') >= 0) {
                if (/\/principal\/questao\/cadastrar/.test(pathname)) {
                    siac.Questao.Cadastrar.iniciar();
                }
            }
            else if (pathname.indexOf('/autoavaliacao') >= 0) {
                if (/\/principal\/autoavaliacao\/realizar\/auto[0-9]+$/.test(pathname)) {
                    siac.Autoavaliacao.Realizar.iniciar();
                }
                else if (/\/principal\/autoavaliacao\/resultado\/auto[0-9]+$/.test(pathname)) {
                    siac.Autoavaliacao.Resultado.iniciar();
                }
                else if (pathname == '/principal/autoavaliacao/gerar') {
                    siac.Autoavaliacao.Gerar.iniciar();
                }
            }
            else if (pathname.indexOf('/academica') >= 0) {
                if (pathname == '/principal/avaliacao/academica/gerar') {
                    siac.Academica.Gerar.iniciar();
                }
                else if (/\/principal\/avaliacao\/academica\/agendar\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Agendar.iniciar();
                }
                else if (/\/principal\/avaliacao\/academica\/resultado\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Resultado.iniciar();
                }
                else if (/\/principal\/avaliacao\/academica\/configurar\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Configurar.iniciar();
                }
                else if (/\/principal\/avaliacao\/academica\/realizar\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Realizar.iniciar();
                }
                else if (/\/principal\/avaliacao\/academica\/acompanhar\/acad[0-9]+$/.test(pathname)) {
                    siac.Academica.Acompanhar.iniciar();
                }
            }
            else if (pathname.indexOf('/reposicao') >= 0) {
                if (/\/principal\/avaliacao\/reposicao\/justificar\/acad[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Justificar.iniciar();
                }
                else if (/\/principal\/avaliacao\/reposicao\/configurar\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Configurar.iniciar();
                }
                else if (/\/principal\/avaliacao\/reposicao\/agendar\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Agendar.iniciar();
                }
                else if (/\/principal\/avaliacao\/reposicao\/realizar\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Realizar.iniciar();
                }
                else if (/\/principal\/avaliacao\/reposicao\/acompanhar\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Acompanhar.iniciar();
                }
                else if (/\/principal\/avaliacao\/reposicao\/resultado\/repo[0-9]+$/.test(pathname)) {
                    siac.Reposicao.Resultado.iniciar();
                }
            }
            else if (pathname.indexOf('/certificacao') >= 0) {
                if (pathname == '/principal/avaliacao/certificacao/gerar') {
                    siac.Certificacao.Gerar.iniciar();
                }
                else if (/\/principal\/avaliacao\/certificacao\/configurar\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Configurar.iniciar();
                }
                else if (/\/principal\/avaliacao\/certificacao\/agendar\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Agendar.iniciar();
                }
                else if (/\/principal\/avaliacao\/certificacao\/avaliados\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Avaliados.iniciar();
                }
                else if (/\/principal\/avaliacao\/certificacao\/realizar\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Realizar.iniciar();
                }
                else if (/\/principal\/avaliacao\/certificacao\/acompanhar\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Acompanhar.iniciar();
                }
                else if (/\/principal\/avaliacao\/certificacao\/resultado\/cert[0-9]+$/.test(pathname)) {
                    siac.Certificacao.Resultado.iniciar();
                }
            }
        }
        else if (pathname.indexOf('/historico/') == 0) {
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
        else if (pathname.indexOf('/configuracoes/') == 0) {
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
            else if (pathname.indexOf('/usuario') >= 0) {
                if (/\/configuracoes\/usuario$/.test(pathname)) {
                    siac.Usuario.Index.iniciar();
                }
                else if (/\/configuracoes\/usuario\/detalhe/.test(pathname)) {
                    siac.Usuario.Detalhe.iniciar();
                }
            }
            else if (/\/configuracoes\/opinioes/.test(pathname)) {
                siac.Configuracoes.Opinioes.iniciar();
            }
            else if (/\/configuracoes\/institucional\/coordenadores/.test(pathname)) {
                siac.Configuracoes.Institucional.iniciar();
            }
        }
        else if (pathname.indexOf('/institucional/') == 0) {
            if (pathname == '/institucional/gerar') {
                siac.Institucional.Gerar.iniciar();
            }
            else if (pathname == '/institucional/historico') {
                siac.Institucional.Historico.iniciar();
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
            else if (/\/institucional\/realizar\/avi[0-9]+$/.test(pathname)) {
                siac.Institucional.Realizar.iniciar();
            }
            else if (/\/institucional\/resultado\/avi[0-9]+$/.test(pathname)) {
                siac.Institucional.Resultado.iniciar();
            }
        }
        else if (pathname.indexOf('/perfil') == 0) {
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

    function habilitarAjuda() {
        $('.ui.ajuda.button').click(function () {
            var $btnAjuda = $(this);

            if ($btnAjuda.hasClass('active')) {
                $('body').find('[data-ajuda]').map(function () {
                    var $elemento = $(this);
                    $elemento.popup('destroy');
                });
                $btnAjuda.removeClass('active');
                siac.Lembrete.Notificacoes.exibir('Sistema de Ajuda desativado!','info');
            } else {
                $('body').find('[data-ajuda]').map(function () {
                    var $elemento = $(this);
                    var textoAjuda = $elemento.data('ajuda');

                    $elemento.popup({
                        on: 'hover',
                        content: textoAjuda
                    });
                });
                $btnAjuda.addClass('active');
                siac.Lembrete.Notificacoes.exibir('Sistema de Ajuda ativado!','info');
            }
        });
    }

    return {
        iniciar: iniciar,
        mensagem: mensagem,
        aviso: aviso
    }
})();

siac.iniciar();