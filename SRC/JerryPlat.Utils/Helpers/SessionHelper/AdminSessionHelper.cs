namespace JerryPlat.Utils.Helpers
{
    public class AdminSessionHelper : SessionHelper
    {
        public new static AdminSessionHelper Instance = SessionHelper.GetInstance<AdminSessionHelper>("JerryPlat_AdminLoginSessionName");
    }
}