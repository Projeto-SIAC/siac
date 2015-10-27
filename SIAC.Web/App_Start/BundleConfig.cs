using System.Web;
using System.Web.Optimization;

namespace SIAC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/libs/modernizr.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/libs/jquery.min.js",
                "~/Scripts/libs/semantic.min.js",
                "~/Scripts/libs/html2canvas.min.js",
                "~/Scripts/plugins/jquery.mask-1.13.4.min.js",
                "~/Scripts/siac.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/questao").Include(
                "~/Scripts/modules/siac.questao.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/configuracoes").Include(
                "~/Scripts/modules/siac.configuracoes.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/autoavaliacao").Include(
                "~/Scripts/modules/siac.autoavaliacao.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/academica").Include(
                "~/Scripts/modules/siac.academica.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                "~/Scripts/plugins/jquery.signalR-2.2.0.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                "~/Scripts/libs/chart.min.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Styles/semantic.min.css",
                "~/Styles/siac.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/css/acesso").Include(
                "~/Styles/siac.acesso.min.css"
            ));
        }
    }
}