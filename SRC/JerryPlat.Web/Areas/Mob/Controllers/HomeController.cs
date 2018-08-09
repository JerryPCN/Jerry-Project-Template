using JerryPlat.BLL.CommonMagage;
using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Pay.WxPay;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class HomeController : AuthurizeBaseController
    {
        // GET: Mob/Home
        public async Task<ActionResult> Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                MemberHelper memberHelper = new MemberHelper();
                await memberHelper.SetMemberReferee(id);
            }

            //if (string.IsNullOrEmpty(_Member.Phone))
            //{
            //    return Redirect("/Mob/Home/Bind");
            //}

            WxPayData data = await WxPayApi.GetJSSDKConfig();
            ViewBag.WxConfig = data == null ? "" : SerializationHelper.ToJson(data.GetValues());

            return View();
        }

        public ActionResult Bind()
        {
            if (!string.IsNullOrEmpty(_Member.Phone))
            {
                return Redirect("/Mob");
            }

            return View(new BindDto
            {
                NickName = _Member.NickName,
                Avatar = _Member.Avatar
            });
        }

        [HttpPost]
        public async Task<ActionResult> SetLocation(LocationDto location)
        {
            MemberHelper memberHelper = new MemberHelper();
            await memberHelper.SetLocation(location);
            return Success();
        }

        public ActionResult Location(LocationDto model)
        {
            OwinConfigHelper helper = new OwinConfigHelper();
            return Success(helper.SetLocation(model));
        }

        [HttpPost]
        public ActionResult GetBannerList(SearchModel searchModel, PageParam pageParam)
        {
            BannerHelper helper = new BannerHelper();
            return Success(helper.GetPageList(searchModel, o => o.OrderIndex, pageParam, true));
        }

        [HttpPost]
        public ActionResult GetCourseList(PageParam pageParam)
        {
            BaseHelper<Course> helper = new BaseHelper<Course>();
            return Success(helper.GetPageList<int, SearchModel>(null, o => o.OrderIndex, pageParam, true));
        }

        [HttpPost]
        public ActionResult GetCityList(PageParam pageParam)
        {
            BaseHelper<City> helper = new BaseHelper<City>();
            return Success(helper.GetPageList<int, SearchModel>(null, o => o.OrderIndex, pageParam, true));
        }

        [HttpPost]
        public ActionResult GetSchoolList(PageParam pageParam)
        {
            BaseHelper<School> helper = new BaseHelper<School>();
            return Success(helper.GetPageList<int, SearchModel>(null, o => o.OrderIndex, pageParam, true));
        }

        [HttpPost]
        public ActionResult GetQuestionTypeList(PageParam pageParam)
        {
            BaseHelper<QuestionType> helper = new BaseHelper<QuestionType>();
            return Success(helper.GetPageList<int, SearchModel>(null, o => o.OrderIndex, pageParam, true));
        }

        [HttpPost]
        public ActionResult GetArticleList(SearchModel searchModel, PageParam pageParam)
        {
            ArticleHelper helper = new ArticleHelper();
            return Success(helper.GetPageList(searchModel, o => o.UpdateTime, pageParam, false));
        }

        [HttpPost]
        public ActionResult GetGroundList(PageParam pageParam)
        {
            BaseHelper<Ground> helper = new BaseHelper<Ground>();
            return Success(helper.GetPageList<int, SearchModel>(null, o => o.OrderIndex, pageParam, true));
        }

        [HttpPost]
        public ActionResult GetCoachList(PageParam pageParam)
        {
            BaseHelper<Coach> helper = new BaseHelper<Coach>();
            return Success(helper.GetPageList<int, SearchModel>(null, o => o.OrderIndex, pageParam, true));
        }

        [HttpPost]
        public ActionResult GetWithdrawTypeList(PageParam pageParam)
        {
            BaseHelper<WithdrawType> helper = new BaseHelper<WithdrawType>();
            return Success(helper.GetPageList<int, SearchModel>(null, o => o.OrderIndex, pageParam, true));
        }

        [HttpPost]
        public async Task<ActionResult> GetShareCode()
        {
            string strShareCode = string.Empty;
            if (!SessionHelper.ShareCode.IsNullSession())
            {
                strShareCode = SessionHelper.ShareCode.GetSession<string>();
            }
            else
            {
                if (_Member.RefereeId > 0)
                {
                    MemberHelper helper = new MemberHelper();
                    Member referee = await helper.GetByIdAsync(_Member.RefereeId);
                    strShareCode = referee.ShareCode;
                }
            }
            return Success(strShareCode);
        }
    }
}