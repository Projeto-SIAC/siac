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
siac.Acesso = siac.Acesso || {};

siac.Acesso.Visitante = (function () {
    function iniciar() {
        $('.atualizar.button').click(function () {
            verificar();
        });
    }

    function verificar() {
        $('.ui.error.message .list').html('');
        var validado = false;
        if (!$('#txtNovaSenha').val()) {
            $('.ui.error.message .list').append('<li>É necessário inserir a nova senha</li>')
        }
        else if (!$('#txtConfirmaNovaSenha').val()) {
            $('.ui.error.message .list').append('<li>É necessário inserir a confirmação da nova senha</li>')
        }
        else if ($('#txtNovaSenha').val() != $('#txtConfirmaNovaSenha').val()) {
            $('.ui.error.message .list').append('<li>As senhas informadas não conferem</li>')
        }
        else if ($('#txtNovaSenha').val() == $('#txtConfirmaNovaSenha').val()) {
            validado = true;
        }

        if (validado) {
            atualizarSenha();
        }
        else {
            $('form').addClass('error');
        }
    }

    function atualizarSenha() {
        $('form').submit();
    }

    return {
        iniciar: iniciar
    }
})();