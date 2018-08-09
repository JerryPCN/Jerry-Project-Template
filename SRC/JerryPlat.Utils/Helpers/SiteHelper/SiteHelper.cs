using System;
using System.IO;
using System.Security;
using System.Web;

namespace JerryPlat.Utils.Helpers
{
    public static class SiteHelper
    {
        #region .NET

        /// <summary>
        /// 获得当前应用程序的信任级别
        /// </summary>
        /// <returns></returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            AspNetHostingPermissionLevel trustLevel = AspNetHostingPermissionLevel.None;
            //权限列表
            AspNetHostingPermissionLevel[] levelList = new AspNetHostingPermissionLevel[] {
                                                                                            AspNetHostingPermissionLevel.Unrestricted,
                                                                                            AspNetHostingPermissionLevel.High,
                                                                                            AspNetHostingPermissionLevel.Medium,
                                                                                            AspNetHostingPermissionLevel.Low,
                                                                                            AspNetHostingPermissionLevel.Minimal
                                                                                            };

            foreach (AspNetHostingPermissionLevel level in levelList)
            {
                try
                {
                    //通过执行Demand方法检测是否抛出SecurityException异常来设置当前应用程序的信任级别
                    new AspNetHostingPermission(level).Demand();
                    trustLevel = level;
                    break;
                }
                catch (SecurityException ex)
                {
                    continue;
                }
            }
            return trustLevel;
        }

        /// <summary>
        /// 修改web.config文件
        /// </summary>
        /// <returns></returns>
        private static bool TryWriteWebConfig()
        {
            try
            {
                File.SetLastWriteTimeUtc(("~/web.config").ToMapPath(), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改global.asax文件
        /// </summary>
        /// <returns></returns>
        private static bool TryWriteGlobalAsax()
        {
            try
            {
                File.SetLastWriteTimeUtc(("~/global.asax").ToMapPath(), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 重启应用程序
        /// </summary>
        public static void RestartAppDomain()
        {
            if (GetTrustLevel() > AspNetHostingPermissionLevel.Medium)//如果当前信任级别大于Medium，则通过卸载应用程序域的方式重启
            {
                HttpRuntime.UnloadAppDomain();
                TryWriteGlobalAsax();
            }
            else//通过修改web.config方式重启应用程序
            {
                bool success = TryWriteWebConfig();
                if (!success)
                {
                    throw new Exception("修改web.config文件重启应用程序");
                }

                success = TryWriteGlobalAsax();
                if (!success)
                {
                    throw new Exception("修改global.asax文件重启应用程序");
                }
            }
        }

        #endregion .NET
    }
}