using System.Web.Mvc;

namespace JerryPlat.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string code)
        {
            return View((object)code);
        }
    }
}