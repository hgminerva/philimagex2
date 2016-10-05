﻿using System.Web;
using System.Web.Optimization;

namespace philimagex
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/wijmo/controls/wijmo.min.js", 
                      "~/wijmo/controls/wijmo.input.min.js", 
                      "~/wijmo/controls/wijmo.grid.min.js", 
                      "~/wijmo/controls/wijmo.chart.min.js",
                      "~/Scripts/nprogress.js",
                      "~/Scripts/toastr.min.js",
                      "~/Scripts/custom-scripts.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/styles.css",
                      "~/Content/nprogress.css",
                      "~/Content/toastr.min.css",
                      "~/wijmo/styles/wijmo.min.css"));
        }
    }
}
