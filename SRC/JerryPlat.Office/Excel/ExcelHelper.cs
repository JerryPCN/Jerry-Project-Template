using JerryPlat.Utils.Helpers;
using NPOI.Extension;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JerryPlat.Office
{
    public static class ExcelHelper
    {
        public const string Excel_ContentType = "application/octet-stream";

        private static void SetFluentConfiguration<TEntity>(Action<FluentConfiguration<TEntity>> action = null)
            where TEntity : class, new()
        {
            Excel.Setting.Company = "Jerry 15802775429";
            Excel.Setting.Author = "Jerry 15802775429";
            Excel.Setting.Subject = "数据导出";
            FluentConfiguration<TEntity> fc = Excel.Setting.For<TEntity>();
            if (action != null)
            {
                action(fc);
                return;
            }
        }

        public static void SetFluentConfigurationProperty<TEntity>(FluentConfiguration<TEntity> fc, Func<PropertyInfo, PropertyConfiguration> action = null)
            where TEntity : class, new()
        {
            int intIndex = 0;
            TypeHelper.DoModel<TEntity>(prop =>
            {
                PropertyConfiguration pc = null;
                if (TypeHelper.IsStringType(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, string>(prop));
                }
                else if (TypeHelper.IsInt32Type(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, int>(prop));
                }
                else if (TypeHelper.IsSingleType(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, Single>(prop));
                }
                else if (TypeHelper.IsDecimalType(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, decimal>(prop));
                }
                else if (TypeHelper.IsDateTimeType(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, DateTime>(prop))
                        .HasDataFormatter("yyyy-MM-dd HH:mm:ss");
                }
                else if (TypeHelper.IsBooleanType(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, bool>(prop));
                }
                else if (TypeHelper.IsNullableInt32Type(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, int?>(prop));
                }
                else if (TypeHelper.IsNullableDecimalType(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, decimal?>(prop));
                }
                else if (TypeHelper.IsNullableSingleType(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, Single?>(prop));
                }
                else if (TypeHelper.IsNullableDateTimeType(prop.PropertyType))
                {
                    fc.Property(PageHelper.GetKeyExpression<TEntity, DateTime?>(prop))
                        .HasDataFormatter("yyyy-MM-dd HH:mm:ss");
                }
                else if (TypeHelper.IsNullableBooleanType(prop.PropertyType))
                {
                    pc = fc.Property(PageHelper.GetKeyExpression<TEntity, bool?>(prop));
                }
                else
                {
                    if (action != null)
                    {
                        pc = action(prop);
                    }
                }

                if (pc != null)
                {
                    pc.HasExcelIndex(intIndex);
                }

                intIndex++;
            });
        }

        public static void SaveExcel<TEntity>(List<TEntity> list, string strExcelFullName, Action<FluentConfiguration<TEntity>> action = null)
             where TEntity : class, new()
        {
            SetFluentConfiguration(action);
            list.ToExcel(strExcelFullName);
        }

        public static byte[] SaveExcelContent<TEntity>(List<TEntity> list, Action<FluentConfiguration<TEntity>> action = null)
             where TEntity : class, new()
        {
            SetFluentConfiguration(action);
            return list.ToExcelContent();
        }

        public static IEnumerable<TEntity> LoadExcel<TEntity>(string strExcelFullName, ValueConverter valueConverter = null, Action<FluentConfiguration<TEntity>> action = null)
           where TEntity : class, new()
        {
            SetFluentConfiguration(action);
            return Excel.Load<TEntity>(strExcelFullName, valueConverter: valueConverter);
        }

        public static string GetExcelName(string strName)
        {
            return $"{strName}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.xlsx";
        }
    }
}