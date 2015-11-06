siac.Questao = siac.Questao || {};

siac.Questao.Index = (function () {
    var _controleTimeout, _controlePartial, _controleQte = 10, _controleAjax;

    var pagina = 1;
    var ordenar = "data_desc";
    var dificuldade = ""; 
    var disciplina = "";
    var tema = "";
    var tipos = []; 
    var pesquisa = ""; 

    function iniciar() {
        $('.ui.dropdown').dropdown();       

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

        $('.tipo.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            var _tipo = $_this.attr('data-tipo');
            if ($_this.hasClass('active')) {
                var _tempTipos = tipos;
                tipos = [];
                for (var i = 0, length = _tempTipos.length; i < length; i++) {
                    if (_tempTipos[i] != _tipo) {
                        tipos.push(_tempTipos[i]);
                    }
                }
                $_this.removeClass('active');
            }
            else {
                tipos.push(_tipo);
                $_this.addClass('active');
            }
            listar();
        });

        $('.dificuldade.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            dificuldade = $_this.attr('data-dificuldade');
            listar();
        });

        $('.disciplina.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            disciplina = $_this.attr('data-disciplina');
            if (!disciplina) {
                tema = "";
            }
            listar();
        });

        $('.tema.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            disciplina = $_this.parents('[data-disciplina]').attr('data-disciplina');
            tema = $_this.attr('data-tema');
            listar();
        });

        $('.ordenar.item').click(function () {
            var $_this = $(this);
            pagina = 1;
            ordenar = $_this.attr('data-ordenar');
            listar();
            $('.ordenar.item').removeClass('active');
            $_this.addClass('active');
        });

        $(window).scroll(function () {
            if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {
                if ($('.cards .card').length == (_controleQte * pagina)) {
                    pagina++;
                    listar();
                }
            }
        });

        listar();
    };
    
    function listar() {
        if (_controleAjax && _controleAjax.readyState != 4) {
            _controleAjax.abort();
        }
        $cards = $('.ui.cards');
        $cards.parent().addClass('loading');
        _controleAjax = $.ajax({
            url: '/Historico/Questao/Listar',
            data: { 
                pagina: pagina,
                ordenar: ordenar,
                dificuldade: dificuldade,
                disciplina: disciplina,
                tema: tema,
                tipos: tipos,
                pesquisa: pesquisa
            },
            method: 'POST',
            success: function (partial) {
                if (partial != _controlePartial) {
                    if (pagina == 1) {
                        $cards.html(partial);
                    }
                    else {
                        $cards.append(partial);
                    }
                    _controlePartial = partial;
                }
            },
            complete: function () {
                $cards.parent().removeClass('loading');
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

siac.Questao.Cadastrar = (function () {
    function iniciar() {
        $('.ui.checkbox').checkbox();

        $('.ui.accordion').accordion({animateChildren: false});

        $('.ui.dropdown').dropdown();

        $('.tabular.menu .item').tab();

        $('.ui.termo.modal')
            .modal({
                closable: false,
                onApprove: function () {
                    $('#btnPesquisa').popup('show')
                }
            }).modal('show');

        $('.cancelar.button').popup({ on: 'click' });

        $('.ui.confirmar.modal')
          .modal({
              onApprove: function () {
                  $('.ui.confirmar.modal .approve').addClass('loading');
                  checarCaptcha();
                  return false;
              }
          });

        $('.ui.confirmar.modal .ui.accordion').accordion({
            animateChildren: false,
            onChange: function () {
                $('.ui.confirmar.modal').modal('refresh');
            }
        });

        $('#btnPesquisa').popup({
            title: 'Pesquisar questão',
            content: 'Clique aqui para descobrir se sua questão já foi cadastrada',
            duration: 100
        });

        $('.ui.pesquisa.modal').modal();

        $('.tabular.menu .item').tab({
            history: true,
            historyType: 'state',
            path: window.location.pathname
        });

        mostrarCamposPorTipo();

        mostrarOpcaoAnexos();

        for (var i = 0; i < 5; i++) {
            adicionarAlternativa();
        }

        // Eventos
        $('.prosseguir.button').click(function () {
            verificar();
        });

        $('#btnPesquisa').click(function () {
            $('.ui.pesquisa.modal').modal('show');
        });

        $('.pesquisa.modal .pesquisar.button').click(function () {
            palavraChave($('#txtPalavraChave').val());
            return false;
        });

        $('.captcha.icon').click(function () {
            novoCaptcha();
        });

        $('#ddlDisciplina').change(function () {
            recuperarTemasPorCodDisciplina();
        });

        $('#ddlTipo').change(function () {
            mostrarCamposPorTipo();
        });

        $('#chkAnexos').change(function () {
            mostrarOpcaoAnexos();
        });

        $('.questao.objetiva .adicionar.button').click(function () {
            adicionarAlternativa();
        });

        $('.anexos .adicionar.button').click(function () {
            adicionarAnexo();
        });

        $('.ui.shape').shape({
            onChange: function () {
                $('.pesquisa.modal').modal('refresh');
            }
        });
    }

    function recuperarTemasPorCodDisciplina() {
        var selecionado = $('#ddlDisciplina :selected').val();
        var ddlTema = $('#ddlTema');
        ddlTema.parent().addClass('loading');
        $.ajax({
            cache: false,
            type: 'POST',
            url: '/Tema/RecuperarTemasPorCodDisciplina',
            data: { "codDisciplina": selecionado },
            success: function (data) {
                ddlTema.html('');
                ddlTema.parent().find('.label').remove();
                $.each(data, function (id, option) {
                    ddlTema.append($('<option></option>').val(option.CodTema).html(option.Descricao));
                });
                ddlTema.parent().removeClass('loading');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                siac.mensagem('Falha ao recuperar temas.');
                ddlTema.parent().removeClass('loading');
            }
        });
    }

    function mostrarCamposPorTipo() {
        var tipo = $('#ddlTipo :selected').val();
        $('.questao').hide();
        if (tipo == 1) {
            $('.questao.objetiva').show();
            $('.segment.questao.objetiva').attr('style', '');
        }
        else if (tipo == 2) {
            $('.questao.discursiva').show();
        }
    }

    function mostrarOpcaoAnexos() {
        var chk = $('#chkAnexos');
        if (chk.is(':checked')) {
            $('.anexos').show();
            $('.segment.anexos').attr('style', '');
        }
        else {
            $('.anexos').hide();
        }
    }

    function marcarCorreta(chk) {
        $chk = $(chk);
        $lstCheckboxes = $('.ui.alternativas.accordion .content input[type="checkbox"]');
        $('.ui.alternativas.accordion .title .label').attr('style', 'display:none');
        for (var i = 0; i < $lstCheckboxes.length; i++) {
            if ($chk.is(':checked')) {
                if ($chk.attr('id') == $lstCheckboxes.eq(i).attr('id')) {
                    $('.ui.alternativas.accordion .title.active .label').removeAttr('style');
                    continue;
                }
                $lstCheckboxes.eq(i).attr({ 'readonly': 'readonly', 'disabled': 'disabled' });
                $lstCheckboxes.eq(i).parent().addClass('disabled');
            }
            else {
                $lstCheckboxes.eq(i).removeAttr('readonly disabled');
                $lstCheckboxes.eq(i).parent().removeClass('disabled');
            }
        }
    }

    function carregarImagem(file) {
        var ind = $(file).parent().parent().parent().prev().find('#txtAnexoIndex').val();
        var pre = $('#preImagem' + ind);
        var arq = $('#fleAnexo' + ind)[0].files[0];
        var reader = new FileReader();

        if (arq.size < 1000000) {
            reader.onloadend = function () {
                pre.attr('src', reader.result);
            }

            if (arq) {
                reader.readAsDataURL(arq);
            } else {
                preview.attr('src', '');
            }
        }
        else {
            siac.mensagem("Selecione um arquivo menor.");
        }
    }

    function adicionarAlternativa() {
        var i = $('.ui.alternativas.accordion .title').length + 1;
        $('.ui.alternativas.accordion').append('\
                        <div class="title">\
                            <input id="txtAlternativaIndex" value="'+ i + '" hidden />\
                            <i class="dropdown icon"></i>Alternativa ' + i + '\
                            <div class="ui small label" style="display:none">Correta</div>\
                        </div>\
                        <div class="content ui segment">\
                            <div class="field required">\
                                <label for="txtAlternativaEnunciado' + i + '">Enunciado</label>\
                                <textarea id="txtAlternativaEnunciado' + i + '" name="txtAlternativaEnunciado' + i + '" rows="2" placeholder="Enunciado..."></textarea>\
                            </div>\
                            <div class="field">\
                                <label for="txtAlternativaComentario' + i + '">Comentário</label>\
                                <textarea id="txtAlternativaComentario' + i + '" name="txtAlternativaComentario' + i + '" rows="2" placeholder="Comentário..."></textarea>\
                            </div>\
                            <div class="field">\
                                <div class="ui toggle checkbox">\
                                    <input id="chkAlternativaCorreta' + i + '" name="chkAlternativaCorreta' + i + '" type="checkbox" tabindex="0" class="hidden">\
                                    <label for="chkAlternativaCorreta' + i + '">Correta</label>\
                                </div>\
                            </div>\
                            <div class="field">\
                                <div class="ui button">\
                                    Remover\
                                </div>\
                                <div class="ui special popup">\
                                    <div class="header">Tem certeza?</div>\
                                    <div class="content"><p>Essa ação não poderá ser desfeita.</p>\
                                    <div class="ui right aligned remover button tiny">Sim, remover</div>\
                                </div>\
                            </div>\
                        </div>\
                    </div>');
        $('#txtQtdAlternativas').val($('.ui.alternativas.accordion .title').length);
        $('[id^="chkAlternativaCorreta"]').off().change(function () {
            marcarCorreta(this);
        });
        $('.ui.alternativas.accordion .remover.button').off().click(function () {
            removerAlternativa(this);
        });
        $('.ui.checkbox').checkbox();
        $('.ui.alternativas.accordion .button').popup({ inline: true, on: 'click', position: 'right center' });
    }

    function renomearAlternativas() {
        var list = $('.ui.alternativas.accordion .title');
        var listContent = $('.ui.alternativas.accordion .content.segment');
        for (var i = 0; i < list.length; i++) {
            var j = list.eq(i).find('#txtAlternativaIndex').val();
            list.eq(i).html('<input id="txtAlternativaIndex" value="' + (i + 1) + '" hidden /><i class="dropdown icon"></i>Alternativa ' + (i + 1) + '<div class="ui small label" style="display:none">Correta</div>');
            if (listContent.eq(i).find('input[name="chkAlternativaCorreta' + j + '"]').is(':checked')) {
                list.eq(i).find('.label').removeAttr('style');
            }
            /* RENOMEAR LABELS, INPUTS e TEXTAREAS */
            listContent.eq(i).find('label[for="txtAlternativaEnunciado' + j + '"]').attr('for', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('label[for="txtAlternativaComentario' + j + '"]').attr('for', 'txtAlternativaComentario' + (i + 1));
            listContent.eq(i).find('label[for="chkAlternativaCorreta' + j + '"]').attr('for', 'chkAlternativaCorreta' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAlternativaEnunciado' + j + '"]').attr('name', 'txtAlternativaEnunciado' + (i + 1)).attr('id', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAlternativaComentario' + j + '"]').attr('name', 'txtAlternativaComentario' + (i + 1)).attr('id', 'txtAlternativaComentario' + (i + 1));
            listContent.eq(i).find('input[name="chkAlternativaCorreta' + j + '"]').attr('name', 'chkAlternativaCorreta' + (i + 1)).attr('id', 'chkAlternativaCorreta' + (i + 1));
        }
    }

    function removerAlternativa(button) {
        var i = $('.ui.alternativas.accordion .title').length;
        if (i > 2) {
            var content = $(button).parent().parent().parent().parent();
            var title = $(content).prev();
            title.remove();
            content.remove();
            renomearAlternativas();
        }
        $('#txtQtdAlternativas').val($('.ui.alternativas.accordion .title').length);
    }

    function adicionarAnexo() {
        var tipoAnexo = $('#ddlTiposAnexo').val();
        var tipoAnexoDescricao = $('#ddlTiposAnexo :selected').text();
        var i = $('.ui.anexos.accordion .title').length + 1;
        if (tipoAnexo == 1) {
            $('.ui.anexos.accordion').append('\
                        <div class="title">\
                            <input id="txtAnexoIndex" value="'+ i + '" class="disabled" readonly hidden/>\
                            <input name="txtAnexoTipo' + i + '" id="txtAnexoTipo' + i + '" value="' + tipoAnexo + '" hidden />\
                            <i class="dropdown icon"></i>Anexo ' + i + '\
                            <span class="ui label" id="txtAnexoTipoDescricao">'+ tipoAnexoDescricao + '</span>\
                        </div>\
                        <div class="content ui segment">\
                            <div class="field required">\
                                <div class="ui card"><div class="image"><img name="preImagem' + i + '" id="preImagem' + i + '" alt="Previsualização" src="" /></div>\
                                <input id="fleAnexo' + i + '" name="fleAnexo' + i + '"type="file" class="ui button" accept="image/*" /></div>\
                            </div>\
                            <div class="field required">\
                                <label for="txtAnexoLegenda' + i + '">Legenda</label>\
                                <textarea maxlength="250" id="txtAnexoLegenda' + i + '" name="txtAnexoLegenda' + i + '" rows="2" placeholder="Legenda..."></textarea>\
                            </div>\
                            <div class="field">\
                                <label for="txtAnexoFonte' + i + '">Fonte</label>\
                                <input maxlength="250" type="text" id="txtAnexoFonte' + i + '" name="txtAnexoFonte' + i + '" placeholder="Fonte..."></textarea>\
                            </div>\
                            <div class="field">\
                                <div class="ui button">\
                                    Remover\
                                </div>\
                                <div class="ui special popup">\
                                    <div class="header">Tem certeza?</div>\
                                    <div class="content"><p>Essa ação não poderá ser desfeita.</p>\
                                    <div class="ui right aligned remover button tiny">Sim, remover</div>\
                                </div>\
                            </div>\
                        </div>\
                    </div>');
        }
        $('.anexos.accordion [id^="fleAnexo"]').off().change(function () {
            carregarImagem(this);
        });
        $('#txtQtdAnexos').val($('.ui.anexos.accordion .title').length);
        $('.anexos.accordion .remover.button').off().click(function () {
            removerAnexo(this);
        });
        $('.ui.anexos.accordion .button').popup({ inline: true, on: 'click', position: 'right center' });
    }

    function renomearAnexos() {
        var list = $('.ui.anexos.accordion .title');
        var listContent = $('.ui.anexos.accordion .content.segment');
        for (var i = 0; i < list.length; i++) {
            var j = list.eq(i).find('#txtAnexoIndex').val();
            var tipoAnexo = list.eq(i).find('#txtAnexoTipo' + j).val();
            var tipoAnexoDescricao = list.eq(i).find('#txtAnexoTipoDescricao').text();
            list.eq(i).html('<input id="txtAnexoIndex" value="' + (i + 1) + '" class="disabled" readonly hidden/><i class="dropdown icon"></i>Anexo ' + (i + 1) + '<input name="txtAnexoTipo' + (i + 1) + '" id="txtAnexoTipo' + (i + 1) + '" value="' + tipoAnexo + '" hidden /><span class="ui label" id="txtAnexoTipoDescricao">' + tipoAnexoDescricao + '</span>');
            listContent.eq(i).find('label[for="txtAnexoLegenda' + j + '"]').attr('for', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('label[for="txtAnexoFonte' + j + '"]').attr('for', 'txtAlternativaComentario' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAnexoLegenda' + j + '"]').attr('name', 'txtAnexoLegenda' + (i + 1)).attr('id', 'txtAnexoLegenda' + (i + 1));
            listContent.eq(i).find('input[name="txtAnexoFonte' + j + '"]').attr('name', 'txtAnexoFonte' + (i + 1)).attr('id', 'txtAnexoFonte' + (i + 1));
            listContent.eq(i).find('input[name="fleAnexo' + j + '"]').attr('name', 'fleAnexo' + (i + 1)).attr('id', 'fleAnexo' + (i + 1));
            listContent.eq(i).find('img[name="preImagem' + j + '"]').attr('name', 'preImagem' + (i + 1)).attr('id', 'preImagem' + (i + 1));
        }
    }

    function removerAnexo(button) {
        var content = $(button).parent().parent().parent().parent();
        var title = $(content).prev();
        title.remove();
        content.remove();
        renomearAnexos();
        $('#txtQtdAnexos').val($('.ui.anexos.accordion .title').length);
    }

    function verificar() {
        $('.ui.error.message .list').html('');
        var validado = false;
        if (!$('#ddlDisciplina').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar uma disciplina</li>')
        }

        if (!$('#ddlTema :selected').length > 0) {
            $('.ui.error.message .list').append('<li>É necessário selecionar pelo menos um tema</li>')
        }
        if (!$('#ddlDificuldade').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar uma dificuldade</li>')
        }

        if (!$('#ddlTipo').val()) {
            $('.ui.error.message .list').append('<li>É necessário selecionar um tipo</li>')
        }

        if (!$('#txtEnunciado').val()) {
            $('.ui.error.message .list').append('<li>É necessário preencher o enunciado</li>')
        }

        if ($('#ddlTipo').val()) {
            var tipo = $('#ddlTipo').val();

            if (tipo == 1) {
                var qteAlternativas = $('#txtQtdAlternativas').val();
                var ok = true;
                for (var i = 0; i < qteAlternativas; i++) {
                    if ($('#txtAlternativaEnunciado' + (i + 1)).val() == '') {
                        ok = false;
                    }
                }
                if (ok) {
                    ok = false;
                    for (var i = 0; i < qteAlternativas; i++) {
                        if ($('#chkAlternativaCorreta' + (i + 1)).is(':checked')) {
                            ok = true;
                        }
                    }
                    if (ok) {
                        if ($('#chkAnexos').is(':checked')) {
                            var qteAnexos = $('#txtQtdAnexos').val();
                            for (var i = 0; i < qteAnexos; i++) {
                                if ($('#fleAnexo' + (i + 1))[0].files.length == 0) {
                                    ok = false;
                                }
                            }
                            if (ok) {
                                validado = true;
                            }
                            else {
                                $('.ui.error.message .list').append('<li>É necessário selecionar os arquivos dos anexos</li>')
                            }
                        }
                        else {
                            validado = true;
                        }
                    }
                    else {
                        $('.ui.error.message .list').append('<li>É necessário selecionar pelo menos umas das alternativas como correta</li>')
                    }
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher os enunciados de todas as alternativas</li>')
                }
            }
            else if (tipo == 2) {
                if ($('#txtChaveDeResposta').val() != '') {
                    if ($('#chkAnexos').is(':checked')) {
                        var ok = true;
                        var qteAnexos = $('#txtQtdAnexos').val();
                        for (var i = 0; i < qteAnexos; i++) {
                            if ($('#fleAnexo' + (i + 1))[0].files.length == 0) {
                                ok = false;
                            }
                        }
                        if (ok) {
                            validado = true;
                        }
                        else {
                            $('.ui.error.message .list').append('<li>É necessário selecionar os arquivos dos anexos</li>')
                        }
                    }
                    else {
                        validado = true;
                    }
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher a chave de resposta</li>')
                }
            }
        }

        if (validado) confirmar();
        else $('#frmQuestao').addClass('error');
    }

    function confirmar() {
        $('#mdlDisciplina').text($('#ddlDisciplina :selected').text());
        $('#mdlDificuldade').text($('#ddlDificuldade :selected').text());
        var temas = $('#ddlTema :selected');
        $('#mdlTagTemas').html('');
        for (var i = 0; i < temas.length; i++) {
            $('#mdlTagTemas').append('<div class="ui tag label">' + temas.eq(i).text() + '</div>');
        }
        $('#mdlEnunciado').text($('#txtEnunciado').val()).attr('data-html', '<b>Objetivo</b>: ' + $('#txtObjetivo').val());
        var tipo = $('#ddlTipo').val();
        if (tipo == 1) {
            var qteAlternativas = $('#txtQtdAlternativas').val();
            $('#mdlListAlternativas').html('');
            for (var i = 0; i < qteAlternativas; i++) {
                if ($('#chkAlternativaCorreta' + (i + 1)).is(':checked')) {
                    $('#mdlListAlternativas')
                    .append('<div class="item">\
                                    <div class="middle aligned content" data-html="<b>Comentário</b>: ' + $('#txtAlternativaComentario' + (i + 1)).val() + '">\
                                        <b>' + (i + 1) + '</b>) ' + $('#txtAlternativaEnunciado' + (i + 1)).val() + '<a class="ui green label">Correta</a>\
                                    </div>\
                                </div>'
                    );
                    continue;
                }
                $('#mdlListAlternativas')
                    .append('<div class="item">\
                                    <div class="middle aligned content" data-html="<b>Comentário</b>: ' + $('#txtAlternativaComentario' + (i + 1)).val() + '">\
                                        <b>' + (i + 1) + '</b>) ' + $('#txtAlternativaEnunciado' + (i + 1)).val() + '\
                                    </div>\
                                </div>'
                    );
            }
            $('#mdlChaveDeResposta').attr('style', 'display:none');
            $('#mdlListAlternativas').removeAttr('style');
        }
        else if (tipo == 2) {
            $('#mdlChaveDeResposta').html('<i>' + $('#txtChaveDeResposta').val() + '</i><a class="ui green label">Chave de Resposta</a>').attr('data-html', '<b>Comentário</b>: ' + $('#txtComentario').val());
            $('#mdlListAlternativas').attr('style', 'display:none');
            $('#mdlChaveDeResposta').removeAttr('style');
        }
        if ($('#chkAnexos').is(':checked')) {
            var qteAnexos = $('#txtQtdAnexos').val();
            $('#mdlCardAnexos').html('');
            for (var i = 0; i < qteAnexos; i++) {
                var tipoAnexo = $('#txtAnexoTipo' + (i + 1)).val();
                if (tipoAnexo == 1) {
                    $('#mdlCardAnexos')
                        .append('<div class="ui card">\
                                        <div class="image">\
                                            <img src="' + $('#preImagem' + (i + 1)).attr('src') + '" />\
                                        </div>\
                                        <div class="content">\
                                            <div class="header">\
                                                ' + $('#txtAnexoLegenda' + (i + 1)).val() + '\
                                            </div>\
                                            <div class="description">\
                                                ' + $('#txtAnexoFonte' + (i + 1)).val() + '\
                                            </div>\
                                        </div>\
                                    </div>'
                    );
                }
            }
            $('#mdlAccordionAnexos').removeAttr('style');
        }
        else {
            $('#mdlAccordionAnexos').attr('style', 'display:none');
        }
        $('.ui.confirmar.modal').modal('show');
        $('.ui.confirmar.modal div').popup();
    }

    function palavraChave(palavras) {
        palavras = palavras.toLowerCase();

        if (palavras.indexOf(";") > -1) {
            palavras = palavras.split(';');
        } else {
            palavras[0] = palavras;
        }
        $('.ui.pesquisa.modal .pesquisar').addClass('loading');
        $.ajax({
            type: 'POST',
            data: { "palavras": palavras },
            url: '/Dashboard/Questao/PalavrasChave',
            success: function (data) {
                $('.ui.pesquisa.modal .pesquisar').removeClass('loading');
                $('#divQuestoes').html('');
                if (data.length != 0) {
                    var $div = $('#divQuestoes');
                    $div.parent().find('.ui.resultado.label').remove();
                    $div.parent().append('<div class="ui resultado label"> Resultado(s)<div class="detail">' + data.length + '</div></div>');
                    for (var i = 0, length = data.length; i < length; i++) {
                        $div.append('\
                        <div class="item">\
                            <div class="content">\
                                <a data-questao="' + data[i].CodQuestao + '" class="header">' + data[i].Enunciado.encurtarTextoEm(140) + '</a>\
                                <div class="description ui labels">\
                                    <span class="ui label">' + data[i].Disciplina + '</span>\
                                    <span class="ui label">' + data[i].Dificuldade + '</span>\
                                    <span class="ui label">' + data[i].TipoQuestao + '</span>\
                                    <span class="ui label">' + data[i].Professor + '</span>\
                                </div>\
                            </div>\
                        </div>\
                        ');
                        if (i < (length - 1)) {
                            $div.append('\
                            <div class="ui horizontal divider">\
                                <i class="icon folder open"></i>\
                              </div>');
                        }
                    }
                } else {
                    $('#divQuestoes').append('\
                        <div class="item">\
                            <div class="content">\
                            <div class="header">Nenhuma questão foi encontrada</div>\
                            </div>\
                        </div>\
                        ');
                }
                $('.ui.pesquisa.modal').modal('refresh');
                $('a[data-questao]').click(function () {
                    var cod = $(this).attr('data-questao');
                    apresentarQuestao(cod);
                    $('.shape').shape('flip right');
                });
            },
            error: function () {
                siac.mensagem('Erro na pesquisa. Por favor, tente novamente.');
            }
        });
    }

    function apresentarQuestao(codQuestao) {
        if (codQuestao) {
            $.ajax({
                url: '/Dashboard/Questao/Apresentar',
                type: 'POST',
                data: { codigo: codQuestao },
                success: function (partial) {
                    $segment = $('.questao.segment');
                    $segment.html(partial);
                    $segment.removeAttr('style');
                    $segment.find('.ui.accordion').accordion({
                        onChange: function () {
                            $segment.parents('.modal').modal('refresh');
                        }
                    });
                    $segment.parents('.modal').modal('refresh');
                    $segment.parent().find('.voltar.button').off().click(function () {
                        $('.shape').shape('flip left');
                    });
                },
                complete: function () {
                    $('.questao.segment').removeClass('loading');
                }
            });
        }
    }

    function checarCaptcha() {
        $.ajax({
            type: 'GET',
            data: { captcha: $('#txtCaptcha').val() },
            url: "/Dashboard/Questao/ChequeCaptcha",
            success: function (resp) {
                if (resp == "true") {
                    $('.ui.pesquisa.modal .pesquisar').removeClass('loading');
                    $('.ui.confirmar.modal').modal('hide');
                    $('#txtCaptcha').next().prop('class', 'checkmark green icon');
                    $('#frmQuestao').addClass('loading').submit();
                }
                else {
                    $('.ui.pesquisa.modal .pesquisar').removeClass('loading');
                    $('.ui.confirmar.modal').modal('show');
                    $('#txtCaptcha').next().prop('class', 'remove red icon');
                }
            },
            error: function () {
                $('.ui.pesquisa.modal .pesquisar').removeClass('loading');
                $('#txtCaptcha').next().prop('class', 'remove icon');
                siac.mensagem("Ocorreu um erro ao verificar o captcha.", "Resultado do Captcha");
            }
        });
    }

    function novoCaptcha() {
        $.ajax({
            type: 'GET',
            url: "/Dashboard/Questao/NovoCaptcha",
            success: function (strBase64) {
                if (strBase64) {
                    $('#imgCaptcha').attr('src', 'data:image/png;base64,' + strBase64);
                }
            },
            error: function () {
                siac.mensagem("Ocorreu um erro ao atualizar o captcha, tente novamente.", "Erro");
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Questao.Detalhe = (function () {
    function iniciar() {
        $('.ui.accordion')
            .accordion({ animateChildren: false })
        ;

        $('div,p')
            .popup()
        ;

        $('.button')
        .popup({
            on: 'click'
        })
        ;
        $('.card.anexo.imagem').off().click(function () {
            var $this = $(this);
            var source = $this.find('img').attr('src');
            var legenda = $this.find('.header').text();
            var fonte = $this.find('.description').text();

            siac.Anexo.expandirImagem(source, legenda, fonte);
        });
        $('.arquivar.button').click(function () {
            var $_this = $(this);
            $_this.addClass('loading');
            $.ajax({
                url: '/Historico/Questao/Arquivar/' + $_this.attr('data-questao'),
                type: 'POST',
                success: function (flag) {
                    if (flag) {
                        $_this.addClass('active').text('Arquivada');
                    }
                    else {
                        $_this.removeClass('active').text('Arquivar');
                    }
                },
                error: function () {
                    siac.mensagem("Ocorreu um erro na tentativa de arquivar.", "Erro inesperado");
                },
                complete: function () {
                    $_this.removeClass('loading');
                }
            });
        });
    }

    return {
        iniciar: iniciar
    }
})();

siac.Questao.Editar = (function () {
    var _tipo, _qteAnexo;

    function iniciar() {
        $elemento = $('#frmQuestao');
        _tipo = $elemento.attr('data-questao-tipo');
        _qteAnexo = $elemento.attr('data-questao-anexo');
        $elemento.removeAttr('data-questao-tipo data-questao-anexo');

        $('.disabled').parent().popup({
            html: '<i class="icon red warning"></i>Este campo não pode ser modificado',
            variation: 'wide'
        });
        $('.ui.checkbox').checkbox();
        $('.ui.accordion').accordion({ animateChildren: false });
        $('.ui.modal .ui.accordion').accordion({
            animateChildren: false,
            onChange: function () { $('.ui.confirmar.modal').modal('refresh'); }
        });
        $('.ui.dropdown').dropdown();
        $('.tabular.menu .item').tab({
            history: true,
            historyType: 'state',
            path: window.location.pathname
        });
        $('.cancelar.button').popup({ on: 'click' });
        $('.ui.confirmar.modal')
          .modal({
              onApprove: function () {
                  $('#frmQuestao').addClass('loading').submit();
              }
          });

        $('.prosseguir.button').click(function () {
            verificar();
        });
    }

    function confirmar() {
        $('#mdlEnunciado').text($('#txtEnunciado').val()).attr('data-html', '<b>Objetivo</b>: ' + $('#txtObjetivo').val());
        if (_tipo == 1) {
            var qteAlternativas = $('.alternativas .title').length;
            $('#mdlListAlternativas').html('');
            for (var i = 0; i < qteAlternativas; i++) {
                if ($('#chkAlternativaCorreta' + (i + 1)).is(':checked')) {
                    $('#mdlListAlternativas')
                    .append('<div class="item">\
                                    <div class="middle aligned content" data-html="<b>Comentário</b>: ' + $('#txtAlternativaComentario' + (i + 1)).val() + '">\
                                        <b>' + (i + 1) + '</b>) ' + $('#txtAlternativaEnunciado' + (i + 1)).val() + '<a class="ui green label">Correta</a>\
                                    </div>\
                                </div>'
                    );
                    continue;
                }
                $('#mdlListAlternativas')
                    .append('<div class="item">\
                                    <div class="middle aligned content" data-html="<b>Comentário</b>: ' + $('#txtAlternativaComentario' + (i + 1)).val() + '">\
                                        <b>' + (i + 1) + '</b>) ' + $('#txtAlternativaEnunciado' + (i + 1)).val() + '\
                                    </div>\
                                </div>'
                    );
            }
            $('#mdlChaveDeResposta').attr('style', 'display:none');
            $('#mdlListAlternativas').removeAttr('style');
        }
        else if (_tipo == 2) {
            $('#mdlChaveDeResposta').html('<i>' + $('#txtChaveDeResposta').val() + '</i><a class="ui green label">Chave de Resposta</a>').attr('data-html', '<b>Comentário</b>: ' + $('#txtComentario').val());
            $('#mdlListAlternativas').attr('style', 'display:none');
            $('#mdlChaveDeResposta').removeAttr('style');
        }
        if (_qteAnexo > 0) {
            $('#mdlCardAnexos').html('');
            for (var i = 0; i < _qteAnexo; i++) {
                var tipoAnexo = $('#txtAnexoTipo' + (i + 1)).val();
                if (tipoAnexo == 1) {
                    $('#mdlCardAnexos')
                        .append('<div class="ui card">\
                                        <div class="image">\
                                            <img src="' + $('#preImagem' + (i + 1)).attr('src') + '" />\
                                        </div>\
                                        <div class="content">\
                                            <div class="header">\
                                                ' + $('#txtAnexoLegenda' + (i + 1)).val() + '\
                                            </div>\
                                            <div class="description">\
                                            ' + $('#txtAnexoFonte' + (i + 1)).val() + '\
                                            </div>\
                                        </div>\
                                    </div>'
                    );
                }
            }
            $('#mdlAccordionAnexos').removeAttr('style');
        }
        else {
            $('#mdlAccordionAnexos').attr('style', 'display:none');
        }
        $('.ui.confirmar.modal').modal('show');
        $('.ui.confirmar.modal div').popup();
    }

    function verificar() {
        $('.ui.error.message .list').html('');
        var validado = false;
        if ($('#txtEnunciado').val() != '') {
            if (_tipo == 1) {
                var qteAlternativas = $('.alternativas .title').length;
                var ok = true;
                for (var i = 0; i < qteAlternativas; i++) {
                    if ($('#txtAlternativaEnunciado' + (i + 1)).val() == '') {
                        ok = false;
                    }
                }
                if (ok) {
                    ok = false;
                    for (var i = 0; i < qteAlternativas; i++) {
                        if ($('#chkAlternativaCorreta' + (i + 1)).is(':checked')) {
                            ok = true;
                        }
                    }
                    if (ok) {
                        validado = true;
                    }
                    else {
                        $('.ui.error.message .list').append('<li>É necessário selecionar pelo menos umas das alternativas como correta</li>')
                    }
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher os enunciados de todas as alternativas</li>')
                }
            }
            else if (_tipo == 2) {
                if ($('#txtChaveDeResposta').val() != '') {
                    validado = true;
                }
                else {
                    $('.ui.error.message .list').append('<li>É necessário preencher a chave de resposta</li>')
                }
            }
        }
        else {
            $('.ui.error.message .list').append('<li>É necessário preencher o enunciado</li>')
        }

        if (validado) confirmar();
        else $('#frmQuestao').addClass('error');
    }

    return {
        iniciar: iniciar
    }
})();

siac.Questao.Gerar = (function () {
    function iniciar() {
        $('.gerar.button').click(function () {
            gerar();
        });
    }

    function gerar() {
        $('div.ui.form').parent().addClass('loading');
        strQte = $('#txtQuantidade').val();
        $.ajax({
            type: 'GET',
            url: '/Dashboard/Questao/Gerar',
            data: { "strQte": strQte },
            success: function (data) {
                $questoes = $('div.questoes');
                $questoes.html('');
                for (var i = 0; i < data.length; i++) {
                    $questao = $('<div class="ui segment"></div>');

                    $labels = $('<div class="ui labels"></div>');
                    $labelProfessor = $('<div class="ui label">\
                                                Professor\
                                                <div class="detail">\
                                                    '+ data[i].Professor + '\
                                                </div>\
                                            </div>');
                    $labelDisciplina = $('<div class="ui label">\
                                                Disciplina\
                                                <div class="detail">\
                                                    '+ data[i].Disciplina + '\
                                                </div>\
                                            </div>');
                    $labelDificuldade = $('<div class="ui label" data-html="<b>Comentário</b>: ' + data[i].Dificuldade.Comentario + '">\
                                                Dificuldade\
                                                <div class="detail">\
                                                    '+ data[i].Dificuldade.Descricao + '\
                                                </div>\
                                            </div>');
                    $labels.append($labelProfessor).append($labelDisciplina).append($labelDificuldade);

                    for (var j = 0; j < data[i].Tema.length; j++) {
                        $labels.append($('<div class="ui tag label" data-html="<b>Comentário</b>: ' + data[i].Tema[j].Comentario + '"></div>').text(data[i].Tema[j].Descricao));
                    }

                    $enunciado = $('<div class="ui dividing header" data-html="<b>Objetivo</b>: ' + data[i].Objetivo + '"></div>');
                    $enunciado.text(data[i].Enunciado);

                    $questao.append($labels);
                    $questao.append($enunciado);

                    if (data[i].TipoQuestao == 1) {
                        $lstAlternativa = $('<div class="ui very relaxed list"></div>');
                        for (var j = 0; j < data[i].Alternativa.length; j++) {
                            $alternativa = $('<div class="item"></div>');
                            $content = $('<div class="middle aligned content" data-html="<b>Comentário</b>: ' + data[i].Alternativa[j].Comentario + '">');
                            $content.append('<b>' + (j + 1) + '</b>) ');
                            $content.append(data[i].Alternativa[j].Enunciado);
                            if (data[i].Alternativa[j].FlagGabarito) {
                                if (data[i].Alternativa[j].FlagGabarito == true) {
                                    $content.append('<a class="ui green label">Correta</a>');
                                }
                            }
                            $alternativa.append($content);
                            $lstAlternativa.append($alternativa);
                        }
                        $questao.append($lstAlternativa);
                    }
                    else {
                        $chaveDeResposta = $('<div data-html="<b>Comentário</b>: ' + data[i].Comentario + '"></div>');
                        $chaveDeResposta.html('<a class="ui green ribbon label">Chave de Resposta</a>' + data[i].ChaveDeResposta);
                        $questao.append($chaveDeResposta);
                    }

                    $questoes.append($questao);
                }
                $('div,p')
                    .popup()
                ;
                $('div.ui.form').parent().removeClass('loading');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('div.ui.form').parent().removeClass('loading');
                siac.mensagem('Erro gerando as questões.');
            }
        });
    }

    return {
        iniciar: iniciar
    }
})();