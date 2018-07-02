using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace DeltaX.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            //bundles.UseCdn = true;   //enable CDN support

            ////add link to jquery on the CDN
            //var jqueryCdnPath = "https://ajax.googleapis.com/ajax/libs/angularjs/1.6.9/angular.min.js";

            //bundles.Add(new ScriptBundle("~/bundles/jquery",
            //            jqueryCdnPath).Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/Areas/bundles/jquery").Include(
            //             "~/scripts/custom-file-input.js",
            //               "~/scripts/jquery.dataTables.min.js",
            //                 "~/scripts/jquery.min.js",
            //                  "~/scripts/responsive-tables.js"

            //            ));

            //bundles.Add(new StyleBundle("~/Areas/Content/css").Include(
            //          "~/content/responsive-tables.css",
            //            "~/content/style.css"

            //          ));

        }
    }
}