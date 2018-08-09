using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace JerryPlat.Utils.Helpers
{
    public class TypeHelper
    {
        public static string GetScript<T>(string strObjectName) where T : class, new()
        {
            return GetScript<T>(strObjectName, new T());
        }

        public static string GetScript<T>(string strObjectName, T instance) where T : class
        {
            string strScript = "var " + strObjectName + " = {";

            Type type = instance.GetType();
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                strScript += $"{field.Name}:\"{field.GetValue(instance).ToString()}\",";
            }
            strScript = strScript.TrimEnd(',');

            strScript += "};";

            return strScript;
        }

        public static PropertyInfo GetPropertyInfo<TEntity>(string strKey) where TEntity : class
        {
            return GetPropertyInfo<TEntity>(new string[] { strKey });
        }

        public static PropertyInfo GetPropertyInfo<TEntity>(string[] aryStrKey) where TEntity : class
        {
            Type type = typeof(TEntity);
            PropertyInfo[] props = type.GetProperties();
            PropertyInfo prop = null;
            foreach (string strKey in aryStrKey)
            {
                prop = props.FirstOrDefault(o => o.Name.ToUpper() == strKey.ToUpper());
                if (prop != null)
                {
                    return prop;
                }
            }

            throw new Exception("Can not find the " + string.Join(",", aryStrKey) + " property in [" + type.Name + "]");
        }

        public static PropertyInfo GetIdPropertyInfo<TEntity>() where TEntity : class
        {
            return GetPropertyInfo<TEntity>("Id");
        }

        public static TModel InitModel<TModel>(Func<PropertyInfo, object> funcValueProvider) where TModel : class, new()
        {
            TModel model = new TModel();

            DoModel<TModel>(prop =>
            {
                object objValue = funcValueProvider(prop);
                if (objValue != null)
                {
                    prop.SetValue(model, objValue);
                }
            });

            return model;
        }

        public static TModel InitModel<TModel>(Func<string, object> funcValueProvider) where TModel : class, new()
        {
            return InitModel<TModel>(prop => funcValueProvider(prop.Name));
        }

        public static TModel InitModel<TModel>(Dictionary<string, string> keyValueList) where TModel : class, new()
        {
            return InitModel<TModel>(prop =>
            {
                if (!keyValueList.Keys.Contains(prop.Name))
                {
                    return null;
                }
                return Convert.ChangeType(keyValueList[prop.Name], prop.PropertyType);
            });
        }

        public static void DoModel<TModel>(Action<PropertyInfo> funcPropProvider) where TModel : class, new()
        {
            Type type = typeof(TModel);
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                funcPropProvider(prop);
            }
        }

        public static string FillContent<TModel>(TModel model, string strTemplate)
        {
            return FillContent(model, strTemplate, "{{", @"\w+", "}}");
        }

        public static string FillContent<TModel>(TModel model, string strTemplate, string strStartWith, string strField, string strEndWith)
        {
            MatchCollection matches = Regex.Matches(strTemplate, $@"{strStartWith}{strField}{strEndWith}");
            if (matches.Count > 0)
            {
                Type type = typeof(TModel);
                PropertyInfo[] props = type.GetProperties();
                PropertyInfo prop = null;
                string strTemp = string.Empty;

                int intStartIndex = strStartWith.Length;
                int intLength = strStartWith.Length + strEndWith.Length;

                foreach (Match match in matches)
                {
                    strTemp = match.Value.Substring(intStartIndex, match.Value.Length - intLength);
                    prop = props.FirstOrDefault(o => o.Name.ToUpper() == strTemp.ToUpper());
                    if (prop != null)
                    {
                        strTemplate = strTemplate.Replace(match.Value, Convert.ToString(prop.GetValue(model)));
                    }
                }
            }
            return strTemplate;
        }

        public static string ToJson<TModel>() where TModel : class, new()
        {
            TModel model = new TModel();
            return SerializationHelper.ToJson(model);
        }

        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }

        #region Check Type

        public static bool IsStringType(Type type)
        {
            return type.Name == "String";
        }

        public static bool IsInt16Type(Type type)
        {
            return type.Name == "Int16";
        }

        public static bool IsNullableInt16Type(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("Int16");
        }

        public static bool IsInt32Type(Type type)
        {
            return type.Name == "Int32";
        }

        public static bool IsNullableInt32Type(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("Int32");
        }

        public static bool IsInt64Type(Type type)
        {
            return type.Name == "Int64";
        }

        public static bool IsNullableInt64Type(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("Int64");
        }

        public static bool IsDecimalType(Type type)
        {
            return type.Name == "Decimal";
        }

        public static bool IsNullableDecimalType(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("Decimal");
        }

        public static bool IsSingleType(Type type)
        {
            return type.Name == "Single";
        }

        public static bool IsNullableSingleType(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("Single");
        }

        public static bool IsDoubleType(Type type)
        {
            return type.Name == "Double";
        }

        public static bool IsNullableDoubleType(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("Double");
        }

        public static bool IsBooleanType(Type type)
        {
            return type.Name == "Boolean";
        }

        public static bool IsNullableBooleanType(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("Boolean");
        }

        public static bool IsDateTimeType(Type type)
        {
            return type.Name == "DateTime";
        }

        public static bool IsNullableDateTimeType(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("DateTime");
        }

        public static bool IsByteType(Type type)
        {
            return type.Name == "Byte";
        }

        public static bool IsNullableByteType(Type type)
        {
            return type.Name.Contains("Nullable")
                && type.FullName.Contains("Byte");
        }

        public static bool IsEnumType(Type type)
        {
            return type.BaseType.Name == "Enum";
        }

        #endregion Check Type
    }
}