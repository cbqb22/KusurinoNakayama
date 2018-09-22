using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel;
using PharmacyTool.Web.Properties;

namespace PharmacyTool.Web.Service.DAO.PharmacyTool.アクセス数管理
{
    public class AccessManagementServiceHostFactory : ServiceHostFactory
    {
#if DEBUG

#elif NAKAYAMA

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost result = base.CreateServiceHost(serviceType,
                new Uri[] { new Uri(Settings.Default.ServerHttpRootNAKAYAMA + "PharmacyTool/Service/DAO/PharmacyTool/アクセス数管理/AccessManagement.svc") });
            return result;
        }

#else
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost result = base.CreateServiceHost(serviceType,
                new Uri[] { new Uri(Settings.Default.ServerHttpRoot + "PharmacyTool/Service/DAO/PharmacyTool/アクセス数管理/AccessManagement.svc") });
            return result;
        }
#endif

    }
}