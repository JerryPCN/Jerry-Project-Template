using JerryPlat.BLL.CommonMagage;
using JerryPlat.Models;
using JerryPlat.Models.Dto;
using JerryPlat.Pay.WxPay;
using JerryPlat.Utils.Helpers;
using JerryPlat.Web.Areas.Base;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JerryPlat.Web.Areas.Mob.Controllers
{
    public class EnrollController : AuthurizeBaseController<EnrollHelper>
    {
        // GET: Mob/Enroll
        public override ActionResult Index()
        {
            Enroll enroll = _helper.GetEnroll();

            if (enroll != null)
            {
                return Redirect("/Mob/Enroll/Process");
            }

            enroll = new Enroll
            {
                Phone = _Member.Phone
            };

            return View(enroll);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            Enroll enroll = null;
            if (id.HasValue)
            {
                enroll = await _helper.GetEnroll(id.Value, false);
            }
            else
            {
                enroll = _helper.GetEnroll();
            }

            if (enroll == null)
            {
                return Redirect("/Mob");
            }

            if (_helper.IsPaid(enroll))
            {
                return Redirect("/Mob/Enroll/Process");
            }

            return View("Index", enroll);
        }

        public async Task<ActionResult> Process()
        {
            EnrollDto enrollDto = _helper.GetEnrollDto();
            if (enrollDto == null)
            {
                return Redirect("/Mob/Enroll");
            }

            if (WxPayApi.IsPaid("", enrollDto.Enroll.OrderNo))
            {
                PayModelDto model = WxPayApi.GetPayModel(enrollDto.Enroll.OrderNo);
                await _helper.SetPayStatus(EnrollStatus.PaySuccess, model);
            }

            WxPayData data = null;
            if (enrollDto.Enroll.Status != EnrollStatus.PaySuccess)
            {
                data = await WxPayApi.GetJSSDKConfig();
            }

            ViewBag.WxConfig = data == null ? "" : SerializationHelper.ToJson(data.GetValues());

            enrollDto.Enroll.IdCard = enrollDto.Enroll.IdCard.ToSpecialString(6, 8);
            return View(enrollDto);
        }

        [HttpPost]
        public async Task<ActionResult> Save(string code, Enroll model)
        {
            if (!SessionHelper.SMS.IsValid(code))
            {
                return Faild("请输入正确的验证码。");
            }

            //if (!SessionHelper.VerifyCode.IsValid(code))
            //{
            //    return Faild("请输入正确的验证码。");
            //}

            MemberHelper memberHelper = new MemberHelper();
            Member referee = await memberHelper.GetByShareCodeAsync(model.ShareCode);
            //if (referee == null && _Member.RefereeId > 0)
            //{
            //    referee = await memberHelper.GetMemberByIdAsync(_Member.RefereeId);
            //}

            if (referee != null)
            {
                if (_Member.Id == referee.Id)
                {
                    return Faild("推荐人不可以是自己。");
                }

                if (referee.RefereeId == _Member.Id)
                {
                    return Faild("不可以相互推荐。");
                }

                model.RefereeId = referee.Id;
            }

            if (string.IsNullOrEmpty(model.OrderNo))
            {
                model.OrderNo = ToolHelper.GetOrderNo(_helper.IsExistOrderNo, "E");
            }

            return Success(await _helper.Save(model, _Member));
        }

        [HttpPost]
        public ActionResult IsPaid()
        {
            return Success(_helper.IsPaid());
        }
    }
}