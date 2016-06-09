siac.Configuracoes = siac.Configuracoes || {};

siac.Configuracoes.Parametros = (function () {
    var MatrizCount = 2;
    var HorarioCount = 2;

    function iniciar() {
        $('.ui.dropdown').dropdown();
        $('.ui.accordion').accordion({ animateChildren: false });

        $('.tabular.menu .item').tab({
            history: true,
            historyType: 'state',
            path: '/configuracoes/parametros'
        });

        $('.tab.geral :input, .tab.termo :input').change(function () {
            parametroModificado();
        });

        $('form input[type="submit"]').click(function () {
            cadastrar('#' + $(this).parents('form').attr('id'));
        });

        $('.adicionar.disciplina').click(function () {
            adicionarDisciplinaMatriz();
        });

        $('.adicionar.horario').click(function () {
            adicionarHorario();
        });

        $('#btnAtualizar').click(function () {
            atualizarParametros();
        });
    }

    function atualizarParametros() {
        $('#btnAtualizar').addClass('loading');
        var form = $(':input');
        $.ajax({
            type: 'POST',
            data: form,
            url: "/configuracoes/parametros",
            success: function () {
                $('#btnAtualizar').addClass('green').text('Atualizado');
                $('#btnAtualizar').removeClass('loading');
            },
            error: function () {
                $('.ui.erro.modal').modal('show');
                $('#btnAtualizar').removeClass('loading');
            }
        });
        return false;
    };

    function parametroModificado() {
        $('#btnAtualizar').removeClass('green').text('Atualizar');
    }

    function cadastrar(itemFormId) {
        $(itemFormId).addClass('ui form loading');
    }

    function adicionarDisciplinaMatriz() {
        var copia = $('[name="ddlDisciplina1"').clone();
        var $tr = $('<tr></tr>');
        var $td1 = $('<td></td>').append('\
                        <div class="field required">\
                            <input type="number" name="txtPeriodo' + MatrizCount + '" placeholder="Periodo..." required/>\
                        </div>');
        var $td2 = $('<td></td>').append(copia.attr('name', 'ddlDisciplina' + MatrizCount).addClass('ui dropdown search fluid'));
        var $td3 = $('<td></td>').append('\
                        <div class="field required">\
                            <input type="number" name="txtCH' + MatrizCount + '" placeholder="Carga Horária..." required />\
                        </div>');
        $tr.append($td1).append($td2).append($td3);
        $('.tab.matriz .table.body').append($tr);
        $('#matrizQte').val(MatrizCount);
        $('.ui.dropdown').dropdown();
        MatrizCount++;
    }

    function adicionarHorario() {
        var horario = $('[name="txtHorario1"').clone();
        var inicio = $('[name="txtInicio1"').clone();
        var termino = $('[name="txtTermino1"').clone();

        var $tr = $('<tr></tr>');
        var $td1 = $('<td></td>').append(horario.attr('name', 'txtHorario' + HorarioCount).val(HorarioCount));
        var $td2 = $('<td></td>').append(inicio.attr('name', 'txtInicio' + HorarioCount));
        var $td3 = $('<td></td>').append(termino.attr('name', 'txtTermino' + HorarioCount));

        $tr.append($td1).append($td2).append($td3);
        $('.table.body.horario').append($tr);
        $('#horarioQte').val(HorarioCount);
        HorarioCount++;
    }

    return {
        iniciar: iniciar
    }
})();

siac.Configuracoes.Opinioes = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 10, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var pesquisa = "";

    function iniciar() {
        $('.ui.dropdown').dropdown();
        $('.ui.modal').modal();

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

        $('.ordenar.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            ordenar = $_this.attr('data-ordenar');
            listar();

            $('.ordenar.item').removeClass('active');
            $_this.addClass('active');
        });

        $('.carregar.button').click(function () {
            if ($('.table .tbody .tr').length == (_controleQte * pagina)) {
                pagina++;
                listar();
            }
        });

        pesquisa = $('.pesquisa input').val();

        listar();
    };

    function listar() {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        var $table = $('.ui.table');
        $table.parent().addClass('loading');
        _controleAjax = $.ajax({
            url: '/configuracoes/listaropinioes',
            data: {
                pagina: pagina,
                ordenar: ordenar,
                pesquisa: pesquisa
            },
            method: 'POST',
            success: function (partial) {
                if (partial != _controlePartial) {
                    if (pagina == 1) {
                        $table.find('tbody').html(partial);
                    }
                    else {
                        $table.find('tbody').append(partial);
                    }
                    _controlePartial = partial;
                }
            },
            complete: function () {
                $table.parent().removeClass('loading');
                if ($('.table .tbody .tr').length < (_controleQte * pagina)) {
                    $('.carregar.button').parents('tfoot').remove();
                }
                $('.table tbody .button').off().click(function () {
                    var $this = $(this);
                    var $tr = $this.parents('tr');
                    var usuario = $tr.find('[data-usuario]').data('usuario');
                    var matricula = $tr.find('[data-matricula]').data('matricula');
                    var opiniao = $tr.find('[data-opiniao]').data('opiniao');
                    var date = $tr.find('time').attr('title');

                    var $modal = $('.opiniao.modal');

                    $modal.find('.header').text(usuario + ' (' + matricula + ')');
                    $modal.find('.content').html($('<blockquote></blockquote>').html(opiniao));
                    $modal.find('.content').append($('<i/>').text('– ' + date + '.'));

                    $modal.modal('show');
                });
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

siac.Configuracoes.Institucional = (function () {
    function iniciar() {
        $('.ui.checkbox').checkbox();
        $('[name^=chkOcupacao]').change(function () {
            $('.salvar.button').removeClass('active').text('Salvar');
        });
        $('[name^=chkCoordenador]').change(function () {
            if ($('[name^=chkCoordenador]:checked').length) {
                $('.trigger.button').removeClass('disabled');
            }
            else {
                $('.trigger.button').addClass('disabled');
            }
        });
        $('.salvar.button').click(function () {
            alterarOcupacoesCoordenadores();
        });
        $('.trigger.button').popup({ inline: true, on: 'click', position: 'right center' });
        $('.coordenador.button').click(function () {
            $('.coordenador.modal').modal('show');
        });
        $('.pesquisar.button').click(function () {
            var strPesquisa = $('#txtPesquisa').val();
            if (strPesquisa && strPesquisa.length >= 3) {
                $(this).parents('.form').addClass('loading');
                pesquisar(strPesquisa);
            }
        });
        $('.coordenador.modal').modal({
            onApprove: function () {
                if ($('[name=rdoPessoa]:checked').length) {
                    var codPessoa = $('[name=rdoPessoa]:checked').val();
                    adicionarOcupacaoCoordenador(codPessoa);
                    $('.approve.button').addClass('loading');
                }
                else {
                    siac.mensagem('Ops... Você ainda não selecionou nenhuma pessoa!');
                }
                return false;
            },
            onDeny: function () {
                $('.coordenador.modal #txtPesquisa').val('');
                $('.coordenador.modal .ui.list').html('');
            }
        });
        $('.remover.button').click(function () { removerOcupacaoCoordenador(); });
    }

    function alterarOcupacoesCoordenadores() {
        var $btnSalvar = $('.salvar.button');
        $btnSalvar.addClass('loading');
        var arrOcupacoes = [];
        var $chkOcupacao = $('[name^=chkOcupacao]');

        for (var i = 0, length = $chkOcupacao.length; i < length; i++) {
            if ($chkOcupacao.eq(i).is(':checked')) {
                var codOcupacao = $chkOcupacao.eq(i).attr('name').split('chkOcupacao')[1];
                arrOcupacoes.push(parseInt(codOcupacao));
            }
        }

        $.ajax({
            url: '/configuracoes/alterarocupacoescoordenadores',
            type: 'post',
            data: {
                ocupacoes: arrOcupacoes
            },
            success: function () { $btnSalvar.removeClass('loading'); },
            error: function () { $btnSalvar.removeClass('loading'); },
            complete: function () { $btnSalvar.addClass('active').text('Salvo'); }
        });
    }

    function adicionarOcupacaoCoordenador(codPessoa) {
        if (codPessoa) {
            $.ajax({
                url: '/configuracoes/adicionarocupacaocoordenador',
                type: 'post',
                data: { codPessoaFisica: codPessoa },
                success: function () { window.location.reload(); },
                error: function () { siac.mensagem('Ocorreu um erro com sua solicitação. Por favor, tente novamente mais tarde.'); }
            });
        }
    }

    function pesquisar(pesquisa) {
        if (pesquisa && pesquisa.length >= 3) {
            var $modal = $('.coordenador.modal');
            $.ajax({
                url: '/configuracoes/listarpessoa',
                type: 'post',
                data: {
                    pesquisa: pesquisa
                },
                success: function (data) {
                    if (data) {
                        var $list = $modal.find('.ui.list');
                        $list.html('');
                        for (var i = 0, length = data.length; i < length; i++) {
                            var $item = $('<div class="item"></div>')
                                .html(' <div class="ui slider checkbox">' +
                                          '<input type="radio" name="rdoPessoa" value="' + data[i].CodPessoa + '">' +
                                          '<label>' + data[i].Nome + '</label>' +
                                        '</div>');
                            $list.append($item);
                        }
                        $modal.find('.ui.checkbox').checkbox();
                    }
                },
                error: function () { siac.mensagem('Ocorreu um erro com sua solicitação. Por favor, tente novamente mais tarde.'); },
                complete: function () {
                    $modal.find('.form').removeClass('loading');
                }
            });
        }
    }

    function removerOcupacaoCoordenador() {
        var $btnRemover = $('.remover.button');
        $btnRemover.addClass('loading');
        var arrPessoas = [];
        var $chkCoordenadores = $('[name^=chkCoordenador]');

        for (var i = 0, length = $chkCoordenadores.length; i < length; i++) {
            if ($chkCoordenadores.eq(i).is(':checked')) {
                var codPessoa = $chkCoordenadores.eq(i).attr('name').split('chkCoordenador')[1];
                arrPessoas.push(parseInt(codPessoa));
            }
        }

        $.ajax({
            url: '/configuracoes/removerocupacaocoordenador',
            type: 'post',
            data: { codPessoaFisica: arrPessoas },
            success: function () { window.location.reload(); },
            error: function () { siac.mensagem('Ocorreu um erro com sua solicitação. Por favor, tente novamente mais tarde.'); },
            complete: function () { $btnRemover.removeClass('loading'); }
        });
    }

    return { iniciar: iniciar }
})();