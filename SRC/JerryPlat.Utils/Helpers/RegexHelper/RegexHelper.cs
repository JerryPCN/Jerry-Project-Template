using System.Text.RegularExpressions;

namespace JerryPlat.Utils.Helpers
{
    public static class RegexHelper
    {
        public static Regex RegexReplace = new Regex(@"{{\w+}}", RegexOptions.Compiled);
    }
}