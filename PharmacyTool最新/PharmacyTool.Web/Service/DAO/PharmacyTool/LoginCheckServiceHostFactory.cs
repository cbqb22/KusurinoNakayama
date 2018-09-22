using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel;
using PharmacyTool.Web.Properties;


namespace PharmacyTool.Web.Service.DAO.PharmacyTool
{
    public class LoginCheckServiceHostFactory : ServiceHostFactory
    {
#if DEBUG

#elif NAKAYAMA
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost result = base.CreateServiceHost(serviceType,
                new Uri[] { new Uri(Settings.Default.ServerHttpRootNAKAYAMA + "PharmacyTool/Service/DAO/PharmacyTool/LoginCheck.svc") });
            return result;
        }

#else
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost result = base.CreateServiceHost(serviceType,
                new Uri[] { new Uri(Settings.Default.ServerHttpRoot + "PharmacyTool/Service/DAO/PharmacyTool/LoginCheck.svc") });
            return result;
        }
#endif


    }
}

