namespace JerryPlat.Utils.Helpers
{
    public class UserSessionHelper : SessionHelper
    {
        public new static UserSessionHelper Instance = SessionHelper.GetInstance<UserSessionHelper>("JerryPlat_UserLoginSessionName");
    }
}