siac.Configuracoes = siac.Configuracoes || {};

siac.Configuracoes = (function () {
    var MatrizCount = 2;
    var HorarioCount = 2;

    function iniciar() {
        $('.menu .item').tab();
        $('.ui.dropdown').dropdown();
        $('.ui.accordion').accordion({ animateChildren: false });

        $('.tabular.menu .item').tab({
            history: true,
            historyType: 'state',
            path: window.location.pathname
        });

        $('.tab.geral :input, .tab.termo :input').change(function () {
            parametroModificado();
        });

        $('form input[type="submit"]').click(function () {
            cadastrar('#'+$(this).parents('form').attr('id'));
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
            url: "/Configuracoes",
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
        $tr = $('<tr></tr>');
        $td1 = $('<td></td>').append('\
                        <div class="field required">\
                            <input type="number" name="txtPeriodo' + MatrizCount + '" placeholder="Periodo..." required/>\
                        </div>');
        $td2 = $('<td></td>').append(copia.attr('name', 'ddlDisciplina' + MatrizCount).addClass('ui dropdown search fluid'));
        $td3 = $('<td></td>').append('\
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

        $tr = $('<tr></tr>');
        $td1 = $('<td></td>').append(horario.attr('name', 'txtHorario' + HorarioCount).val(HorarioCount));
        $td2 = $('<td></td>').append(inicio.attr('name', 'txtInicio' + HorarioCount));
        $td3 = $('<td></td>').append(termino.attr('name', 'txtTermino' + HorarioCount));

        $tr.append($td1).append($td2).append($td3);
        $('.table.body.horario').append($tr);
        $('#horarioQte').val(HorarioCount);
        HorarioCount++;
    }

    return {
        iniciar: iniciar
    }
})();