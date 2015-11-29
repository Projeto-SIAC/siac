siac.Perfil = siac.Perfil || (function () {
    function iniciar() {
        $('#estatisticas').load('/perfil/estatisticas');
    }

    return {
        iniciar: iniciar
    }
})();