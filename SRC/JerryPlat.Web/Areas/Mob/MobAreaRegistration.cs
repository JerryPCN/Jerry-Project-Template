using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob
{
    public class MobAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mob";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mob_default",
                "Mob/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "JerryPlat.Web.Areas.Mob.Controllers" }
            );
        }
    }
}