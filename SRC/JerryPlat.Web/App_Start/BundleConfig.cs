using System.Web.Optimization;

namespace JerryPlat.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/base").Include(
                       "~/Content/vue.element.ui.min.css",
                       "~/Content/site.web.css"));

            bundles.Add(new ScriptBundle("~/script/base").Include(
                        "~/Scripts/jquery-3.3.1.min.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/vue.min.js",
                        "~/Scripts/vue.element.ui.min.js",
                        "~/Scripts/vue.jerry.helper.web.js"
                        ));

            bundles.Add(new StyleBundle("~/content/mobbase").Include(
                       "~/Content/vue.mint.ui.2.2.13.min.css",
                       "~/Content/site.mob.css"));

            bundles.Add(new ScriptBundle("~/script/mobbase").Include(
                        "~/Scripts/jquery-3.3.1.min.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/vue.min.js",
                        "~/Scripts/vue.mint.ui.2.2.13.min.js",
                        "~/Scripts/vue.jerry.helper.mob.js"
                        ));
        }
    }
}