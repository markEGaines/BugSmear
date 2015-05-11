using System.Web;
using System.Web.Optimization;

namespace BugSmear
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                   "~/js/jquery.nanoscroller/jquery.nanoscroller.js",
                   "~/js/jquery.ui/jquery-ui.js",
	               "~/js/jquery.nestable/jquery.nestable.js",
                   "~/js/jquery.select2/select2.min.js",
                   "~/js/jquery.gritter/js/jquery.gritter.js",
         //          "~/js/jquery.nanoscroller/jquery.nanoscroller.min.js.map",
	               "~/js/jquery.sparkline/jquery.sparkline.min.js",
          //       "~/js/jquery.easypiechart/jquery.easy-pie-chart.js",
                   "~/js/jquery.multiselect/js/jquery.multi-select.js",
                   "~/js/jquery.quicksearch/jquery.quicksearch.js" ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
	               "~/js/bootstrap.switch/bootstrap-switch.min.js",
                   "~/js/bootstrap.datepicker/js/bootstrap-datepicker.min.js",
                   "~/js/bootstrap.slider/js/bootstrap-slider.js",
                   "~/js/bootstrap.multiselect/js/bootstrap-multiselect.js"));

            bundles.Add(new ScriptBundle("~/bundles/cleanzone").Include(
                   "~/js/skycons/skycons.js",
                   "~/js/intro.js/intro.js",
                   "~/js/behaviour/general.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                    .Include("~/Content/bootstrap.css")
                      .Include("~/js/jquery.gritter/css/jquery.gritter.css")
                    .Include( "~/fonts/font-awesome-4/css/font-awesome.min.css", new CssRewriteUrlTransform())
                      .Include("~/js/jquery.nanoscroller/nanoscroller.css")
                      //.Include("~/js/jquery.easypiechart/jquery.easy-pie-chart.css")
                      .Include("~/js/bootstrap.switch/bootstrap-switch.css")
                      .Include("~/js/bootstrap.datepicker/css/bootstrap-datepicker.min.css")
                      .Include("~/js/jquery.select2/select2.css")
                      //.Include("~/js/bootstrap.slider/css/slider.css")
                      .Include("~/js/jquery.multiselect/css/multi-select.css")
                      //.Include("~/js/bootstrap/dist/css/bootstrap.css")
                      .Include("~/js/bootstrap.multiselect/css/bootstrap-multiselect.css")
                      .Include("~/js/jquery.multiselect/css/multi-select.css")
                      .Include("~/Content/site.css")
                      .Include("~/Content/jquery-ui.min.css")
                      .Include("~/css/style.css"));

        }
    }
}
