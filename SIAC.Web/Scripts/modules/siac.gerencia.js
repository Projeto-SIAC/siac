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