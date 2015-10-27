﻿using System.Web;
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

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/scripts/libs/jquery.min.js",
                "~/scripts/libs/semantic.min.js",
                "~/scripts/libs/html2canvas.min.js",
                "~/scripts/plugins/jquery.mask-1.13.4.min.js",
                "~/scripts/plugins/jquery.address-1.6.min.js",
                "~/scripts/siac.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/questao").Include(
                "~/scripts/modules/siac.questao.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/configuracoes").Include(
                "~/scripts/modules/siac.configuracoes.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/autoavaliacao").Include(
                "~/scripts/modules/siac.autoavaliacao.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js/academica").Include(
                "~/scripts/modules/siac.academica.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                "~/scripts/plugins/jquery.signalR-2.2.0.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                "~/scripts/libs/chart.min.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/styles/semantic.min.css",
                "~/styles/siac.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/css/acesso").Include(
                "~/styles/siac.acesso.min.css"
            ));
        }
    }
}