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