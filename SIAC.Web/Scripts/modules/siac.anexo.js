siac.Anexo = siac.Anexo || (function () {
    function iniciar() {
        $('.card.anexo.imagem .image').dimmer({
            on: 'hover'
        });
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