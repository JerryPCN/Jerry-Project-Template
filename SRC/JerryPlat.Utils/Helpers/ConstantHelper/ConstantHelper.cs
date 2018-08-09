namespace JerryPlat.Utils.Helpers
{
    public class ConstantHelper
    {
        #region Response Status

        public const string Ok = "Ok";
        public const string Error = "Error";
        public const string Invalid = "Invalid";
        public const string NotFound = "NotFound";
        public const string Existed = "Existed";
        public const string Logout = "Logout";

        #endregion Response Status

        #region Script

        private static string _constantScript;

        public static string GetScript()
        {
            if (string.IsNullOrEmpty(_constantScript))
            {
                _constantScript = TypeHelper.GetScript<ConstantHelper>("constantHelper");
            }

            return _constantScript;
        }

        #endregion Script
    }
}