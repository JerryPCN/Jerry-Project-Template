namespace JerryPlat.Utils.Helpers
{
    public class SMSSessionHelper : SessionHelper
    {
        public new static SMSSessionHelper Instance = SessionHelper.GetInstance<SMSSessionHelper>("JerryPlat_SMSSessionName");
    }
}