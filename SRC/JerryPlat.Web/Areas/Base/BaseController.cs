using JerryPlat.DAL;
using JerryPlat.DAL.Context;
using JerryPlat.Models.Dto;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.App_Start;
using log4net;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Base
{
    public class BaseController : Controller
    {
        #region Response Result

        protected ActionResult Result(bool bIsSucess)
        {
            if (bIsSucess)
            {
                return Success(string.Empty);
            }

            return Faild(string.Empty);
        }

        protected ActionResult Result<T>(bool bIsSucess, T data)
        {
            if (bIsSucess)
            {
                return Success<T>(data);
            }

            return Faild(string.Empty);
        }

        protected ActionResult Success()
        {
            return Return(ResponseModel<string>.Ok(""));
        }

        protected ActionResult Success<T>(T data, string strMessage = "")
        {
            return Return(ResponseModel<T>.Ok(data, strMessage));
        }

        protected ActionResult Faild(string strMsg)
        {
            return Return(ResponseModel<string>.Error(strMsg));
        }

        protected ActionResult Invalid(string strMsg)
        {
            return Return(ResponseModel<string>.Invalid(strMsg));
        }

        protected ActionResult NotFound()
        {
            return Return(ResponseModel<string>.NotFound());
        }

        protected ActionResult Existed()
        {
            return Return(ResponseModel<string>.Existed());
        }

        private ActionResult Return<T>(ResponseModel<T> responseModel)
        {
            return new JsonNetResult { Data = responseModel, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return Json(responseModel);
        }

        #endregion Response Result
    }

    public class BaseController<THelper> : BaseController where THelper : DbContextBaseHelper<JerryPlatDbContext>, new()
    {
        #region Helper

        private THelper helper;
        protected THelper _helper => helper ?? (helper = new THelper());

        #endregion Helper

        public virtual ActionResult Index()
        {
            return View();
        }

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing && helper != null)
            {
                helper.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion Dispose
    }

    public class BaseController<THelper, TEntity> : BaseController<THelper>
        where THelper : BaseHelper<TEntity>, new()
        where TEntity : class, new()
    {
        [HttpPost]
        public virtual ActionResult GetList(SearchModel search, PageParam pageParam)
        {
            PageData<TEntity> pageData = _helper.GetPageList(search, PageHelper.GetKeyExpression<TEntity, int>(new string[] { "OrderIndex", "Id" }), pageParam, true);
            return Success(pageData);
        }

        [HttpPost]
        public virtual async Task<ActionResult> GetDetail(int id)
        {
            TEntity entity = await _helper.GetById(id);
            return Success(entity);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Save(TEntity entity)
        {
            bool result = await _helper.SaveAsync(entity);
            return Success(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Delete(int id)
        {
            bool result = await _helper.DeleteAsync(id);
            return Success(result);
        }

        [HttpPost]
        public virtual async Task<ActionResult> DeleteList(List<int> idList)
        {
            bool result = await _helper.DeleteListAsync(idList);
            return Success(result);
        }
    }

    public class BaseHelperController<TEntity> : BaseController<BaseHelper<TEntity>, TEntity>
        where TEntity : class, new()
    {
    }
}