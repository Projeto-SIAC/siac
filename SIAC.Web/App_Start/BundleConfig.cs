using System.Web;
using System.Web.Optimization;

namespace SIAC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/scripts/libs/modernizr.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/acesso").Include(
                "~/scripts/libs/jquery.min.js",
                "~/scripts/libs/semantic.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/scripts/libs/jquery.min.js",
                "~/scripts/libs/semantic.min.js",
                "~/scripts/libs/html2canvas.min.js",
                "~/scripts/libs/alertify.min.js",
                "~/scripts/plugins/jquery.mask-1.13.4.min.js",
                "~/scripts/plugins/jquery.address-1.6.min.js",
                "~/scripts/siac.js",
                "~/scripts/modules/siac.lembrete.js",
                "~/scripts/modules/siac.anexo.js",
                "~/scripts/modules/siac.utilitario.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/questao").Include(
                "~/scripts/modules/siac.questao.js"
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

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                "~/scripts/plugins/jquery.signalR-2.2.0.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar/js").Include(
                "~/scripts/libs/moment.min.js",
                "~/scripts/libs/fullcalendar.min.js",
                "~/scripts/libs/fullcalendar.pt-br.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                "~/scripts/libs/chart.min.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/styles/semantic.min.css",
                "~/styles/themes/semantic.min.css",
                "~/styles/alertify.min.css",
                "~/styles/siac.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/fullcalendar/css").Include(
                "~/styles/fullcalendar.css",
                "~/styles/fullcalendar.print.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/css/acesso").Include(
                "~/styles/semantic.min.css",
                "~/styles/siac.acesso.min.css"
            ));
        }
    }
}