using System.Web;
using System.Web.Optimization;

namespace SIAC.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Resources/modernizr.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Resources/jquery.min.js",
                "~/Resources/semantic.min.js",
                "~/Resources/jquery.mask.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                "~/Resources/jquery.signalR-2.2.0.min.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Resources/semantic.min.css",
                "~/Resources/siac.scrollbar.css"
            ));
        }
    }
}