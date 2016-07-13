siac.Simulado = siac.Simulado || {
    adicionarEventoNoFormulario: function () {
        $('form').off('submit').on('submit', function () {
            var $this = $(this);
            if ($this.hasClass('modal')) {
                $this.modal('hide');
            }
            $this.find('.button[type=submit]').addClass('loading');
        });
    }
};

siac.Simulado.Novo = (function () {
    function iniciar() {
        $('.ui.modal').modal();
        $('.ui.termo.modal').modal({ closable: false }).modal('show');
        $('.cancelar.button').popup({ on: 'click' });
        $('.ui.confirmar.modal')
         .modal({
             onApprove: function () {
                 $('form').submit();
             }
         });

        $('.prosseguir.button').click(function () {
            prosseguir();
        });
    }

    function prosseguir() {
        var $errorList = $('form .error.message .list');

        $errorList.html('');
        $('form').removeClass('error');

        var valido = true;

        if (!$('#txtTitulo').val().trim()) {
            $errorList.append('<li>Insira o título</li>');
            valido = false;
        }

        if (!$('#txtQteVagas').val().trim() || $('#txtQteVagas').val() <= 0) {
            $errorList.append('<li>Insira a quantidade de vagas</li>');
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

        $('#txtModalTitulo').val($('#txtTitulo').val());
        $('#txtModalDescricao').val($('#txtDescricao').val());
        $('#txtModalQteVagas').val($('#txtQteVagas').val());

        $modal.modal('show');
    }

    return {
        iniciar: iniciar
    }
})();

siac.Simulado.Provas = (function () {
    var _codigo = '',
        _disciplinaProfessores = {};

    function iniciar() {
        _codigo = window.location.pathname.toLowerCase().match(/simul[0-9]+/)[0];
        _disciplinaProfessores = JSON.parse($('#disciplinaProfessores').html());
        $('#disciplinaProfessores').remove();

        $('.ui.informacoes.modal').modal();
        $('.ui.accordion').accordion({
            animateChildren: false
        });
        $('.ui.dropdown').dropdown();
        $('[data-html]').popup({
            on: 'click',
            onVisible: function () {
                $('.remover.button').click(function () {
                    removerDia($(this));
                });
            }
        });

        $('.novo.dia.button').click(function () {
            $('.novo.dia.modal').modal('show');
        })

        $('.novo.dia.modal .approve.button').click(function () {
            if (!validarNovoDia()) {
                $('.novo.dia.modal .form').addClass('error');
                return false;
            } else {
                $('form.novo.dia').submit();
            }
        })

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        $('.editar.button').click(function () {
            editarDia($(this));
        });

        $('.provas.button').click(function () {
            carregarProvas($(this));
        });

        siac.Simulado.adicionarEventoNoFormulario();
        listarProfessoresPorDisciplina();
    }

    function validarNovoDia() {
        var dataRealizacao = $('[name=txtDataRealizacao]').val(),
            horarioInicio = $('[name=txtHorarioInicio]').val(),
            horarioTermino = $('[name=txtHorarioTermino]').val(),
            retorno = true;

        lstErro = $('.novo.dia.modal .form .error.message .list');
        lstErro.html('');
        $('.novo.dia.modal .form').removeClass('error');

        if (!dataRealizacao) {
            lstErro.append('<li>Especifique a data de realização</li>');
            retorno = false;
        } else {
            var strDate = siac.Utilitario.formatarData(dataRealizacao + 'T00:00:00');
            if (!siac.Utilitario.dataEFuturo(strDate)) {
                lstErro.append('<li>Especifique uma data de realização futura</li>');
                retorno = false;
            }
        }

        if (!horarioInicio) {
            lstErro.append('<li>Especifique o horário de início das provas</li>');
            retorno = false;
        }

        if (!horarioTermino) {
            lstErro.append('<li>Especifique o horário de término das provas</li>');
            retorno = false;
        }

        if (dataRealizacao && horarioInicio && horarioTermino) {
            var strDateA = dataRealizacao + 'T' + horarioInicio + ':00';
            var strDateB = dataRealizacao + 'T' + horarioTermino + ':00';
            if (siac.Utilitario.compararData(strDateA, strDateB) >= 0) {
                lstErro.append('<li>Especifique um horário de término superior ao horário de início</li>');
                retorno = false;
            }
        }

        return retorno;
    }

    function removerDia($button) {
        var codDia = $button.data('dia');
        $button.addClass('loading');

        $.ajax({
            type: 'POST',
            url: $button.data('action'),
            data: { codDia: codDia },
            complete: function () {
                location.reload();
            }
        })
    }

    function editarDia($button) {
        var codDia = $button.data('dia');

        $.ajax({
            type: 'POST',
            url: $button.data('action'),
            data: {
                codDia: codDia
            },
            beforeSend: function () {
                $button.addClass('loading');
            },
            success: function (data) {
                $('.editar.dia.modal').remove();
                $('body').append($(data))

                $('.editar.modal [data-mask]').map(function () {
                    $(this).mask($(this).data('mask'));
                });

                $('.editar.dia.modal')
                    .modal('show');
            },
            error: function () {
                siac.mensagem('Aconteceu um erro na operação! Atualize a página para tentar novamente.')
            },
            complete: function () {
                $button.removeClass('loading');
                siac.Simulado.adicionarEventoNoFormulario();
            }
        })
    }

    function carregarProvas($button) {
        var codDia = $button.data('dia'),
            $modalProvas = $('.ui.provas.modal');

        $.ajax({
            type: 'POST',
            url: $button.data('action'),
            data: { codDia: codDia },
            beforeSend: function () {
                $button.addClass('loading');
            },
            success: function (data) {
                $modalProvas.html(data).modal('show');
                $modalProvas.find('.novo.prova.button').off('click').click(function () {
                    abrirModalNovaProva(codDia);
                });
                $modalProvas.find('.remover.button').off('click').click(function () {
                    removerProva(codDia, $(this));
                });

                $modalProvas.find('.editar.button').off('click').click(function () {
                    editarProva(codDia, $(this));
                });
            },
            error: function () {
                siac.mensagem('Falha ao recuperar as Provas desse dia. Atualize a página para tentar novamente.');
            },
            complete: function () {
                $button.removeClass('loading');
                siac.Simulado.adicionarEventoNoFormulario();
            }
        })
    }

    function abrirModalNovaProva(codDia) {
        var $form = $('form.novo.prova'),
            url = $form.prop('action');

        if (url.indexOf('=') > -1) { url = url.replace(/=[0-9]+/, '=' + codDia); }
        else { url = url + '?codDia=' + codDia; }

        $form.prop('action', url);

        $form.modal({ closable: false }).modal('show').data('dia', codDia);

        siac.Simulado.adicionarEventoNoFormulario();
    }

    function removerProva(codDia, $button) {
        var codProva = $button.data('prova');

        $.ajax({
            type: 'POST',
            url: $button.data('action'),
            data: {
                codDia: codDia,
                codProva: codProva
            },
            beforeSend: function () {
                $button.addClass('loading');
            },
            success: function () {
                $('.provas.modal').modal('hide');
                location.reload();
            },
            error: function () {
                siac.mensagem('Aconteceu um erro na operação! Atualize a página para tentar novamente.')
            },
            complete: function () {
                $button.removeClass('loading');
                siac.Simulado.adicionarEventoNoFormulario();
            }
        })
    }

    function editarProva(codDia, $button) {
        var codProva = $button.data('prova');

        $.ajax({
            type: 'POST',
            url: $button.data('action'),
            data: {
                codDia: codDia,
                codProva: codProva
            },
            beforeSend: function () {
                $button.addClass('loading');
            },
            success: function (data) {
                // Remove da página o modal existente
                $('.editar.prova.modal').remove();
                // Adiciona e exibe modal retornado na página
                $('body').append($(data))
                $('.editar.prova.modal').modal('show');
                $('.editar.prova.modal .dropdown').dropdown();
            },
            error: function () {
                siac.mensagem('Aconteceu um erro na operação! Atualize a página para tentar novamente.')
            },
            complete: function () {
                $button.removeClass('loading');
                siac.Simulado.adicionarEventoNoFormulario();
                listarProfessoresPorDisciplina();
            }
        })
    }

    function listarProfessoresPorDisciplina() {
        $('[name=ddlDisciplina]').off('change').on('change', function () {
            var $this = $(this),
                $form = $this.closest('form'),
                $ddlProfessor = $form.find('[name=ddlProfessor]'),
                codDisciplina = $this.val();

            $ddlProfessor.html('<option value="">Professor</option>');

            _disciplinaProfessores[codDisciplina].forEach(function (professor) {
                $ddlProfessor.append('<option value="' + professor.CodProfessor + '">' + professor.Nome + '</option>');
            });

            $ddlProfessor
                .dropdown('refresh')
                .dropdown('set placeholder text', 'Professor')
                .dropdown('set value', '');
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Simulado.Datas = (function () {
    function iniciar() {
        $('.ui.informacoes.modal').modal();

        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal').modal({
            onApprove: function () {
                $('form').addClass('loading').submit();
            }
        });

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        $('.confirmar.button').click(function () {
            confirmar();
            return false;
        });
    }

    function validar() {
        var inicioInscricao = $('#txtInicioInscricao').val(),
            terminoInscricao = $('#txtTerminoInscricao').val(),
            retorno = true;

        lstErro = $('form .error.message .list');
        lstErro.html('');
        $('form').removeClass('error');

        if (!inicioInscricao) {
            lstErro.append('<li>Especifique a data de início das inscrições</li>');
            retorno = false;
        } else {
            var strDate = siac.Utilitario.formatarData(inicioInscricao + 'T00:00:00');
            if (!siac.Utilitario.dataEFuturo(strDate)) {
                lstErro.append('<li>Especifique uma data de início futura</li>');
                retorno = false;
            }
        }

        if (!terminoInscricao) {
            lstErro.append('<li>Especifique a data de término das inscrições</li>');
            retorno = false;
        } else {
            var strDate = siac.Utilitario.formatarData(terminoInscricao + 'T00:00:00');
            if (!siac.Utilitario.dataEFuturo(strDate)) {
                lstErro.append('<li>Especifique uma data de término futura</li>');
                retorno = false;
            }
        }

        if (inicioInscricao && terminoInscricao) {
            var strDateA = inicioInscricao + 'T00:00:00';
            var strDateB = terminoInscricao + 'T00:00:00';
            if (siac.Utilitario.compararData(strDateA, strDateB) >= 0) {
                lstErro.append('<li>Especifique uma data de término posterior à data de início</li>');
                retorno = false;
            }
        }

        return retorno;
    }

    function confirmar() {
        var $form = $('form');
        if (validar()) {
            var $div = $('<div class="ui form"></div>');
            $div.append($form.html());
            var lstInput = $div.find(':input');
            $div.find('.button').remove();
            $div.find('#txtInicioInscricao').attr('value', $form.find('#txtInicioInscricao').val());
            $div.find('#txtTerminoInscricao').attr('value', $form.find('#txtTerminoInscricao').val());
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

siac.Simulado.Salas = (function () {
    var _codigo = '',
        _blocos = [],
        _salas = [],
        ddlCampus = $('[name=ddlCampus]'),
        ddlBloco = $('[name=ddlBloco]'),
        ddlSala = $('[name=ddlSala]');

    function iniciar() {
        _codigo = window.location.pathname.toLowerCase().match(/simul[0-9]+/)[0];
        _blocos = JSON.parse($('script#blocos').html());
        _salas = JSON.parse($('script#salas').html());
        $('script#salas').remove();
        $('script#blocos').remove();

        $('.ui.dropdown').dropdown();

        $('[data-html]').popup({
            on: 'click',
            onVisible: function () {
                $('.remover.button').click(function () {
                    removerSala($(this));
                });
            }
        });

        $('.informacoes.button').click(function () {
            $('.ui.informacoes.modal').modal('show');
        });

        ddlCampus.off('change').change(function () {
            atualizarBlocos()
        });
        ddlBloco.off('change').change(function () {
            atualizarSalas()
        });

        $('.selecionar.button').click(function () {
            validar();
        });
    };

    function atualizarBlocos() {
        var codCampus = ddlCampus.val(),
            label = 'Bloco';

        ddlBloco.html('<option value="">' + label + '</option>');

        for (var i = 0, length = _blocos.length; i < length; i++) {
            if (_blocos[i].Campus == codCampus) {
                ddlBloco.append('<option value="' + _blocos[i].CodBloco + '">' + _blocos[i].Descricao + '</option>');
            }
        }

        ddlBloco
            .dropdown('refresh')
            .dropdown('set placeholder text', label)
            .dropdown('set value', '')
        ;

        atualizarSalas();
    };

    function atualizarSalas() {
        var codBloco = ddlBloco.val(),
            label = 'Sala';

        ddlSala.html('<option value="">' + label + '</option>');

        if (codBloco) {
            for (var i = 0, length = _salas.length; i < length; i++) {
                if (_salas[i].CodBloco == codBloco) {
                    ddlSala.append('<option value="' + _salas[i].CodSala + '">' + _salas[i].Descricao + '</option>');
                }
            }
        }

        ddlSala
            .dropdown('refresh')
            .dropdown('set placeholder text', label)
            .dropdown('set value', '')
        ;
    }

    function validar() {
        var $errorList = $('form .error.message .list');

        $errorList.html('');
        $('form').removeClass('error');

        var valido = true;

        if (!ddlCampus.val().trim()) {
            $errorList.append('<li>Selecione o campus</li>');
            valido = false;
        }
        if (!ddlBloco.val().trim()) {
            $errorList.append('<li>Selecione o bloco</li>');
            valido = false;
        }
        if (!ddlSala.val().trim()) {
            $errorList.append('<li>Selecione a sala</li>');
            valido = false;
        }

        if (valido) {
            selecionarSala();
        }
        else {
            $('form').addClass('error');
            $('html, body').animate({
                scrollTop: $('form .error.message').offset().top
            }, 1000);
        }
    }

    function selecionarSala() {
        $('form').submit();
    }

    function removerSala($button) {
        var codSala = $button.data('sala');
        $button.addClass('loading');
        $.ajax({
            type: 'POST',
            url: $button.data('action'),
            data: { codSala: codSala },
            complete: function () {
                location.reload();
            }
        })
    }

    return {
        iniciar: iniciar
    }
})();

siac.Simulado.Detalhe = (function () {
    var _codigo = '';

    function iniciar() {
        _codigo = window.location.pathname.toLowerCase().match(/simul[0-9]+/)[0];
        $('.ui.accordion').accordion({
            animateChildren: false
        });
        $('[data-content]').popup();

        $('.ui.acoes.dropdown').dropdown({
            action: 'hide'
        });

        $('.editar.item').click(function () {
            $('.editar.modal').modal('show');
        });

        $('.email.item').click(function () {
            $('.email.modal').modal('show');
        });

        $('.encerrar.item').click(function () {
            $('.encerrar.modal').modal({
                onApprove: function () {
                    var $modal = $(this);
                    $.ajax({
                        type: 'POST',
                        url: $modal.data('action'),
                        beforeSend: function () {
                            $modal.find('.approve.button').addClass('loading');
                        },
                        complete: function () {
                            $modal.modal('hide');
                            location.reload();
                        }
                    });
                    return false;
                }
            }).modal('show');
        });

        $('.candidatos.item').click(function () {
            $('.candidatos.modal').modal('show');
        });

        $('.liberar.item').click(function () {
            $('.liberar.modal').modal({
                onApprove: function () {
                    var $modal = $(this);
                    $.ajax({
                        type: 'POST',
                        url: $modal.data('action'),
                        complete: function () {
                            location.reload();
                        }
                    })
                }
            }).modal('show');
        });

        $('.bloquear.item').click(function () {
            $('.bloquear.modal').modal({
                onApprove: function () {
                    var $modal = $(this);
                    $.ajax({
                        type: 'POST',
                        url: $modal.data('action'),
                        complete: function () {
                            location.reload();
                        }
                    })
                }
            }).modal('show');
        });

        $('.alterar.prazo.item').click(function () {
            $('.alterar.prazo.modal').modal('show');
        });

        $('.mapear.item').click(function () {
            $('.mapear.modal').modal({
                onApprove: function () {
                    var $modal = $(this);
                    $.ajax({
                        type: 'POST',
                        url: $modal.data('action'),
                        beforeSend: function () {
                            $modal.find('.approve.button').addClass('loading');
                        },
                        complete: function () {
                            $modal.modal('hide');
                            location.reload();
                        }
                    });
                    return false;
                }
            }).modal('show');
        });

        $('.button[data-sala]').click(function () {
            var $button = $(this),
                $modal = $('.sala.candidato.modal');

            $.ajax({
                type: 'POST',
                url: $button.data('action'),
                beforeSend: function () {
                    $button.addClass('loading');
                },
                success: function (data) {
                    if (data) {
                        $modal.html(data);
                        $modal.modal('show');
                    }
                },
                error: function () {
                    siac.mensagem('Ocorreu um erro na operação.');
                },
                complete: function () {
                    $button.removeClass('loading');
                }
            });
        });

        $('.finalizar.provas.item').click(function () {
            $('.finalizar.provas.modal').modal({
                onApprove: function () {
                    var $modal = $(this);
                    $.ajax({
                        type: 'POST',
                        url: $modal.data('action'),
                        beforeSend: function () {
                            $modal.find('.approve.button').addClass('loading');
                        }, complete: function () {
                            $modal.modal('hide');
                            location.reload();
                        }
                    });
                    return false;
                }
            }).modal('show');
        });

        $('.calcular.resultados.item').click(function () {
            $('.calcular.resultados.modal').modal({
                onApprove: function () {
                    var $modal = $(this);
                    $.ajax({
                        type: 'POST',
                        url: $modal.data('action'),
                        beforeSend: function () {
                            $modal.find('.approve.button').addClass('loading');
                        },
                        complete: function () {
                            $modal.modal('hide');
                            location.reload();
                        }
                    });
                    return false;
                }
            }).modal('show');
        });

        $('.classificacao.button').click(function () {
            var $button = $(this),
                $modal = $('.classificacao.modal');

            $.ajax({
                type: 'POST',
                url: $button.data('action'),
                beforeSend: function () {
                    $button.addClass('loading');
                },
                success: function (data) {
                    $modal.html(data);
                    $modal.find('[data-candidato]').click(function () {
                        abrirCandidatoDetalhe($(this));
                    });
                    $modal.modal('show');
                },
                error: function () {
                    siac.mensagem('Ocorreu um erro na operação.');
                },
                complete: function () {
                    $button.removeClass('loading');
                }
            });
        });

        $('.ordem-desempate.item').click(function () {
            $('.ordem-desempate.modal').modal({
                onApprove: function () {
                    var $modal = $(this);
                    $.ajax({
                        type: 'POST',
                        url: $modal.data('action'),
                        data: {
                            provas: retornarOrdemDesempate()
                        },
                        beforeSend: function () {
                            $modal.find('.approve.button').addClass('loading');
                        },
                        complete: function () {
                            $modal.find('.approve.button').removeClass('loading');
                            $modal.modal('hide');
                        }
                    });
                    return false;
                }
            }).modal('show');
        });

        $('.provas-peso.item').click(function () {
            $('.provas-peso.modal').modal({
                onApprove: function () {
                    var $modal = $(this);
                    $.ajax({
                        type: 'POST',
                        url: $modal.data('action'),
                        data: {
                            provas: retornarProvasPeso()
                        },
                        beforeSend: function () {
                            $modal.find('.approve.button').addClass('loading');
                        },
                        complete: function () {
                            $modal.find('.approve.button').removeClass('loading');
                            $modal.modal('hide');
                        }
                    });
                    return false;
                }
            }).modal('show');
        });

        gerenciarOrdemDesempate();

        siac.Simulado.adicionarEventoNoFormulario();
    }

    function retornarProvasPeso() {
        var $provas = $('.provas-peso.modal .content tr[data-prova]');
        var provas = {};
        for (var i = 0, length = $provas.length; i < length; i++) {
            var prova = $provas.eq(i).data('prova');
            provas[prova] = $('[name="' + prova + '"]').val();
        }
        console.log($provas, provas);
        return provas;
    }

    function retornarOrdemDesempate() {
        var $provas = $('.ordem-desempate.modal .content .list .item[data-prova]');
        var provas = [];
        for (var i = 0, length = $provas.length; i < length; i++) {
            provas.push($provas.eq(i).data('prova'));
        }
        return provas;
    }

    function gerenciarOrdemDesempate() {
        var $lista = $('.ordem-desempate.modal .content .list');

        $('[data-opcao=cima]').click(function () {
            var $item = $(this).closest('.item');
            $item.insertBefore($item.prev());
            atualizar();
        });
        $('[data-opcao=baixo]').click(function () {
            var $item = $(this).closest('.item');
            $item.insertAfter($item.next());
            atualizar();
        });

        function atualizar() {
            $lista.find('[data-opcao]').removeClass('disabled');
            $lista.find('.item:first-child [data-opcao=cima]').addClass('disabled');
            $lista.find('.item:last-child [data-opcao=baixo]').addClass('disabled');
        }

        atualizar();
    }

    function abrirCandidatoDetalhe($button) {
        var $modal = $('.candidato.resultado.modal'),
            codCandidato = $button.data('candidato');

        $.ajax({
            type: 'POST',
            url: $button.data('action'),
            beforeSend: function () {
                $button.addClass('loading');
            },
            success: function (data) {
                $modal.html(data);

                $modal.find('.voltar.button').click(function () {
                    $('.classificacao.modal').modal('show');
                });

                $modal.modal('show');
            },
            error: function () {
                siac.mensagem('Ocorreu um erro na operação.');
            },
            complete: function () {
                $button.removeClass('loading');
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Simulado.Respostas = (function () {
    var _codigo = '',
        $listaRespostas = '';

    function iniciar() {
        _codigo = window.location.pathname.toLowerCase().match(/simul[0-9]+/)[0];
        $listaRespostas = $('.lista.respostas');
        $('.ui.dropdown').dropdown();

        $('.voltar.button').popup({
            on: 'click'
        });

        $('.topo.button').click(function () { topo(); });

        $('.prova.button').click(function () {
            var $provaButton = $(this);

            if (validarProva()) {
                var prova = $('#ddlProva').val(),
                    codDia = prova.split('.')[0],
                    codProva = prova.split('.')[1];

                $.ajax({
                    type: 'POST',
                    url: $provaButton.data('action'),
                    data: {
                        codDia: codDia,
                        codProva: codProva
                    },
                    beforeSend: function () {
                        $provaButton.addClass('loading');
                    },
                    success: function (data) {
                        if (data) {
                            $listaRespostas.html(data);
                            alterarCheckBoxProva();
                            tratarEnvioFormulario('prova');
                        }
                    },
                    error: function () {
                        siac.mensagem('Ocorreu um erro na operação.');
                    },
                    complete: function () {
                        $provaButton.removeClass('loading');
                    }
                })
            }
        });

        $('.candidato.button').click(function () {
            var $candidatoButton = $(this);

            if (validarCandidato()) {
                var prova = $('#ddlProva').val(),
                    codDia = prova.split('.')[0],
                    codProva = prova.split('.')[1],
                    codCandidato = $('#ddlCandidato').val();

                $.ajax({
                    type: 'POST',
                    url: $candidatoButton.data('action'),
                    data: {
                        codDia: codDia,
                        codProva: codProva,
                        codCandidato: codCandidato
                    },
                    beforeSend: function () {
                        $candidatoButton.addClass('loading');
                    },
                    success: function (data) {
                        if (data) {
                            $listaRespostas.html(data);
                            alterarCheckBoxCandidato();
                            sobreporCandidatoEmDimmer();
                            tratarEnvioFormulario('candidato');
                        }
                    },
                    error: function () {
                        siac.mensagem('Ocorreu um erro na operação.');
                    },
                    complete: function () {
                        $candidatoButton.removeClass('loading');
                    }
                })
            }
        });
    }

    function tratarEnvioFormulario(provaOuCandidato) {
        var formulario = $('.respostas form');
        var botao = formulario.find('[type=submit]');
        var provaOuCandidato = provaOuCandidato;

        if (formulario) {
            formulario.off('submit').on('submit', function () {
                var dados = formulario.serialize();
                var url = formulario.attr('action');
                var metodo = formulario.attr('method');
                $.ajax({
                    type: metodo,
                    url: url,
                    data: dados,
                    beforeSend: function () {
                        botao.addClass('loading');
                    },
                    success: function () {
                        if (provaOuCandidato === 'prova') {
                            var dropdown = $('#ddlProva');
                            var atual = dropdown.val();
                            var proximo = dropdown.find('option[value="' + atual + '"]').next();

                            if (proximo.attr('value')) {
                                dropdown.dropdown('set selected', proximo.attr('value'));
                                siac.Lembrete.Notificacoes.exibir('Avançando para prova "' + proximo.text() + '".', 'info');
                                $('.prova.button').click();
                            }
                        }
                        else if (provaOuCandidato === 'candidato') {
                            var dropdown = $('#ddlCandidato');
                            var atual = dropdown.val();
                            var proximo = dropdown.find('option[value="' + atual + '"]').next();

                            if (proximo.attr('value')) {
                                dropdown.dropdown('set selected', proximo.attr('value'));
                                siac.Lembrete.Notificacoes.exibir('Avançando para candidato "' + proximo.text() + '".', 'info');
                                $('.candidato.button').click();
                            }
                        }

                        topo();
                    },
                    error: function () {
                        siac.Lembrete.Notificacoes.exibir('Não foi possível enviar as respostas.', 'negativo');
                    },
                    complete: function () {
                        botao.removeClass('loading');
                    }
                });
                return false;
            });
        }
    }

    function validarProva() {
        var prova = $('#ddlProva').val(),
            $form = $('.form'),
            $lstErro = $form.find('.error.message .list');

        $lstErro.html('');
        $form.removeClass('error');

        if (!prova) {
            $lstErro.append('<li>Selecione a prova para corrigir</li>');
            $form.addClass('error');
            return false;
        }
        return true;
    }

    function validarCandidato() {
        var prova = $('#ddlProva').val(),
            candidato = $('#ddlCandidato').val(),
            $form = $('.form'),
            $lstErro = $form.find('.error.message .list'),
            valido = true;

        $lstErro.html('');
        $form.removeClass('error');

        if (!prova) {
            $lstErro.append('<li>Selecione a prova para corrigir</li>');
            $form.addClass('error');
            valido = false;
        }

        if (!candidato) {
            $lstErro.append('<li>Selecione o candidato para corrigir</li>');
            $form.addClass('error');
            valido = false;
        }
        return valido;
    }

    function topo() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    }

    function alterarCheckBoxProva() {
        $('.ui.checkbox').checkbox();
        $('input[type=checkbox]').change(function () {
            var $checkbox = $(this),
                checked = $checkbox.is(':checked'),
                $tr = $checkbox.closest('tr'),
                trClass = 'error',
                inputClass = 'disabled',
                $input = $tr.find('input[type=number]');

            $input.val('');

            if (checked) {
                $tr.addClass(trClass);
                $tr.find('.field').addClass(inputClass);
                $input.prop('required', false);
            } else {
                $tr.removeClass(trClass);
                $tr.find('.field').removeClass(inputClass);
                $input.prop('required', true);
            }
        });
    }

    function alterarCheckBoxCandidato() {
        $('.ui.checkbox').checkbox();
        $('input[type=checkbox]').change(function () {
            var $checkbox = $(this),
                checked = $checkbox.is(':checked'),
                $content = $checkbox.closest('form').find('.segment').eq(0);

            if (checked) {
                $content.find(':input').prop('required', false);
                $content.dimmer({
                    closable: false
                }).dimmer('show');
            } else {
                $content.find(':input').prop('required', true);
                $content.dimmer('hide');
            }
        });
        $('input[type=checkbox]').change();
    }

    function sobreporCandidatoEmDimmer() {
        var $dimmer = $('.segment.lista.respostas .dimmer');
        $dimmer.css('z-index', 5);
    }

    return {
        iniciar: iniciar
    }
})();

siac.Simulado.Imprimir = (function () {
    function iniciar() {
        $('.informacoes.button').click(function () {
            $('.ui.accordion').accordion({
                animateChildren: false
            });
            $('.ui.informacoes.modal').modal('show');
        });

        $('.imprimir.button').click(function () {
            var url = $(this).data('url');
            window.open(url);
        });

        if (siac.Utilitario.eChrome || siac.Utilitario.eBlink) {
            $('[only-browser-message=chrome]').hide();
            $('[only-browser=chrome]').removeClass('disabled');
        }
    }

    return {
        iniciar: iniciar
    }
})();

siac.Simulado.Pontuacoes = (function () {
    var _codigo = '',
        $listaRespostas = '';

    function iniciar() {
        _codigo = window.location.pathname.toLowerCase().match(/simul[0-9]+/)[0];
        $listaRespostas = $('.lista.respostas');
        $('.ui.dropdown').dropdown();

        $('.voltar.button').popup({
            on: 'click'
        });

        $('.topo.button').click(function () { topo(); });

        $('.prova.button').click(function () {
            var $provaButton = $(this);

            if (validarProva()) {
                var prova = $('#ddlProva').val(),
                    codDia = prova.split('.')[0],
                    codProva = prova.split('.')[1];

                $.ajax({
                    type: 'POST',
                    url: $provaButton.data('action'),
                    data: {
                        codDia: codDia,
                        codProva: codProva
                    },
                    beforeSend: function () {
                        $provaButton.addClass('loading');
                    },
                    success: function (data) {
                        if (data) {
                            $listaRespostas.html(data);
                            alterarCheckBoxProva();
                            tratarEnvioFormulario('prova');
                        }
                    },
                    error: function () {
                        siac.mensagem('Ocorreu um erro na operação.');
                    },
                    complete: function () {
                        $provaButton.removeClass('loading');
                    }
                })
            }
        });

        $('.candidato.button').click(function () {
            var $candidatoButton = $(this);

            if (validarCandidato()) {
                var prova = $('#ddlProva').val(),
                    codDia = prova.split('.')[0],
                    codProva = prova.split('.')[1],
                    mascaraCandidato = $('#ddlCandidato').val();

                $.ajax({
                    type: 'POST',
                    url: $candidatoButton.data('action'),
                    data: {
                        codDia: codDia,
                        codProva: codProva,
                        mascaraCandidato: mascaraCandidato
                    },
                    beforeSend: function () {
                        $candidatoButton.addClass('loading');
                    },
                    success: function (data) {
                        if (data) {
                            $listaRespostas.html(data);
                            alterarCheckBoxCandidato();
                            sobreporCandidatoEmDimmer();
                            tratarEnvioFormulario('candidato');
                        }
                    },
                    error: function () {
                        siac.mensagem('Ocorreu um erro na operação.');
                    },
                    complete: function () {
                        $candidatoButton.removeClass('loading');
                    }
                })
            }
        });

        $('.editar.item').click(function () {
            $('.editar.modal').modal('show');
        });

        $('.encerrar.item').click(function () {
            $('.encerrar.modal').modal({
                onApprove: function () {
                    $.ajax({
                        type: 'POST',
                        url: '/simulado/encerrar/' + _codigo,
                        complete: function () {
                            location.reload();
                        }
                    })
                }
            }).modal('show');
        });
    }

    function tratarEnvioFormulario(provaOuCandidato) {
        var formulario = $('.respostas form');
        var botao = formulario.find('[type=submit]');
        var provaOuCandidato = provaOuCandidato;

        if (formulario) {
            formulario.off('submit').on('submit', function () {
                var dados = formulario.serialize();
                var url = formulario.attr('action');
                var metodo = formulario.attr('method');
                $.ajax({
                    type: metodo,
                    url: url,
                    data: dados,
                    beforeSend: function () {
                        botao.addClass('loading');
                    },
                    success: function () {
                        if (provaOuCandidato === 'prova') {
                            var dropdown = $('#ddlProva');
                            var atual = dropdown.val();
                            var proximo = dropdown.find('option[value="' + atual + '"]').next();

                            if (proximo.attr('value')) {
                                dropdown.dropdown('set selected', proximo.attr('value'));
                                siac.Lembrete.Notificacoes.exibir('Avançando para prova "' + proximo.text() + '".', 'info');
                                $('.prova.button').click();
                            }
                        }
                        else if (provaOuCandidato === 'candidato') {
                            var dropdown = $('#ddlCandidato');
                            var atual = dropdown.val();
                            var proximo = dropdown.find('option[value="' + atual + '"]').next();

                            if (proximo.attr('value')) {
                                dropdown.dropdown('set selected', proximo.attr('value'));
                                siac.Lembrete.Notificacoes.exibir('Avançando para candidato "' + proximo.text() + '".', 'info');
                                $('.candidato.button').click();
                            }
                        }

                        topo();
                    },
                    error: function () {
                        siac.Lembrete.Notificacoes.exibir('Não foi possível enviar as respostas.', 'negativo');
                    },
                    complete: function () {
                        botao.removeClass('loading');
                    }
                });
                return false;
            });
        }
    }

    function validarProva() {
        var prova = $('#ddlProva').val(),
            $form = $('.form'),
            $lstErro = $form.find('.error.message .list');

        $lstErro.html('');
        $form.removeClass('error');

        if (!prova) {
            $lstErro.append('<li>Selecione a prova para corrigir</li>');
            $form.addClass('error');
            return false;
        }
        return true;
    }

    function validarCandidato() {
        var prova = $('#ddlProva').val(),
            candidato = $('#ddlCandidato').val(),
            $form = $('.form'),
            $lstErro = $form.find('.error.message .list'),
            valido = true;

        $lstErro.html('');
        $form.removeClass('error');

        if (!prova) {
            $lstErro.append('<li>Selecione a prova para corrigir</li>');
            $form.addClass('error');
            valido = false;
        }

        if (!candidato) {
            $lstErro.append('<li>Selecione o candidato para corrigir</li>');
            $form.addClass('error');
            valido = false;
        }
        return valido;
    }

    function topo() {
        $("html, body").animate({
            scrollTop: 0
        }, 500);
        return false;
    }

    function alterarCheckBoxProva() {
        $('.ui.checkbox').checkbox();
        $('input[type=checkbox]').change(function () {
            var $checkbox = $(this),
                checked = $checkbox.is(':checked'),
                $tr = $checkbox.closest('tr'),
                trClass = 'error',
                inputClass = 'disabled',
                $input = $tr.find('input[type=number]');

            $input.val('');

            if (checked) {
                $tr.addClass(trClass);
                $tr.find('.field').addClass(inputClass);
                $input.prop('required', false);
            } else {
                $tr.removeClass(trClass);
                $tr.find('.field').removeClass(inputClass);
                $input.prop('required', true);
            }
        });
    }

    function alterarCheckBoxCandidato() {
        $('.ui.checkbox').checkbox();
        $('input[type=checkbox]').change(function () {
            var $checkbox = $(this),
                checked = $checkbox.is(':checked'),
                $content = $checkbox.closest('form').find('.segment').eq(0);

            if (checked) {
                $content.find(':input').prop('required', false);
                $content.dimmer({
                    closable: false
                }).dimmer('show');
            } else {
                $content.find(':input').prop('required', true);
                $content.dimmer('hide');
            }
        });
        $('input[type=checkbox]').change();
    }

    function sobreporCandidatoEmDimmer() {
        var $dimmer = $('.segment.lista.respostas .dimmer');
        $dimmer.css('z-index', 5);
    }

    return {
        iniciar: iniciar
    }
})();