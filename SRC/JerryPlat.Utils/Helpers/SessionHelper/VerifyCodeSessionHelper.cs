namespace JerryPlat.Utils.Helpers
{
    public class VerifyCodeSessionHelper : SessionHelper
    {
        public new static VerifyCodeSessionHelper Instance = SessionHelper.GetInstance<VerifyCodeSessionHelper>("JerryPlat_VerifyCodeSessionName");
    }
}