using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.App_Start.Filter;

namespace JerryPlat.Web.Areas.Base
{
    [LoginAuthorize]
    public class AuthurizeBaseController : BaseController
    {
        protected Member _Member => SessionHelper.Mob.GetSession<Member>();
    }

    [LoginAuthorize]
    public class AuthurizeBaseController<THelper> : BaseController<THelper> where THelper : DbContextBaseHelper<JerryPlatDbContext>, new()
    {
        protected Member _Member => SessionHelper.Mob.GetSession<Member>();
    }

    [LoginAuthorize]
    public class AuthurizeBaseController<THelper, TEntity> : BaseController<THelper, TEntity>
    where THelper : BaseHelper<TEntity>, new()
    where TEntity : class, new()
    {
        protected Member _Member => SessionHelper.Mob.GetSession<Member>();
    }

    [LoginAuthorize]
    public class AuthurizeBaseHelperController<TEntity> : BaseHelperController<TEntity>
        where TEntity : class, new()
    {
        protected Member _Member => SessionHelper.Mob.GetSession<Member>();
    }
}