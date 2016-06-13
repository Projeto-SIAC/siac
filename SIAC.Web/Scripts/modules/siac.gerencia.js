siac.Gerencia = siac.Gerencia || {
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

siac.Gerencia.Blocos = (function () {
    function iniciar() {
        $('.ui.modal').modal();
        $('.ui.dropdown').dropdown();
        $('[data-content]').popup();

        $('.novo.button').click(function () { $('.ui.novo.modal').modal('show') });

        $('.editar.button').click(function () {
            var _this = $(this);
            $.ajax({
                url: '/simulado/gerencia/carregarbloco',
                method: 'POST',
                data: {
                    'bloco': _this.data('bloco')
                },
                beforeSend: function () {
                    _this.addClass('loading');
                },
                success: function (data) {
                    $('form.editar').attr('action', '/simulado/gerencia/editarbloco/' + _this.data('bloco'))
                    $('.ui.editar.modal').html(data).modal('show');
                    $('.ui.editar.modal .ui.dropdown').dropdown();
                },
                error: function () {
                    siac.mensagem('Falha ao recuperar o Bloco para edição. Atualize a página para tentar novamente.');
                },
                complete: function () {
                    _this.removeClass('loading');
                    siac.Gerencia.adicionarEventoNoFormulario();
                }
            });
        });

        $('.excluir.button').click(function () {
            var _this = $(this),
                $modal = $('.ui.excluir.modal'),
                bloco = _this.data('bloco'),
                blocoDescricao = _this.closest('tr').find('[bloco-descricao]').text(),
                blocoCampus = _this.closest('tr').find('[bloco-campus]').data('content'),
                blocoSigla = _this.closest('tr').find('[bloco-sigla]').text();

            $modal.find('[bloco-descricao]').text(blocoDescricao);
            $modal.find('[bloco-campus]').text(blocoCampus);
            $modal.find('[bloco-sigla]').text(blocoSigla);

            $modal.modal({
                onApprove: function () {
                    $.ajax({
                        url: '/simulado/gerencia/excluirbloco',
                        method: 'POST',
                        data: {
                            'codigo': bloco
                        },
                        complete: function () {
                            location.reload();
                        }
                    });
                }
            }).modal('show');
        });

        siac.Gerencia.adicionarEventoNoFormulario();
    }

    return {
        iniciar: iniciar
    }
})();

siac.Gerencia.Salas = (function () {
    var _blocos = []

    function iniciar() {
        _blocos = JSON.parse($('script#blocos').html());

        $('.ui.modal').modal();
        $('.ui.dropdown').dropdown();
        $('[data-content]').popup();

        atualizarBlocosPorCampus('.novo.modal');

        $('.novo.button').click(function () { $('.ui.novo.modal').modal('show') });
        $('.editar.button').click(function () {
            var _this = $(this);
            $.ajax({
                url: '/simulado/gerencia/carregarsala',
                method: 'POST',
                data: {
                    'sala': _this.data('sala')
                },
                beforeSend: function () {
                    _this.addClass('loading');
                },
                success: function (data) {
                    $('form.editar').attr('action', '/simulado/gerencia/editarsala/' + _this.data('sala'))
                    $('.ui.editar.modal').html(data).modal('show');
                    $('.ui.editar.modal .ui.dropdown').dropdown();
                    atualizarBlocosPorCampus('.editar.modal');
                },
                error: function () {
                    siac.mensagem('Falha ao recuperar a Sala para edição. Atualize a página para tentar novamente.');
                },
                complete: function () {
                    _this.removeClass('loading');
                    siac.Gerencia.adicionarEventoNoFormulario();
                }
            });
        });
        $('.excluir.button').click(function () {
            var _this = $(this),
                $modal = $('.ui.excluir.modal'),
                sala = _this.data('sala'),
                salaDescricao = _this.closest('tr').find('[sala-descricao]').text(),
                salaCampus = _this.closest('tr').find('[sala-campus]').data('content'),
                salaBloco = _this.closest('tr').find('[sala-bloco]').data('content'),
                salaSigla = _this.closest('tr').find('[sala-sigla]').text();

            $modal.find('[sala-descricao]').text(salaDescricao);
            $modal.find('[sala-campus]').text(salaCampus);
            $modal.find('[sala-bloco]').text(salaBloco);
            $modal.find('[sala-sigla]').text(salaSigla);

            $modal.modal({
                onApprove: function () {
                    $.ajax({
                        url: '/simulado/gerencia/excluirsala',
                        method: 'POST',
                        data: {
                            'codigo': sala
                        },
                        complete: function () {
                            location.reload();
                        }
                    });
                }
            }).modal('show');
        });
        siac.Gerencia.adicionarEventoNoFormulario();
    }

    function atualizarBlocosPorCampus(contexto) {
        var ddlCampus = $(contexto + ' [name=ddlCampus]'),
            ddlBloco = $(contexto + ' [name=ddlBloco]');

        ddlCampus.off('change').change(function () {
            var _this = $(this),
                codCampus = _this.val();

            ddlBloco.html('<option value="">Bloco</option>');

            for (var i = 0, length = _blocos.length; i < length; i++) {
                if (_blocos[i].Campus == codCampus) {
                    ddlBloco.append('<option value="' + _blocos[i].CodBloco + '">' + _blocos[i].Descricao + '</option>');
                }
            }

            ddlBloco
                .dropdown('refresh')
                .dropdown('set placeholder text', 'Bloco')
                .dropdown('set value', '')
            ;
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Gerencia.Disciplinas = (function () {
    function iniciar() {
        $('.ui.modal').modal();

        $('.ui.dropdown').dropdown();

        $('.novo.button').click(function () { $('.ui.novo.modal').modal('show') });

        $('.editar.button').click(function () {
            var _this = $(this);
            $.ajax({
                url: '/simulado/gerencia/carregardisciplina',
                method: 'POST',
                data: {
                    'disciplina': _this.data('disciplina')
                },
                beforeSend: function () {
                    _this.addClass('loading');
                },
                success: function (data) {
                    $('form.editar').attr('action', '/simulado/gerencia/editardisciplina/' + _this.data('disciplina'))
                    $('.ui.editar.modal').html(data).modal('show');
                },
                error: function () {
                    siac.mensagem('Falha ao recuperar a Disciplina para edição. Atualize a página para tentar novamente.');
                },
                complete: function () {
                    _this.removeClass('loading');
                    siac.Gerencia.adicionarEventoNoFormulario();
                }
            });
        });

        $('.excluir.button').click(function () {
            var _this = $(this),
                $modal = $('.ui.excluir.modal'),
                disciplina = _this.data('disciplina'),
                disciplinaDescricao = _this.closest('tr').find('[disciplina-descricao]').text(),
                disciplinaSigla = _this.closest('tr').find('[disciplina-sigla]').text();

            $modal.find('[disciplina-descricao]').text(disciplinaDescricao);
            $modal.find('[disciplina-sigla]').text(disciplinaSigla);

            $modal.modal({
                onApprove: function () {
                    $.ajax({
                        url: '/simulado/gerencia/excluirdisciplina',
                        method: 'POST',
                        data: {
                            'codigo': disciplina
                        },
                        complete: function () {
                            location.reload();
                        }
                    });
                }
            }).modal('show');
        });
        siac.Gerencia.adicionarEventoNoFormulario();
    }

    return {
        iniciar: iniciar
    }
})();

siac.Gerencia.Professores = (function () {
    function iniciar() {
        $('.ui.modal').modal();

        $('.ui.dropdown').dropdown();

        $('.novo.button').click(function () { $('.ui.novo.modal').modal('show') });

        $('.disciplinas.button').click(function () {
            var _this = $(this),
                _professor = _this.data('professor');

            $.ajax({
                url: '/simulado/gerencia/carregarprofessordisciplinas',
                method: 'POST',
                data: {
                    'professor': _professor
                },
                beforeSend: function () {
                    _this.addClass('loading');
                },
                success: function (data) {
                    $('.ui.disciplinas.modal').html(data).modal('show');
                },
                error: function () {
                    siac.mensagem('Falha ao recuperar o Professor para listagem. Atualize a página para tentar novamente.');
                },
                complete: function () {
                    _this.removeClass('loading');
                }
            });
        });

        $('.editar.button').click(function () {
            var _this = $(this)
            _professor = _this.data('professor');;
            $.ajax({
                url: '/simulado/gerencia/carregarprofessor',
                method: 'POST',
                data: {
                    'professor': _professor
                },
                beforeSend: function () {
                    _this.addClass('loading');
                },
                success: function (data) {
                    $('form.editar').attr('action', '/simulado/gerencia/editarprofessor/' + _professor)
                    $('.ui.editar.modal').html(data).modal('show');
                    $('.ui.editar.modal .ui.dropdown').dropdown();
                },
                error: function () {
                    siac.mensagem('Falha ao recuperar o Professor para edição. Atualize a página para tentar novamente.');
                },
                complete: function () {
                    _this.removeClass('loading');
                    siac.Gerencia.adicionarEventoNoFormulario();
                }
            });
        });

        $('.excluir.button').click(function () {
            var _this = $(this),
                $modal = $('.ui.excluir.modal'),
                professor = _this.data('professor'),
                professorNome = _this.closest('tr').find('[professor-nome]').text(),
                professorMatricula = _this.closest('tr').find('[professor-matricula]').text();

            $modal.find('[professor-nome]').text(professorNome);
            $modal.find('[professor-matricula]').text(professorMatricula);

            $modal.modal({
                onApprove: function () {
                    $.ajax({
                        url: '/simulado/gerencia/excluirprofessor',
                        method: 'POST',
                        data: {
                            'codigo': professor
                        },
                        complete: function () {
                            location.reload();
                        }
                    });
                }
            }).modal('show');
        });

        siac.Gerencia.adicionarEventoNoFormulario();
    }

    return {
        iniciar: iniciar
    }
})();

siac.Gerencia.Provas = (function () {
    function iniciar() {
        $('.ui.modal').modal();

        $('.abrir.button').off('click').on('click', function () {
            var $this = $(this),
                simulado = $this.data('simulado'),
                dia = $this.data('dia'),
                prova = $this.data('prova');

            $this.addClass('loading');

            $.ajax({
                url: '/simulado/gerencia/carregarprovaconfigurar',
                method: 'POST',
                data: {
                    'simulado': simulado,
                    'dia': dia,
                    'prova': prova
                },
                success: function (data) {
                    $('.ui.prova.modal').html(data).modal('show');
                    adicionarEventos(simulado, dia, prova);
                    $('[data-content], [data-html]').popup();
                    $('.ui.accordion').accordion({ animateChildren: false });
                    $('.right.floated.button').popup({
                        inline: true,
                        on: 'click'
                    });
                    $('[data-content]').popup();
                    $('#recarregarQuestoes').off('submit').on('submit', function () {
                        var $this = $(this);
                        $this.closest('.ui.modal').modal('hide');
                        $this.find('button[type=submit]').addClass('loading');
                    });
                },
                error: function () {
                    siac.mensagem('Falha ao recuperar o Prova para Configuração. Atualize a página para tentar novamente.');
                },
                complete: function () {
                    $this.removeClass('loading');
                    siac.Gerencia.adicionarEventoNoFormulario();
                }
            });
        });
    }

    function adicionarEventos(simulado, dia, prova) {
        siac.Anexo.iniciar();

        $('.trocar.button').off().click(function () {
            var $_this = $(this);
            var questao = $_this.parents('[data-questao]').attr('data-questao');
            var indice = $_this.parents('[data-indice]').attr('data-indice');

            trocar(simulado, dia, prova, questao, indice);
        });

        $('.desfazer.button').off().click(function () {
            var $_this = $(this);
            var questao = $_this.parents('[data-questao]').attr('data-questao');
            var indice = $_this.parents('[data-indice]').attr('data-indice');

            desfazer(simulado, dia, prova, questao, indice);
        });
    }

    function trocar(simulado, dia, prova, questao, indice) {
        var card = $('#cardQuestao' + indice);
        card.addClass('ui form loading');
        $('#cardQuestao' + indice + ' div').popup('hide');
        $.ajax({
            type: "POST",
            url: '/simulado/gerencia/trocarprovaconfigurar',
            data: {
                'indice': indice,
                'questao': questao,
                'simulado': simulado,
                'dia': dia,
                'prova': prova
            },
            success: function (data) {
                if (data) {
                    card.html(data);
                    $('.ui.accordion').accordion({ animateChildren: false });
                    $('.right.floated.button').popup({ on: 'click', position: 'left center' });
                    $('.ui.button.disabled').removeClass('disabled');
                    $('#cardQuestao' + indice + ' .ui.desfazer.button').parents('.popup').prev().show();
                }
                else {
                    siac.Lembrete.iniciar();
                }
                card.removeClass('ui form loading');
            },
            error: function () {
                card.removeClass('ui form loading');
                siac.mensagem('Ocorreu um erro não esperado.');
            },
            complete: function () {
                adicionarEventos(simulado, dia, prova, questao, indice);
            }
        });
    }

    function desfazer(simulado, dia, prova, questao, indice) {
        var card = $('#cardQuestao' + indice);
        card.addClass('ui form loading');
        $('#cardQuestao' + indice + ' div').popup('hide');
        $.ajax({
            type: 'POST',
            url: '/simulado/gerencia/desfazerprovaconfigurar',
            data: {
                'indice': indice,
                'questao': questao,
                'simulado': simulado,
                'dia': dia,
                'prova': prova
            },
            success: function (data) {
                if (data) {
                    card.html(data);
                    $('.ui.accordion').accordion({ animateChildren: false });
                    $('.right.floated.button').popup({ on: 'click', position: 'left center' });
                    $('#cardQuestao' + indice + ' .ui.desfazer.button').parents('.popup').prev().hide();
                }
                card.removeClass('ui form loading');
            },
            error: function () {
                card.removeClass('ui form loading');
                siac.mensagem('Ocorreu um erro não esperado.');
            },
            complete: function () {
                adicionarEventos(simulado, dia, prova, questao, indice);
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Gerencia.Permissoes = (function () {
    function iniciar() {
        $('.ui.checkbox').checkbox();
        $('[id^=chkCoordenador]').change(function () {
            console.log('[id^=chkCoordenador] change()');
            if ($('[id^=chkCoordenador]:checked').length) {
                $('.coordenadores .trigger.button').removeClass('disabled');
            }
            else {
                $('.coordenadores .trigger.button').addClass('disabled');
            }
        });
        $('[id^=chkColaborador]').change(function () {
            console.log('[id^=chkColaborador] change()');
            if ($('[id^=chkColaborador]:checked').length) {
                $('.colaboradores .trigger.button').removeClass('disabled');
            }
            else {
                $('.colaboradores .trigger.button').addClass('disabled');
            }
        });

        $('.trigger.button').popup({ inline: true, on: 'click', position: 'right center' });

        $('.coordenadores .adicionar.button').click(function () {
            $('.pessoas.modal .header').text('Adicionar Coordenador de Simulado');
            $('.pessoas.modal').modal({
                onApprove: function () {
                    if ($('[name=rdoPessoa]:checked').length) {
                        var codPessoa = $('[name=rdoPessoa]:checked').val();
                        adicionarCoordenador(codPessoa);
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
            }).modal('show');
        });

        $('.colaboradores .adicionar.button').click(function () {
            $('.pessoas.modal .header').text('Adicionar Colaborador de Simulado');
            $('.pessoas.modal').modal({
                onApprove: function () {
                    if ($('[name=rdoPessoa]:checked').length) {
                        var codPessoa = $('[name=rdoPessoa]:checked').val();
                        adicionarColaborador(codPessoa);
                        $('.approve.button').addClass('loading');
                    }
                    else {
                        siac.mensagem('Ops... Você ainda não selecionou nenhuma pessoa!');
                    }
                    return false;
                },
                onDeny: function () {
                    $('.pessoas.modal #txtPesquisa').val('');
                    $('.pessoas.modal .ui.list').html('');
                }
            }).modal('show');
        });

        $('.pesquisar.button').click(function () {
            var pesquisa = $('#txtPesquisa').val();
            console.log('.pesquisar.button click()', pesquisa);
            if (pesquisa && pesquisa.length >= 3) {
                $(this).parents('.form').addClass('loading');
                pesquisar(pesquisa);
            }
        });

        $('.ui.modal').modal();

        $('.colaboradores .remover.button').click(function () {
            removerColaborador();
        });
        $('.coordenadores .remover.button').click(function () {
            removerCoordenador();
        });
    }

    function adicionarCoordenador(codPessoa) {
        if (codPessoa) {
            $.ajax({
                url: '/simulado/gerencia/adicionarcoordenador',
                type: 'post',
                data: { codPessoaFisica: codPessoa },
                success: function () { window.location.reload(); $('.ui.modal').modal('hide'); },
                error: function () { siac.mensagem('Ocorreu um erro com sua solicitação. Por favor, tente novamente mais tarde.'); }
            });
        }
    }

    function adicionarColaborador(codPessoa) {
        if (codPessoa) {
            $.ajax({
                url: '/simulado/gerencia/adicionarcolaborador',
                type: 'post',
                data: { codPessoaFisica: codPessoa },
                success: function () { window.location.reload(); $('.ui.modal').modal('hide'); },
                error: function () { siac.mensagem('Ocorreu um erro com sua solicitação. Por favor, tente novamente mais tarde.'); }
            });
        }
    }

    function pesquisar(pesquisa) {
        console.log('on pesquisar()', pesquisa);
        if (pesquisa && pesquisa.length >= 3) {
            var $modal = $('.pessoas.modal');
            $.ajax({
                url: '/simulado/gerencia/listarpessoas',
                type: 'post',
                data: {
                    pesquisa: pesquisa
                },
                success: function (data) {
                    console.log('on success()', data);
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
                error: function () {
                    siac.mensagem('Ocorreu um erro com sua solicitação. Por favor, tente novamente mais tarde.');
                },
                complete: function () {
                    $modal.find('.form').removeClass('loading');
                }
            });
        }
    }

    function removerCoordenador() {
        var $btnRemover = $('.coordenadores .remover.button');
        $btnRemover.addClass('loading');
        var arrPessoas = [];
        var $chkCoordenadores = $('[id^=chkCoordenador]');

        for (var i = 0, length = $chkCoordenadores.length; i < length; i++) {
            if ($chkCoordenadores.eq(i).is(':checked')) {
                var codPessoa = $chkCoordenadores.eq(i).attr('id').replace(/^\D+/g, '');
                arrPessoas.push(parseInt(codPessoa));
            }
        }

        $.ajax({
            url: '/simulado/gerencia/removercoordenador',
            type: 'post',
            data: { codPessoaFisica: arrPessoas },
            success: function () { window.location.reload(); },
            error: function () { siac.mensagem('Ocorreu um erro com sua solicitação. Por favor, tente novamente mais tarde.'); },
            complete: function () { $btnRemover.removeClass('loading'); }
        });
    }

    function removerColaborador() {
        var $btnRemover = $('.colaboradores .remover.button');
        $btnRemover.addClass('loading');
        var arrPessoas = [];
        var $chkCoordenadores = $('[id^=chkColaborador]');

        for (var i = 0, length = $chkCoordenadores.length; i < length; i++) {
            if ($chkCoordenadores.eq(i).is(':checked')) {
                var codPessoa = $chkCoordenadores.eq(i).attr('id').replace(/^\D+/g, '');
                arrPessoas.push(parseInt(codPessoa));
            }
        }

        $.ajax({
            url: '/simulado/gerencia/removercolaborador',
            type: 'post',
            data: { codPessoaFisica: arrPessoas },
            success: function () { window.location.reload(); },
            error: function () { siac.mensagem('Ocorreu um erro com sua solicitação. Por favor, tente novamente mais tarde.'); },
            complete: function () { $btnRemover.removeClass('loading'); }
        });
    }

    return { iniciar: iniciar }
})();