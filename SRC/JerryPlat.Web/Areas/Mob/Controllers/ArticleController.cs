using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class ArticleController : AuthurizeBaseController<ArticleHelper>
    {
        // GET: Mob/Article
        public override ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> TopOne(ArticleType id)
        {
            return View("Detail", await _helper.GetTopOneByArticleTypeAsync(id));
        }

        public async Task<ActionResult> Detail(int id)
        {
            return View(await _helper.GetByIdAsync(id, false));
        }
    }
}