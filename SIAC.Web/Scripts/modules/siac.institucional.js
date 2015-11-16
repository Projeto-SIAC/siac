siac.Institucional = siac.Institucional || {};

siac.Institucional.Configurar = (function () {
	function iniciar() {

		//$('.menu .item').tab();
		$('.ui.dropdown').dropdown();
		$('.ui.accordion').accordion({ animateChildren: false });

		$('.tabular.menu .item').tab({
			history: true,
			historyType: 'state',
			path: '/institucional/configurar'
		});
	}

	return {
		iniciar: iniciar
	}
})();

siac.Institucional.Gerar = (function () {
    function iniciar() {

        $('.ui.dropdown').dropdown();
        $('.tabular.menu .item').tab();
        $('.ui.accordion').accordion({ animateChildren: false });
        $('.cancelar.button').popup({
            on: 'click',
            inline: true
        });

        //Definindo o Tipo Default como 1 ( Objetiva )
        $('#ddlTipo').dropdown('set selected', 1);

        //Adicionando Alternativas por default
        for (var i = 0; i < 5; i++) {
            adicionarAlternativa();
        }

        //Funções
        $('#ddlTipo').change(function () {
            mostrarCamposPorTipo();
        });

        $('.questao.objetiva .adicionar.button').click(function () {
            adicionarAlternativa();
        });
    }

    function adicionarAlternativa() {
        var i = $('.ui.alternativas.accordion .title').length + 1;
        $('.ui.alternativas.accordion').append('\
                        <div class="title">\
                            <input id="txtAlternativaIndex" value="'+ i + '" hidden />\
                            <i class="dropdown icon"></i>Alternativa ' + i + '\
                        </div>\
                        <div class="content ui segment">\
                            <div class="field required">\
                                <label for="txtAlternativaEnunciado' + i + '">Enunciado</label>\
                                <textarea id="txtAlternativaEnunciado' + i + '" name="txtAlternativaEnunciado' + i + '" rows="2" placeholder="Enunciado..."></textarea>\
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
        $('#txtQtdAlternativas').val(i);
        $('.ui.alternativas.accordion .remover.button').off().click(function () {
            removerAlternativa(this);
        });
        $('.ui.alternativas.accordion .button').popup({ inline: true, on: 'click', position: 'right center' });
        $('.ui.alternativas.accordion').accordion({animateChildren:false});

    }

    function renomearAlternativas() {
        var list = $('.ui.alternativas.accordion .title');
        var listContent = $('.ui.alternativas.accordion .content.segment');
        for (var i = 0; i < list.length; i++) {
            var j = list.eq(i).find('#txtAlternativaIndex').val();
            list.eq(i).html('<input id="txtAlternativaIndex" value="' + (i + 1) + '" hidden /><i class="dropdown icon"></i>Alternativa ' + (i + 1));
            
            /* RENOMEAR LABELS, INPUTS e TEXTAREAS */
            listContent.eq(i).find('label[for="txtAlternativaEnunciado' + j + '"]').attr('for', 'txtAlternativaEnunciado' + (i + 1));
            listContent.eq(i).find('textarea[name="txtAlternativaEnunciado' + j + '"]').attr('name', 'txtAlternativaEnunciado' + (i + 1)).attr('id', 'txtAlternativaEnunciado' + (i + 1));
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
            i--;
        } else {
            siac.aviso('É preciso ter, no mínimo, duas alternativas por questão', 'red');
        }
        $('#txtQtdAlternativas').val(i);
    }

    function mostrarCamposPorTipo() {
        var tipo = $('#ddlTipo').val();
        $('.questao.objetiva').hide();
        if (tipo == 1) {
            $('.questao.objetiva').show();
            $('.segment.questao.objetiva').attr('style', '');
        }
        else if (tipo == 2) {
            $('.questao.objetiva').hide();
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

    return {
        iniciar: iniciar
    }
})();