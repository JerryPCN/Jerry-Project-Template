using JerryPlat.DAL;
using JerryPlat.Models;
using JerryPlat.Utils.Helpers;
using JerryPlat.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JerryPlat.BLL.AdminManage
{
    public class SystemConfigHelper : BaseHelper
    {
        public SystemConfigModel Get()
        {
            if (SystemConfigModel.Instance == null)
            {
                SystemConfigModel.Reset(GetQueryableList<SystemConfig>().ToDictionary(o => o.Name, o => o.Config));
            }

            return SystemConfigModel.Instance;
        }

        public void Save(SystemConfigModel model)
        {
            List<SystemConfig> systemConfigList = GetDbSet<SystemConfig>().ToList();
            SystemConfig systemConfig = null;

            TypeHelper.DoModel<SystemConfigModel>(prop =>
            {
                systemConfig = systemConfigList.FirstOrDefault(o => o.Name == prop.Name);
                if (systemConfig == null)
                {
                    systemConfig = new SystemConfig
                    {
                        Name = prop.Name
                    };

                    _Db.SystemConfigs.Add(systemConfig);
                }

                systemConfig.Config = Convert.ToString(prop.GetValue(model));
            });

            _Db.SaveChanges();

            SystemConfigModel.Reset(model);
        }
    }
}