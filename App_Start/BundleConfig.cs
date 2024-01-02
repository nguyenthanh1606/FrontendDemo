using System.Web;
using System.Web.Optimization;

namespace Frontend
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                          "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/datepicker/moment.min.js",
                        "~/Scripts/datepicker/locale/vi.js",
                        "~/Scripts/datepicker/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                        "~/Areas/Admin/Scripts/jquery.mCustomScrollbar.concat.js",
                        "~/Areas/Admin/Scripts/select2/select2.js",
                        "~/Areas/Admin/Scripts/select2/vi.js",
                        "~/Areas/Admin/Scripts/vue.min.js",
                        "~/Areas/Admin/Scripts/admin.js",
                        "~/Areas/Admin/Scripts/admin_vue.js"));

            bundles.Add(new ScriptBundle("~/bundles/elFinder").Include(
                       "~/Scripts/jquery-ui.js",
                       "~/Scripts/elfinder.full.js",
                       "~/Scripts/elfinder.vi.js"));

            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                      "~/Scripts/ckeditor/ckeditor.js",
                      "~/Scripts/ckeditor/adapters/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                    "~/Scripts/vue.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/default").Include(
                  "~/Scripts/default.js"));

            bundles.Add(new StyleBundle("~/bundles/slider").Include(
                      "~/Scripts/owl.carousel.js",
                      "~/Scripts/swiper.min.js",
                      "~/Scripts/jquery.elevatezoom.js",
                      "~/Scripts/wow.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/jquery-{version}.js",
                       "~/Scripts/bootstrap.js",
                       "~/Scripts/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/datePicker").Include(
                    "~/Scripts/datepicker/moment.min.js",
                    "~/Scripts/datepicker/locale/vi.js",
                    "~/Scripts/datepicker/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/owlCarousel").Include(
                     "~/Scripts/owl.carousel.js"));

            bundles.Add(new StyleBundle("~/Content/css/site").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/font-awesome.css",
                        "~/Content/css/bootstrap-datetimepicker.min.css",
                      "~/Content/css/font.css",
                      "~/Content/css/site.css"));

            bundles.Add(new StyleBundle("~/Areas/Admin/Content/css/site").Include(
                      "~/Areas/Admin/Content/css/bootstrap.css",
                      "~/Areas/Admin/Content/css/font-awesome.css",
                       "~/Areas/Admin/Content/css/bootstrap-datetimepicker.min.css",
                      "~/Areas/Admin/Content/css/font.css",
                      "~/Areas/Admin/Content/css/site.css"));


            bundles.Add(new StyleBundle("~/Content/css/default").Include(
                      "~/Content/css/Screen.css",
                      "~/Content/css/Responsive.css",
                      "~/Content/css/Menu.css",
                      "~/Content/css/Product.css"));

            bundles.Add(new StyleBundle("~/Content/css/slider").Include(
                    "~/Content/css/owl.carousel.css",
                    "~/Content/css/owl.theme.css",
                    "~/Content/css/owl.transitions.css",
                    "~/Content/css/animate.css"));

            bundles.Add(new StyleBundle("~/Content/pagedList").Include(
                      "~/Content/css/PagedList.css"));

            bundles.Add(new StyleBundle("~/Content/animate").Include(
                      "~/Content/css/animate.css"));

            bundles.Add(new StyleBundle("~/Areas/Admin/Content/css/admin").Include(
                      "~/Areas/Admin/Content/css/admin.css",
                      "~/Areas/Admin/Content/css/jquery.mCustomScrollbar.css",
                      "~/Areas/Admin/Content/css/select2.css"));

            bundles.Add(new StyleBundle("~/Content/css/elFinder").Include(
                      "~/Content/css/jquery-ui.css",
                      "~/Content/css/elfinder.full.css",
                      "~/Content/css/elfinder-theme.css"));

            bundles.Add(new StyleBundle("~/Content/css/common").Include(
                      "~/Content/css/font-awesome.css",
                      "~/Content/css/bootstrap.css"));

            bundles.Add(new StyleBundle("~/bundles/css/datePicker").Include(
                  "~/Content/css/bootstrap-datepicker.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/owlCarousel").Include(
                    "~/Content/css/owl.carousel.css",
                    "~/Content/css/owl.theme.default.css"));
        }
    }
}
