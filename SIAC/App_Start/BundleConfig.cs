using System.Web.Optimization;

namespace SIAC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Scripts

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/bower_components/modernizr/modernizr.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/essentials").Include(
                "~/bower_components/jquery/dist/jquery.min.js",
                "~/bower_components/semantic-ui/dist/semantic.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/bower_components/jquery/dist/jquery.min.js",
                "~/bower_components/semantic-ui/dist/semantic.min.js",
                "~/bower_components/html2canvas/dist/html2canvas.min.js",
                "~/bower_components/alertify-js/build/alertify.min.js",
                "~/scripts/plugins/jquery.mask-1.13.4.min.js",
                "~/scripts/plugins/jquery.address-1.6.min.js",
                "~/scripts/siac.js",
                "~/scripts/modules/siac.lembrete.js",
                "~/scripts/modules/siac.anexo.js",
                "~/scripts/modules/siac.utilitario.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/acesso").Include(
                "~/scripts/modules/siac.acesso.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/questao").Include(
                "~/scripts/modules/siac.questao.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/agenda").Include(
                "~/scripts/modules/siac.agenda.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/perfil").Include(
                "~/scripts/modules/siac.perfil.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/configuracoes").Include(
                "~/scripts/modules/siac.configuracoes.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/usuario").Include(
                "~/scripts/modules/siac.usuario.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/autoavaliacao").Include(
                "~/scripts/modules/siac.autoavaliacao.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/academica").Include(
                "~/scripts/modules/siac.academica.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/certificacao").Include(
                "~/scripts/modules/siac.certificacao.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/institucional").Include(
                "~/scripts/modules/siac.institucional.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/reposicao").Include(
                "~/scripts/modules/siac.reposicao.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/visitante").Include(
                "~/scripts/modules/siac.visitante.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/simulado").Include(
                "~/scripts/modules/siac.simulado.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/gerencia").Include(
                "~/scripts/modules/siac.gerencia.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/arquivo").Include(
                "~/scripts/modules/siac.arquivo.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                "~/scripts/plugins/jquery.signalR-2.2.0.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar/js").Include(
                "~/bower_components/moment/min/moment.min.js",
                "~/bower_components/fullcalendar/dist/fullcalendar.min.js",
                "~/bower_components/fullcalendar/dist/lang/pt-br.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                "~/bower_components/chart.js/chart.min.js"
            ));

            #endregion Scripts

            #region Styles

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/bower_components/semantic-ui/dist/semantic.min.css",
                "~/bower_components/alertify-js/build/css/themes/semantic.min.css",
                "~/bower_components/alertify-js/build/css/alertify.min.css",
                "~/styles/siac.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/css/simulado").Include(
                "~/styles/siac.simulado.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/fullcalendar/css").Include(
                "~/bower_components/fullcalendar/dist/fullcalendar.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/fullcalendar/css/print").Include(
                "~/bower_components/fullcalendar/dist/fullcalendar.print.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/css/acesso").Include(
                "~/bower_components/semantic-ui/dist/semantic.min.css",
                "~/styles/siac.acesso.css"
            ));

            #endregion Styles
        }
    }
}