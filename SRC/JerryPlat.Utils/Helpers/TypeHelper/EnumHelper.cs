using System;
using System.ComponentModel;

namespace JerryPlat.Utils.Helpers
{
    /// <summary>
    /// Need to test
    /// </summary>
    public static class EnumHelper
    {
        public static T ToEnum<T>(int intValue)
        {
            //(T)intValue
            return (T)Enum.ToObject(typeof(T), intValue);
        }

        public static T ToEnum<T>(string strValue)
        {
            return (T)Enum.ToObject(typeof(T), strValue);
        }

        public static string GetName(Type enumType, int intValue)
        {
            return Enum.GetName(enumType, intValue);
        }

        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}