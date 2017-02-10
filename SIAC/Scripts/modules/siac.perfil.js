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
siac.Perfil = siac.Perfil || (function () {
    function iniciar() {
        $('#estatisticas').load('/perfil/estatisticas');
        $('.ui.opiniao.modal').modal({
            onApprove: function () {
                if (verificar()) {
                    enviarOpiniao();
                    $(this).find('.approve.button').addClass('loading');
                    return false;
                }
                else {
                    $('.opiniao.modal .form').addClass('error');
                    return false;
                }
            }
        });
        $('.opiniao.button').click(function () {
            $('.opiniao.modal').modal('show');
        });
        $('.senha.button').click(function () {
            $('.senha.modal').modal('show');
        });
    }

    function verificar() {
        var $form = $('.opiniao.modal .form');
        $form.removeClass('error');
        $form.find('.message').remove();
        var $message = $('<div class="ui error message"></div>');
        $message.append($('<div class="header"></div>').text('Verifique os seguintes erros'));
        var $list = $('<ul class="list"></ul>');

        if (!$('#txtOpiniao').val().trim()) {
            $list.append($('<li></li>').html('Insira uma <b>opinião</b>'));
        }

        $message.append($list);

        $form.prepend($message);

        return $list.children().length == 0;
    }

    function enviarOpiniao() {
        var txtOpiniao = $('#txtOpiniao').val().trim();
        if (txtOpiniao) {
            $.ajax({
                url: '/perfil/enviaropiniao',
                type: 'POST',
                data: { opiniao: txtOpiniao },
                success: function () {
                    siac.aviso('Opinião registrada com sucesso!', 'green');
                },
                error: function () {
                    siac.aviso('Ocorreu um erro na tentativa de registrar opinião. Por favor, tente novamente mais tarde.', 'red');
                },
                complete: function () {
                    $('#txtOpiniao').val('');
                    $('.ui.opiniao.modal').modal('hide');
                    $('.ui.opiniao.modal .approve.button').removeClass('loading');
                }
            });
        }
    }

    return {
        iniciar: iniciar
    }
})();