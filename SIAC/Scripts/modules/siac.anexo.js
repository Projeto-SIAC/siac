/*
This file is part of SIAC.

Copyright (C) 2016 Felipe Mateus Freire Pontes <felipemfpontes@gmail.com>
Copyright (C) 2016 Francisco Bento da Silva Júnior <francisco.bento.jr@hotmail.com>

SIAC is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details. 
*/
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
        $('.card.anexo.texto').off().click(function () {
            var $this = $(this);
            var texto = $this.find('blockquote.texto').html();

            $header = $this.find(':not(.extra).content .header');
            var legenda = $header.data('legenda') ? $header.data('legenda') : $header.text();

            $description = $this.find('.extra.content .description');
            var fonte = $description.data('fonte') ? $description.data('fonte') : $description.text();

            expandirTexto(texto, legenda, fonte);
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

    function expandirTexto(texto, legenda, fonte) {
        $('.ui.anexo.modal').remove();

        $modal = $('<div></div>').addClass('ui large anexo basic modal');

        $modal.append($('<div class="ui centered header"></div>').text(legenda));
        $modal.append($('<div class="content" style="font-family: serif, sans-serif; font-size: 1.5em; text-align: justify"></div>').html(texto));
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